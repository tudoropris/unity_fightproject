using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {

    Animator m_Animator;
    public Animation anim;
    

    public Collider2D normAtkCol;
    public int normAtkDamage;
    private bool isNormAtk;
    public float normAtkCooldown = 0.3f;
    public float normAtkStunTime = 1f;
    public float normAtkForce = 2f;

    public GameObject flameSpecial1;
    public float flameSpawnPosAbsX;
    public float flameSpawnPosAbsY;
    public float flameSpecial1Speed;
    public int flameSpecial1Dmg;
    public int flameSpecial1MaxDmg;
    public float flameSpecial1DmgIncRate;
    private GameObject tempFlame;
    private bool isCharging;
    private float tempFlameDmg;
    private bool isSpecial1;

    public int flameBurstDamage;
    private bool isSpecial3;
    public Collider2D flameBurstCol;
    public float flameBurstCooldown = 0.5f;
    public float flameBurstStunTime = 3f;
    public float flameBurstForce = 2f;

    private PhotonView photonView;
    private bool isAttacking;
    public bool isBlocking;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        normAtkCol.enabled = false;
        flameBurstCol.enabled = false;
        isAttacking = false;
        isCharging = false;
        isBlocking = false;
        isNormAtk = false;
        isSpecial1 = false;
        isSpecial3 = false;
        m_Animator = GetComponent<Animator>();
        
    }
    
    void Update()
    {
        if (gameObject.GetComponent<HealthScript>().isStunned)
        {
            return;
        }
        if ((Input.GetKeyDown(KeyCode.C)) && (photonView.isMine) && (!isAttacking))
        {
            
            StartCoroutine(NormalAttack());
        }
        
        
        if ((Input.GetKeyDown(KeyCode.V)) && (photonView.isMine) && (!isAttacking))
        {
            isAttacking = true;
            isSpecial1 = true;
            SpecialAtk1();
        }

        if ((Input.GetKey(KeyCode.V)) && (photonView.isMine) && (isCharging))
        {
            if (tempFlameDmg < flameSpecial1MaxDmg)
                tempFlameDmg = tempFlameDmg + flameSpecial1DmgIncRate*Time.deltaTime;
        }

        if ((Input.GetKeyUp(KeyCode.V)) && (photonView.isMine) && (!isNormAtk) && (!isSpecial3))
        {
            tempFlame.GetComponent<flame1Dmg>().flameDmg = (int)tempFlameDmg;
            tempFlame.GetComponent<flame1Dmg>().flameSpeed = flameSpecial1Speed;
            isCharging = false;
            isSpecial1 = false;
            isAttacking = false;
            m_Animator.SetBool("Special1Charge", false);
        }

        if ((Input.GetKeyDown(KeyCode.N)) && (photonView.isMine) && (!isAttacking))
        {
            
            StartCoroutine(SpecialAtk3());
        }

        if ((Input.GetKeyDown(KeyCode.Z)) && (photonView.isMine) && (!isAttacking))
        {

            isAttacking = true;
            Block();
        }

        if ((Input.GetKeyUp(KeyCode.Z)) && (photonView.isMine))
        {
            m_Animator.SetBool("IsBlocking", false);
            isAttacking = false;
        }
    }

    

    IEnumerator NormalAttack()
    {
        m_Animator.SetTrigger("Atk");
        photonView.RPC("NormalAtkAnimRPC", PhotonTargets.Others);

        isAttacking = true;
        normAtkCol.enabled = true;
        isNormAtk = true;

        yield return new WaitForSeconds(normAtkCooldown);

        normAtkCol.enabled = false;
        isNormAtk = false;
        isAttacking = false;

    }

    
    [PunRPC]
    void NormalAtkAnimRPC()
    {
        m_Animator.SetTrigger("Atk");
    }

    void SpecialAtk1()
    {
        m_Animator.SetTrigger("Special1");
        m_Animator.SetBool("Special1Charge", true);

        float flameSpawnPosX;
        float flameSpawnPosY;
        Vector3 flameSpawnPos;

        if (gameObject.GetComponent<JumpAndRunMovement>().facingRight)
            flameSpawnPosX = transform.position.x + flameSpawnPosAbsX;
        else
            flameSpawnPosX = transform.position.x - flameSpawnPosAbsX;

        flameSpawnPosY = transform.position.y + flameSpawnPosAbsY;

        flameSpawnPos = new Vector3(flameSpawnPosX, flameSpawnPosY, transform.position.z);

        Quaternion flameRotation = Quaternion.identity;
        flameRotation.eulerAngles = new Vector3(0, 0, 0);
        

        GameObject flame = PhotonNetwork.Instantiate(flameSpecial1.name, flameSpawnPos, flameRotation, 0);
        flame.GetComponent<flame1Dmg>().flameSpeed = 0;
        tempFlame = flame;
        flame.GetComponent<flame1Dmg>().targetName = gameObject.GetComponent<HealthScript>().opponentName;
        flame.GetComponent<flame1Dmg>().casterName = gameObject.name;
        flame.GetComponent<flame1Dmg>().flameDmg = flameSpecial1Dmg;
        tempFlameDmg = flameSpecial1Dmg;
        isCharging = true;
    }

    IEnumerator SpecialAtk3()
    {
        //doAnim locally
        photonView.RPC("FlameBurstAnimRPC", PhotonTargets.Others);

        flameBurstCol.enabled = true;
        isAttacking = true;
        isSpecial3 = true;

        yield return new WaitForSeconds(flameBurstCooldown);

        flameBurstCol.enabled = false;
        isAttacking = false;
        isSpecial3 = false;
    }

    [PunRPC]
    void FlameBurstAnimRPC()
    {
        //doAnim locally again
    }

    void Block()
    {
        m_Animator.SetBool("IsBlocking", true);
        isBlocking = true;
        photonView.RPC("BlockAnimRPC", PhotonTargets.Others);
    }

    [PunRPC]
    void BlockAnimRPC()
    {
        m_Animator.SetBool("IsBlocking", true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "notLocalPlayer")
            if (isNormAtk)
            {
                col.transform.GetComponent<HealthScript>().TakeDamage(normAtkDamage);
                col.transform.GetComponent<HealthScript>().StartCoroutine("GetStunned", normAtkStunTime);
                if (gameObject.GetComponent<JumpAndRunMovement>().facingRight)
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * normAtkForce);
                else
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * normAtkForce);
                normAtkCol.enabled = false;
            }
               
            else if (isSpecial3)
            {
                col.transform.GetComponent<HealthScript>().TakeDamage(flameBurstDamage);
                col.transform.GetComponent<HealthScript>().StartCoroutine("GetStunned", flameBurstStunTime);
                if (gameObject.GetComponent<JumpAndRunMovement>().facingRight)
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * flameBurstForce);
                else
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * flameBurstForce);
                flameBurstCol.enabled = false;
            }
                

        
        
    }

   

}
