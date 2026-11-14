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
    private float moveSpeed, lerpSpeed, rotateSpeed;
    [SerializeField] 
    private Vector2 lookSpeed;

    public static CameraMovement instance { get; private set; }

    private Controls _controls;
    private Vector2 _moveAxis;
    private Vector2 _lookAxis;
    private bool _looking = false;
    private bool _canMove = true;
    private bool _rotate = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

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
        _controls.Player.Exit.performed += GetExit;
        _controls.Player.Exit.canceled += GetExit;
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
        _controls.Player.Exit.performed -= GetExit;
        _controls.Player.Exit.canceled -= GetExit;
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

    private void GetExit(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SetRotate(false);
            ChangeMove(true);
            GameManager.instance.ExitTool();
        }
    }

    private void Move()
    {
        if (_canMove)
        {
            Vector2 camDirForward = new Vector2(pivot.transform.forward.x, pivot.transform.forward.z);
            Vector2 camDirRight = new Vector2(pivot.transform.right.x, pivot.transform.right.z);
            camDirForward.Normalize();
            camDirRight.Normalize();
            Vector2 direction = _moveAxis.y * camDirForward + _moveAxis.x * camDirRight;
            direction.Normalize();
            Vector3 finalVector = new Vector3(direction.x, 0f, direction.y) ;
            pivot.transform.position += moveSpeed * Time.deltaTime * finalVector;
        }
        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, cameraPos.transform.position,
            Time.deltaTime * lerpSpeed);
    }

    private void Look()
    {
        cameraObject.transform.localRotation =
            Quaternion.LookRotation((pivot.transform.position - cameraObject.transform.position).normalized);
       
        if (_looking && _canMove)
        {
            
            Vector3 newRotation =
                new Vector3(Mathf.DeltaAngle(0, pivot.transform.rotation.eulerAngles.x) + -1 * _lookAxis.y * Time.deltaTime * lookSpeed.y,
                    pivot.transform.rotation.eulerAngles.y + _lookAxis.x * Time.deltaTime * lookSpeed.x, 0f);
            pivot.transform.localRotation = Quaternion.Euler(Mathf.Clamp(newRotation.x, -15f, 9f), newRotation.y, 0f);
        }

        if (!_canMove && _rotate)
        {
            pivot.transform.localRotation = Quaternion.Euler(6f, pivot.transform.rotation.eulerAngles.y + rotateSpeed * Time.deltaTime , 0f);
        }
        
    }

    public void ChangeMove(bool set)
    {
        _canMove = set;
    }
    public void SnapPosition(Vector2 pos)
    {
        pivot.transform.position = new Vector3(pos.x, pivot.transform.position.y, pos.y);
        
    }

    public void SetRotate(bool set)
    {
        _rotate = set;
    }

    public Vector3 GetCameraPos()
    {
        return cameraObject.transform.position;
    }
    
    void LateUpdate()
    {
        Move();
        Look();
    }
}
