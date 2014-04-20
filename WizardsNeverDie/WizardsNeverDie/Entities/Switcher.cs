using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Animation;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Entities
{
    public class Switcher : AbstractEntity
    {
        private bool _isOn;

        public Switcher(SpriteAnimation animation, Vector2 position)
        {
            this.spriteManager = animation;
            this.body = new StaticBody(this, position - new Vector2(0, -.25f), 1f, 1.5f);
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = body.Position + (new Vector2(0, .25f));
            spriteManager.Update(gameTime);
        }
        //public override bool WillCollide(AbstractEntity collidedWith)
        //{
        //    return true;
        //}
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
            }
        }
    }
}