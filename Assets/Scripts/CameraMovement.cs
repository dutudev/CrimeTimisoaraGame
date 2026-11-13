using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject pivot, cameraPos, cameraObject;
    [SerializeField] 
    private float moveSpeed, lerpSpeed;
    [SerializeField] 
    private Vector2 lookSpeed;

    private Controls _controls;
    private Vector2 _moveAxis;
    private Vector2 _lookAxis;
    private bool _looking = false;
    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
        _controls.Player.Move.performed += GetMove;
        _controls.Player.Move.canceled += GetMove;
        _controls.Player.Look.performed += GetLook;
        _controls.Player.Look.canceled += GetLook;
        _controls.Player.Pan.performed += GetLooking;
        _controls.Player.Pan.canceled += GetLooking;
        //add controls here
    }

    private void OnDisable()
    {
        _controls.Player.Move.performed -= GetMove;
        _controls.Player.Move.canceled -= GetMove;
        _controls.Player.Look.performed -= GetLook;
        _controls.Player.Look.canceled -= GetLook;
        _controls.Player.Pan.performed -= GetLooking;
        _controls.Player.Pan.canceled -= GetLooking;
        _controls.Disable();
    }

    private void GetMove(InputAction.CallbackContext ctx)
    {
        _moveAxis = ctx.ReadValue<Vector2>().normalized;
    }

    private void GetLook(InputAction.CallbackContext ctx)
    {
        _lookAxis = ctx.ReadValue<Vector2>();
    }

    private void GetLooking(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _looking = true;
        }else if (ctx.canceled)
        {
            _looking = false;
        }
    }

    private void Move()
    {
        Vector2 camDirForward = new Vector2(pivot.transform.forward.x, pivot.transform.forward.z);
        Vector2 camDirRight = new Vector2(pivot.transform.right.x, pivot.transform.right.z);
        camDirForward.Normalize();
        camDirRight.Normalize();
        Vector2 direction = _moveAxis.y * camDirForward + _moveAxis.x * camDirRight;
        direction.Normalize();
        Vector3 finalVector = new Vector3(direction.x, 0f, direction.y) ;
        pivot.transform.position += moveSpeed * Time.deltaTime * finalVector;
        //add lerp for camera pos
        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, cameraPos.transform.position,
            Time.deltaTime * lerpSpeed);
    }

    private void Look()
    {
        cameraObject.transform.localRotation =
            Quaternion.LookRotation((pivot.transform.position - cameraObject.transform.position).normalized);
       
        if (_looking)
        {
            
            Vector3 newRotation =
                new Vector3(Mathf.DeltaAngle(0, pivot.transform.rotation.eulerAngles.x) + -1 * _lookAxis.y * Time.deltaTime * lookSpeed.y,
                    pivot.transform.rotation.eulerAngles.y + _lookAxis.x * Time.deltaTime * lookSpeed.x, 0f);
            pivot.transform.localRotation = Quaternion.Euler(Mathf.Clamp(newRotation.x, -15f, 9f), newRotation.y, 0f);
        }
        
    }
    
    void LateUpdate()
    {
        Move();
        Look();
    }
}
