using System.Collections;
using UnityEngine;

enum MovementStates
{
    Grounded,
    WallSliding,
    Juming,
}
public class PlayerController : MonoBehaviour
{
    private MovementStates _currentState;
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _wallJump;
    [SerializeField] private AudioClip _collectCoin;
    [SerializeField] private AudioClip _hitEnemie;
    [SerializeField] private AudioClip _getHitEnemie;
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 20;
    [SerializeField] private float _jumpSpeedMultiplier;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 25;
    private bool _canJump;
    [Header("Walls")]
    [SerializeField] private float maxWallSlideSpeed;
    [Header("Stun")]
    [SerializeField] private float _stunEffectMultiplier;
    [SerializeField] private float _stunTime = 0.5f;
    private bool _isStunned;
    private float _stunEffect = 1f;
    [SerializeField] private int _enemieLayer;
    [Header("Raycast")]
    [SerializeField] private float _wallRayCastLength = 0.1f;
    [SerializeField] private int _groundLayer;
    [SerializeField] private float _groundRayCastLength = 0.25f;
    [SerializeField] private float _offsetY = 0.01f;
    private float _bodyWith;
    private bool _facingRight = true;
    [Header("Score")]
    [SerializeField] private int _enemyMultiplier;
    [SerializeField] private int _coinMultiplier;
    [SerializeField] private int _heightMultiplier;

    [SerializeField] private ParticleSystem _running;
    [SerializeField] private ParticleSystem _jumping;

    public int CoinCount { get; private set; }
    public int HeightCount { get; private set; }
    public int EnemieCount { get; private set; }
    public int Score { get; private set; }
    private int _currentScore;

    private void Awake()
    {
        _currentState = MovementStates.Juming;
        _groundLayer = 1 << _groundLayer;
        _enemieLayer = 1 << _enemieLayer;
        _bodyWith = _collider.size.x / 2 - 0.01f;
    }
    private void Start()
    {
        ApplyMoveSpeed();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) { return; }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.PauseGame();
        }
        else if(Time.timeScale == 1)
        {
            CalculateScore();
            CheckForGround();
            CheckForJump();
            ApplyMoveSpeed();
        }
    }
    private void CheckForJump()
    {
        if (Input.anyKeyDown)
        {
            if (_currentState == MovementStates.Grounded)
            {
                PlaySound(_jump);
                Jump();
            }
            else if (_canJump && _currentState != MovementStates.WallSliding)
            {
                _canJump = false;
                PlaySound(_jump);
                Jump();
            }
            else if (_currentState == MovementStates.WallSliding)
            {
                _canJump = true;
                ChangeDirection();
                PlaySound(_wallJump);
                Jump();
            }
        }
    }
    private void CalculateScore()
    {
        if (HeightCount < Mathf.FloorToInt(transform.position.y))
        {
            HeightCount = Mathf.FloorToInt(transform.position.y);
        }
        _currentScore = (HeightCount * _heightMultiplier) + (CoinCount * _coinMultiplier) + (EnemieCount * _enemyMultiplier);
        Score = _currentScore;
    }
    private void Jump()
    {
        _jumping.Play();
        _running.Stop();
        _currentState = MovementStates.Juming;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Force);
    }
    private void CheckForGround()
    {
        Debug.DrawRay(new Vector2(transform.position.x - _bodyWith, transform.position.y + _offsetY), Vector2.down * _groundRayCastLength, Color.green, 0f, false);
        Debug.DrawRay(new Vector2(transform.position.x + _bodyWith, transform.position.y + _offsetY), Vector2.down * _groundRayCastLength, Color.green, 0f, false);

        RaycastHit2D GroundHitLeft  = Physics2D.Raycast(new Vector2(transform.position.x - _bodyWith, transform.position.y + _offsetY), Vector2.down, _groundRayCastLength, _groundLayer);
        RaycastHit2D GroundHitRight = Physics2D.Raycast(new Vector2(transform.position.x + _bodyWith, transform.position.y + _offsetY), Vector2.down, _groundRayCastLength, _groundLayer);

        if (GroundHitLeft.collider != null || GroundHitRight.collider != null)
        {
            if(_currentState != MovementStates.Grounded)
            {
                _running.Play();
            }

            _currentState = MovementStates.Grounded;

            if (IsWalled())
            {
                ChangeDirection();
            }
            _canJump = true;
        }
        else
        {
            if(IsWalled())
            {
                _currentState = MovementStates.WallSliding;
            }
        }
    }
    private void StunPlayer()
    {
        if (!_isStunned)
        {
            _rigidbody.excludeLayers = _enemieLayer;
            _isStunned = true;
            StartCoroutine(StunTimer());
        }
    }
    private IEnumerator StunTimer()
    {
        _stunEffect = _stunEffectMultiplier;
        yield return new WaitForSeconds(_stunTime);
        _rigidbody.excludeLayers = 0;
        _stunEffect = 1;
        _isStunned = false;
    }
    private bool IsWalled()
    {
        float bodyWith = _facingRight ? _bodyWith : -_bodyWith;
        Vector2 VectorDirection = _facingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(new Vector2(transform.position.x + bodyWith, transform.position.y), VectorDirection * _wallRayCastLength, Color.green, 0f, false);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(transform.position.x + bodyWith, transform.position.y), VectorDirection, _wallRayCastLength, _groundLayer);

        if (rightHit.collider != null)
        {
            return true;
        }
        return false;
    }
    private void ApplyMoveSpeed()
    {
        switch(_currentState)
        {
            case MovementStates.Grounded:
                _rigidbody.velocity = new Vector2(_moveSpeed * _stunEffect, _rigidbody.velocity.y);
                break;
            case MovementStates.WallSliding:
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -maxWallSlideSpeed, maxWallSlideSpeed)) * _stunEffect;
                break;
            case MovementStates.Juming:
                _rigidbody.velocity = new Vector2(_moveSpeed * _stunEffect, _rigidbody.velocity.y);
                break;
        }
    }
    private void ChangeDirection()
    {
        _facingRight = !_facingRight;
        if (_facingRight)
        {
            _moveSpeed = Mathf.Abs(_moveSpeed);
        }
        else
        {
            _moveSpeed = -Mathf.Abs(_moveSpeed);
        }
        ApplyMoveSpeed();

        Vector3 scale = _animator.transform.localScale;
        scale.x *= -1;
        _animator.transform.localScale = scale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;

            bool isCollisionBelow = collisionDirection.y > 0;

            if (isCollisionBelow)
            {
                StartCoroutine(CameraEffector.Instance.ShakeCamera(1, 1));
                PlaySound(_getHitEnemie);
                StunPlayer();   
            }
            else
            {
                try
                {
                    StartCoroutine(collision.gameObject.GetComponentInParent<EnemieMovement>().GetHit());
                }
                finally
                {
                    EnemieCount += 1;
                    StartCoroutine(CameraEffector.Instance.ShakeCamera(0.5f, 0.5f));
                    PlaySound(_hitEnemie);
                    Jump();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            GameController.Instance.EndGame();
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Coins"))
        {
            StartCoroutine(CameraEffector.Instance.ShakeCamera(0.5f, 0.1f));
            CoinCount += 1;
        }
    }
    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
