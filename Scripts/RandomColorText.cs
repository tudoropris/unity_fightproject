using UnityEngine;
using System.Collections;

public class RandomColorText : MonoBehaviour {

	IEnumerator Start()
    {
        GetComponent<UnityEngine.UI.Text>().color = Random.ColorHSV();
        yield return new WaitForSeconds(0.5f);
        int a = 1;
        while (a == 1)
        {
            yield return new WaitForSeconds(0.5f);
            GetComponent<UnityEngine.UI.Text>().color = Random.ColorHSV();
        }
    }

    
}
