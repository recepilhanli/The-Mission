using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PassiveObject;
    public string TriggererTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == TriggererTag)
        {
            PassiveObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
