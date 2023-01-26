using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuePoint : MonoBehaviour
{


    public int GameMode = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hostage" && other.gameObject.activeInHierarchy == true)
        {

            if (GameMode == 0)
            {
                Manager.Rescued++;

                other.gameObject.GetComponent<AI_Hostage>().Kill(true);

                Manager.SortHostages();
            }


            else if (GameMode == 1)
            {
                AI_Hostage actor = other.gameObject.GetComponent<AI_Hostage>();
                if(actor.mission == 1)
                {
                    Manager.Rescued++;

                    other.gameObject.GetComponent<AI_Hostage>().Kill(true);

                    Manager.SortHostages();
                }
            
            }


        }
    }

}
