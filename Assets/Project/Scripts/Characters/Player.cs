using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action<int> OnTakeDamage;


    [Header("Player settings")]
    [SerializeField] float _jumpForce = 5.5f;
    [SerializeField] float _moveSpeed = 5f;
    Rigidbody2D _rb;
    float _moveX;
    bool _isJumping;
    bool _isCrouching;
    bool _isGrounded;
    bool _isAlive = true;

    [Header("Audio/Animator")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _hitClip;
    [SerializeField] Animator _animator;

    [Header("InputManagement")]
    PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    [Header("LayerCheck")]
    [SerializeField] float _groundRadius = 0.08f;
    [SerializeField] Transform _groundCheck;
    [SerializeField] Transform _ceillingCheck;
    LayerMask _groundLayer;

    public void Initiate(Vector3 position, float jumpForce, float moveSpeed)
    {
        gameObject.transform.position = position;
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
    }

    private void Awake()
    {
        var savedMoveAction = PlayerPrefs.GetString("PlayerInput", "MoveKeyboard");
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions[savedMoveAction];
        _jumpAction = _playerInput.actions["Jump"];
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _groundLayer = LayerMask.GetMask("Ground");

        // More fluid movement
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        _jumpAction.Enable();
        _moveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = _moveAction.ReadValue<Vector2>();
        _isJumping = _jumpAction.IsPressed();
        _moveX = move.x;

        var isCrouching = move.y < 0;
        if (isCrouching || (!isCrouching && _isCrouching && IsTouchCeilling()))
        {
            _rb.transform.GetComponent<CircleCollider2D>().isTrigger = true;
            _isCrouching = true;
        }
        else
        {
            _rb.transform.GetComponent<CircleCollider2D>().isTrigger = false;
            _isCrouching = false;
        }
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
    private void OnDisable()
    {
        _jumpAction.Disable();
        _moveAction.Disable();
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

    bool IsTouchCeilling()
    {
        return Physics2D.OverlapCircle(_ceillingCheck.position, _groundRadius, _groundLayer) != null;
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
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
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
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        this.GetComponent<SpriteRenderer>().color = Color.red;
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
