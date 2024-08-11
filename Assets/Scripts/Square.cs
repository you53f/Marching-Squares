using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct Square
{
    private Vector2 originalPosition;

    Vector2 topRight;
    Vector2 bottomRight;
    Vector2 bottomLeft;
    Vector2 topLeft;

    Vector2 topMid;
    Vector2 rightMid;
    Vector2 bottomMid;
    Vector2 leftMid;


    private List<Vector3> vertices;
    private List<int> triangles;

    public Square(Vector3 position, float gridCubeLength)
    {
        this.originalPosition = position;

        topRight = originalPosition + gridCubeLength * Vector2.one / 2;
        bottomRight = topRight + Vector2.down * gridCubeLength;
        bottomLeft = bottomRight + Vector2.left * gridCubeLength;
        topLeft = bottomLeft + Vector2.up * gridCubeLength;

        rightMid = (topRight + bottomRight) / 2;
        bottomMid = (bottomLeft + bottomRight) / 2;
        leftMid = (bottomLeft + topLeft) / 2;
        topMid = (topRight + topLeft) / 2;

        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void Triangulate(float isoValue, float[] values)
    {
        vertices.Clear();
        triangles.Clear();
        
        int configuration = GetConfig(isoValue, values);

        Interpolate(isoValue, values);

        Triangulate(configuration);
    }
    private void Interpolate(float isoValue, float[] values)
    {
        float topLerp = Mathf.InverseLerp(values[3], values[0], isoValue);
        topMid = topLeft + (topRight - topLeft) * topLerp;

        float bottomLerp = Mathf.InverseLerp(values[2], values[1], isoValue);
        bottomMid = bottomLeft + (bottomRight - bottomLeft) * bottomLerp;

        float rightLerp = Mathf.InverseLerp(values[0], values[1], isoValue);
        rightMid = topRight + (bottomRight - topRight) * rightLerp;

        float leftLerp = Mathf.InverseLerp(values[3], values[2], isoValue);
        leftMid = topLeft + (bottomLeft - topLeft) * leftLerp;
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

    private int GetConfig(float isoValue, float[] values)
    {
        int config = 0;

        if (values[0] > isoValue)
            config = config | (1 << 0);
        if (values[1] > isoValue)
            config = config | (1 << 1);
        if (values[2] > isoValue)
            config = config | (1 << 2);
        if (values[3] > isoValue)
            config = config | (1 << 3);

        return config;
    }

    public Vector3[] GetVertices()
    {
        return vertices.ToArray();
    }
    public int[] GetTriangles()
    {
        return triangles.ToArray();
    }
}