using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;

    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode jumpKey;

    [SerializeField]
    private float charSpeed = 10f;
    private float currentSpeed;

    
    

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);

        AssignControls();
        currentSpeed = charSpeed;
        
    }




    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = Input.GetKeyDown(jumpKey);
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.

        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        if (Input.GetKey(rightKey))
        {
            h = currentSpeed;
        }
        else if (Input.GetKey(leftKey))
        {
            h = -currentSpeed;
        }
        else h = 0;

        // Pass all parameters to the character control script.
      

        m_Character.Move(h, m_Jump);
        
        m_Jump = false;
    }

    public IEnumerator SlowDown(float howLong)
    {
        
        currentSpeed = charSpeed / 2;

        yield return new WaitForSeconds(howLong);

        currentSpeed = charSpeed;
    }


    void AssignControls()
    {
        if (gameObject.name == "Player1")
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey = KeyCode.W;
        }
        if (gameObject.name == "Player2")
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.UpArrow;
        }
    }

}

