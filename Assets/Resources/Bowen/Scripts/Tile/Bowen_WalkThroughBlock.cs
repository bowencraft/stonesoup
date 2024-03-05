using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowen_WalkThroughBlock : IndestructibleWall
{
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
