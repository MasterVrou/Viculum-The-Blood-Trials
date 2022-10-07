using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float 
        inputTimer,
        attack1Radius,
        attack1Damage;

    private AttackDetails attackDetails;

    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    [SerializeField]
    private float stunDamageAmount = 1f;

    [SerializeField]
    private GameObject shield;

    private bool gotInput;
    private bool isAttacking;
    private bool isFirstAttack;
    private bool shielded;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    private PlayerController PC;
    private PlayerStats PS;
    private HealthUI HU;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        HU = GetComponent<HealthUI>();

        shield.SetActive(false);
        shielded = false;
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
        CheckShield();
    }

    private void CheckShield()
    {
        if (Input.GetMouseButtonDown(1) && !shielded)
        {
            shield.SetActive(true);
            shielded = true;

            //call NoShield 3 seconds later
            Invoke("NoShield", 0.2f);
        }
        //else
        //{
        //    shield.SetActive(false);
        //    shielded = false;
        //}
    }

    void NoShield()
    {
        shield.SetActive(false);
        shielded = false;
    }

    private void CheckCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                //Atempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }    
    }


    private void CheckAttacks()
    {
        if (gotInput)
        {
            //perform attack
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            //wait for input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            attack1Damage = 10f;
            collider.transform.parent.SendMessage("Damage", attackDetails);
            
            //hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        
        int direction;

        //PS.DecreasedHealth(attackDetails.damageAmount);

        if (attackDetails.position.x < transform.position.x)
        {
           direction = 1;
        }
        else
        {
            direction = -1;
        }

        if (shielded)
        {
            if (PC.GetFacingDirection() == 1 && attackDetails.position.x < transform.position.x)
            {
                HU.LoseHP(attackDetails.damageAmount);
                PS.DecreasedHealth(attackDetails.damageAmount);
                PC.KnockBack(direction);
            }
        }
        else
        {
            HU.LoseHP(attackDetails.damageAmount);
            PS.DecreasedHealth(attackDetails.damageAmount);
            PC.KnockBack(direction);
            
        }
        
        
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
