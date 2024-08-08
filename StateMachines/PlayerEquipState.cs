using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipState : PlayerBaseState, IRootState
{
    
    public PlayerEquipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();

    }
    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
    public override void EnterState()
    {
        Ctx.IsEquiping = true;
        Ctx.Animator.SetTrigger(Ctx.IsEquipingHash);
        HandleGravity();

    }
    public override void ExitState() 
    {
        
        
    }
    public override void CheckSwitchState()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            Debug.Log("Switch GroundState");
            SwitchState(Factory.Grounded());
        }
    }
    public override void UpdateState()
    {


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
}
