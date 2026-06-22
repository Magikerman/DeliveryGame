using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;

    private float speed;
    private float rotation;

    private UnityEngine.InputSystem.Gyroscope gyro;

    public float Speed => speed;

    private float timeSurvived = 0;

    [SerializeField] private TextMeshProUGUI realTimeScoreText;

    [Header("Player Values")]
    [SerializeField] private float speedMult;
    [SerializeField] private float rotationMult;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float screenResWidth;

    void Start()
    {
        Time.timeScale = 1f;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;

        screenResWidth = Screen.currentResolution.width;
    }

    void OnFingerDown(Finger controllFinger)
    {
        speed = playerInput.actions["Drive"].ReadValue<float>();

        speed = (speed - 2160 / 2);

        if (speed < 0)
        {
            speed = -1;
            rotation *= -1;
        }
        else if (speed > 0)
            speed = 1;

        void OnFingerMove(Finger finger)
        {
            if (finger.index != controllFinger.index)
                return;
            int delta = (int)((finger.screenPosition.x - controllFinger.screenPosition.x) / 100);
            speed = playerInput.actions["Drive"].ReadValue<float>();

            speed = (speed - screenResWidth / 2);

            if (speed < 0)
            {
                speed = -1;
                rotation *= -1;
            }
            else if (speed > 0)
                speed = 1;

            if (delta != 0)
            {
                UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
            }
        }

        void OnFingerUp(Finger finger)
        {
            if (finger.index != controllFinger.index)
                return;

            speed = 0;

            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
        }

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
    }

    void Update()
    {
        

        rotation = playerInput.actions["PhoneRot"].ReadValue<float>();

        //Borrar al buildear
        /*if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            speed = playerInput.actions["Drive"].ReadValue<float>();

            if (speed < 0)
                rotation *= -1;
        }*/

        Debug.Log(Input.gyro.attitude);

        timeSurvived += Time.deltaTime;
        realTimeScoreText.text = (float)((int)(timeSurvived * 10)) /10+ "s";
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * speedMult * Time.fixedDeltaTime, ForceMode.Impulse);

        float speedRotModifier = Mathf.Clamp(rb.linearVelocity.magnitude / 10, 0, 10);
        Vector3 rot = transform.right * rotation * rotationMult * speedRotModifier;

        transform.forward = Vector3.Lerp(transform.forward, rot, Time.fixedDeltaTime);
    }

    public void Die()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);

        scoreText.text = "Score: " + (float)((int)(timeSurvived * 10)) / 10;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
