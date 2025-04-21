using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpdateCardController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D box;
    [SerializeField] private GameManager gm;
    public int ID;
    public bool isClickUpdate = false;
    [SerializeField] private Camera mainCamera; // Tham chiếu thủ công tới camera
    // [SerializeField] private GameObject border;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ClickOn();
    }

    private void ClickOn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (box.OverlapPoint(mousePos))
            {
                if (!isClickUpdate && GameManager.Instance.isCanClick)
                {
                    GameManager.Instance.isCanClick = false;
                    Vector3 Scale = transform.localScale;
                    transform.DOScale(Scale + new Vector3(0.2f,0.2f,0.2f),0.2f)
                        .OnUpdate(UpdateCollider)
                        .OnComplete(() => {
                            Debug.Log("Card clicked, animation complete");
                            // Thêm xử lý hiện hero hoặc thêm xử lý vào map
                            gm.IdChoice = ID;
                            isClickUpdate = true;
                            Destroy(this.gameObject);
                        });
                    GameManager.Instance.isCanClick = true;
                }
                
            }
        }
    }
    
    private void UpdateCollider()
    {
        if (box != null && sr != null)
        {
            box.offset = Vector2.zero;
            // Đảm bảo kích thước collider không quá nhỏ
            box.size = Vector2.Max(sr.size , new Vector2(0.1f, 0.1f));
            // Debug.Log("Collider updated - Size: " + boxCollider.size);
        }
    }
}
