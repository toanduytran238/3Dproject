using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
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
        
        HandleGravity();

    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.IsJumpPressed )
        {
            Debug.Log("Switch jumpState");
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
        else if (Ctx.IsAttackPressed&&!Ctx.IsAttacking&&Ctx.IsActiveSword)
        {
            
            SwitchState(Factory.Attack());
        }
        else if (Ctx.IsAttackPressed && !Ctx.IsActiveSword&&!Ctx.IsThrowing)
        {

            SwitchState(Factory.Throw());
        }
        else if (Ctx.IsEquipPressed&&!Ctx.IsEquiping)
        {
            SwitchState(Factory.Equip());
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