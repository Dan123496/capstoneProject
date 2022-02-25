using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    [Header("References")]
    public Raycaster raycaster;
    public Transform graphicTransform;
    //public Animator graphicAnimator;

    [Header("Stats")]
    public float speed;

    public float slopeMaxThreshold = 50f;

    // Natural gravity
    public float gravity;
    bool isGrounded;

    // Simple jump mechanic
    public AnimationCurve jumpCurve;
    public int maxAllowedJumps = 2;
    bool isJumping;
    int remainingAllowedJumps;
    float timeSinceJumped;

    // Wall-jump mechanic
    
   

    void Start()
    {
        isJumping = false;
        remainingAllowedJumps = maxAllowedJumps - 1;
    }

    void Update()
    {
        UpdateHorizontalMovement();
        UpdateVerticalMovement();
        
    }

    void UpdateHorizontalMovement()
    {
        float currentMovement = 0f;
        if (Input.GetKey(KeyCode.A)) currentMovement--;
        if (Input.GetKey(KeyCode.D)) currentMovement++;

        //graphicAnimator.SetBool("IsWalking", (currentMovement != 0));

        HorizontalMove(speed * currentMovement * Time.deltaTime);
    }

    float previousFrameHeight;

    void UpdateVerticalMovement()
    {
        // apply either gravity or jump upwards speed
        float currentVerticalMotion = 0f;

        // Vertical movement applied as part of the jump curve
        if (isJumping)
        {
            // Calculate jump movement, for current frame, by checking the two successive states of the curve
            float currentFrameHeight = jumpCurve.Evaluate(timeSinceJumped);
            currentVerticalMotion = currentFrameHeight - previousFrameHeight;

            // save it for next frame
            previousFrameHeight = currentFrameHeight;
        }
        // Not jumping? Then we just apply the regular gravity
        else
        {
            currentVerticalMotion = Time.deltaTime * gravity * -1.0f;
        }

        // actual movement
        VerticalMove(currentVerticalMotion);

        // process jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            JumpStart();
        }

        // Timers related to jump
        JumpUpdate();
    }

    // If we get to implement some walljump mechanic, since it exists across multiple frames, it needs its own Update
   

    // Called when trying to initiate a jump
    void JumpStart()
    {
        // make sure the player is allowed to jump, otherwise reject the function call
        if (remainingAllowedJumps <= 0)
        {
            return;
        }

        // character starts jumping, so it stops touching the ground
        isGrounded = false;
        //graphicAnimator.SetBool("IsGrounded", false);
        //graphicAnimator.SetTrigger("Jump");
        isJumping = true;
        timeSinceJumped = 0f;
        previousFrameHeight = 0f;
        remainingAllowedJumps--;
    }

    void JumpUpdate()
    {
        // no need to call this update if we're not in the middle of a jump
        if (!isJumping) return;

        // update timers
        timeSinceJumped += Time.deltaTime;

        // Total jump time is given by the X-position of the very last keyframe in the curve
        int lastKeyIndex = jumpCurve.keys.Length - 1;
        if (timeSinceJumped > jumpCurve.keys[lastKeyIndex].time)
        {
            // end the jump if the timer reaches its end
            isJumping = false;
        }
    }

    // We're not using it, but it might be needed as an external call 
    public void Move(Vector2 totalMovement)
    {
        HorizontalMove(totalMovement.x);
        VerticalMove(totalMovement.y);
    }

    // positive = right, negative = left
    // returns whether the movement has been performed successfully
    public bool HorizontalMove(float distance)
    {
        // discarding movement function if the player didn't press any key
        if (distance == 0)
        {
            return false;
        }

        // finding out movement direction
        MoveDirection dir = MoveDirection.Right;
        if (distance < 0) dir = MoveDirection.Left;

        // adjusting character scale so it looks towards left or right
        Vector3 characterScale = graphicTransform.localScale;
        characterScale.x = Mathf.Abs(characterScale.x); // "absolute value" so it's always positive when facing right
        if (distance >0) characterScale.x *= -1;
        graphicTransform.localScale = characterScale;

        // check for collision, and apply movement if applicable
        RaycastHit2D hit;
        bool weAreColliding = raycaster.ThrowRays(dir, distance, out hit);

        // TODO : wall jump could be processed here
        //if (weAreColliding && isJumping)
        //    timeSinceBeganWallJump = 0;

        // Not colliding? It means we can finally apply the movement
        if (!weAreColliding)
        {
            transform.Translate(Vector3.right * distance);
            return true;
        }

        #region slope handling
        else
        {
            // was there a slope? if so, if the angle allows it, move up so we stay above the ground

            // store the horizontal distance to travel here, for the sake of clarity
            float horizontalDistance = Mathf.Abs(distance);

            // calculate slope angle and only move if it's low enough
            Vector2 movementVector = (Vector2.right * Mathf.Sign(distance));
            Vector2 surfaceNormal = hit.normal;
            float slopeAngle = Mathf.Abs(90f - Vector2.Angle(movementVector, surfaceNormal));
            if (slopeAngle < slopeMaxThreshold)
            {
                // calc the vertical displacement it would represent if it was performed
                float verticalDistance = Mathf.Abs(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * horizontalDistance);

                // try to perform vertical movement
                bool verticalMovementSucceeded = VerticalMove(verticalDistance);

                // if this vertical movement didn't hit the ceiling, we can perform the horizontal movement
                if (verticalMovementSucceeded)
                {
                    transform.Translate(Vector3.right * distance);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        #endregion
    }

    // positive = up, negative = down
    // returns whether the movement has been performed successfully
    public bool VerticalMove(float distance)
    {
        if (distance == 0)
        {
            return false;
        }

        // finding out movement direction
        MoveDirection dir = MoveDirection.Up;
        if (distance < 0) dir = MoveDirection.Down;

        // check for collision, and apply movement if applicable
        RaycastHit2D hit;
        bool weAreColliding = raycaster.ThrowRays(dir, distance, out hit);

        // Not colliding? It means we can finally apply the movement
        if (!weAreColliding)
        {
            transform.Translate(Vector3.up * distance);

            // if character is falling down, but not colliding with ground, it means it's not grounded
            if (dir == MoveDirection.Down)
            {
                isGrounded = false;
                //graphicAnimator.SetBool("IsGrounded", false);
            }

            return true;
        }

        // If character collided... 
        else
        {
            // ...and it's with something beneath its feet...
            if (dir == MoveDirection.Down)
            {
                // it means the character is now grounded.
                isGrounded = true;
                //graphicAnimator.SetBool("IsGrounded", true);
                isJumping = false;
                remainingAllowedJumps = maxAllowedJumps;
            }

            return false;
        }
    }
}
