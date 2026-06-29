using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public Rigidbody playerRb;
    public FuelTank gasTank;
    public PathfindingNode playerClosestNode;
    private void Awake()
    {
        instance = this;
    }
}
