using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1JumpState : Player1State
{
    public Player1JumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //¿ÉÐÞ¸Ä
        player.rb.AddForce(new Vector2(0,5f));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(player.isGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
