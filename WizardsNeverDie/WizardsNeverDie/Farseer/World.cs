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
using FarseerPhysics;
using FarseerPhysics.DebugViews;

namespace WizardsNeverDie.Physics
{
    public sealed class Farseer
    {
        private static readonly Farseer instance = new Farseer();
        public readonly World World;
        public readonly DebugViewXNA DebugView;
        private static Vector2 gravity = new Vector2(0, 20);
        private Farseer()
        {
            World = new World(gravity);
            DebugView = new DebugViewXNA(World);
        }
        public void Update(GameTime gameTime)
        {
            World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .001));
        }
        public static Farseer Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
