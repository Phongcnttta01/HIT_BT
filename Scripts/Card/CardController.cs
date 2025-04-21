using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    [SerializeField] public bool isClick = false;
    [SerializeField] public bool isChose = false;
    private bool isAnimating;
    public bool isHandle =true;
    [SerializeField] private Camera mainCamera;
    [SerializeField] public int cardNumber;
    private AudioManager am;
    private bool isOff = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        isClick = false;
        isChose = false;
        isOff = false;
    }

    private void OnEnable()
    {
        if (!isOff)
        {
            mainCamera = Camera.main;
            isClick = false;
            isChose = false;
        }
    }
    
    private void OnDisable()
    {
        isOff = true;
        isClick = false;
        isChose = false;
        isAnimating = false;
        isHandle = true;
    }

    void Start()
    {
        if(isChose) isChose = false;
        if(isClick) isClick = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        // Đảm bảo tham chiếu camera
            mainCamera = Camera.main;
            am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }

    void FixedUpdate()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (GameManager.Instance.isCanClick)
        {
            if (Input.GetMouseButtonDown(0) && !isAnimating && !isHandle)
            {
                am.PlaySFX(am.clickClip);
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                // Kiểm tra xem chuột có nằm trong bounds của collider không
                if (boxCollider.OverlapPoint(mousePos))
                {
                    GameManager.Instance.isCanClick = false;
                    isClick = !isClick;
                    float targetY = isClick ? transform.position.y + 1f : transform.position.y - 1f;
                    isAnimating = true;

                    // Dừng tween cũ trước khi bắt đầu mới
                    transform.DOKill();
                    transform.DOMoveY(targetY, 0.5f)
                        .SetEase(Ease.OutQuad)
                        .OnUpdate(UpdateCollider)
                        .OnComplete(() =>
                        {
                            isAnimating = false;
                            GameManager.Instance.isCanClick = true;
                        });
                    isChose = isClick;

                }

            }
        }
    }

    public void OnClick2(Vector3 clickedTransform)
    {
            if (Input.GetMouseButtonDown(1) && !isAnimating && !isHandle && !isClick)
            {
                am.PlaySFX(am.clickClip);
                if (mainCamera == null || boxCollider == null)
                {
                    return;
                }

                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (boxCollider.OverlapPoint(mousePos) && GameManager.Instance.isCanClick)
                {
                    GameManager.Instance.isCanClick = false;
                    transform.position = clickedTransform;
                    isAnimating = true;
                    transform.DOKill();
                    transform.DOScale(0f, 0.3f)
                        .SetEase(Ease.InQuad)
                        .OnUpdate(UpdateCollider)
                        .OnComplete(() =>
                        {
                            transform.localScale = new Vector3(1f, 1f);
                            UpdateCollider();
                            isAnimating = false;
                        });
                    GameManager.Instance.isCanClick = true;
                }
            }
    }


    private void UpdateCollider()
    {
        if (boxCollider != null && sr != null)
        {
            boxCollider.offset = Vector2.zero;
            // Đảm bảo kích thước collider không quá nhỏ
            boxCollider.size = Vector2.Max(sr.bounds.size, new Vector2(0.1f, 0.1f));
            // Debug.Log("Collider updated - Size: " + boxCollider.size);
        }
    }

    // Đặt lại trạng thái nếu cần (dự phòng)
    private void ResetAnimating()
    {
        isAnimating = false;
        Debug.Log("Forced reset isAnimating");
    }

    // Gọi khi thoát để dừng tất cả tween
    private void OnDestroy()
    {
        transform.DOKill();
    }
}