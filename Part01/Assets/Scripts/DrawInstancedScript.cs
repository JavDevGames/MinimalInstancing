using UnityEngine;

public class DrawInstancedScript : MonoBehaviour
{
    const float BATCH_MAX_FLOAT = 1023f;
    const int BATCH_MAX = 1023;

    public GameObject prefab;
    public Material meshMaterial;
    public int width;
    public int depth;    
    public float spacing;
    
    private MeshFilter mMeshFilter;
    private MeshRenderer mMeshRenderer;
    private Matrix4x4[] matrices;

    void Start ()
    {
        mMeshFilter = prefab.GetComponent<MeshFilter>();
        mMeshRenderer = prefab.GetComponent<MeshRenderer>();
        
        InitData();
    }

    private void InitData()
    {
        int count = width * depth;

        matrices = new Matrix4x4[count];
        Vector3 pos = new Vector3();
        Vector3 scale = new Vector3(1, 1, 1);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < depth; ++j)
            {
                int idx = i * depth + j;

                matrices[idx] = Matrix4x4.identity;

                pos.x = i * spacing;
                pos.y = 0;
                pos.z = j * spacing;

                matrices[idx].SetTRS(pos, Quaternion.identity, scale);
            }
        }
    }

    void Update ()
    {
        int total = width * depth;
        int batches = Mathf.CeilToInt(total / BATCH_MAX_FLOAT);

        for (int i = 0; i < batches; ++i)
        {
            int batchCount = Mathf.Min(1023, total - (BATCH_MAX * i));
            int start = Mathf.Max(0, (i - 1) * BATCH_MAX);

            Matrix4x4[] batchedMatrices = GetBatchedMatrices(start, batchCount);
            Graphics.DrawMeshInstanced(mMeshFilter.sharedMesh, 0, meshMaterial, batchedMatrices, batchCount);
        }
    }

    private Matrix4x4[] GetBatchedMatrices(int offset, int batchCount)
    {
        Matrix4x4[] batchedMatrices = new Matrix4x4[batchCount];

        for(int i = 0; i < batchCount; ++i)
        {
            batchedMatrices[i] = matrices[i + offset];
        }

        return batchedMatrices;
    }
}
