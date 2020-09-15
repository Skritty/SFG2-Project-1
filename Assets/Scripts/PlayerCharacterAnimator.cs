using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string SprintState = "Sprint";
    const string LandState = "Land";

    [SerializeField]
    PlayerController _controller = null;

    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    public void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    public void OnStartFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .6f);
    }

    public void OnStartJump()
    {
        _animator.CrossFadeInFixedTime(JumpState, .1f);
    }

    public void OnLand()
    {
        _animator.CrossFadeInFixedTime(LandState, .1f);
    }

    public void OnStartSprint()
    {
        _animator.CrossFadeInFixedTime(SprintState, .3f);
    }

    private void OnEnable()
    {
        _controller.Idle += OnIdle;
        _controller.StartRunning += OnStartRunning;
        _controller.Fall += OnStartFalling;
        _controller.Jump += OnStartJump;
        _controller.Land += OnLand;
        _controller.Sprint += OnStartSprint;
    }

    private void OnDisable()
    {
        _controller.Idle -= OnIdle;
        _controller.StartRunning -= OnStartRunning;
        _controller.Fall -= OnStartFalling;
        _controller.Jump -= OnStartJump;
        _controller.Land -= OnLand;
        _controller.Sprint -= OnStartSprint;
    }
}
