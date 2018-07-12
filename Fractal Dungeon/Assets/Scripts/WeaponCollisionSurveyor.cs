using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionSurveyor : MonoBehaviour {

    public PlayerEntity player;

    public delegate void CollisionDelegate();
    public event CollisionDelegate EnemyDetect;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        CheckForEnemy(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stay");
        CheckForEnemy(other);
    }

    void CheckForEnemy(Collider2D input)
    {
        Debug.Log(player.Attacking.ToString()); 
        if (input.CompareTag("Enemy") && player.Attacking)
        {
            if (EnemyDetect != null)
                EnemyDetect();
            input.GetComponent<EnemyEntity>().TakeDamage(player.attackDamage);
        }
    }
}
