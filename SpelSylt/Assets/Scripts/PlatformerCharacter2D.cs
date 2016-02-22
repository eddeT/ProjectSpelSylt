using System;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
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

        int horizontalRays = 6;
        int verticalRays = 4;
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
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

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
            // Mouse Raycasting



            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

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

                    Debug.DrawLine(origin, new Vector2(origin.x, origin.y - distance), Color.black, 1f);

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
                    m_Grounded = true;
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

            bool canJump = true;

            if (grounded || velocity.y > 0)
            {
                float upRayLength = grounded ? margin : velocity.y * Time.deltaTime;

                bool connection = false;
                int lastConnection = 0;

                Vector2 min = new Vector2(box.xMin, box.center.y);
                Vector2 max = new Vector2(box.xMax, box.center.y);

                RaycastHit2D[] upRays = new RaycastHit2D[verticalRays];

                for (int i = 0; i < verticalRays; i++)
                {
                    Vector2 start = Vector2.Lerp(min, max, (float)i / (float)verticalRays);
                    Vector2 end = start + Vector2.up * (upRayLength + box.height / 2);

                    upRays[i] = Physics2D.Linecast(start, end, mask);

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
                bool connected = false;

                Debug.DrawLine(startPoint, endPoint, Color.green);

                for (int i = 0; i < horizontalRays; i++)
                {
                    float lerpAmount = (float)i / (float)(horizontalRays - 1);
                    Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

                    Debug.DrawLine(origin, new Vector2(origin.x + (sideRayLength * horizontalAxis), origin.y), Color.yellow, 1f);

                    hitInfos[i] = Physics2D.Raycast(origin, direction, sideRayLength, mask);

                    if (hitInfos[i].fraction > 0)
                    {
                        float hitDistance = Vector2.Distance(origin, hitInfos[i].point);

                        hitDistance -= 0.01f;
                        
                        connected = true;

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
            Move(horizontalAxis, false, input);
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


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                //m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
           /* if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            */
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


      
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.tag == "Pink_Enemy")
            {
                UpdateHealth(-1);
                Destroy(other.gameObject);
            }

        }

        private void UpdateHealth(int HealthChange)
        {
            healthUIScript.HealthUIUpdate(HealthChange);
            Health += HealthChange;
            
            if(Health <= 0)
            {
                Dead = true;
                SceneManager.LoadScene(0);
            }
        }
    
}
