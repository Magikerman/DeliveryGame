using UnityEngine;

public class Context : MonoBehaviour
{
    public Transform player;
    public Transform self;
    public Rigidbody playerRb;

    [Header("Line of sight")]
    public float range;
    public float angle;
    public LayerMask mask;

    [Header("Distance checks")]
    public float keepDistance;
    public float keepClose;
}
