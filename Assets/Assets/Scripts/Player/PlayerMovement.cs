using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player Player;

    private PlayerControlls playerControlls;
    private CharacterController CharacterController;
    private Animator animator;


    [Header("Movement Info")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float TrunSpeed;
    private float Speed;
    private float verticalVelocity;

    public Vector2 MoveInput {  get; private set; }
    private Vector3 MovementDir;

    private bool IsRunning = false;


    private void Start()
    {
        Player = GetComponent<Player>();

        CharacterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Speed = WalkSpeed;
        
        InitInputEvent();
    }

    private void Update()
    {
        if (CharacterController != null)
        {
            ApplyMovement();
            ApplyRot();
            AnimatorControllers();
        }
    }
    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(MovementDir.normalized, transform.right);
        float zVelocity = Vector3.Dot(MovementDir.normalized, transform.forward);
        animator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        bool bPlayerRunning = IsRunning && MovementDir.magnitude > 0;
        animator.SetBool("IsRunning", bPlayerRunning);
    }
    private void ApplyRot()
    {

        Vector3 LookingDir = Player.PlayerAim.GetMouseHitInfo().point - transform.position;
        LookingDir.y = 0.0f;
        LookingDir.Normalize();

        Quaternion desiredRot = Quaternion.LookRotation(LookingDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, TrunSpeed*Time.deltaTime);
    }

    private void ApplyMovement()
    {
        MovementDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        ApplyGravity();

        if (MovementDir.magnitude > 0)
        {
            CharacterController.Move(MovementDir * Time.deltaTime * Speed);
        }
    }

    private void ApplyGravity()
    {
        if (CharacterController.isGrounded == false)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
            MovementDir.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
        }
    }
    private void InitInputEvent()
    {
        playerControlls = Player.Controls;


        playerControlls.Character.Movement.performed += context => MoveInput = context.ReadValue<Vector2>();
        playerControlls.Character.Movement.canceled += context => MoveInput = Vector2.zero;

        playerControlls.Character.Run.performed += context =>
        {

            Speed = RunSpeed;
            IsRunning = true;

        };
        playerControlls.Character.Run.canceled += context =>
        {
            Speed = WalkSpeed;
            IsRunning = false;
        };
    }


}
