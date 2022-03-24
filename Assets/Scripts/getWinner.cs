using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class getWinner : MonoBehaviour
{
    public TextMeshProUGUI winner; // Start is called before the first frame update
    void Start()
    {
        if(creatFallingObjects.player1Score > creatFallingObjects.player2Score)
        {
            winner.text = "Congratulations Player 1 \n You Win this level";
        }
        else if (creatFallingObjects.player1Score < creatFallingObjects.player2Score)
        {
            winner.text = "Congratulations Player 2 \n You Win this level";
        }
        else
        {
            winner.text = "The Scores were Tied! \n This level is a Tie";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
