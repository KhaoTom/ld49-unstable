using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera lookCamera;
    public CharacterController characterController;
    public MeshRenderer capsuleMeshRenderer;
    public GameObject shotPrefab;

    public bool canMove = true;
    public Transform followTransform;

    public float lookSensitivity = 2;
    public float moveSpeed = 4;
    public float gravity = -9;

    public float shotCooldown = 1;

    public float movementLimit = 3;
    public Vector3 initialPosition = Vector3.zero;

    private float shotCooldownRemaining;

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
        transform.position = initialPosition;

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

        UpdateShooting();
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

        var moveInput = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime, 0) * moveSpeed;
        var move = Vector3.right * moveInput.x;
        characterController.Move(move);

        var x = Mathf.Clamp(transform.position.x, initialPosition.x - movementLimit, initialPosition.x + movementLimit);
        transform.position = new Vector3(
                x,
                transform.position.y,
                transform.position.z
                );
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

    private void UpdateShooting()
    {
        if (shotCooldownRemaining <= 0 && Input.GetButtonDown("Fire1"))
        {
            shotCooldownRemaining = shotCooldown;
            RaycastHit hit;
            var hitSomething = Physics.Raycast(lookCamera.transform.position, lookCamera.transform.forward, out hit);
            if (hitSomething)
            {
                hit.transform.GetComponent<UnstableTone>()?.ResetStability();
                Instantiate(shotPrefab, hit.transform.position, Quaternion.identity);
            }
        }
        else
        {
            shotCooldownRemaining -= Time.deltaTime;
        }
    }
}
