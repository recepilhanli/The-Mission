using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{

    [SerializeField] public string NotificationText;


    private float cooldown = 7.1f;

    private float yaxxis = 40.0f;
 
    private Text ConText;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        ConText = gameObject.GetComponentInChildren<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform == null) Debug.Log("Error");

        GameObject canvas = Manager.Canvas;

        

        if(canvas != null)
        {
            rectTransform.anchoredPosition = new Vector2(0, yaxxis);
            rectTransform.SetParent(canvas.transform);
        }

    }

    public static GameObject ShowNotification(string notification)
    {
      
        if (GameObject.Find("Notification(Clone)") != null) Destroy(GameObject.Find("Notification(Clone)"));

        

        GameObject notf = Instantiate(Resources.Load("Prefabs/UI/Notification", typeof(GameObject))) as GameObject;
        notf.GetComponentInChildren<NotificationController>().NotificationText = notification;

        

        if (notf.GetComponentInChildren<Text>() == null || notf == null) Debug.Log("ERROR");
        return notf;
    }



    // Update is called once per frame
    void Update()
    {
        if (ConText.text != NotificationText)
        {
            ConText.text = NotificationText;
        }
            
        if (cooldown <= 0.0f && yaxxis >= 40.0f)
        {
            Destroy(gameObject);
            return;
        }
        else if(cooldown <= 0.0f)
        {
            yaxxis += Time.deltaTime * 200;
            rectTransform.anchoredPosition = new Vector2(0, yaxxis);
           
            return;
        }

        else if(cooldown <= 7.0f)
        {
            cooldown -= Time.deltaTime;
            
            return;
        }

        if (yaxxis <= -40 && cooldown == 7.1f)
        {
            cooldown = 7.0f;
            return;
        }
        
        yaxxis -= Time.deltaTime * 250;
        rectTransform.anchoredPosition = new Vector2(0, yaxxis);
        
    }
}
