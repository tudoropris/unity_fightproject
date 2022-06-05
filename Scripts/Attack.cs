using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Attack : NetworkBehaviour {

    public BoxCollider2D normAtk;

    void Start ()
    {
        normAtk.enabled = false;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            NormalAtk();
        }
    }

    public void NormalAtk()
    {
        //do anim
        //then on frame x
        normAtk.enabled = true;
        
    }

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (hit.tag == "notLocalPlayer") hit.GetComponent<Health>().CmdTakeDamage(49);
    }
}
