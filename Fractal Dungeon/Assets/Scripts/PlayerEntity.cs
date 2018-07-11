using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {

    private Rigidbody2D rb2D;

    private float scaledSpeed;

    public float speed;

    public static int currentIteration;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate PlayerScaleIncrement;
    public static event PlayerDelegate PlayerScaleDecrement;

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

    void OnPlayerAscent()
    {
        UpdatePlayerScale(currentIteration + 1, true);
    }

    void OnPlayerDescent()
    {
        UpdatePlayerScale(currentIteration - 1, false);
    }

    public void UpdatePlayerScale(int newIteration, bool incremented)
    {
        if (incremented)
            if (PlayerScaleIncrement != null)
                PlayerScaleIncrement();

        if (!incremented)
            if (PlayerScaleDecrement != null)
                PlayerScaleDecrement();


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
