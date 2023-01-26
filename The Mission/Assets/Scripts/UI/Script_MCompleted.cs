using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Script_MCompleted : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int curLevel = PlayerPrefs.GetInt("curLevel");
        if ( Manager.actuallevel  == curLevel )
        {
            PlayerPrefs.SetInt("curLevel", curLevel+1);
            PlayerPrefs.Save();
        }

        Time.timeScale = 0f; //pause the game

        TextMeshProUGUI t = GameObject.Find("Text (Total)").GetComponent<TextMeshProUGUI>();
        if(Manager.TotalDeadHostages > 0) t.text = "Total Rescued Hostages: " + $"{(Manager.Rescued).ToString().AddColor(Color.red)}";
        else t.text = "Total Rescued Hostages: " + $"{(Manager.Rescued).ToString().AddColor(Color.cyan)}";


        System.TimeSpan time = System.TimeSpan.FromSeconds(Manager.TotalTime - Manager.RemainingTime);

        t = GameObject.Find("Text (Time)").GetComponent<TextMeshProUGUI>();
        if ((Manager.TotalTime - Manager.RemainingTime) / 60 >= 1.0f) t.text = "Elapsed Time: " + (time).ToString(@"mm\:ss") + $"{" Minutes".AddColor(Color.cyan)}";
        else t.text = "Elapsed Time: " + (time).ToString(@"ss") + $"{" Seconds".AddColor(Color.cyan)}";

        int star = 3;

        t = GameObject.Find("Text (Notice)").GetComponent<TextMeshProUGUI>();
        if(Manager.Noticed == false) t.text = "Noticed: " + $"{"No".AddColor(Color.cyan)}";
        else t.text = "Noticed: " + $"{"Yes".AddColor(Color.red)}";
         
        Image img = GameObject.Find("Star (1)").GetComponent<Image>();
        if (Manager.TotalDeadHostages > 0)
        {
            img.color = Color.black;
            star--;
        }
            

        img = GameObject.Find("Star (3)").GetComponent<Image>();
        if (Manager.Noticed == true)
        {
            img.color = Color.black;
            star--;
        }

        img = GameObject.Find("Star (2)").GetComponent<Image>();
        if (Manager.RemainingTime < Manager.TotalTime*0.3f)
        {
            img.color = Color.black;
            star--;
        }

        int curstar = PlayerPrefs.GetInt("star_Level" + Manager.actuallevel);

        if(curstar < star)
        { 
        PlayerPrefs.SetInt("star_Level" + Manager.actuallevel, star);
        PlayerPrefs.Save();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        //reset the all variables
        Manager.Rescued = 0;
        Manager.TotalDeadHostages = 0;
        Manager.TotalHostages = 0;
        Manager.Noticed = false;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.UnloadSceneAsync("MCompleted");
        SceneManager.LoadScene("MainMenu");


    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        //reset the all variables
        Manager.Rescued = 0;
        Manager.TotalDeadHostages = 0;
        Manager.TotalHostages = 0;
        Manager.Noticed = false;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.UnloadSceneAsync("MCompleted");
        
        SceneManager.LoadScene("Level (" + (Manager.actuallevel+1) + ")");
        Manager.actuallevel++;

    }

    



}
