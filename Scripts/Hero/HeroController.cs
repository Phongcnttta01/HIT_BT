using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HeroController : HeroBase
{
    //----------------- Drag & Drop -----------------//
    [Header("Drag & Drop")]
    private Vector3 offset;
    private Vector3 originalPos;
    private Vector3 lastSlotPos;
    private Transform targetSlot;
    private Transform playerTarget;
    private Transform attackTarget;
    private Transform currentPos;
    [SerializeField] private bool isDragging = false;

    //----------------- Components -----------------//
    [Header("Component")]
    private BoxCollider2D myCollider;
    private SpriteRenderer sr;
    public Animator ani;
    private Rigidbody2D rb;

    //----------------- Status Flags -----------------//
    [Header("Status Flags")]
    [SerializeField] public bool isFighting = false;
    [SerializeField] public bool isInBox = false;
    [SerializeField] public bool isInSpawnBox = false;
    [SerializeField] public bool isUpgrade = false;

    //----------------- Hero Stats -----------------//
    [Header("Hero Stats")]
    [SerializeField] public int ID;
    [SerializeField] private float originalMoveSpeed;
    [SerializeField] private float originalAttackRange;
    [SerializeField] private float originalDamage;
    [SerializeField] private float originalMaxHp;

    //----------------- UI -----------------//
    [Header("UI")]
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBorder;
    [SerializeField] private Image manaBar;

    //----------------- Mana -----------------//
    [Header("Mana System")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] public float currentMana;
    [SerializeField] private float manaRegenRate = 20f;
    private bool canUseSkill = false;

    //----------------- Prefabs / Effect -----------------//
    [Header("Prefab & VFX")]
    [SerializeField] private GameObject aceBulletPrefab;
    [SerializeField] private GameObject aceSkillPrefab;
    [SerializeField] private GameObject halo;
    private AudioManager am;

    //----------------- Enemy / Target -----------------//
    [Header("Enemy / Target")]
    private GameObject[] _enemies;

    void Awake()
    {
        // Khởi tạo các thành phần
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();

        // Đảm bảo collider tồn tại
        if (myCollider == null)
        {
            myCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        myCollider.isTrigger = true; // Đặt là trigger để kéo thả
        if (sr.sprite != null)
        {
            myCollider.size = sr.sprite.bounds.size;
        }
        else
        {
            myCollider.size = new Vector2(1f, 1f); // Kích thước mặc định nếu sprite chưa tải
            Debug.LogWarning("SpriteRenderer không có sprite, sử dụng kích thước collider mặc định.");
        }

        // Thiết lập Rigidbody
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }

    void Start()
    {
        //am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        isFighting = false;
        originalPos = transform.position;
        lastSlotPos = originalPos;
        currentPos = transform;

        if (hpBar != null)
        {
            hpBar.gameObject.SetActive(false);
            hpBorder.gameObject.SetActive(false);
            currentHp = maxHp;
            UpdateHpBar();
        }
        

        if (Mathf.Approximately(gameObject.transform.localScale.x, 1.2f))
        {
            isUpgrade = true;
            ID += 10;
        }

        ApplyUpgradeScaling();
        CheckInitialCollisions();

        currentMana = 0f;
        if (isUpgrade && manaBar != null)
        {
            manaBar.gameObject.SetActive(true);
            if (halo != null) halo.SetActive(true);
            UpdateManaBar();
        }
        else if (manaBar != null)
        {
            manaBar.gameObject.SetActive(false);
        }
    }

    private void ApplyUpgradeScaling()
    {
        if (isUpgrade)
        {
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            moveSpeed *= 1.2f;
            attackRange *= 1.2f;
            damage *= 2f;
            maxHp *= 2f;
            crit *= 2f;
            currentHp = maxHp;
            if (halo != null) halo.SetActive(true);
        }
    }

    private void CheckInitialCollisions()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, myCollider.size, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("MapBox"))
            {
                isInSpawnBox = true;
                targetSlot = hit.transform;
                Debug.Log($"Khởi tạo: Ở trong MapBox tại {hit.transform.position}");
            }
            else if (hit.CompareTag("HeroBox"))
            {
                isInBox = true;
                targetSlot = hit.transform;
                Debug.Log($"Khởi tạo: Ở trong HeroBox tại {hit.transform.position}");
            }
            else if (hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
                Debug.Log($"Khởi tạo: Phát hiện Player tại {hit.transform.position}");
            }
        }
    }

    private void Update()
    {
        // if (ani.GetBool("IsAttack") || ani.GetBool("IsSkill"))
        // {
        //     am.PlaySFX(am.fightClip);
        // }
        myCollider.size = sr.bounds.size;
        if (isUpgrade && halo != null)
        {
            halo.SetActive(true);
        }

        if (isFighting)
        {
            gameObject.tag = "PlayerActive";
            if (hpBar != null && hpBorder != null)
            {
                hpBar.gameObject.SetActive(true);
                hpBorder.gameObject.SetActive(true);
                UpdateHpBar();
            }

            if (!isDragging)
            {
                AutoFindEnemy();
            }

            if (isUpgrade && isFighting)
            {
                RegenerateMana();
            }
        }
        else
        {
            HandleDragDrop();
            if (hpBar != null && hpBorder != null)
            {
                hpBar.gameObject.SetActive(false);
                hpBorder.gameObject.SetActive(false);
            }
        }

        if (manaBar != null && !isFighting)
        {
            manaBar.gameObject.SetActive(false);
        }

        if (currentHp <= 0)
        {
            ani.SetBool("IsDie", true);
        }
    }

    private void HandleDragDrop()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (myCollider.OverlapPoint(mousePos))
            {
                isDragging = true;
                offset = transform.position - (Vector3)mousePos;
                Debug.Log($"Bắt đầu kéo tại {transform.position}");
            }
            else
            {
                Debug.Log($"Chuột tại {mousePos} không chạm collider tại {transform.position}");
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = (Vector3)mousePos + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            //am.PlaySFX(am.dragClip);
            isDragging = false;
            Debug.Log($"Thả tại {transform.position}");

            // Kiểm tra va chạm với box tại vị trí thả
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, myCollider.size, 0f);
            Transform newTargetSlot = null;
            Transform newPlayerTarget = null;

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("MapBox"))
                {
                    newTargetSlot = hit.transform;
                    isInSpawnBox = true;
                    isInBox = false;
                    Debug.Log($"Thả: Phát hiện MapBox tại {hit.transform.position}");
                }
                else if (hit.CompareTag("HeroBox"))
                {
                    newTargetSlot = hit.transform;
                    isInBox = true;
                    isInSpawnBox = false;
                    Debug.Log($"Thả: Phát hiện HeroBox tại {hit.transform.position}");
                }
                else if (hit.CompareTag("Player") && hit.transform != transform)
                {
                    newPlayerTarget = hit.transform;
                    Debug.Log($"Thả: Phát hiện Player tại {hit.transform.position}");
                }
            }

            if (newPlayerTarget != null) // Swap Hero
            {
                HeroController otherHero = newPlayerTarget.GetComponent<HeroController>();
                if (otherHero != null)
                {
                    Vector3 tempPos = newPlayerTarget.position;
                    transform.position = tempPos;
                    otherHero.transform.position = lastSlotPos;
                    (otherHero.lastSlotPos, lastSlotPos) = (lastSlotPos, otherHero.lastSlotPos);
                    currentPos.position = transform.position;
                    Debug.Log($"Hoán đổi với hero tại {tempPos}");
                }
            }
            else if (newTargetSlot != null) // Drop vào box
            {
                transform.position = newTargetSlot.position;
                lastSlotPos = newTargetSlot.position;
                currentPos.position = transform.position;
                targetSlot = newTargetSlot;
                Debug.Log($"Đính vào box tại {newTargetSlot.position}");
            }
            else // Drop thất bại
            {
                transform.position = lastSlotPos;
                currentPos.position = transform.position;
                Debug.Log($"Không tìm thấy box, quay lại {lastSlotPos}");
            }

            targetSlot = null;
            playerTarget = null;
        }
    }

    private void AutoFindEnemy()
    {
        if (!isFighting) return;

        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Kiểm tra nếu attackTarget không hợp lệ (null, bị phá hủy, hoặc ra khỏi phạm vi)
        if (attackTarget == null || !attackTarget.gameObject.activeInHierarchy || 
            Vector3.Distance(transform.position, attackTarget.position) > attackRange * 1.5f)
        {
            attackTarget = FindClosestEnemy(); // Tìm mục tiêu mới nếu không hợp lệ
        }

        if (_enemies.Length > 0 && attackTarget != null)
        {
            FlipToTarget(attackTarget);
            float distance = Vector3.Distance(transform.position, attackTarget.position);

            if (distance > attackRange)
            {
                ani.SetBool("IsRun", true);
                ani.SetBool("IsAttack", false);
                ani.SetBool("IsSkill", false);
                Vector2 direction = (attackTarget.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                ani.SetBool("IsRun", false);
                rb.velocity = Vector2.zero;

                if (isUpgrade && canUseSkill)
                {
                    ani.SetBool("IsSkill", true);
                    ani.SetBool("IsAttack", false);
                    UseSkill();
                }
                else
                {
                    ani.SetBool("IsSkill", false);
                    ani.SetBool("IsAttack", true);
                    ani.speed = attackSpeed;
                }
            }
        }
        else
        {
            // Không có kẻ thù hoặc mục tiêu không hợp lệ
            attackTarget = null;
            ani.SetBool("IsRun", false);
            ani.SetBool("IsAttack", false);
            ani.SetBool("IsSkill", false);
            rb.velocity = Vector2.zero;
        }
    }

    private Transform FindClosestEnemy()
    {
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    private void FlipToTarget(Transform target)
    {
        float scaleX = target.position.x < transform.position.x ? -1f : 1f;
        transform.localScale = new Vector3(scaleX * (isUpgrade ? 1.2f : 1f), isUpgrade ? 1.2f : 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDragging) return;

        if (other.CompareTag("MapBox"))
        {
            targetSlot = other.transform;
            isInSpawnBox = true;
            isInBox = false;
        }
        else if (other.CompareTag("HeroBox"))
        {
            targetSlot = other.transform;
            isInBox = true;
            isInSpawnBox = false;
        }
        else if (other.CompareTag("Player") && other.transform != transform)
        {
            playerTarget = other.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isDragging) return;

        if (other.CompareTag("MapBox"))
        {
            targetSlot = other.transform;
            isInSpawnBox = true;
            isInBox = false;
        }
        else if (other.CompareTag("HeroBox"))
        {
            targetSlot = other.transform;
            isInBox = true;
            isInSpawnBox = false;
        }
        else if (other.CompareTag("Player") && other.transform != transform)
        {
            playerTarget = other.transform;
        }
        else if (other.CompareTag("Delete"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetSlot == other.transform)
        {
            targetSlot = null;
        }
        if (other.CompareTag("MapBox"))
        {
            isInSpawnBox = false;
        }
        if (other.CompareTag("HeroBox"))
        {
            isInBox = false;
        }
        if (playerTarget == other.transform)
        {
            playerTarget = null;
        }
    }

    public void SpawnBulletAttack()
    {
        if (!isFighting || attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > attackRange)
            return;

        GameObject bullet = PoolingManager.Spawn(aceBulletPrefab, attackTarget.position, Quaternion.identity);
        if (bullet != null)
        {
            bullet.GetComponent<Animator>().speed = 1.2f;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = damage;
                bulletScript.crit = crit;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        UpdateHpBar();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void ToggleFighting()
    {
        isFighting = !isFighting;
    }

    public void Return()
    {
        transform.position = lastSlotPos;
    }

    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
        }
    }

    private void RegenerateMana()
    {
        if (manaBar != null)
        {
            manaBar.gameObject.SetActive(true);
        }
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.fixedDeltaTime;
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
            UpdateManaBar();
        }
        canUseSkill = currentMana >= maxMana;
    }

    public void UseSkill()
    {
        if (attackTarget == null) return;

        Debug.Log("Skill activated!");
        if (halo != null) halo.SetActive(false);
        ani.SetBool("IsAttack", false);
        ani.SetBool("IsSkill", true);
    }

    public void CreateSkill()
    {
        if (attackTarget == null) return;

        GameObject skill = PoolingManager.Spawn(aceSkillPrefab, attackTarget.position, Quaternion.identity);
        if (skill != null)
        {
            Bullet skillBullet = skill.GetComponent<Bullet>();
            if (skillBullet != null)
            {
                skillBullet.damage = damage;
                skillBullet.crit = crit;
            }
        }
    }

    public void CreateSkill1()
    {
        if (attackTarget == null) return;

        GameObject skill = PoolingManager.Spawn(aceSkillPrefab, attackTarget.position, Quaternion.identity);
        if (skill != null)
        {
            Bullet skillBullet = skill.GetComponent<Bullet>();
            if (skillBullet != null)
            {
                skillBullet.damage = damage;
                skillBullet.crit = crit;
            }
        }
        sr.enabled = false;
    }

    public void EndSkill()
    {
        ani.SetBool("IsSkill", false);
        currentMana = 0f;
        canUseSkill = false;
        UpdateManaBar();
    }

    public void EndSkill1()
    {
        sr.enabled = true;
        if (attackTarget != null)
        {
            transform.position = attackTarget.position;
        }
        ani.SetBool("IsSkill", false);
        currentMana = 0f;
        canUseSkill = false;
        UpdateManaBar();
    }
}