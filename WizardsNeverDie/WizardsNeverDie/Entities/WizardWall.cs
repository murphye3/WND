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
    class WizardWall : AbstractEntity
    {
        public WizardWall(Vector2 position, float width, float height)
        {
            this.body = new StaticBody(this, position, width, height);
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
        }
    }
}