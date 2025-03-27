using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1IdleState : Player1GroundState
{
    public Player1IdleState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {

        }
    }
}
