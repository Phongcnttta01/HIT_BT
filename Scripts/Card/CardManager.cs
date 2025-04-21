using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private RectTransform canvas; // Use RectTransform for Canvas
    [SerializeField] private CardData cardData;
    [SerializeField] private List<MiniBox> cardPos;
    [SerializeField] private List<MiniBox> cardSavePos;
    [SerializeField] private Transform boxTransform; // Reference to the Box
    [SerializeField] private List<GameObject> cardList = new List<GameObject>();
    [SerializeField] private GameObject alertBox;
    public List<CardController> cardAcList = new List<CardController>();
    public List<CardController> cardChoice = new List<CardController>();
    [SerializeField] private int cardChoiceNum = 0;
    [SerializeField] private TextMeshProUGUI cardNum;
    private bool canClick = true;
    public int cardNums;

    private void OnEnable()
    {
        cardAcList.Clear(); // Sau khi Despawn xong mới clear
        TakeCard();
        cardNums = cardList.Count;
        for (int i = 0; i < cardPos.Count; i++)
        {
            if (cardPos[i] == null || cardPos[i].gameObject == null )
            {
                continue; // Bỏ qua phần tử này
            }
            
            if (!cardPos[i].isChoice && cardList.Count > 0)
            {
                cardPos[i].isChoice = true;
                GameObject cardPrefab = cardList[0];
                cardList.RemoveAt(0);
                CardController newCard = PoolingManager.Spawn<CardController>(cardPrefab, cardPos[i].transform.position, Quaternion.identity);
                newCard.gameObject.GetComponent<CardController>().isHandle = false;
                if(cardAcList.Count <=5)
                  cardAcList.Add(newCard);
                UpdateCardNumber();
            }
        }
        
    }

    private void OnDisable()
    {
        
        ClearAllSpawnedCards();
        
    }

    private void ClearAllSpawnedCards()
    {
        // Despawn toàn bộ cardChoice
        foreach (var card in cardChoice)
        {
            if (card != null )
            {
                PoolingManager.Despawn(card.gameObject);
                
            }
        }
        cardChoice.Clear(); // đảm bảo sạch

        // Despawn toàn bộ cardAcList
        foreach (var card in cardAcList)
        {
            if (card != null)
            {
                PoolingManager.Despawn(card.gameObject);
            }
        }
        cardAcList.Clear();
        cardList.Clear(); // reset lại danh sách card
        UpdateCardNumber();
    }




    void Update()
    {
        CleanInactiveCards();
        cardNums = cardList.Count;
        // Update Canvas position to follow the Box
        if (boxTransform != null)
        {
            canvas.position = boxTransform.position;
        }
        for (int i = 0; i < cardPos.Count; i++)
        {
            // Kiểm tra xem cardPos[i] có null hoặc đã bị hủy không
            if (cardPos[i] == null || cardPos[i].gameObject == null)
            {
                continue; // Bỏ qua phần tử này
            }

            if (!cardPos[i].isChoice && cardList.Count > 0)
            {
                cardPos[i].isChoice = true;
                SpawnCard(cardPos[i].gameObject);
            }
        }
        SaveCard();
        IsChoiceCard();
        cardChoiceNum = cardChoice.Count;
        if (cardChoiceNum >= 5)
        {
            OffHandle();
        }
        else
        {
            OnHandle();
        }

        if (cardChoice.Count == 5)
        {
            alertBox.SetActive(true);
        }
        else
        {
            alertBox.SetActive(false);
        }
    }

    private void CleanInactiveCards()
    {
        cardAcList.RemoveAll(card => card == null || !card.gameObject.activeSelf);
    }
    private void TakeCard()
    {
        cardList.Clear(); // 💥 reset trước
        foreach (var card in cardData.cards)
        {
            if (card != null)
            {
                cardList.Add(card);
            }
        }

        cardList = cardList.OrderBy(x => Random.value).ToList();
    }


    private void SpawnCard(GameObject pos)
    {
        GameManager.Instance.isCanClick = false;
        GameObject cardPrefab = cardList[0];
        cardList.RemoveAt(0);
        CardController newCard = PoolingManager.Spawn<CardController>(cardPrefab, pos.transform.position, Quaternion.identity);
        newCard.transform.localScale = Vector3.zero;
        newCard.transform.position = pos.transform.position;
        newCard.transform.DOScale(1f, 0.5f).SetEase(Ease.InQuart).OnComplete(()=>
        {
            newCard.gameObject.GetComponent<CardController>().isHandle = false;
            GameManager.Instance.isCanClick = true;
        });
        cardAcList.Add(newCard);
        UpdateCardNumber();
    }

    private void SaveCard()
    {
        if (!canClick) return;
        

        foreach (var card in cardAcList)
        {
            if (card != null)
            {
                for (int i = 0; i < cardSavePos.Count; i++)
                {
                    if (!cardSavePos[i].isChoice && canClick)
                    {
                        if (cardSavePos[i].transform != null)
                            card.OnClick2(cardSavePos[i].transform.position);

                    }
                }
            }
        }
        
    }

    private void IsChoiceCard()
    {
        foreach (var card in cardAcList)
        {
            if (card.isChose && !cardChoice.Contains(card))
            {
                cardChoice.Add(card);
            }
        }

        cardChoice.RemoveAll(card => !card.isChose);
    }

    public void RemoveAllChoices()
    {
        GameManager.Instance.isCanClick = false;
        OffHandle();
        List<CardController> cardsToRemove = new List<CardController>();
        for (int i = cardAcList.Count - 1; i >= 0; i--)
        {
            if (cardAcList[i] != null && cardAcList[i].isChose)
            {
                cardAcList[i].isHandle = true;
                CardController card = cardAcList[i];
                card.transform.localScale = Vector3.one;

                card.transform.DOScale(0f, 1.5f).SetEase(Ease.InCubic);
                card.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        PoolingManager.Despawn(card.gameObject);
                    });

                cardsToRemove.Add(card);
            }
        }
        for (int i = cardChoice.Count - 1; i >= 0; i--)
        {
            if (cardChoice[i] != null && cardChoice[i].isChose)
            {
                cardChoice[i].isHandle = true;
                CardController card = cardChoice[i];
                card.transform.localScale = Vector3.one;

                card.transform.DOScale(0f, 1.5f).SetEase(Ease.InCubic);
                card.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        PoolingManager.Despawn(card.gameObject);
                        cardChoice.Remove(card);
                    });

                cardsToRemove.Add(card);
            }
        }

        UpdateCardNumber();
        OffHandle();
        GameManager.Instance.isCanClick = true;
    }

    public List<int> Allow()
    {
        List<int> answer = new List<int>();

        List<int> numbers = cardChoice.Select(card => card.cardNumber).ToList();

        if (numbers.Contains(-1))
            answer.Add(1);
        if (numbers.Contains(15))
            answer.Add(2);

        bool hasThreeSame = numbers.GroupBy(n => n).Any(g => g.Count() >= 3);

        List<int> uniqueNumbers = numbers.Distinct().ToList();
        uniqueNumbers.Sort();

        bool hasSequence = false;
        if (uniqueNumbers.Count >= 3)
        {
            for (int i = 0; i <= uniqueNumbers.Count - 3; i++)
            {
                if (uniqueNumbers[i] + 1 == uniqueNumbers[i + 1] &&
                    uniqueNumbers[i + 1] + 1 == uniqueNumbers[i + 2])
                {
                    hasSequence = true;
                    break;
                }
            }
        }

        if (hasThreeSame)
            answer.Add(1);
        if (hasSequence)
            answer.Add(2);
        
        answer.Sort();
        answer.Reverse();
        
        return answer;
    }

    private void UpdateCardNumber()
    {
        cardNum.text = cardList.Count.ToString();
    }

    private void OnHandle()
    {
        foreach (var card in cardAcList)
        {
            if (!card.isChose)
                card.isHandle = false;
        }
    }

    private void OffHandle()
    {
        foreach (var card in cardAcList)
        {
            if (!card.isChose)
                card.isHandle = true;
        }
    }
}