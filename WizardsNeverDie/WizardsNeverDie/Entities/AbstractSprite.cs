using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Utility;
using WizardsNeverDie.Animation;

namespace WizardsNeverDie.Entities
{
    public abstract class AbstractSprite
    {
        protected SpriteManager spriteManager;
        
        public virtual SpriteManager SpriteManager
        {
            get { return spriteManager; }
        }
        public abstract Vector2 DisplayPosition();

        public virtual void Initialize()
        {}
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.spriteManager.Draw(spriteBatch);
        }
    }
}

