using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void EndDayDelegate();
public delegate void LevelStartDelegate(int level);
public delegate void GameResultDelegate(bool isVictory);

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public event LevelStartDelegate OnLevelStart;
    public event GameResultDelegate OnGameEnded;

    [SerializeField] private CardData cardData;
    [SerializeField] private MapManager mg;
    [SerializeField] private CardManager cm;
    [SerializeField] private SelectManager sm;
    [SerializeField] GameObject vicPrefab;
    [SerializeField] GameObject vicPrefab1;
    [SerializeField] GameObject defeatPrefab;
    [SerializeField] GameObject[] levelInfos;

    [SerializeField] private GameObject cardBox;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject level1_5;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject loading1;

    [SerializeField] public int Level;
    public int IdChoice = -1;
    public bool isEndDay = false;
    public bool inPlay = false;
    public bool isCanClick;

    [Header("Auto Merge Hero Settings")]
    [SerializeField] private float checkMergeTime = 1f;
    private float currentTime = 0f;
    public int currentlevel = 1;
    
   

    private void Start()
    {
        
        Instance = this;
        Application.targetFrameRate = 60;
        Level = PlayerPrefs.GetInt("Level", 1);

        OnGameEnded += HandleGameEnded;
        OnLevelStart += HandleLevelStart;
    }

    private void Update()
    {
        if (!isEndDay && inPlay)
        {
            CheckEndDay();
        }

        AutoMergeHero();
        
        if(Camera.main.transform.position.y == 0)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Result");
            if (obj != null)
            {
                PoolingManager.Despawn(obj);
            }
        }
        
        foreach (var hero in mg.heroActive)
        {
            if (hero != null)
            {
                var hc = hero.GetComponent<HeroController>();
                if(!hc.isFighting && !hc.isInSpawnBox)
                {
                    hc.isInBox = true;
                }
            }
        }
    }

    private void CheckEndDay()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] heross = GameObject.FindGameObjectsWithTag("PlayerActive");

        if (  heross.Length == 0 && enemies.Length >= 0 && mg.isInGame)
        {
            Debug.Log("dhsghsdgfsdgfsdgfuysh");
                GameManager.Instance.isCanClick = false;
                isEndDay = true;
                transform.DOScale(1f, 1f).OnComplete(() => OnGameEnded?.Invoke(false));
        }
        else if (enemies.Length == 0 && mg.isInGame)
        {
            GameManager.Instance.isCanClick = false;
            isEndDay = true;
            if (mg.day < 5 && mg.level < 3 || (mg.day < 2 && mg.level >= 4))
            {
                mg.day++;
                Vector3 spawnPos = transform.position;
                spawnPos.y += 2f;
                GameObject vic = PoolingManager.Spawn(vicPrefab1, spawnPos, Quaternion.identity);
                vic.transform.localScale = Vector3.zero;
                foreach (var hero in mg.heroActive)
                {
                    if (hero != null)
                    {
                        var hc = hero.GetComponent<HeroController>();
                        hc.ani.SetBool("IsAttack", false);
                        hc.ani.SetBool("IsSkill", false);
                        hc.ani.SetBool("IsRun", false);
                        hc.currentHp = hc.maxHp;
                        hc.currentMana = 0f;
                        hc.tag = "Player";
                        hc.isFighting = false;
                        hc.transform.localScale = new Vector3(
                            Mathf.Abs(hc.transform.localScale.x),
                            Mathf.Abs(hc.transform.localScale.y),
                            Mathf.Abs(hc.transform.localScale.z));
                        if (!hc.isInSpawnBox) hc.isInBox = true;
                        hc.Return();
                    }
                }
                vic.transform.DOScale(1f, 1f).OnComplete(() =>
                {
                    OnGameEnded?.Invoke(true);
                });
            }
            else
            {
                mg.day++;
                GameObject vic = PoolingManager.Spawn(vicPrefab, transform.position, Quaternion.identity);
                vic.transform.localScale = Vector3.zero;
                vic.transform.DOScale(1f, 1f).OnComplete(() =>
                {
                    OnGameEnded?.Invoke(true);
                    });
            }
        }
    }

    private void HandleGameEnded(bool isVictory)
    {
        if (isVictory)
        {
            if ((mg.day <= 5 && mg.level <= 3) || (mg.day <= 2 && mg.level >= 4))
            {
                
                mg.myTeam.SetActive(true);
                mg.canvas.gameObject.SetActive(true);
                mg.isInGame = false;
                mg.isSapwning = false;
                
            }
            else
            {
                PlayerPrefs.SetInt("Level", Level + 1);
                PlayerPrefs.Save();
                currentlevel++;
                inPlay = false;
                mg.isInGame = false;
                GameManager.Instance.isCanClick = true;
            }
        }
        else
        {
            foreach (var hero in mg.heroActive)
            {
                if (hero != null)
                    PoolingManager.Despawn(hero);
            }

            GameObject def = PoolingManager.Spawn(defeatPrefab, transform.position, Quaternion.identity);
            def.transform.localScale = Vector3.zero;
            def.transform.DOScale(1f, 3f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                GameManager.Instance.isCanClick = true;
                //Restart();
                //if(!inPlay) PoolingManager.Despawn(def);
            });
        }
      
        sm.enabled = false;
    }

    private void AutoMergeHero()
    {
        currentTime += Time.deltaTime;
        if (currentTime < checkMergeTime) return;
        currentTime = 0f;

        Dictionary<int, List<GameObject>> heroGroups = new();

        foreach (var hero in mg.heroActive)
        {
            if (hero != null)
            {
                var hc = hero.GetComponent<HeroController>();
                if (hc == null || hc.ID >= 10) continue;

                if (!heroGroups.ContainsKey(hc.ID))
                    heroGroups[hc.ID] = new List<GameObject>();

                heroGroups[hc.ID].Add(hero);
            }
        }

        foreach (var group in heroGroups)
        {
            if (group.Value.Count >= 3)
            {
                StartCoroutine(MergeHero(group.Value.GetRange(0, 3)));
                break;
            }
        }
    }

    private IEnumerator MergeHero(List<GameObject> heroesToMerge)
    {
        Vector3 spawnPos = heroesToMerge[0].transform.position;
        HeroController firstHero = heroesToMerge[0].GetComponent<HeroController>();
        Vector3 Scale = firstHero.transform.localScale;

        foreach (var hero in heroesToMerge)
        {
            hero.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        }

        yield return new WaitForSeconds(0.35f);

        foreach (var hero in heroesToMerge)
        {
            mg.heroActive.Remove(hero);
            Destroy(hero);
        }

        GameObject newhero = PoolingManager.Spawn(cardData.herosList[firstHero.ID - 1], spawnPos, Quaternion.identity);
        newhero.transform.localScale = Scale * 1.2f;
        mg.heroActive.Add(newhero);
    }

    public void InLevel(int level)
    {
        sm.enabled = true;
        mg.level = level;
        currentlevel = level;
        mg.day = 1;
        if(cardBox != null) 
         cardBox.SetActive(true);
        if(map != null)
         map.SetActive(true);
        transform.DOScale(1f, 1.5f).OnComplete(() =>
        {
            OnLevelStart?.Invoke(level);
        });
    }

    public void InLevel1() => InLevel(1);
    public void InLevel2() => InLevel(2);
    public void InLevel3() => InLevel(3);
    public void InLevel4() => InLevel(4);
    public void InLevel5() => InLevel(5);

    public void Out()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isEndDay = false;
            GameManager.Instance.isCanClick = false;
                GameObject def = PoolingManager.Spawn(defeatPrefab, transform.position, Quaternion.identity);
                def.transform.localScale = Vector3.zero;
                def.transform.DOScale(1f, 3f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    GameManager.Instance.isCanClick = true;
                });
        }
    }
    

    public void Restart()
    {
        loading1.SetActive(true);
        Vector3 pos = loading1.transform.localScale;
        loading1.transform.localScale = Vector3.zero;
        Vector3 pos1 = loading.transform.localScale;
        loading1.transform.DOScale(pos, 1f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            inPlay = false;
            loading.SetActive(true);
            cardBox.SetActive(false);
            map.SetActive(false);
            Camera.main.transform.DOMoveY(Camera.main.transform.position.y - 20f, 0.05f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                mg.isInGame = false;
                currentlevel = 1;
                loading1.transform.localScale = pos;
                loading1.SetActive(false);
                level1_5.SetActive(true);
                loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(()=>
                {
                    loading.transform.localScale = pos1;
                    loading.SetActive(false);
                    GameManager.Instance.isCanClick = true;
                });
                
            });
        });
    }

    public void Reload()
    {
        loading1.SetActive(true);
        Vector3 pos = loading1.transform.localScale;
        loading1.transform.localScale = Vector3.zero;
        Vector3 pos1 = loading.transform.localScale;
        loading1.transform.DOScale(pos, 1.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            inPlay = false;
            loading.SetActive(true);
            cardBox.SetActive(false);
            map.SetActive(false);
            Camera.main.transform.DOMoveY(Camera.main.transform.position.y - 20f, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                mg.isInGame = false;
                loading1.transform.localScale = pos;
                loading1.SetActive(false);
                InLevel(currentlevel);
                loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(()=>
                {
                    loading.transform.localScale = pos1;
                    loading.SetActive(false);
                    GameManager.Instance.isCanClick = true;
                });
                
            });
        });
    }

    public void NextLevel()
    {
        loading1.SetActive(true);
        Vector3 pos = loading1.transform.localScale;
        loading1.transform.localScale = Vector3.zero;
        Vector3 pos1 = loading.transform.localScale;
        loading1.transform.DOScale(pos, 1f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            inPlay = false;
            loading.SetActive(true);
            cardBox.SetActive(false);
            map.SetActive(false);
            Camera.main.transform.DOMoveY(Camera.main.transform.position.y - 20f, 0.05f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                mg.isInGame = false;
                loading1.transform.localScale = pos;
                loading1.SetActive(false);
                level1_5.SetActive(true);
                loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(()=>
                {
                    loading.transform.localScale = pos1;
                    loading.SetActive(false);
                });
                
            });
        });
        levelInfos[currentlevel - 1].SetActive(true);
        Vector3 pos2 = levelInfos[currentlevel - 1].transform.localScale;
        levelInfos[currentlevel - 1].SetActive(true);
        levelInfos[currentlevel - 1].transform.localScale = Vector3.zero;
        levelInfos[currentlevel - 1].transform.DOScale(pos2, 1f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            levelInfos[currentlevel - 1].transform.localScale = pos2;
            GameManager.Instance.isCanClick = true;
        });
    }
    
    private void HandleLevelStart(int level)
    {
        Debug.Log($"Level {level} started.");
        // Additional setup logic if needed
    }
}
