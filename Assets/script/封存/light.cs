using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    public Light mylight;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("f")&& mylight.enabled==true)
        {
            mylight.enabled = false;
        }
        else if(Input.GetKeyDown("f")&& mylight.enabled==false) 
        {
            mylight.enabled = true;
        }
        
    }
}
