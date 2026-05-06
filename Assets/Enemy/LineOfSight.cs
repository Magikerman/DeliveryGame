using UnityEngine;
using UnityEngine.Assertions.Must;

public static class LineOfSight
{
    public static bool HasLoS(Transform self, Transform target, float range, float angle, LayerMask layer)
    {
        return InRange(self, target, range) && InAngle(self, target, angle) && InSight(self, target, layer);
    }

    public static bool InRange(Transform self, Transform target, float range)
    {
        return (target.position - self.position).magnitude <= range;
    }

    private static bool InAngle(Transform self, Transform target, float angle)
    {
        Vector3 dir = (target.position - self.position).normalized;
        return Vector3.Angle(self.forward, dir) < angle/2;
    }

    private static bool InSight(Transform self, Transform target, LayerMask layer)
    {
        Vector3 dir = (target.position - self.position);
        return !Physics.Raycast(self.position, dir, dir.magnitude, layer);
    }
}
