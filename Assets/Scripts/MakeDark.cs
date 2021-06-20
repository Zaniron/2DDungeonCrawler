using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MakeDark : MonoBehaviour
{
    public Light2D globalLight;
    public bool lightOff = false;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.CompareTag("Player") && lightOff == false)
        {
            globalLight.enabled = false;
            lightOff=true;
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Play("BossBattle");
        }
    }
}
