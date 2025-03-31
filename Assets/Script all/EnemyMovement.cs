using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] waypoints; 
    private int currentWaypoint = 0;
    public float speed = 2f;
    private BaseHealth baseHealth;

    // Sự kiện khi Enemy đến điểm cuối
    public event Action OnReachEnd;

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("❌ Enemy không có waypoint!");
            return;
        }

        transform.position = waypoints[0].position;
        Debug.Log("✅ Waypoints đã gán cho: " + gameObject.name);
    }

    void Start()
    {
        baseHealth = FindObjectOfType<BaseHealth>();
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(10);
                }

                // Gọi sự kiện khi enemy đến điểm cuối
                OnReachEnd?.Invoke();
                
                Destroy(gameObject);
            }
        }
    }
}
