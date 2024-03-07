using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stevenroom : Room
{
    public GameObject wallPrefab;
    public GameObject damagingTilePrefab;
    public GameObject enemyType1Prefab;
    public GameObject enemyType2Prefab;

    public int minDamagingTiles = 2, maxDamagingTiles = 6;
    public int minEnemiesType1 = 1, maxEnemiesType1 = 3;
    public int minEnemiesType2 = 1, maxEnemiesType2 = 2;

    public float wallProbability = 0.75f;

    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {
        generateWalls(ourGenerator, requiredExits);

        placeDamagingTiles();
        placeEnemies(enemyType1Prefab, minEnemiesType1, maxEnemiesType1);
        placeEnemies(enemyType2Prefab, minEnemiesType2, maxEnemiesType2);
    }

    protected void generateWalls(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {
        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                if ((x == 0 || x == LevelGenerator.ROOM_WIDTH - 1 || y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1) && Random.value <= wallProbability)
                {
                    if (!(requiredExits.upExitRequired && x == LevelGenerator.ROOM_WIDTH / 2 && y == LevelGenerator.ROOM_HEIGHT - 1) &&
                        !(requiredExits.rightExitRequired && x == LevelGenerator.ROOM_WIDTH - 1 && y == LevelGenerator.ROOM_HEIGHT / 2) &&
                        !(requiredExits.downExitRequired && x == LevelGenerator.ROOM_WIDTH / 2 && y == 0) &&
                        !(requiredExits.leftExitRequired && x == 0 && y == LevelGenerator.ROOM_HEIGHT / 2))
                    {
                        Tile.spawnTile(wallPrefab, transform, x, y);
                    }
                }
            }
        }
    }

    protected void placeDamagingTiles()
    {
        int numTiles = Random.Range(minDamagingTiles, maxDamagingTiles + 1);
        spawnRandomObjectsInRoom(damagingTilePrefab, numTiles);
    }

    protected void placeEnemies(GameObject enemyPrefab, int minEnemies, int maxEnemies)
    {
        int numEnemies = Random.Range(minEnemies, maxEnemies + 1);
        spawnRandomObjectsInRoom(enemyPrefab, numEnemies);
    }

    protected void spawnRandomObjectsInRoom(GameObject prefab, int numObjects)
    {
        List<Vector2> availablePositions = getAvailablePositionsInRoom();
        for (int i = 0; i < numObjects && availablePositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 spawnPos = availablePositions[randomIndex];
            Tile.spawnTile(prefab, transform, (int)spawnPos.x, (int)spawnPos.y);
            availablePositions.RemoveAt(randomIndex);
        }
    }

    protected List<Vector2> getAvailablePositionsInRoom()
    {
        List<Vector2> positions = new List<Vector2>();
        for (int x = 1; x < LevelGenerator.ROOM_WIDTH - 1; x++)
        {
            for (int y = 1; y < LevelGenerator.ROOM_HEIGHT - 1; y++)
            {
                positions.Add(new Vector2(x, y));
            }
        }
        return positions;
    }
}
