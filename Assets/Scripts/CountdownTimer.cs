using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountdownTimer : MonoBehaviour
{
    public GameObject textDisplay;
    public int secondsLeft = 30;
    public bool takingAway = false;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay.GetComponent<Text>().text = "00:" + secondsLeft;
        text = gameObject.GetComponent<Text> ();
        text.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingAway ==false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }

        if (takingAway ==false && secondsLeft > 10)
        {
            text.color = Color.red;
        }
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -=1;
        textDisplay.GetComponent<Text>().text = "00:" + secondsLeft;
        takingAway = false;
    }
}
