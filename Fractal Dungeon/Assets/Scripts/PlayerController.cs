using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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