using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthGain = 5;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerController player = hitInfo.GetComponent<PlayerController>();
        if(hitInfo.CompareTag("Player"))
        {
            player.GainHealth(healthGain);
            FindObjectOfType<AudioManager>().Play("GemPickup");
            Destroy(gameObject);
        }
    }
}
