using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apples: MonoBehaviour
{
    float fallDown;
    public float gravity;
    public Raycaster raycaster;
    
    public GameObject apple1;
    public GameObject roof;
    public Text player1Text;
    public Text player2Text;
    string scoreText1;
    string scoreText2;







    // Start is called before the first frame update
    void Start()
    {
        // sets the rate at which the apples will fall, using fixDeltaTime not deltatime so the speed is not effected by frame rate.
        // the gravity can be edited in the inspector
        fallDown = Time.fixedDeltaTime * gravity ;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // moves this insancance of an apple down by the fallDown
        transform.Translate(Vector3.down * fallDown);
        //transform.Translate(Vector3.up * -fallDown); would be the same as the above  
        
        //runs the checkCollistions function every frame. 
        CheckCollision();
 
    }
    void CheckCollision()
    {
        //calls the raycast function 
        RaycastHit2D hit;
        // uses the raycasts ThrowRays function to check for collisions with the apple. 
        // this is done in all directions as the apple could be hit in any directions. 
        int weAreColliding = raycaster.ThrowRays(MoveDirection.Down, fallDown, out hit);
        int weAreColliding2 = raycaster.ThrowRays(MoveDirection.Up, fallDown, out hit);
        int weAreColliding3 = raycaster.ThrowRays(MoveDirection.Left, fallDown, out hit);
        int weAreColliding4 = raycaster.ThrowRays(MoveDirection.Right, fallDown, out hit);

        // if the apple collides with player 1 from any direction ThrowRays will return 1
        // so if any of the weAreColliding are equal to 1, this apple has collided with player1 
        if (weAreColliding == 1|| weAreColliding2 == 1|| weAreColliding3 == 1 || weAreColliding4 == 1)
        {
            // this.gameObject refers to this apple, the apple that collided.
            // setActive false, disables this apples gameObject so it can no longer interact with anything in game
            this.gameObject.SetActive(false);

            // this adds 1 to the player1Score variable in the createFallingObjects script, to increase  player 1's score
            // this is a bad way of doing this and i should really give the player1Score variable in the creatFallingObjects script a getter and a setter
            // i will get round to changing this eventerly
            creatFallingObjects.player1Score = creatFallingObjects.player1Score + 1;

            // sets the string scoreText1 to be equal to the player1Score variable in the creatFallingObjects script.
            //again, this should really be done with a getter, and i will get round to changing it
            scoreText1 = creatFallingObjects.player1Score.ToString();

            //player1Text is the Text displayed in game. this sets its text to Player 1, and the players score (scoreText1). 
            player1Text.text = "Player 1 /  " + scoreText1;
            
        }
        // if the apple collides with player 2 from any direction ThrowRays will return 2
        // so if any of the weAreColliding are equal to 2, this apple has collided with player2 
        else if (weAreColliding == 2 || weAreColliding2 == 2 || weAreColliding3 == 2 || weAreColliding4 == 2)
        {
            //this all dose the same as for player1 but for player2 insted, refer to above 
            this.gameObject.SetActive(false);
            creatFallingObjects.player2Score = creatFallingObjects.player2Score + 1; 
            scoreText2 = creatFallingObjects.player2Score.ToString();
            player2Text.text = "Player 2 /  " + scoreText2;
            
        }
        // if the apple collides with the ground ThrowRays will return 2
        // so if any of the weAreColliding are equal to 3, this apple has collided with thw ground
        else if (weAreColliding == 3 || weAreColliding2 == 3 || weAreColliding3 == 3 || weAreColliding4 == 3)
        {
            //as with above, this disables the apple if it collides with the ground 
            this.gameObject.SetActive(false);
        }
    } 
   
}
