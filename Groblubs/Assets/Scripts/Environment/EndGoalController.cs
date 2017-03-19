using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoalController : MonoBehaviour
{
    public float length;
    public int resolution;
    public float radius;
    public int bulgeLength;
    public float bulgeSize;

    private bool bulging = false;
    private bool bulgeEnded = false;
    private bool initialised = false;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] indices;

    public delegate void BulgeEnded();
    public static event BulgeEnded OnBulgeEnd;

	// Use this for initialization
	void Start ()
    {
        CreateMesh();	
	}

    void OnAwake()
    {
        CreateMesh();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(bulgeEnded)
        {
            if(OnBulgeEnd != null)
            {
                OnBulgeEnd();
            }
            bulgeEnded = false;
        }
	}

    void CreateMesh()
    {
        int numSlices = resolution;
        int numVertices = numSlices * resolution;

        vertices = new Vector3[numVertices];
        uvs = new Vector2[numVertices];
        indices = new int[(numVertices * 6)];

        Vector3 pos = Vector3.zero;

        for(int i = 0; i < numSlices; i++)
        {
            for (int vert = 0; vert < resolution; vert++)
            {
                float angle = (float)vert / (float)resolution * 2 * Mathf.PI;
                vertices[i * resolution + vert] = pos + Vector3.up * radius * Mathf.Sin(angle) - Vector3.forward * radius * Mathf.Cos(angle);
                uvs[i * resolution + vert] = new Vector2(i % 2, Mathf.Sin(angle));
            }
            pos -= Vector3.right * (length / resolution);
        }

        int index = 0;
        for (int vert = 0; vert < vertices.Length - resolution - 1; vert++)
        {
            indices[index] = vert;
            indices[index + 1] = vert + resolution;
            indices[index + 2] = vert + 1;
            indices[index + 3] = vert + 1;
            indices[index + 4] = vert + resolution;
            indices[index + 5] = vert + resolution + 1;

            index += 6;
        }

        indices[index] = 0;
        indices[index + 1] = resolution - 1;
        indices[index + 2] = resolution;
        indices[index + 3] = vertices.Length - 1;
        indices[index + 4] = vertices.Length - resolution;
        indices[index + 5] = vertices.Length - resolution - 1;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = indices;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        initialised = true;
    }

    IEnumerator BulgeForward()
    {
        if (!initialised)
            yield break;

        bulging = true;

        for(int bulge = 1; bulge < resolution -1; bulge++)
        {
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < resolution; i++)
            {
                float rad = radius;
                int bulgeDistance = Mathf.Abs(bulge - i);
                if(Mathf.Abs(bulge - i) < bulgeLength && i != 0 && i != resolution - 1)
                {
                    rad = radius + bulgeSize * (1 - ((float)bulgeDistance / (float)bulgeLength));
                }
                for (int vert = 0; vert < resolution; vert++)
                {
                    float angle = (float)vert / (float)resolution * 2 * Mathf.PI;
                    vertices[i * resolution + vert] = pos + Vector3.up * rad * Mathf.Sin(angle) - Vector3.forward * rad * Mathf.Cos(angle);
                    uvs[i * resolution + vert] = new Vector2(i % 2, Mathf.Sin(angle));
                }
                pos -= Vector3.right * (length / resolution);
            }

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            yield return null;
        }

        ResetVertices();

        bulging = false;
        bulgeEnded = true;
    }

    IEnumerator BulgeBackward()
    {
        if (!initialised)
            yield break;

        bulging = true;

        for (int bulge = resolution - 2; bulge > 1; bulge--)
        {
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < resolution-1; i++)
            {
                float rad = radius;
                int bulgeDistance = Mathf.Abs(bulge - i);
                if (Mathf.Abs(bulge - i) < bulgeLength && i != 0 && i != resolution - 1)
                {
                    rad = radius + bulgeSize * (1 - ((float)bulgeDistance / (float)bulgeLength));
                }
                for (int vert = 0; vert < resolution; vert++)
                {
                    float angle = (float)vert / (float)resolution * 2 * Mathf.PI;
                    vertices[i * resolution + vert] = pos + Vector3.up * rad * Mathf.Sin(angle) - Vector3.forward * rad * Mathf.Cos(angle);
                    uvs[i * resolution + vert] = new Vector2(i % 2, Mathf.Sin(angle));
                }
                pos -= Vector3.right * (length / resolution);
            }

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            yield return null;
        }

        ResetVertices();

        bulging = false;
        bulgeEnded = true;
    }

    void ResetVertices()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < resolution; i++)
        {
            float rad = radius;
            for (int vert = 0; vert < resolution; vert++)
            {
                float angle = (float)vert / (float)resolution * 2 * Mathf.PI;
                vertices[i * resolution + vert] = pos + Vector3.up * rad * Mathf.Sin(angle) - Vector3.forward * rad * Mathf.Cos(angle);
                uvs[i * resolution + vert] = new Vector2(i % 2, Mathf.Sin(angle));
            }
            pos -= Vector3.right * (length / resolution);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
