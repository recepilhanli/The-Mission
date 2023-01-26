using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentBoxLite : MonoBehaviour
{
    //constants
    const int DIALOG_TUT1 = 0;
    const int DIALOG_TUT2 = 1;
    const int DIALOG_TUT3 = 2;
    const int DIALOG_TUT4 = 3;
    const int DIALOG_TUT5 = 4;

    public int CommentID = -1;

    public static GameObject commentobj;

    [SerializeField]
    private GameObject TurnOn;

    void Start()
    {
        if (CommentID == DIALOG_TUT1 || CommentID == DIALOG_TUT2 || CommentID == DIALOG_TUT3 || CommentID == DIALOG_TUT4 || CommentID == DIALOG_TUT5) Time.timeScale = 0;

    }

    public void OnClick()
    {
        
        if (CommentID >= 0) OnPlayerClickCommentBox(CommentID);
        Destroy(gameObject);

    }
    public void OnPlayerClickCommentBox(int ID)
    {
        if (ID == DIALOG_TUT1)
        {
            Manager.PlaySound("Effects/distract");
            Time.timeScale = 1;
        }

        else if (ID == DIALOG_TUT2)
        {
            Manager.PlaySound("Effects/distract");
            Time.timeScale = 1;
        }
        else if (ID == DIALOG_TUT3)
        {
            Manager.PlaySound("Effects/distract");
            Time.timeScale = 1;
        }
        else if (ID == DIALOG_TUT4)
        {
            Manager.PlaySound("Effects/distract");
            Time.timeScale = 1;
        }
        else if (ID == DIALOG_TUT5)
        {
            Manager.PlaySound("Effects/distract");
            Time.timeScale = 1;
        }
        CommentID = -1;
    }

}
