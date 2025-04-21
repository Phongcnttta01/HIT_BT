using System;
using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour
{
    [SerializeField] private Vector3 scaleFactor;
    [SerializeField] private Vector3 scaleOriginal;
    [SerializeField] private float duration = 1f;
    private AudioManager am;
    private Sequence bounceSequence;

    void Start()
    {
        am = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        scaleFactor = gameObject.transform.localScale * 1.1f;
        scaleOriginal = gameObject.transform.localScale;
        StartBouncing();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    public void StartBouncing()
    {

        bounceSequence = DOTween.Sequence()
            .Append(transform.DOScale(scaleFactor, duration).SetEase(Ease.InOutSine))
            .Append(transform.DOScale(scaleOriginal, duration).SetEase(Ease.InOutSine))
            .SetLoops(-1);
    }

    public void StopBouncing()
    {
        if (bounceSequence != null && bounceSequence.IsActive())
        {
            bounceSequence.Kill();
            transform.localScale = Vector3.one;
        }
    }

    void OnDestroy()
    {
        StopBouncing();
    }
    
    public void OnClick()
    {
        if (am != null)
        {
            am.PlaySFX(am.clickClip);
        }
        Debug.Log("Button clicked!");
    }
}