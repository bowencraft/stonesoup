using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_HeldablePlank : Tile
{
    public GameObject prefabToSpawn;
    private Vector2 direction;
    public override void useAsItem(Tile tileDroppingUs) {

        // 确定检测碰撞的目标位置。我们使用tileDroppingUs的aimDirection来决定这个位置。
        // 假设每个格子的大小是由Tile.TILE_SIZE定义的。
        direction = tileDroppingUs.aimDirection.normalized;
        Vector2 checkPosition = (Vector2)transform.position + direction * Tile.TILE_SIZE;

        // 使用Physics2D.OverlapPoint来检查目标位置是否有其他Tile存在。
        Collider2D hitCollider = Physics2D.OverlapPoint(checkPosition);

        if (hitCollider != null && hitCollider.GetComponent<Tile>().tileName == "Water") {
            // if ()
            // {
                Vector2 standardSpawnPoint = toWorldCoord(toGridCoord(checkPosition));
                SpawnPrefabAtLocation(standardSpawnPoint, hitCollider.transform);
                
                Destroy(hitCollider.gameObject);
                
                if (deathSFX != null) {
                    AudioManager.playAudio(deathSFX);
                }
                
                takeDamage(this, 1, DamageType.Explosive);
                return;
            // }
        }
        // else
        // {
        //     Vector2 standardSpawnPoint = toWorldCoord(toGridCoord(checkPosition));
        //     SpawnPrefabAtLocation(standardSpawnPoint);
        //         
        //     if (deathSFX != null) {
        //         AudioManager.playAudio(deathSFX);
        //     }
        //         
        //     takeDamage(this, 1, DamageType.Explosive);
        //     return;
        // }
        
        
    }
    
    void SpawnPrefabAtLocation(Vector2 location, Transform parentTransform = null) {
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, location, Quaternion.identity);
        if (parentTransform != null) spawnedPrefab.transform.parent = parentTransform.transform.parent;
        if (spawnedPrefab.GetComponent<Tile>() != null) {
            spawnedPrefab.GetComponent<Tile>().init();
        }
    }

    void Update() {
        if (_tileHoldingUs != null) {
            tileName = string.Format("Placeable Block (Amount: {0})", health);
        }
    }
    protected override void updateSpriteSorting() {
        if (_sprite == null) {
            return;
        }
        if (_tileHoldingUs != null) {
            _sprite.sortingLayerID = _tileHoldingUs.sprite.sortingLayerID;
            _sprite.sortingOrder = _tileHoldingUs.sprite.sortingOrder+1;
            return;
        }
        else if (hasTag(TileTags.CanBeHeld)) {
            _sprite.sortingLayerID = SortingLayer.NameToID("Floor");
        }
        else {
            _sprite.sortingLayerID = SortingLayer.NameToID("Floor");
        }
        _sprite.sortingOrder = -(int)globalY;
    }

}
