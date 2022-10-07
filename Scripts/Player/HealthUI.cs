using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public float health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;

    

    private void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health / 10)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = halfHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void LoseHP(float ad)
    {
        health -= ad;
    }
}
