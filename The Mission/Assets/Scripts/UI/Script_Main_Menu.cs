using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Script_Main_Menu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelpanel = null;
    public GameObject settingspanel = null;

    [Header("Stars")]
    public GameObject[] Stars;


    [Header("Design")]
    public Image Cloud;
    public Image Sight;


    public Sprite CloudNight;
    public Sprite SightNight;


    private static int width;

    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Debug.Log(SceneManager.sceneCount);
        if (width == 0)
        {
            width = Screen.currentResolution.width;
        }

        if(width == Screen.currentResolution.width)    Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);



        DateTime dt = DateTime.Now;
        int hourOfTheDay = dt.Hour;
        
        if((hourOfTheDay >= 0 && hourOfTheDay <= 6) || hourOfTheDay >= 20)
        {
            Cloud.sprite = CloudNight;
            Sight.sprite = SightNight;
        }
      


        Time.timeScale = 1;
    }


    public void StartTheGame()
    {
        if (levelpanel.activeInHierarchy == true)
        {
            levelpanel.SetActive(false);
            return;
        }
        settingspanel.SetActive(false);
        levelpanel.SetActive(true);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void Settings()
    {

        if(settingspanel.activeInHierarchy == true)
        {
            settingspanel.SetActive(false);
            return;
        }
            
        levelpanel.SetActive(false);
        settingspanel.SetActive(true);
    }


}
