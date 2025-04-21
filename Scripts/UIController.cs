using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UICardBoxManager uiCardBoxManager;
    [SerializeField] private RectTransform canvasCard;
    [SerializeField] private RectTransform canvasMap;
    [SerializeField] private Transform boxCard; // Box mà canvasCard sẽ dính vào
    [SerializeField] private Transform boxMap;  // Box mà canvasMap sẽ dính vào
    [SerializeField] private MapManager mg;
    [SerializeField] private AudioManager am;

    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject level1_5;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private GameObject quitBackPack;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject[] levelInfos;
    
    [SerializeField] private GameObject[] listLock;

    [SerializeField] private Sprite offSound;
    [SerializeField] private Sprite onSound;
    private bool isOnSound = true;
    

    void Start()
    {
        mainCamera = Camera.main;
        
    }

    void Update()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (i + 1 <= GameManager.Instance.Level-1)
            {
                listLock[i].SetActive(false); // Mở khóa
            }
            else
            {
                listLock[i].SetActive(true); // Khóa
            }
        }
    }

    public void Return()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isCanClick = false;
            mainCamera.transform.DOMoveY(mainCamera.transform.position.y - 20f, 1f).OnComplete(()=>GameManager.Instance.isCanClick = true);
        }
    }

    public void Team()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isCanClick = false;
            mainCamera.transform.DOMoveY(mainCamera.transform.position.y + 20f, 1f).OnComplete(()=>GameManager.Instance.isCanClick = true);
        }
    }

    public void Play()
    {
        if (GameManager.Instance.isCanClick)
        {
            foreach (var hero in mg.heroActive)
            {
                if (hero != null)
                {
                    HeroController heroController = hero.GetComponent<HeroController>();
                    if (heroController.isInBox) heroController.isFighting = true;
                }
            }
            mg.myTeam.SetActive(false);
            mg.canvas.gameObject.SetActive(false);
            transform.DOScale(1f, 0.5f).OnComplete(() =>
            {
                mg.isInGame = true;
                GameManager.Instance.isEndDay = false;
                GameManager.Instance.inPlay = true;
            });
            
        }
    }

    
    // LevelInfo 
    public void PlayLevel()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (levelInfos[0].activeSelf)
            {
                GameManager.Instance.isCanClick = false;
                loading.SetActive(true);
                Vector3 pos = loading.transform.localScale;
                loading.transform.localScale = Vector3.zero;
                loading.transform.DOScale(pos, 1f).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        levelInfos[0].SetActive(false);
                        level1_5.SetActive(false);
                        Vector3 howPos = transform.position;
                        howPos.y += 4f;
                        PoolingManager.Spawn(howToPlay, howPos, Quaternion.identity);
                        GameManager.Instance.InLevel1();
                        loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                        {
                            loading.transform.localScale = pos;
                            loading.SetActive(false);
                            GameManager.Instance.isCanClick = true;
                            GameManager.Instance.currentlevel = 1;
                            mg.level = 1;
                        });
                    });
            }

            if (levelInfos[1].activeSelf)
            {
                if (!listLock[0].activeSelf)
                {
                    GameManager.Instance.isCanClick = false;
                    loading.SetActive(true);
                    Vector3 pos = loading.transform.localScale;
                    loading.transform.localScale = Vector3.zero;
                    loading.transform.DOScale(pos, 1f).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            levelInfos[1].SetActive(false);
                            level1_5.SetActive(false);
                            GameManager.Instance.InLevel2();
                            loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                            {
                                loading.transform.localScale = pos;
                                loading.SetActive(false);
                                GameManager.Instance.isCanClick = true;
                                GameManager.Instance.currentlevel = 2;
                                mg.level = 2;
                            });
                        });
                }
            }

            if (levelInfos[2].activeSelf)
            {
                if (!listLock[1].activeSelf)
                {
                    GameManager.Instance.isCanClick = false;
                    loading.SetActive(true);
                    Vector3 pos = loading.transform.localScale;
                    loading.transform.localScale = Vector3.zero;
                    loading.transform.DOScale(pos, 1f).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            levelInfos[2].SetActive(false);
                            level1_5.SetActive(false);
                            GameManager.Instance.InLevel3();
                            loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                            {
                                loading.transform.localScale = pos;
                                loading.SetActive(false);
                                GameManager.Instance.isCanClick = true;
                                GameManager.Instance.currentlevel = 3;
                                mg.level = 3;
                            });
                        });
                }
            }

            if (levelInfos[3].activeSelf)
            {
                if (!listLock[2].activeSelf)
                {
                    GameManager.Instance.isCanClick = false;
                    loading.SetActive(true);
                    Vector3 pos = loading.transform.localScale;
                    loading.transform.localScale = Vector3.zero;
                    loading.transform.DOScale(pos, 1f).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            levelInfos[3].SetActive(false);
                            level1_5.SetActive(false);
                            GameManager.Instance.InLevel4();
                            loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                            {
                                loading.transform.localScale = pos;
                                loading.SetActive(false);
                                GameManager.Instance.isCanClick = true;
                                GameManager.Instance.currentlevel = 4;
                                mg.level = 4;
                            });
                        });
                }
            }

            if (levelInfos[4].activeSelf)
            {
                if (!listLock[3].activeSelf)
                {
                    GameManager.Instance.isCanClick = false;
                    loading.SetActive(true);
                    Vector3 pos = loading.transform.localScale;
                    loading.transform.localScale = Vector3.zero;
                    loading.transform.DOScale(pos, 1f).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            levelInfos[4].SetActive(false);
                            level1_5.SetActive(false);
                            GameManager.Instance.InLevel5();
                            loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                            {
                                loading.transform.localScale = pos;
                                loading.SetActive(false);
                                GameManager.Instance.isCanClick = true;
                                GameManager.Instance.currentlevel = 5;
                                mg.level = 5;
                            });
                        });
                }
            }
        }

    }
    public void BackLevel()
    {
        if (GameManager.Instance.isCanClick)
        {
            foreach (var level in levelInfos)
            {
                if (level.activeSelf)
                {
                    GameManager.Instance.isCanClick = false;
                    Vector3 pos = level.transform.localScale;
                    level.transform.DOScale(0f, 1f).SetEase(Ease.InCubic).OnComplete(() =>
                    {
                        level.transform.localScale = pos;
                        level.SetActive(false);
                        GameManager.Instance.isCanClick = true;
                    });
                }
            }

        }
    }
    
    
    // Level
    public void Level1()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isCanClick = false;
            Vector3 pos = levelInfos[0].transform.localScale;
            levelInfos[0].SetActive(true);
            levelInfos[0].transform.localScale = Vector3.zero;
            levelInfos[0].transform.DOScale(pos, 1f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                levelInfos[0].transform.localScale = pos;
                GameManager.Instance.isCanClick = true;
            });
        }
    }

    public void Level2()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (!listLock[0].activeSelf)
            {
                GameManager.Instance.isCanClick = false;
                Vector3 pos = levelInfos[0].transform.localScale;
                levelInfos[1].SetActive(true);
                levelInfos[1].transform.localScale = Vector3.zero;
                levelInfos[1].transform.DOScale(pos, 1f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    levelInfos[1].transform.localScale = pos;
                    GameManager.Instance.isCanClick = true;
                });
            }
        }
    }
    public void Level3()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (!listLock[1].activeSelf)
            {
                GameManager.Instance.isCanClick = false;
                Vector3 pos = levelInfos[0].transform.localScale;
                levelInfos[2].SetActive(true);
                levelInfos[2].transform.localScale = Vector3.zero;
                levelInfos[2].transform.DOScale(pos, 1f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    levelInfos[2].transform.localScale = pos;
                    GameManager.Instance.isCanClick = true;
                });
            }
        }
    }
    public void Level4()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (!listLock[2].activeSelf)
            {
                GameManager.Instance.isCanClick = false;
                Vector3 pos = levelInfos[0].transform.localScale;
                levelInfos[3].SetActive(true);
                levelInfos[3].transform.localScale = Vector3.zero;
                levelInfos[3].transform.DOScale(pos, 1f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    levelInfos[3].transform.localScale = pos;
                    GameManager.Instance.isCanClick = true;
                });
            }
        }
    }
    public void Level5()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (!listLock[3].activeSelf)
            {
                GameManager.Instance.isCanClick = false;
                Vector3 pos = levelInfos[0].transform.localScale;
                levelInfos[4].SetActive(true);
                levelInfos[4].transform.localScale = Vector3.zero;
                levelInfos[4].transform.DOScale(pos, 1f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    levelInfos[4].transform.localScale = pos;
                    GameManager.Instance.isCanClick = true;
                });
            }
        }
    }

    public void QuitBackPack()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isCanClick = false;
            Vector3 pos = quitBackPack.transform.localScale;
            quitBackPack.transform.DOMoveY(25f, 1f).SetEase(Ease.Flash).OnComplete(() =>
            {
                quitBackPack.SetActive(false);
                GameManager.Instance.isCanClick = true;
                // menu.SetActive(true);
            });
        }
    }
    public void ComeBackPack()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isCanClick = false;
            quitBackPack.SetActive(true);
            quitBackPack.transform.DOMoveY(-1f, 1f).SetEase(Ease.Flash).OnComplete(() =>
            {
                GameManager.Instance.isCanClick = true;
                // menu.SetActive(true);
            });
        }
    }
    public void ComeLevel()
    {
        if (GameManager.Instance.isCanClick)
        {
            loading.SetActive(true);
            Vector3 pos = loading.transform.localScale;
            loading.transform.localScale = Vector3.zero;
            loading.transform.DOScale(pos, 0.8f).SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    GameManager.Instance.isCanClick = false;
                    level1_5.SetActive(true);
                    loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        loading.transform.localScale = pos;
                        loading.SetActive(false);
                        menu.SetActive(false);
                        GameManager.Instance.isCanClick = true;
                    });
                });
            
        }
    }
    public void QuitLevel()
    {
        if (GameManager.Instance.isCanClick)
        {
            loading.SetActive(true);
            Vector3 pos = loading.transform.localScale;
            loading.transform.localScale = Vector3.zero;
            loading.transform.DOScale(pos, 0.8f).SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    level1_5.SetActive(false);
                    GameManager.Instance.isCanClick = false;
                    menu.SetActive(true);
                    loading.transform.DOScale(0f, 1f).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        loading.transform.localScale = pos;
                        loading.SetActive(false);
                        GameManager.Instance.isCanClick = true;
                    });
                });
        }
    }
    
    public void Out1()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isEndDay = false;
            GameManager.Instance.isCanClick = false;
            GameManager.Instance.Restart();
        }
    }

    public void ReLoad()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isEndDay = false;
            GameManager.Instance.isCanClick = false;
            GameManager.Instance.Reload();
        }
    }
    
    public void NextLevel()
    {
        if (GameManager.Instance.isCanClick)
        {
            GameManager.Instance.isEndDay = false;
            GameManager.Instance.isCanClick = false;
            GameManager.Instance.NextLevel();
            
        }
    }
    public void OffSound()
    {
        if (isOnSound)
        {
            am.musicAudioSource.mute = true;
            settings.gameObject.GetComponent<Image>().sprite = offSound;
            isOnSound = false;
        }
        else
        {
            am.musicAudioSource.mute = false;
            settings.gameObject.GetComponent<Image>().sprite = onSound;
            isOnSound = true;
        }
        
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
