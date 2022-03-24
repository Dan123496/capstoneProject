using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 120;
    private TextMeshProUGUI textMesh;
    public GameObject Player1Wins;
    public GameObject Player2Wins;
    public GameObject tie;
    public GameObject moveableObjects;
    public GameObject enviroment;
    float timer = 0;


    public string levelName;
    
    // Start is called before the first frame update
    void Start()
    {
        Player1Wins.SetActive(false);
        Player2Wins.SetActive(false);
        tie.SetActive(false);

        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        currentTime = startingTime;
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = currentTime.ToString("0");
        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
        }
           

        if (currentTime < 10)
            textMesh.color = Color.red;

        if (currentTime < 0)
        {
            WhoWins();
           
        }

    }

    void WhoWins()
    {
        if (creatFallingObjects.player1Score > creatFallingObjects.player2Score)
        {
            Player1Wins.SetActive(true);
            Debug.Log("1 wins");

        }
        else if (creatFallingObjects.player1Score < creatFallingObjects.player2Score)
        {
            Player2Wins.SetActive(true);
        }
        else
        {
            tie.SetActive(true);
        }
        enviroment.SetActive(false);
        moveableObjects.SetActive(false);
        StartCoroutine(Endgame());


    }
    IEnumerator  Endgame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(levelName);
        

    }
}