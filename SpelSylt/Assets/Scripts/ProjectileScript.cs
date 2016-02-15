using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight}

    private float fSpeed;
    private Direction dDirection;
    private float fDir;
    private Vector2 v2Movement;

    public void SetProjectile(float pfSpeed, Direction pDirection)
    {
        fSpeed = pfSpeed;
        dDirection = pDirection;
    }

    void Start()
    {
//transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        fSpeed = 1.0f;
        fDir = 1.0f;
        dDirection = (Direction)Random.Range(0,7);
    }

    // Update is called once per frame
    void Update()
    {
        float fCalculateVel = 0;
        v2Movement = transform.position;
        fCalculateVel = fSpeed * fDir * Time.deltaTime;

        if (dDirection == Direction.Up || dDirection == Direction.UpLeft || dDirection == Direction.UpRight)
        {
            v2Movement.y += fCalculateVel;
        }
        if (dDirection == Direction.Down || dDirection == Direction.DownLeft || dDirection == Direction.DownRight)
        {
            v2Movement.y -= fCalculateVel;
        }
        if (dDirection == Direction.Right || dDirection == Direction.UpRight || dDirection == Direction.DownRight)
        {
            v2Movement.x += fCalculateVel;
        }
        if (dDirection == Direction.Left || dDirection == Direction.UpLeft || dDirection == Direction.DownLeft)
        {
            v2Movement.x -= fCalculateVel;
        }


        transform.position = v2Movement;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag != "Player" && other.transform.tag != "Projectile")
        {
            if (other.transform.tag == "Enemy")
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }

    }
}
