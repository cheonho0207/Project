
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float v = 0.0f;
    public float h = 0.0f;
    public float moveSpeed = 1.0f;
    private Transform tr;
    private Animator animator;

    public int power;
    public GameObject bulletPrefab;
    private Transform SpPoint;
    public GameObject WoodenBow;

    public float DestroyTime = 3.0f;
    public float upwardForce = 10.0f;

    private bool arrowSpawned = false;

    public int power1 = 10;
    public int power2 = 10;
    public int power3 = 10;
    private Material sparkEffectMaterial;
    private float fadeSpeed = 2f;
    private Animator playerAnimator;
    private bool Shooting = false;
    void Start()
    {
        tr = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        SpPoint = GameObject.Find("SpPoint").transform;
        power = 10;
    }

    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        tr.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime);
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        if (Input.GetMouseButtonDown(0) && !arrowSpawned)
        {
            Shooting = true;
            playerAnimator.SetTrigger("Shooting");
            StartCoroutine(ShootBulletAfterAnimation());
            arrowSpawned = true;
        }


    }

    IEnumerator ShootBulletAfterAnimation()
    {
        yield return new WaitForSeconds(3.2f);

        // 화살 1 발사 (거리 10)
        GameObject bullet1 = Instantiate(bulletPrefab, SpPoint.position, SpPoint.rotation) as GameObject;
        bullet1.transform.localRotation = Quaternion.Euler(80, 0, 0);
        Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
        Vector3 forwardForce1 = bullet1.transform.forward * power1;
        bulletRigidbody1.AddForce(forwardForce1);
        Vector3 upwardForceVector1 = Vector3.up * upwardForce;
        bulletRigidbody1.AddForce(upwardForceVector1);

        // 화살 2 발사 (거리 15, 조금 가깝게)
        GameObject bullet2 = Instantiate(bulletPrefab, SpPoint.position, SpPoint.rotation) as GameObject;
        bullet2.transform.localRotation = Quaternion.Euler(80, 0, 0);
        Rigidbody bulletRigidbody2 = bullet2.GetComponent<Rigidbody>();
        Vector3 forwardForce2 = bullet2.transform.forward * power2;
        bulletRigidbody2.AddForce(forwardForce2);
        Vector3 upwardForceVector2 = Vector3.up * upwardForce;
        bulletRigidbody2.AddForce(upwardForceVector2);

        // 화살 3 발사 (거리 20, 더 가깝게)
        GameObject bullet3 = Instantiate(bulletPrefab, SpPoint.position, SpPoint.rotation) as GameObject;
        bullet3.transform.localRotation = Quaternion.Euler(80, 0, 0);
        Rigidbody bulletRigidbody3 = bullet3.GetComponent<Rigidbody>();
        Vector3 forwardForce3 = bullet3.transform.forward * power3;
        bulletRigidbody3.AddForce(forwardForce3);
        Vector3 upwardForceVector3 = Vector3.up * upwardForce;
        bulletRigidbody3.AddForce(upwardForceVector3);

        // 각 화살의 발사 위치를 조절
        bullet1.transform.Translate(Vector3.right * 1); // 오른쪽으로 조절
        bullet2.transform.Translate(Vector3.right * -1); // 왼쪽으로 조절
        bullet3.transform.Translate(Vector3.right * 0); // 중앙으로 조절

        yield return new WaitForSeconds(3.0f);

        Destroy(bullet1);
        Destroy(bullet2);
        Destroy(bullet3);
    }
}