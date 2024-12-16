using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CustomMesh : MonoBehaviour
{
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;
    [SerializeField] private float tileSize = 10;
   
    private void Start()
    {
       Debug.Log("Custom Mesh Test");
       Mesh mesh = new Mesh();
       
       Vector3[] vertices = new Vector3[4 * (width * height)];
       Vector2[] uvs = new Vector2[4 * (width * height)];
       int[] triangles = new int[6 * (width * height)];

       for (int i = 0; i < width; i++)
       {
           for (int j = 0; j < height; j++)
           {
               int index = i * height + j;
               
               vertices[index * 4 + 0] = new Vector3(tileSize * i,       tileSize * j);
               vertices[index * 4 + 1] = new Vector3(tileSize * i,       tileSize * (j + 1));
               vertices[index * 4 + 2] = new Vector3(tileSize * (i + 1), tileSize * (j + 1));
               vertices[index * 4 + 3] = new Vector3(tileSize * (i + 1), tileSize * j);
       
               uvs[index * 4 + 0] = new Vector2(0, 0);
               uvs[index * 4 + 1] = new Vector2(0, 1);
               uvs[index * 4 + 2] = new Vector2(1, 1);
               uvs[index * 4 + 3] = new Vector2(1, 0);
       
               triangles[index * 6 + 0] = index * 4 + 0;
               triangles[index * 6 + 1] = index * 4 + 1;
               triangles[index * 6 + 2] = index * 4 + 2;
               
               triangles[index * 6 + 3] = index * 4 + 0;
               triangles[index * 6 + 4] = index * 4 + 2;
               triangles[index * 6 + 5] = index * 4 + 3;
           }
       }
       
       mesh.vertices = vertices;
       mesh.triangles = triangles;
       mesh.uv = uvs;
       
       mesh.RecalculateNormals();
       
       GetComponent<MeshFilter>().mesh = mesh;
    }
}
