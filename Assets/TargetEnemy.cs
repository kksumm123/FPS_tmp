using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    public NavMeshAgent agent;
    private float moveSpeed;
    public List<Transform> wayPoints;
    public List<Vector3> wayPointsVectors;
    // 타겟을 매 프레임 쫒아가자.
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        ///상태 1) 페트롤: 
        ///지정된 웨이 포인트 이동
        ///웨이 포인트를 무한 반복하게 만들자
        ///페트롤이 끝나는 조건:
        ///1.시야 범위 안에 적이 들어 옴->추적으로 전환.
        ///2.소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환

        // 첫번째 웨이 포인트로 가자
        animator.Play("run");
        while (true)
        {
            agent.destination = wayPoints[0].position;
            while (true)
            {
                //도착 했는지 (Approximately쓰면 안되나?)
                if (agent.remainingDistance == 0)
                {
                    Debug.Log("도착");
                    // 2번째 웨이포인트 이동.
                }
                else
                {
                    // 기다리자
                }
                // 유니티는 싱글스레드 이기때문에 이대로 돌리면 무한루프
                // 그렇기 때문에
                yield return null; // 1프레임 쉬자
            }
        }
        // 패트롤 상태
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

    public void OnHit()
    {
        Debug.Log("OnHit : " + name, transform);
        hp--;

        if (hp > 0)
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
