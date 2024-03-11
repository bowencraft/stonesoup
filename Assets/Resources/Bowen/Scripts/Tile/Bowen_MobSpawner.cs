using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_MobSpawner : Wall {
    public GameObject prefabToSpawn;
    public bool transferSprite = true;
    public GameObject spawnEffect;
    public GameObject refreshingEffect;
    public float spawnCooldown = 5f;
    private float spawnTimer;
    public float checkMonsterRadius = 5f;
    public float checkPlayerRadius = 15f;
    public float spawnDistance = 2f;
    
    public bool standardSpawn = true;

    public Color color1;
    public Color color2;

    private SpriteRenderer mobSprite;
    private SpriteRenderer backgroundSprite;

    protected void Start() {
        spawnTimer = spawnCooldown;
        if (transform.childCount > 0) mobSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (transferSprite) mobSprite.sprite = prefabToSpawn.GetComponent<SpriteRenderer>().sprite;
        
        if (transform.childCount > 1) backgroundSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        // if (backgroundSprite != null)
        // {
        //     backgroundSprite.sortingLayerID = SortingLayer.NameToID("Floor");
        //     backgroundSprite.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
        // }
    }

    protected void Update() {
        if (ShouldTrySpawnPrefab()) {
            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else {
                    TrySpawnPrefab();
                spawnTimer = spawnCooldown;
            }
            // keep rotating mobSprite, and speed will increase as spawnTimer decreases
            if (mobSprite != null) mobSprite.transform.Rotate(0, 0, 360 * Time.deltaTime * (1 - spawnTimer / spawnCooldown) * 1.5f);
            
            // slightly change color of mobSprite from color 1 to color 2, as when the spwanTimer decreases
            
            if (backgroundSprite != null) backgroundSprite.color = Color.Lerp(color1, color2, (1 - spawnTimer / spawnCooldown));
        }
    }

    
    bool ShouldTrySpawnPrefab() {
        
        int existingPrefabs = CountPrefabsInRadius(transform.position, checkPlayerRadius, "Player");
        return existingPrefabs > 0 ;
    }

    int CountPrefabsInRadius(Vector2 center, float radius, string layer) {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        
        int count = 0;
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer(layer)) { // 假设所有的prefabToSpawn都有相同的Tag
                count++;
            }
        }
        return count;
    }
    
    int CountPrefabsInRadiusByTag(Vector2 center, float radius, string tag) {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        
        int count = 0;
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.CompareTag(tag)) { // 假设所有的prefabToSpawn都有相同的Tag
                count++;
            }
        }
        return count;
    }
    
    void TrySpawnPrefab() {
        Vector2 spawnPoint;
        bool spotFound = false;

        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); // Random.insideUnitCircle.normalized;
            spawnPoint = (Vector2)transform.position + randomDirection * spawnDistance;
            
            Vector2 standardSpawnPoint;
            if (standardSpawn)
                standardSpawnPoint = toWorldCoord(toGridCoord(spawnPoint));
            else 
                standardSpawnPoint = spawnPoint;
            
            // Collider2D collider = Physics2D.OverlapCircle(standardSpawnPoint, checkRadius);
            
            if (CountPrefabsInRadiusByTag(transform.position, checkMonsterRadius, "Monster") < 4) {
                spotFound = true;
                SpawnPrefabAtLocation(standardSpawnPoint);
                if (spawnEffect != null) Instantiate(spawnEffect, standardSpawnPoint, Quaternion.identity);
                if (refreshingEffect != null) Instantiate(refreshingEffect, transform.position, Quaternion.identity);
                // takeDamage(this, 1, DamageType.Explosive);
                
                if (deathSFX != null) {
                    AudioManager.playAudio(deathSFX);
                }
                
                break;
            }
        }

        // if (!spotFound) {
        //     Debug.Log("No suitable location found to spawn the prefab.");
        // }
    }

    void SpawnPrefabAtLocation(Vector2 location) {
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, location, Quaternion.identity);
        spawnedPrefab.transform.parent = this.transform.parent;
        if (spawnedPrefab.GetComponent<Tile>() != null) {
            spawnedPrefab.GetComponent<Tile>().init();
        }
    }
    
    
}