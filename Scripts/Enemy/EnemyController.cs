using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : EnemyBase
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator ani;
    private Transform playerTransform;

    [SerializeField] private float findPlayerDelay = 1f; 
    [SerializeField] private GameObject bulletPrefab;  // Gắn prefab EnemyBullet
    private float findPlayerTimer;
    [SerializeField] private Image hpBar;
    private AudioManager am;
    public bool isActive = false;
    
    private float originalMoveSpeed;
    private float originalAttackSpeed;
    private float originalDamage;
    private float originalMaxHp;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        am = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        // Lưu chỉ số gốc
        originalMoveSpeed = moveSpeed;
        originalAttackSpeed = attackSpeed;
        originalDamage = damage;
        originalMaxHp = maxHp;
    }

    private void OnEnable()
    {
        ResetEnemyState();
    }

    private void ResetEnemyState()
    {
        // Reset chỉ số gốc
        moveSpeed = originalMoveSpeed;
        attackSpeed = originalAttackSpeed;
        damage = originalDamage;
        maxHp = originalMaxHp;

        // Reset HP và thanh máu
        currentHp = maxHp;
        UpdateHpBar();

        // Reset animation state
        ani.SetBool("IsRun", false);
        ani.SetBool("IsAttack", false);
        ani.SetBool("IsDie", false);

        // Reset scale về mặc định
       

        // Reset target
        playerTransform = null;
        findPlayerTimer = 0;

        // Tìm player gần nhất nếu có
        FindNearestPlayer();
    }


    void Start()
    {
        //am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentHp = maxHp;
        UpdateHpBar();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        
        FindNearestPlayer();
    }

    void Update()
    {
        // if (ani.GetBool("IsAttack") )
        // {
        //     am.PlaySFX(am.fightClip);
        // }
        UpdateHpBar();
        if (playerTransform == null)
        {
            findPlayerTimer -= Time.deltaTime;
            ani.SetBool("IsRun", false);
            ani.SetBool("IsAttack", false);
        if (findPlayerTimer <= 0)
            {
                FindNearestPlayer();
                findPlayerTimer = findPlayerDelay;
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        FacePlayer();

        if (distance > attackRange)
        {
            ani.SetBool("IsRun", true);
            ani.SetBool("IsAttack", false);

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            ani.SetBool("IsRun", false);
            ani.SetBool("IsAttack", true);
            //AttackHit();  // Tấn công ngay khi Player ở trong phạm vi tấn công
        }

        if (currentHp <= 0)
        {
            ani.SetBool("IsDie", true);
        }
    }


    private void FacePlayer()
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerActive");

        float minDistance = Mathf.Infinity;
        Transform nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player.transform;
            }
        }

        playerTransform = nearestPlayer;
    }

    public void AttackHit()  // Gọi trong Animation Event Attack
    {
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            playerTransform.GetComponent<HeroController>().TakeDamage(damage);
        }
    }
    public void AttackHitBoss()  // Gọi trong Animation Event Attack
    {
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            playerTransform.GetComponent<HeroController>().TakeDamage(damage);
            //playerTransform.GetComponent<HeroController>().attackSpeed -= 0.3f;
            //playerTransform.GetComponent<HeroController>().damage-= 30f;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    private void UpdateHpBar()
    {
        hpBar.fillAmount = currentHp / maxHp;
    }

    public void SpawnBullet()
    {
        if (playerTransform == null) return;

        if (playerTransform != null)
        {

            // Spawn đạn tại vị trí của kẻ địch
            GameObject bullet = PoolingManager.Spawn(bulletPrefab, transform.position, Quaternion.identity);

            // Tính toán hướng từ kẻ địch đến Player
            Vector2 dir = (playerTransform.position - transform.position).normalized;

            // Cập nhật các thông số của đạn
            bullet.GetComponent<EnemyBullet>().Init(dir);
            bullet.GetComponent<EnemyBullet>().damage = damage;
            bullet.GetComponent<EnemyBullet>().speed = attackSpeed;
        }
    }

}
