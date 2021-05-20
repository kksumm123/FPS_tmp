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
        while (true)
        {
            agent.destination = target.position;
            //yield return new WaitForSeconds(1); //1초 쉬는거
            yield return null; // 1프레임 쉬는거
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
