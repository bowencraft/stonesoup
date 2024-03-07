using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zw3089Spike : Wall
{
    public Tile toucher; // Íæ¼ÒµÄTile
    public bool touched = false;
    protected override void updateSpriteSorting()
    {
        base.updateSpriteSorting();
        _sprite.sortingLayerID = SortingLayer.NameToID("Floor");
    }

    public override void tileDetected(Tile otherTile)
    {
        if (otherTile == this)
        {
            return;
        }

        else if (touched == false)
        {
            touched = true;
            toucher = otherTile;
            if (toucher.hasTag(TileTags.Player))
            {
                if (GameObject.Find("energyShieldCircle") == null)
                {
                    toucher.takeDamage(this, 1, DamageType.Normal);
                }
                
            }
            else
            {
                toucher.takeDamage(this, 1, DamageType.Normal);
            }
        }
    }

    public override void tileNoLongerDetected(Tile detectedTile)
    {
        if (detectedTile == this)
        {
            return;
        }

        else if (touched == true)
        {
            touched = false;
        }
    }
}
