using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatFallingObjects : MonoBehaviour 
{
    public int numberOfApplesCreated;
    public int numberOfRocksCreated;
    float spawnWidth;
    float rnd;
    bool spawnLeft = true;
    bool spawnLeftRock = true;
    
    public GameObject roof;
    List<GameObject> theApples;
    public GameObject apple1;
    public Transform ApplesFolder;
    public int totalApples = 50;
    GameObject newApple;
    GameObject newRock;
    List<GameObject> theRocks;
    public GameObject rock1;
    public Transform RocksFolder;
    public int totalRocks = 25;
    GameObject aRock;
    public int player1Score = 0;
     public int player2Score = 0;
    public float aTimer;
    public float rTimer;


    // Start is called before the first frame update
    void Start()
    {
        // so to get the width of the screen, i created a roof object thats left and right edges end at the left and right of the screen, 
        //with a bit of a gap (to avoid items aparing partially off screen)
        //so the width of the screen will be the x value of the size of the roofs sprite renderer.
        // note to self: this is a bit converluded might be best to switch the camera to an orthographic and using the code below to get its width:
        //spawnWidth = 2.0f * cam.orthographicSize*cam.aspect;
        spawnWidth = roof.GetComponent<SpriteRenderer>().bounds.size.x;

        //declaring a list of apples and a list of rocks
        // was going to use these as i think i might need them to judge if two items are overlaping
        //but i havent implemented that yet so these lists are unused for now. 
        theApples = new List<GameObject>();
        theRocks = new List<GameObject>();
        
        //the first apple and rock are already created (to give the other items somthing to copy) 
        // this code just moves them to the spawn point were the rest of the apples and rock will be created
        // made them 20 and -20 to make sure they are apart. 
        apple1.transform.position =roof.transform.position+new Vector3(20,0,0);
        rock1.transform.position = roof.transform.position+ new Vector3(-20, 0, 0);

        //adds the first apple and rock to their respective lists
        //again this isnt really being used yet
        theApples.Add(apple1);
        theRocks.Add(rock1);

        // changes the number of created apples and rock to one, to make sure apple1 and rock1 are included in the totals
        numberOfApplesCreated = 1;
        numberOfRocksCreated = 1;
       

        // invokeRepeting is a special function in unity that will call a function repetedly with a set interval in between.
        // the word in speach marks is the name of the function you want to call (SpawnApples() in this case)
        // the first float is the time to wait before the function is first called, 0.5f in this case
        // the second float is the time betwen each new call, 0.5f in this case.
        // so 0.5 seconds after the start of the game the function for spawning apples will run, and every 0.5 seconds it will run again. 
        //InvokeRepeating("SpawnApples", 0.5f, 0.5f);
     
        // same for the Spawn rocks function here
        // the inteval between is 1 as their are half as many rocks to drop as apples,
        //having the inteval at one ensures the rock stop falling at the same time as the apples
        //InvokeRepeating("SpawnRocks", 1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        rTimer += Time.deltaTime;
        aTimer += Time.deltaTime;
        if (aTimer >= 0.5f)
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
     void SpawnApples()
    {

        //check to make sure the number of apples created is not greater than or equal to the total numer of apples we want to spawn.
        if (numberOfApplesCreated<= totalApples)
        {          
                // so here, to make the distrabution of apples a bit fairer, i made sure that apples one apple will fall on the left and then one will fall on the right
                // untill all the apples have spawns, protects aginst the rare scenario of all or almost all of the apples spawnning on one side. 
                // so if spawnLeft is true, it will run the code to spawn an apple on the left.
                if (spawnLeft)
                {

                //Random.Range() generates a random number between two values. 
                //roof.transform.position.x is the middle of the screen
                // 0.5f * spawnWidth is half the width of the screen
                //so roof.transform.position.x - (0.5f * spawnWidth) will be the left side of the screen.
                // so this will generate a random number betwen the x value of the left side of the screen and the middle of the screen.
                // so an apple will fall somewhere on the top left of the screen. 
                rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                    
                    if (numberOfApplesCreated == 1)
                    {
                    //Instantiate creates a copy of a gameObject, in this case our apple
                    // if the number of apples created is 1, then the new apple will be a copy of apple1
                    // rand is the random number created above,  roof.transform.position.y is the hight of the roof, so the fall from the top of the screen
                    newApple = GameObject.Instantiate(apple1, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);

                    }
                    else
                    {
                        //if the number of apples created are more than one, then new apples are create as copys of the last apple created. this is because apple1
                        // and all other apples , will be disabled if they collide, so if you were to make copies of apple1, they would be copied as disabled
                        //meaning the apples would never show up on screen.
                        newApple = GameObject.Instantiate(newApple, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                    }

                    //after an apple is created, add 1 to the numberOfAppleCreated 
                    numberOfApplesCreated++;
                    
                    // changes the apple's name to "apple" with the number of apples created next to it. so as to not have emutible apples with the same name. 
                    newApple.name = "apple" + numberOfApplesCreated.ToString();
                   
                    //puts apples in to a "apples folder" psrent object, to not cluter up the sceen. 
                    newApple.transform.SetParent(ApplesFolder);
                    
                    //adds the new apple to theApples list 
                    //not used to do anything yet 
                    theApples.Add(newApple);

                    //sets spawn left to false so the next apple will spawn on the right 
                    spawnLeft = false;
                }
                else
                {
                    //this time the random number is between the roofs x postion, (the middle of the screen)
                    // and the roof x postion + half the roofs width (the far right of the screen)  
                    rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
                    
                    //as the frist apple instantiated will be on the left, this code for the right dosent need to check if the numberOfApplesCreated is = to 1
                    // and can imediently create a new apple coping the last apple. 
                    newApple = GameObject.Instantiate(newApple, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                    
                    //this is the same as the code for the left so refer to the above
                    numberOfApplesCreated++;
                    newApple.name = "apple" + numberOfApplesCreated.ToString();
                    theApples.Add(newApple);
                    newApple.transform.SetParent(ApplesFolder);

                    //sets spawn left to true so the next apple will spawn on the left 
                    spawnLeft = true;
                }   

        }
    }
    void SpawnRocks()
    {// this is pretty much the excact same code as SpawnApples but for the rocks, 
        //everything is the same expet the veribles have been changed to their rock versions.
        if (numberOfRocksCreated <= totalRocks)
        {
            Debug.Log("there");
            if (spawnLeftRock)
            {

                rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                if (numberOfRocksCreated == 1)
                {
                    newRock = GameObject.Instantiate(rock1, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);

                }
                else
                {
                    newRock = GameObject.Instantiate(newRock, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                }

                numberOfRocksCreated++;
                newRock.name = "rock" + numberOfRocksCreated.ToString();
                theRocks.Add(newRock);
                newRock.transform.SetParent(RocksFolder);
                spawnLeftRock = false;
            }
            else
            {
                rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));

                newRock = GameObject.Instantiate(newRock, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                numberOfRocksCreated++;
                newRock.name = "rock" + numberOfRocksCreated.ToString();
                theRocks.Add(newRock);
                newRock.transform.SetParent(RocksFolder);
                spawnLeftRock = true;
            }
        }
           
    }
}
