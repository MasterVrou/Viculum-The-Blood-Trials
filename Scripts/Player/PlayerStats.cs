using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private GameMaster gm;

    [SerializeField]
    private float maxHealth;

    public float currentHealth;

    //public HealthUI healthUI;

    private void Start()
    {
        currentHealth = maxHealth;

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

    }

    public void DecreasedHealth(float amount)
    {
        currentHealth -= amount;
        //healthUI.health = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (SceneManager.GetActiveScene().name == "Malum Fight Room")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Destroy(gameObject);
    }
}
