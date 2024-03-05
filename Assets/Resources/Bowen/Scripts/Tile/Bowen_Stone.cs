using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Bowen_Stone : Wall
{


	[SerializeField] private GameObject[] dropItems;
	
	[SerializeField]
	private GameObject[] minerals;
	protected override void die()
	{
		
		GameObject dropItem = dropItems[Random.Range(0,dropItems.Length)];

		if (dropItem != null)
		{
			GameObject mineralsObj = Instantiate(dropItem, transform.position, Quaternion.identity) as GameObject;
			//
			Tile tile = mineralsObj.GetComponent<Tile>();
			// //
			// Debug.Log(" "+localX + " " + localY + " " +globalX + " " +globalY);
			// Vector2 tilePos = toWorldCoord(localX, localY);
			// tile.globalX = tilePos.x;
			// tile.globalY = tilePos.y;
			if (tile != null)
			{
				tile.init();
				tile.health = 2;
			}
		}

		// //
		// //
		
		CheckAndReplaceWallsAround();
		
		base.die();
	}
	
	void CheckAndReplaceWallsAround() {
		Vector2[] directions = new Vector2[] {
			Vector2.up, // 上
			Vector2.down, // 下
			Vector2.left, // 左
			Vector2.right // 右
		};

		foreach (Vector2 direction in directions) {
			Vector2 checkPosition = (Vector2)transform.position + direction * Tile.TILE_SIZE;
			Collider2D hitCollider = Physics2D.OverlapPoint(checkPosition);

			if (hitCollider != null && hitCollider.GetComponent<Wall>() != null) {
				// 检测到Wall，替换为Stone
				Vector2 standardSpawnPoint = toWorldCoord(toGridCoord(checkPosition));
				SpawnStoneAtLocation(standardSpawnPoint);
                
				Destroy(hitCollider.gameObject); // 销毁原有Wall
			}
		}
	}

	void SpawnStoneAtLocation(Vector2 location) {
		
		GameObject mineral = minerals[Random.Range(0,minerals.Length)];

		if (mineral != null)
		{
			GameObject spawnedStone = Instantiate(mineral, location, Quaternion.identity);
		if (spawnedStone.GetComponent<Tile>() != null) {
			spawnedStone.GetComponent<Tile>().init();
		}
		}

	}
}
