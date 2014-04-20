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
using System.Diagnostics;

namespace WizardsNeverDie.Intelligence
{
    public class OdinIntelligence : AbstractIntelligence
    {
        public static Random random;
        bool attack = true;
        OdinAnimation enemy = new OdinAnimation();
        public AbstractCreature creature;
        public AbstractCreature target;
        float speed;
        int xRand = 0;
        int yRand = 0;
        float speedX = 0;
        float speedY = 0;
        TimeSpan runTimeStart;
        TimeSpan attackTimeStart;
        Stopwatch attackTimer = new Stopwatch();
        Stopwatch runTimer = new Stopwatch();
        protected Orientation lastOrientation;
        protected TimeSpan swapTimer;
        protected TimeSpan swapCooldown = TimeSpan.FromMilliseconds(50);
        protected float _targetDistance;
        Stopwatch swapWatch = new Stopwatch();
        public OdinIntelligence(AbstractCreature creature, AbstractCreature target, float speed, float targetDistance)
        {
            random = new Random();
            this.creature = creature;
            this.target = target;
            this.speed = speed;
            this._targetDistance = targetDistance;
        }

        public bool canSwap(Orientation current)
        {
            swapTimer = swapWatch.Elapsed;
            bool canSwap = true;
            if (swapTimer.Milliseconds < swapCooldown.Milliseconds)
            {
                
                //switch (current)
                //{
                //    //case Orientation.Down: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.DownRight) canSwap = false; break;
                //    //case Orientation.Up: if (lastOrientation == Orientation.UpLeft || lastOrientation == Orientation.UpRight) canSwap = false; break;
                //    //case Orientation.Left: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.UpLeft) canSwap = false; break;
                //    //case Orientation.Right: if (lastOrientation == Orientation.DownRight || lastOrientation == Orientation.UpRight) canSwap = false; break;
                //    //case Orientation.DownLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Down) canSwap = false; break;
                //    //case Orientation.DownRight: if (lastOrientation == Orientation.Down || lastOrientation == Orientation.Right) canSwap = false; break;
                //    //case Orientation.UpLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Up) canSwap = false; break;
                //    //case Orientation.UpRight: if (lastOrientation == Orientation.Up || lastOrientation == Orientation.Right) canSwap = false; break;
                //    //default: break;
                        
                //}
                canSwap = false;
            }else{
                swapWatch.Reset();
                swapWatch.Stop();
            }
            return canSwap;
        }

        public void swapWalkOrientation(Orientation current)
        {
            SpriteAnimation animation = (SpriteAnimation)creature.SpriteManager;
            if (current == Orientation.None)
                animation.SetAnimationState(AnimationState.Stop);
            else if(canSwap(animation.GetOrientation()))
            {
                animation.SetAnimationState(AnimationState.Walk);
                animation.SetOrientation(current);
                
                lastOrientation = current;
                swapTimer = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
            swapWatch.Start();
            OdinAnimation e = (OdinAnimation)creature.SpriteManager;
            PhysicsBody body = creature.getBody();
            if (e.GetAnimationState() == AnimationState.Death)
            {
                attack = false;
                if(!attackTimer.IsRunning)
                {
                    attackTimer.Start();
                    attackTimeStart = attackTimer.Elapsed;
                }           
            }
            TimeSpan attackTimeElapsed = attackTimer.Elapsed;
            TimeSpan runTimeElapsed = attackTimer.Elapsed;
            int totalAttackTime = attackTimeElapsed.Seconds - attackTimeStart.Seconds;
            int totalRunTime = runTimeElapsed.Seconds - runTimeStart.Seconds;
            if (totalAttackTime > 4)
            {
                attack = true;
                attackTimer.Stop();
                attackTimer.Reset();
                
            }
            if (e.GetAnimationState() == AnimationState.Attack)
            {
                attackTimer.Stop();
                attackTimer.Reset();
                attack = false;
                attackTimer.Start();
            }
            if (attack == false && e.GetAnimationState() != AnimationState.Death && e.GetAnimationState() != AnimationState.Revived && e.GetAnimationState() != AnimationState.OdinDeath && runTimer.ElapsedMilliseconds < 200)
            {
                
                if (!runTimer.IsRunning)
                {
                    
                    while (speedX == 0 && speedY == 0)
                    {
                        
                        xRand = random.Next(-100, 100);
                        yRand = random.Next(-100, 100);

                        speedX = speed * xRand * 3 / 100;
                        speedY = speed * yRand * 3 / 100;
                    }
                    runTimer.Start();
                    runTimeStart = runTimer.Elapsed;
                }
                
                if (speedX > 0 && speedY > 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.DownRight);
                }
                else if (speedX > 0 && speedY == 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.Right);
                }
                else if (speedX > 0 && speedY < 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.UpRight);
                }
                else if (speedX == 0 && speedY > 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.Down);
                }
                else if (speedX == 0 && speedY == 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    e.SetAnimationState(AnimationState.Stop);
                }
                else if (speedX == 0 && speedY < 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.Up);
                }
                else if (speedX < 0 && speedY > 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.DownLeft);
                }
                else if (speedX < 0 && speedY == 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.Left);
                }
                else if (speedX < 0 && speedY < 0)
                {
                    e.SetAnimationState(AnimationState.Walk);
                    swapWalkOrientation(Orientation.UpLeft);
                }
                else
                {
                    
                }
                
                body.Move(new Vector2(speedX, speedY));
            }
            else
            {
                runTimeStart = TimeSpan.Zero;
                runTimer.Stop();
                runTimer.Reset();
                xRand = 0;
                yRand = 0;
                speedX = 0;
                speedY = 0;
            }
            if (target == null)
                return;
            bool canMove = true;
            OdinAnimation animation = (OdinAnimation)this.creature.SpriteManager;
            if (animation.GetAnimationState() == AnimationState.Attack || animation.GetAnimationState() == AnimationState.Death || animation.GetAnimationState() == AnimationState.OdinDeath)
                canMove = false;
            float targetDistance = (float)Math.Sqrt(Math.Pow((creature.Position.X - target.Position.X), 2) + Math.Pow((creature.Position.Y - target.Position.Y), 2));
            Vector2 direction = Vector2.Subtract(target.Position, creature.Position);
            direction.Normalize();
            double angle = - Math.Atan2(direction.Y, direction.X);
            SpriteAnimation sa = (SpriteAnimation) creature.SpriteManager;
            
            
            if (canMove == true && targetDistance < _targetDistance && attack == true)
            {
                if (angle > -Math.PI / 8 && angle <= Math.PI / 8) // Right
                {
                    swapWalkOrientation(Orientation.Right);
                    if (canMove)
                        body.Move(new Vector2(speed, 0));
                }
                else if (angle > Math.PI / 8 && angle <= 3 * Math.PI / 8) // Up Right
                {
                    swapWalkOrientation(Orientation.UpRight);
                    if (canMove)
                        body.Move(new Vector2(speed, -speed));
                }
                else if (angle > 3 * Math.PI / 8 && angle <= 5 * Math.PI / 8) // UP
                {
                    swapWalkOrientation(Orientation.Up);
                    if (canMove)
                        body.Move(new Vector2(0, -speed));
                }
                else if (angle > 5 * Math.PI / 8 && angle <= 7 * Math.PI / 8) // Up Left
                {
                    swapWalkOrientation(Orientation.UpLeft);
                    if (canMove)
                        body.Move(new Vector2(-speed, -speed));
                }
                else if (angle > 7 * Math.PI / 8 || angle <= -7 * Math.PI / 8) // Left
                {
                    swapWalkOrientation(Orientation.Left);
                    if (canMove)
                        body.Move(new Vector2(-speed, 0));
                }
                else if (angle > -7 * Math.PI / 8 && angle <= -5 * Math.PI / 8) // Down Left
                {
                    swapWalkOrientation(Orientation.DownLeft);
                    if (canMove)
                        body.Move(new Vector2(-speed, speed));
                }
                else if (angle > -5 * Math.PI / 8 && angle <= -3 * Math.PI / 8) // Down
                {
                    swapWalkOrientation(Orientation.Down);
                    if (canMove)
                        body.Move(new Vector2(0, speed));
                }
                else if (angle > -3 * Math.PI / 8 && angle <= -Math.PI / 8) // Down Right
                {
                    swapWalkOrientation(Orientation.DownRight);
                    if (canMove)
                        body.Move(new Vector2(speed, speed));
                }
            }
            else if (targetDistance > _targetDistance && attack == true)
            {
                e.SetAnimationState(AnimationState.Stop);
            }
        }


        public bool AttackState
        {
            get
            {
                return this.attack;
            }
            set
            {
                this.attack = value;
            }
        }
        public float TargetDistance
        {
            get
            {
                return _targetDistance;
            }
            set
            {
                _targetDistance = value;
            }
        }
    }
}
