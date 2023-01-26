using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class LevelSelection : MonoBehaviour
{

    // Start is called before the first frame update

    public static string Selected;
    public bool stop = false;

    [SerializeField]
    private Sprite LevelScreen;

    [SerializeField]
    private Script_Main_Menu MainMenu;


    public void PressTheLevelButton()
    {
        int num = GetNumber(gameObject.name);
        int curLevel = PlayerPrefs.GetInt("curLevel");
        if (curLevel < num || num == 0)
        {
            NotificationController.ShowNotification("Before playing this level, you must complete level " + (num - 1) + "!");
            return;
        }
        Selected = gameObject.name;
        LevelPanel p = GameObject.Find("LevelPanel").GetComponent<LevelPanel>();

        p.LevelNum.text = "Level " + GetNumber(Selected);
   
        string[] LArray = Resources.Load<TextAsset>("Text/LevelInfo").text.Split('\n');

        string brief = LArray[num - 1].Replace(("Level " + (num) + " = "),""); 

        p.LevelBrief.text = brief;

        if(LevelScreen != null) p.LevelScreenShot.sprite = LevelScreen;

        p.LevelInfo.SetActive(true);

        for(int i = 0; i < MainMenu.Stars.Length; i++)
        {
            MainMenu.Stars[i].SetActive(false);
            
        }

        int star = PlayerPrefs.GetInt("star_Level" + num);
        Debug.Log(star);
        for (int i = 0; i < star; i++)
        {
            MainMenu.Stars[i].SetActive(true);
        }
    }


   public void StartLevel()
   {
        Manager.RestartGame(false);
        SceneManager.LoadScene(Selected);
        SceneManager.UnloadSceneAsync("MainMenu");
        Manager.actuallevel = GetNumber(Selected);
   }




    int GetNumber(string a)
    {


        string b = string.Empty;

        int val = 0;

        for (int i = 0; i < a.Length; i++)
        {
            if (Char.IsDigit(a[i]))
                b += a[i];
        }

        if (b.Length > 0)
            val = int.Parse(b);

        return val;
    }


    void Start()
    {
        if (stop == true) return;
        int curLevel = PlayerPrefs.GetInt("curLevel");

        if (curLevel <= 0) //showcase
        {
            PlayerPrefs.SetInt("curLevel", 1);
            PlayerPrefs.Save();
            curLevel = 1;
        }


        if(gameObject.GetComponent<Image>() == null)
        {
            Debug.Log("ERROR");
            return;
        }

        if (gameObject.GetComponentInChildren<Text>() == null)
        {
            Debug.Log("ERROR");
            return;
        }

        gameObject.GetComponentInChildren<Text>().text = GetNumber(gameObject.name).ToString();



        if (curLevel < GetNumber(gameObject.name))
        {
            gameObject.GetComponent<Image>().color = new Color(0.5f, 0.1f, 0.1f);
        }
            



    }

  
}
