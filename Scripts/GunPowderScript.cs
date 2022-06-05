using UnityEngine;
using System.Collections;

public class GunPowderScript : MonoBehaviour
{

    public string targetName;
    public string casterName;
    public float speed;

    public int boom_Dmg;
    public float boom_StunTime;
    public float boom_Force;

    public float survivalTime;
    public GameObject target;
    public bool followTarget;
    
    public float timeLeft;

    public GameObject explosionPrefab;

    private bool goRight;

    void Start()
    {
        followTarget = false;
        
        
        goRight = true;
        if (GameObject.Find(casterName).GetComponent<PlatformerCharacter2D>().m_FacingRight)
            goRight = true;
        else
            goRight = false;

        timeLeft = survivalTime;
    }

    void Update()
    {
        if(!followTarget)
        {
            if (goRight)
               transform.Translate(Vector3.right * speed * Time.deltaTime);
            else
               transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.position = target.transform.position;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) Destroy(gameObject);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.name == targetName)
        {
            target = GameObject.Find(targetName);

            followTarget = true;
            col.GetComponent<Platformer2DUserControl>().StartCoroutine("SlowDown", timeLeft);
        }
        
    }
}