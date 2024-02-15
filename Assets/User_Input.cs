using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class User_Input : MonoBehaviour
{

    public static User_Input instance;

    public bool MainMoveUp { get; private set;}
    public bool MainMoveDown { get; private set;}
    public bool CloneMoveUp { get; private set;}
    public bool CloneMoveDown { get; private set;}


    private PlayerInput _playerInput;
    private InputAction _mainMoveUp;
    private InputAction _mainMoveDown;
    private InputAction _cloneMoveDown;
    private InputAction _cloneMoveUp;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();

        SetUpPlayerInputs();
    }

    private void SetUpPlayerInputs()
    {
        _mainMoveUp = _playerInput.actions["moveup-main"];
        _mainMoveDown = _playerInput.actions["movedown-main"];
        _cloneMoveDown = _playerInput.actions["movedown-clone"];
        _cloneMoveUp = _playerInput.actions["moveup-clone"];
    }

    void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        MainMoveUp = _mainMoveUp.WasPressedThisFrame();
        MainMoveDown = _mainMoveDown.WasPressedThisFrame();
        CloneMoveDown = _cloneMoveDown.WasPressedThisFrame();
        CloneMoveUp = _cloneMoveUp.WasPressedThisFrame();
    }
}
