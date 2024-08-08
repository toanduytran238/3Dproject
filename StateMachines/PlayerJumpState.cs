using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    float time = 0;
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        InitializeSubState();
        IsRootState = true;
    }
    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        Ctx.JumpCount = 0;
    }
    public override void EnterState()
    {

        HandleJump();
    }
    public override void ExitState()
    {

        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);

        //Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(jumpResetRoutine());
        if (Ctx.JumpCount == 2)
        {
            Ctx.JumpCount = 0;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }
    public override void CheckSwitchState()
    {

        if (Ctx._characterController.isGrounded)
        {

            SwitchState(Factory.Grounded());
        }
    }
    public override void UpdateState()
    {
        time += Time.deltaTime;
        if (Ctx.IsJumpPressed && time > 0.5f && Ctx.JumpCount < 2)
        {
            Debug.Log("Double Jump");
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
            Ctx.JumpCount += 1;

            Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];

            time = 0;
        }
            HandleGravity();
        CheckSwitchState();

    }
    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }
    void HandleJump()
    {
        
            if (Ctx.JumpCount < 2 && Ctx.CurrentJumpResetRoutine != null)
            {
                Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
            }

            Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);

            Ctx.IsJumping = true;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
            Ctx.JumpCount += 1;

            Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        
        

    }
    public void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMutiplier = 2.0f;

        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMutiplier * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
        }
    }
}
