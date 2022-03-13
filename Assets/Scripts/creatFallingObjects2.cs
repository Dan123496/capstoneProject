using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creatFallingObjects2 : MonoBehaviour 
{
    int numberOfApplesCreated;
    int numberOfRocksCreated;
    float spawnWidth;
    float rnd;
    bool spawnLeft = true;
    bool spawnLeftRock = true;
    
    public GameObject roof;
    
    public GameObject apple1;
    public Transform ApplesFolder;
    public int totalApples = 100;
    
    public GameObject rock1;
    public Transform RocksFolder;
    public int totalRocks = 40;

    GameObject newApple;
    GameObject newRock;
    

   
    [HideInInspector]
    public  int player1Score = 0;
    [HideInInspector]
    public  int player2Score = 0;
    float aTimer;
    float rTimer;
   
    Scene currentScene;
    string sceneName;
    public string levelName;



    // Start is called before the first frame update
    void Start()
    {
       
        player1Score = 0;
        player2Score = 0;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        spawnWidth = roof.GetComponent<SpriteRenderer>().bounds.size.x;

        
       
        

    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Level2")
        {
            Updatelevel2();

        }
        if (numberOfApplesCreated >= totalApples && numberOfRocksCreated >= totalRocks)
        {
            bool endLevel = true;
            for (int i = 0; i < creatFallingObjects.theApples.Length; i++)
            {
                if (creatFallingObjects.theApples[i].activeSelf == true)
                {
                    endLevel = false;
                    break;
                }
            }
            if (endLevel)
            {
                SceneManager.LoadScene(levelName);
            }
        }
        
    }
    void SpawnApples()
    {

        //check to make sure the number of apples created is not greater than or equal to the total numer of apples we want to spawn.
        if (numberOfApplesCreated <= totalApples )
        {          
                
                if (spawnLeft)
                {

                    
                    rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                    for(int i=0; i < creatFallingObjects.theApples.Length; i++)
                    {
                    if (creatFallingObjects.theApples[i].activeSelf == false)
                        {
                        creatFallingObjects.theApples[i].SetActive(true);
                        creatFallingObjects.theApples[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        numberOfApplesCreated++;
                        spawnLeft = false;
                        break;
                        }
                    }
                 
                }
                else
                {
                    rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
                    for (int i = 0; i < creatFallingObjects.theApples.Length; i++)
                    {
                        if (creatFallingObjects.theApples[i].activeSelf == false)
                        {
                        creatFallingObjects.theApples[i].SetActive(true);
                        creatFallingObjects.theApples[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                            numberOfApplesCreated++;
                            spawnLeft = true;
                        break;
                        }
                    }
                    
                }   

        }
    }
    void SpawnRocks()
    {
       
        if (numberOfRocksCreated <= totalRocks)
        {
            
            if (spawnLeftRock)
            {

                rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                for (int i = 0; i < creatFallingObjects.theRocks.Length; i++)
                {
                    if (creatFallingObjects.theRocks[i].activeSelf == false)
                    {
                        creatFallingObjects.theRocks[i].SetActive(true);
                        creatFallingObjects.theRocks[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        Debug.Log(roof.transform.position.y);
                        numberOfRocksCreated++;
                        spawnLeft = false;
                        break;
                    }
                }
            }
            else
            {
                rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
                for (int i = 0; i < creatFallingObjects.theRocks.Length; i++)
                {
                    if (creatFallingObjects.theRocks[i].activeSelf == false)
                    {
                        creatFallingObjects.theRocks[i].SetActive(true);
                        creatFallingObjects.theRocks[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        numberOfRocksCreated++;
                        spawnLeft = true;
                        break;
                    }
                }
                
            }
        }
           
    }
    void Updatelevel1()
    {
        rTimer += Time.deltaTime;
        aTimer += Time.deltaTime;
        if (aTimer >= 0.75f)
        {

            SpawnApples();
            aTimer = 0f;
        }
        if (rTimer >= 1.5f)
        {

            SpawnRocks();
            rTimer = 0f;
        }
    }
    void Updatelevel2()
    {
        rTimer += Time.deltaTime;
        aTimer += Time.deltaTime;
        if (aTimer >= 0.4f)
        {

            SpawnApples();
            aTimer = 0f;
        }
        if (rTimer >= 1f)
        {

            SpawnRocks();
            rTimer = 0f;
        }
    }
}
