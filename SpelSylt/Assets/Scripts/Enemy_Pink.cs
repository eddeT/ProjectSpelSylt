using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Enemy_Pink : Enemy_Base
{
    public GameObject particles;


    public override void Awake()
    {
        print("Is awake happening");
        base.Awake();
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if ((Mathf.Abs(playerTransform.position.x) - 20) <= Mathf.Abs(transform.position.x))
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 1.0f*Time.deltaTime);
                
               // transform.Translate((-playerTransform.position.x * Time.deltaTime), playerTransform.position.y * Time.deltaTime, 0);
        }

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (iHealth == 2)
        {
            rend.color = new Color(0.5f, 0.4f, 0.3f);
        }
        else if (iHealth == 1)
        {
            rend.color = new Color(0.3f, 0.2f, 0.4f);
        }
    }

    public override void DoBeforeDestroy()
    {
        base.DoBeforeDestroy();
        GameObject particleObject = Instantiate(particles);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();



        particleObject.transform.position = this.transform.GetChild(0).transform.position;
        Vector3 boxSize = GetComponent<BoxCollider2D>().size;
        particleObject.SetActive(true);
        particleSystem.Play();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();       

    }
}
