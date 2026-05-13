using UnityEngine;

public class FuelRecharge : MonoBehaviour
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private float verticalSpeed;
    private Vector3 originalPos;

    private float sineValue;

    private void Awake()
    {
        originalPos = transform.position;
    }

    private void FixedUpdate()
    {
        sineValue = Mathf.Sin(Time.time) * verticalSpeed;

        transform.Rotate(new Vector3(0,Time.fixedDeltaTime * rotSpeed, 0));
        transform.position = new Vector3(originalPos.x, originalPos.y + sineValue, originalPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        FuelTank fuel = other.GetComponent<FuelTank>();

        if (fuel != null)
        {
            fuel.Refill(30f);
            Destroy(gameObject);
        }
    }
}
