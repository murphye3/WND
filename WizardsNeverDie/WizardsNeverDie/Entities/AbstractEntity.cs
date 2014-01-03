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

namespace WizardsNeverDie.Entities
{
    public abstract class AbstractEntity : AbstractSprite
    {
        protected PhysicsBody body;

        public PhysicsBody getBody()
        {
            return body;
        }

        public override Microsoft.Xna.Framework.Vector2 DisplayPosition()
        {
            return Utility.ConvertUnits.ToDisplayUnits(body.Position);
        }
        public Vector2 Position
        {
            get { return body.Position; }
        }
        public float Rotation
        {
            get { return body.Rotation;  }
        }
        public Vector2 Velocity
        {
            get { return body.Velocity; }
        }

        public virtual void OnCollision(AbstractEntity collidedWith)
        {
        }
        public virtual bool WillCollide(AbstractEntity collidedWith)
        {
            return true;
        }
        public virtual void OnSeperation(AbstractEntity collidedWith)
        {
        }
    }
}
