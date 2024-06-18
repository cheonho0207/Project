using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public float startSpeed = 3f;

    [HideInInspector]
    public float speed;

    public float startHealth = 10;
    private float health;

    public int worth = 50;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    //public Image healthBar;
    Animator enemyAnimator;
    public bool isDead = false;

    private Credit credit;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
        credit = FindObjectOfType<Credit>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            // 죽은 상태에서의 투명성 및 이동 처리
            // ...
            return;
        }
        UpdateTarget();
    }

    void UpdateTarget()
    {
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<Animator>();
        }

        if (!isDead)
        {
            transform.rotation = Quaternion.Euler(0, 260, 0); // Set rotation to Y 260 degrees
            enemyAnimator.SetBool("Run", true);
        }
        else
        {
            enemyAnimator.SetBool("Run", false); // 적이 죽었을 때 "Run" 애니메이션 중지
        }
    }

    public float GetHealth()
    {
        return health;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (collision.gameObject.GetComponent<Arrow>().IsAlreadyProcessed())
            {
                return;
            }

            int damage = 5;
            TakeDamage(damage);

            collision.gameObject.GetComponent<Arrow>().MarkAsProcessed();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        //healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            speed = 0;
            Die();
            credit.SumCredit();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        isDead = true;
        

        // Set "Death1" trigger animation
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Death1");
        }

        //PlayerStats.Money += worth;

        //GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;
        WaveSpawner.score++;
        Debug.Log("남은 적 : " + WaveSpawner.EnemiesAlive);
        Destroy(gameObject, 2f); // Add delay to allow death animation to play
    }

}