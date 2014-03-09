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
    class PotionExplosion : AbstractCreature
    {
        private Vector2 _position;
        private Wizard player;
        public PotionExplosion(SpriteAnimation animation, Wizard player, List<MeleeRedIfrit> enemy, Vector2 position)
        {
            _position = position;
            
            this.spriteManager = animation;
            animation.Position = position;
            this.intelligence = new PotionExplosionIntelligence(player, enemy);
            this.player = player;
        }
        public void Update(GameTime gameTime)
        {
            intelligence.Update(gameTime);
            spriteManager.Position = new Vector2(player.Position.X, player.Position.Y);
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            return false;
        }
    }
}
