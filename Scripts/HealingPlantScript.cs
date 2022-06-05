using UnityEngine;
using System.Collections;

public class HealingPlantScript : MonoBehaviour {

    public CircleCollider2D plant_Collider;

    public float plant_Duration;
    
    public float plant_HealRate;
    public float plant_ColRadius;

    public float timer;

    void Start()
    {
        plant_Collider.radius = plant_ColRadius;
        timer = plant_Duration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D col)
    {
       
        if (col.tag == "Player")
        {
            col.GetComponent<HealthManager>().TakeDamage(-plant_HealRate * Time.deltaTime);
        }
    }
}
