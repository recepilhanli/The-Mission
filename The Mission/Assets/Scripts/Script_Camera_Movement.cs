using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Camera_Movement : MonoBehaviour
{
    
    private float zaxis = 0f;
    private float cooldown = 0f;
    private int pingpong = 0;


  

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        if (pingpong == 0) zaxis += 35 * Time.deltaTime;
        else if (pingpong == 1) zaxis += -35 * Time.deltaTime;

        if (zaxis >= 60 && pingpong == 0)
        {
            cooldown = 4.5f;
            pingpong = 1;
            return;
        }
        else if (zaxis <= -60 && pingpong == 1)
        {
            cooldown = 4.5f;
            pingpong = 0;
            return;
        }

       transform.localRotation = Quaternion.Euler(-90, -zaxis, 0);

    }
}
