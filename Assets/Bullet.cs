using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    public Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * speed;
    }
    //void Update()
    //{
    //    transform.Translate(0, 0, speed * Time.deltaTime);
    //}
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.transform.name}과 총알 충돌");
        Destroy(gameObject);

        // 적일 경우 데미지 감소,
        var targetEnemyRoot = collision.transform.root;
        if (targetEnemyRoot == null)
            return;
        var targetEnemy = targetEnemyRoot.GetComponent<TargetEnemy>();
        if (targetEnemy)
            targetEnemy.OnHit();

    }
}
