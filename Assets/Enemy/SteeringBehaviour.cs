using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class SteeringBehaviour
{
    public static Vector3 GetRotation(Transform self, Transform player, Rigidbody playerRb, float maxPredictionTime)
    {
        Vector3 futurePos = GetFuturePos(self, player, playerRb, maxPredictionTime);
        Vector3 dir = (futurePos - self.position).normalized;
        return dir;
    }

    public static Vector3 GetFuturePos(Transform self, Transform player, Rigidbody playerRb, float maxPredictionTime)
    {
        Vector3 targetVelocity = Vector3.zero;
        if (playerRb != null)
        {
            targetVelocity = playerRb.linearVelocity;
        }

        Vector3 toTarget = player.position - self.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;
        float predictionTime = Mathf.Clamp(distance / 5f, 0f, maxPredictionTime);

        Vector3 futurePos = player.position + targetVelocity * predictionTime;

        return futurePos;
    }

    /*public static Vector3 Wander(Transform self, )
    {

    }*/

    public static ThrowData GetPredictedPositionThrowData(Vector3 targetPosition, Vector3 startPosition, Rigidbody playerRb)
    {
        ThrowData directThrowData = CalculateThrowData(targetPosition, startPosition);

        Vector3 throwVelocity = directThrowData.throwVelocity;
        throwVelocity.y = 0f;
        float time = directThrowData.deltaXZ / throwVelocity.magnitude;
        Vector3 playerMovement;

        playerMovement = playerRb.linearVelocity * time;
        
        Vector3 newTargetPosition = new Vector3(
            targetPosition.x + playerMovement.x,
            startPosition.y,
            targetPosition.z + playerMovement.z);

        ThrowData predictiveThrowData = CalculateThrowData(
            newTargetPosition,
            startPosition
            );

        return predictiveThrowData;
    }

    private static ThrowData CalculateThrowData(Vector3 targetPosition, Vector3 startPosition)
    {
        Vector3 displacement = new Vector3(
            targetPosition.x,
            startPosition.y,
            targetPosition.z
            ) - startPosition;
        float deltaY = targetPosition.y - startPosition.y;
        float deltaXZ = displacement.magnitude;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float throwStrenght = Mathf.Clamp(
            Mathf.Sqrt(
                gravity
                * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY, 2)
                + Mathf.Pow(deltaXZ, 2)))),
                0.01f,
                100f
        );

        float angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));

        Vector3 initialVelocity = Mathf.Cos(angle) * throwStrenght * displacement.normalized
            + Mathf.Sin(angle) * throwStrenght * Vector3.up;

        return new ThrowData(angle, deltaY, deltaXZ, initialVelocity);
    }
}
