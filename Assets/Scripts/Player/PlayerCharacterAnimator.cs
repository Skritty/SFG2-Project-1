using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    #region State Strings
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string SprintState = "Sprint";
    const string LandState = "Land";
    const string DieState = "Die";
    const string HurtState = "Hurt";
    #endregion

    [SerializeField]
    PlayerController _controller = null;

    [System.Serializable]
    public class Particles
    {
        public GameObject particleSystem;
        public Transform location;
    }
    [System.Serializable]
    public class Sound
    {
        public AudioClip sound;
        public Transform location;
    }

    [Header("Particles")]
    [SerializeField]
    Particles landingParticles;

    [Header("Sounds")]
    Sound landingSound;
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
        Feedback(landingParticles);
    }

    public void OnStartSprint()
    {
        _animator.CrossFadeInFixedTime(SprintState, .3f);
    }

    public void OnHurt()
    {
        _animator.CrossFadeInFixedTime(HurtState, .1f);
    }

    public void OnDie()
    {
        _animator.CrossFadeInFixedTime(DieState, .4f);
    }

    private void OnEnable()
    {
        _controller.Idle += OnIdle;
        _controller.StartRunning += OnStartRunning;
        _controller.Fall += OnStartFalling;
        _controller.Jump += OnStartJump;
        _controller.Land += OnLand;
        _controller.Sprint += OnStartSprint;
        _controller.Die += OnDie;
        _controller.Hurt += OnHurt;
    }

    private void OnDisable()
    {
        _controller.Idle -= OnIdle;
        _controller.StartRunning -= OnStartRunning;
        _controller.Fall -= OnStartFalling;
        _controller.Jump -= OnStartJump;
        _controller.Land -= OnLand;
        _controller.Sprint -= OnStartSprint;
        _controller.Die -= OnDie;
        _controller.Hurt -= OnHurt;
    }

    private void Feedback(Particles particles)
    {
        if (particles == null) return;
        if (particles.particleSystem == null || particles.particleSystem.GetComponent<ParticleSystem>() == null || particles.location == null) return;
        Destroy(Instantiate(particles.particleSystem, particles.location.position, particles.location.rotation), particles.particleSystem.GetComponent<ParticleSystem>().main.duration);
    }
    private void Feedback(Sound sound)
    {
        if (sound == null) return;
        if (sound.sound == null || sound.location == null) return;
    }
    private void Feedback(Particles particles, Sound sound)
    {
        if (particles != null && particles.particleSystem != null && particles.particleSystem.GetComponent<ParticleSystem>() != null)
        {
            Destroy(Instantiate(particles.particleSystem, particles.location.position, particles.location.rotation), particles.particleSystem.GetComponent<ParticleSystem>().main.duration);
        }
        if(sound != null && sound.sound != null && sound.location != null)
        {

        }
        
    }
}
