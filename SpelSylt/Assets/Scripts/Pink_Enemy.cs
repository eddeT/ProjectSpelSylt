using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Pink_Enemy : MonoBehaviour
{

    Transform playerTransform;
    public GameObject particles;
    private Renderer rend;

    //
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.tag = "Pink_Enemy";
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

    void OnDestroy()
    {
        GameObject particleObject = Instantiate(particles);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();



        particleObject.transform.position = this.transform.GetChild(0).transform.position;
        Vector3 boxSize = GetComponent<BoxCollider2D>().size;
        particleObject.SetActive(true);
        particleSystem.Play();

    }
}
