using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
    public float scale = 4.0f;
    public float forwardFeelerDepth = 30;
    public float sideFeelerDepth = 15;
    FeelerInfo[] feelers = new FeelerInfo[5];

    public float frontFeelerUpdatesPerSecond = 10.0f;
    public float sideFeelerUpdatesPerSecond = 5.0f;

    public float feelerRadius = 2.0f;

    public enum ForceType
    {
        normal,
        incident,
        up,
        braking
    };

    public ForceType forceType = ForceType.normal;

    public LayerMask mask = -1;

    public void OnEnable()
    {
        StartCoroutine(UpdateFrontFeelers());
        StartCoroutine(UpdateSideFeelers());
    }

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled)
        {
            foreach (FeelerInfo feeler in feelers)
            {
                Gizmos.color = Color.gray;
                if (Application.isPlaying)
                {
                    Gizmos.DrawLine(transform.position, feeler.point);
                }
                if (feeler.collided)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(feeler.point, feeler.point + (feeler.normal * 5));
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(feeler.point, feeler.point + force);
                }
            }
        }
    }

    Vector3 lerpedForce = Vector3.zero;
    public override Vector3 Calculate()
    {
        force = Vector3.zero;

        for (int i = 0; i < feelers.Length; i++)
        {
            FeelerInfo info = feelers[i];
            if (info.collided)
            {
                force += CalculateSceneAvoidanceForce(info);
            }
        }
        lerpedForce = Vector3.Lerp(lerpedForce,force, Time.deltaTime);
        return lerpedForce;
    }

    void UpdateFeeler(int feelerNum, Quaternion localRotation, float baseDepth, FeelerInfo.FeeelerType feelerType)
    {
        Vector3 direction = localRotation * transform.rotation * Vector3.forward;
        float depth = baseDepth + ((boid.velocity.magnitude / boid.maxSpeed) * baseDepth);

        RaycastHit info;
        bool collided = Physics.SphereCast(transform.position, feelerRadius, direction, out info, depth, mask.value);
        Vector3 feelerEnd = collided ? info.point : (transform.position + direction * depth);
        feelers[feelerNum] = new FeelerInfo(feelerEnd, info.normal
            , collided, feelerType);
    }

    System.Collections.IEnumerator UpdateFrontFeelers()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        while (true)
        {
            UpdateFeeler(0, Quaternion.identity, this.forwardFeelerDepth, FeelerInfo.FeeelerType.front);
            yield return new WaitForSeconds(1.0f / frontFeelerUpdatesPerSecond);
        }
    }

    System.Collections.IEnumerator UpdateSideFeelers()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        float angle = 45;
        while (true)
        {
            // Left feeler
            UpdateFeeler(1, Quaternion.AngleAxis(angle, Vector3.up), sideFeelerDepth, FeelerInfo.FeeelerType.side);
            // Right feeler
            UpdateFeeler(2, Quaternion.AngleAxis(-angle, Vector3.up), sideFeelerDepth, FeelerInfo.FeeelerType.side);
            // Up feeler
            UpdateFeeler(3, Quaternion.AngleAxis(angle, Vector3.right), sideFeelerDepth, FeelerInfo.FeeelerType.side);
            // Down feeler
            UpdateFeeler(4, Quaternion.AngleAxis(-angle, Vector3.right), sideFeelerDepth, FeelerInfo.FeeelerType.side);

            yield return new WaitForSeconds(1.0f / sideFeelerUpdatesPerSecond);
        }
    }
    

    Vector3 CalculateSceneAvoidanceForce(FeelerInfo info)
    {
        Vector3 force = Vector3.zero;

        Vector3 fromTarget = fromTarget = transform.position - info.point;
        float dist = Vector3.Distance(transform.position, info.point);

        switch (forceType)
        {
            case ForceType.normal:
                force = info.normal * (forwardFeelerDepth * scale / dist);
                break;
            case ForceType.incident:
                fromTarget.Normalize();
                force -= Vector3.Reflect(fromTarget, info.normal) * (forwardFeelerDepth / dist);
                break;
            case ForceType.up:
                force += Vector3.up * (forwardFeelerDepth * scale / dist);
                break;
            case ForceType.braking:
                force += fromTarget * (forwardFeelerDepth / dist);
                break;
        }
        return force;
    }
    
    struct FeelerInfo
    {
        public Vector3 point;
        public Vector3 normal;
        public bool collided;
        public FeeelerType feelerType;

        public enum FeeelerType
        {
            front,
            side
        };

        public FeelerInfo(Vector3 point, Vector3 normal, bool collided, FeeelerType feelerType)
        {
            this.point = point;
            this.normal = normal;
            this.collided = collided;
            this.feelerType = feelerType;
        }
    }
}