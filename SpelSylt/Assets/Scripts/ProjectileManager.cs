using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour
{
    public GameObject ProjectileSimple;
    
    
    
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
}
