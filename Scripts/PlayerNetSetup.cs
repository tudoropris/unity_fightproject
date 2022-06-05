using UnityEngine;
using UnityEngine.Networking;


public class PlayerNetSetup : NetworkBehaviour {

 

    [SerializeField] Behaviour[] notLocalPlayerDisable;
    [SerializeField] Behaviour[] localPlayerDisable;


    void Start ()
    {
        

        if (!isLocalPlayer)
        {
            gameObject.layer = 10;
            gameObject.tag = "notLocalPlayer";
            for (int i=0;i<notLocalPlayerDisable.Length; i++)
            {
                notLocalPlayerDisable[i].enabled = false;
            }
        }
        else
        {
            gameObject.layer = 9;
            gameObject.tag = "localPlayer";
            for (int i=0;i<localPlayerDisable.Length; i++)
            {
                localPlayerDisable[i].enabled = false;
            }
        }
    }
	
}
