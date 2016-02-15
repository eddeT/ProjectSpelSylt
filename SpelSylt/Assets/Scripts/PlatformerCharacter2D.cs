using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
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

        float acceleration = 4f;
        float maxSpeed = 150f;
        float gravity = 0.2f;
        float maxFall = 2f;
        float jump = 200f;

        LayerMask mask;

        Rect box;

        Vector2 velocity;

        bool grounded = false;
        bool falling = false;

        int horizontalRays = 6;
        int verticalRays = 4;
        float margin = 1;
        float boundsMargin = 2f;

        Collider2D collider2DValue;

        void Start()
        {
            mask = LayerMask.NameToLayer("NormalCollisions");

            velocity = new Vector2(0, 0);
        }
        
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            collider2DValue = GetComponent<Collider2D>();
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
            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

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
                
                print("distance should be " + (box.height / 2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime))));

                float distance = (box.height / 2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime)));

                print("distance is " + distance);

                bool connected = false;

                Debug.DrawLine(startPoint, endPoint, Color.green);

                for (int i = 0; i < verticalRays; i++)
                {
                    float lerpAmount = (float)i / (float)(verticalRays - 1);
                    Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
                    Ray2D ray = new Ray2D(origin, Vector2.down);

                    RaycastHit2D hitInfo = Physics2D.Raycast(origin, Vector2.down, distance);

                    Debug.DrawLine(origin, new Vector2(origin.x, origin.y - distance), Color.blue, 1f);

                    if (hitInfo.collider != null)
                    {
                        print("name of target = " + hitInfo.collider.name);

                        print("box height = " + box.height.ToString());

                        float hitDistance = Vector2.Distance(origin, hitInfo.point);

                        print("hitDistance = " + hitDistance);

                        connected = true;
                        grounded = true;
                        m_Grounded = true;
                        falling = false;
                        transform.Translate(Vector2.down * (hitDistance - (box.height / 2)));
                        velocity = new Vector2(velocity.x, 0);
                        break;
                    }
                }

                if (!connected)
                {
                    grounded = false;
                }
            }
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
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

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
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labeled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
