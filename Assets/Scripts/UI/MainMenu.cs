using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Player_Retry _Retry;

    public AudioSource _audioSource;


    public LoadingScene _loadingScene;
    public Camera_Follow _camera;

    public GameObject Main_menu;
    public GameObject Main_menu_Text;
    public GameObject Setting;
    public GameObject SettingESC;
    public GameObject GameOver;


    public GameObject BacktoMenu;
    public bool isActive_BacktoMenu;
    public AudioSource local_audio;

    public Player_Controller _player;

    //public Camera _camera;
    void Start()
    {
        local_audio = _player.local_audio;
        _camera.isPlay = false;
        Setting.SetActive(false);
        SettingESC.SetActive(false);
        BacktoMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (!isActive_BacktoMenu)
            {
                BacktoMenu.SetActive(true);
                isActive_BacktoMenu = true;
                Time.timeScale = 0f;
            }
            else
            {
                BacktoMenu.SetActive(false);
                isActive_BacktoMenu = false;
                Time.timeScale = 1f;
            }
        } 
    }

    IEnumerator PlayGame()
    {
        _loadingScene.FadeOut();
        _audioSource.Play();
        yield return new WaitForSecondsRealtime(2f);
        
        Main_menu.SetActive(false);
        local_audio.Play();
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);       
        StartCoroutine(_loadingScene.FadeIn());
    }

    public void Button_Continue()
    {
        BacktoMenu.SetActive(false);
        isActive_BacktoMenu = false;
        Time.timeScale = 1f;
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
    public void Button_Setting_fromECS()
    {
        _audioSource.Play();
        BacktoMenu.SetActive(false);
        SettingESC.SetActive(true);
    }     
    public void Button_SettingBacktoMenu()
    {
        _audioSource.Play();
        Setting.SetActive(false);
        Main_menu_Text.SetActive(true);       
    }    
    public void Button_SettingBacktoESC()
    {
        _audioSource.Play();
        SettingESC.SetActive(false);
        BacktoMenu.SetActive(true);       
    }    
    public void Button_Quit()
    {
        Application.Quit();
    }

    public IEnumerator BacktoMenu_fc()
    {
        _loadingScene.FadeOut();
        _audioSource.Play();

        local_audio = _player.local_audio;
        yield return new WaitForSecondsRealtime(1f);

        local_audio.Stop();
        local_audio.time = 0f;
        BacktoMenu.SetActive(false);
        Main_menu.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(_loadingScene.FadeIn());
        
    }    
    public IEnumerator BacktoMenu_from_GameOVer()
    {
        _loadingScene.FadeOut();
        _audioSource.Play();

        local_audio = _player.local_audio;
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        
        local_audio.Stop();
        local_audio.time = 0f;
        GameOver.SetActive(false);
        _Retry.OnRetry();
        Main_menu.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        StartCoroutine(_loadingScene.FadeIn());
        
    }    
    public IEnumerator PlayerRetry()
    {
        _loadingScene.FadeOut();
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        
        GameOver.SetActive(false);
        _Retry.OnRetry();
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(_loadingScene.FadeIn());
        
    }
    public void Button_BacktoMenu()
    {
        StartCoroutine(BacktoMenu_fc());
    }    
    public void Button_BacktoMenu_from_GameOver()
    {
        StartCoroutine(BacktoMenu_from_GameOVer());
    }
    public void Button_Retry()
    {
        StartCoroutine(PlayerRetry());
    }
}
