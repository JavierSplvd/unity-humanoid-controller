using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

public class ObjData
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    public ObjData(Transform t)
    {
        pos = t.position;
        scale = t.localScale;
        rot = t.rotation;
    }

    public ObjData(Vector3 position, bool randomScale)
    {
        pos = position;
        if(randomScale)
        {
            scale = Vector3.one * Random.RandomRange(0.8f, 1.2f);
        }
        else
        {
            scale = Vector3.one;
        }
        rot = Quaternion.Euler(0, Random.Range(0,180), 0);
    }

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }
}
[ExecuteInEditMode]
public class GPUSpawner : MonoBehaviour
{
    public int instances;
    public Mesh objMesh;
    public Material objMat;
    public int intancesPerChild = 75;
    public ShadowCastingMode shadowCastMode = ShadowCastingMode.Off;
    public bool randomScale = false;

    private List<ObjData> pool = new List<ObjData>();

    private List<List<ObjData>> batches = new List<List<ObjData>>();
    private List<Matrix4x4[]> matrices = new List<Matrix4x4[]>();
    void Start()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        InitPool();
        instances = pool.Count;

        int numberOfBatchLists = (int) Mathf.Ceil(instances / 1023f);
        for(int n = 0; n < numberOfBatchLists; n++)
        {
            batches.Add(new List<ObjData>());
            matrices.Add(new Matrix4x4[1]);
        }

        for (int i = 0; i < instances; i++)
        {
            batches[(int) Mathf.Floor(i / 1023f)].Add(pool[i]);
        }

        int j = 0;
        foreach(var b in batches)
        {
            matrices[j] = b.Select((c) => c.matrix).ToArray();
            j = j + 1;
        }

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.LogWarning("Elapsed time (ms) of the GPUSpawner: " + elapsedMs);

    }

    void Update()
    {
        RenderBatches();
    }

    private void RenderBatches()
    {
        int i = 0;
        foreach(var b in batches)
        {
            Graphics.DrawMeshInstanced(
                objMesh, 
                0, 
                objMat, 
                matrices[i],
                count: matrices[i].Length, 
                properties: null,
                castShadows:shadowCastMode,
                receiveShadows: true,
                layer:0,
                camera:null,
                lightProbeUsage: LightProbeUsage.BlendProbes,
                lightProbeProxyVolume: null
            );
            i = i + 1;
        }
    }

    public static float NextGaussian() {
        float v1, v2, s;
        do {
            v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
    
        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
    
        return v1 * s;
    }

    private void InitPool()
    {
        Vector3 modPos = Vector3.zero;
        Vector3 currentPos = Vector3.zero;
        for(int i = 0; i< transform.childCount; i++)
        {
            currentPos = transform.GetChild(i).transform.position;
            for(int j = 0; j< intancesPerChild; j ++)
            {
                modPos = currentPos;
                modPos.x += NextGaussian();
                modPos.z += NextGaussian();
                pool.Add(new ObjData(modPos, randomScale));
            }
        }
    }
}