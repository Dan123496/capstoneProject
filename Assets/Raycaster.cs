using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

// RequireComponent makes it so when the component gets placed into an object, a BoxCollider2D gets added as well.
// It also prevents removing the BoxCollider2D as long as this component is still on the object.
// It's an extra safety for whoever works with your scripts in Unity.
[RequireComponent(typeof(BoxCollider2D))]
public class Raycaster : MonoBehaviour
{
    // Makes it into a slider in the Inspector
    [Range(2, 10)]
    public int accuracyLevel = 3;
    public float skinWidth = 0.01f;
    [Range(0f, 1f)]
    public float extraDistanceRatioFromCorners = 0.02f;

    // Just a reference to its component, as usual
    public BoxCollider2D selfBox;

    public LayerMask affectedLayers1;

    // We're going to need the exact coordinates of every corner.
    // We could just use references to empty Transforms...
    // ...but instead we're going to calculate them using the BoxCollider2D.
    //public Transform lowerLeftCorner, lowerRightCorner, upperLeftCorner, upperRightCorner;

    // Extra safety: automatically assign the component if we forgot to do the drag-and-drop
    void Start()
    {
        if (selfBox == null)
            selfBox = GetComponent<BoxCollider2D>();
    }

    // Main function to be called by our future movement script.
    // Returns "true" if we're hitting something, "false" otherwise.
    // The "out" keyword allows us to transfer data from the raycast to other scripts.
    public bool ThrowRays(MoveDirection moveDirection, float rayDistance, out RaycastHit2D hit)
    {
        bool result = false;

        // Every ray will have the same direction
        Vector2 rayDirection = Vector2.zero;
        if (moveDirection == MoveDirection.Up) rayDirection = Vector2.up;
        if (moveDirection == MoveDirection.Down) rayDirection = Vector2.down;
        if (moveDirection == MoveDirection.Left) rayDirection = Vector2.left;
        if (moveDirection == MoveDirection.Right) rayDirection = Vector2.right;

        // We setup a loop : each iteration, we throw a different ray.
        for (int i = 0; i < accuracyLevel; i++)
        {
            // Each ray has a different origin (but the same direction).
            // Given the direction and the current iteration index (i), we calculate the current origin.
            Vector2 rayOrigin = CalculateRayOrigin(moveDirection, i);

            // Apply the skin width so we don't collide with our own box
            if (moveDirection == MoveDirection.Up) rayOrigin.y += skinWidth;
            if (moveDirection == MoveDirection.Down) rayOrigin.y -= skinWidth;
            if (moveDirection == MoveDirection.Left) rayOrigin.x -= skinWidth;
            if (moveDirection == MoveDirection.Right) rayOrigin.x += skinWidth;

            // Actual Raycast call, and at the same time we call Debug.DrawRay to see it in the viewport.
            RaycastHit2D rayResult = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, affectedLayers1);
            Debug.DrawRay(rayOrigin, rayDirection, Color.black, 0.3f);
            
            // If we hit something, we return true.
            if (rayResult.collider != null)
            {
                Debug.Log(rayResult.collider.name);
                result = true;
                
                // we can also "break;" here because once we hit something, we're no longer interested in processing extra rays
                // that's a nice practice if we're being picky about performance, since it avoids useless rays.
                
                hit = rayResult;
                
                return true;
            }        
        }

        hit = new RaycastHit2D();

        return result;
    }

    // this function calculates the exact origin of any ray, given its direction and index
    Vector2 CalculateRayOrigin(MoveDirection dir, int rayIndex)
    {
        Vector2 result = transform.position;

        // step 1 : calculate the two relevant corners
        Vector2 firstCorner = Vector2.zero;
        Vector2 lastCorner = Vector2.zero;

        if (dir == MoveDirection.Down)
        {
            firstCorner = GetSpecificCorner(false, false); // lower left
            firstCorner.x += 0.5f * selfBox.size.x * transform.localScale.x * extraDistanceRatioFromCorners;

            lastCorner = GetSpecificCorner(false, true); // lower right
            lastCorner.x -= 0.5f * selfBox.size.x * transform.localScale.x * extraDistanceRatioFromCorners;
            

        }
        else if (dir == MoveDirection.Up)
        {
            firstCorner = GetSpecificCorner(true, false); // upper left
            firstCorner.x += 0.5f * selfBox.size.x * transform.localScale.x * extraDistanceRatioFromCorners;

            lastCorner = GetSpecificCorner(true, true); // upper right
            lastCorner.x -= 0.5f * selfBox.size.x * transform.localScale.x * extraDistanceRatioFromCorners;
        }
        else if (dir == MoveDirection.Left)
        {
            firstCorner = GetSpecificCorner(false, false); // lower left
            firstCorner.y += 0.5f * selfBox.size.y * transform.localScale.y * extraDistanceRatioFromCorners;

            lastCorner = GetSpecificCorner(true, false); // upper left
            lastCorner.y -= 0.5f * selfBox.size.y * transform.localScale.y * extraDistanceRatioFromCorners;
        }
        else if (dir == MoveDirection.Right)
        {
            firstCorner = GetSpecificCorner(true, true); // upper right
            firstCorner.y -= 0.5f * selfBox.size.y * transform.localScale.y * extraDistanceRatioFromCorners;

            lastCorner = GetSpecificCorner(false, true); // lower right
            lastCorner.y += 0.5f * selfBox.size.y * transform.localScale.y * extraDistanceRatioFromCorners;
        }

        // step 2 : find the "position ratio", based on the "i" value
        // warning: we need to cast all the ints to floats, so the result does not get rounded
        float positionRatio = (float)rayIndex / (float)(accuracyLevel-1f);

        // step 3 : weighted average of the two corners, knowning the position ration
        // These two lines do exactly the same
        result = firstCorner * positionRatio + lastCorner * (1-positionRatio);
        result = Vector2.Lerp(firstCorner, lastCorner, positionRatio);

        return result;
    }

    // Calculating the position of a corner.
    Vector2 GetSpecificCorner(bool up, bool right)
    {
        // Use the object's position as a starting point
        Vector2 result = transform.position;

        float invertX = 1;
        float invertY = 1;
        if (!right) invertX = -1;
        if (!up) invertY = -1;

        //result.x += (selfBox.size.x * 0.5f * invertX + selfBox.offset.x) * transform.lossyScale.x;
        
        // Let's break this down into smaller steps:
        result.x += selfBox.offset.x; // apply offset
        result.x += selfBox.size.x * 0.5f * invertX; // add or remove half the size
        result.x *= transform.localScale.x; // apply scale to everything

        // Then we do the same for Y.
        result.y += (selfBox.size.y * 0.5f * invertY + selfBox.offset.y) * transform.localScale.y;

        // lossyScale refers to "world scale", not just relative to direct parent

        return result;
    }
}
