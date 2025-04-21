using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private float destroyTime = 3f;
    private float currentTime = 0f;
    private Vector2 direction;
    public float speed ;
    public float damage ;

    public void Init(Vector2 dir)
    {
        direction = dir;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        ani.enabled = false;
    }

    void Update()
    {
        // Di chuyển đối tượng theo hướng và tốc độ
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerActive"))
        {
            HeroController hero = other.GetComponent<HeroController>();
            if (hero != null)
            {
                ani.enabled = true;
                hero.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("Wall"))
        {
            ani.enabled = true;
        }
        
    }

    public void Destroybullet()
    {
        Destroy(gameObject);
    }
    
}