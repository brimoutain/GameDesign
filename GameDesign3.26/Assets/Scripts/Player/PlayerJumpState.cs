using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //¿ÉÐÞ¸Ä
        player.rb.velocity=new Vector2(player.rb.velocity.x,15f);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log(player.rb.velocity);
    }

    public override void Update()
    {
        base.Update();
        if(player.rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
