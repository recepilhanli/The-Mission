using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_Distcrating : MonoBehaviour
{

    public GameObject[] ToDistract;

    private Outline outline;

    private float cooldown = 0f;

    public float CoolDownTime = 8f;

    // Start is called before the first frame update

    private Material oldMat;
    [SerializeField]
    private Material newMat;
    private Renderer dRenderer;

    private Transform childTrans;
    void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.OutlineColor = Color.cyan;

        dRenderer = gameObject.GetComponent<Renderer>();
        oldMat = dRenderer.material;

        
    }

    public void Distract()
    {
        if (outline.OutlineColor == Color.red) return;
        if (cooldown != 0f) return;
        cooldown = CoolDownTime;
        outline.OutlineColor = Color.yellow;
        if(newMat != null) dRenderer.material = newMat;
        Manager.PlaySound("Effects/distract",0.5f);

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
        cooldown = 0f;
        if(outline.OutlineColor != Color.red) outline.OutlineColor = Color.cyan;
        if (newMat != null) dRenderer.material = oldMat;
        for (int i = 0; i< ToDistract.Length; i++)
        {
            
            AI_Enemy ai = ToDistract[i].GetComponent<AI_Enemy>();
            ai.Distracted = false;
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
}
