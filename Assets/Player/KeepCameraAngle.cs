using UnityEngine;

public class KeepCameraAngle : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 position;

    private void FixedUpdate()
    {
        transform.position = player.position + position;
        transform.LookAt(player);
    }
}
