using System.Collections;
using System.Drawing;
using UnityEngine;

public class SuckPoint : MonoBehaviour
{
    MeshCollider mesh;
    public GameObject point;
    public bool isBitten = false;

    private void Start()
    {
        mesh = GetComponent<MeshCollider>();

        if (point != null && mesh != null)
        {
            PointSpawn();
        }

        if(isBitten == true)
        {
            Debug.Log("Bitten");
            Destroy(point);
            StartCoroutine(RespawnPoint());
        }
    }

    private Vector3 GetRandomPointOnTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float a = Random.value;
        float b = Random.value;

        // Ensure the point is inside the triangle
        if (a + b > 1)
        {
            a = 1 - a;
            b = 1 - b;
        }

        float c = 1 - a - b;

        // Calculate the random point
        return a * v0 + b * v1 + c * v2;
    }

    private void PointSpawn()
    {
        // Get the mesh from the MeshCollider
        Mesh meshData = mesh.sharedMesh;

        // Get the vertices and triangles of the mesh
        Vector3[] vertices = meshData.vertices;
        int[] triangles = meshData.triangles;

        // Select a random triangle
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;

        // Get the vertices of the selected triangle
        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        // Convert local vertices to world space
        v0 = mesh.transform.TransformPoint(v0);
        v1 = mesh.transform.TransformPoint(v1);
        v2 = mesh.transform.TransformPoint(v2);

        // Generate a random point on the triangle
        Vector3 randomPoint = GetRandomPointOnTriangle(v0, v1, v2);

        // Instantiate the point GameObject at the random point
        Instantiate(point, randomPoint, Quaternion.identity); 
        isBitten = false;
    }

    private IEnumerator RespawnPoint()
    {
        yield return new WaitForSeconds(5);
        PointSpawn();
        Debug.Log("Respawn");
    }
}
