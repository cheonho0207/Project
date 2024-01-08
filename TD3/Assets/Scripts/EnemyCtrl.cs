using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float moveSpeed = 1;// �÷��̾��� �̵� �ӵ�
    private Transform tr;//  �÷��̾��� Transform ������Ʈ
    private Rigidbody rd;// �÷��̾��� Rigidbody ������Ʈ
    public float gravityScale = 2.0f; // �߷� ������
    private bool isGrounded = false;// ���� ��Ҵ��� ���θ� ��Ÿ���� ����

    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    private float health;

    public int worth = 50;


    private bool isDead = false;

    void Start()
    {
        tr = GetComponent<Transform>(); //Transform ������Ʈ ������ tr ������ �Ҵ�
        rd = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ�� ������ rd ������ �Ҵ�
        rd.useGravity = true;// Rigidbody�� �߷� ��� Ȱ��ȭ
        rd.isKinematic = false; // "isKinematic" ��Ȱ��ȭ
        rd.interpolation = RigidbodyInterpolation.Interpolate; // Rigidbody ���� ����
        
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0 && !isDead)
        {
            Debug.Log("�� ���");
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Update()
    {
        // �Է� ó�� �޼��� ȣ��
    }

    

    private void Move(Vector3 moveDirection) // �̵� �޼���
    {
        Vector3 moveVector = moveDirection * moveSpeed * Time.deltaTime;
        //�̵� ���� ���Ϳ� �̵� �ӵ��� �ð� ������ ���Ͽ� �̵� ���� ���
        rd.velocity = new Vector3(moveVector.x, rd.velocity.y, moveVector.z);
        // Rigidbody �ӵ��� �̵� ���ͷ� �����Ͽ� �÷��̾� �̵� ó��
    }


    void FixedUpdate() //���ӵ��� �߷� ���� �޼���
    {
        Vector3 gravityForce = Physics.gravity * gravityScale;
        // �߷� �� ���� ��� (�߷� ������ ����)
        rd.AddForce(gravityForce, ForceMode.Acceleration);
        // Rigidbody�� �߷� ���� ���� (���ӵ� ����)
    }
}
