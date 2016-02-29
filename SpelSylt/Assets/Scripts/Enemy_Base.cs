using UnityEngine;
using System.Collections;

public class Enemy_Base : MonoBehaviour
{

    protected Transform playerTransform;
    protected SpriteRenderer rend;


    //Enemy Stats
    protected int iHealth = 3;

    protected int iDamage = 1;
    public virtual int GetDamage()
    {
        return iDamage;
    }

    //For enemy mode
    protected int Mode = 0;

    protected enum EnemyMode : int { Idle, Attack }
    //Idle 
    protected Vector2 origEnemyPos;

    protected bool bIdleMoveBack = false;

    protected float fIdleXoffset = -2.0f;
    protected float fIdleYoffset = -1.0f;
    protected float fIdleMoveSpeed = .5f;


    //Attack
    protected float fAttackMoveSpeed = 1.0f;
    protected float fAggroTimer = 0.0f;
    protected float fAggroLimit = 0.0f;


    //For destroying 
    protected bool bToBeDestroyed = false;
    public virtual void SetToBeDestroyed(bool pbTBD)
    {
        bToBeDestroyed = pbTBD;
    }

    public virtual void Awake()
    {
        origEnemyPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.tag = "Enemy";
        rend = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (bToBeDestroyed == true)
        {
            DoBeforeDestroy();
            Destroy(gameObject);
        }
        if (Mode == (int)EnemyMode.Attack)
        {
            fAggroTimer += Time.deltaTime;
        }


    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Projectile")
        {
            iHealth -= other.GetComponent<ProjectileScript>().GetProjectileDamge();
            if (iHealth <= 0)
            {
                bToBeDestroyed = true;
            }

        }

    }
    public virtual void DoBeforeDestroy()
    {

    }
    public virtual void OnDestroy()
    {

    }


}
