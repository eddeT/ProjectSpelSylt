using UnityEngine;
using System.Collections.Generic;

public class EnvironmentRandomizer : MonoBehaviour
{
    private GameObject go;

    private List<GameObject> tileTypes;

    private List<List<GameObject>> tileList;

    private List<List<int>> mapList;

    private const float xOffset = -8;
    private const float yOffset = -4;

    private float xSize;
    private float ySize;

    private const int xAmount = 40;
    private const int yAmount = 25;

    private BoxCollider2D totalCollider;

    private SpriteRenderer renderer;
    
	// Use this for initialization
	void Start ()
    {
        mapList = new List<List<int>>();

        for (int x = 0; x < xAmount; x++)
        {
            mapList.Add(new List<int>());

            for (int y = 0; y < yAmount; y++)
            {
                mapList[x].Add(0);
            }
        }
        
        tileTypes = new List<GameObject>();

        go = (GameObject)Resources.Load("Rock");

        tileTypes.Add((GameObject)Resources.Load("Rock"));
        tileTypes.Add((GameObject)Resources.Load("RockYellow"));
        tileTypes.Add((GameObject)Resources.Load("RockRed"));
        tileTypes.Add((GameObject)Resources.Load("RockPurple"));
        tileTypes.Add((GameObject)Resources.Load("RockCyan"));
        tileTypes.Add((GameObject)Resources.Load("RockGreen"));
        tileTypes.Add((GameObject)Resources.Load("RockBlue"));

        tileList = new List<List<GameObject>>();

        renderer = go.GetComponent<SpriteRenderer>();

        xSize = renderer.bounds.size.x;
        ySize = renderer.bounds.size.y;

        for (int x = 0; x < xAmount; x++)
        {
            tileList.Add(new List<GameObject>());

            for (int y = 0; y < yAmount; y++)
            {
                int temp = Random.Range((int)0, (int)2);
                if (temp == 0)
                {
                    continue;
                }
                mapList[x][y] = temp;
                tileList[x].Add((GameObject)Instantiate(tileTypes[Random.Range(0, tileTypes.Count)], new Vector3(x * xSize + xOffset, y * ySize + yOffset, 0), new Quaternion(0, 0, 0, 0)));
            }
        }

        /*totalCollider = tileList[0][0].AddComponent<BoxCollider2D>();

        totalCollider.size = new Vector2(xAmount, yAmount);
        totalCollider.offset = new Vector2(xAmount * xSize - xSize, yAmount * ySize - ySize);*/
    }

    // Update is called once per frame
    void Update () {
	
	}
}
