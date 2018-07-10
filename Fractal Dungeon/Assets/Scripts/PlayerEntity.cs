using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {

    private Vector2 velocity;
    private Rigidbody2D rb2D;

    public float speed;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }



    public void MovePlayer(Vector2 input)
    {
        rb2D.MovePosition(rb2D.position + input.normalized * Time.fixedDeltaTime);
    }
}
