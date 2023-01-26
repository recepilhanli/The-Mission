using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AI_Enemy : Entity
{
    // Start is called before the first frame update

    private Animator animator;
    private NavMeshAgent navMesh;
  


    public List<GameObject> hasSeen;
    public GameObject FocusOn = null;

    public float DistanceBetweenHostage = 2.2f;

    public static float sightdistance = 16.0f;


    public Vector3 SpawnPoint;

    public bool Distracted = false;

    private const int max_row = 10;

    private Manager manager;

    private float DefaultSpeed;

    void Step()
    {
        string randomsound;
        int random = Random.Range(0,5);

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
        DistanceBetweenHostage = DistanceBetweenHostage * gameObject.transform.localScale.x;

        sightdistance = 16.0f;
        hasSeen = new List<GameObject>();
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();

        SpawnPoint = transform.position;
        navMesh.autoBraking = false;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        if (manager == null) Debug.Log("Couldn't find manager.");

        DefaultSpeed = navMesh.speed;

    }


  



    private void Look()
    {
        if (manager == null) return;





        if (hasSeen.Count < Manager.TotalHostages - (Manager.TotalDeadHostages + Manager.Rescued))
        {
            for (int i = 0; i < manager.Hostages.Length; i++)
            {
                if (manager.Hostages[i].activeInHierarchy == false) continue;
                if (hasSeen.Contains(manager.Hostages[i])) continue;
                
                GameObject player = manager.Hostages[i];
                Entity entity = player.GetComponent<Entity>();

                if (entity == null || entity.alive == false) continue;

                Transform playerPos = manager.Hostages[i].transform;

                if (Vector3.Distance(transform.position, playerPos.position) < 2.15f)
                {
                    Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                    float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                    if (angleBetweenGuardAndPlayer < 360 / 2)
                    {
                        //  Debug.Log("viewAngle " + 270);
                        if (!Physics.Linecast(transform.position, playerPos.position))
                        {
                            Manager.PlayNoticedSoundEffect();
                            hasSeen.Add(player);
                            FocusOn = player;
                            Distracted = false;
                            Debug.Log("The hostage has been spotted (back).");                           
                            Manager.Noticed = true;
                        }
                    }
                }


                else if (Vector3.Distance(transform.position, playerPos.position) < 3.2f)
                {
                    Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                    float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                    if (angleBetweenGuardAndPlayer < 270 / 2)
                    {
                        //  Debug.Log("viewAngle " + 270);
                        if (!Physics.Linecast(transform.position, playerPos.position))
                        {
                            Manager.PlayNoticedSoundEffect();
                            hasSeen.Add(player);
                            FocusOn = player;
                            Distracted = false;
                            Debug.Log("The hostage has been spotted.");
                            Manager.Noticed = true;
                        }
                    }
                }

                else if (Vector3.Distance(transform.position, playerPos.position) < sightdistance)
                {
                    Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                    float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                    if (angleBetweenGuardAndPlayer < 135 / 2)
                    {
                        //  Debug.Log("viewAngle " + 270);
                        if (!Physics.Linecast(transform.position, playerPos.position))
                        {
                            Manager.PlayNoticedSoundEffect();
                            hasSeen.Add(player);
                            FocusOn = player;
                            Distracted = false;
                            Debug.Log("The hostage has been spotted.");
                            Manager.Noticed = true;
                        }
                    }
                }



    



            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (alive == false) return;

  
        if (navMesh.isStopped == false && FocusOn == null && navMesh.hasPath)
        {
            if (navMesh.remainingDistance <= 0.55f)
            {
                navMesh.ResetPath();
                navMesh.isStopped = true;
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsWalking", false);
            }
            
        }
        

        Look();


        if(hasSeen.Contains(FocusOn) == true && FocusOn == null) hasSeen.Remove(FocusOn); //disappeared

        if (hasSeen.Contains(FocusOn) == true && FocusOn.GetComponent<Entity>().alive == true)
        {
            navMesh.isStopped = false;

            if(hasSeen.Count > 1)
            {
                foreach(GameObject go in hasSeen)
                {
                    if(go == null)
                    {
                        hasSeen.Remove(go);
                        continue;
                    }

                    else if (go == FocusOn) continue;

                    if (Vector3.Distance(transform.position, go.transform.position) < Vector3.Distance(transform.position, FocusOn.transform.position)) FocusOn = go;
                }
            }

            if (Vector3.Distance(FocusOn.transform.position, transform.position) < DistanceBetweenHostage)
            {
                // Debug.Log("Enemy AI is near the hostage.");

                // navMesh.isStopped = true;

                 
                transform.LookAt(FocusOn.transform.position);

                animator.SetBool("IsMoving",false);
                animator.SetBool("IsWalking",false);

                animator.SetTrigger("Hit");

               
            }
      
            else
            {
                //Debug.Log("Enemy AI is chasing to the hostage.");
                navMesh.SetDestination(FocusOn.transform.position);
            }
        }
        else if (hasSeen.Count > 0)
        {

           

            foreach (GameObject go in hasSeen)
            {
                
                if(go == null || go.GetComponent<Entity>().alive == false)
                {
                    hasSeen.Remove(go);
                    break;
                }

                FocusOn = go;
                break;
            }
            

        }

        else if (hasSeen.Count == 0 && GetComponent<Patrolling>() == null && Distracted == false)
        {
           
            if ( Vector3.Distance(SpawnPoint, transform.position) > 0.5)
            {
                navMesh.speed = DefaultSpeed/4.5f;
                navMesh.isStopped = false;
                navMesh.SetDestination(SpawnPoint);              
            }
            else
            {
                navMesh.isStopped = true;
                animator.ResetTrigger("Hit");
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsWalking", false);
            }
   
        }








        if ((navMesh.isStopped == false && FocusOn == null) || (navMesh.isStopped == false && !(Vector3.Distance(FocusOn.transform.position, transform.position) < DistanceBetweenHostage)))
        {
           
                if (hasSeen.Count != 0)
                {
                    navMesh.speed = DefaultSpeed;
                    animator.SetBool("IsMoving", true);
                    animator.SetBool("IsWalking", false);
                }

                else
                {
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsWalking", true);

                }

            }
        

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hostage" && !hasSeen.Contains(other.gameObject))
        {
            if (other.GetComponent<Entity>().alive == false) return;
            hasSeen.Add(other.gameObject);
            FocusOn = other.gameObject;
            Distracted = false;
            Debug.Log("The hostage has been spotted.");
            Manager.Noticed = true;
        }

    }*/


}
