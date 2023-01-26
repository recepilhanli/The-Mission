using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_GameOver : MonoBehaviour
{
    public string reason;

    private Text ReasonText;

    public void GoBackTheMenu()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MainMenu");
        
    }

    public void RestartTheGame()
    {
        Manager.RestartGame();
    }


    // Start is called before the first frame update
    void Awake()
    {
        ReasonText = GameObject.Find("Text (Reason)").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ReasonText.text != reason)
        {
            ReasonText.text = reason;
        }
    }
}
