using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Demo_2 : MonoBehaviour
{

    private struct Particle
    {
        public Vector3 basePosition;
        public Vector3 position;
        public Vector4 color;
        public float scale;
    }

    [SerializeField]
    private int _Count = 10000;
    [SerializeField]
    private ComputeShader computeShader = null;
    [SerializeField]
    private Mesh mainMesh;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Color _color = Color.blue;
    private ComputeBuffer particleBuffer = null;
    private ComputeBuffer argBuffer = null;
    private uint[] args = new uint[5]{0,0,0,0,0};
    [SerializeField]
    private float particleMaxScale = 0.03f;

    private int kernelID = 0;
    // Start is called before the first frame update
    void Start()
    {
        kernelID = computeShader.FindKernel("ParticleMain");

        List<Vector3> vertices = new List<Vector3>();
        meshFilter.mesh.GetVertices(vertices);
        
        Particle[] particles = new Particle[_Count];

        for (int i = 0; i < _Count; i++)
        {
            particles[i] = new Particle
            {
                basePosition = vertices[i % vertices.Count],
                position = vertices[i % vertices.Count] + Random.insideUnitSphere*10f,
                color = _color,
                scale = Random.Range(0.01f,particleMaxScale),
            };
        }

        particleBuffer = new ComputeBuffer (_Count , Marshal.SizeOf(typeof(Particle)));
        particleBuffer.SetData(particles);

        computeShader.SetBuffer(kernelID , "_ParticleBuffer" ,particleBuffer);

        material.SetBuffer("_ParticleBuffer" , particleBuffer);

        int subMeshIndex = 0;

        args[0] = meshFilter.mesh.GetIndexCount(subMeshIndex);
        args[1] = (uint)_Count;
        args[2] = meshFilter.mesh.GetIndexStart(subMeshIndex);
        args[3] = meshFilter.mesh.GetBaseVertex(subMeshIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
