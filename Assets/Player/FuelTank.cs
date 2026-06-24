using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    [SerializeField] private float maxValue;
    private float currentValue;

    private bool isEmpty;
    public bool IsEmpty => isEmpty;

    [SerializeField] private Image bar;
    [SerializeField] private PlayerController player;
    [SerializeField] private float drainMult;

    private void Awake()
    {
        currentValue = maxValue;
    }

    private void Update()
    {
        currentValue -= (player.Rb.linearVelocity.magnitude * Time.deltaTime) * drainMult/10;

        bar.fillAmount = currentValue/maxValue;

        if (currentValue <= 0)
        {
            isEmpty = true;
            OnEmpty();
        }
    }

    public void Refill(float amount = 10f)
    {
        if (currentValue + amount >= maxValue) currentValue = maxValue;
        else currentValue += amount;
    }

    private void OnEmpty()
    {
        player.Die();
    }
}
