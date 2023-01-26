using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public bool alive = true;
    [Space]

    [System.NonSerialized]
    public Animator eAnim;
    [System.NonSerialized]
    public Collider eCollider;
    [System.NonSerialized]
    public NavMeshAgent eNav;
    [System.NonSerialized]
    public Rigidbody eRig;

    // Start is called before the first frame update

    void GetAllComponents()
    {
        eAnim = gameObject.GetComponent<Animator>();
        eCollider = gameObject.GetComponent<CapsuleCollider>();
        eNav = gameObject.GetComponent<NavMeshAgent>();
        eRig = gameObject.GetComponent<Rigidbody>();
        
    }

    void Awake()
    {
      
        if (alive == false) Kill();

        GetAllComponents();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (alive == false || eAnim == null) return;
    }

    public void Kill(bool forrescue = false)
    {
        if (eAnim == null) return;
        alive = false;

        gameObject.name = gameObject.name + " [Dead]";

        if (gameObject.activeInHierarchy == false && forrescue == false) gameObject.SetActive(true);
        else if (forrescue == true) gameObject.SetActive(false);

        
        if (gameObject.tag == "Hostage")
        {
         

            if (Manager.SelectedHostages.Contains(gameObject))
            {
                Manager.SelectedHostages.Remove(gameObject);
                //Manager.SelectedHostages.Sort();
            }

        


            if (forrescue == false)
            {     
            Manager.PlaySound("Effects/Punch", 0.3f);
            Manager.TotalDeadHostages++;
            }
            eNav.ResetPath();
            eNav.isStopped = true;
            eAnim.ResetTrigger("Kill");
            eAnim.SetBool("IsMoving", false);
            eAnim.SetBool("IsWalking", false);
            if (forrescue == false) eAnim.SetTrigger("Kill");

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemyo in enemies)
            {
                AI_Enemy enemy = enemyo.GetComponent<AI_Enemy>();
                if (enemy != null && enemy.FocusOn == gameObject)
                {
                    enemy.FocusOn = null;
                    enemy.hasSeen.Remove(gameObject);
                    enemy.hasSeen.Sort();
                }
                    
            }

            
            AI_Hostage h = gameObject.GetComponent<AI_Hostage>();
            Destroy(h.marker.gameObject);

            if (h.cabinet != null) h.cabinet.GetOut();
            Destroy(h);
            Destroy(gameObject.GetComponent<Outline>());
            Destroy(eRig);
            Destroy(eCollider);
            Destroy(eNav);
            gameObject.AddComponent<TraumaInducer>();
           
        }

        else if (eAnim.tag == "Enemy")
        {
            eNav.isStopped = true;
            eAnim.ResetTrigger("Kill");
            eAnim.SetBool("IsMoving", false);
            eAnim.SetBool("IsWalking", false);
            eAnim.SetTrigger("Kill");

            Destroy(gameObject.GetComponent<AI_Hostage>());
            Destroy(gameObject.GetComponent<Outline>());
            if(gameObject.GetComponent<Patrolling>() != null) Destroy(gameObject.GetComponent<Patrolling>());
            Destroy(eRig);
            Destroy(eCollider);
            Destroy(eNav);
        }

        gameObject.tag = "Dead";

        GetAllComponents();


    }




}
