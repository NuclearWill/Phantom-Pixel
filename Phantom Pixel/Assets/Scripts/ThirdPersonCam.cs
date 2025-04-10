using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] 
    Transform cameraObject;
    [SerializeField] 
    float masterMouseSensitivity = 3f;
    [SerializeField]
    float horizontalMouseSensitivity = 1f;
    [SerializeField]
    float verticalMouseSensitivity = 1f;
    [SerializeField]
    float orbitRadius = 20f;
    [SerializeField]
    [Tooltip("The total height in units the camera can ascend")]
    float maxHeight = 10f;
    [SerializeField]
    [Tooltip("The total height in units the camera can descend")]
    float minHeight = 0f;
    [SerializeField]
    [Tooltip("The starting height of the camera in the level")]
    float startingHeight = 5f;

    [Header("Input Action Reference")]
    public InputSystem_Actions playerControls;


    // internal variables
    private InputAction lookInput;
    private float verticalInput;
    private float horizontalInput;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        // starts the camera at the starting height
        ChangeCamHeight(startingHeight);
        verticalInput = startingHeight;
    }
    private void OnEnable()
    {
        lookInput = playerControls.Player.Look;
        lookInput.Enable();
    }

    private void OnDisable()
    {
        lookInput.Disable();
    }

    private void Start()
    {
        // makes the cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInput();
        
        MoveCamera();
    }

    private void GetInput()
    {
        Vector2 initialInput = lookInput.ReadValue<Vector2>() * masterMouseSensitivity * Time.deltaTime;
        horizontalInput = initialInput.x * horizontalMouseSensitivity;
        verticalInput = initialInput.y * verticalMouseSensitivity;
    }

    private void MoveCamera()
    {
        ChangeCamHeight(verticalInput);

        ChangeCamRot(horizontalInput);
    }

    private void ChangeCamHeight(float inputHeight)
    {
        float finalHeight = Mathf.Clamp(transform.position.y + inputHeight, minHeight, maxHeight);
        transform.position = new Vector3(transform.position.x, finalHeight, transform.position.z);
    }

    private void ChangeCamRot(float inputRotation)
    {
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + inputRotation, transform.rotation.z, transform.rotation.w);
        transform.Rotate(0, inputRotation, 0);
    }
}
