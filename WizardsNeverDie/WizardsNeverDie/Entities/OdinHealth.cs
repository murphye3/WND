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
    public class OdinHealth : AbstractSprite
    {
        private Odin _odin;

        public OdinHealth(HealthAnimation animation, Odin odin, Vector2 position)
        {
            this.spriteManager = animation;
            animation.Position = position;
            _odin = odin;
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(_odin.Position.X, _odin.Position.Y - 1.5F);
            HealthAnimation healthAnimation = (HealthAnimation)spriteManager;
            healthAnimation.SetHealthState(_odin.Health);
            healthAnimation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override Vector2 DisplayPosition()
        {
            throw new NotImplementedException();
        }
    }
}