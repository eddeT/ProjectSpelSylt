   using UnityEngine;
using Assets.Scripts;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private ProjectileManager m_ProjectileM;
        private bool m_Jump;
        bool crouch;

        float h = 0;
        int d = -1;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            m_ProjectileM = GetComponent<ProjectileManager>();
        }


        private void Update()
        {
            h = 0;
            d = -1;
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
				if (Input.GetKey (KeyCode.Space)) 
				{
					m_Jump = true;
				}
               
            }

            if (Input.GetKey(KeyCode.A))
            {
                h += -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                h += +1;
            }
            crouch = Input.GetKey(KeyCode.S);



            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                d = (int)Direction.UpLeft;
            }
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                d = (int)Direction.UpRight;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                d = (int)Direction.DownLeft;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                d = (int)Direction.DownRight;
            }



            else if (Input.GetKey(KeyCode.UpArrow))
            {
                d = (int)Direction.Up;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                d = (int)Direction.Down;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                d = (int)Direction.Left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                d = (int)Direction.Right;
            }
        }

        
        private void FixedUpdate()
        {
            // Read the inputs.
            

            

            // Pass all parameters to the character control script.
            //m_Character.Move(h, crouch, m_Jump);
            //print(h);
            m_ProjectileM.Shoot(d);
            m_Jump = false;
        }
    }
}
