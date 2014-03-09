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
using System.IO;

namespace WizardsNeverDie.Entities
{
    public class RangedPurplePlasma : AbstractEntity
    {
        private bool _isDead = false;
        private bool _isDeadOnEnemy = false;
        public RangedPurplePlasma(SpriteAnimation animation, Vector2 position, Vector2 force)
        {
            this.spriteManager = animation;
            animation.Position = position;
            animation.SetAnimationState(AnimationState.Attack);
            this.body = new PlasmaBody(this, position, force, .5f);
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            if (collidedWith is MeleeRedIfrit)
            {
                MeleeRedIfrit enemy = (MeleeRedIfrit)collidedWith;
                enemy.IsDead = false;
                this.IsDeadOnEnemy = false;
                return false;
            }
            if (collidedWith is RangedPurpleIfrit)
            {
                RangedPurpleIfrit enemy = (RangedPurpleIfrit)collidedWith;
                enemy.IsDead = false;
                this.IsDeadOnEnemy = false;
                return false;
            }
            if (collidedWith is Wizard)
            {
                Wizard player = (Wizard)collidedWith;

                if (player.Health == HealthAnimation.HealthState.Health100)
                    player.Health = HealthAnimation.HealthState.Health75;
                else if (player.Health == HealthAnimation.HealthState.Health75)
                    player.Health = HealthAnimation.HealthState.Health50;
                else if (player.Health == HealthAnimation.HealthState.Health50)
                    player.Health = HealthAnimation.HealthState.Health25;
                else if (player.Health == HealthAnimation.HealthState.Health25)
                    player.Health = HealthAnimation.HealthState.Health0;
                
                this._isDeadOnEnemy = true;
                return false;
            }
            if (collidedWith is Brick)
            {
                this._isDead = true;
            }
            if (collidedWith is Spawner)
            {
                this._isDead = false;
            }
            if (collidedWith is WizardPlasma)
            {
                this._isDead = true;
                
                return false;
            }
            if (collidedWith is RangedPurplePlasma)
            {
                this._isDead = false;

                return false;
            }
            this._isDead = true;
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
        public bool IsDeadOnEnemy
        {
            get
            {
                return _isDeadOnEnemy;
            }
            set
            {
                _isDeadOnEnemy = value;
            }
        }
    }
}