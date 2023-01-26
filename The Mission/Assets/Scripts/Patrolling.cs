using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    // This was maden for Enemy_AI script.

    [Header("Way Points")]
    public Transform[] WayPoints;

    private AI_Enemy e;
    private NavMeshAgent navMesh;

    private int PatrolCount = -1;
    private bool PatrolCountGoingDown = false;
    private Animator animator;

    public void Patrol()
    {

        if (WayPoints.Length == 0) return;


        if(e != null)
        { 
        if(e.hasSeen.Count != 0)
        {
            navMesh.speed = 5.5f;
            return;
        }
        }
       
        if (PatrolCountGoingDown == false) PatrolCount++;
        else PatrolCount--;

        if (PatrolCount == WayPoints.Length && PatrolCountGoingDown == false)
        {
            PatrolCount -= 2;
            PatrolCountGoingDown = true;
        }

        else if (PatrolCount == -1 && PatrolCountGoingDown == true)
        {
            PatrolCount += 2;
            PatrolCountGoingDown = false;
        }
       

        if(e != null) navMesh.speed = 1.5f;
        else navMesh.speed = 5.5f;

        navMesh.ResetPath();
        navMesh.SetDestination(WayPoints[PatrolCount].position);
        navMesh.isStopped = false;
        

    }



    void Start()
    {
  
        if (gameObject.tag == "Enemy")
        {
            e = gameObject.GetComponent<AI_Enemy>();
        }
        else
        {
            animator = gameObject.GetComponent<Animator>();
        }   

        navMesh = gameObject.GetComponent<NavMeshAgent>();
    
    }





    // Update is called once per frame
    void FixedUpdate()
    {


        if(e != null)
        {

       
        if (e.hasSeen.Count == 0)
        {
            if (PatrolCount == -1) Patrol();
            else if (navMesh.remainingDistance < 0.1) Patrol();

        }
        else navMesh.speed = 5.5f;

        }
        else
        {
            animator.SetBool("IsMoving", true);
            if (PatrolCount == -1) Patrol();
            else if (navMesh.remainingDistance < 0.1) Patrol();
        }
    }
}
