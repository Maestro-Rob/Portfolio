using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 50f;
    public Transform cameraTransform;

    [Header("Player Controls")]
    public string horizontalInput = "Horizontal";  // Anpassbar für Spieler 2
    public string verticalInput = "Vertical";
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public string mouseXInput = "Mouse X";  // Maussteuerung für Splitscreen
    public string mouseYInput = "Mouse Y";

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public Slider staminaBar;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;
    private float stamina;
    private bool isSprinting;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        stamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = stamina;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        UpdateStamina();
        UpdateStaminaBar();
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal"); // Angepasste Eingabe für Spieler 1
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0;
        float speed = isSprinting ? runSpeed : walkSpeed;

        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) // Angepasste Eingabe
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void UpdateStamina()
    {
        if (isSprinting)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            if (stamina < 0) stamina = 0;
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = stamina;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Angepasste Eingabe
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
