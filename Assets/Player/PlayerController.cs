using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;

    private float speed;
    //private float rotation;
    private Vector3 rotation;

    public float Speed => speed;

    private float timeSurvived = 0;

    [SerializeField] private TextMeshProUGUI realTimeScoreText;

    [Header("Player Values")]
    [SerializeField] private float speedMult;
    [SerializeField] private float rotationMult;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Joystick joystick;

    [SerializeField] private Vector3 axisFix;
    [SerializeField] private Vector3 axisFixSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //speed = playerInput.actions["Drive"].ReadValue<Vector2>().y;
        //rotation = playerInput.actions["Drive"].ReadValue<Vector2>().x;

        //if (speed < 0) rotation *= -1;
    }

    private void FixedUpdate()
    {
        /*rb.AddForce(transform.forward * speed * speedMult * Time.fixedDeltaTime, ForceMode.Impulse);

        float speedRotModifier = Mathf.Clamp(rb.linearVelocity.magnitude / 10, 0, 10);
        Vector3 rot = transform.right * rotation * rotationMult * speedRotModifier;

        transform.forward = Vector3.Lerp(transform.forward, rot, Time.fixedDeltaTime);*/

        rotation = axisFix * joystick.Horizontal + axisFixSecond * joystick.Vertical;
        //rotation = Vector3.forward * joystick.Horizontal + -Vector3.right * joystick.Vertical + axisFix;

        rotation.y = transform.forward.y;

        Vector3 dir = transform.forward;
        dir.y = 0;

        if (rotation.x != 0 && rotation.z != 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, rotation, rotationMult * Time.fixedDeltaTime);
            rb.AddForce(dir * speedMult * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        timeSurvived += Time.fixedDeltaTime;
        realTimeScoreText.text = (float)((int)(timeSurvived * 10)) / 10 + "s";
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
