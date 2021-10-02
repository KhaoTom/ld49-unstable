using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera lookCamera;
    private CharacterController characterController;
    private MeshRenderer capsuleMeshRenderer;

    private bool canMove = true;
    private Transform followTransform;

    private float lookSensitivity = 2;
    private float moveSpeed = 4;
    private float gravity = -9;

    private Vector2 lookAbsolute;
    private Vector2 lookSmooth;
    private Vector3 targetCharacterDirection;
    private Vector3 targetDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        capsuleMeshRenderer = GetComponent<MeshRenderer>();
        lookCamera = GetComponentInChildren<Camera>();
        capsuleMeshRenderer.enabled = false;
    }

    private void Start()
    {
        targetDirection = lookCamera.transform.localRotation.eulerAngles;
        targetCharacterDirection = transform.localRotation.eulerAngles;

        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateCameraLook();
        UpdateCharacterMove();
        //UpdateCharacterGravity();
        UpdateCursorLock();
    }

    private void UpdateCameraLook()
    {
        var targetLookOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        var lookDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        if (lookDelta == Vector2.zero)
        {
            lookDelta = new Vector2(Input.GetAxis("LookHorizontal"), Input.GetAxis("LookVertical"));
        }
        lookDelta = Vector2.Scale(lookDelta, new Vector2(lookSensitivity * 3, lookSensitivity * 3));
        lookSmooth.x = Mathf.Lerp(lookSmooth.x, lookDelta.x, 1f / 3);
        lookSmooth.y = Mathf.Lerp(lookSmooth.y, lookDelta.y, 1f / 3);
        lookAbsolute += lookSmooth;
        lookCamera.transform.localRotation = Quaternion.AngleAxis(-lookAbsolute.y, targetLookOrientation * Vector3.right);
        lookAbsolute.y = Mathf.Clamp(lookAbsolute.y, -90, 90);
        lookCamera.transform.localRotation *= targetLookOrientation;
        transform.localRotation = Quaternion.AngleAxis(lookAbsolute.x, transform.up);
        transform.localRotation *= targetCharacterOrientation;
    }

    private void UpdateCharacterGravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(transform.up * gravity * Time.deltaTime);
        }
    }

    private void UpdateCharacterMove()
    {
        if (!canMove)
        {
            transform.position = followTransform.position - Vector3.up;
            return;
        }

        var moveInput = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime, Input.GetAxis("Vertical") * Time.deltaTime) * moveSpeed;
        var move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(move);
    }

    private void UpdateCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Cursor.lockState == CursorLockMode.None && Input.anyKeyDown)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
