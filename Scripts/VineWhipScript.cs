using UnityEngine;
using System.Collections;

public class VineWhipScript : MonoBehaviour {

    public float damage;
    public float stunTime;
    public float force;
    public float duration;
    public string targetName;

    public BoxCollider2D vineCol;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        vineCol.enabled = false;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == targetName)
        {
            col.GetComponent<HealthManager>().TakeDamage(damage);
            col.GetComponent<HealthManager>().StartCoroutine("GetStunned", stunTime);
            if (transform.localScale.x > 0)
                col.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
            else
                col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
        }
    }
}
