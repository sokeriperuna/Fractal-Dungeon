using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRefs
{
    public Transform   transform;

    public Transform[] corners                  = new Transform[4];
    public Transform[] doorSpawnPointEntrances  = new Transform[4];
    public Transform[] doorSpawnPointExits      = new Transform[4];
    public Transform[] doorSpawnPointLinks      = new Transform[4];

    List<Transform> existingDoors = new List<Transform>();
    public RoomData data;
}

public enum DOOR_SPAWN_ENTRANCE
{
    AX, BX, XB, YB
};

public enum DOOR_SPAWN_MIDDLE
{
    AB1, AB2, XY1, XY2
};

public struct RoomData
{
    public int iteration;
    public double scale;
}

public class RoomManager : MonoBehaviour {

    private int deepestIterationVisited = 3;

    public GameObject roomPrefab;
    public GameObject roomLinker;

    public float roomSize;

    public Transform rooms;

    public CameraController cameraController;

    List<RoomRefs> existingRooms;

    private void Awake()
    {
        existingRooms = new List<RoomRefs>();
        PlayerEntity.PlayerScaleDecrement += OnPlayerScaleChange;
        PlayerEntity.PlayerScaleIncrement += OnPlayerScaleChange;
    }

    private void Start()
    {
        for (int i = -1; i < 9; i++)
            SpawnNewRoom(i);

        OnPlayerScaleChange();
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
        existingRooms[lastIteration + 1].transform = tempObject.transform;

        for(int i = 0; i < 4; i++)
        {
            existingRooms[lastIteration + 1].corners[i] = existingRooms[lastIteration + 1].transform.GetChild(0).GetChild(i);

            existingRooms[lastIteration + 1].doorSpawnPointEntrances[i] = existingRooms[lastIteration + 1].transform.GetChild(1).GetChild(0).GetChild(i);
            existingRooms[lastIteration + 1].doorSpawnPointExits[i]     = existingRooms[lastIteration + 1].transform.GetChild(1).GetChild(1).GetChild(i); // FIX
            existingRooms[lastIteration + 1].doorSpawnPointLinks[i]     = existingRooms[lastIteration + 1].transform.GetChild(1).GetChild(2).GetChild(i); // FIX
        }


        existingRooms[lastIteration + 1].data.iteration = lastIteration + 1;
        if (lastIteration > -1)
            existingRooms[lastIteration + 1].data.scale = GetIterationScale(lastIteration + 1);
        else
            existingRooms[lastIteration + 1].data.scale = 1F;

        existingRooms[lastIteration + 1].transform.localPosition = new Vector3 (1.25f, 1.25f, 0f);
        existingRooms[lastIteration + 1].transform.localScale    = new Vector3 (0.5f, 0.5f, 1f);

        SpawnDoors(existingRooms[lastIteration + 1]);

        if (lastIteration >= 0)
            LinkRooms(existingRooms[lastIteration], existingRooms[lastIteration + 1]);
    }

    private void SpawnDoors(RoomRefs room)
    {
        // SPAWNING AX/BX

        int randomResult = Random.Range(0, 2);

        GameObject spawnedDoor1;
        GameObject spawnedDoor2;

        if (randomResult == 1)
            spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.AX].position, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.AX].rotation, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.AX]);
        else
            spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.BX].position, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.BX].rotation, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.BX]);

        randomResult = Random.Range(0, 2);

        if (randomResult == 1)
            spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.XB].position, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.XB].rotation, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.XB]);
        else
            spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.YB].position, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.YB].rotation, room.doorSpawnPointExits[(int)DOOR_SPAWN_ENTRANCE.YB]);


        // SPAWNING MIDDLE DOORS

        randomResult = Random.Range(0, 2);

        if (randomResult == 1)
        {
            spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1]);
            spawnedDoor2 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2]);
            LinkDoors(spawnedDoor1.GetComponent<DoorScript>(), spawnedDoor2.GetComponent<DoorScript>(), room, room);
        }
        else
        {
            randomResult = Random.Range(0, 2);
            if(randomResult == 1)
            {
                spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1]);
                spawnedDoor2 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2]);
                LinkDoors(spawnedDoor1.GetComponent<DoorScript>(), spawnedDoor2.GetComponent<DoorScript>(), room, room);
            }
            else
            {
                randomResult = Random.Range(0, 2);
                if (randomResult == 1)
                {
                    spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB1]);
                    spawnedDoor2 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.AB2]);
                    LinkDoors(spawnedDoor1.GetComponent<DoorScript>(), spawnedDoor2.GetComponent<DoorScript>(), room, room);
                }
                else
                {
                    spawnedDoor1 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY1]);
                    spawnedDoor2 = Instantiate(roomLinker, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2].position, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2].rotation, room.doorSpawnPointLinks[(int)DOOR_SPAWN_MIDDLE.XY2]);
                    LinkDoors(spawnedDoor1.GetComponent<DoorScript>(), spawnedDoor2.GetComponent<DoorScript>(), room, room);
                }
            }
        }
    }

    private void LinkDoors(DoorScript door1, DoorScript door2, RoomRefs room1, RoomRefs room2)
    {
        door1.InitializeDoor(door2, room1.data.iteration);
        door2.InitializeDoor(door1, room2.data.iteration);
    }

    private int FindTransformIndex(Transform[] array, Transform target)
    {
        for (int i = 0; i < array.Length; i++)
            if (Object.ReferenceEquals(array[i], target))
                return i;

        return -1;
    }

    private void LinkRooms(RoomRefs higherRoom, RoomRefs lowerRoom)
    {
        foreach (Transform i in higherRoom.doorSpawnPointExits)
            foreach(Transform j in i)
            {
                int doorIndex = FindTransformIndex(higherRoom.doorSpawnPointExits, i);
                GameObject k  = Instantiate(roomLinker, lowerRoom.doorSpawnPointEntrances[doorIndex].position, lowerRoom.doorSpawnPointEntrances[doorIndex].rotation, lowerRoom.doorSpawnPointEntrances[doorIndex]);
                LinkDoors(j.GetComponent<DoorScript>(), k.GetComponent<DoorScript>(), higherRoom, lowerRoom);
            }

    }

    private void IntializeRoom()
    {

    }

    public Transform[] GetRoomCorners(int roomIteraton)
    {
        Debug.Log("Room Start");
        if (roomIteraton < 0 || roomIteraton >= existingRooms.Count)
        {
            Debug.LogError("Incorrect iteration parameters");
            return null;
        }
        else
            foreach (Transform transform in existingRooms[roomIteraton].corners)
                Debug.Log(transform);
            return existingRooms[roomIteraton].corners;
    }

    public bool GetRoomCorner(int roomIndex, int cornerIndex, ref Transform outputCorner)
    {
        outputCorner = existingRooms[roomIndex].corners[cornerIndex];
        return true;
    }

    private void OnPlayerScaleChange()
    {
        if (deepestIterationVisited < PlayerEntity.currentIteration)
        {
            deepestIterationVisited = PlayerEntity.currentIteration;
            SpawnNewRoom(existingRooms.Count - 1);
        }

        cameraController.UpdateTracking(existingRooms[PlayerEntity.currentIteration].transform, PlayerEntity.currentIteration);
    }

    public static double GetIterationScale(int iteration) {
        if (iteration == 0)
            return 1F;
        else
            return Mathf.Pow(0.5f, iteration);
    }

}
