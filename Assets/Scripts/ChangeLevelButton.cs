using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelButton : MonoBehaviour
{
    public string levelName;

    // Start is called before the first frame update
    
    public void LaunchLevelChange(){
        SceneManager.LoadScene(levelName);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
