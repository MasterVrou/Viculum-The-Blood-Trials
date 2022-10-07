using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_PlayerDetectedState : PlayerDetectedState
{
    private Archer archer;

    public Archer_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Archer archer) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.archer = archer;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(archer.lookForPlayerState);
        }
        else if (performCloseRangeAction)
        {
            if (Time.time >= archer.dodgeState.startTime + archer.dodgeStateData.dodgeCoolDown)
            {
                stateMachine.ChangeState(archer.dodgeState);
            }
            else if(Time.time > startTime + stateData.coolDown)
            {
                stateMachine.ChangeState(archer.rangedAttackState);
            }
        }
        else if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(archer.rangedAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
