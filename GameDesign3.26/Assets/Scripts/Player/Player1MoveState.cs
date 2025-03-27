using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1MoveState : Player1GroundState
{
    public Player1MoveState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput);
        if(xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
