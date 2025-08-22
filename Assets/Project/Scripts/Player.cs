using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action<int> OnTakeDamage;

    Rigidbody2D _rb;
    float _moveX;
    bool _isJumping;
    bool _isCrouching;
    bool _isGrounded;
    LayerMask _groundLayer;
    Transform _groundCheck;
    bool _isAlive = true;

    [SerializeField] float _jumpForce = 5.5f;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _groundRadius = 0.08f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _hitClip;
    [SerializeField] Animator _animator;

    public void Initiate(Vector3 position, float jumpForce, float moveSpeed)
    {
        gameObject.transform.position = position;
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _groundCheck = this.transform.Find("GroundCheck");
        _groundLayer = LayerMask.GetMask("Ground");

        // More fluid movement
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        _moveX = Input.GetAxis("Horizontal");
        _isJumping = Input.GetKey(KeyCode.Space);
        _isCrouching = Input.GetKey(KeyCode.DownArrow);
        Vector3 scale = this._rb.transform.localScale;
        if (_moveX > 0) // Face right
            this._rb.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        else if (_moveX < 0)  // Face left
            this._rb.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
    }

    void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        _rb.velocity = new Vector2(_moveX * _moveSpeed, _rb.velocity.y);
        if (_isGrounded && _rb.velocity.y < 0f)
            _rb.velocity = new Vector2(_rb.velocity.x, -2f); // negative value to be sure to touch the ground
        if (_isGrounded && _isJumping)
            Jump();
        SetAnimation();
    }

    void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        _audioSource.PlayOneShot(_jumpClip);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer) != null;
    }

    void SetAnimation()
    {
        this._animator.SetBool("isGrounded", _isGrounded);
        this._animator.SetBool("isCrouching", _isCrouching);
        this._animator.SetBool("isJumping", _isJumping);
        this._animator.SetFloat("moveX", Math.Abs(_moveX));
        this._animator.SetFloat("velocityY", _rb.velocity.y);
        this._animator.SetBool("isAlive", _isAlive);
    }


    public void TakeDamage(int hit)
    {
        _rb.velocity = new Vector2(-_rb.velocity.x, _jumpForce);
        OnTakeDamage?.Invoke(hit);
        StartCoroutine(BlinkRed());
    }

    public IEnumerator BlinkRed()
    {
        _audioSource.PlayOneShot(_hitClip);
        this.GetComponent<SpriteRenderer>().color = Color.red; // Set color to red
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white; // Reset color to white
    }


    public void Killed()
    {
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        _isAlive = false;
        Debug.Log("Player is dead!");
        _audioSource.PlayOneShot(_hitClip);
        GetComponent<CapsuleCollider2D>().isTrigger = true; // Disable collision
        this.GetComponent<SpriteRenderer>().color = Color.red; // Set color to red
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }

    // DEBUG
    void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
        }
    }
}
