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
    public class Spawner : AbstractCreature 
    {
        private bool _isDead;
        private bool _activated = false;
        public Spawner(SpriteAnimation animation, Vector2 position, AbstractCreature target, float width, float height, bool activated)
        {
            String widthString = animation.AnimationName;
            this.spriteManager = animation;
            animation.Position = position;
            _activated = activated;
            this.body = new StaticBody(this, position, width, height);
            this.intelligence = new SpawnerIntelligence(this, target);
        }
        public void Update(GameTime gameTime)
        {
            intelligence.Update(gameTime);
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
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
        public bool IsActivated
        {
            get
            {
                return _activated;
            }
            set
            {
                _activated = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {
            if (collidedWith is Plasma)
            {
                this._isDead = true;
            }
            return false;
        }
    }
}
