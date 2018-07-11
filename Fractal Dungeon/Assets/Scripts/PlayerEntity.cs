using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {

    private Rigidbody2D rb2D;

    private float scaledSpeed;

    public float speed;

    public static int currentIteration;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerScaleIncrement;
    public static event PlayerDelegate OnPlayerScaleDecrement;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentIteration = 0;
        scaledSpeed      = speed;
    }

    private void Start()
    {
        currentIteration = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            UpdatePlayerScale(++currentIteration, true);
        if (Input.GetKeyDown(KeyCode.Mouse1))
            UpdatePlayerScale(--currentIteration, false);
    }

    public void UpdatePlayerScale(int newIteration, bool incremented)
    {
        if (incremented)
            if (OnPlayerScaleIncrement != null)
                OnPlayerScaleIncrement();

        if (!incremented)
            if (OnPlayerScaleDecrement != null)
                OnPlayerScaleDecrement();


        currentIteration       = newIteration;
        double iterationScale  = RoomManager.GetIterationScale(newIteration);
        scaledSpeed = (float)(speed * iterationScale);

        transform.localScale = new Vector3((float)iterationScale, (float)iterationScale, 1f);

    }

    public void MovePlayer(Vector2 input)
    {
        rb2D.MovePosition(rb2D.position + input.normalized * Time.fixedDeltaTime * scaledSpeed);
    }
}
