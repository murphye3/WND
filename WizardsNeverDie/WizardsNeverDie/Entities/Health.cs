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
    public class Health : AbstractSprite
    {
        private Player _player;

        public Health(HealthAnimation animation, Player player, Vector2 position)
        {
            this.spriteManager = animation;
            animation.Position = position;
            _player = player;
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(_player.Position.X, _player.Position.Y-1.5F);
            HealthAnimation healthAnimation = (HealthAnimation)spriteManager;
            healthAnimation.SetHealthState(_player.Health);
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