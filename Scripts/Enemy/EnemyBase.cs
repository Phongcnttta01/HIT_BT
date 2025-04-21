using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] public float maxHp = 1000f;

    [SerializeField] public float currentHp ;
    
    [SerializeField] public float damage = 50f;
    
    [SerializeField] public float attackSpeed = 1f;
    
    [SerializeField] public float attackRange = 5f;
    
    [SerializeField] public float moveSpeed = 5f;
}
