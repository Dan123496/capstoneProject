using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatFallingObjects : MonoBehaviour 
{
    public int numberOfApplesCreated;
    float spawnWidth;
    float rnd;
    bool spawnLeft = true;
    List<GameObject> theApples;
    public GameObject roof;
    public GameObject apple1;
    public Transform ApplesFolder;
    public int totalApples = 50;
    GameObject anApple;

    // Start is called before the first frame update
    void Start()
    {
        spawnWidth = roof.GetComponent<SpriteRenderer>().bounds.size.x;
        theApples = new List<GameObject>();
        apple1.transform.position =roof.transform.position;
        theApples.Add(apple1);
        
        numberOfApplesCreated = 1;
        spawnWidth = roof.GetComponent<SpriteRenderer>().bounds.size.x;
        InvokeRepeating("SpawnApples", 0.5f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
       
    }
     void SpawnApples()
    {

        
        if (numberOfApplesCreated<= totalApples)
        {
            if (spawnLeft)
            {

                rnd = Random.Range(roof.transform.position.x - (0.5f * spawnWidth), roof.transform.position.x);
                if(numberOfApplesCreated == 1 )
                {
                     anApple = GameObject.Instantiate(apple1, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);

                }else
                {
                    anApple = GameObject.Instantiate(anApple, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                }
                
                numberOfApplesCreated++;
                anApple.name = "apple" + numberOfApplesCreated.ToString();
                theApples.Add(anApple);
                anApple.transform.SetParent(ApplesFolder);
                spawnLeft = false;
            }
            else
            {
                rnd = Random.Range(roof.transform.position.x, roof.transform.position.x + (0.5f * spawnWidth));
               
                 anApple = GameObject.Instantiate(anApple, new Vector3(rnd, roof.transform.position.y, 0), this.gameObject.transform.rotation);
                numberOfApplesCreated++;
                anApple.name = "apple" + numberOfApplesCreated.ToString();
                theApples.Add(anApple);
                anApple.transform.SetParent(ApplesFolder);
                spawnLeft = true;
            }

        }
    }
}
