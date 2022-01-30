using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionInput : MonoBehaviour
{
    // Parameters
    // Cached
    private MovementInput movementComponent;
    private List<GameObject> currentCollisions = new List<GameObject>();
    // State

    private void Start()
    {
        movementComponent = gameObject.GetComponent<MovementInput>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject currentCollision = collision.gameObject;
        if (currentCollision.gameObject.tag == "ChangeDirectionObj")
        {
            if (!CheckIfExists(currentCollision))
            {
                currentCollisions.Add(currentCollision);
                movementComponent.HandleCollision();
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject currentCollision = collision.gameObject;
        if (currentCollision.gameObject.tag == "ChangeDirectionObj")
        {
            if (CheckIfExists(currentCollision))
            {
                currentCollisions.Remove(currentCollision);
            }
        }
    }

    // TODO :
    // Handle Collisions that enter and leave
    // Use Collisions to Handle Rotations and Flips

    private bool CheckIfExists(GameObject currentGO)
    {
        if (currentCollisions.Where(cl => cl.Equals(currentGO)).FirstOrDefault() != null) return true;
        else return false;
    }
}
