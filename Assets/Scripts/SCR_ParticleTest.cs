using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_ParticleTest : MonoBehaviour
{
    private const int MAX_QUADS_AMOUNT = 15000;

    [System.Serializable]
    public struct ParticleUVPixels
    {
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }
    
    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private SCR_PlayerWeapon playerWeapon;
    [SerializeField] private ParticleUVPixels[] particleUVPixelsArray;
    private UVCoords[] uvCoordsArray;
    
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private int quadIndex;
    
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
        
        /*if (playerWeapon != null)
        {
            playerWeapon.AttackEvent += OnPlayerAttack;
            Debug.Log("AttackEvent subscribed successfully");
        }
        else
        {
            Debug.LogError("playerWeapon is null!");
        }*/
        
        mesh = new Mesh();

        vertices = new Vector3[4 * MAX_QUADS_AMOUNT];
        uv = new Vector2[4 * MAX_QUADS_AMOUNT];
        triangles = new int[6 * MAX_QUADS_AMOUNT];

        /*playerWeapon.AttackEvent += OnPlayerAttack;*/
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
       
        GetComponent<MeshFilter>().mesh = mesh;
        
        Material material = GetComponent<MeshRenderer>().material;
        Texture mainTexture = material.mainTexture;
        int textureWidth = mainTexture.width;
        int textureHeight = mainTexture.height;
        
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
        }
        uvCoordsArray = uvCoordsList.ToArray();
    }

    private void OnPlayerAttack(Vector2 attackPosition, Vector2 attackDirection)
    {
        Debug.Log("OnPlayerAttack called");
        Vector3 quadPosition = attackPosition;
        /*float rotation = 0f;*/
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
        }*/
    }
    
    public void RecalculateMeshBounds()
    {
        mesh.RecalculateBounds();
    }
    
    public int AddQuad(Vector3 position, float rotation, Vector3 quadSize, bool skewed, int uvIndex)
    {
        if (quadIndex >= MAX_QUADS_AMOUNT) return 0; // Mesh full
        
        UpdateQuad(quadIndex, position, 0f, new Vector3(1f, 1f), skewed, uvIndex);
        
        int spawnedQuadIndex = quadIndex;
        quadIndex++;
        
        return spawnedQuadIndex;
    }

    public void UpdateQuad(int quadIndex, Vector3 position, float rotation, Vector3 quadSize, bool skewed, int uvIndex)
    {
        // Relocate vertices
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        if (skewed)
        {
            vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
            vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
            vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, +quadSize.y);
            vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, -quadSize.y);
        }
        else
        {
            vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation - 180) * quadSize;
            vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation - 270) * quadSize;
            vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation - 0) * quadSize;
            vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation - 90) * quadSize;
        }
      
        
        // UV
        UVCoords uvCoords = uvCoordsArray[uvIndex];
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
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void OnDestroy()
    {
        if (playerWeapon != null)
        {
            playerWeapon.AttackEvent -= OnPlayerAttack;
        }
    }
}
