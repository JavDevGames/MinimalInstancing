using UnityEngine;

public class DrawInstancedScript : MonoBehaviour
{
    const float BATCH_MAX_FLOAT = 1023f;
    const int BATCH_MAX = 1023;

    public GameObject prefab;
    public Material meshMaterial;
    
    public int width;
    public int depth;
    public int height;

    public float spacing;
    public float threshold;

    private MeshFilter mMeshFilter;
    private MeshRenderer mMeshRenderer;
    private Vector4[] colorArray;
    private Matrix4x4[] matrices;
    private MaterialPropertyBlock propertyBlock;
    
    void Start()
    {
        mMeshFilter = prefab.GetComponent<MeshFilter>();
        mMeshRenderer = prefab.GetComponent<MeshRenderer>();
        
        InitData();
    }

    private void InitData()
    {
        Vector3 pos = new Vector3();
        Vector3 scale = new Vector3(1, 1, 1);

        int count = width * depth;
        matrices = new Matrix4x4[count];
        colorArray = new Vector4[count];
        propertyBlock = new MaterialPropertyBlock();
        
        Color[] colors = {
                            HexToColor(0x03A9F4),
                            HexToColor(0xE1F5FE),
                            HexToColor(0xB3E5FC),
                            HexToColor(0x81D4FA),
                            HexToColor(0x4FC3F7),
                            HexToColor(0x29B6F6),
                            HexToColor(0x03A9F4),
                            HexToColor(0x039BE5),
                            HexToColor(0x0288D1),
                            HexToColor(0x0277BD),
                            HexToColor(0x01579B),
                            HexToColor(0x80D8FF),
                            HexToColor(0x40C4FF),
                            HexToColor(0x00B0FF),
                            HexToColor(0x0091EA)
                        };

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < depth; ++j)
            {
                int idx = i * depth + j;

                matrices[idx] = Matrix4x4.identity;

                pos.x = i * spacing;
                pos.y = Random.Range(-height, height);
                pos.z = j * spacing;

                float curNoise = Mathf.PerlinNoise(pos.x / (float)width, pos.z / (float)depth);

                if (curNoise >= threshold)
                {
                    matrices[idx].SetTRS(pos, Quaternion.identity, scale);
                }

                colorArray[idx] = colors[idx % colors.Length];
            }
        }
    }

    private Color HexToColor(uint hex)
    {
        float r = (hex >> 16) & 0xff;
        float g = (hex >> 8) & 0xff;
        float b = (hex) & 0xff;
        Color newColor = new Color(r / 255f, g / 255f, b / 255f);

        return newColor;
    }

    void Update()
    {
        int total = width * depth;
        int batches = (int)Mathf.Ceil(total / BATCH_MAX_FLOAT);

        for (int i = 0; i < batches; ++i)
        {
            int batchCount = Mathf.Min(BATCH_MAX, total - (BATCH_MAX * i));

            int start = Mathf.Max(0, (i - 1) * BATCH_MAX);

            Matrix4x4[] batchedMatrices = GetBatchedMatrices(start, batchCount);
            Vector4[] batchedColorArray = GetBatchedArray(start, batchCount);

            propertyBlock.SetVectorArray("_Color", batchedColorArray);

            Graphics.DrawMeshInstanced(mMeshFilter.sharedMesh, 0, meshMaterial, batchedMatrices, batchCount, propertyBlock);
        }
    }

    private Vector4[] GetBatchedArray(int offset, int batchCount)
    {
        Vector4[] batchedArray = new Vector4[batchCount];

        for (int i = 0; i < batchCount; ++i)
        {
            batchedArray[i] = colorArray[i + offset];
        }

        return batchedArray;
    }

    private Matrix4x4[] GetBatchedMatrices(int offset, int batchCount)
    {
        Matrix4x4[] batchedMatrices = new Matrix4x4[batchCount];

        for (int i = 0; i < batchCount; ++i)
        {
            batchedMatrices[i] = matrices[i + offset];
        }

        return batchedMatrices;
    }
}
