using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_Electricity : MonoBehaviour
{

    public GameObject[] ToDistract;

    private Outline outline;

    [System.NonSerialized]
    public float cooldown = 0f;

    public float CoolDownTime = 8f;

    public GameObject[] Cameras;

    
    // Start is called before the first frame update
    void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.OutlineColor = Color.green;

      
    }

    public void Run()
    {
        Debug.Log("Test");
        if (cooldown != 0f) return;

        Manager.PlaySound("Effects/PowerDown");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Distracting");
        foreach(GameObject go in gameObjects)
        {
            if (go.GetComponent<Outline>() != null) go.GetComponent<Outline>().OutlineColor = Color.red;
        }

        for(int i = 0; i<Cameras.Length;i++)
        {
            Cameras[i].SetActive(false);
        }

        cooldown = CoolDownTime;
        outline.OutlineColor = new Color(0,0,0);

        NotificationController.ShowNotification("The lights were turned off.");


        if (Manager.Lights != null) Manager.Lights.SetActive(false);

        AI_Enemy.sightdistance = 6f;


        for (int i = 0; i < ToDistract.Length; i++)
        {
            NavMeshAgent nav = ToDistract[i].GetComponent<NavMeshAgent>();
            AI_Enemy ai = ToDistract[i].GetComponent<AI_Enemy>();
            if (nav == null) Debug.Log("Error: Could not find the specified guard.");
            ai.Distracted = true;
            nav.speed = 1.5f;
            nav.isStopped = false;


            Transform childTrans = gameObject.transform.Find("Distraction_Point");
            if (childTrans == null) Debug.Log("Couldn't find Disctraction Point");
            nav.SetDestination(childTrans.position);

        }

    }

    public void GetReady()
    {
        if (cooldown == 0f) return;

        NotificationController.ShowNotification("The lights were turned on.");

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Distracting");
        foreach (GameObject go in gameObjects)
        {
            if (go.GetComponent<Outline>() != null) go.GetComponent<Outline>().OutlineColor = Color.cyan;
        }

        AI_Enemy.sightdistance = 16f;
        cooldown = 0f;
        outline.OutlineColor = Color.green;
        for(int i = 0; i< ToDistract.Length; i++)
        {
            
            AI_Enemy ai = ToDistract[i].GetComponent<AI_Enemy>();
            ai.Distracted = false;

            

        }
        if(Manager.Lights != null) Manager.Lights.SetActive(true);

        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].SetActive(true);
        }

    }
    void Update()
    {
        if(cooldown != 0f)
        {
           cooldown -= Time.deltaTime;
            if (cooldown <= 0f)
            {
                GetReady();
            }
        }

    }



    private void OnTriggerEnter(Collider other)
    {
    if(other.gameObject.tag == "Hostage")
        {
            Run();
        }

    }



}
