using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour {
    Animator animator;
    bool isDead, canAttack;
    int hp, attackDamage;
    float attackCooldown;
    Character myTarget;
    float rotateSpeed = 2;
    float attackRange = 1.5f;
    public bool isVisible { get; private set; }
   

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(myTarget == null && isVisible)
        {
            Character[] characters = FindObjectsOfType<Character>();
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != this && characters[i].isVisible)
                {
                    myTarget = characters[i];
                    return;
                }
            }
        }
        else if (TargetIsInRange() && !isDead)
        {
            RotateToTarget();
            Attack();
        }
    }

    private void RotateToTarget()
    {
        Vector3 targetDir = myTarget.transform.position - transform.position;
        float step =  Time.deltaTime*rotateSpeed;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void LostTarget()
    {
        isVisible = false;
        myTarget = null;
    }

    public void ResetCharacter()
    {
        animator.Play("Wait");
        isDead = false;
        hp = 100;
        attackDamage = UnityEngine.Random.Range(10, 30);
        attackCooldown  = UnityEngine.Random.Range(0.5f, 3f);
        canAttack = true;
        animator.SetBool("isDead", isDead);
        isVisible = true;
    }

    void Attack()
    {
        if(canAttack)
        {
            canAttack = false;
            myTarget.GetDamage(attackDamage);
            animator.SetTrigger("attack");
            Invoke("UnlockAttack", attackCooldown);
        }
    }

    private bool TargetIsInRange()
    {
        if(myTarget != null)
        {
            return Vector3.Distance(transform.position, myTarget.transform.position) <= attackRange;
        }
        return false;
    }

    public void GetDamage(int damage)
    {
        if (!isDead)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger("getDamage");
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("dead");
        animator.SetBool("isDead", isDead);
    }

    void UnlockAttack()
    {
        canAttack = true;
    }
}
