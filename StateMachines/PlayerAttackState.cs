using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState,IRootState
{
    int comboAttack = 0;
    float time=0;
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();

    }
    IEnumerator attackResetRoutine()
    {
        yield return new WaitForSeconds(1f);
        Ctx.AttackCount = 0;
        Ctx.Animator.SetInteger(Ctx.AttackCountHash, Ctx.AttackCount);
    }
    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
    public override void EnterState()
    {
        Ctx.IsAttacking = true;
        Ctx.Sword = Ctx.GetComponentInChildren<Sword>();
        //Ctx.Sword._canDamage = true;
        handleAttack();
        HandleGravity();
        
    }
    public override void ExitState() 
    {
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, false);
        Ctx.CurrentAttackResetRoutine = Ctx.StartCoroutine(attackResetRoutine());
        if (Ctx.AttackCount >= 3)
        {
            Ctx.AttackCount = 0;
            Ctx.Animator.SetInteger(Ctx.AttackCountHash, Ctx.AttackCount);
        }
    }
    public override void CheckSwitchState()
    {
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.IsAttacking)
        {
            
            SwitchState(Factory.Grounded());
        }
    }
    public override void UpdateState()
    {
        time += Time.deltaTime;
        if (Ctx.IsAttackPressed && time > 0.8f&&Ctx.AttackCount<4)
        {
            Ctx.AttackCount ++;
            Ctx.Sword._canDamage = true;
        
            Ctx.Animator.SetInteger(Ctx.AttackCountHash, Ctx.AttackCount);
            
            time = 0;
        }
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
    void handleAttack()
    {
        if (Ctx.AttackCount < 4 && Ctx.CurrentAttackResetRoutine != null)
        {
            
            Ctx.StopCoroutine(Ctx.CurrentAttackResetRoutine);
        }
        Ctx.Sword._canDamage = true;
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, true);
        Ctx.AttackCount += 1;
        comboAttack++;
        Ctx.Animator.SetInteger(Ctx.AttackCountHash, Ctx.AttackCount);
        
    }
}
