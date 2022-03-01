using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apples: MonoBehaviour
{
    float fallDown;
    public float gravity;
    public Raycaster raycaster;
    
    public GameObject apple1;
    
    
   

    public int player1Score = 0;
    public int player2Score = 0;

   

    // Start is called before the first frame update
    void Start()
    {
        
        fallDown = Time.deltaTime * gravity * -1.0f;
        
        




    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.down * -fallDown);
        CheckCollision();
        



    }
    void CheckCollision()
    {
        RaycastHit2D hit;
        int weAreColliding = raycaster.ThrowRays(MoveDirection.Down, fallDown, out hit);
        int weAreColliding2 = raycaster.ThrowRays(MoveDirection.Up, fallDown, out hit);
        int weAreColliding3 = raycaster.ThrowRays(MoveDirection.Left, fallDown, out hit);
        int weAreColliding4 = raycaster.ThrowRays(MoveDirection.Right, fallDown, out hit);
        if (weAreColliding == 1|| weAreColliding2 == 1|| weAreColliding3 == 1 || weAreColliding4 == 1)
        {
            this.gameObject.SetActive(false);
            player1Score++;
            Debug.Log("player1 score = " + player1Score);
        }
        else if (weAreColliding == 2 || weAreColliding2 == 2 || weAreColliding3 == 2 || weAreColliding4 == 2)
        {
            this.gameObject.SetActive(false);
            player2Score++;
            Debug.Log("player2 score = " + player2Score);
        }
        else if (weAreColliding == 3 || weAreColliding2 == 3 || weAreColliding3 == 3 || weAreColliding4 == 3)
        {
            this.gameObject.SetActive(false);
            if (this.gameObject.activeSelf == false)Debug.Log("Destroyed");
        }
    } 
   
}
