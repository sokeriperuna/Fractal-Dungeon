using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour {

    private int hitPoints;

    public int startingHitPoint;

    public delegate void EnemyDelegate();
    public event EnemyDelegate enemyKilled;

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy took: " + damage.ToString() + " point(s) of damage!");

        hitPoints -= damage;
        if (hitPoints <= 0)
            Die();

    }

    private void Die()
    {
        if (enemyKilled != null)
            enemyKilled(); 

        Debug.Log("Enemy died!");

        Destroy(this.gameObject);
    }
}
