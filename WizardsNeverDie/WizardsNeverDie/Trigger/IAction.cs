using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Trigger
{
    public interface IAction
    {
        void Draw();
        void Update(GameTime gameTime);
        bool IsDead { get; set; }
    }
}
