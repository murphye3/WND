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
        private bool _plasmaBounce = false;
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
            if (collidedWith is Teleporter)
            {
                Teleporter teleporter = (Teleporter)collidedWith;
                teleporter.PlasmaCollided = true;
                this.IsDead = false;
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
                return true;
            }
            if (collidedWith is Gotfraggon)
            {
                this._isDead = true;
                return false;
            }
            if (collidedWith is Key)
            {
                Key key = (Key)collidedWith;
                if (_plasmaBounce == true)
                {
                    key.CollideWithPlasma = true;
                }
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
            if (collidedWith is BouncingWall)
            {
                _plasmaBounce = true;
                this.IsDead = false;
                return true;
            }
            if (collidedWith is Switcher)
            {
                this.IsDeadOnEnemy = true;
                Switcher switcher = (Switcher)collidedWith;
                if (switcher.IsOn == false)
                {
                    switcher.IsOn = true;
                }
                else if (switcher.IsOn == true)
                {
                    switcher.IsOn = false;
                }
                return false;
            }
            this.IsDead = true;
            return true;
        }
        public bool PlasmaBounce
        {
            get
            {
                return _plasmaBounce;
            }
            set
            {
                _plasmaBounce = value;
            }
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