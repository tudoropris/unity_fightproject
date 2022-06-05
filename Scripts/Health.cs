using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour
{
    public string ID;
    public int maxHealth = 100;

    [SyncVar(hook = "TakeDmgCallback")]
  //  [SyncVar]
    public int currentHealth;

    public UnityEngine.UI.Slider healthBar;

    public void Awake()
    {
        ID = "Player " + gameObject.GetComponent<NetworkIdentity>().netId;
        gameObject.name = ID;

        if (GameObject.Find("Player 1") == null) gameObject.name = "Player 1";
        else gameObject.name = "Player 2";

        if (gameObject.name == "Player 1") healthBar = GameObject.Find("Healthbar1").GetComponent<UnityEngine.UI.Slider>();
        else if (gameObject.name == "Player 2") healthBar = GameObject.Find("Healthbar2").GetComponent<UnityEngine.UI.Slider>();

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
       // if (gameObject.name == "Player 1") TakeDamage(20);
       //else TakeDamage(69);
    }
    [Command]
    public void CmdTakeDamage(int amount)
    {
       // if (!isServer)
       //     return;

        currentHealth -= amount;
        healthBar.value = currentHealth;
    }

    void TakeDmgCallback (int currentHealth)
    {
        healthBar.value = currentHealth;
    }
}
