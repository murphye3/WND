using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Animation;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Entities
{
    class Oracle : AbstractEntity
    {
        private bool _isDead;

        public Oracle(SpriteAnimation animation, Vector2 position)
        {
            this.spriteManager = animation;
            this.body = new BasicBody(this, position, 1f);
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = body.Position;
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
    }
}
