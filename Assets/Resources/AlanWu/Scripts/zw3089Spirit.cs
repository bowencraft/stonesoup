using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class zw3089Spirit : BasicAICreature
{

    public Tile player; // ��ҵ�Tile

    public Transform playerT; // ��ҵ�Transform

    protected bool isFollowing = false; // �Ƿ����ڸ������

    public float distanceMax = 2.0f; // ����
    public float distanceMin = 1.2f; // ����

    [SerializeField]
    protected float distance; // ����

    protected int currentHealth;

    protected bool findPlayer = false;

    bool closeEnough = false;

    bool getItem = false;
    public override void tileDetected(Tile otherTile)
    {
        if (otherTile == this)
        {
            return;
        }
        
        if(otherTile.hasTag(TileTags.Player) && findPlayer==false)
        {

            isFollowing = true;
            findPlayer = true;
            player = otherTile;
            playerT = player.transform;
            transform.parent = player.transform;
            currentHealth = player.health;
            distance = Random.Range(distanceMin,distanceMax);
            
        }

        if (isFindingItem && (otherTile.hasTag(TileTags.CanBeHeld) || otherTile.hasTag(TileTags.Weapon)) && (otherTile.hasTag(TileTags.Enemy)==false))
        {
            neededItem.gameObject.GetComponent<Tile>().pickUp(this);
            isFindingItem = false;
            getItem = true;
        }
    }

    protected override void updateSpriteSorting()
    {
        _sprite.sortingOrder = 100;
    }
    protected override void takeStep()
    {
        
        if (isFollowing)
        {
            OrbitAroundPlayer();
        }
    }

    protected virtual void Update()
    {
        takeStep();
        spiritMagic();
    }
    public virtual void OrbitAroundPlayer()
    {
        if (player == null) return;

        //�ҵ����λ��
        Vector3 playerPos = playerT.position;
        //���㵱ǰλ�������λ�ò��
        Vector3 toPlayer = transform.position - playerPos;
        // ��������Ѿ�����Ҹ������Ͳ���Ҫ��һ���ƶ�
        if (toPlayer.magnitude < distance) { 
            if(neededItem!=null && getItem==true)
            {
                neededItem.gameObject.GetComponent<Tile>().dropped(this);
            }
            return; }
        

        // ���㱣��һ�������Ŀ��λ��
        Vector3 targetPosition = playerT.position + toPlayer.normalized * distance;

        _targetGridPos = Tile.toGridCoord(targetPosition.x, targetPosition.y);
        
    }

    private bool isFindingItem = false;
    GameObject neededItem;
    public virtual void spiritMagic()
    {
        if (player == null) return;
        if (player.health < currentHealth)
        {
            player.health ++;
            die();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (!isFindingItem && hit.collider != null)
            {
                Tile hitTile = hit.collider.GetComponent<Tile>();
                if (hitTile != null && (hitTile.hasTag(TileTags.CanBeHeld) || (hitTile.hasTag(TileTags.Weapon))) && (hitTile.hasTag(TileTags.Enemy) == false))
                {
                    isFindingItem = true;
                    neededItem = hit.collider.gameObject;
                }
            }
        }

        if (isFindingItem)
        {
            if (neededItem != null) { carryItem(neededItem); }

            //Vector3 targetposition = Tile.toWorldCoord(_targetGridPos.x, _targetGridPos.y);
            //if (Vector3.Distance(transform.position, targetposition) < 0.01f)
            //{
            //    isFindingItem = false;
            //}
        }
    }

    public virtual void carryItem(GameObject item)
    {
        Vector3 iPos = neededItem.transform.position;

        _targetGridPos = Tile.toGridCoord(iPos.x, iPos.y);
    }

    public override void Start()
    {
        base.Start();
        Random.InitState((int)System.DateTime.Now.Ticks);
    }


}
