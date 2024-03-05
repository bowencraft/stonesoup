using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_pickaxe : apt283BetterSword
{
    void OnTriggerEnter2D(Collider2D collider) {
        HandleCollision(collider);
    }

    void OnTriggerStay2D(Collider2D collider) {
        HandleCollision(collider);
    }

    void HandleCollision(Collider2D collider) {
        if (_swinging) {
            Tile otherTile = collider.GetComponent<Tile>();
            if (otherTile != null && !otherTile.isBeingHeld) {
                if (otherTile is Wall) {
                    
                    otherTile.takeDamage(this, 1, DamageType.Explosive);
                    
                    takeDamage(this, 1, DamageType.Explosive);
                    // Debug.Log("Added damage to wall");
                } else {
                    // Debug.Log("not wall");
                    Rigidbody2D otherBody = collider.GetComponent<Rigidbody2D>();
                    if (!collider.isTrigger && otherBody != null && otherTile != _tileHoldingUs) {
                        otherBody.AddForce((otherTile.transform.position - _tileHoldingUs.transform.position).normalized * 1000f * otherBody.mass);
                        if (!_attackedDuringSwing.Contains(otherTile)) {
                            otherTile.takeDamage(this, 1); 
                            
                            _attackedDuringSwing.Add(otherTile);
                            takeSwingDamage();
                        }
                    }
                }
            }
        }
    }
    
    
    void Update() {
        if (_tileHoldingUs != null) {
            tileName = string.Format("Golden Pickaxe (HP: {0})", health);
        }
    }

}
