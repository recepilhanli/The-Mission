using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class StringExtensions
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}


public class Script_UI : MonoBehaviour
{
    [SerializeField]
    private Manager manager;

    private Text Text_PR;

    private Text Text_Selection;

    private TextMeshProUGUI Text_Rescued;

    private TextMeshProUGUI Text_Time;


    private Manager Manager;


    private GameObject pause = null;


    [SerializeField]
    private TextMeshProUGUI Text_FPS;

  




    private void Awake()
    {

        GameObject OBJ;

        OBJ = GameObject.Find("Text (Pause)");
        if (OBJ == null) Debug.Log("Pause object initialization failed.");
        Text_PR = OBJ.GetComponent<Text>();

        if(manager.GameMode == 0)
        { 
        OBJ = GameObject.Find("Text (Selection)");
        if (OBJ == null) Debug.Log("Selection object initialization failed.");
        Text_Selection = OBJ.GetComponent<Text>();
        }



        if (Text_PR == null) Debug.Log("Pause Button Text initialization failed.");
        if (Text_Selection == null) Debug.Log("Select Button Text initialization failed.");


        OBJ = GameObject.Find("Manager");
        Manager = OBJ.GetComponent<Manager>();

        OBJ = GameObject.Find("Text (Rescued)");
        Text_Rescued = OBJ.GetComponent<TextMeshProUGUI>();

        OBJ = GameObject.Find("Text (Time)");
        Text_Time = OBJ.GetComponent<TextMeshProUGUI>();



        if (Manager == null) Debug.Log("The Manager Object couldn't find.");
    }


    void UpdateTexts()
    {

        if (Text_FPS != null)
        {
            Text_FPS.text = "FPS: " + 1f / Time.deltaTime;
        }

        if(manager.GameMode == 0)
        { 
        if((Manager.SelectedHostages.Count - Manager.TotalDeadHostages) == Manager.TotalHostages && Text_Selection.text != "Deselect All")
        {
            Text_Selection.text = "Deselect All";
            Text_Selection.color = Color.white;
        }
        else if ((Manager.SelectedHostages.Count - Manager.TotalDeadHostages) != Manager.TotalHostages && Text_Selection.text != "Select All")
        {
            Text_Selection.text = "Select All";
            Text_Selection.color = new Color(0.0011f, 1, 0);
        }
        }


        if (manager.GameMode == 0)
        {
        if (Manager.TotalDeadHostages == 0) Text_Rescued.text = "Rescued: " + Manager.Rescued + "/" + Manager.TotalHostages;

        else Text_Rescued.SetText("Rescued: " + Manager.Rescued + "/" + $"{(Manager.TotalHostages - Manager.TotalDeadHostages).ToString().AddColor(Color.red)}");
        }

        else if (manager.GameMode == 1)
        {
        Text_Rescued.SetText("Steal the diamond.");
        }

        System.TimeSpan time = System.TimeSpan.FromSeconds(Manager.RemainingTime);
       

        if (Manager.RemainingTime/60 >= 1.0f) Text_Time.text = "Time: " + (time).ToString(@"mm\:ss") + "m";
        else Text_Time.text = "Time: " + (time).ToString(@"ss") + "s";
    }


    




    public void Selection()
    {
        if (Time.timeScale == 0) return;
        if (Manager.Hostages.Length - Manager.Rescued == 0) Text_Selection.text = "All hostages were rescued.";
        else if (Manager.SelectedHostages.Count == Manager.Hostages.Length - Manager.Rescued)
        {
            Text_Selection.text = "Select All";
            Text_Selection.color = new Color(0.0011f, 1, 0);
            for (int i = 0; i < Manager.Hostages.Length; i++)
            {
                if (Manager.Hostages[i] == null) continue;             
                else if (!Manager.SelectedHostages.Contains(Manager.Hostages[i])) continue;

                Manager.SelectHostage(Manager.Hostages[i], false);
            }

        }
        else
        {

            Text_Selection.text = "Deselect All";
            Text_Selection.color = Color.white;
            for (int i = 0; i < Manager.Hostages.Length; i++)
            {
                if (Manager.Hostages[i] == null) continue;
                else if (Manager.Hostages[i].activeInHierarchy == false) continue;
                else if (Manager.SelectedHostages.Contains(Manager.Hostages[i])) continue;

                Manager.SelectHostage(Manager.Hostages[i], true);
            }

        }

    }

    private void Update()
    {
        UpdateTexts();

        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }


    public void PauseGame()
    {

            if (pause != null) return;
          

            pause = Instantiate(Resources.Load("Prefabs/UI/Pause", typeof(GameObject))) as GameObject;

            RectTransform rectTransform = pause.GetComponent<RectTransform>();

            if (rectTransform == null) Debug.Log("ERROR");


            rectTransform.position = Manager.Canvas.transform.position;
            rectTransform.sizeDelta = new Vector2(0, 0);
            rectTransform.SetParent(Manager.Canvas.transform);

            Time.timeScale = 0;

    }





    



}
