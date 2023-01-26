using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Pause : MonoBehaviour
{
    public void ReturnMenu()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    } 


    public void Restrat()
    {
        ResumeTheGame();
        Manager.RestartGame();
    }


    



    // Start is called before the first frame update

}
