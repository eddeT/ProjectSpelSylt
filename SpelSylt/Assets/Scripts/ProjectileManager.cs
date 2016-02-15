using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ProjectileManager : MonoBehaviour
{
    public GameObject ProjectileSimple;
    public Rigidbody2D PlayerBody;

    float shootCooldown = 0.4f;
    float shootTimer = 0.0f;
    
    void Awake()
    {
        PlayerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }


    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            GameObject.Instantiate(ProjectileSimple);


            print("space key was pressed");
        }
           
    }

    void FixedUpdate()
    {
        shootTimer += Time.deltaTime;
    }

    public void Shoot(int pDir)
    {
        if(pDir == -1)
        {
            return;
        }
        if(shootTimer < shootCooldown)
        {
            return;
        }
        shootTimer = 0.0f;

        GameObject newProjectile = GameObject.Instantiate(ProjectileSimple);
        ProjectileScript ProScript = newProjectile.GetComponent<ProjectileScript>();
        ProScript.SetProjectile((Direction)pDir, 4.5f, PlayerBody.velocity);



    }
}
