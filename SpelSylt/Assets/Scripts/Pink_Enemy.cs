using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Pink_Enemy : MonoBehaviour
{

    Transform playerTransform;
    public GameObject particles;
    private Renderer rend;

    private bool gameIsShuttingDown = false;

    private int iHealth = 3;
    private int iDamage = 1;
    public int GetDamage()
    {
        return iDamage;
    }

    //
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.tag = "Enemy";
    }


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Mathf.Abs(playerTransform.position.x) - 20) <= Mathf.Abs(transform.position.x))
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 1.0f*Time.deltaTime);
                
               // transform.Translate((-playerTransform.position.x * Time.deltaTime), playerTransform.position.y * Time.deltaTime, 0);
        }

    }

    void OnApplicationQuit()
    {
        gameIsShuttingDown = true;
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Projectile")
        {
            iHealth -= other.GetComponent<ProjectileScript>().GetProjectileDamge();
            if(iHealth == 2)
            {
                GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.4f, 0.3f, 1f);
            }
            else if(iHealth == 1)
            {
                GetComponent<SpriteRenderer>().color = new Color(0.3f,0.2f,0.4f, 1f);
            }
            if(iHealth <= 0)
            {
                Destroy(gameObject);
            }
            
        }

    }

    void OnDestroy()
    {

        if(gameObject && gameIsShuttingDown == false)
        {
            GameObject particleObject = Instantiate(particles);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();



            particleObject.transform.position = this.transform.GetChild(0).transform.position;
            Vector3 boxSize = GetComponent<BoxCollider2D>().size;
            particleObject.SetActive(true);
            particleSystem.Play();
        }
       

    }
}
