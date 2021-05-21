using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    private float moveSpeed;
    public List<Transform> wayPoints;
    public List<Vector3> wayPointsVectors;

    public Animator animator;
    public int wayPointIdx = 0;
    // 타겟을 매 프레임 쫒아가자.
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        yield return StartCoroutine(PetrolCo());
    }


    public float viewingDistance = 3f;
    public float viewingAngle = 90f;
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, viewingDistance);
        // 시야각 표시
        // 호 표시.
        //Transform tr = GetComponent<Transform>();
        Transform tr = transform;
        float halfAngle = viewingAngle * 0.5f;
        Handles.color = Color.red;
        Handles.DrawWireArc(tr.position, tr.up
            , tr.forward.AngleToYDirection(-halfAngle), viewingAngle, viewingDistance);

        // 좌, 우 선 표시
        Handles.DrawLine(tr.position
            , tr.position + tr.forward.AngleToYDirection(-halfAngle) * viewingDistance);// 왼쪽선 그리기.

        Handles.DrawLine(tr.position
            , tr.position + tr.forward.AngleToYDirection(halfAngle) * viewingDistance); // 오른쪽선 그리기.

    }
    IEnumerator PetrolCo()
    {
        ///상태 1) 페트롤: 
        ///지정된 웨이 포인트 이동
        ///웨이 포인트를 무한 반복하게 만들자
        ///페트롤이 끝나는 조건:
        ///1.시야 범위 안에 적이 들어 옴->추적으로 전환.
        ///2.소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환
        ///
        // 첫번째 웨이 포인트로 가자
        animator.Play("run");
        while (true)
        {
            if (wayPointIdx >= wayPoints.Count)
                wayPointIdx = 0;

            Debug.Log("wayPointIdx : " + wayPointIdx);
            agent.destination = wayPoints[wayPointIdx].position;
            yield return null; // 1프레임 쉬자

            while (true)
            {
                //도착 했는지 (Approximately쓰면 안되나?)
                // 별로 좋지 않다.
                // remainingDistance가 시작되자마자 0이다 
                // 왜? 1프레임이 지나야 갱신된다
                if (agent.remainingDistance == 0)
                {
                    Debug.Log("도착");
                    // 다음 웨이포인트 이동.
                    break;
                }
                //플레이어 탐지
                //플레이어와 나와의 위치를 구하자
                float distance = Vector3.Distance(transform.position, player.position);
                // 시야거리 이내라면
                if (distance < viewingDistance)
                {
                    // 시야각에 들어왔는지 판단할 bool 변수
                    bool insideViewingAngle = false;

                    // 시야각에 들어왔는지 확인하는 로직
                    Vector3 targetDir = player.position - transform.position;
                    targetDir.Normalize();
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    if (Mathf.Abs(angle) <= viewingAngle * 0.5f)
                    {
                        insideViewingAngle = true;
                    }

                    if (insideViewingAngle)
                    {
                        Debug.LogWarning("찾았다 -> 추적 상태로 전환해야함");
                    }
                }


                // 유니티는 싱글스레드 이기때문에 이대로 돌리면 무한루프
                // 그렇기 때문에
                yield return null; // 1프레임 쉬자
            }
            wayPointIdx++;
        }
        // 패트롤 상태
        //while (true)
        //{
        //    agent.destination = player.position;
        //    //yield return new WaitForSeconds(1); //1초 쉬는거
        //    yield return null; // 1프레임 쉬는거
        //}
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
