   using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private ProjectileManager m_ProjectileM;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            m_ProjectileM = GetComponent<ProjectileManager>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
				if (Input.GetKey (KeyCode.Space)) 
				{
					m_Jump = true;
				}
               
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = 0;
            if (Input.GetKey(KeyCode.A))
            {
                h += -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                h += +1;
            }
            crouch = Input.GetKey(KeyCode.S);


                
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);

            m_Jump = false;
        }
    }
}
