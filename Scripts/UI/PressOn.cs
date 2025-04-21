using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PressOn : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameObject.transform.DOScale(0f, 0.7f).SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    GameManager.Instance.isCanClick = true;
                    PoolingManager.Despawn(gameObject);
                });
        }
    }
}
