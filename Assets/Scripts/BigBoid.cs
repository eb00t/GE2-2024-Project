using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoid : MonoBehaviour
{
 public Vector3 velocity;
    public float speed;
    public Vector3 acceleration;
    public Vector3 force;
    public float maxSpeed = 5;
    public float maxForce = 10;

    public float mass = 1;
    
    public Transform seekTargetTransform;
    public Vector3 seekTarget;
    
    public Transform arriveTargetTransform;
    public Vector3 arriveTarget;
    public float slowingDistance = 80;

    public Path path;
    public bool pathFollowingEnabled = false;
    public float waypointDistance = 3;

    // Banking
    public float banking = 0.1f; 

    public float damping = 0.1f;
    
    public BigBoid pursueTarget;

    public Vector3 pursueTargetPos; 
    
    public BigBoid leader;
    public Vector3 offset;
    private Vector3 worldTarget;
    private Vector3 targetPos;
    
    public enum State
    {
        Seek,
        Arrive,
        PathFollowing,
        Pursue,
        OffsetPursue
    }

    public State _state;

    public Vector3 Pursue(BigBoid pursueTarget)
    {
        float dist = Vector3.Distance(pursueTarget.transform.position, transform.position);
        float time = dist / maxSpeed;
        pursueTargetPos = pursueTarget.transform.position 
                    + pursueTarget.velocity * time;
        return Seek(pursueTargetPos);
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);

        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + force * 10);

        if (_state == State.Arrive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(arriveTargetTransform.position, slowingDistance);
        }

        if (_state == State.Pursue && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, pursueTargetPos);
        }

    }

    public Vector3 OffsetPursue(BigBoid leader)
    {
        // This is a bug!!
        //worldTarget = leader.transform.TransformPoint(offset);
        worldTarget = (leader.transform.rotation * offset) 
                + leader.transform.position;


        float dist = Vector3.Distance(transform.position, worldTarget);
        float time = dist / maxSpeed;

        targetPos = worldTarget + (leader.velocity * time);
        return Arrive(targetPos);
    }
    
    void Start()
    {
        if (_state == State.OffsetPursue)
        {
            offset = transform.position - leader.transform.position;
            offset = Quaternion.Inverse(leader.transform.rotation) * offset;
        }
    }
    
    public Vector3 PathFollow()
    {
        Vector3 nextWaypoint = path.Next();
        if (!path.isLooped && path.IsLast())
        {
            return Arrive(nextWaypoint);
        }
        else
        {
            if (Vector3.Distance(transform.position, nextWaypoint) < waypointDistance)
            {
                path.AdvanceToNext();
            }
            return Seek(nextWaypoint);
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return (desired - velocity);
    } 

    public Vector3 Arrive(Vector3 target)
    {
       Vector3 toTarget = target - transform.position;
       float dist = toTarget.magnitude;
       if (dist == 0.0f)
       {
           return Vector3.zero;
       }
       float ramped = (dist / slowingDistance) * maxSpeed;
       float clamped = Mathf.Min(ramped, maxSpeed);
       Vector3 desired = clamped * (toTarget / dist);
       return desired - velocity;
    }

    public Vector3 CalculateForce()
    {
        Vector3 f = Vector3.zero;

        switch (_state)
        {
            case State.Seek:
                if (seekTargetTransform != null)
                {
                    seekTarget = seekTargetTransform.position;
                }
                f += Seek(seekTarget);
                break;
            case State.Arrive:
                if (arriveTargetTransform != null)
                {
                    arriveTarget = arriveTargetTransform.position;                
                }
                f += Arrive(arriveTarget);
                break;
            case State.PathFollowing:
                f += PathFollow();
                break;
            case State.Pursue:
                f += Pursue(pursueTarget);
                break;
            case State.OffsetPursue:
                f += OffsetPursue(leader);
                break;
        }
        return f;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        force = CalculateForce();
        acceleration = force / mass;
        velocity = velocity + acceleration * Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
        speed = velocity.magnitude;
        if (speed > 0)
        {
            //transform.forward = velocity;
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + velocity, tempUp);

            //velocity *= 0.9f;

            // Remove 10% of the velocity every second
            velocity -= (damping * velocity * Time.deltaTime);
        }        
    }
}

