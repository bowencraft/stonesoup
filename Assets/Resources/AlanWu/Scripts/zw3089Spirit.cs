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

        //�ҵ����λ��
        Vector3 playerPos = playerT.position;
        //���㵱ǰλ�������λ�ò��
        Vector3 toPlayer = transform.position - playerPos;
        // ��������Ѿ�����Ҹ������Ͳ���Ҫ��һ���ƶ�
        if (toPlayer.magnitude < distance) return;

        // ���㱣��һ�������Ŀ��λ��
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
