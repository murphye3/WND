using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.DebugViews;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Level;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Intelligence;
using WizardsNeverDie.Entities;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace WizardsNeverDie.Entities
{
    class Enemy: AbstractCreature
    {
        private bool _isDead = false;
        
        public Enemy(EnemyAnimation spriteManager, AbstractCreature target, Vector2 position, float width, float height, float targetDistance)
        {
            this.spriteManager = spriteManager;
            this.body = new BasicBody(this, position, 1f);

            this.intelligence = new CreatureIntelligence(this, target, .05f, targetDistance);
        }
        public void Update(GameTime gameTime)
        {
            intelligence.Update(gameTime);
            spriteManager.Position = body.Position;
            spriteManager.Update(gameTime);
        }
        public AbstractCreature Target
        {
            get
            {
                return ((CreatureIntelligence)intelligence).target;
            }
            set
            {
                ((CreatureIntelligence)intelligence).target = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {
            
            EnemyAnimation animation = (EnemyAnimation)this.SpriteManager;
            if (collidedWith is Player && animation.GetAnimationState() != AnimationState.Death)
            {
                Player player = (Player)collidedWith;
                animation.SetAnimationState(AnimationState.Attack);
                //if (animation.PreviousAnimationState = AnimationState.Walk && animation.GetAnimationState() == AnimationState.Walk)
                //{
                    if (player.Health == HealthAnimation.HealthState.Health100)
                        player.Health = HealthAnimation.HealthState.Health75;
                    else if (player.Health == HealthAnimation.HealthState.Health75)
                        player.Health = HealthAnimation.HealthState.Health50;
                    else if (player.Health == HealthAnimation.HealthState.Health50)
                        player.Health = HealthAnimation.HealthState.Health25;
                    else if (player.Health == HealthAnimation.HealthState.Health25)
                        player.Health = HealthAnimation.HealthState.Health0;
                //}
            }
            if(collidedWith is Plasma)
            {
                Plasma plasma = (Plasma)collidedWith;
                animation.SetAnimationState(AnimationState.Death);
                return false;
            }
            if (collidedWith is Enemy)
            {
                return true;
            }
            return true;     
        }
        
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }
    }
}
