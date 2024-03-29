using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

   
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class Controller : MonoBehaviour, IPlayerController
    {
        [SerializeField] private bool _allowDoubleJump, _allowDash;

        // Public for external hooks
        public FrameInput Input { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => _grounded;
        public event Action<bool> OnGroundedChanged;
        public event Action OnJumping, OnDoubleJumping;
        public event Action<bool> OnDashingChanged;
        public Animator animator;

        private Rigidbody2D _rb;
        private BoxCollider2D _collider;
        private Vector3 _lastPosition;
        private Vector3 _velocity;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;
        private int _fixedFrame;
        [SerializeField]
        private int facingDirection;
        private float moveInput;
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
          
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public bool flip = false;
     
        private void Update()
        {
           
               if(Input.X ==1 ){
                 transform.localScale = new Vector3(2, 2, 2);
               }
               if(Input.X== -1){
                  transform.localScale = new Vector3(-2, 2, 2);
               }
               
            
            moveInput = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (Math.Abs(moveInput) > 0&&Grounded)
            {
                animator.SetBool("walking", true);
               // audioManager.play("walking");
            }
            else
            {
                audioManager.Stop("walking");
                audioManager.Stop("walking2");
                animator.SetBool("walking", false);
            }
            facingDirection = (int)transform.localScale.x;
            // Calculate velocity
            _velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
        }
    public bool returning = true;
   public AudioManager audioManager;
        void FixedUpdate()
        {

            _fixedFrame++;

            RunCollisionChecks();

            CalculateWalk();
            CalculateJumpApex();
            CalculateGravity();
            CalculateJump();
            CalculateDash();
            MoveCharacter();
        }

        #region Gather Input

        private void GatherInput()
        {
      
{
    Input = new FrameInput
    {
        JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
        JumpHeld = UnityEngine.Input.GetButton("Jump"),
        DashDown = UnityEngine.Input.GetButtonDown("Dash"),
        X = UnityEngine.Input.GetAxisRaw("Horizontal"),
        Y = UnityEngine.Input.GetAxisRaw("Vertical")
    };

    if (Input.DashDown) _dashToConsume = true;
    if (Input.JumpDown)
    {
        _lastJumpPressed = _fixedFrame;
        _jumpToConsume = true;
    }
}

        }

        #endregion

        #region Collisions

        [Header("COLLISION")][SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;

        public bool _hittingCeiling, _grounded, _colRight, _colLeft;

        private float _timeLeftGrounded;

        public Transform safepos;
        // We use these raycast checks for pre-collision information
        public LayerMask layerMask;
        public float detectrange;
        public Transform groundcheckPoint;
        public bool groundedCheck;
        public Transform leftwallPoint;
        public Transform rightwallPoint;
        public AnimationManagerWookie animationManager;
        private void RunCollisionChecks()
        {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
             bool groundedCheck = RunDetection(_raysDown);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(groundcheckPoint.position, detectrange, layerMask);
            if (hitEnemies.Length > 0)
            {
                groundedCheck = true;
            }
            else
            {
                groundedCheck = false;
            }
            if (_grounded && !groundedCheck)
            {
                _timeLeftGrounded = _fixedFrame; // Only trigger when first leaving
                                                 // safepos.position = transform.position;
                OnGroundedChanged?.Invoke(false);
            }
            else if (!_grounded && groundedCheck)
            {
                _coyoteUsable = true; // Only trigger when first touching
                audioManager.Play("land");
                animationManager.Land();
                _executedBufferedJump = false;
                _doubleJumpUsable = true;
                _canDash = true;
                OnGroundedChanged?.Invoke(true);
            }

            _grounded = groundedCheck;
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);

            // The rest
            _hittingCeiling = RunDetection(_raysUp);

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged()
        {
            var b = _collider.bounds;

            _raysDown = new RayRange(b.min.x, b.min.y, b.max.x, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x, b.max.y, b.max.x, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y, b.min.x, b.max.y, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y, b.max.x, b.max.y, Vector2.right);
        }


        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
             Gizmos.DrawWireSphere(groundcheckPoint.position,detectrange);
            if (!_collider) _collider = GetComponent<BoxCollider2D>();

            // Rays
            if (!Application.isPlaying) CalculateRayRanged();
            Gizmos.color = Color.blue;
            foreach (var range in new List<RayRange> { _raysDown, _raysUp })
            {
                foreach (var point in EvaluateRayPositions(range))
                {
                    Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                }
            }
        }

        #endregion


        #region Walk

        [Header("WALKING")][SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (Input.X != 0)
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += Input.X * _acceleration * Time.fixedDeltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.fixedDeltaTime;
            }
            else
            {
                // No input. Let's slow the character down
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.fixedDeltaTime);
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
            {
                // Don't pile up useless horizontal
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;


        private void CalculateGravity()
        {
            if (_grounded)
            {
                // // Move out of the ground
                if (_currentVerticalSpeed < 0)
                {
                    _currentVerticalSpeed = 0;
                }
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.fixedDeltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")][SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private int _coyoteTimeThreshold = 7;
        [SerializeField] private int _jumpBuffer = 7;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _jumpToConsume;
        private bool _coyoteUsable;
        private bool _executedBufferedJump;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed = Single.MinValue;
        private bool _doubleJumpUsable;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _timeLeftGrounded + _coyoteTimeThreshold > _fixedFrame;
        private bool HasBufferedJump => (_grounded || _cornerStuck) && _lastJumpPressed + _jumpBuffer > _fixedFrame && !_executedBufferedJump;
        private bool CanDoubleJump => _allowDoubleJump && _doubleJumpUsable && !_coyoteUsable;

        private void CalculateJumpApex()
        {
            if (!_grounded)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            if (_jumpToConsume && CanDoubleJump)
            {
                  audioManager.Play("jump");
                _currentVerticalSpeed = _jumpHeight;
                _doubleJumpUsable = false;
                _endedJumpEarly = false;
                _jumpToConsume = false;
                OnDoubleJumping?.Invoke();
            }


            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if ((_jumpToConsume && CanUseCoyote) || HasBufferedJump)
            {
              audioManager.Play("jump");
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _jumpToConsume = false;
                _timeLeftGrounded = _fixedFrame;
                _executedBufferedJump = true;
                OnJumping?.Invoke();
            }

            // End the jump early if button released
            if (!_grounded && !Input.JumpHeld && !_endedJumpEarly && _velocity.y > 0) _endedJumpEarly = true;

            if (_hittingCeiling && _currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
        }

        #endregion

        #region Dash

        [Header("DASH")][SerializeField] private float _dashPower = 50;
        [SerializeField] private int _dashLength = 3;
        [SerializeField] private float _dashEndHorizontalMultiplier = 0.25f;
        private float _startedDashing;
        private bool _canDash;
        private Vector2 _dashVel;


        private bool _dashing;
        private bool _dashToConsume;

        void CalculateDash()
        {
            if (!_allowDash) return;
            if (_dashToConsume && _canDash)
            {
                _dashToConsume = false;
                var vel = new Vector2(Input.X, _grounded && Input.Y < 0 ? 0 : Input.Y);
                if (vel == Vector2.zero) return;
                _dashVel = vel * _dashPower;
                _dashing = true;
                OnDashingChanged?.Invoke(true);
                _canDash = false;
                _startedDashing = _fixedFrame;
            }

            if (_dashing)
            {
                _currentHorizontalSpeed = _dashVel.x;
                _currentVerticalSpeed = _dashVel.y;
                // Cancel when the time is out or we've reached our max safety distance
                if (_startedDashing + _dashLength < _fixedFrame)
                {
                    _dashing = false;
                    OnDashingChanged?.Invoke(false);
                    _currentVerticalSpeed = 0;
                    _currentHorizontalSpeed *= _dashEndHorizontalMultiplier;
                    if (_grounded) _canDash = true;
                }
            }
        }

        #endregion

        #region Move

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.fixedDeltaTime;

            _rb.MovePosition(_rb.position + (Vector2)move);

            RunCornerPrevention();
        }

        #region Corner Stuck Prevention

        private Vector2 _lastPos;
        private bool _cornerStuck;

        // This is a little hacky, but it's very difficult to fix.
        // This will allow walking and jumping while right on the corner of a ledge.
        void RunCornerPrevention()
        {
            // There's a fiddly thing where the rays will not detect ground (right inline with the collider),
            // but the collider won't fit. So we detect if we're meant to be moving but not.
            // The downside to this is if you stand still on a corner and jump straight up, it won't trigger the land
            // when you touch down. Sucks... but not sure how to go about it at this stage
            _cornerStuck = !_grounded && _lastPos == _rb.position && _lastJumpPressed + 1 < _fixedFrame;
            _currentVerticalSpeed = _cornerStuck ? 0 : _currentVerticalSpeed;
            _lastPos = _rb.position;
        }

        #endregion

        #endregion
    }
