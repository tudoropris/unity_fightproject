using UnityEngine;
using System.Collections;

public class LifeStealingPlantScript : MonoBehaviour {

    public float plant_Duration;
    public float plant_StealRate;
    public string plant_targetName;
    public string plant_casterName;

    private Animator anim;

    private GameObject caster;
    private GameObject target;

    public float timer;

    void Start()
    {
        caster = GameObject.Find(plant_casterName);
        timer = 1f;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (target != null)
            {
                
                target.GetComponent<HealthManager>().anim.SetBool("Stun", false);
                target.GetComponent<HealthManager>().isStunned = false;
            }
            
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == plant_targetName)
        {
            plant_StealRate = plant_StealRate * col.transform.GetComponent<HealthManager>().maxHP / 100;
            timer = plant_Duration;
            target = GameObject.Find(col.name);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.name == plant_targetName)
        {
            col.transform.GetComponent<HealthManager>().TakeDamage(plant_StealRate * Time.deltaTime);
            caster.transform.GetComponent<HealthManager>().TakeDamage(-plant_StealRate * Time.deltaTime);
            col.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            col.transform.position = transform.position;
            col.transform.GetComponent<HealthManager>().anim.SetBool("Stun", true);
            col.transform.GetComponent<HealthManager>().isStunned = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.name == plant_targetName)
        {
            col.transform.GetComponent<HealthManager>().anim.SetBool("Stun", false);
            col.transform.GetComponent<HealthManager>().isStunned = false;
        }
    }


}
