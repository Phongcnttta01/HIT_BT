using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBox : MonoBehaviour
{
    public bool isChoice = false;
    [SerializeField] private int cardCount = 0; // Đếm số Card đang va chạm

    private void OnEnable()
    {
        isChoice = false;
        cardCount = 0; // Đặt lại trạng thái khi GameObject được kích hoạt
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            if (cardCount == 0) // Chỉ cho phép chứa 1 lá bài
            {
                cardCount++;
                isChoice = true;
            }
            else
            {
                // Nếu đã có 1 lá bài, không nhận thêm
                other.gameObject.SetActive(false); // Hoặc xử lý theo cách phù hợp với game của bạn
            }
        }
    }
    
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            cardCount--;
            if (cardCount <= 0)
            {
                isChoice = false;
                cardCount = 0; // Đảm bảo không bị âm
            }
        }
    }
}

