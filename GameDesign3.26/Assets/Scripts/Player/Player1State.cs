using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player1State : PlayerState
{
    public Player1State(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        if (UnityEngine.Input.GetKeyDown("A"))
        {
            xInput = 1;
        }
        else if (UnityEngine.Input.GetKeyDown("D"))
        {
            xInput = -1;
        }
        else xInput = 0;
    }
}
