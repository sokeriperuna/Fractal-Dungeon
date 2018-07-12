using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
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

        if (Input.GetKeyDown(KeyCode.Space) && player.NextAttack <= Time.time)
            player.Attack();
        else
        if (input != Vector2.zero)
            player.Move(input);
        else
        {
            player.animator.Play(player.GetStandAnimationName(player.Facing));
        }
    }

    private Vector2 GetInput()
    {
        if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetAxisRaw("Horizontal") == -1f) && (Input.GetAxisRaw("Vertical") == 1f || Input.GetAxisRaw("Vertical") == -1f))
            return Vector2.zero;
        else
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

}