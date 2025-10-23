using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboInput : MonoBehaviour
{
    public InputActionAsset inputActions;

    public InputAction lightAttack;
    public InputAction heavyAttack;

    private void OnEnable() => inputActions?.Enable();
   // private void OnDisable() => inputActions?.Disable();

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        lightAttack = inputActions["LightAttack"];
        heavyAttack = inputActions["HeavyAttack"];
    }
}
