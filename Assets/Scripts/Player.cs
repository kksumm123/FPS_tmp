using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어 캐릭터 추가
    //움직이기

    //    기존 코드 재사용, 보는 방향으로 이동
    //카메라 회전
    //총쏘기

    //    트레일 렌더러, 물리로 던지기
    //수류탄 던지기
    public float speed = 5f;
    public float mouseSensitivity = 1f;

    public Animator animator;
    public Transform cameraTr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cameraTr = transform.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        UseWeapon();
        Move();
        CamaraRotate();
    }


    public GameObject bullet;
    public GameObject grenade;
    public Transform bulletSpawnPosition;
    private void UseWeapon()
    {
        //마우스클릭 총알발사
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, bulletSpawnPosition.position, transform.rotation);
        }
        // g키 누르면 수류탄 발사
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(grenade, bulletSpawnPosition.position, transform.rotation);
        }
    }


    private void CamaraRotate()
    {
        // 카메라 로에티션을 바꾸자 - 마우스 이동량에 따라
        float mouseMoveX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseMoveY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // cameraTr
        var worldUp = cameraTr.InverseTransformDirection(Vector3.up);
        var rotation = cameraTr.rotation *
                       Quaternion.AngleAxis(mouseMoveX, worldUp) *
                       Quaternion.AngleAxis(mouseMoveY, Vector3.left);
        transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        cameraTr.rotation = rotation;
    }

    private void Move()
    {
        // WASD, W위로, A왼쪽,S아래, D오른쪽
        float moveX = 0;
        float moveZ = 0;

        // || -> or
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveZ = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveZ = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1;

        // 이 트렌스폼의 앞쪽으로 움직여야 한다
        Vector3 move = transform.forward * moveZ
                    + transform.right * moveX;
        // 방향이니까 노멀라이즈해야한다
        move.Normalize();
        // 공간은 월드 절대좌표로
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("shoot") == false)
        {
            if (moveX != 0 || moveZ != 0)
                animator.Play("run");
            else
                animator.Play("idle");
        }
    }
}
