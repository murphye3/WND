using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class TrippinTrees : AbstractEntity
    {
        private Vector2 _position;
        public TrippinTrees(SpriteAnimation animation,  Vector2 position)
        {
            this.spriteManager = animation;
            animation.Position = position;
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            return false;
        }
    }
}