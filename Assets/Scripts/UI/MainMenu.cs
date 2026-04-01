using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Canvas _canvas;

    public AudioSource _audioSource;


    public LoadingScene _loadingScene;
    public Camera_Follow _camera;

    public GameObject Main_menu;
    public GameObject Main_menu_Text;
    public GameObject Setting;
    public GameObject local;
    public GameObject Player;

    //public Camera _camera;
    void Start()
    {
        local.SetActive(false);
        Player.SetActive(false);
        _camera.isPlay = false;
        Setting.SetActive(false);
    }

    void Update()
    {
    }

    IEnumerator PlayGame()
    {
        _loadingScene.fadeImage.enabled = true;
        _loadingScene.FadeOut();
        _audioSource.Play();
        yield return new WaitForSeconds(2f);
        
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Main_menu.SetActive(false);
        local.SetActive(true);

        yield return new WaitForSeconds(1f);
        Player.SetActive(true);
        StartCoroutine(_loadingScene.FadeIn()); ;
    }

    public void Button_Play()
    {
        Debug.Log("click");
        StartCoroutine(PlayGame());
        _camera.isPlay = true;
    }    
    public void Button_Setting()
    {
        _audioSource.Play();
        Main_menu_Text.SetActive(false);
        Setting.SetActive(true);
    }     
    public void Button_SettingBacktoMenu()
    {
        _audioSource.Play();
        Setting.SetActive(false);
        Main_menu_Text.SetActive(true);       
    }    
    public void Button_Quit()
    {

    }

    public IEnumerator Button_BacktoMenu()
    {
        _loadingScene.FadeOut();
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;

        

        yield return new WaitForSeconds(1f);

        _loadingScene.FadeIn();
    }
}
