using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Entities;
namespace WizardsNeverDie.Intelligence
{
    public class GotfraggonIntelligence : AbstractIntelligence
    {
        GotfraggonAnimation enemy = new GotfraggonAnimation();
        public AbstractCreature creature;
        public AbstractCreature target;
        float speed;
        protected Orientation lastOrientation;
        protected TimeSpan swapTimer;
        protected TimeSpan swapCooldown = TimeSpan.FromSeconds(6d);
        protected float _targetDistance;
        public GotfraggonIntelligence(AbstractCreature creature, AbstractCreature target, float speed, float targetDistance)
        {
            this.creature = creature;
            this.target = target;
            this.speed = speed;
            this._targetDistance = targetDistance;
        }

        public bool canSwap(Orientation current)
        {
            bool canSwap = true;
            if (swapTimer < swapCooldown)
            {
                switch (current)
                {
                    case Orientation.Down: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.DownRight) canSwap = false; break;
                    case Orientation.Up: if (lastOrientation == Orientation.UpLeft || lastOrientation == Orientation.UpRight) canSwap = false; break;
                    case Orientation.Left: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.UpLeft) canSwap = false; break;
                    case Orientation.Right: if (lastOrientation == Orientation.DownRight || lastOrientation == Orientation.UpRight) canSwap = false; break;
                    case Orientation.DownLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Down) canSwap = false; break;
                    case Orientation.DownRight: if (lastOrientation == Orientation.Down || lastOrientation == Orientation.Right) canSwap = false; break;
                    case Orientation.UpLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Up) canSwap = false; break;
                    case Orientation.UpRight: if (lastOrientation == Orientation.Up || lastOrientation == Orientation.Right) canSwap = false; break;
                    default: break;
                }
            }
            return canSwap;
        }

        public void swapWalkOrientation(Orientation current)
        {
            SpriteAnimation animation = (SpriteAnimation)creature.SpriteManager;
            if (current == Orientation.None)
                animation.SetAnimationState(AnimationState.Stop);
            else if (canSwap(animation.GetOrientation()))
            {
                animation.SetAnimationState(AnimationState.Walk);
                animation.SetOrientation(current);
                lastOrientation = current;
                swapTimer = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (target == null)
                return;
            bool canMove = true;
            GotfraggonAnimation animation = (GotfraggonAnimation)this.creature.SpriteManager;
            if (animation.GetAnimationState() == AnimationState.Attack || animation.GetAnimationState() == AnimationState.Death)
                canMove = false;
            float targetDistance = (target.Position.Y+5 - creature.Position.Y) / 2;
            Vector2 direction = Vector2.Subtract(target.Position, creature.Position);
            direction.Normalize();
            double angle = -Math.Atan2(direction.Y, direction.X);
            SpriteAnimation sa = (SpriteAnimation)creature.SpriteManager;
            GotfraggonAnimation e = (GotfraggonAnimation)creature.SpriteManager;
            PhysicsBody body = creature.getBody();
            if (canMove == true && Math.Abs(targetDistance) > .5 )
            {
                if (target.Position.Y + 5 <= creature.Position.Y &&
                    target.Position.X < creature.Position.X) // Up Left
                {
                    swapWalkOrientation(Orientation.Left);
                    if (canMove)
                        body.Move(new Vector2(0, -speed));
                }
                else if (target.Position.Y + 5 >= creature.Position.Y &&
                    target.Position.X < creature.Position.X) // Down Left
                {
                    swapWalkOrientation(Orientation.Left);
                    if (canMove)
                        body.Move(new Vector2(0, speed));
                }
                else if (target.Position.Y + 5 <= creature.Position.Y &&
                    target.Position.X > creature.Position.X) // Up Right
                {
                    swapWalkOrientation(Orientation.Right);
                    if (canMove)
                        body.Move(new Vector2(0, -speed));
                }
                else if (target.Position.Y + 5 >= creature.Position.Y &&
                    target.Position.X > creature.Position.X) // Down Right
                {
                    swapWalkOrientation(Orientation.Right);
                    if (canMove)
                        body.Move(new Vector2(0, speed));
                }
                
            }
            else
            {
                e.SetAnimationState(AnimationState.PurpleSpell);
                if (canMove)
                    body.Move(new Vector2(0, 0));
            }
        }
    }
}
