using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class zw3089Spirit : BasicAICreature
{

    public Tile player; // 玩家的Tile

    public Transform playerT; // 玩家的Transform

    protected bool isFollowing = false; // 是否正在跟随玩家

    public float distanceMax = 2.0f; // 距离
    public float distanceMin = 1.2f; // 距离

    [SerializeField]
    protected float distance; // 距离

    protected int currentHealth;

    protected bool findPlayer = false;
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

        //找到玩家位置
        Vector3 playerPos = playerT.position;
        //计算当前位置与玩家位置差距
        Vector3 toPlayer = transform.position - playerPos;
        // 如果物体已经在玩家附近，就不需要进一步移动
        if (toPlayer.magnitude < distance) return;

        // 计算保持一定距离的目标位置
        Vector3 targetPosition = playerT.position + toPlayer.normalized * distance;

        _targetGridPos = Tile.toGridCoord(targetPosition.x, targetPosition.y);
        
    }

    public virtual void spiritMagic()
    {
        if (player == null) return;
        if (player.health < currentHealth)
        {
            player.health ++;
            die();
        }
    }

    public override void Start()
    {
        base.Start();
        Random.InitState((int)System.DateTime.Now.Ticks);
    }


}
