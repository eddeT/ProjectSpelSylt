using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Use this for initialization
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    
    void FixedUpdate()
    {
        if(particleSystem.isPlaying == true)
        {
            return;
        }
        Destroy(gameObject);
    }
}