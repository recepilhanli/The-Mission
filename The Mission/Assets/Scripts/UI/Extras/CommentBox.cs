using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommentBox : CommentBoxLite
{
    public RectTransform mainrectTransform;

    public RectTransform rectTransform;

    public TextMeshProUGUI tmp;




    public float[] points = new float[4];

    public string Comment;





  

   

    // Start is called before the first frame update
    void Start()
    {



       //rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform == null) Debug.Log("Error");

     
        GameObject canvas = Manager.Canvas;

        if (canvas != null)
        {
            mainrectTransform.SetParent(canvas.transform);
            rectTransform.SetParent(mainrectTransform.transform);
            rectTransform.anchoredPosition = new Vector2(points[0], points[1]) / 2;
          //  rectTransform.sizeDelta = new Vector2(points[2], points[3]) / 2;
        }
       
        if (Comment != tmp.text) tmp.text = Comment;


    }
    /*
    private void Update()
    {
        if (Comment != tmp.text) tmp.text = Comment;
    }
    */
    public static GameObject ShowComment(int id, string comment,float px = 1150, float py = 600, float sx = 1000, float sy = 600)
    {
    

        if (GameObject.Find("CommentBox(Clone)") != null) Destroy(GameObject.Find("CommentBox(Clone)"));

        GameObject comm = Instantiate(Resources.Load("Prefabs/UI/CommentBox", typeof(GameObject))) as GameObject;
        

        CommentBox commentBox;
        commentBox = comm.GetComponent<CommentBox>();
        commentBox.Comment = comment;
        commentBox.points[0] = px;
        commentBox.points[1] = py;
        commentBox.points[2] = sx;
        commentBox.points[3] = sy;


        return comm;
    }


}
