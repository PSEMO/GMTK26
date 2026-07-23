using PSEMO.Core.StateMachine;
using UnityEngine;

namespace PSEMO.Player
{
    public class DashState : PlayerBaseState
    {
        private float dashTimeLeft;
        private bool originalGravity;
        private bool isDashing;

        public DashState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter(IState previousState)
        {
            //animator.Play(DashAnimHash);
        
            ctx.canDash = false;
            ctx.DashInput = false;
        
            dashTimeLeft = ctx.data.dashDuration;
            originalGravity = ctx.rb.useGravity;
            ctx.rb.useGravity = false;

            isDashing = true;

            ctx.rb.linearVelocity = ctx.facingDirection * ctx.data.dashForce;
        }

        public override void Update()
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft < 0f)
            {
                isDashing = false;
            }
        }

        public override void FixedUpdate()
        {
            if (ctx.IsFacingWall())
            {
                isDashing = false;
                return;
            }

            if (isDashing)
            {
                ctx.rb.linearVelocity = ctx.facingDirection * ctx.data.dashForce;
            }
        }

        public override void OnExit(IState nextState)
        {
            ctx.rb.useGravity = originalGravity;
            ctx.rb.linearVelocity = Vector3.zero; 
        
            isDashing = false;
        }

        public bool IsDashing() => isDashing;
    }
}