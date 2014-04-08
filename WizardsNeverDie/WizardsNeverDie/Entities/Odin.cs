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
    public class Odin : AbstractCreature
    {
        private HealthAnimation.HealthState _healthState;
        private bool _isDead = false;
        float _targetDistance;
        OdinIntelligence o;
        public Odin(OdinAnimation spriteManager, AbstractCreature target, Vector2 position, float width, float height, float targetDistance)
        {
            _targetDistance = targetDistance;
            _healthState = WizardsNeverDie.Animation.HealthAnimation.HealthState.Health100;
            this.spriteManager = spriteManager;
            this.body = new BasicBody(this, position, 1f);
            o = new OdinIntelligence(this, target, .25f, targetDistance);
            this.intelligence = o;
            
        }
        public void Update(GameTime gameTime)
        {
            this.getBody().Bodies[0].ResetDynamics();
            spriteManager.Position = body.Position;
            spriteManager.Update(gameTime);
            o.TargetDistance = this.TargetDistance;
            o.Update(gameTime);
        }
        public AbstractCreature Target
        {
            get
            {
                return ((OdinIntelligence)intelligence).target;
            }
            set
            {
                ((OdinIntelligence)intelligence).target = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {
            
            
            OdinAnimation animation = (OdinAnimation)this.SpriteManager;
            if (collidedWith is Wizard && animation.GetAnimationState() != AnimationState.Death && animation.GetAnimationState() != AnimationState.OdinDeath)
            {
                Wizard player = (Wizard)collidedWith;
                WizardAnimation a = (WizardAnimation)player.SpriteManager;
                player = (Wizard)collidedWith;
                if (a.GetAnimationState() != AnimationState.Revived)
                {
                    animation.SetAnimationState(AnimationState.Attack);
                }
                //if (animation.PreviousAnimationState = AnimationState.Walk && animation.GetAnimationState() == AnimationState.Walk)
                    if (player.Health == HealthAnimation.HealthState.Health100)
                        player.Health = HealthAnimation.HealthState.Health50;
                    if (player.Health == HealthAnimation.HealthState.Health75)
                        player.Health = HealthAnimation.HealthState.Health25;
                    else if (player.Health == HealthAnimation.HealthState.Health50)
                        player.Health = HealthAnimation.HealthState.Health0;
                    else if (player.Health == HealthAnimation.HealthState.Health25)
                        player.Health = HealthAnimation.HealthState.Health0;
                //}
            }
            if(collidedWith is WizardPlasma)
            {
                OdinAnimation odinAnimation = (OdinAnimation)this.SpriteManager;

                if (this.Health == HealthAnimation.HealthState.Health100 && (odinAnimation.GetAnimationState() == AnimationState.Walk || odinAnimation.GetAnimationState() == AnimationState.Stop ||
                    odinAnimation.GetAnimationState() == AnimationState.Attack))
                {
                    this.Health = HealthAnimation.HealthState.Health80;
                }
                else if (this.Health == HealthAnimation.HealthState.Health80 && (odinAnimation.GetAnimationState() == AnimationState.Walk || odinAnimation.GetAnimationState() == AnimationState.Stop ||
                    odinAnimation.GetAnimationState() == AnimationState.Attack))
                {
                    this.Health = HealthAnimation.HealthState.Health60;
                }
                else if (this.Health == HealthAnimation.HealthState.Health60 && (odinAnimation.GetAnimationState() == AnimationState.Walk || odinAnimation.GetAnimationState() == AnimationState.Stop ||
                    odinAnimation.GetAnimationState() == AnimationState.Attack))
                {
                    this.Health = HealthAnimation.HealthState.Health40;
                }
                else if (this.Health == HealthAnimation.HealthState.Health40 && (odinAnimation.GetAnimationState() == AnimationState.Walk || odinAnimation.GetAnimationState() == AnimationState.Stop ||
                    odinAnimation.GetAnimationState() == AnimationState.Attack))
                {
                    this.Health = HealthAnimation.HealthState.Health20;
                }
                else if (this.Health == HealthAnimation.HealthState.Health20 && (odinAnimation.GetAnimationState() == AnimationState.Walk || odinAnimation.GetAnimationState() == AnimationState.Stop ||
                    odinAnimation.GetAnimationState() == AnimationState.Attack))
                {
                    this.Health = HealthAnimation.HealthState.Health0;
                    odinAnimation.SetAnimationState(AnimationState.OdinDeath);
                }
                WizardPlasma plasma = (WizardPlasma)collidedWith;
                if (animation.GetAnimationState() != AnimationState.Revived && o.AttackState == true && animation.GetAnimationState() != AnimationState.OdinDeath)
                {
                    animation.SetAnimationState(AnimationState.Death);
                }
                return false;
            }
            if (collidedWith is MeleeRedIfrit)
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

        public WizardsNeverDie.Animation.HealthAnimation.HealthState Health
        {
            get
            {
                return _healthState;
            }
            set
            {
                _healthState = value;
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
