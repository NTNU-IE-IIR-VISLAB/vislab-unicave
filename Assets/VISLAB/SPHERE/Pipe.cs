using UnityEngine;

/// <summary>
/// Generates a Pipe(torus) on runtime. 
/// The torus has
///  - curve radius (the radius of the "circle")
///  - Pipe radius (The diameter of the tupe)
///  - Currve segment count (How many vertex circles around the circle: higher will produce smoother circle)
///  - Pipe segment counts (How many vertecices around the pipe: higher produce smoother tube)
/// 
/// The pipe can be altered at runtime.
/// 
/// Inspiration from:
/// https://catlikecoding.com/unity/tutorials/swirly-pipe/
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Pipe : MonoBehaviour {

    /// <summary>
    /// The radius of the curve / the circle size
    /// </summary>
    [SerializeField]
    private float curveRadius;
    public float CurveRadius { get { return curveRadius; } set { curveRadius = value; } }

    /// <summary>
    /// The radius of the pipe/tube
    /// </summary>
    [SerializeField]
    private float pipeRadius;
    public float PipeRadius { set { pipeRadius = value; } }

    /// <summary>
    /// How many verteci group segement around the circle
    /// </summary>
    [SerializeField]
    private int curveSegmentCount;
    public int CurveSegmentCount { set { curveSegmentCount = value; } }

    /// <summary>
    /// How many vertecies around the tube: hiher value provides smoother tube
    /// </summary>
    [SerializeField]
    private int pipeSegmentCount;

    public int PipeSegmentCount { set { pipeSegmentCount = value; } }

    public float RingDistance { get; set; } = Mathf.PI * 2;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake() {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";
    }
    public void Generate() {
        SetVertices();
        SetTriangles();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    private void SetVertices() {

        vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];
        float uStep = RingDistance / curveSegmentCount;
        CreateFirstQuadRing(uStep);
        int iDelta = pipeSegmentCount * 4;
        for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
            CreateQuadRing(u * uStep, i);
        }
        mesh.vertices = vertices;
    }

    private void CreateQuadRing(float u, int i) {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;
        int ringOffset = pipeSegmentCount * 4;

        Vector3 vertex = GetPointOnTorus(u, 0f);
        for (int v = 1; v <= pipeSegmentCount; v++, i += 4) {
            vertices[i] = vertices[i - ringOffset + 2];
            vertices[i + 1] = vertices[i - ringOffset + 3];
            vertices[i + 2] = vertex;
            vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
        }
    }

    private void CreateFirstQuadRing(float u) {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;

        Vector3 vertexA = GetPointOnTorus(0f, 0f);
        Vector3 vertexB = GetPointOnTorus(u, 0f);
        for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4) {
            vertices[i] = vertexA;
            vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
            vertices[i + 2] = vertexB;
            vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
        }
    }

    private void SetTriangles() {

        triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }
        mesh.triangles = triangles;
    }

    private Vector3 GetPointOnTorus(float u, float v) {
        Vector3 p;
        float r = curveRadius + pipeRadius * Mathf.Cos(v);
        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = pipeRadius * Mathf.Sin(v);
        return p;
    }

    // private void OnDrawGizmos() {
    //     float uStep = (2f * Mathf.PI) / curveSegmentCount;
    //     float vStep = (2f * Mathf.PI) / pipeSegmentCount;

    //     for (int u = 0; u < curveSegmentCount; u++) {
    //         for (int v = 0; v < pipeSegmentCount; v++) {
    //             Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
    //             Gizmos.color = new Color(
    //                 1f,
    //                 (float) v / pipeSegmentCount,
    //                 (float) u / curveSegmentCount);
    //             Gizmos.DrawSphere(point, 0.05f);
    //         }
    //     }
    // }

}