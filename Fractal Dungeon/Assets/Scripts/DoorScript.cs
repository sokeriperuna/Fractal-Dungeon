using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private DoorScript linkedDoor;

    private Transform teleportDestination;

    public int doorIteration = 0;

    private static bool playerHasTeleported = false;
    
    private static float doorAvailableNext = 0f;

    public float cooldown = 1f;
    
    public delegate void DoorDelegate();
    public static event  DoorDelegate PlayerAscent;
    public static event  DoorDelegate PlayerDescent;

    public void InitializeDoor(DoorScript newLink, int iteration)
    {
        linkedDoor          = newLink;
        doorIteration       = iteration;
        teleportDestination = newLink.transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (playerHasTeleported)
            playerHasTeleported = false;
    }

    public int Iteration { get { return doorIteration; } }

    public Transform Destintaion { get { return teleportDestination; } }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (doorAvailableNext <= Time.time && !playerHasTeleported)
        {
            if (this.doorIteration < linkedDoor.Iteration)
                if (PlayerDescent != null)
                    PlayerDescent();

            if (this.doorIteration > linkedDoor.Iteration)
                if (PlayerAscent != null)
                    PlayerAscent();



            if (other.CompareTag("Player"))
                other.transform.position = teleportDestination.position;
            doorAvailableNext = Time.time + cooldown;
            playerHasTeleported = true;
        }

    }
} 