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
    //float uiFactor = 50f;
    //float uiOffsetY = 32f / 100f;//32 pixlar 
    //float uiOffsetX = 32f / 100f;
    void Start ()
    {
        this.transform.localPosition = new Vector2(0f, 0f);

        float sWidth = Screen.width;
        float sHeight = Screen.height;


        float aspect = sWidth / sHeight;
        var uiWidth = sWidth/100f;
        var uiHeight = sHeight/100f;
        print("Dela 100");
        print(uiWidth);
        print(uiHeight);
        print(sWidth / sHeight);

        uiHeight /= 2.0f;
        uiWidth /= 2.0f;

        //this.transform.localPosition = new Vector2(-uiWidth + uiOffsetX, uiHeight - uiOffsetY);
        this.transform.localPosition = new Vector2(-8.50f, 4.50f);

    }
	
	void Update ()
    {
	
	}
}
