﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Animation;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Physics;

namespace WizardsNeverDie.Entities
{
    class CastleWall : AbstractEntity
    {
        public CastleWall(SpriteAnimation animation, Vector2 position, float width, float height)
        {
            this.spriteManager = animation;
            animation.Position = position;
            this.body = new StaticBody(this, position, width, height); 
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            return false;
        }
    }
}
