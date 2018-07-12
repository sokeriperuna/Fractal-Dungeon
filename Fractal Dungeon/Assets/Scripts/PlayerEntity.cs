using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_DIRECTION
{
    RIGHT, UP, DOWN, LEFT
};

public enum PLAYER_ATTACK_TYPE
{
    MELEE
};

[RequireComponent(typeof(AudioSource))]
public class PlayerEntity : MonoBehaviour {

    private Rigidbody2D rb2D;

    private AudioSource audio;

    public Animator animator;

    private Collider2D[] playerAttackColliders = new Collider2D[4];

    private PLAYER_DIRECTION facingDirection = PLAYER_DIRECTION.DOWN;

    private float scaledSpeed;
    private float nextAttack    = 0;
    private float firstFrameEnd = 0;

    private SpriteRenderer spriteRenderer;

    private bool isAttacking = false;

    public float speed;

    public float attackCooldown = 0.1f;

    public float firstFrameDuration = 0.5f;

    public int attackDamage = 1;

    public AudioClip slash;

    public static int currentIteration;

    public delegate void PlayerDelegate();
    public static event  PlayerDelegate PlayerScaleIncrement;
    public static event  PlayerDelegate PlayerScaleDecrement;


    private void Awake()
    {
        rb2D     = GetComponent<Rigidbody2D>();
        audio    = GetComponent<AudioSource>();
        animator = GetComponent<Animator>   ();

        currentIteration = 0;
        scaledSpeed      = speed;
        DoorScript.PlayerAscent  += OnPlayerAscent;
        DoorScript.PlayerDescent += OnPlayerDescent;
    }

    private void Update()
    {
        if(isAttacking)
        {
            if (nextAttack <= Time.time)
                isAttacking = false;
            else
            {
                if (firstFrameEnd <= Time.time)
                    animator.Play(GetAttackFrameNames(this.Facing, true));
                else
                    animator.Play(GetAttackFrameNames(this.Facing, false));
            }
        }
    }

    private void Start()
    {
        currentIteration = 0;
        nextAttack = 0f;
    }

    private PLAYER_DIRECTION GetDirection(Vector2 vector)
    {
        if (vector.x == 1f)
            return PLAYER_DIRECTION.RIGHT;

        if (vector.x == -1f)
            return PLAYER_DIRECTION.LEFT;

        if (vector.y == 1f)
            return PLAYER_DIRECTION.UP;

        if (vector.y == -1f)
            return PLAYER_DIRECTION.DOWN;

        Debug.LogError("Incorrect vector input");
        return PLAYER_DIRECTION.DOWN;
    }

    private string GetWalkAnimationName(PLAYER_DIRECTION dir)
    {
        switch (dir)
        {
            case PLAYER_DIRECTION.DOWN:
                return "yui_walking_down";

            case PLAYER_DIRECTION.LEFT:
                return "yui_walking_left";

            case PLAYER_DIRECTION.RIGHT:
                return "yui_walking_right";

            case PLAYER_DIRECTION.UP:
                return "yui_walking_up";

            default:

                return "yui_stand_down";
        }
    }

    public string GetStandAnimationName(PLAYER_DIRECTION dir)
    {
        switch (dir)
        {
            case PLAYER_DIRECTION.DOWN:
                return "yui_stand_down";

            case PLAYER_DIRECTION.LEFT:
                return "yui_stand_left";

            case PLAYER_DIRECTION.RIGHT:
                return "yui_stand_right";

            case PLAYER_DIRECTION.UP:
                return "yui_stand_up";

            default:

                return "yui_stand_down";
        }
    }

    private string GetAttackAnimationName(PLAYER_DIRECTION dir)
    {
        switch (dir)
        {
            case PLAYER_DIRECTION.DOWN:
                return "yui_attack_down";

            case PLAYER_DIRECTION.LEFT:
                return "yui_attack_left";

            case PLAYER_DIRECTION.RIGHT:
                return "yui_attack_right";

            case PLAYER_DIRECTION.UP:
                return "yui_attack_up";

            default:

                return "yui_stand_down";
        }
    }

    void OnPlayerAscent()
    {
        UpdatePlayerScale(currentIteration - 1, true);
    }

    void OnPlayerDescent()
    {
        UpdatePlayerScale(currentIteration + 1, false);
    }

    public void UpdatePlayerScale(int newIteration, bool incremented)
    {
        currentIteration = newIteration;

        if (incremented)
            if (PlayerScaleIncrement != null)
                PlayerScaleIncrement();

        if (!incremented)
            if (PlayerScaleDecrement != null)
                PlayerScaleDecrement();

        double iterationScale  = RoomManager.GetIterationScale(newIteration);
        scaledSpeed = (float)(speed * iterationScale);

        transform.localScale = new Vector3((float)iterationScale * 5500f, (float)iterationScale * 5500f, 1f);

    }

    public void Move(Vector2 input)
    {
        facingDirection = GetDirection(input);

        if (!this.Attacking)
        {
            animator.Play(GetWalkAnimationName(facingDirection));
            rb2D.MovePosition(rb2D.position + input.normalized * Time.fixedDeltaTime * scaledSpeed);
        }

    }

    private string GetAttackFrameNames(PLAYER_DIRECTION dir, bool firstFrame)
    {
        if (!firstFrame)
        {
            switch (dir)
            {
                case PLAYER_DIRECTION.DOWN:
                    return "yui_attack_down_0";

                case PLAYER_DIRECTION.LEFT:
                    return "yui_attack_left_0";

                case PLAYER_DIRECTION.RIGHT:
                    return "yui_attack_right_0";

                case PLAYER_DIRECTION.UP:
                    return "yui_attack_up_0";

                default:

                    return "yui_stand_down";
            }
        }
        else
        {
            switch (dir)
            {
                case PLAYER_DIRECTION.DOWN:
                    return "yui_attack_down_1";

                case PLAYER_DIRECTION.LEFT:
                    return "yui_attack_left_1";

                case PLAYER_DIRECTION.RIGHT:
                    return "yui_attack_right_1";

                case PLAYER_DIRECTION.UP:
                    return "yui_attack_up_1";

                default:

                    return "yui_stand_down";
            }
        }

    }

    public void Attack()
    {
        audio.clip = slash;
        audio.Play();

        isAttacking   = true;
        firstFrameEnd = (attackCooldown * firstFrameDuration) + Time.time;
        nextAttack    =  attackCooldown + Time.time;
    }

    public float NextAttack        { get { return nextAttack;  } }

    public bool Attacking          { get { return isAttacking; } }

    public PLAYER_DIRECTION Facing { get { return facingDirection;  } }
}
