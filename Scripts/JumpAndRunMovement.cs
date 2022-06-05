using UnityEngine;
using System.Collections;

public class JumpAndRunMovement : MonoBehaviour 
{
    public float Speed;
    public float JumpForce;
    public LayerMask jumpLayerMask;

    Animator m_Animator;
    Rigidbody2D m_Body;
    PhotonView m_PhotonView;

    bool m_IsGrounded;
    bool isStunned;
    public bool facingRight;

    void Awake() 
    {
        m_Animator = GetComponent<Animator>();
        m_Body = GetComponent<Rigidbody2D>();
        m_PhotonView = GetComponent<PhotonView>();
        isStunned = false;
        facingRight = true;
    }

    void Update() 
    {
        UpdateIsGrounded();
        UpdateIsRunning();
        UpdateFacingDirection();
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<HealthScript>().isStunned)
        {
            return;
        }
        if ( m_PhotonView.isMine == false )
                   {
                       return;
                   }
            
                 UpdateMovement();
                 UpdateJumping();
        
       
    }

    void UpdateFacingDirection()
    {
        //if( m_Body.velocity.x > 0.2f )

        if (m_PhotonView.isMine)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.2f)
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingRight = true;
                m_PhotonView.RPC("FuckingFlip", PhotonTargets.AllViaServer, facingRight);
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.2f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingRight = false;
                m_PhotonView.RPC("FuckingFlip", PhotonTargets.AllViaServer, facingRight);
            }
        }
        else
            if (facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


    }

    [PunRPC]
    void FuckingFlip(bool fuckingRight)
    {
        facingRight = fuckingRight;
    }

    void UpdateJumping()
    {
        if( Input.GetKey( KeyCode.Space ) == true && m_IsGrounded == true )
        {
            m_Animator.SetTrigger( "IsJumping" );
            m_Body.AddForce( Vector2.up * JumpForce );
            m_PhotonView.RPC( "DoJump", PhotonTargets.Others );
        }
    }

    [PunRPC]
    void DoJump()
    {
        m_Animator.SetTrigger( "IsJumping" );
    }

    void UpdateMovement()
    {
        Vector2 movementVelocity = m_Body.velocity;

        if( Input.GetAxisRaw( "Horizontal" ) > 0.5f )
        {
            movementVelocity.x = Speed;
            
        }
        else if( Input.GetAxisRaw( "Horizontal" ) < -0.5f )
        {
            movementVelocity.x = -Speed;
        }
        else
        {
            movementVelocity.x = 0;
        }

        m_Body.velocity = movementVelocity;
    }

    void UpdateIsRunning()
    {
        m_Animator.SetBool( "IsRunning", Mathf.Abs( m_Body.velocity.x ) > 0.1f );
    }

    void UpdateIsGrounded()
    {
        Vector2 position = new Vector2( transform.position.x, transform.position.y );

        //RaycastHit2D hit = Physics2D.Raycast( position, -Vector2.up, 0.1f, 1 << LayerMask.NameToLayer( "Ground" ) );
        RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up, 0.1f, jumpLayerMask);

        m_IsGrounded = hit.collider != null;
        m_Animator.SetBool( "IsGrounded", m_IsGrounded );
    }
}
