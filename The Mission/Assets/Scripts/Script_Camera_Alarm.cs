using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Camera_Alarm: MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
     //   transform.localRotation = Quaternion.Euler(85, transform.localRotation.y, transform.localRotation.z);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hostage")
        {
            Manager.PlayNoticedSoundEffect();
            Manager.Noticed = true;
            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for(int i = 0; i < Enemies.Length; i++)
            {
               
                if (Enemies[i].GetComponent<AI_Enemy>() != null)
                {
                    AI_Enemy ai = Enemies[i].GetComponent<AI_Enemy>();
                    if (ai.hasSeen.Contains(other.gameObject)) continue;
                    ai.hasSeen.Add(other.gameObject);
                }
                    
            }

        }
    }

}
