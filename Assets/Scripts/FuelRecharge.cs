using UnityEngine;

public class FuelRecharge : MonoBehaviour
{
    [SerializeField] private float refill;
    private void OnTriggerEnter(Collider other)
    {
        FuelTank fuel = other.GetComponent<FuelTank>();

        if (fuel != null)
        {
            fuel.Refill(refill);
            Destroy(gameObject);
        }
    }
}
