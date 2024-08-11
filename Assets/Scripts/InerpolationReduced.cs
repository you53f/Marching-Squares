using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolationReduced : MonoBehaviour
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
    [SerializeField] private float topRightValue;
    [SerializeField] private float bottomRightValue;
    [SerializeField] private float bottomLeftValue;
    [SerializeField] private float topLeftValue;


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
        vertices.Clear();
        triangles.Clear();

        Square square = new Square(Vector3.zero, gridCubeLength);
        square.Triangulate(isoValue, new float[] {topRightValue, bottomRightValue, bottomLeftValue, topLeftValue});
        
        mesh.vertices = square.GetVertices();
        mesh.triangles = square.GetTriangles();

        filter.mesh = mesh;
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
