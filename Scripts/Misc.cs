using UnityEngine;
using System.Collections;

public class Misc : MonoBehaviour {

    public bool destroyFlameOnTouch;

	public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
