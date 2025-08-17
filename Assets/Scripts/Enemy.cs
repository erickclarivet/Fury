using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// if osbtacle (hole, wall) going back
enum MovementDirection
{
    Left,
    Right
}

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LayerMask _groundLayer;
    private Transform _groundCheck;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundRadius;
    [SerializeField] private MovementDirection _direction = MovementDirection.Right;

    // Start is called before the first frame update
    void Start()
    {
        _groundLayer = LayerMask.GetMask("Ground");
        _groundCheck = this.transform.Find("GroundCheck");
        _rb = GetComponent<Rigidbody2D>();

        // More fluid movement
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_direction == MovementDirection.Right)
            _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
        else
            _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
        if (!IsGrounded())
        {
            // If not grounded, reverse direction
            if (_direction == MovementDirection.Right)
            {
                _direction = MovementDirection.Left;
                this._rb.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                _direction = MovementDirection.Right;
                this._rb.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer) != null;
    }

    void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius); // même radius que l'OverlapCircle
        }
    }
}
