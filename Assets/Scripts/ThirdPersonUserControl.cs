using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityEngine.Networking;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    //public class ThirdPersonUserControl : NetworkBehaviour
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public enum JumpState { Run,Prepare,Jump}
        public JumpState current_jump_state;

        public enum PlayerState { Run, Aim }
        public PlayerState current_player_state;

        //delegate void CalculateMovement();
        //CalculateMovement[] calculate_movement=new CalculateMovement[2];
        //public Transform camera_target;



        public void GoJump()
        {
            current_jump_state = JumpState.Jump;
        }
        //public override void OnStartLocalPlayer()
        //{
        //    //Camera.main.GetComponent<CameraMove>().player = this.gameObject.transfor
        //    Camera.main.GetComponent<CameraMove>().player = GetComponentInChildren<CameraTarget>().gameObject.transform;
        //}

        private void Start()
        {
            current_player_state = PlayerState.Run;
            //calculate_movement[(int)PlayerState.Run] = RunMovement;
            //calculate_movement[(int)PlayerState.Aim] = AimMovement;

            current_jump_state = JumpState.Run;


            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (Input.GetMouseButtonDown(1))
            {
                current_player_state = PlayerState.Aim;
                PlayerUI.player_ui.ShowUI();
            }
            else
                if (Input.GetMouseButtonUp(1))
            {
                current_player_state = PlayerState.Run;
                PlayerUI.player_ui.HideUI();
            }
        }

        //void AimMovement()
        //{

        //}

        //void RunMovement()
        //{
        //    // read inputs
        //    float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //    float v = CrossPlatformInputManager.GetAxis("Vertical");
        //    bool crouch = Input.GetKey(KeyCode.C);
        //    // calculate move direction to pass to character
        //    if (m_Cam != null)
        //    {
        //        // calculate camera relative direction to move:
        //        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //        m_Move = v * m_CamForward + h * m_Cam.right;
        //    }
        //    //else
        //    //{
        //    //    // we use world-relative directions in the case of no main camera
        //    //    m_Move = v * Vector3.forward + h * Vector3.right;
        //    //}

        //    // walk speed multiplier
        //    if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;

        //    // pass all parameters to the character control script
        //    switch (current_jump_state)
        //    {

        //        case JumpState.Run:
        //            {
        //                if (m_Jump)
        //                {
        //                    current_jump_state = JumpState.Prepare;
        //                    m_Character.Move(m_Move, crouch, false, true,false);
        //                }
        //                else
        //                    m_Character.Move(m_Move, crouch, false, false, false);
        //            }
        //            break;
        //        case JumpState.Prepare:
        //            {
        //                m_Character.Move(m_Move, false, false, true, false);
        //                //состояние изменяется через animation event
        //            }
        //            break;
        //        case JumpState.Jump:
        //            {
        //                m_Character.Move(m_Move, false, true, false, false);
        //                current_jump_state = JumpState.Run;
        //            }
        //            break;
        //    }
        //}

        // Fixed update is called in sync with physics

        private void FixedUpdate()
        {
            //if (!isLocalPlayer)
                //return;
            bool aim = (current_player_state == PlayerState.Aim) ? true : false; ;

            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            //else
            //{
            //    // we use world-relative directions in the case of no main camera
            //    m_Move = v * Vector3.forward + h * Vector3.right;
            //}

            // walk speed multiplier
            if ((Input.GetKey(KeyCode.LeftShift))||(aim))
                m_Move *= 0.5f;



            // pass all parameters to the character control script
            switch (current_jump_state)
            {

                case JumpState.Run:
                    {
                        if (m_Jump)
                        {
                            current_jump_state = JumpState.Prepare;
                            m_Character.Move(m_Move, crouch, false, true, aim);
                        }
                        else
                            m_Character.Move(m_Move, crouch, false, false, aim);
                    }
                    break;
                case JumpState.Prepare:
                    {
                        m_Character.Move(m_Move, false, false, true, aim);
                        //состояние изменяется через animation event
                    }
                    break;
                case JumpState.Jump:
                    {
                        m_Character.Move(m_Move, false, true, false, aim);
                        current_jump_state = JumpState.Run;
                    }
                    break;
            }
            //calculate_movement[(int)current_player_state]();




            //print(m_Jump);
            //m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
