using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int maxHP;
    public int currentHP;
    public GameObject opponent;
    public string opponentName;

    public bool isStunned;

    public UnityEngine.UI.Slider healthbar;

    PhotonView photonView;

    void Start()
    { 
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("InitializePlayer", PhotonTargets.AllBufferedViaServer);
        isStunned = false;
        InitializeTags();
    }

    void InitializeTags ()
    {
        if (photonView.isMine)
            gameObject.tag = "localPlayer";
        else
            gameObject.tag = "notLocalPlayer";
    }

    [PunRPC]
    void SetOpponent(string TheNameOfYourOpponent)
    {
        opponentName = TheNameOfYourOpponent;
    }

    [PunRPC]
    void InitializePlayer()
    {
        if (GameObject.Find("Player1") == null)
        {
            gameObject.name = "Player1";
            healthbar = GameObject.Find("Healthbar1").GetComponent<UnityEngine.UI.Slider>();
            currentHP = maxHP;
            healthbar.maxValue = maxHP;
            healthbar.value = maxHP;
            photonView.RPC("UpdateHealthbars", PhotonTargets.AllBufferedViaServer);
            opponentName = "Player2";
        }
        else
        {
            gameObject.name = "Player2";
            healthbar = GameObject.Find("Healthbar2").GetComponent<UnityEngine.UI.Slider>();
            currentHP = maxHP;
            healthbar.maxValue = maxHP;
            healthbar.value = maxHP;
            photonView.RPC("UpdateHealthbars", PhotonTargets.AllBufferedViaServer);
            opponentName = "Player1";
        }

    }

    [PunRPC]
    void UpdateHealthbars()
    {
        healthbar.value = currentHP;
       // healthbar2.value = GameObject.Find("Player2").GetComponent<HealthScript>().currentHP;
    }

    [PunRPC]
    void TakeDamageRPC(int amount)
    {
        currentHP = currentHP - amount;
        photonView.RPC("UpdateHealthbars", PhotonTargets.AllViaServer);
    }

    public void TakeDamage (int amount)
    {
        photonView.RPC("TakeDamageRPC", PhotonTargets.AllViaServer, amount);
    }

    [PunRPC]
    void StunManagerRPC(bool x)
    {
        if (x)
        {
            isStunned = true;
            //play stun anim
        }

        else
        {
            isStunned = false;
            //play idle anim
        }
    }

    public IEnumerator GetStunned (float howLong)
    {
        isStunned = true;
        //play stun anim
        photonView.RPC("StunManagerRPC", PhotonTargets.Others, isStunned);
        Debug.Log("Start " + Time.time);
        yield return new WaitForSeconds(howLong);
        Debug.Log("Start " + Time.time);
        isStunned = false;
        //play idle anim
        photonView.RPC("StunManagerRPC", PhotonTargets.Others, isStunned);
    }

   


 //   void Update ()
 //   {
 //     if ((Input.GetKeyDown(KeyCode.C)) && (photonView.isMine))
 //       {
 //           photonView.RPC("TakeDamage", PhotonTargets.AllBufferedViaServer, 60);
 //       }
 //      
 //   }
}
