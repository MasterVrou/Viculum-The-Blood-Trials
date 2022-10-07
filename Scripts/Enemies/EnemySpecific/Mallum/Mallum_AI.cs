using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mallum_AI : MonoBehaviour
{
    private GameObject aliveGO;
    private GameObject guard;
    private GameObject archer;
    private GameObject guard2;
    private GameObject archer2;
    private GameObject guard3;
    private GameObject archer3;
    private GameObject eyes;

    private Rigidbody2D rb;
    private Animator anim;

    private float time;
    private float movementSpeed = 5.0f;
    public float maxHealth = 100;
    public float currentHealth;
    
    //once booleans allow the code to be parsed only once
    private bool onceRight = true;
    private bool onceLeft = false;
    private bool onceSpawn = false;
    private bool onceSpawn2 = false;
    public bool onceTeleport = false;

    public bool onceIsFalling = false;
    public bool twiceIsFalling = false;

    public bool onceEndSong = false;

    public bool enemiesSpawn = false;
    public bool enemiesSpawn2 = false;

    public bool isFalling = false;

    private string songPlaying;

    // Start is called before the first frame update
    void Start()
    {
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();

        guard = transform.Find("Enemy1").gameObject;
        guard.SetActive(false);
        archer = transform.Find("Archer").gameObject;
        archer.SetActive(false);

        guard2 = transform.Find("Enemy12").gameObject;
        guard2.SetActive(false);
        archer2 = transform.Find("Archer2").gameObject;
        archer2.SetActive(false);

        guard3 = transform.Find("Enemy13").gameObject;
        guard3.SetActive(false);
        archer3 = transform.Find("Archer3").gameObject;
        archer3.SetActive(false);

        eyes = transform.Find("Eyes").gameObject;
        eyes.SetActive(false);

        currentHealth = maxHealth;

        FindObjectOfType<AudioManager>().Play("MalumTalk");
        FindObjectOfType<AudioManager>().Play("MalumTheme1");

        songPlaying = "MalumTheme2";
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;

        TimeFunction();
        ManageEnemies();

        MallumFall();

        if (!aliveGO.activeSelf)
        {
            eyes.SetActive(false);
        }

        if (!onceEndSong)
        {
            if (!FindObjectOfType<AudioManager>().StillPlaying())
            {
                FindObjectOfType<AudioManager>().Play2(songPlaying);
            }
            

            if(songPlaying == "MalumTheme4")
            {
                onceEndSong = true;
            }
        }


        if (onceEndSong && !FindObjectOfType<AudioManager>().StillPlaying())
        {
            SceneManager.LoadScene("End Scene");
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void MallumFall()
    {
        if (!guard.activeSelf && !archer.activeSelf && !onceIsFalling && onceSpawn)
        {
            isFalling = true;
            onceIsFalling = true;
        }
        if (currentHealth < 70 && !onceTeleport)
        {
            rb.position = new Vector2(rb.position.x, 9);
            isFalling = false;

            onceTeleport = true;
            eyes.SetActive(true);
            enemiesSpawn2 = true;
            onceSpawn2 = true;
        }
        else if (!guard2.activeSelf && !archer2.activeSelf && !guard3.activeSelf && !archer3.activeSelf && onceSpawn2 && !twiceIsFalling)
        {
            isFalling = true;
            twiceIsFalling = true;
        }

    }

    private void ManageEnemies()
    {
        if (enemiesSpawn)
        {
            guard.SetActive(true);
            archer.SetActive(true);
            
            enemiesSpawn = false;
        }

        if (enemiesSpawn2)
        {
            guard2.SetActive(true);
            archer2.SetActive(true);
            guard3.SetActive(true);
            archer3.SetActive(true);

            enemiesSpawn2 = false;
            FindObjectOfType<AudioManager>().Stop();
            songPlaying = "MalumTheme3";
        }
    }

    private bool TimeFunction()
    {
        if (time > 6 && !onceSpawn)
        {
            enemiesSpawn = true;
            onceSpawn = true;
            
        }
        else
        {
            return false;
        }
        return false;
        
    }

    public void Damage(AttackDetails attackDetails)
    {
        if (isFalling)
        {
            currentHealth -= attackDetails.damageAmount;
        }

        if (currentHealth <= 0)
        {
            Died();
        }
    }

    private void ApplyMovement()
    {
        if (!isFalling) 
        {
            if (rb.position.x <= -12 && !onceRight && onceLeft)
            {
                movementSpeed = -movementSpeed;
                onceRight = !onceRight;
                onceLeft = !onceLeft;
            }
            else if (rb.position.x >= 17 && onceRight && !onceLeft)
            {
                movementSpeed = -movementSpeed;
                onceRight = !onceRight;
                onceLeft = !onceLeft;
            }
            //with this if statement I try to create a acceleration effect 
            if (movementSpeed > 0)
            {
                if (rb.position.x < 7)
                {
                    movementSpeed = movementSpeed * 1.0001f;
                }
                else
                {
                    movementSpeed = movementSpeed * 0.9999f;
                }
            }
            else
            {
                if (rb.position.x >= 7)
                {
                    movementSpeed = movementSpeed * 1.0001f;
                }
                else
                {
                    movementSpeed = movementSpeed * 0.9999f;
                }
            }
            rb.velocity = new Vector2(movementSpeed, Mathf.Cos(4 *time) * 3f);
        }
        else if(isFalling)
        {
            rb.velocity = new Vector2(0, -1.5f);
        }
        
    }

    private void Died()
    {
        aliveGO.SetActive(false);
        songPlaying = "MalumTheme4";
    }
}
