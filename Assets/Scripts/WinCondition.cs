using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Time.timeScale = 0f;
            victoryScreen.SetActive(true);
        }
    }
}
