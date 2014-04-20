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
    public class Wizard : AbstractCreature
    {
        private List<List<TriggerBody>> _allTriggers = new List<List<TriggerBody>>();
        private HealthAnimation.HealthState _healthState;
        private List<WizardPlasma> _plasma;
        public Wizard(SpriteAnimation animation, Vector2 position, List<WizardPlasma> plasma)
        {
            _plasma = plasma;
            this.spriteManager = animation;
            this.body = new BasicBody(this, position, 1f);
            this.intelligence = new PlayerIntelligence(this, .15f);
            _healthState = WizardsNeverDie.Animation.HealthAnimation.HealthState.Health100;
        }
        public void Update(GameTime gameTime)
        {
            for (int k = 0; k < _plasma.Count; k++)
            {
                this.getBody().Bodies[0].IgnoreCollisionWith(_plasma[k].getBody().Bodies[0]);

            }
            this.getBody().Bodies[0].ResetDynamics();
            intelligence.Update(gameTime);
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {

            if (collidedWith is Odin)
            {
                
                return false;
            }
            if (collidedWith is WizardWall)
            {
                return false;
            }
            if (collidedWith is Teleporter)
            {
                Teleporter teleporter = (Teleporter)collidedWith;
                teleporter.Collided = true;
            }
            if (collidedWith is RangedPurplePlasma)
            {
                return false;
            }

            return true;
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
    }
}