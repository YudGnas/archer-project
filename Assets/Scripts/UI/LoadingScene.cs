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
        fadeImage.DOFade(0f, duration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(duration);

        fadeImage.enabled = false;
    }

    public void FadeOut()
    {
        fadeImage.DOFade(1f, duration).SetEase(Ease.InOutQuad);
    }

    /*public void LoadScene(string sceneName)
    {
        FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }*/
}
