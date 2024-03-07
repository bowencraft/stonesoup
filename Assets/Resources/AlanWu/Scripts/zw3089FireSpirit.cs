using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zw3089FireSpirit : zw3089Spirit
{
    public GameObject fireBallPrefab;
    bool isAttacking = false;
    GameObject enemy;
    

    public override void spiritMagic()
    {
        if(isFollowing)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (!isAttacking && hit.collider!=null)
                {
                    Tile hitTile = hit.collider.GetComponent<Tile>();
                    if(hitTile!=null && hitTile.hasTag(TileTags.Enemy))
                    {
                        isAttacking = true;
                        enemy = hit.collider.gameObject;
                    }
                }
            }

            if (isAttacking)
            {
                if (enemy != null) { Attack(enemy); }
                
                Vector3 targetposition = Tile.toWorldCoord(_targetGridPos.x, _targetGridPos.y);
                if (Vector3.Distance(transform.position, targetposition) < 0.1f)
                {
                    isAttacking = false;
                }
            }

            
        }

    }

    public override void OrbitAroundPlayer()
    {
        if(isAttacking) { return; }
        base.OrbitAroundPlayer();
    }

    public override void tileDetected(Tile otherTile)
    {
        base.tileDetected(otherTile);
        if (otherTile.hasTag(TileTags.Enemy) && isFollowing)
        {
            otherTile.takeDamage(this,1,DamageType.Normal);
        }
    }

    public virtual void Attack(GameObject objectBeingAttacked)
    {
        Vector3 enemyPos = objectBeingAttacked.transform.position;

        _targetGridPos = Tile.toGridCoord(enemyPos.x, enemyPos.y);
    }
    

}
