using UnityEngine;
using System.Collections.Generic;

public class EnvironmentRandomizer : MonoBehaviour
{
    private GameObject go;
    private List<List<GameObject>> tileList;

    private const float xOffset = -8;
    private const float yOffset = -4;

    private const float xSize = 0.5f;
    private const float ySize = 0.5f;

    private const int xAmount = 25;
    private const int yAmount = 5;

    private BoxCollider2D totalCollider;
    private EdgeCollider2D totalEdgeCollider;

    private List<Vector2> edgeColliderPoints;

	// Use this for initialization
	void Start ()
    {
        go = (GameObject)Resources.Load("SimpleCrate");

        tileList = new List<List<GameObject>>();

        for (int i = 0; i < xAmount; i++)
        {
            tileList.Add(new List<GameObject>());

            for (int j = 0; j < yAmount; j++)
            {
                tileList[i].Add((GameObject)Instantiate(go, new Vector3(i*xSize+xOffset, j*ySize+yOffset, 0), new Quaternion(0,0,0,0)));
            }
        }

        totalEdgeCollider = tileList[0][0].AddComponent<EdgeCollider2D>();

        edgeColliderPoints = new List<Vector2>();

        for (int i = 0; i < xAmount; i++)
        {
            for (int j = 0; j < yAmount; j++)
            {
                edgeColliderPoints.Add(new Vector2(tileList[i][j].transform.position.x - (xSize / 2), tileList[i][j].transform.position.y + (ySize / 2)));
                edgeColliderPoints.Add(new Vector2(tileList[i][j].transform.position.x + (xSize / 2), tileList[i][j].transform.position.y + (ySize / 2)));
                edgeColliderPoints.Add(new Vector2(tileList[i][j].transform.position.x + (xSize / 2), tileList[i][j].transform.position.y - (ySize / 2)));
                edgeColliderPoints.Add(new Vector2(tileList[i][j].transform.position.x - (xSize / 2), tileList[i][j].transform.position.y - (ySize / 2)));
            }
        }


        totalEdgeCollider.points = edgeColliderPoints.ToArray();
        

        /*totalCollider = tileList[0][0].AddComponent<BoxCollider2D>();

        totalCollider.size = new Vector2(xAmount, yAmount);
        totalCollider.offset = new Vector2(xAmount * xSize - xSize, yAmount * ySize - ySize);*/
    }

    // Update is called once per frame
    void Update () {
	
	}
}
