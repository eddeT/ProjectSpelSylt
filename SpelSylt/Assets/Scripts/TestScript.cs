using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        Instantiate(g);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
