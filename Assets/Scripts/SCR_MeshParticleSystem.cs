using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_MeshParticleSystem : MonoBehaviour
{
    public const int MAX_QUADS_AMOUNT = 20000;

    [System.Serializable]
    public struct ParticleUVPixels
    {
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    public struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private SCR_EnemyHealth enemyHealth;
    [SerializeField] private float particleSizeScale = 1f;
    [SerializeField] private float particleRotationSpeed = 45f; // degrees per second
    [SerializeField] private ParticleUVPixels[] particleUVPixelsArray;
    public UVCoords[] uvCoordsArray;
    private UVCoords[] quadUVCoords;

    private Mesh mesh;
    private Vector3[] quadVelocities;
    private Vector3[] quadPositions;
    private float[] quadRotations;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private int quadIndex;

    private bool updateVerticies;
    private bool updateUVs;
    private bool updateTriangles;
    
    private void Awake()
    {
        if (GetComponent<MeshFilter>() == null)
        {
            gameObject.AddComponent<MeshFilter>();
        }

        if (GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
        }

        mesh = new Mesh();
        
        quadUVCoords = new UVCoords[MAX_QUADS_AMOUNT];
        quadVelocities = new Vector3[MAX_QUADS_AMOUNT];
        quadPositions = new Vector3[MAX_QUADS_AMOUNT];
        quadRotations = new float[MAX_QUADS_AMOUNT];
        vertices = new Vector3[4 * MAX_QUADS_AMOUNT];
        uv = new Vector2[4 * MAX_QUADS_AMOUNT];
        triangles = new int[6 * MAX_QUADS_AMOUNT];

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        Material material = GetComponent<MeshRenderer>().material;
        Texture mainTexture = material.mainTexture;
        int textureWidth = mainTexture.width;
        int textureHeight = mainTexture.height;

        // Create a list of UVCoords and populate it
        List<UVCoords> uvCoordsList = new List<UVCoords>();
        foreach (ParticleUVPixels particleUVPixels in particleUVPixelsArray)
        {
            UVCoords uvCoords = new UVCoords
            {
                uv00 = new Vector2(
                    (float)particleUVPixels.uv00Pixels.x / textureWidth,
                    (float)particleUVPixels.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(
                    (float)particleUVPixels.uv11Pixels.x / textureWidth,
                    (float)particleUVPixels.uv11Pixels.y / textureHeight),
            };
            uvCoordsList.Add(uvCoords);

            // Debugging to ensure everything is added correctly
            /*Debug.Log($"Added UVCoords: uv00 = {uvCoords.uv00}, uv11 = {uvCoords.uv11}");*/
        }

        // Check if the list has valid entries
        /*Debug.Log($"UV Coords List Count: {uvCoordsList.Count}");*/

        // Use the random selection method
        if (uvCoordsList.Count > 0)
        {
            UVCoords randomParticle = GetRandomParticleFromList(uvCoordsList);
            /*Debug.Log($"Random Particle Selected: uv00 = {randomParticle.uv00}, uv11 = {randomParticle.uv11}");*/
        }
        else
        {
            Debug.LogError("The UV Coords List is empty! Cannot select a random particle.");
        }

        // Convert the list to an array for use elsewhere in the code
        uvCoordsArray = uvCoordsList.ToArray();
    }
    
    public UVCoords GetRandomParticleFromList(List<UVCoords> uvCoordsList)
    {
        if (uvCoordsList == null || uvCoordsList.Count == 0)
        {
            Debug.LogError("UVCoordsList is null or empty");
            throw new System.InvalidOperationException("No valid particles available.");
        }
      
        int randomIndex = Random.Range(0, uvCoordsList.Count);
        return uvCoordsList[randomIndex];
    }

    /*private void OnPlayerAttack(Vector2 attackPosition, Vector2 attackDirection)
    {
        Vector3 quadPosition = attackPosition;
        /*float rotation = 0f;#1#
        Vector3 quadSize = new Vector3(1f, 1f);
        
        SCR_WeaponParticles.Instance.SpawnWeaponParticle(quadPosition, new Vector3(1f, 1f));
        
        /*if (quadIndex < MAX_QUADS_AMOUNT)
        {
            int spawnedQuadIndex = AddQuad(quadPosition, rotation, quadSize, true, 0);

            Debug.Log($"Attack at: {attackPosition}, {gameObject.name}");

            SCR_FunctionUpdater.Create(() =>
            {
                quadPosition += new Vector3(1, 1) * Time.deltaTime;
                UpdateQuad(spawnedQuadIndex, quadPosition, 1f, new Vector3(1f, 1f), true, 0);

                mesh.RecalculateBounds();

                if (Vector3.Distance(quadPosition, transform.position) > 5f)
                {
                    return true;
                }

                return false;
            });
        }#1#
    }*/
    
    public void RecalculateMeshBounds()
    {
        mesh.RecalculateBounds();
    }
    
    public int AddQuad(Vector3 position, Vector3 particleDirection, float rotation, Vector3 quadSize, UVCoords uvCoords)
    {
        if (quadIndex >= MAX_QUADS_AMOUNT)
        {
            Debug.LogWarning("Max quads reached - no new quads will be added.");
            return -1;
        } 
       
        int allocatedIndex = quadIndex;
        /*Debug.Log($"Allocated quadIndex: {allocatedIndex} for position: {position}");
        */

        quadPositions[quadIndex] = position;
        quadRotations[quadIndex] = rotation;

        const float particleSpeed = 2f;
        quadVelocities[quadIndex] = particleDirection.normalized * particleSpeed;
        
        quadUVCoords[quadIndex] = uvCoords;
        
        Vector3 scaleQuadSize = quadSize * particleSizeScale;
        UpdateQuad(quadIndex, position, rotation, scaleQuadSize, uvCoords);
        
        /*int spawnedQuadIndex = quadIndex;*/
        quadIndex++;
        
        return allocatedIndex;
    }
    
    public void SetQuadPosition(int quadIndex, Vector3 position)
    {
        if (quadIndex < 0 || quadIndex >= MAX_QUADS_AMOUNT) return;
        quadPositions[quadIndex] = position;
        
        updateVerticies = true;
    }

    public void UpdateQuad(int quadIndex, Vector3 position, float rotation, Vector3 quadSize, UVCoords uvCoords)
    {
        Vector3 scaledQuadSize = quadSize * particleSizeScale;
        float totalRotation = rotation + quadRotations[quadIndex];
        
        // Relocate vertices
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        /*if (skewed)
        {*/
            vertices[vIndex0] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation) * 
                new Vector3(-scaledQuadSize.x, -scaledQuadSize.y);
            vertices[vIndex1] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation) * 
                new Vector3(-scaledQuadSize.x, +scaledQuadSize.y);
            vertices[vIndex2] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation) * 
                new Vector3(+scaledQuadSize.x, +scaledQuadSize.y);
            vertices[vIndex3] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation) * 
                new Vector3(+scaledQuadSize.x, -scaledQuadSize.y);
        /*}
        else
        {
            vertices[vIndex0] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation - 180) * 
                scaledQuadSize;
            vertices[vIndex1] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation - 270) * 
                scaledQuadSize;
            vertices[vIndex2] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation - 0) * 
                scaledQuadSize;
            vertices[vIndex3] = 
                position + 
                Quaternion.Euler(0, 0, totalRotation - 90) * 
                scaledQuadSize;
        }*/
      
        
        // UV
        /*UVCoords uvCoords = uvCoordsArray[uvIndex];*/
        uv[vIndex0] = uvCoords.uv00;
        uv[vIndex1] = new Vector2(uvCoords.uv00.x, uvCoords.uv11.y);
        uv[vIndex2] = uvCoords.uv11;
        uv[vIndex3] = new Vector2(uvCoords.uv11.x, uvCoords.uv00.y);
        
        // Create Triangles
        int tIndex = quadIndex * 6;
        
        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;
        
        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;
        
        updateVerticies = true;
        updateUVs = true;
        updateTriangles = true;
    }

    public void DestroyQuad(int quadIndex)
    {
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;
        
        vertices[vIndex0] = Vector3.zero;
        vertices[vIndex1] = Vector3.zero;
        vertices[vIndex2] = Vector3.zero;
        vertices[vIndex3] = Vector3.zero;
        
        updateVerticies = true;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < quadIndex; i++)
        {
            if (quadVelocities[i] == Vector3.zero) continue;
            
            /*quadPositions[i] += quadVelocities[i] * Time.deltaTime;*/
            
            quadRotations[i] += particleRotationSpeed * Time.deltaTime;
            UpdateQuad(i, quadPositions[i], quadRotations[i], Vector3.one, quadUVCoords[i]);
        }
        
        if (updateVerticies)
        {
            mesh.vertices = vertices;
            updateVerticies = false;
        }

        if (updateUVs)
        {
            mesh.uv = uv;
            updateUVs = false;
        }

        if (updateTriangles)
        {
            mesh.triangles = triangles;
            updateTriangles = false;
        }
    }
}
