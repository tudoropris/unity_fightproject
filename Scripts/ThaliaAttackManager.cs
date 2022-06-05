using UnityEngine;
using System.Collections;

public class ThaliaAttackManager : MonoBehaviour
{
    private Animator anim;

    public GameObject opponent;

    //setting controls
    private KeyCode normAtk_Key;
    private KeyCode special_I_Key;
    private KeyCode special_II_Key;
    private KeyCode special_III_Key;
    private KeyCode manaCharge_Key;

    //mana stuff
    public float manaRechargeRate;
    public GameObject manaBallsPrefab;
    public float manaBalls_OffsetY;
    private GameObject manaBalls;

    //state bools
    private bool isAttacking;
    private bool isNormAtk1;
    private bool isNormAtk2;
    private bool isSpecial1;
    private bool isSpecial2;
    private bool isSpecial3;
    private bool isCharging;
    private bool isChanneling;

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
    public float SpecialA_Cost;
    
    public GameObject vine_Prefab;
    public float vine_Dmg;
    public float vine_StunTime;
    public float vine_Force;
    public float vine_OffsetX;

    //for special 2
    public int SpecialB_Cost;
    public float SpecialB_OffsetX;
    public float SpecialB_OffsetY;
    public float SpecialB_Cooldown;
    public GameObject healingPlant_Prefab;
    public float healingPlant_Duration;
    public float healingPlant_HealRate;
    public float healingPlant_ColRadius;

    //for special3
    public int SpecialC_Cost;
    public float SpecialC_OffsetX;
    public float SpecialC_OffsetY;
    public float SpecialC_Cooldown;

    public GameObject lifeStealingPlant_Prefab;
    public float lifeStealingPlant_Duration;
    public float lifeStealingPlant_StealRate;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);

        AssignControls();
        GetOpponent();

        anim = GetComponent<Animator>();

        isAttacking = false;
        isNormAtk1 = false;
        isNormAtk2 = false;
        isSpecial1 = false;
        isSpecial2 = false;
        isSpecial3 = false;
        isCharging = false;
        isChanneling = false;


        normAtk_Col.enabled = false;
        
    }

    void Update()
    {
        //if (GetComponent<HealthManager>().isStunned) return;
        if (!GetComponent<PlatformerCharacter2D>().m_Grounded) return;

        if (Input.GetKeyDown(normAtk_Key) && (!isAttacking || isNormAtk1) && !isCharging)
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

        

        if (Input.GetKeyDown(special_II_Key) && !isAttacking && !isCharging)
        {
            if (GetComponent<HealthManager>().currentMana >= SpecialB_Cost)
            {
                GetComponent<HealthManager>().TakeMana(SpecialB_Cost);
                StartCoroutine(SpecialAtkB());
            }

        }

        if (Input.GetKeyDown(special_III_Key) && !isAttacking && !isCharging)
        {
            if (GetComponent<HealthManager>().currentMana >= SpecialC_Cost)
            {
                GetComponent<HealthManager>().TakeMana(SpecialC_Cost);
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
        isNormAtk1 = true;
        if (normAtkCombo)
        {
            anim.SetTrigger("Atk2");
            isNormAtk1 = false;
            normAtk_Col.enabled = false;
            normAtk_Col.enabled = true;
        }

        normAtkCombo = true;

        yield return new WaitForSeconds(normAtk_Cooldown);


        normAtkCombo = false;
        isNormAtk1 = false;
        normAtk_Col.enabled = false;
        isAttacking = false;

        GetComponent<HealthManager>().canMove = true;
    }

    void SpecialAtkA()
    {
        GetComponent<HealthManager>().canMove = false;

        anim.SetTrigger("Special1");
        //anim.SetBool("Special1Charge", true);

        float projectileSpawnPosX;
        
        Vector3 projectileSpawnPos;

        if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
            projectileSpawnPosX = transform.position.x + vine_OffsetX;
        else
            projectileSpawnPosX = transform.position.x - vine_OffsetX;

        projectileSpawnPos = new Vector3(projectileSpawnPosX, transform.position.y, transform.position.z);

        GameObject proj = Instantiate(vine_Prefab, projectileSpawnPos, Quaternion.identity) as GameObject;
        if (GetComponent<PlatformerCharacter2D>().m_FacingRight) proj.transform.localScale = new Vector3(-proj.transform.localScale.x, proj.transform.localScale.y, proj.transform.localScale.z);



        proj.GetComponent<VineWhipScript>().damage = vine_Dmg;
        proj.GetComponent<VineWhipScript>().stunTime = vine_StunTime;
        proj.GetComponent<VineWhipScript>().force = vine_Force;
        proj.GetComponent<VineWhipScript>().targetName = opponent.name;

        GetComponent<HealthManager>().canMove = true;
    }

    IEnumerator SpecialAtkB()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<HealthManager>().isStunned = true;

        anim.SetTrigger("Special2");

        isAttacking = true;
        isSpecial2 = true;

        float projectileSpawnPosX;
        float projectileSpawnPosY;
        Vector3 projectileSpawnPos;

        if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
            projectileSpawnPosX = transform.position.x + SpecialB_OffsetX;
        else
            projectileSpawnPosX = transform.position.x - SpecialB_OffsetX;

        projectileSpawnPosY = transform.position.y + SpecialB_OffsetY;

        projectileSpawnPos = new Vector3(projectileSpawnPosX, projectileSpawnPosY, transform.position.z);

        GameObject proj = Instantiate(healingPlant_Prefab, projectileSpawnPos, Quaternion.identity) as GameObject;

        proj.GetComponent<HealingPlantScript>().plant_Duration = healingPlant_Duration;
        proj.GetComponent<HealingPlantScript>().plant_HealRate = healingPlant_HealRate;
        proj.GetComponent<HealingPlantScript>().plant_ColRadius = healingPlant_ColRadius;

        yield return new WaitForSeconds(SpecialB_Cooldown);

        isAttacking = false;
        isSpecial2 = false;

        GetComponent<HealthManager>().isStunned = false;
    }

    IEnumerator SpecialAtkC()
    {

        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        GetComponent<HealthManager>().isStunned = true;
        anim.SetTrigger("Special3");
        anim.SetBool("Special3Charge", true);

        isAttacking = true;
        isSpecial3 = true;

        float projectileSpawnPosX;
        float projectileSpawnPosY;
        Vector3 projectileSpawnPos;

        if (GetComponent<PlatformerCharacter2D>().m_FacingRight)
            projectileSpawnPosX = transform.position.x + SpecialC_OffsetX;
        else
            projectileSpawnPosX = transform.position.x - SpecialC_OffsetX;

        projectileSpawnPosY = transform.position.y + SpecialC_OffsetY;

        projectileSpawnPos = new Vector3(projectileSpawnPosX, projectileSpawnPosY, transform.position.z);

        GameObject proj = Instantiate(lifeStealingPlant_Prefab, projectileSpawnPos, Quaternion.identity) as GameObject;

        proj.GetComponent<LifeStealingPlantScript>().plant_Duration = lifeStealingPlant_Duration;
        proj.GetComponent<LifeStealingPlantScript>().plant_StealRate = lifeStealingPlant_StealRate;
        proj.GetComponent<LifeStealingPlantScript>().plant_targetName = opponent.name;
        proj.GetComponent<LifeStealingPlantScript>().plant_casterName = gameObject.name;

        yield return new WaitForSeconds(1.1f);

        if (proj != null) yield return new WaitForSeconds(lifeStealingPlant_Duration - 1f);
        anim.SetBool("Special3Charge", false);

        isSpecial3 = false;
        isAttacking = false;

        GetComponent<HealthManager>().isStunned = false;

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == opponent.name)
        {

            if (isNormAtk1)
            {

                col.GetComponent<HealthManager>().StartCoroutine("GetStunned", normAtk_StunTime);
                col.GetComponent<HealthManager>().TakeDamage(normAtk_Dmg);
            }

            if (isAttacking && normAtkCombo && !isNormAtk1)
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
