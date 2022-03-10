using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject moveableObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (moveableObjects.activeSelf )
            {
                moveableObjects.SetActive(false);
            }
            else
            {
                moveableObjects.SetActive(true);
            }
            
        }
    }
}
