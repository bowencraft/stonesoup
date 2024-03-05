using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_Greenland : Bowen_MobSpawner
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入的Collider2D是否有Player组件
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null && player.health < 3)
        {
            player.health += 1;
            takeDamage(this, 100, DamageType.Explosive);
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
