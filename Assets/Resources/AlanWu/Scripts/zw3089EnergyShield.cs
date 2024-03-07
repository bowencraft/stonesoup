using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class zw3089EnergyShield : Tile
{
    public float shieldLastTime = 5f;
    public bool isShieldOn = false;
    public float shieldTimer;
    public GameObject protectorPrefab;
    public Sprite heldSprite;
    public Sprite dropSprite;
    public int currentPHealth;
    public Tile player;

    private GameObject currentShield;
    public override void useAsItem(Tile tileUsingUs)
    {
        if (isShieldOn) return;

        isShieldOn = true;

        if (protectorPrefab != null)
        {
            player = tileUsingUs;
            currentPHealth = tileUsingUs.health;
            currentShield = Instantiate(protectorPrefab, tileUsingUs.transform.position, Quaternion.identity);
            currentShield.transform.SetParent(tileUsingUs.transform); // 将玩家设为护盾的父对象
            currentShield.name = "energyShieldCircle";
            int playerLayer = LayerMask.NameToLayer("Player");
            int shieldLayer = LayerMask.NameToLayer("Shield");
            Physics2D.IgnoreLayerCollision(playerLayer, shieldLayer, true);
        }

        StartCoroutine(ShieldTimer());
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        bool shieldExist = GameObject.Find("energyShieldCircle");
        if(shieldExist) return;
        base.pickUp(tilePickingUsUp);

    }

    public override void dropped(Tile tileDroppingUs)
    {
        if (isShieldOn)
        {
            base.dropped(tileDroppingUs);
            removeTag(TileTags.CanBeHeld);
            _sprite.enabled = false;
        }
        else
        {
            base.dropped(tileDroppingUs);
        }

    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(shieldLastTime);
        isShieldOn = false;
        Destroy(currentShield);
        int playerLayer = LayerMask.NameToLayer("Player");
        int shieldLayer = LayerMask.NameToLayer("Shield");
        Physics2D.IgnoreLayerCollision(playerLayer, shieldLayer, false);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(base.isBeingHeld)
        {
            _sprite.sprite = heldSprite;
        }
        else
        {
            _sprite.sprite = dropSprite;
        }

        if(isShieldOn)
        {
            player.health = currentPHealth;
        }
    }

}
