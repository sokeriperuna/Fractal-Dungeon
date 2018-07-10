using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public DoorScript linkedDoor;

    public Transform teleportdestination;


    void OnTriggerEnter2D(Collider2D _c)
    {
        if (_c.tag == "Player")
            Debug.Log("Teleport!!");
            //transform.position = teleportdestination.position;
    }
} 