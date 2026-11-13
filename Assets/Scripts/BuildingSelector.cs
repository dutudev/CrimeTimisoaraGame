using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelector : MonoBehaviour
{

    private Controls _controls;
    private Camera _camera;
    private Vector2 _mousePos;
    private Vector3 _Debugpoint;

    private void Awake()
    {
        _camera = Camera.main;
    }
    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
        _controls.Player.CLick.performed += GetClick;
        _controls.Player.MousePosition.performed += GetMousePos;
        _controls.Player.MousePosition.canceled += GetMousePos;
    }

    private void OnDisable()
    {
        _controls.Player.CLick.performed -= GetClick;
        _controls.Player.MousePosition.performed -= GetMousePos;
        _controls.Player.MousePosition.canceled -= GetMousePos;
        _controls.Disable();
    }
    
    private void GetClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
        }
    }

    private void GetMousePos(InputAction.CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }

    private void SendRay()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       SendRay();
    }
    
}
