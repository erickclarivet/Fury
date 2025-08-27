using System.Collections.Generic;
using UnityEngine;

enum PlatformDirection
{
    Forward,
    Backward
}

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject _platform;
    [SerializeField] List<Transform> _waypoints;
    [SerializeField] bool _changeDirection = false;
    [SerializeField] float _speed = 2f; 

    int _currentWaypointIndex = 1;
    PlatformDirection _direction = PlatformDirection.Forward;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(_platform.transform.localPosition, _waypoints[_currentWaypointIndex].localPosition) < 0.1f)
        {
             if (_direction == PlatformDirection.Forward)
            {
                if (_currentWaypointIndex < _waypoints.Count - 1)
                {
                    _currentWaypointIndex++;
                }
                else
                {
                    _direction = PlatformDirection.Backward;
                    if (_changeDirection)
                        _platform.transform.localScale = new Vector3(-Mathf.Abs(_platform.transform.localScale.x), _platform.transform.localScale.y, _platform
                        .transform.localScale.z);
                    _currentWaypointIndex--;
                }
            }
            else
            {
                if (_currentWaypointIndex > 0)
                {
                    _currentWaypointIndex--;
                }
                else
                {
                    _direction = PlatformDirection.Forward;
                    if (_changeDirection)
                        _platform.transform.localScale = new Vector3(Mathf.Abs(_platform.transform.localScale.x), _platform.transform.localScale.y, _platform
                        .transform.localScale.z);
                    _currentWaypointIndex++;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 targetPosition = _waypoints[_currentWaypointIndex].localPosition;
        _platform.transform.localPosition = Vector2.MoveTowards(_platform.transform.localPosition, targetPosition, _speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(_platform.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
