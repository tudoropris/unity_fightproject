using UnityEngine;
using System.Collections;

public class AttackManager : MonoBehaviour
{
    private Animator anim;

    public GameObject opponent;

    //setting controls
    private KeyCode normAtk_Key;
    private KeyCode special_I_Key;
    private KeyCode special_II_Key;
    private KeyCode special_III_Key;
    private KeyCode manaCharge_Key;

    //state bools
    private bool isAttacking;
    private bool isNormalAtk1;
    private bool isNormAtk2;
    private bool isSpecial1;
    private bool isSpecial2;
    private bool isSpecial3;
    private bool isChanneling;

    //for mana stuff

    public float manaRechargeRate;
    public float manaBalls_OffsetY;
    public GameObject manaBallsPrefab;
    private GameObject manaBalls;

    //for normal atk
    public Collider2D normAtk_Col;
    public int normAtk_Dmg;
    public int normAtk_Cost;
    public float normAtk_StunTime;
    public float normAtk_ComboTime;
    public float normAtk_Cooldown;
    public float normAtk_Force;
    private bool normAtkCombo;

    //for special1
    public GameObject flameSpecial1;
    public GameObject tempFlame;
    public float tempFlameDmg;
    public float specialA_speed;
    public int SpecialA_Cost;
    public int specialA_baseDmg;
    public int specialA_maxDmg;
    public float specialA_DamageIncRate;
    public float flameSpawnPosAbsX;
    public float flameSpawnPosAbsY;
    public bool isCharging;

    //for special 2
    public GameObject gunPwdPrefab;
    public int SpecialB_Cost;
    public float gunPwdSpeed;
    public float gunPwdSpawnPosAbsX;
    public float gunPwdSpawnPosAbsY;

    //for special3
    public Collider2D specialC_Col;
    public GameObject flameshieldPrefab;
    public int specialC_Dmg;
    public int specialC_Cost;
    public float specialC_StunTime;
    public float specialC_Cooldown;
    public float specialC_Force;
    public float flameshield_OffsetY;


    IEnumerator Start()
    {

        yield return new WaitForSeconds(3f);
        AssignControls();
        GetOpponent();
        
        anim = GetComponent<Animator>();

        isAttacking = false;
        isNormalAtk1 = false;
        isNormAtk2 = false;
        isSpecial1 = false;
        isSpecial2 = false;
        isSpecial3 = false;
        isCharging = false;



        normAtk_Col.enabled = false;
        specialC_Col.enabled = false;
    }

    void Update()
    {
       // if (GetComponent<HealthManager>().isStunned) return;
        if (!GetComponent<PlatformerCharacter2D>().m_Grounded) return;

        if (Input.GetKeyDown(normAtk_Key) && (!isAttacking || isNormalAtk1) && !isCharging)
        {
            if (GetComponent<HealthManager>().currentMana >= normAtk_Cost)
            {
                GetComponent<HealthManager>().TakeMana(normAtk_Cost);
                StartCoroutine(NormalAtk1());
            } 


        }

        

        if (Input.GetKeyDown(special_I_Key) && !isAttacking)
        {
            if (GetComponent<HealthManager>().currentMana >= SpecialA_Cost)
            {
                GetComponent<HealthManager>().TakeMana(SpecialA_Cost);
                SpecialAtkA();
            }

        }

        if (Input.GetKey(special_I_Key) && isCharging)
        {
            

            if ((tempFlame != null) && (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) < 0.005) && (!GetComponent<HealthManager>().isStunned)) 
            {
                if (tempFlameDmg < specialA_maxDmg)
                    tempFlameDmg = tempFlameDmg + specialA_DamageIncRate * Time.deltaTime;
            }
            else
            {
                if (tempFlame != null) Destroy(tempFlame);

                isCharging = false;
                isAttacking = false;
                anim.SetBool("Special1Charge", false);
                GetComponent<HealthManager>().canMove = true;
            }
        }

        if(Input.GetKeyUp(special_I_Key) && isCharging)
        {
            if (tempFlame != null)
            {
                tempFlame.GetComponent<flame1Dmg>().flameDmg = (int)tempFlameDmg;
                tempFlame.GetComponent<flame1Dmg>().flameSpeed = specialA_speed;
            }
            
            isCharging = false;
            isAttacking = false;
            anim.SetBool("Special1Charge", false);
            GetComponent<HealthManager>().canMove = true;
        }

        if(Input.GetKeyDown(special_II_Key) && !isAttacking && !isCharging)
        {
            if (GetComponent<HealthManager>().currentMana >= SpecialB_Cost)
            {
                GetComponent<HealthManager>().TakeMana(SpecialB_Cost);
                SpecialAtkB();
            }

        }

        if (Input.GetKeyDown(special_III_Key) && !isAttacking && !isCharging)
        {
            if (GetComponent<HealthManager>().currentMana >= specialC_Cost)
            {
                GetComponent<HealthManager>().TakeMana(specialC_Cost);
                StartCoroutine(SpecialAtkC());
            }

        }

        if (Input.GetKeyDown(manaCharge_Key) && !isAttacking && !isCharging)
        {
            anim.SetBool("IsChanneling", true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<HealthManager>().canMove = false;
            manaBalls = Instantiate(manaBallsPrefab, new Vector3(transform.position.x, transform.position.y + manaBalls_OffsetY, transform.position.z), Quaternion.identity) as GameObject;
            isAttacking = true;
            isChanneling = true;
        }

        if (Input.GetKey(manaCharge_Key) && isChanneling)
        {
            if ((manaBalls != null) && (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) < 0.005) && (!GetComponent<HealthManager>().isStunned))
                GetComponent<HealthManager>().TakeMana(-manaRechargeRate * Time.deltaTime);
            else
            {
                if (manaBalls != null) Destroy(manaBalls);
                anim.SetBool("IsChanneling", false);
                GetComponent<HealthManager>().canMove = true;
                isChanneling = false;
                isAttacking = false;
            }
        }

        if (Input.GetKeyUp(manaCharge_Key) && anim.GetBool("IsChanneling") && isChanneling)
        {
            anim.SetBool("IsChanneling", false);
            Destroy(manaBalls);
            GetComponent<HealthManager>().canMove = true;
            isChanneling = false;
            isAttacking = false;
        }

    }

    IEnumerator NormalAtk1()
    {
        GetComponent<HealthManager>().canMove = false;

        anim.SetTrigger("Atk");

        isAttacking = true;
        normAtk_Col.enabled = true;
        isNormalAtk1 = true;
        if (normAtkCombo)
        {
            anim.SetTrigger("Atk2");
            isNormalAtk1 = false;
            normAtk_Col.enabled = false;
            normAtk_Col.enabled = true;
        }

        normAtkCombo = true;

        yield return new WaitForSeconds(normAtk_Cooldown);


        normAtkCombo = false;
        isNormalAtk1 = false;
        normAtk_Col.enabled = false;
        isAttacking = false;

        GetComponent<HealthManager>().canMove = true;
    }

    /*
    IEnumerator NormalAtk2()
    {
        anim.SetTrigger("Atk2");

        isAttacking = true;
        
        normAtk_Col.enabled = false;
        isNormAtk2 = true;
        normAtk_Col.enabled = true;

        yield return new WaitForSeconds(normAtk_Cooldown);

        isNormAtk2 = false;
        isNormAtk1 = false;
        
        normAtk_Col.enabled = false;
        isAttacking = false;

    }
    */

    void SpecialAtkA()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<HealthManager>().canMove = false;

        isAttacking = true;
        isSpecial2 = true;

        anim.SetTrigger("Special1");
        anim.SetBool("Special1Charge", true);

        float projectileSpawnPosX;
        float projectileSpawnPosY;
        Vector3 projectileSpawnPos;

        if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
            projectileSpawnPosX = transform.position.x + flameSpawnPosAbsX;
        else
            projectileSpawnPosX = transform.position.x - flameSpawnPosAbsX;

        projectileSpawnPosY = transform.position.y + flameSpawnPosAbsY;

        projectileSpawnPos = new Vector3(projectileSpawnPosX, projectileSpawnPosY, transform.position.z);

        GameObject proj = Instantiate(flameSpecial1, projectileSpawnPos, Quaternion.identity) as GameObject;
        if (GetComponent<PlatformerCharacter2D>().m_FacingRight) proj.transform.localScale = new Vector3(-proj.transform.localScale.x, proj.transform.localScale.y, proj.transform.localScale.z);
        proj.GetComponent<flame1Dmg>().flameSpeed = 0;

        tempFlame = proj;

        proj.GetComponent<flame1Dmg>().targetName = opponent.name;
        proj.GetComponent<flame1Dmg>().casterName = gameObject.name;
        proj.GetComponent<flame1Dmg>().flameDmg = specialA_baseDmg;

        tempFlameDmg = specialA_baseDmg;
        isCharging = true;

    }

    void SpecialAtkB()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        GetComponent<HealthManager>().canMove = false;

        anim.SetTrigger("Special2");

        float projectileSpawnPosX;
        float projectileSpawnPosY;
        Vector3 projectileSpawnPos;

        if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
            projectileSpawnPosX = transform.position.x + gunPwdSpawnPosAbsX;
        else
            projectileSpawnPosX = transform.position.x - gunPwdSpawnPosAbsX;

        projectileSpawnPosY = transform.position.y + gunPwdSpawnPosAbsY;

        projectileSpawnPos = new Vector3(projectileSpawnPosX, projectileSpawnPosY, transform.position.z);

        GameObject proj = Instantiate(gunPwdPrefab, projectileSpawnPos, Quaternion.identity) as GameObject;

        proj.GetComponent<GunPowderScript>().speed = gunPwdSpeed;
        proj.GetComponent<GunPowderScript>().targetName = opponent.name;
        proj.GetComponent<GunPowderScript>().casterName = gameObject.name;

        GetComponent<HealthManager>().canMove = true;
    }

    IEnumerator SpecialAtkC()
    {
        GetComponent<HealthManager>().canMove = false;

        anim.SetTrigger("Special3");
        GameObject shield = Instantiate(flameshieldPrefab, new Vector3(transform.position.x, transform.position.y + flameshield_OffsetY, transform.position.z), Quaternion.identity) as GameObject;
        isAttacking = true;
        specialC_Col.enabled = true;
        isSpecial3 = true;

        yield return new WaitForSeconds(specialC_Cooldown);
        
        specialC_Col.enabled = false;
        isSpecial3 = false;
        isAttacking = false;
        Destroy(shield);
        GetComponent<HealthManager>().canMove = true;
    }    


    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.name == opponent.name)
        {
            
            if (isNormalAtk1)
            {
                
                col.GetComponent<HealthManager>().StartCoroutine("GetStunned", normAtk_StunTime);
                col.GetComponent<HealthManager>().TakeDamage(normAtk_Dmg);
            }

            if (isAttacking && normAtkCombo && !isNormalAtk1)
            {
                col.GetComponent<HealthManager>().StartCoroutine("GetStunned", normAtk_StunTime);
                col.GetComponent<HealthManager>().TakeDamage(normAtk_Dmg);
                if (normAtkCombo)
                {
                    if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
                        col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * normAtk_Force);
                    else
                        col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * normAtk_Force);
                }
            }


            if (isSpecial3)
            {
                
                col.GetComponent<HealthManager>().StartCoroutine("GetStunned", specialC_StunTime);
                col.GetComponent<HealthManager>().TakeDamage(specialC_Dmg);
                
                if (transform.position.x < opponent.transform.position.x)
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * specialC_Force);
                else
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * specialC_Force);
            }
            
        }
    }




    void AssignControls()
    {
        if (gameObject.name == "Player1")
        {
            normAtk_Key = KeyCode.V;
            special_I_Key = KeyCode.B;
            special_II_Key = KeyCode.N;
            special_III_Key = KeyCode.M;
            manaCharge_Key = KeyCode.F;
        }
        else if (gameObject.name == "Player2")
        {
            normAtk_Key = KeyCode.Keypad4;
            special_I_Key = KeyCode.Keypad7;
            special_II_Key = KeyCode.Keypad8;
            special_III_Key = KeyCode.Keypad9;
            manaCharge_Key = KeyCode.Keypad6;
        }
    }

    void GetOpponent()
    {
        if (gameObject.name == "Player1")
            opponent = GameObject.Find("Player2");
        else if (gameObject.name == "Player2")
            opponent = GameObject.Find("Player1");
    }

}
