using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    [Header("Brush Settings")]
    [SerializeField] private int brushRadius;
    [SerializeField] private float brushStrength;
    [SerializeField] private float brushFallback;

    [Header("Elements")]
    [SerializeField] private MeshFilter filter;


    [Header("Data")]
    [SerializeField] private int gridCubeColumns;
    [SerializeField] private float gridCubeLength;
    [SerializeField] private float isoValue;

    private SquareGrid squareGrid;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    Mesh mesh;

    private float[,] grid;
    void Awake()
    {
        InputManager.onTouching += TouchingCallback;
    }

    private void OnDestroy()
    {
        InputManager.onTouching -= TouchingCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        mesh = new Mesh();

        grid = new float[gridCubeColumns, gridCubeColumns];
        for (int x = 0; x < gridCubeColumns; x++)
        {
            for (int y = 0; y < gridCubeColumns; y++)
            {
                grid[x, y] = isoValue + 0.1f;
            }
        }

        squareGrid = new SquareGrid(gridCubeColumns - 1, gridCubeLength, isoValue);
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TouchingCallback(Vector3 hitLocation)
    {
        hitLocation.z = 0;
        //hitLocation = transform.InverseTransformPoint(hitLocation);

        Debug.Log("World Position:  " + hitLocation);

        Vector2Int gridPosition = GetGridPositionFromWorldPosition(hitLocation);

        bool shouldGenerate = false;

        for (int y = gridPosition.y - brushRadius; y <= gridPosition.y + brushRadius; y++)
        {
            for (int x = gridPosition.x - brushRadius; x <= gridPosition.x + brushRadius; x++)
            {
                Vector2Int currentGridPosition = new Vector2Int(x, y);
                if (!IsValidGridPosition(currentGridPosition))
                {
                    Debug.Log("barra el grid fel loop");
                    continue;
                }

                Debug.Log("Grid Position:  " + gridPosition);

                float distance = Vector2.Distance(currentGridPosition, gridPosition);
                float factor = brushStrength * Mathf.Exp(-distance * brushFallback / brushRadius);

                grid[currentGridPosition.x, currentGridPosition.y] -= factor;

                shouldGenerate = true;
            }
        }
        if (shouldGenerate)
            GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();

        vertices.Clear();
        triangles.Clear();

        squareGrid.Update(grid);

        mesh.vertices = squareGrid.GetVertices();
        mesh.triangles = squareGrid.GetTriangles();

        filter.mesh = mesh;

        GenerateCollider();
    }

    private void GenerateCollider()
    {
        if (filter.TryGetComponent(out MeshCollider meshCollider))
        {
            meshCollider.sharedMesh = mesh;
        }
        else
        {
            filter.gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }
    }

    private Vector2 GetWorldPosfromGridPos(int x, int y)
    {
        Vector2 worldPosition = new Vector2(x, y) * gridCubeLength;

        worldPosition.x -= gridCubeLength * gridCubeColumns / 2 - gridCubeLength / 2;
        worldPosition.y -= gridCubeLength * gridCubeColumns / 2 - gridCubeLength / 2;
        return worldPosition;
    }

    private Vector2Int GetGridPositionFromWorldPosition(Vector2 worldPos)
    {
        Vector2Int gridPosition = new Vector2Int
        {
            x = Mathf.FloorToInt((worldPos.x + gridCubeColumns * gridCubeLength / 2 + gridCubeLength / 2) / gridCubeLength),
            y = Mathf.FloorToInt((worldPos.y + gridCubeColumns * gridCubeLength / 2 + gridCubeLength / 2) / gridCubeLength)
        };

        return gridPosition;
    }


    private bool IsValidGridPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridCubeColumns && pos.y >= 0 && pos.y < gridCubeColumns;

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
            return;

        Gizmos.color = Color.yellow;
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Vector2 worldPosition = GetWorldPosfromGridPos(x, y);

                Gizmos.DrawSphere(worldPosition, gridCubeLength / 4);
                Handles.Label(worldPosition + Vector2.up * gridCubeLength / 3 + Vector2.left * gridCubeLength / 3, grid[x, y].ToString());
            }
        }
    }
#endif
}
