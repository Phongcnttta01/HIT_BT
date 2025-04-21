using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject intro;
    [SerializeField] private float loadingTime = 3f; // Thời gian tải

    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        float timer = 0f;
        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            float fillValue = timer / loadingTime;
            UpdateLoadingBar(fillValue);
            yield return null;
        }

        // Đảm bảo fill = 1 sau khi hoàn tất
        UpdateLoadingBar(1f);

        // Ẩn GameObject sau khi loading xong
        loading.SetActive(true);
        var pos = loading.transform.localScale;
        loading.transform.localScale = new Vector3(0, 0, 0);
        loading.transform.DOScale(pos, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
             gameObject.SetActive(false);
            
             loading.transform.DOScale(0f,0.5f).SetEase(Ease.InQuad).OnUpdate(() =>
             {
                 intro.SetActive(true);
             }).OnComplete(() =>
             {
                 loading.transform.localScale = pos;
                 loading.SetActive(false);
             });
        });
    }

    private void UpdateLoadingBar(float value)
    {
        loadingBar.fillAmount = value;
    }
}