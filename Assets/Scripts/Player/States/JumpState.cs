using PSEMO.Audio;
using PSEMO.Core.StateMachine;
using UnityEngine;

namespace PSEMO.Player
{
    public class JumpState : PlayerBaseState
    {
        public JumpState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter(IState previousState)
        {
            //animator.Play(JumpAnimHash);
            
            //AudioManager.Instance.PlayAudio(AudioConstants.Jump);
        
            ctx.rb.linearVelocity = new Vector3(ctx.rb.linearVelocity.x, ctx.data.jumpForce, ctx.rb.linearVelocity.z);
            ctx.jumpBufferCounter = 0f;
            ctx.coyoteTimeCounter = 0f;
            ctx.jumpsLeft--;
            ctx.hasJumped = true;
        }

        public override void FixedUpdate()
        {
            ctx.Run();
        }
    }
}