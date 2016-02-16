using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ProjectileScript : MonoBehaviour
{
    private float fVelocity = 1.0f;
    private float fOrigSpeed = 1.0f;
    private Direction dDirection;
    private float fDir = 1;

    private float fPlayerVelocityDecay = 0.1f;

    private Vector2 v2Movement;
    private Vector2 v2PlayerVelocity;

    private Vector2 v2OrigPlayerVelocity;

    private Vector2 v2FireFront;
    private Vector2 v2FireBack;

    private static float DIAGONAL_VELOCITY_DECAY_FACTOR = 0.66f;


    private float fprojectileDuration = 2.0f;
    private float fprojectileDurationTimer = 0.0f;

    public void SetProjectile(Direction pDirection, float pfVelocity, Vector2 pv2PlayerVelocity)
    {
        fOrigSpeed = pfVelocity;
        v2PlayerVelocity = pv2PlayerVelocity;
        v2OrigPlayerVelocity = v2PlayerVelocity;
        dDirection = pDirection;
    }
    public void SetProjectile(Direction pDirection)
    {
        dDirection = pDirection;
    }

    void Awake()
    {
        v2FireFront = GameObject.Find("ProjectileFireFront").transform.position;
        v2FireBack = GameObject.Find("ProjectileFireBack").transform.position;
        transform.position = v2FireFront;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float normalVelocity = 0;
        v2Movement = transform.position;
        fVelocity = fOrigSpeed;

        //Calculate Normal velocity
        normalVelocity = (fVelocity * fDir);

        //Calculate Relative velocity and Decay
        var playerVelocityX = 0.0f;
        var playerVelocityY = 0.0f;

        if (v2OrigPlayerVelocity.x > 0)
        {
            if (v2PlayerVelocity.x > 0)
            {
                playerVelocityX = v2PlayerVelocity.x;

                v2PlayerVelocity.x -= fPlayerVelocityDecay;
            }
        }
        else if (v2OrigPlayerVelocity.x < 0)
        {
            if (v2PlayerVelocity.x < 0)
            {
                playerVelocityX = v2PlayerVelocity.x;

                v2PlayerVelocity.x += fPlayerVelocityDecay;
            }
        }
        if (v2OrigPlayerVelocity.y > 0)
        {
            if (v2PlayerVelocity.y > 0)
            {
                playerVelocityY = v2PlayerVelocity.y;

                v2PlayerVelocity.y -= fPlayerVelocityDecay;
            }

        }
        if (v2OrigPlayerVelocity.y < 0)
        {
            if (v2PlayerVelocity.y < 0)
            {
                playerVelocityY = v2PlayerVelocity.y;

                v2PlayerVelocity.y += fPlayerVelocityDecay;
            }

        }




        if (dDirection == Direction.Up && v2PlayerVelocity.y < 0)
        {
            playerVelocityY = 0.0f;
        }
        if (dDirection == Direction.Down && v2PlayerVelocity.y > 0)
        {
            playerVelocityY = 0.0f;
        }

        //If direction is diagonal
        if ((int)dDirection > 3)
        {

            normalVelocity *= DIAGONAL_VELOCITY_DECAY_FACTOR;
            playerVelocityX *= DIAGONAL_VELOCITY_DECAY_FACTOR;
            playerVelocityY *= DIAGONAL_VELOCITY_DECAY_FACTOR;
        }


        normalVelocity *= Time.deltaTime;
        playerVelocityX *= Time.deltaTime;
        playerVelocityY *= Time.deltaTime;

        if (dDirection == Direction.Up || dDirection == Direction.UpLeft || dDirection == Direction.UpRight)
        {
            v2Movement.y += (normalVelocity + playerVelocityY);
        }
        if (dDirection == Direction.Down || dDirection == Direction.DownLeft || dDirection == Direction.DownRight)
        {
            v2Movement.y += -(normalVelocity - (playerVelocityY));
        }
        if (dDirection == Direction.Right || dDirection == Direction.UpRight || dDirection == Direction.DownRight)
        {
            v2Movement.x += (normalVelocity + playerVelocityX);
        }
        if (dDirection == Direction.Left || dDirection == Direction.UpLeft || dDirection == Direction.DownLeft)
        {
            v2Movement.x += -(normalVelocity - (playerVelocityX));
        }


        transform.position = v2Movement;


        UpdateProjectileTimer();
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag != "Player" && other.transform.tag != "Projectile")
        {
            if (other.transform.tag == "Pink_Enemy")
            {
                Destroy(other.gameObject);
            }

            Destroy(gameObject);
        }

    }


    void UpdateProjectileTimer()
    {
        fprojectileDurationTimer += Time.deltaTime;
        if (fprojectileDurationTimer >= fprojectileDuration)
        {
            Destroy(gameObject);
        }
    }
}
