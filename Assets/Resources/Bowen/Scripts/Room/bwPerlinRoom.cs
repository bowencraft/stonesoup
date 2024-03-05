using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An example of a simple room generator.
// Walls are spawned randomly (unless an exit is required)
// Also spawns a random number of normal rocks and basic enemies.
public class bwPerlinRoom : Room {

	// The room needs references to the things it can spawn.
	public GameObject riverPrefab;
	public GameObject wallPrefab;
	public GameObject rockPrefab;
	public GameObject[] orePrefabs;

	public int minNumRocks = 4, maxNumRocks = 14;
	public int minNumEnemies = 1, maxNumEnemies = 4;

	// Chance a wall
	public float borderWallProbability = 0.7f;


    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// It's very likely you'll want to do different generation methods depending on which required exits you receive
		// Here's an example of randomly choosing between two generation methods.

			roomGenerationVersionTwo(ourGenerator, requiredExits);

	}

	protected void roomGenerationVersionTwo(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// 生成墙壁和其他内容。
		// generateWalls(ourGenerator, requiredExits);
		
		List<Vector2> roomTiles = new List<Vector2>();
		List<Vector2> spawners = new List<Vector2>();
    
		// 生成噪声影响的岩石。
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				// 使用柏林噪声决定是否放置岩石。
				
				float noiseValue = Mathf.PerlinNoise((Tile.toWorldCoord(new Vector2(x,y)).x + transform.position.x) * 0.04f, (Tile.toWorldCoord(new Vector2(x,y)).y + transform.position.y) * 0.04f); // 放大系数可以调整以改变噪声的“频率”。
				if (noiseValue > 0.7f) {
					// 如果噪声值高于0.3，留空；否则放置岩石。
					// 这里需要确保不在边界放置岩石，以避免阻塞出口。
					// if (!(x == 0 || x == LevelGenerator.ROOM_WIDTH - 1 || y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1)) {
					Tile.spawnTile(wallPrefab, transform, x, y);
					
					roomTiles.Add(new Vector2(x,y));
					// }
				} else if (noiseValue < 0.2f)
				{
					// roomTiles[x][y] = Tile.spawnTile(rockPrefab, transform, x, y);
					if (!IsSpawnerInRadius(spawners, new Vector2(x, y), 5)) {
						// 在这里生成spawnerPrefab
						Tile.spawnTile(rockPrefab, transform, x, y);
						spawners.Add(new Vector2(x, y));
						
						roomTiles.Add(new Vector2(x,y));
					}
				}
				else if (noiseValue > 0.4f && noiseValue < 0.6f)
				{
					Tile.spawnTile(riverPrefab, transform, x, y);
					
					roomTiles.Add(new Vector2(x,y));
				}
			}
		}

		// 生成敌人。
		int numEnemies = Random.Range(minNumEnemies, maxNumEnemies + 1);
		List<Vector2> possibleSpawnPositions = new List<Vector2>();
		for (int i = 0; i < numEnemies; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 1; x < LevelGenerator.ROOM_WIDTH - 1; x++) {
				for (int y = 1; y < LevelGenerator.ROOM_HEIGHT - 1; y++) {
					// 检查位置是否已被占用（例如，通过岩石）。
					// if (!Physics2D.OverlapPoint(Tile.toWorldCoord(new Vector2(x,y)))) {
					if (!roomTiles.Contains(new Vector2(x, y))) {
						Debug.Log("possible Spawn Position added");
						possibleSpawnPositions.Add(new Vector2(x, y));
					}
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				GameObject orePrefab = orePrefabs[Random.Range(0,orePrefabs.Length)];
				
				
				Tile.spawnTile(orePrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				roomTiles.Add(new Vector2((int)spawnPos.x,(int)spawnPos.y));
			}
		}
	}

	protected bool IsSpawnerInRadius(List<Vector2> spawners, Vector2 position, float radius) {
		foreach (var spawner in spawners) {
			if (Vector2.Distance(spawner, position) <= radius) {
				return true; // 找到了一个spawner在指定半径内
			}
		}
		return false; // 没有找到任何spawner在指定半径内
	}



	protected void generateWalls(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// Basically we go over the border and determining where to spawn walls.
		bool[,] wallMap = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					
					if (x == LevelGenerator.ROOM_WIDTH/2 
						&& y == LevelGenerator.ROOM_HEIGHT-1
                        && requiredExits.upExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH-1
						     && y == LevelGenerator.ROOM_HEIGHT/2
                             && requiredExits.rightExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH/2
						     && y == 0
                             && requiredExits.downExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == 0 
						     && y == LevelGenerator.ROOM_HEIGHT/2 
                             && requiredExits.leftExitRequired) {
						wallMap[x, y] = false;
					}
					else {
						wallMap[x, y] = Random.value <= borderWallProbability;
					}
					continue;
				}
				wallMap[x, y] = false;
			}
		}

		// Now actually spawn all the walls.
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (wallMap[x, y]) {
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
				}
			}
		}
	}



	// Simple utility function because I didn't bother looking up a more general Contains function for arrays.
	// Whoops.
	protected bool containsDir (Dir[] dirArray, Dir dirToCheck) {
		foreach (Dir dir in dirArray) {
			if (dirToCheck == dir) {
				return true;
			}
		}
		return false;
	}

}
