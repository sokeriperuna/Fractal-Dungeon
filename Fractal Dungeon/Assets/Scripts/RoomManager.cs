using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRefs
{
    public Transform   transform;
    public Transform[] corners = new Transform[4];
    public RoomData    data;
}

public struct RoomData
{
    public int iteration;
    public double scale;
}

public class RoomManager : MonoBehaviour {

    public GameObject roomPrefab;
    public GameObject roomLinker;

    public float roomSize;

    public Transform rooms;

    List<RoomRefs> existingRooms;

    private void Awake()
    {
        existingRooms = new List<RoomRefs>();
    }

    private void Start()
    {
        for (int i = -1; i < 9; i++)
            SpawnNewRoom(i);
    }

    private void SpawnNewRoom(int lastIteration)
    {
        if (lastIteration < -1 || lastIteration >= existingRooms.Count)
            Debug.LogError("Incorrect lastIteration: " + lastIteration.ToString());
            

        GameObject tempObject = null;
        if (lastIteration < 0)
            tempObject = GameObject.Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, rooms);
        else
            tempObject = GameObject.Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, existingRooms[lastIteration].transform);

        existingRooms.Add(new RoomRefs());
        existingRooms[lastIteration + 1].transform      = tempObject.transform;

        for(int i = 0; i < 4; i++)
        {
            existingRooms[lastIteration + 1].corners[i] = existingRooms[lastIteration + 1].transform.GetChild(0);
            Debug.Log(existingRooms[lastIteration + 1].corners[i]);
        }

        existingRooms[lastIteration + 1].data.iteration = lastIteration + 1;
        if (lastIteration > -1)
            existingRooms[lastIteration + 1].data.scale = GetIterationScale(lastIteration + 1);
        else
            existingRooms[lastIteration + 1].data.scale = 1F;

        existingRooms[lastIteration + 1].transform.localPosition = new Vector3(1.25f, 1.25f, 0f);
        existingRooms[lastIteration + 1].transform.localScale    = new Vector3(0.5f, 0.5f, 1f);
    }

    private void IntializeRoom()
    {

    }

    public RoomRefs GetRoomRefs(int index)
    {
        if (index < 0 || index >= existingRooms.Count)
        {
            Debug.LogError("Incorrect Index");
            return null;
        }
        return existingRooms[index];
    }

    public static double GetIterationScale(int iteration) {
        if (iteration == 0)
            return 1F;
        else
            return (1F / (2F * iteration));
    }

}
