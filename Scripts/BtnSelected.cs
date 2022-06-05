using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BtnSelected : MonoBehaviour, ISelectHandler {

    public string charName;
    public GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (gameManager.GetComponent<GameManager>().charSelectCanvas1.activeInHierarchy)
        {
            gameManager.GetComponent<GameManager>().player1choice = charName;   

            if(charName == "Joom Byx")
            {
                gameManager.GetComponent<GameManager>().joomByxCharSelSprite1.SetActive(true);
                gameManager.GetComponent<GameManager>().thaliaCharSelSprite1.SetActive(false);
            }

            else
                if(charName == "Thalia")
            {
                gameManager.GetComponent<GameManager>().joomByxCharSelSprite1.SetActive(false);
                gameManager.GetComponent<GameManager>().thaliaCharSelSprite1.SetActive(true);
            }

        }

        else
            if (gameManager.GetComponent<GameManager>().charSelectCanvas2.activeInHierarchy)
        {
            gameManager.GetComponent<GameManager>().player2choice = charName;

            if (charName == "Joom Byx")
            {
                gameManager.GetComponent<GameManager>().joomByxCharSelSprite2.SetActive(true);
                gameManager.GetComponent<GameManager>().thaliaCharSelSprite2.SetActive(false);
            }

            else
    if (charName == "Thalia")
            {
                gameManager.GetComponent<GameManager>().joomByxCharSelSprite2.SetActive(false);
                gameManager.GetComponent<GameManager>().thaliaCharSelSprite2.SetActive(true);
            }
        }

    }
}
