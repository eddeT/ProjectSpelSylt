using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerCharacter2D : MonoBehaviour
{

    private Animator m_Anim;            // Reference to the player's animator component.
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    float acceleration = 2f;
    float maxSpeed = 8f;
    float gravity = 1.5f;
    float maxFall = 20f;

    LayerMask mask;

    Rect box;

    Vector2 velocity;

    bool grounded = false;
    bool falling = false;
    
    int horizontalRays = 12;
    int verticalRays = 10;

    float margin = 0.1f;

    bool lastInput = false;

    float jumpPressedTime;
    float jumpPressLeeway = 0.1f;
    float impulse = 20f;

    float angleLeeway = 10f;

    Collider2D collider2DValue;
    bool flip = false;
    //Health
    int Health = 6;

    bool Dead = false;

    HealthUI healthUIScript;

    void Start()
    {
        mask = 0;
        mask |= (1 << LayerMask.NameToLayer("NormalCollisions"));

        velocity = new Vector2(0, 0);

        healthUIScript.StartHealthUI(Health);
    }

    private void Awake()
    {
        // Setting up references.
        m_Anim = GetComponent<Animator>();

        collider2DValue = GetComponent<Collider2D>();

        healthUIScript = GameObject.Find("HealthUI").GetComponent<HealthUI>();
    }

    void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box.center, box.size);
        }
    }

    private void FixedUpdate()
    {

        UpdateAnimations();

        // Gravity

        box = new Rect(
            collider2DValue.bounds.min.x,
            collider2DValue.bounds.min.y,
            collider2DValue.bounds.size.x,
            collider2DValue.bounds.size.y);

        if (!grounded)
        {
            velocity = new Vector2(velocity.x, Mathf.Max(velocity.y - gravity, -maxFall));
        }

        if (velocity.y < 0)
        {
            falling = true;
        }

        if (grounded || falling)
        {
            Vector2 startPoint = new Vector2(box.xMin, box.center.y);
            Vector2 endPoint = new Vector2(box.xMax, box.center.y);

            RaycastHit2D[] hitInfos = new RaycastHit2D[verticalRays];

            float distance = (box.height / 2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime)));

            float smallestFraction = Mathf.Infinity;
            int indexUsed = 0;

            bool connected = false;

            Debug.DrawLine(startPoint, endPoint, Color.green);

            for (int i = 0; i < verticalRays; i++)
            {
                float lerpAmount = (float)i / (float)(verticalRays - 1);
                Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

                hitInfos[i] = Physics2D.Raycast(origin, Vector2.down, distance, mask);

                Debug.DrawLine(origin, new Vector2(origin.x, origin.y - distance), Color.black, 5f);

                if (hitInfos[i].fraction > 0)
                {
                    connected = true;

                    if (hitInfos[i].fraction < smallestFraction)
                    {
                        indexUsed = i;
                        smallestFraction = hitInfos[i].fraction;
                    }
                }
            }

            if (connected)
            {
                m_Anim.SetBool("Ground", true);
                grounded = true;
                falling = false;
                transform.Translate(Vector3.down * (hitInfos[indexUsed].fraction * distance - box.height / 2));
                velocity = new Vector2(velocity.x, 0);
            }
            else
            {
                grounded = false;
            }
        }

        // Ceiling Check

        if (grounded || velocity.y > 0)
        {
            float upRayLength = grounded ? margin : velocity.y * Time.deltaTime;

            bool connection = false;
            int lastConnection = 0;

            Vector2 startPoint = new Vector2(box.xMin, box.center.y);
            Vector2 endPoint = new Vector2(box.xMax, box.center.y);

            float distance = (box.height / 2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime)));

            RaycastHit2D[] upRays = new RaycastHit2D[verticalRays];

            for (int i = 0; i < verticalRays; i++)
            {
                float lerpAmount = (float)i / (float)(verticalRays - 1);
                Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

                upRays[i] = Physics2D.Raycast(origin, Vector2.up, distance, mask);

                Debug.DrawLine(origin, new Vector2(origin.x, origin.y + distance), Color.magenta, 5f);

                if (upRays[i].fraction > 0)
                {
                    connection = true;
                    lastConnection = i;
                }
            }

            if (connection)
            {
                velocity = new Vector2(velocity.x, 0);
                transform.position += Vector3.up * (upRays[lastConnection].point.y - box.yMax);
            }
        }

        // Movement

        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        float newVelocityX = velocity.x;

        if (horizontalAxis != 0)
        {
            newVelocityX += acceleration * horizontalAxis;
            newVelocityX = Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed);
        }
        else if (velocity.x != 0)
        {
            int modifier = velocity.x > 0 ? -1 : 1;
            newVelocityX += acceleration * modifier;
        }

        velocity = new Vector2(newVelocityX, velocity.y);

        if (velocity.x != 0)
        {
            Vector2 startPoint = new Vector2(box.center.x, box.yMin);
            Vector2 endPoint = new Vector2(box.center.x, box.yMax);

            RaycastHit2D[] hitInfos = new RaycastHit2D[horizontalRays];
            int amountConnected = 0;
            float lastFraction = 0;

            float sideRayLength = box.width / 2 + Mathf.Abs(newVelocityX * Time.deltaTime);
            Vector2 direction = newVelocityX > 0 ? Vector2.right : Vector2.left;

            Debug.DrawLine(startPoint, endPoint, Color.green);

            for (int i = 0; i < horizontalRays; i++)
            {
                float lerpAmount = (float)i / (float)(horizontalRays - 1);
                Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

                Debug.DrawLine(origin, new Vector2(origin.x + (sideRayLength * horizontalAxis), origin.y), Color.yellow, 5f);

                hitInfos[i] = Physics2D.Raycast(origin, direction, sideRayLength, mask);

                if (hitInfos[i].fraction > 0)
                {
                    float hitDistance = Vector2.Distance(origin, hitInfos[i].point);

                    hitDistance -= 0.01f;

                    if (lastFraction > 0)
                    {
                        float angle = Vector2.Angle(hitInfos[i].point - hitInfos[i - 1].point, Vector2.right);

                        if (Mathf.Abs(angle - 90) < angleLeeway)
                        {
                            transform.Translate(direction * (hitDistance - box.width / 2));
                            velocity = new Vector2(0, velocity.y);
                            break;
                        }
                    }
                }

                amountConnected++;
                lastFraction = hitInfos[i].fraction;
            }
        }

        // Jumping

        bool input = Input.GetButton("Jump");

        if (input && !lastInput)
        {
            jumpPressedTime = Time.time;
        }
        else if (!input)
        {
            jumpPressedTime = 0;
        }

        if (grounded && Time.time - jumpPressedTime < jumpPressLeeway)
        {
            velocity = new Vector2(velocity.x, impulse);
            jumpPressedTime = 0;
            grounded = false;
        }

        lastInput = input;
    }

    private void LateUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Flip()
    {
        // Switch the way the player is labeled as facing.
        m_FacingRight = !m_FacingRight;
        flip = !flip;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public bool GetFlip()
    {
        return flip;
    }

    private void UpdateAnimations()
    {
        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", velocity.y);
        m_Anim.SetFloat("Speed", Mathf.Abs(velocity.x));

        if ((velocity.x > 0 && !m_FacingRight) || (velocity.x < 0 && m_FacingRight))
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            Enemy_Base enemyB = other.GetComponent<Enemy_Base>();
            UpdateHealth(-enemyB.GetDamage());
            enemyB.SetToBeDestroyed(true);
        }

    }

    private void UpdateHealth(int HealthChange)
    {
        healthUIScript.HealthUIUpdate(HealthChange);
        Health += HealthChange;

        if (Health <= 0)
        {
            Dead = true;
            SceneManager.LoadScene(0);
        }
    }

}
