using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour {

    private int hitPoints;

    private Animator animator;

    private Rigidbody2D rb2D;

    private AudioSource audio;

    public int monsterIteration = 0;

    public float recoilImpulseForce = 5f;

    public float monsterChaseRadius = 1;

    public float defaultMonsterMass = 1;

    public float monsterSpeed = 1;

    public int startingHitPoints = 5;

    public int monsterDamage = 1;

    public AudioClip monsterHurt;

    public delegate void EnemyDelegate();
    public event EnemyDelegate enemyKilled;

    private void Awake()
    {
        animator = GetComponent<Animator>   ();
        rb2D     = GetComponent<Rigidbody2D>();
        audio    = GetComponent<AudioSource>(); 
    }

    private void Start()
    {
        hitPoints = startingHitPoints;
    }

    private void Initialize() {

        rb2D.mass = defaultMonsterMass * (float)RoomManager.GetIterationScale(monsterIteration);
    }

    private bool playerInRadius()
    {
        return true;
    }

    public void Move(Vector2 playerPosition)
    {
        animator.Play("green_guy_shuffle");

        if ((monsterIteration == PlayerEntity.currentIteration) && playerInRadius())
        {
            Vector2 direction = (PlayerEntity.playerPosition - transform.position).normalized;
            Debug.Log(direction.ToString());
            Debug.Log(direction * Time.fixedDeltaTime * monsterSpeed * (float)RoomManager.GetIterationScale(monsterIteration));
            rb2D.AddForce(direction * Time.fixedDeltaTime * monsterSpeed * (float)RoomManager.GetIterationScale(monsterIteration));
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy took: " + damage.ToString() + " point(s) of damage!");

        audio.clip = monsterHurt;
        audio.Play();

        hitPoints -= damage;
        if (hitPoints <= 0)
            Die();

        Vector2 direction = (PlayerEntity.playerPosition - transform.position).normalized;
        rb2D.AddForce(direction * (float)RoomManager.GetIterationScale(monsterIteration) * recoilImpulseForce * 1.25f, ForceMode2D.Impulse);

    }

    private void Die()
    {
        if (enemyKilled != null)
            enemyKilled(); 

        Debug.Log("Enemy died!");

        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (PlayerEntity.playerPosition - transform.position).normalized;
            rb2D.AddForce(direction * (float)RoomManager.GetIterationScale(monsterIteration) * recoilImpulseForce, ForceMode2D.Impulse);
            coll.gameObject.GetComponent<PlayerEntity>().TakeDamage(monsterDamage);
        }
    }
}
