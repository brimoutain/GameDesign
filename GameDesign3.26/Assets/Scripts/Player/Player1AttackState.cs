using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1AttackState : Player1State
{
    public Player1AttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isTriggerCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(player.isTriggerCalled)
        {

        }
    }
}
