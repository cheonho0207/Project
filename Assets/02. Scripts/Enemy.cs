using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TIME_CONTROL;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private Transform target;
    [Header("�Ӽ�")]
    public float range = 1.7f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    [Header("Unity ���� �ʵ�")]
    public string enemyTag = "Enemy";
    Animator enemyAnimator;
    public Transform partToRotate;

    public int Hp = 5;
    public float fadeDuration = 1.0f; // ���������� �� �ɸ��� �ð�
    public Image imgHpBar;
    private int initHP;
   
    private bool isFading = false;
    private float destroyDelay = 3.0f;
    private bool isDead = false;


    public static int TotalEnemyCount;
    public static int DeadEnemyCount;
    // ���� ���� ȣ���
    void Start()
    {
        
        InvokeRepeating("UpdateTarget", 0f, 0.01f);
        enemyAnimator = GetComponent<Animator>();
        initHP = Hp;
       

}

    void UpdateTarget()
    {
        if (GetComponent<Tween_Path>().HasCollided())
        {
            if (enemyAnimator == null)
            {
                enemyAnimator = GetComponent<Animator>();
            }

            if (!isDead)
            {
                enemyAnimator.SetBool("Run", true);
            }
            else
            {
                enemyAnimator.SetBool("Run", false); // ���� �׾��� �� "Run" �ִϸ��̼� ����
            }

            // ���� ��� ���� �����ϰ� HP�� 0�� �� ��� ó��
            if (GetComponent<Tween_Path>().HasReachedEnd() && Hp <= 0)
            {
                DeadEnemyCount++; // ���� DeadEnemyCount�� ����
                if (DeadEnemyCount >= 9) // 4�� �ƴ϶� 5�� ����
                {
                    SceneManager.LoadScene("WinScene");
                }
                StopTweenPath(); // ���� �̵��� ����ϴ�.
                StartCoroutine(DestroyEnemy());
                isFading = true;
            }
        }
        else
        {
            if (enemyAnimator != null && !isDead)
            {
                enemyAnimator.SetBool("Run", false);
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemyObject in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyObject.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyObject;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;

            // �߰�: ��ο� ���� ���� HP�� 0�̸� ��� ó��
            if (Hp <= 0 && !isFading)
            {
                DeadEnemyCount++; // ���� DeadEnemyCount�� ����
                if (DeadEnemyCount >= 9) // 4�� �ƴ϶� 5�� ����
                {
                    SceneManager.LoadScene("WinScene");
                }
                StartCoroutine(DestroyEnemy());
                isFading = true; // ���� ���� �ڷ�ƾ ȣ���� �����ϱ� ���� ���⼭ isFading�� true�� �����մϴ�.
            }
        }
    }

    // �� �����Ӹ��� ȣ���
    void Update()
    {
        if (isDead)
        {
            // ���� ���¿����� ���� �� �̵� ó��
            // ...
            return;
        }

        if (Hp <= 0 && !isFading)
        {
            DeadEnemyCount++; // ���� DeadEnemyCount�� ����
            if (DeadEnemyCount >= 6) // 4�� �ƴ϶� 5�� ����
            {
                SceneManager.LoadScene("WinScene");
            }
            StartCoroutine(DestroyEnemy());
           
            return;
        }

        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        lookRotation *= Quaternion.Euler(0f, 532f, 0f);
        partToRotate.rotation = lookRotation;

        if (Vector3.Distance(transform.position, target.position) <= range)
        {
            if (IsPlayerInAttackRange())
            {
                StopTweenPath();
                Attack();
                fireCountdown = 1f / fireRate;
            }
        }
        else
        {
            if (!IsPlayerInAttackRange())
            {
                ResumeTweenPath();
            }
        }
    }

    void Attack()
    {
        int randomAttack = Random.Range(1, 3);

        switch (randomAttack)
        {
            case 1:
                enemyAnimator.SetTrigger("Attack");
                break;
            case 2:
                enemyAnimator.SetTrigger("Attack2");
                break;
        }
    }


    public void winScene()
    {
        SceneManager.LoadScene("WinScene");
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (collision.gameObject.GetComponent<Arrow>().IsAlreadyProcessed())
            {
                return;
            }

            int damage = 1;
            TakeDamage(damage);

            collision.gameObject.GetComponent<Arrow>().MarkAsProcessed();
        }
    }

    public void TakeDamage(int damage)
    {
        Hp = Mathf.Max(0, Hp - damage);

        if (imgHpBar != null)
        {
            imgHpBar.fillAmount = (float)Hp / (float)initHP;
        }
        else
        {
            Debug.LogError("Unity Editor���� imgHpBar�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (Hp <= 0 && !isFading)
        {
            DeadEnemyCount++; // ���� DeadEnemyCount�� ����
            if (DeadEnemyCount >= 9) // 4�� �ƴ϶� 5�� ����
            {
                SceneManager.LoadScene("WinScene");
            }

            StopTweenPath();
            StartCoroutine(DestroyEnemy());
            isFading = true; // ���� ���� �ڷ�ƾ ȣ���� �����ϱ� ���� ���⼭ isFading�� true�� �����մϴ�.
        }
    }


  
    IEnumerator DestroyEnemy()
    {
        DeadEnemyCount++; // ���� DeadEnemyCount�� ����
        if (DeadEnemyCount >= 9) // 4�� �ƴ϶� 5�� ����
        {
            SceneManager.LoadScene("WinScene");
        }

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void StopTweenPath()
    {
        Tween_Path tweenPath = GetComponent<Tween_Path>();
        if (tweenPath != null)
        {
            tweenPath.SetPause(true);
            enemyAnimator.SetTrigger("Death1");
        }
    }

    void ResumeTweenPath()
    {
        Tween_Path tweenPath = GetComponent<Tween_Path>();
        if (tweenPath != null)
        {
            tweenPath.EnemyExitedPath();
        }
    }

    bool IsPlayerInAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Tween_Path tweenPath = collider.GetComponent<Tween_Path>();
                if (tweenPath != null)
                {
                    return !tweenPath.HasCollided();
                }
            }
        }
        return false;
    }
}