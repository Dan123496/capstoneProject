using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 120f;
    private TextMeshProUGUI textMesh;
    public GameObject SpawnBlocker;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlocker.SetActive(false);
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = currentTime.ToString("0");
        currentTime -=1 * Time.deltaTime;

        if(currentTime < 10)
        textMesh.color = Color.red;

        if(currentTime < 0)
        Endgame();
    }

    void Endgame()
    {
        SpawnBlocker.SetActive(true);
        Time.timeScale = 0;
    }
}
