using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockBehaviour : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField]
    [Tooltip("Key to move the block to the left")]
    private KeyCode _moveLeftKey;

    [SerializeField]
    [Tooltip("Key to move the block to the right")]
    private KeyCode _moveRightKey;

    [SerializeField]
    [Tooltip("Key to rotate the block to the left")]
    private KeyCode _rotateLeftKey;

    [SerializeField]
    [Tooltip("Key to rotate the block to the right")]
    private KeyCode _rotateRightKey;

    [SerializeField]
    [Tooltip("Key to increase the speed at which the block falls")]
    private KeyCode _speedUpKey;

    [Header("Movement Attributes")]
    [SerializeField]
    [Tooltip("Meters covered by the block when it shifts to the right or left")]
    [Range(0.1f, 2f)]
    private float _lateralShift = 0.5f;

    [SerializeField]
    [Tooltip("Speed at which the block falls to the ground")]
    [Range(0.1f, 1f)]
    private float _fallingSpeed = 0.1f;

    public float ScoreSpeedModifier { get; set; }

    [SerializeField]
    [Tooltip("Multiplier value by which block's falling speed will be increased pressing the speed up key")]
    [Range(1.5f, 5f)]
    private float _speedUpModifier = 3f;

    private Rigidbody2D _rigidbody;

    [HideInInspector]
    public BlockManager Manager;

    private bool _outOfControl;

    [Header("Effects")]
    [SerializeField]
    private AudioClip _collisionSoundFX;

    public AudioManager AudioManager { get; set; }

    private ParticleSystem _collisionParticleFX;

    public BlockAttributes Attributes { get; private set; }

    private void Awake()
    {
        SetUpKeys();
        SetUpComponents();
        ScoreSpeedModifier = 1;
    }

    private void SetUpKeys()
    {
        _moveLeftKey = KeyCode.LeftArrow;
        _moveRightKey = KeyCode.RightArrow;
        _rotateLeftKey = KeyCode.LeftControl;
        _rotateRightKey = KeyCode.LeftAlt;
        _speedUpKey = KeyCode.DownArrow;
    }

    private void SetUpComponents()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collisionParticleFX = GetComponentInChildren<ParticleSystem>();
        Attributes = GetComponent<BlockAttributes>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKeyDown(_moveLeftKey))
            transform.Translate(-_lateralShift, 0, 0, Space.World);
        if (Input.GetKeyDown(_moveRightKey))
            transform.Translate(_lateralShift, 0, 0, Space.World);
        if (Input.GetKeyDown(_rotateRightKey))
            transform.Rotate(0, 0, -90, Space.Self);
        if (Input.GetKeyDown(_rotateLeftKey))
            transform.Rotate(0, 0, 90, Space.Self);
    }

    private void FixedUpdate()
    {
        KeepFalling();
    }

    private void KeepFalling()
    {
        float actualSpeed = _fallingSpeed * ScoreSpeedModifier;
        if (Input.GetKey(_speedUpKey))
            actualSpeed *= _speedUpModifier;
        transform.Translate(0, -actualSpeed, 0, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayEffects();
        if (_outOfControl)
            return;

        SetToIdle();
        Manager.HandleNext(transform);
        PlayerManager.Singleton.CurrentPoints += Attributes.Points;
    }

    private void SetToIdle()
    {
        enabled = false;
        _outOfControl = true;
        _rigidbody.gravityScale = 1f;
    }

    private void PlayEffects()
    {
        _collisionParticleFX.Play();
        AudioManager.PlaySound(_collisionSoundFX, Attributes.SoundPitch);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Sink();
        if (!_outOfControl) //in case the player drives the block directly in the water
            Manager.HandleNext();
    }

    private void Sink()
    {
        Destroy(gameObject);
        PlayerManager.Singleton.CurrentLives--;
    }
}
