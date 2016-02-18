using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour {

    public GameObject HealthBalls;
    private List<GameObject> BallList = new List<GameObject>();
    public void StartHealthUI(int pHealth)
    {

        for(int i = 0; i < pHealth; i++)
        {
            BallList.Add(createHealthBall(new Vector2(transform.position.x + 0.64f * i, transform.position.y)));
        }
    }

    public void HealthUIUpdate(int pHealthChange)
    {
        if(pHealthChange < 0)
        {
            pHealthChange = Mathf.Abs(pHealthChange);
            for (int i = 0; i < pHealthChange; i++)
            {
                //BallList[BallList.Count - pHealthChange].SetActive(false);
                Destroy(BallList[BallList.Count - 1]);
                BallList.RemoveAt(BallList.Count - 1);
            }
        }
        else
        {
            for(int i = 0; i < pHealthChange; i++)
            {
                
                BallList.Add(createHealthBall(new Vector2(BallList[BallList.Count - 1].transform.position.x + 0.64f, transform.position.y)));
            }
        }
        
    }

    private GameObject createHealthBall(Vector2 pos)
    {
        GameObject healthBall;
        healthBall = (GameObject)Instantiate(HealthBalls, pos, Quaternion.identity);
        healthBall.transform.parent = gameObject.transform;
        healthBall.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        return healthBall;
    }
    void Awake()
    {
        
    }

    void Start ()
    {
        this.transform.localPosition = new Vector2(-4.17f, 4.52f);
	}
	
	void Update ()
    {
	
	}
}
