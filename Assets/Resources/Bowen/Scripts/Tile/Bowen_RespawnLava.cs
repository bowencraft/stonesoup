using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_RespawnLava : Bowen_MobSpawner
{
    public int damageAmount = 1;
    public float damageForce = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        Tile otherTile = other.gameObject.GetComponent<Tile>();
        if (otherTile != null)
        {
            if (damageAmount > 0) otherTile.takeDamage(this, damageAmount);
            Vector2 toOtherTile = (Vector2)otherTile.transform.position - (Vector2)transform.position;
            toOtherTile.Normalize();
            otherTile.addForce(damageForce * toOtherTile);
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
