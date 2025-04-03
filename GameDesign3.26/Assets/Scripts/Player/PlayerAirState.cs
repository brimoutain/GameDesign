using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(player.rb.velocity.ToString());
    }

    public override void Update()
    {
        base.Update();
        if (player.isGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
