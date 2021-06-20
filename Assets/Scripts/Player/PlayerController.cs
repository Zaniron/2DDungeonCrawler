using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    private int currentHealth;
    public HealthBar healthBar;
    public TextMeshProUGUI gemCounter;
    public Animator anim;

    private int gemCount;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void GainHealth(int healthGain)
    {
        currentHealth += healthGain;
        healthBar.SetHealth(currentHealth);

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

    public void Update()
    {
        anim.SetFloat("health", currentHealth);
    }

    public void AddGem(int gem)
    {
        gemCount += gem;
        Debug.Log(gemCount);
        gemCounter.text = gemCount.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Took " + damage + " damage");
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Destroy(gameObject);
        FindObjectOfType<GameManager>().EndGame();
    }
}
