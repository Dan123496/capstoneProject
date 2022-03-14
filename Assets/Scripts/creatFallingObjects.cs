using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creatFallingObjects : MonoBehaviour 
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
    public int totalApples = 40;
    
    public GameObject rock1;
    public Transform RocksFolder;
    public int totalRocks = 20;

    GameObject newApple;
    GameObject newRock;

    public GameObject player1Wins;
    public GameObject player2Wins;

    public GameObject enviroment;
    public GameObject player1;
    public GameObject player2;


    public static GameObject[] theApples;
    public static GameObject[] theRocks;
    [HideInInspector]
    public static int player1Score = 0;
    [HideInInspector]
    public static int player2Score = 0;
    float aTimer;
    float rTimer;
   
    Scene currentScene;
    string sceneName;
    public string levelName;



    // Start is called before the first frame update
    void Start()
    {
        // so to get the width of the screen, i created a roof object thats left and right edges end at the left and right of the screen, 
        //with a bit of a gap (to avoid items aparing partially off screen)
        //so the width of the screen will be the x value of the size of the roofs sprite renderer.
        // note to self: this is a bit converluded might be best to switch the camera to an orthographic and using the code below to get its width:
        //spawnWidth = 2.0f * cam.orthographicSize*cam.aspect;
        player1Score = 0;
        player2Score = 0;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        spawnWidth = roof.GetComponent<SpriteRenderer>().bounds.size.x;

        //declaring a list of apples and a list of rocks
        // was going to use these as i think i might need them to judge if two items are overlaping
        //but i havent implemented that yet so these lists are unused for now. 
       
        theApples = new GameObject[20];
        theRocks = new GameObject[10];
            
        theApples[0] = apple1;
        theRocks[0] = rock1;
        for(int i=1; i<20; i++)
        {

            newApple = GameObject.Instantiate(apple1, new Vector3(0, 500, 0), this.gameObject.transform.rotation);
            newApple.name = "Apple" + (i+1).ToString();
            newApple.transform.SetParent(ApplesFolder);
            theApples[i] = newApple;
                
                

        }
        for (int i = 1; i < 10; i++)
        {
            newRock = GameObject.Instantiate(rock1, new Vector3(0, 500, 0), this.gameObject.transform.rotation);
            newRock.name = "Rock" + (i+1).ToString();
            newRock.transform.SetParent(RocksFolder);
                
            theRocks[i] = newRock;
        }
        for (int i = 0; i < theApples.Length; i++)
        {
           
            theApples[i].SetActive(false);
        }
        for (int i = 1; i < theRocks.Length; i++)
        {
               
            theRocks[i].SetActive(false);
        }
        
        
        
            //the first apple and rock are already created (to give the other items somthing to copy) 
            // this code just moves them to the spawn point were the rest of the apples and rock will be created
            // made them 20 and -20 to make sure they are apart. 
        
        

            //adds the first apple and rock to their respective lists
            //again this isnt really being used yet
       

            // changes the number of created apples and rock to one, to make sure apple1 and rock1 are included in the totals
        
       

       

    }

    // Update is called once per frame
    void Update()
    {
        if(sceneName == "Level1")
        {
            Updatelevel1();
        }else if (sceneName == "Level2")
        {
            Updatelevel2();
        }

        if (numberOfApplesCreated >= totalApples && numberOfRocksCreated >= totalRocks)
        {
            bool endLevel = true;
            for (int i = 0; i < theApples.Length; i++)
            {
                if (theApples[i].activeSelf == true)
                {
                    endLevel = false;
                    break;
                }
            }
            if (endLevel && sceneName == "Level1")
            {
                if(player1Score> player2Score)
                {
                    player1Wins.SetActive(true);
                }
                else
                {
                    player2Wins.SetActive(true);
                }

                enviroment.SetActive(false);
                player1.SetActive(false);
                player2.SetActive(false);
                StartCoroutine(EndGame());
            }
        }
        
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(levelName);
    }
    void SpawnApples()
    {

        //check to make sure the number of apples created is not greater than or equal to the total numer of apples we want to spawn.
        if (numberOfApplesCreated <= totalApples )
        {          
                
                if (spawnLeft)
                {

                    
                    rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                    for(int i=0; i < theApples.Length; i++)
                    {
                    if (theApples[i].activeSelf == false)
                        {
                        theApples[i].SetActive(true);
                        theApples[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        numberOfApplesCreated++;
                        spawnLeft = false;
                        break;
                        }
                    }
                 
                }
                else
                {
                    rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
                    for (int i = 0; i < theApples.Length; i++)
                    {
                        if (theApples[i].activeSelf == false)
                        {
                            theApples[i].SetActive(true);
                            theApples[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
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
                for (int i = 0; i < theRocks.Length; i++)
                {
                    if (theRocks[i].activeSelf == false)
                    {
                        theRocks[i].SetActive(true);
                        theRocks[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        Debug.Log(roof.transform.position.y);
                        numberOfRocksCreated++;
                        spawnLeftRock = false;
                        break;
                    }
                }
            }
            else
            {
                rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
                for (int i = 0; i < theRocks.Length; i++)
                {
                    if (theRocks[i].activeSelf == false)
                    {
                        theRocks[i].SetActive(true);
                        theRocks[i].transform.position = new Vector3(rnd, roof.transform.position.y, 0);
                        numberOfRocksCreated++;
                        spawnLeftRock = true;
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
        if (rTimer >= 0.95f)
        {

            SpawnRocks();
            rTimer = 0f;
        }
    }
}
