using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyEntity))]
public class EnemyController : MonoBehaviour {


    EnemyEntity enemy;

    void Awake()
    {
        enemy = GetComponent<EnemyEntity>();
    }

    void FixedUpdate()
    {
        enemy.Move(PlayerEntity.playerPosition);
    }
}
