using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private DoorScript linkedDoor;

    private Transform teleportDestination;

    private int doorIteration = 0;

    public delegate void DoorDelegate();
    public static event DoorDelegate PlayerDecent;
    public static event DoorDelegate PlayerAscent;

    private void Awake()
    {
        teleportDestination = transform.GetChild(0);
    }

    public void InitializeDoor(DoorScript newLink, int iteration)
    {
        linkedDoor    = newLink;
        doorIteration = iteration;
    }

    public int Iteration { get { return doorIteration; } }

    public Transform Destintaion { get { return teleportDestination; } }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
            Debug.Log("TELEPORT.");

        if (linkedDoor.Iteration > this.Iteration)
            if (PlayerAscent != null)
                PlayerAscent();

        if (linkedDoor.Iteration < this.Iteration)
            if (PlayerDecent != null)
                PlayerDecent();


            //other.transform.position = linkedDoor.teleportDestination.position;
    }
} 