using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public Image fadeImage;
    public GameObject fadeImage_GO;
    public float duration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());       
    }

    public IEnumerator FadeIn()
    {
        fadeImage.DOFade(0f, duration).SetEase(Ease.InOutQuad).SetUpdate(true);

        yield return new WaitForSecondsRealtime(duration);

    }

    public void FadeOut()
    {
        fadeImage.DOFade(1f, duration).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    /*public void LoadScene(string sceneName)
    {
        FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }*/
}
