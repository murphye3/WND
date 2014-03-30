using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Animation;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Entities
{
    public class Gate : AbstractEntity
    {
        private bool _unlock = false;
        private Vector2 _position;
        private bool _gateCollideFirstTime = true;
        private float _scale;
        public Gate(SpriteAnimation animation, Vector2 position, float width, float height, float scale)
        {
            this._position = position;
            this.spriteManager = animation;
            this.body = new StaticBody(this, position, width, height);
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = _position;
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            if (collidedWith is Wizard)
            {
                this._unlock = true;
            }
            return false;
        }

        public bool Unlock
        {
            get
            {
                return this._unlock;
            }
            set
            {
                this._unlock = value;
            }
        }
        public bool GateCollideFirstTime
        {
            get
            {
                return this._gateCollideFirstTime;
            }
            set
            {
                this._gateCollideFirstTime = value;
            }
        }
    }
}