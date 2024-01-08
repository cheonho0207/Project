using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float moveSpeed = 1;// 플레이어의 이동 속도
    private Transform tr;//  플레이어의 Transform 컴포넌트
    private Rigidbody rd;// 플레이어의 Rigidbody 컴포넌트
    public float gravityScale = 2.0f; // 중력 스케일
    private bool isGrounded = false;// 땅에 닿았는지 여부를 나타내는 변수

    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    private float health;

    public int worth = 50;


    private bool isDead = false;

    void Start()
    {
        tr = GetComponent<Transform>(); //Transform 컴포넌트 가져와 tr 변수에 할당
        rd = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 가져와 rd 변수에 할당
        rd.useGravity = true;// Rigidbody에 중력 사용 활성화
        rd.isKinematic = false; // "isKinematic" 비활성화
        rd.interpolation = RigidbodyInterpolation.Interpolate; // Rigidbody 보간 설정
        
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0 && !isDead)
        {
            Debug.Log("적 사망");
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Update()
    {
        // 입력 처리 메서드 호출
    }

    

    private void Move(Vector3 moveDirection) // 이동 메서드
    {
        Vector3 moveVector = moveDirection * moveSpeed * Time.deltaTime;
        //이동 방향 벡터에 이동 속도와 시간 간역을 곱하여 이동 벡터 계산
        rd.velocity = new Vector3(moveVector.x, rd.velocity.y, moveVector.z);
        // Rigidbody 속도를 이동 벡터로 설정하여 플레이어 이동 처리
    }


    void FixedUpdate() //가속도와 중력 적용 메서드
    {
        Vector3 gravityForce = Physics.gravity * gravityScale;
        // 중력 힘 벡터 계산 (중력 스케일 적용)
        rd.AddForce(gravityForce, ForceMode.Acceleration);
        // Rigidbody에 중력 힘을 적용 (가속도 모드로)
    }
}
