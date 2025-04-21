using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private AudioManager am;
    [SerializeField] private List<MiniBoxController> heroSpawn = new List<MiniBoxController>();
    [SerializeField] public List<GameObject> heroActive = new List<GameObject>();
    [SerializeField] private CardData cardData;
    [SerializeField] private Camera maincam;
    [SerializeField] public GameObject myTeam;
    [SerializeField] public GameObject spawnBoxs;
    [SerializeField] public TextMeshProUGUI dayText;
    [SerializeField] public GameObject upEffect;
    [SerializeField] public Canvas canvas;
    [SerializeField] public EnemySpawn enemySpawn;
    [SerializeField] public int level = 1;
    [SerializeField] public int day = 1;
    public bool isSapwning = false;
    public bool isInGame = false;

    private bool canSpawn = true;
    private bool isChoosingTarget = false;

    private void OnEnable()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (Camera.main != null)
            maincam = Camera.main;
        SetDay(day);
        //isInGame = true;
        canvas.gameObject.SetActive(true);
        dayText.enabled = true;
        myTeam.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.isEndDay = false;
        isSapwning = false;
        isInGame = false;
        canSpawn = true;
        isChoosingTarget = false;

        // Xoá toàn bộ Hero đang active
        foreach (var hero in heroActive)
        {
            if (hero != null)
            {
                PoolingManager.Despawn(hero.gameObject);
            }
        }
        heroActive.Clear();

        // Xoá các Hero trong box spawn
        foreach (var hero in heroSpawn)
        {
            if (hero != null)
            {
                if (hero.currentHero != null)
                {
                    PoolingManager.Despawn(hero.currentHero);
                    hero.currentHero = null;
                }
                hero.isHasHero = false;
            }
        }

        // 💥 Xoá toàn bộ Enemy đang có
        foreach (var enemy in enemySpawn.enemyActive)
        {
            if (enemy != null)
            {
                PoolingManager.Despawn(enemy);
            }
        }
        enemySpawn.enemyActive.Clear();
        
    }


    void Update()
    {
        SetDay(day);
        //Debug.Log(heroActive.Count);
        if(level == 1)
        {
            SpawnLevel1();
        }
        if(level == 2)
        {
            SpawnLevel2();
        }
        if(level == 3)
        {
            SpawnLevel3();
        }
        if(level == 4)
        {
            SpawnLevel4();
        }
        if(level == 5)
        {
            SpawnLevel5();
        }
        // Trường hợp chọn Hero
        if (GameManager.Instance.IdChoice > -1 && canSpawn && GameManager.Instance.IdChoice < 10)
        {
                SpawnHero();
        }

        
        // Xử lý chọn target Player
        if (GameManager.Instance.IdChoice >= 10)
        {
            MoveCameraUp();
            //HandleChooseTarget();
            if(heroActive.Count > 0)
            {
                foreach (var hero in heroActive)
                {
                    if (hero != null)
                    {
                        var hc = hero.GetComponent<HeroController>();
                        if (hc != null)
                        {
                            PoolingManager.Spawn(upEffect, hc.transform.position, Quaternion.identity);
                        }
                    }
                }
                if (GameManager.Instance.IdChoice - 10 == 0)
                {
                   
                    foreach (var hero in heroActive)
                    {
                        if (hero != null)
                        {
                            HeroController heroController = hero.GetComponent<HeroController>();
                            heroController.crit += 0.05f;
                        }
                    }

                    GameManager.Instance.IdChoice = -1;
                }
                if (GameManager.Instance.IdChoice - 10 == 2)
                {
                    foreach (var hero in heroActive)
                    {
                        if (hero != null)
                        {
                            HeroController heroController = hero.GetComponent<HeroController>();
                            heroController.attackSpeed += 0.1f;
                        }
                    }
                    GameManager.Instance.IdChoice = -1;
                }
                if (GameManager.Instance.IdChoice - 10 == 1)
                {
                    foreach (var hero in heroActive)
                    {
                        if (hero != null)
                        {
                            HeroController heroController = hero.GetComponent<HeroController>();
                            heroController.damage += 10f;
                        }
                    }
                    GameManager.Instance.IdChoice = -1;
                }
            }
            else
            {
                GameManager.Instance.IdChoice = -1;
            }
        }
    }

    // Spawn Hero vào slot trống
    private void SpawnHero()
    {
        foreach (var pos in heroSpawn)
        {
            if (!pos.isHasHero)
            {
                canSpawn = false;
                GameObject hero = PoolingManager.Spawn(cardData.herosList[GameManager.Instance.IdChoice - 1], pos.transform.position, Quaternion.identity);
                heroActive.Add(hero);
                hero.gameObject.GetComponent<HeroController>().isInSpawnBox = true;
                pos.isHasHero = true;
                pos.currentHero = hero;

                MoveCameraUp(() =>
                {
                    GameManager.Instance.IdChoice = -1;
                    StartCoroutine(DelaySpawn());
                });

                break;
            }
        }
    }

    // Kiểm tra map có Player chưa
    private void HandleChooseTarget()
    {
        bool hasPlayer = false;
        foreach (var pos in heroSpawn)
        {
            if (pos.isHasHero)
            {
                hasPlayer = true;
                break;
            }
        }

        if (hasPlayer)
        {
            isChoosingTarget = true;
            canSpawn = false;

            HighlightTargets(true);
        }
        else
        {
            GameManager.Instance.IdChoice = -1;
        }
    }

    // Đổi màu vàng cho Player có thể click
    private void HighlightTargets(bool isHighlight)
    {
        foreach (var pos in heroActive)
        {
            if (pos == null) continue;

            SpriteRenderer render = pos.GetComponentInChildren<SpriteRenderer>();
            render.material.color = isHighlight ? Color.yellow : Color.white;
        }
            
    }

    // Camera move lên cho đẹp
    private void MoveCameraUp(TweenCallback onComplete = null)
    {
        Vector3 camPos = maincam.transform.position;
        camPos.y = 0f;
        maincam.transform.position = camPos;

        maincam.transform.DOMoveY(camPos.y + 20f, 1f).OnComplete(onComplete);
    }

    // Xử lý khi chọn trúng Player
    private void DoSomething(GameObject target)
    {
        target.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);
        Debug.Log("Làm gì đó với: " + target.name);
    }

    // Reset lại sau khi chọn target xong
    private void ResetAfterChooseTarget()
    {
        HighlightTargets(false);
        GameManager.Instance.IdChoice = -1;
        isChoosingTarget = false;
        StartCoroutine(DelaySpawn());
    }

    // Delay tí rồi cho spawn tiếp
    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(1.5f);
        canSpawn = true;
    }

    private void SpawnLevel1()
    {
        if (!isSapwning && day == 1)
        {
            enemySpawn.Spawn1Day1();
            isSapwning = true;
        }
        if (!isSapwning && day == 2)
        {
            enemySpawn.Spawn1Day2();
            isSapwning = true;
        }
        if (!isSapwning && day == 3)
        {
            enemySpawn.Spawn1Day3();
            isSapwning = true;
        }
        if (!isSapwning && day == 4)
        {
            enemySpawn.Spawn1Day4();
            isSapwning = true;
        }
        if (!isSapwning && day == 5)
        {
            enemySpawn.Spawn1Day5();
            isSapwning = true;
        }
    }

    private void SpawnLevel2()
    {
        if (!isSapwning && day == 1)
        {
            enemySpawn.Spawn2Day1();
            isSapwning = true;
        }

        if (!isSapwning && day == 2)
        {
            enemySpawn.Spawn2Day2();
            isSapwning = true;
        }

        if (!isSapwning && day == 3)
        {
            enemySpawn.Spawn2Day3();
            isSapwning = true;
        }

        if (!isSapwning && day == 4)
        {
            enemySpawn.Spawn2Day4();
            isSapwning = true;
        }

        if (!isSapwning && day == 5)
        {
            enemySpawn.Spawn2Day5();
            isSapwning = true;
        }
    }
    
    private void SpawnLevel3()
    {
        if (!isSapwning && day == 1)
        {
            enemySpawn.Spawn3Day1();
            isSapwning = true;
        }

        if (!isSapwning && day == 2)
        {
            enemySpawn.Spawn3Day2();
            isSapwning = true;
        }

        if (!isSapwning && day == 3)
        {
            enemySpawn.Spawn3Day3();
            isSapwning = true;
        }

        if (!isSapwning && day == 4)
        {
            enemySpawn.Spawn3Day4();
            isSapwning = true;
        }

        if (!isSapwning && day == 5)
        {
            enemySpawn.Spawn3Day5();
            isSapwning = true;
        }
    }

    private void SpawnLevel4()
    {
        if (!isSapwning && day == 1)
        {
            enemySpawn.Spawn4Day1();
            isSapwning = true;
        }

        if (!isSapwning && day == 2)
        {
            enemySpawn.Spawn4Day2();
            isSapwning = true;
        }
    }
    private void SpawnLevel5()
    {
        if (!isSapwning && day == 1)
        {
            enemySpawn.Spawn5Day1();
            isSapwning = true;
        }

        if (!isSapwning && day == 2)
        {
            enemySpawn.Spawn5Day2();
            isSapwning = true;
        }
    }

    public void SetDay(int day)
    {
        this.day = day;
        dayText.text = "Day " + day;
    }
}
