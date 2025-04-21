using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class Bullet : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float crit;

    private AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && gameObject.tag == "AceSkill")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if(Random.value <= crit)
                    enemy.TakeDamage(damage*4.5f);// Example damage value
                else 
                    enemy.TakeDamage(damage*3f);
            }
        }
        if (other.CompareTag("Enemy") && gameObject.tag == "LuffySkill")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                float finalDamage = Random.value <= crit ? damage * 3.5f : damage * 2f;
                enemy.TakeDamage(finalDamage);

                float knockbackDistance = 8f;
                float moveTime = 0.8f;

                // Xác định hướng đẩy: Luffy bên trái -> đẩy qua phải | Luffy bên phải -> đẩy qua trái
                float direction = (other.gameObject.transform.localScale.x < 0) ? 1 : -1;

                Vector2 start = other.transform.position;
                Vector2 end = start + Vector2.right * direction * knockbackDistance;
        
                RaycastHit2D hit = Physics2D.Raycast(start, Vector2.right * direction, knockbackDistance, LayerMask.GetMask("Wall"));

                float targetX = (hit.collider != null) ? hit.point.x - direction * 0.5f : end.x;

                other.transform.DOMoveX(targetX, moveTime);
            }
        }

        if (other.CompareTag("Enemy") && gameObject.tag == "ZoroSkill")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                float finalDamage = Random.value <= crit ? damage * 3.5f : damage * 2f;
                enemy.TakeDamage(finalDamage);

                float moveTime = 0.5f;  // Tốc độ hút về

                // Hút enemy về vị trí hero (this.transform.position)
                other.transform.DOMoveX(transform.position.x, moveTime);
            }

        }



        else if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if(Random.value <= crit)
                  enemy.TakeDamage(damage*2f);// Example damage value
                else 
                  enemy.TakeDamage(damage);
            }
            
        }
    }
 
    private void UpEffect()
    {
        if (am != null)
        {
            am.PlaySFX(am.upClip);
        }
    }
    
}
