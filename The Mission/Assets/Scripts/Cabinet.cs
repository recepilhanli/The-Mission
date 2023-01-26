using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    [SerializeField]
    private Transform cabinetdoor;
    float zaxis = 1; 
    int pingpong;
    float cooldown = 0f;
    [System.NonSerialized]
    public float SelectionCountDown = 0f;
    
    Outline outline;


    [System.NonSerialized]
    public GameObject Hided;

    public GameObject stored;
    [SerializeField]
    private GameObject spawnPos;



    // Start is called before the first frame update
    void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.OutlineColor = Color.white;
        stored.SetActive(false);
    }

   


    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0f && Hided == null) cooldown -= Time.deltaTime;
        else if(cooldown < 0f && Hided == null) cooldown = 0f;

        if (SelectionCountDown > 0f && Hided == null) SelectionCountDown -= Time.deltaTime;
        else if (SelectionCountDown < 0f && Hided == null) SelectionCountDown = 0f;

        if (pingpong != 0)
        {


            if (zaxis >= 115)
            {
                pingpong++;
                
                zaxis = 115;

                
            }

            else if (zaxis <= 0)
            {
                pingpong = 3;
                zaxis = 0;
            }

      

            if (pingpong == 1) zaxis += 240 * Time.deltaTime;
            else if (pingpong == 2) zaxis -= 240 * Time.deltaTime;

            cabinetdoor.localRotation = Quaternion.Euler(-90, -zaxis, 0);
            


            if (pingpong == 3)
            {
                zaxis = 1;
                pingpong = 0;
            }

        }
    }


    void AnimateTheDoor()
    {
        if (pingpong == 0)
        {
            pingpong = 1;
            
        }
            

    }


    public void GetOut()
    {
   
        if (Hided == null) return;
 
        Manager.PlaySound("Effects/DoorOpening", 1f);
        cooldown = 2f;
        SelectionCountDown = 0f;
        outline.OutlineColor = Color.white;
        AnimateTheDoor();
        Hided.SetActive(true);
        Hided.transform.position = spawnPos.transform.position;
        Hided.gameObject.GetComponent<AI_Hostage>().cabinet = null;
        Hided = null;
        if (stored != null) stored.SetActive(false);
        
    }

    public void GetIn(GameObject other)
    {
        if (Hided == null && cooldown == 0f && other.tag == "Hostage")
        {
            Manager.PlaySound("Effects/DoorOpening", 1f);

            other.gameObject.GetComponent<AI_Hostage>().cabinet = gameObject.GetComponent<Cabinet>();


            Hided = other;
            cooldown = 1.5f;

            other.SetActive(false);

            outline.OutlineColor = Color.green;
            if (stored != null) stored.SetActive(true);
            AnimateTheDoor();

        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        GetIn(other.gameObject);
        
        
    }*/



}
