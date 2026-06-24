using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;

    public Rigidbody Rb => rb;

    private Vector3 rotation;

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

        GlobarObjectiveMarker.marker.SetPlayer(transform);
    }

    private void FixedUpdate()
    {
        rotation = axisFix * joystick.Horizontal + axisFixSecond * joystick.Vertical;

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
