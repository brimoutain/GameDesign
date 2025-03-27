using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1GroundState : Player1State
{
    public Player1GroundState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        //可以调整
        if(Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(this);
        }
    }
}
