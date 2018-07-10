using System.Collections;
using UnityEngine;

<<<<<<< HEAD:Fractal Dungeon/Assets/Scripts/PlayerController.cs
public class PlayerController : MonoBehaviour
{
=======
public class CameraController : MonoBehaviour {
>>>>>>> esko:Fractal Dungeon/Assets/Scripts/CameraController.cs

    private PlayerEntity player;

    private void Awake()
    {
        player = GetComponent<PlayerEntity>();
    }

    private void FixedUpdate()
    {
        Vector2 input = GetInput();
        if (input != Vector2.zero)
            player.MovePlayer(input);
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

}