using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInterpolation : MonoBehaviour
{
    Vector2 topRight;
    Vector2 bottomRight;
    Vector2 bottomLeft;
    Vector2 topLeft;
    Vector2 topMid;
    Vector2 rightMid;
    Vector2 bottomMid;
    Vector2 leftMid;

    // Start is called before the first frame update

    [Header("Elements")]
    [SerializeField] private MeshFilter filter;

    [Header("Settings")]
    [SerializeField] private float gridCubeLength;
    [SerializeField] private float isoValue;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    [Header("Configurations")]
    [SerializeField] private bool topRightState;
    [SerializeField] private bool bottomRightState;
    [SerializeField] private bool bottomLeftState;
    [SerializeField] private bool topLeftState;


    void Start()
    {
        topRight = gridCubeLength * Vector2.one / 2;
        bottomRight = topRight + Vector2.down * gridCubeLength;
        bottomLeft = bottomRight + Vector2.left * gridCubeLength;
        topLeft = bottomLeft + Vector2.up * gridCubeLength;

        rightMid = (topRight + bottomRight) / 2;
        bottomMid = (bottomLeft + bottomRight) / 2;
        leftMid = (bottomLeft + topLeft) / 2;
        topMid = (topRight + topLeft) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = new Mesh();
        Debug.Log("Configuration is:    " + GetConfig());
        vertices.Clear();
        triangles.Clear();

        Triangulate(GetConfig());

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        filter.mesh = mesh;
    }

    private void Triangulate(int conf) //The method responsible for updating the vertices and triangles list
    {
        switch (conf)
        {
            case (0):       //0000
                break;

            case (1):        //0001
                vertices.AddRange(new Vector3[] { topRight, rightMid, topMid });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;

            case (2):        //0010
                vertices.AddRange(new Vector3[] { rightMid, bottomRight, bottomMid });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;

            case (3):        //0011
                vertices.AddRange(new Vector3[] { topRight, bottomRight, topMid, bottomMid });
                triangles.AddRange(new int[] { 0, 1, 2, 2, 1, 3 });
                break;

            case (4):        //0100
                vertices.AddRange(new Vector3[] { leftMid, bottomMid, bottomLeft });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;

            case (5):        //0101
                vertices.AddRange(new Vector3[] { topRight, rightMid, bottomMid, bottomLeft, leftMid, topMid });
                triangles.AddRange(new int[] { 0, 1, 5, 1, 2, 4, 4, 2, 3, 5, 1, 4 });
                break;

            case (6):        //0110
                vertices.AddRange(new Vector3[] { rightMid, bottomRight, bottomLeft, leftMid });
                triangles.AddRange(new int[] { 0, 1, 3, 3, 1, 2 });
                break;

            case (7):        //0111
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, leftMid, topMid });
                triangles.AddRange(new int[] { 0, 1, 4, 4, 1, 3, 3, 1, 2 });
                break;

            case (8):        //1000
                vertices.AddRange(new Vector3[] { topLeft, topMid, leftMid });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;

            case (9):        //1001
                vertices.AddRange(new Vector3[] { topRight, rightMid, leftMid, topLeft });
                triangles.AddRange(new int[] { 0, 1, 3, 3, 1, 2 });
                break;

            case (10):        //1010
                vertices.AddRange(new Vector3[] { rightMid, bottomRight, bottomMid, leftMid, topLeft, topMid });
                triangles.AddRange(new int[] { 0, 1, 2, 2, 3, 5, 4, 5, 3, 5, 0, 2 });
                break;

            case (11):        //1011
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomMid, leftMid, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 });
                break;

            case (12):        //1100
                vertices.AddRange(new Vector3[] { bottomMid, bottomLeft, topLeft, topMid });
                triangles.AddRange(new int[] { 3, 0, 1, 2, 3, 1 });
                break;

            case (13):        //1101
                vertices.AddRange(new Vector3[] { topRight, rightMid, bottomMid, bottomLeft, topLeft });
                triangles.AddRange(new int[] { 0, 1, 4, 4, 1, 2, 4, 2, 3 });
                break;

            case (14):        //1110
                vertices.AddRange(new Vector3[] { rightMid, bottomRight, bottomLeft, topLeft, topMid });
                triangles.AddRange(new int[] { 0, 1, 2, 4, 0, 2, 4, 2, 3 });
                break;

            case (15):        //1111
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;
        }
    }

    private int GetConfig()
    {
        int config = 0;

        if (topRightState)
            config = config | (1 << 0);
        if (bottomRightState)
            config = config | (1 << 1);
        if (bottomLeftState)
            config = config | (1 << 2);
        if (topLeftState)
            config = config | (1 << 3);

        return config;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(topRight, gridCubeLength / 30);
        Gizmos.DrawSphere(bottomRight, gridCubeLength / 30);
        Gizmos.DrawSphere(bottomLeft, gridCubeLength / 30);
        Gizmos.DrawSphere(topLeft, gridCubeLength / 30);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(topMid, gridCubeLength / 50);
        Gizmos.DrawSphere(rightMid, gridCubeLength / 50);
        Gizmos.DrawSphere(bottomMid, gridCubeLength / 50);
        Gizmos.DrawSphere(leftMid, gridCubeLength / 50);
    }
}
