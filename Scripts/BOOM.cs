using UnityEngine;
using System.Collections;

public class BOOM : MonoBehaviour {

    public GameObject explosionPrefab;

    public int boom_Dmg;
    public float boom_StunTime;
    public float boom_Force;

    public float boom_Duration;

	void Start()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        //play boom anim
        StartCoroutine("expDuration");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<HealthManager>().TakeDamage(boom_Dmg);
            col.GetComponent<HealthManager>().StartCoroutine("GetStunned", boom_StunTime);

            if (col.transform.position.x > transform.position.x)
                col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * boom_Force);
            else
                col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * boom_Force);
            Destroy(gameObject);
        }

        if (col.tag == "Flammable")
        {
            Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
            col.GetComponent<Misc>().SelfDestruct();
            //Destroy(gameObject);
        }
    }

    IEnumerator expDuration()
    {
        yield return new WaitForSeconds(boom_Duration);
        Destroy(gameObject);
    }
}
