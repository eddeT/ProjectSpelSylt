using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ProjectileManager : MonoBehaviour
{
    public GameObject ProjectileSimple;
    public Rigidbody2D PlayerBody;

    float shootCooldown = 0.4f;
    float shootTimer = 0.0f;
    Transform v2FireFront;
    Transform v2FireBack;
    PlatformerCharacter2D CharScript;
        
void Awake()
    {
        PlayerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        CharScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        v2FireFront = GameObject.Find("ProjectileFireFront").transform;
        v2FireBack = GameObject.Find("ProjectileFireBack").transform;
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
        newProjectile.transform.position = v2FireFront.position;

        if(CharScript.GetFlip() == false)
        {
            if (pDir == (int)Direction.Left || pDir == (int)Direction.DownLeft || pDir == (int)Direction.UpLeft)
            {
                newProjectile.transform.position = v2FireBack.position;
            }
        }
        else
        {
            if (pDir == (int)Direction.Right || pDir == (int)Direction.DownRight || pDir == (int)Direction.UpRight)
            {
                newProjectile.transform.position = v2FireBack.position;
            }
        }
        

    }
}
