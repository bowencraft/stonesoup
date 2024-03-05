using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Bowen_Golden_Ore : Wall {
	
	[SerializeField]
	private GameObject[] minerals;
	protected override void die()
	{
		GameObject mineral = minerals[Random.Range(0,minerals.Length)];
		
		GameObject mineralsObj = Instantiate(mineral, transform.position, Quaternion.identity) as GameObject;
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
		//
		//
		
		// Debug.Log("Golden Ore has been mined!");
		base.die();
	}
}
