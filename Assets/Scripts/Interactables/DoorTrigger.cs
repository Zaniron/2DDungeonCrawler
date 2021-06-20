using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorTriggerLight;
    public GameObject doorLight;
    public GameObject door;
    public Animator doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.CompareTag("Projectile"))
        {
            doorLight.SetActive(true);
            doorTriggerLight.SetActive(true);
            doorAnim.SetTrigger("DoorOpening");
            FindObjectOfType<AudioManager>().Play("DoorTrigger");
        }
    }
}
