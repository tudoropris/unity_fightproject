using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int gameTime;

    public GameObject TitleScreenCanvas;
    public GameObject BattleCanvas;
    public GameObject charSelectCanvas1;
    public GameObject charSelectCanvas2;
    private GameObject timer;
    public GameObject finCanvas;
    public GameObject winnerNameTxt;
    public GameObject JoomByxWinSprite;
    public GameObject ThaliaWinSprite;
    public GameObject joomByxBtn1;
    public GameObject thaliaBtn1;
    public GameObject joomByxBtn2;
    public GameObject thaliaBtn2;
    public GameObject joomByxCharSelSprite1;
    public GameObject thaliaCharSelSprite1;
    public GameObject joomByxCharSelSprite2;
    public GameObject thaliaCharSelSprite2;

    public float timeDecreaseRate;
    public float currentTime;

    public GameObject Stage1;

    public GameObject JoomByx;
    public GameObject Thalia;

    public string player1choice;
    public string player2choice;

    public GameObject player1;
    public GameObject player2;

    public void StartGame()
    {
        charSelectCanvas2.SetActive(false);
        timeDecreaseRate = 1;
        currentTime = gameTime;
        TitleScreenCanvas.SetActive(false);
        Instantiate(Stage1, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject spawnPos1 = GameObject.Find("spawnPos1");
        GameObject spawnPos2 = GameObject.Find("spawnPos2");
        ActivateBattleCanvas();

        //GameObject player1 = null;

        if (player1choice == "Joom Byx")
            player1 = Instantiate(JoomByx, spawnPos1.transform.position, Quaternion.identity) as GameObject;
        else
            if (player1choice == "Thalia")
            player1 = Instantiate(Thalia, spawnPos1.transform.position, Quaternion.identity) as GameObject;

        player1.name = "Player1";

        //GameObject player2 = null;

        if (player2choice == "Joom Byx")
            player2 = Instantiate(JoomByx, spawnPos2.transform.position, Quaternion.identity) as GameObject;
        else
            if (player2choice == "Thalia")
            player2 = Instantiate(Thalia, spawnPos2.transform.position, Quaternion.identity) as GameObject;

        player2.GetComponent<PlatformerCharacter2D>().Flip();
        player2.name = "Player2";
    }

    public void Rematch()
    {
        finCanvas.SetActive(false);
        timeDecreaseRate = 1;
        currentTime = gameTime;
        TitleScreenCanvas.SetActive(false);
        //Instantiate(Stage1, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(player1);
        Destroy(player2);
        GameObject spawnPos1 = GameObject.Find("spawnPos1");
        GameObject spawnPos2 = GameObject.Find("spawnPos2");
        ActivateBattleCanvas();

        //GameObject player1 = null;

        if (player1choice == "Joom Byx")
            player1 = Instantiate(JoomByx, spawnPos1.transform.position, Quaternion.identity) as GameObject;
        else
            if (player1choice == "Thalia")
            player1 = Instantiate(Thalia, spawnPos1.transform.position, Quaternion.identity) as GameObject;

        player1.name = "Player1";

        //GameObject player2 = null;

        if (player2choice == "Joom Byx")
            player2 = Instantiate(JoomByx, spawnPos2.transform.position, Quaternion.identity) as GameObject;
        else
            if (player2choice == "Thalia")
            player2 = Instantiate(Thalia, spawnPos2.transform.position, Quaternion.identity) as GameObject;

        player2.name = "Player2";
    }

    public void Reload()
    {
        Application.LoadLevel(0);
    }

    public void EnterCharSelect()
    {
        TitleScreenCanvas.SetActive(false);
        charSelectCanvas1.SetActive(true);
        joomByxBtn1.GetComponent<UnityEngine.UI.Button>().Select();
    }

    public void NextPlayerSelect()
    {
        charSelectCanvas1.SetActive(false);
        charSelectCanvas2.SetActive(true);
        joomByxBtn2.GetComponent<UnityEngine.UI.Button>().Select();
    }

    public void Win(GameObject winner)
    {
        BattleCanvas.SetActive(false);
        finCanvas.SetActive(true);
        player1.GetComponent<HealthManager>().isStunned = true;
        player2.GetComponent<HealthManager>().isStunned = true;
        if (player1.name == winner.name) player2.GetComponent<Animator>().SetBool("Dead", true);
        if (player2.name == winner.name) player1.GetComponent<Animator>().SetBool("Dead", true);
        winnerNameTxt.GetComponent<UnityEngine.UI.Text>().text = winner.name + " WINS!";
        JoomByxWinSprite.SetActive(false);
        ThaliaWinSprite.SetActive(false);
        if (winner.GetComponent<HealthManager>().characterName == "Joom Byx")
            JoomByxWinSprite.SetActive(true);
        else
            if (winner.GetComponent<HealthManager>().characterName == "Thalia")
            ThaliaWinSprite.SetActive(true);
    }

    public void ActivateBattleCanvas()
    {
        BattleCanvas.SetActive(true);
        timer = GameObject.Find("Timer");
    }

    void Update()
    {
        if (timer != null)
            UpdateTimer();
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime * timeDecreaseRate;
        if (((int)currentTime).ToString() != timer.GetComponent<UnityEngine.UI.Text>().text)
            timer.GetComponent<UnityEngine.UI.Text>().text = ((int)currentTime).ToString();
        if (currentTime<=0)
        {
            if (player1.GetComponent<HealthManager>().currentHP > player2.GetComponent<HealthManager>().currentHP)
                Win(player1);
            else
                Win(player2);
        }
    }
}
