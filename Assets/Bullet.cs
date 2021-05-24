using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision}과 총알 충돌");
    }
}
