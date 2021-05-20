using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    // 타겟을 매 프레임 쫒아가자.
    IEnumerator Start()
    {
        moveSpeed = agent.speed;
        while (true)
        {
            agent.destination = target.position;
            //yield return new WaitForSeconds(1); //1초 쉬는거
            yield return null; // 1프레임 쉬는거
        }
    }

    public GameObject attackedEffect;
    public GameObject destroyEffect;
    public int hp = 3;
    private float moveSpeed;

    public void OnHit()
    {
        Debug.Log("OnHit : " + name, transform);
        hp--;

        if (hp>0)
        {
            // 맞을 때 이펙트
            Instantiate(attackedEffect, transform.position, transform.rotation);

            //경직
            StartCoroutine(ChangeSpeed(0.3f));
        }
        else
        {
            // 3대 맞으면 폭발
            Instantiate(destroyEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
        // 총알에 맞으면 0.3초 스탑
        // 총알 맞을 때 이펙트보여주기
        // HP 추가 3대 맞으면 폭발
    }

    private IEnumerator ChangeSpeed(float stopTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(stopTime);
        agent.speed = moveSpeed;
    }
}
