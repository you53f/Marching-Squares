using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int terrainGridSize;
    [SerializeField] private float terrainGridScale;

    [Header("Elements")]
    [SerializeField] private Chunks terrainGeneratorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Generate()
    {
        float terrainWorldSize = terrainGridScale * (terrainGridSize-1);

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector2 spawnPosition = Vector2.zero;

                spawnPosition.x = x * terrainWorldSize;
                spawnPosition.y = y * terrainWorldSize;

                spawnPosition.x -= (((float)gridSize.x/2)*terrainWorldSize)-terrainWorldSize/2;
                spawnPosition.y -= (((float)gridSize.y/2)*terrainWorldSize)-terrainWorldSize/2;

                Chunks terrainGenerator =
                Instantiate(terrainGeneratorPrefab,spawnPosition,Quaternion.identity, transform);
            
                terrainGenerator.Initialize(terrainGridSize, terrainGridScale);
            }
        }
    }
}
