using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerRefs
{
    public Transform    transform; 
    public PlayerEntity entity;
}

[RequireComponent(typeof(RoomManager))]
public class GameController : MonoBehaviour {

    private Camera           mainCamera;
    private CameraController cameraController;
    private PlayerRefs       playerRefs;
    private RoomManager      roomManager;

    public GameObject player;

    private void Awake()
    {
        InitializeGame();
        PlayerEntity.PlayerScaleIncrement += OnPlayerScaleChange;
        PlayerEntity.PlayerScaleDecrement += OnPlayerScaleChange;
    }

    private void Start()
    {
        StartGame();
    }

    private void InitializeGame()
    {
        mainCamera           = Camera.main;
        cameraController     = mainCamera.GetComponent<CameraController>();
        playerRefs.transform = player.transform;
        playerRefs.entity    = player.GetComponent<PlayerEntity>();
    }

    private void StartGame()
    {
        
    }

    private void OnPlayerScaleChange()
    {

    }
}