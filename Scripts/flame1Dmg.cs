using UnityEngine;
using System.Collections;

public class flame1Dmg : MonoBehaviour {

    public GameObject explosionPrefab;

    public int flameDmg = 50;
    public string targetName;
    public string casterName;
    public float flameSpeed;
    public float flameStunTime;

    
    private bool goRight;

    void Start()
    {
        
        goRight = true;
        if (GameObject.Find(casterName).GetComponent<PlatformerCharacter2D>().m_FacingRight)
            goRight = true;
        else
            goRight = false;
    }

    void Update()
    {
        if (goRight)
            transform.Translate(Vector3.right * flameSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.left * flameSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.name == targetName)
        {
            col.transform.GetComponent<HealthManager>().TakeDamage(flameDmg);
            col.transform.GetComponent<HealthManager>().StartCoroutine("GetStunned", flameStunTime);
            Destroy(gameObject);
        }
        else if (col.tag == "Walls" || col.tag == "Ground")
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
        else if (col.tag == "Flammable")
        {
            Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
            col.GetComponent<Misc>().SelfDestruct();
            Destroy(gameObject);
        }
        else if (col.GetComponent<Misc>() != null)
            if (col.GetComponent<Misc>().destroyFlameOnTouch)
            {
                int a = 1;
                if (col.GetComponent<flame1Dmg>() != null)
                    if (casterName == col.GetComponent<flame1Dmg>().casterName) a = 2;

                if (a==1)
                    Destroy(gameObject);
            }


    }


}
