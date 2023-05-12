using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(CharacterController))]

public class PlayerMotion : MonoBehaviour
{

    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05f;
   
    private bool isRunning = false;

    // Animation
    private Animator _anim;

    // Movement
    private CharacterController _controller;
    private float _jumpForce = 14.0f;
    private float _gravity = 12.0f;
    private float _verticalVelocity;
    private int _desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

    // Speed Modifier
    private float _originalSpeed = 7.0f;
    private float _speed = 20.0f; //7
    private float _speedIncreaseLastTick;
    private float _speedIncreaseTime = 2.5f;
    private float _speedIncreaseAmount = 0.1f;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _speed = _originalSpeed;
    }


    private void Update()
    // Gather input on which lane unicorn should be
    {

        if (!isRunning)
            return;

        if (Time.time - _speedIncreaseLastTick > _speedIncreaseTime)
        {
            _speedIncreaseLastTick = Time.time;
            _speed += _speedIncreaseAmount;

            GameManagerScript.Instance.UpdateModifier(_speed - _originalSpeed);
        }


        if (MobileTouchInput.Instance.SwipeLeft)
            MoveLane(false);
        if (MobileTouchInput.Instance.SwipeRight)
            MoveLane(true);

        // Calculate where should unicorn be

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (_desiredLane == 0)

            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (_desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        // Calculate unicorn move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * _speed; ///.normalized.

        bool isGrounded = this.isGrounded();
        _anim.SetBool("Grounded", isGrounded);


        // Calculate Y
        if (this.isGrounded()) // if grounded // GOTTA DOUBLE CHECK THIS !!!!!!!!
        {
            _verticalVelocity = -0.1f;


            if (MobileTouchInput.Instance.SwipeUp)
            {
                // jump
                _verticalVelocity = _jumpForce;
                _anim.SetTrigger("Jump");
            }

        }
        else
        {
            _verticalVelocity -= (_gravity * Time.deltaTime);

            // Fast falling mechanic
            if (MobileTouchInput.Instance.SwipeDown)
            {
                _verticalVelocity = -_jumpForce;
            }
        }

        moveVector.y = _verticalVelocity;
        moveVector.z = _speed;
        moveVector.y = -0.1f;
        moveVector.z = _speed;

        // Move Unicorn
        _controller.Move(moveVector * Time.deltaTime);

        //Rotate the unicorn to where it is going
        Vector3 dir = _controller.velocity;

        if (dir != Vector3.zero)
        {
            dir.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }

    }


    private void MoveLane(bool goingRight)
    {

        _desiredLane += (goingRight) ? 1 : -1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);

    }

    private bool isGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                _controller.bounds.center.x,
                _controller.bounds.center.y - _controller.bounds.extents.y + 0.2f,
                _controller.bounds.center.z),
            Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1f);

        return Physics.Raycast(groundRay, 0.3f);
    }

    public void StartRunning()
    {
        isRunning = true;
        _anim.SetTrigger("StartRun");
    }

    private void _Crash()
    {
        _anim.SetTrigger("Death");
        isRunning = false;
        GameManagerScript.Instance.OnGameOver();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                _Crash();
                break;
        }
    }

}

