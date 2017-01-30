using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behaviour responsible for moving the boat through a series
/// of waypoints.
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

    /// <summary>
    /// Squared error used in distance to target calculation.
    /// </summary>
    private const float SqrError = 0.01f;

    private List<Transform>.Enumerator waypoints;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        waypoints = Waypoints.GetEnumerator();
        UpdateVelocity();
    }

    private void FixedUpdate()
    {
        if (waypoints.Current != null && // treats edge case where there are no waypoints
            (waypoints.Current.position - transform.position).sqrMagnitude <= SqrError)
        {
            UpdateVelocity();
        }
    }

    private void UpdateVelocity()
    {
        if (waypoints.MoveNext())
        {
            rigidBody.velocity = (waypoints.Current.position - transform.position).normalized * Speed;
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }
}
