using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_HeldablePlank : Tile
{
    public GameObject prefabToSpawn;
    private Vector2 direction;
    public override void dropped(Tile tileDroppingUs) {

        // 确定检测碰撞的目标位置。我们使用tileDroppingUs的aimDirection来决定这个位置。
        // 假设每个格子的大小是由Tile.TILE_SIZE定义的。
        direction = tileDroppingUs.aimDirection.normalized;
        Vector2 checkPosition = (Vector2)transform.position + direction * Tile.TILE_SIZE;

        // 使用Physics2D.OverlapPoint来检查目标位置是否有其他Tile存在。
        Collider2D hitCollider = Physics2D.OverlapPoint(checkPosition);

        if (hitCollider != null) {
            if (hitCollider.GetComponent<Tile>().tileName == "Water")
            {
                Vector2 standardSpawnPoint = toWorldCoord(toGridCoord(checkPosition));
                SpawnPrefabAtLocation(standardSpawnPoint, hitCollider.transform);
                
                Destroy(hitCollider.gameObject);
                
                if (deathSFX != null) {
                    AudioManager.playAudio(deathSFX);
                }
                
                takeDamage(this, 1, DamageType.Explosive);
                return;
            }
        }
        base.dropped(tileDroppingUs);
        
    }
    
    void SpawnPrefabAtLocation(Vector2 location, Transform parentTransform) {
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, location, Quaternion.identity);
        spawnedPrefab.transform.parent = parentTransform.transform.parent;
        if (spawnedPrefab.GetComponent<Tile>() != null) {
            spawnedPrefab.GetComponent<Tile>().init();
        }
    }

    void Update() {
        if (_tileHoldingUs != null) {
            tileName = string.Format("Oak Plank (Amount: {0})", health);
        }
    }
}
