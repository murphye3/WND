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
    public class WizardPlasma : AbstractEntity
    {
        private bool _isDead = false;
        private bool _isDeadOnEnemy = false;
        public WizardPlasma(SpriteAnimation animation, Vector2 position, Vector2 force)
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
                enemy.IsDead = true;
                this.IsDeadOnEnemy = true;
                return false;
            }
            if (collidedWith is RangedPurpleIfrit)
            {
                RangedPurpleIfrit enemy = (RangedPurpleIfrit)collidedWith;
                enemy.IsDead = true;
                this.IsDeadOnEnemy = true;
                return false;
            }
            if (collidedWith is Oracle)
            {
                Oracle oracle = (Oracle)collidedWith;
                oracle.IsDead = false;
                this.IsDead = true;
                return false;
            }
            if (collidedWith is Wizard)
            {
                return false;
            }
            if (collidedWith is Brick)
            {
                this._isDead = true;
            }
            if (collidedWith is Spawner)
            {
                this.IsDeadOnEnemy = true;
                return false;
            }
            if (collidedWith is WizardPlasma)
            {
                return false;
            }
            if (collidedWith is RangedPurplePlasma)
            {
                this._isDead = true;
                return false;
            }
            if (collidedWith is Gotfraggon)
            {
                this._isDead = true;
                return false;
            }

            if (collidedWith is PlasmaWall)
            {
                this._isDead = false;
                return false;
            }
            if (collidedWith is Odin)
            {
                
                this._isDeadOnEnemy = true;
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