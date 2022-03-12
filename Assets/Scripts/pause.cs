using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pause : MonoBehaviour
{
    public GameObject moveableObjects;
    public GameObject pauseImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (moveableObjects.activeSelf )
            {
                moveableObjects.SetActive(false);
                pauseImage.SetActive(true);
            }
            else
            {
                moveableObjects.SetActive(true);
                pauseImage.SetActive(false);
            }
            
        }
    }
}
