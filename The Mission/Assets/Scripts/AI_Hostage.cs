using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_Hostage : Entity
{
    // Start is called before the first frame update

    private Animator animator;
    private NavMeshAgent navMesh; 
    float speed = 0f;

    
    public Marker marker;
    private Outline outline;


    [System.NonSerialized]
    public Cabinet cabinet;

    public int mission;

    void Step()
    {
        string randomsound;
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                randomsound = "walk (1)";
                break;

            case 1:
                randomsound = "walk (3)";
                break;

            case 2:
                randomsound = "walk (3)";
                break;

            case 3:
                randomsound = "walk (1)";
                break;

            default:
                randomsound = "walk (1)";
                break;
        }


        Manager.PlaySound("Effects/" + randomsound, 0.03f);
    }

   
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();  
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        outline = gameObject.GetComponent<Outline>();
      
        navMesh.autoBraking = false;
    }

    // Update is called once per frame




    void FixedUpdate()
    {
      
        speed = Mathf.Abs(navMesh.velocity.x + navMesh.velocity.y + navMesh.velocity.z);


        if(marker != null)
        {
            marker.Color = outline.OutlineColor;
        }

        if (navMesh.hasPath)
        {
            if (navMesh.remainingDistance <= 1.0f)
            {

                navMesh.velocity = Vector3.zero;
                navMesh.isStopped = true;
                navMesh.ResetPath();
                speed = 0.0f;
            }
        }


        if (speed >= 0.01f && navMesh.isStopped == false) animator.SetBool("IsMoving", true);
        else animator.SetBool("IsMoving", false);



    }



}
