using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncVarManager : NetworkBehaviour {

    [SyncVar]
    public Vector3 scale1;

    [SyncVar]
    public Vector3 scale2;


}
