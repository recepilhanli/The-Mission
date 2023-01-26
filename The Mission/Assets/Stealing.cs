using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealing : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test");
        if(other.gameObject.tag == "Hostage")
        {
            other.gameObject.GetComponent<AI_Hostage>().mission = 1;
            Destroy(gameObject);
            Manager.PlaySound("Effects/distract");
        }
    }
}
