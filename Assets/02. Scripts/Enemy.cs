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
    [Header("속성")]
    public float range = 1.7f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    [Header("Unity 설정 필드")]
    public string enemyTag = "Enemy";
    Animator enemyAnimator;
    public Transform partToRotate;
    public GameObject sparkEffectPrefab;
    public int Hp = 15;
    public float fadeDuration = 1.0f; // 투명해지는 데 걸리는 시간
    public Image imgHpBar;
    private int initHP;

    private bool isFading = false;
    private float destroyDelay = 3.0f;
    private bool isDead = false;
    public static Image HpBar2;

    private static float currHp;
    private static readonly float initHP2 = 100.0f;

    public static int TotalEnemyCount;
    public static int DeadEnemyCount;

    private Score score;

    // 정적 생성자를 이용해 static 변수를 초기화합니다.
    static Enemy()
    {
        currHp = initHP2;
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.01f);
        enemyAnimator = GetComponent<Animator>();
        initHP = Hp;

        if (HpBar2 == null)
        {
            HpBar2 = GameObject.FindGameObjectWithTag("HP")?.GetComponent<Image>();
        }

        DisplayHealth();

        score = GameObject.Find("Score").GetComponent<Score>();

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
                enemyAnimator.SetBool("Run", false); // 적이 죽었을 때 "Run" 애니메이션 중지
            }

            if (GetComponent<Tween_Path>().HasReachedEnd() && Hp <= 0)
            {
                DeadEnemyCount++;
                Debug.Log("DeadEnemyCount: " + DeadEnemyCount);
                if (DeadEnemyCount >= 9)
                {
                    SceneManager.LoadScene("WinScene");
                }
                StopTweenPath();
                StartCoroutine(DestroyEnemy());
                isFading = true;
                Score.Instance.Initialize();
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

            if (Hp <= 0 && !isFading)
            {
                DeadEnemyCount++;
                if (DeadEnemyCount >= 9)
                {
                    SceneManager.LoadScene("WinScene");
                }
                StartCoroutine(DestroyEnemy());
                isFading = true;
            }
        }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0 && !isFading)
        {
            DeadEnemyCount++;
            if (DeadEnemyCount >= 9)
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
            Debug.LogError("Unity Editor에서 imgHpBar가 할당되지 않았습니다.");
        }

        if (Hp <= 0 && !isFading)
        {
            DeadEnemyCount++;
            Debug.Log("DeadEnemyCount: " + DeadEnemyCount);
            if (DeadEnemyCount >= 9)
            {
                DeadEnemyCount = 0;
                SceneManager.LoadScene("WinScene");
                currHp = 100.0f;
            }

            StopTweenPath();
            StartCoroutine(DestroyEnemy());
            isFading = true;
        }
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
        Score.Instance.DipScore(50);
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

    private void OnTriggerEnter(Collider other)
    {
        if (currHp > 0.0f && other.CompareTag("Finish"))
        {
            Instantiate(sparkEffectPrefab, transform.position, Quaternion.Euler(0, -90, 0));
            currHp -= 10.0f;
            DisplayHealth();
            Destroy(gameObject);
            if (currHp <= 0.0f)
            {
                DeadEnemyCount = 0;
                SceneManager.LoadScene("LoseScene");

                currHp = 100.0f;
            }
        }
    }

    void DisplayHealth()
    {
        if (HpBar2 != null)
        {
            HpBar2.fillAmount = currHp / initHP2;
        }
        else
        {
            Debug.LogError("Unity Editor에서 HpBar2가 할당되지 않았습니다.");
        }
    }
}
