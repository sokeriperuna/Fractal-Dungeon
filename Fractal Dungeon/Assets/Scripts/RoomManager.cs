using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomRefs
{
    public Transform transform;
    public RoomData data;
}

public struct RoomData
{
    int iteration;
}

public class RoomManager : MonoBehaviour {

    public GameObject roomPrefab;
    public GameObject roomLinker;

    List<RoomRefs> existingRooms;

    private void Awake()
    {
        existingRooms = new List<RoomRefs>();
    }


    private void SpawnNewRoom()
    {
        GameObject tempObject = null;
        if (existingRooms.Count > 0)
            tempObject = GameObject.Instantiate(roomPrefab, Vector3.zero/*Replace Vector3.zero with a proper value*/, Quaternion.identity, existingRooms[existingRooms.Count - 1].transform);
        else
            tempObject = GameObject.Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        existingRooms.Add(new RoomRefs());
        int i = 0;
        //existingRooms[i];
    }

    private void IntializeRoom()
    {

    }
}
