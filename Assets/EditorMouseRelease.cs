﻿using FPSControllerLPFP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMouseRelease : MonoBehaviour
{
    FpsControllerLPFP controller;
    void Start()
    {
        controller = GetComponent<FpsControllerLPFP>();
    }


    bool mouseLock = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMouseRelease();
        }
    }

    private void ToggleMouseRelease()
    {
        mouseLock = !mouseLock;
        SetMouseReleaseMode();
    }

    private void SetMouseReleaseMode()
    {
        controller.enabled = mouseLock;
        if (mouseLock)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
