using TMPro;
using UnityEngine;

public class GlobarObjectiveMarker : MonoBehaviour
{
    public static GlobarObjectiveMarker marker;

    private Transform playerPosition;
    private Transform goalPosition;

    private Vector3 direction;
    private float rotation;
    private float distance;

    [SerializeField] private float addToRot;
    [SerializeField] private TextMeshProUGUI distanceText;

    private void Awake()
    {
        if (marker == null)
            marker = this;
        else
            Destroy(gameObject);
    }

    public void SetPlayer(Transform player)
    {
        playerPosition = player;
    }

    public void SetGoal(Transform goal)
    {
        goalPosition = goal;
    }

    private void FixedUpdate()
    {
        direction = goalPosition.position - playerPosition.position;
        direction.y = 0;
        distance = (float)(int)(direction.magnitude * 10) / 10;

        distanceText.text = distance + "km";

        rotation = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        rotation += addToRot;

        transform.rotation = Quaternion.Euler(0,0,rotation);
    }
}
