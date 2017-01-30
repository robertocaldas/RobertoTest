using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behaviour responsible for moving the boat through a series
/// of waypoints, adjusting its position and rotation accordingly.
/// </summary>
public class BoatBehaviour : MonoBehaviour
{
    /// <summary>
    /// Boat speed in units/s.
    /// </summary>
    /// <remarks>
    /// Ideally shouldn't be greater than
    /// 2 * Error / FixedTimestep = 10 units/s
    /// otherwise it may skip a waypoint.
    /// </remarks>
    public float Speed = 1;

    public List<Transform> Waypoints;

    public Transform BoatModel;

    /// <summary>
    /// Squared error used in distance to target calculation.
    /// </summary>
    private const float SqrError = 0.01f;

    private List<Transform>.Enumerator waypoints;
    private Rigidbody rigidBody;

    Quaternion lastRotation;

    float timeSinceLastWaypoint = 0;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        waypoints = Waypoints.GetEnumerator();

        lastRotation = BoatModel.rotation;
    }

    private void FixedUpdate()
    {
        if (waypoints.Current == null || // treats edge case where there are no waypoints
            (waypoints.Current.position - transform.position).sqrMagnitude <= SqrError)
        {
            if (waypoints.MoveNext())
            {
                lastRotation = BoatModel.rotation;
                timeSinceLastWaypoint = Time.time;
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
                return;
            }
        }

        BoatModel.rotation = Quaternion.Lerp(lastRotation, 
            Quaternion.LookRotation(waypoints.Current.transform.position - transform.position), 
            (Time.time - timeSinceLastWaypoint) * Speed * 0.75f);

        rigidBody.velocity = BoatModel.forward * Speed;
    }
}
