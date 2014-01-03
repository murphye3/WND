using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Entities;

namespace WizardsNeverDie.Intelligence
{
    public abstract class AbstractIntelligence
    {
        public abstract void Update(GameTime gameTime);
       
    }
}
