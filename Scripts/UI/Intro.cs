using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject menu;
    

    void Start()
    {
        
        // Gắn sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        loading.SetActive(true);
        var originalScale = loading.transform.localScale;
        loading.transform.localScale = Vector3.zero;

        // Scale loading lên
        yield return loading.transform
            .DOScale(originalScale, 0.5f)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
        menu.SetActive(true);
        // Tắt VideoPlayer
        gameObject.SetActive(false);

        // Scale loading nhỏ lại
        yield return loading.transform
            .DOScale(0f, 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                loading.transform.localScale = originalScale;
                loading.SetActive(false);
            });

    }
}