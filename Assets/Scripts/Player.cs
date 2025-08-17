using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// mettre la mort quand ca atteint tombe dans le vide ! v
// Ajouter 1 niveau v
// Ajouter Music v
// Best sprite handling v
// Ajouter bruitage (mort, win) v
// score v
// Fix score qui tremble v
// refacto
// fix le score
// Ajouter le son de mort quand on prend un degat
// Pause + affichage score quand mort
// ajouter joystick mobile
// GAMEPLAY : DEVENIR PETIT POUR PASSER DES OBSTACLES

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _moveX;
    private bool _isJumping;
    private bool _isCrouching;
    private bool _isGrounded;
    private LayerMask _groundLayer;
    private Transform _groundCheck;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _deathClip;

    [SerializeField] private int _score;
    [SerializeField] private int _life;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _groundRadius = 0.05f;
    [SerializeField] private Life _lifeScript;
    [SerializeField] private Score _scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        this._rb = this.GetComponent<Rigidbody2D>();
        this._animator = this.GetComponent<Animator>();
        this._groundCheck = this.transform.Find("GroundCheck");
        this._groundLayer = LayerMask.GetMask("Ground");

        // More fluid movement
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.freezeRotation = true;
        _life = _lifeScript.InitLife(4);
        _score = _scoreScript.UpdateScore();
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

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        _rb.velocity = new Vector2(_moveX * _moveSpeed, _rb.velocity.y);
        if (_isGrounded && _rb.velocity.y < 0f)
            _rb.velocity = new Vector2(_rb.velocity.x, -2f); // une petite valeur négative stable
        if (_isGrounded && _isJumping)
            Jump();
        SetAnimation();
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        _audioSource.PlayOneShot(_jumpClip);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer) != null;
    }

    private void SetAnimation()
    {
        this._animator.SetBool("isGrounded", this._isGrounded);
        this._animator.SetBool("isCrouching", this._isCrouching);
        this._animator.SetBool("isJumping", this._isJumping);
        this._animator.SetFloat("moveX", Math.Abs(this._moveX));
        this._animator.SetFloat("velocityY", this._rb.velocity.y);
        this._animator.SetInteger("life", this._life);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            StartCoroutine(BlinkRed());
            if (_life == 0)
            {
                Killed();
            }
        }
    }

    public IEnumerator BlinkRed()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red; // Set color to red
        _life = this._lifeScript.LooseHeart();
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white; // Reset color to white
    }


    public void Killed()
    {
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        this._life = 0;
        Debug.Log("Player is dead!");
        _audioSource.PlayOneShot(_deathClip);
        GetComponent<CapsuleCollider2D>().isTrigger = true; // Disable collision
        this.GetComponent<SpriteRenderer>().color = Color.red; // Set color to red
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }

    public void AddPoints(int points)
    {
        _score = this._scoreScript.UpdateScore(points);
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
