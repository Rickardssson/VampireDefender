using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUADS_AMOUNT = 10000;
    private Mesh Mesh;
    
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    
    private int quadIndex;
    
    private void Start()
    {
        Debug.Log("Custom Mesh Test");
        Mesh mesh = new Mesh();
        
        vertices = new Vector3[4 * MAX_QUADS_AMOUNT];
        uv = new Vector2[4 * MAX_QUADS_AMOUNT];
        triangles = new int[6 * MAX_QUADS_AMOUNT];

        AddQuad(new Vector3(0, 0));
        AddQuad(new Vector3(0, 5));
       
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
       
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    private void AddQuad(Vector3 position)
    {
        if (quadIndex >= MAX_QUADS_AMOUNT) return; // Mesh full
        
        // Relocate vertices
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;
        
        Vector3 quadSize = new Vector3(1, 1);
        float rotation = 0f;
        vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation - 180) * quadSize;
        vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation - 270) * quadSize;
        vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation - 0) * quadSize;
        vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation - 90) * quadSize;
        
        // UV
        uv[vIndex0] = new Vector2(0, 0);
        uv[vIndex1] = new Vector2(0, 1);
        uv[vIndex2] = new Vector2(1, 1);
        uv[vIndex3] = new Vector2(1, 0);
        
        // Create Triangles
        int tIndex = quadIndex * 6;
        
        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;
        
        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;
        
        quadIndex++;
    }
}
