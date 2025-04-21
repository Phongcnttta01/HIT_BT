using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class SelectBoxController : MonoBehaviour
{
    [SerializeField] private Transform selectPos;
    [SerializeField] private GameObject selectEffect;
    [SerializeField] private List<UpdateCardController> updateCards = new List<UpdateCardController>();
    [SerializeField] private List<GameObject> cardTaken = new List<GameObject>();
    [SerializeField] private CardData cardData;
    [SerializeField] public int selectID;
    [SerializeField] public bool isDestroyed = false;
    public bool isSelected = false;
    void Start()
    {
        GameManager.Instance.isCanClick = false;
        selectEffect.SetActive(false);
        if(gameObject.tag == "Select1") TakeCards1();
        if(gameObject.tag == "Select2") TakeCards2();   
        Vector3 targetScale = transform.localScale;
        transform.localScale = Vector3.zero;

        gameObject.transform.DOScale(targetScale, 1f)
            .SetEase(Ease.InOutQuad)
            .OnUpdate(() =>
            {
                if (gameObject.transform.localScale.magnitude >= targetScale.magnitude * 0.1f && !selectEffect.activeSelf)
                {
                    selectEffect.SetActive(true);
                    selectEffect.transform.localScale = Vector3.zero;
                    selectEffect.transform.DOScale(targetScale, 0.7f).SetEase(Ease.InOutQuad);
                }
            }).OnComplete(()=>
            {
                SpawnCard();
                GameManager.Instance.isCanClick = true;
                MoveCard();
            });
    }


    void Update()
    {
        foreach (var card in updateCards)
        {
            if (card != null)
            {
                if (card.isClickUpdate)
                {
                    isSelected = true;
                    selectID = card.ID;
                    isSelected = true;
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    gameObject.transform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InOutQuad).OnComplete(()=>Destroy(gameObject));
                }
            }
        }
    }

    private void TakeCards1()
    {
        foreach (var card in cardData.cardHero)
        {
            cardTaken.Add(card);
            
        }

        cardTaken = cardTaken.OrderBy(x => Random.value).ToList();
    }
    
    private void TakeCards2()
    {
        foreach (var card in cardData.cardUpdate)
        {
            cardTaken.Add(card);
            
        }

        cardTaken = cardTaken.OrderBy(x => Random.value).ToList();
    }
    
    private void SpawnCard()
    {
        List<GameObject> cards = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            cards.Add(cardTaken[i]);;
        }

        foreach (var card in cards)
        {
            Vector3 targetPos = selectPos.position;
            UpdateCardController cardnew =   PoolingManager.Spawn<UpdateCardController>(card, targetPos , Quaternion.identity);
            cardnew.transform.SetParent(selectPos.parent);
            updateCards.Add(cardnew);
        }
    }

    private void MoveCard()
    {
        Vector3 targetPos = selectPos.position;
        Vector3 targetPosNext = new (updateCards[0].transform.localScale.x , updateCards[0].transform.localScale.y, updateCards[0].transform.localScale.z);
        updateCards[0].transform.position = new Vector3(targetPos.x - 11f, targetPos.y, targetPos.z);
        updateCards[1].transform.position = new Vector3(targetPos.x , targetPos.y, targetPos.z);
        updateCards[2].transform.position = new Vector3(targetPos.x + 11f, targetPos.y, targetPos.z);

        foreach (var card in updateCards)
        {
            card.transform.localScale = Vector3.zero;
            card.transform.DOScale(targetPosNext, 0.8f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                card.transform.localScale = targetPosNext;
                if (!card.isClickUpdate)
                {
                    card.transform.DOScaleX(card.transform.localScale.x+0.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    card.transform.DOScaleY(card.transform.localScale.y+0.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                }
            });
            
        }
        
    }

    private void EndCard()
    {
        foreach (var card in updateCards)
        {
            card.transform.localScale = new (card.transform.localScale.x , card.transform.localScale.y, card.transform.localScale.z);
            card.transform.DOScale(0f,0.2f).SetEase(Ease.InQuad).OnComplete(()=>Destroy(card.gameObject));
        }
    }

    public void ChangeBox()
    {
        
    }
}


