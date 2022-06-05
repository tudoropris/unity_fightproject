using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public string characterName;

    [SerializeField]
    public float maxHP = 100f;

    [SerializeField]
    private int maxMana = 100;

    [SerializeField]

    public Component atkScript;

    public Animator anim;
    public float currentHP;
    public float currentMana;
    public bool isStunned = false;
    public bool canMove = true;
    public UnityEngine.UI.Slider healthbar;
    public UnityEngine.UI.Slider manabar;

    IEnumerator Start()
    {
        

        currentHP = maxHP;
        currentMana = maxMana;
        anim = GetComponent<Animator>();
        
        InitializeHealthbar();
        UpdateHealthbar();
        UpdateManabar();
        yield return new WaitForSeconds(3f);
        canMove = true;
    }

    public void TakeDamage(float amount)
    {
        currentHP = currentHP - amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        UpdateHealthbar();

        if (currentHP <= 0)
            Death();
    }

    public void TakeMana(float amount)
    {
        currentMana = currentMana - amount;

        if (currentMana < 0) currentMana = 0;
        else if (currentMana > maxMana) currentMana = maxMana;

        UpdateManabar();
    }

    public IEnumerator GetStunned(float howLong)
    {
        isStunned = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        anim.SetBool("Stun", true);

        yield return new WaitForSeconds(howLong);

        anim.SetBool("Stun", false);
        isStunned = false;
    }

    public void ApplyForce(Vector2 force)
    {
        Debug.Log("HERE!");
        GetComponent<Rigidbody2D>().velocity = force;
    }

    void UpdateHealthbar()
    {
        healthbar.value = currentHP;
    }

    void UpdateManabar()
    {
        manabar.value = currentMana;
    }

    void Death()
    {
        anim.SetBool("Stun", false);
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;

        anim.SetBool("Dead", true);

        GetComponent<Platformer2DUserControl>().enabled = false;
        GetComponent<PlatformerCharacter2D>().enabled = false;
       // GetComponent<HealthManager>().isStunned = true;
        //GetComponent<AttackManager>().opponent.GetComponent<Platformer2DUserControl>().enabled = false;
       // GetComponent<AttackManager>().opponent.GetComponent<PlatformerCharacter2D>().enabled = false;
       // GetComponent<AttackManager>().opponent.GetComponent<HealthManager>().isStunned = true;
        //atkScript.enabled = false;

        if (characterName == "Joom Byx")
        GameObject.Find("GameManager").GetComponent<GameManager>().Win(GetComponent<AttackManager>().opponent);
        else if (characterName == "Thalia")
            GameObject.Find("GameManager").GetComponent<GameManager>().Win(GetComponent<ThaliaAttackManager>().opponent);
        
        //get game manager to finish game;
    }

    void InitializeHealthbar()
    {
        if (gameObject.name == "Player1")
        {
            healthbar = GameObject.Find("healthbar1").GetComponent<UnityEngine.UI.Slider>();
            manabar = GameObject.Find("manabar1").GetComponent<UnityEngine.UI.Slider>();
        }


        else if (gameObject.name == "Player2")
        {
            healthbar = GameObject.Find("healthbar2").GetComponent<UnityEngine.UI.Slider>();
            manabar = GameObject.Find("manabar2").GetComponent<UnityEngine.UI.Slider>();
        }


        healthbar.maxValue = maxHP;
        manabar.maxValue = maxMana;
    }

    


}
