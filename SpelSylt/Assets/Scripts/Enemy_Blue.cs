using UnityEngine;
using System.Collections;

public class Enemy_Blue : Enemy_Base
{

    public GameObject particles;
    

    public override void Awake()
    {
        base.Awake();

        rend.color = new Color(0.1f, 0.1f, 0.9f);


        fAggroLimit = 3f;
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (playerTransform.position.x >= transform.position.x)
        {
            Mode = (int)EnemyMode.Attack;
        }
        if (playerTransform.position.x <= transform.position.x && fAggroTimer >= fAggroLimit)
        {
            fAggroTimer = 0;
            Mode = (int)EnemyMode.Idle;
        }

        if (Mode == (int)EnemyMode.Idle)
        {
            if (!bIdleMoveBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(origEnemyPos.x + fIdleXoffset, origEnemyPos.y + fIdleYoffset), .5f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(origEnemyPos.x, origEnemyPos.y), fIdleMoveSpeed * Time.deltaTime);
            }
            if (transform.position.x <= origEnemyPos.x + fIdleXoffset || transform.position.x == origEnemyPos.x)
            {
                bIdleMoveBack = !bIdleMoveBack;
            }
        }
        if(Mode == (int)EnemyMode.Attack)
        {
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, fAttackMoveSpeed * Time.deltaTime);
            origEnemyPos = transform.position;
        }
       

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (iHealth == 2)
        {
            rend.color = new Color(0.1f, 0.1f, 0.4f);
            fAttackMoveSpeed = 2.5f;
            
        }
        else if (iHealth == 1)
        {
            rend.color = new Color(0.1f, 0.1f, 0.2f);
            fAttackMoveSpeed = 5f;
        }
    }

    public override void DoBeforeDestroy()
    {
        base.DoBeforeDestroy();
        GameObject particleObject = Instantiate(particles);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();



        particleObject.transform.position = this.transform.GetChild(0).transform.position;
        Vector3 boxSize = new Vector2(0.5f, 0.5f);
        particleSystem.startColor = new Color(.1f, .1f, .8f);

        particleObject.SetActive(true);
        particleSystem.Play();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();



    }
}
