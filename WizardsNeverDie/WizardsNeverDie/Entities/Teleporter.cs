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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace WizardsNeverDie.Entities
{
    class Teleporter : AbstractEntity
    {
        private bool _plasmaCollided;
        World world = Farseer.Instance.World;
        private bool _collided;
        private Vector2 _position;
        public Teleporter(SpriteAnimation animation,  Vector2 position, float width, float height)
        {
            this.spriteManager = animation;
            animation.Position = position;
            this.body = new StaticBody(this, position, width, height);
            Body leftWall = BodyFactory.CreateRectangle(world, .05f, 1.6f, 1f, position + new Vector2(-1.1f, .2f));
            Body rightWall = BodyFactory.CreateRectangle(world, .05f, 1.6f, 1f, position + new Vector2(1.1f, .2f));
            Body topWall = BodyFactory.CreateRectangle(world, 2f, .05f, 1f, position + new Vector2(.05f, -1.3f));
        }
        public void Update(GameTime gameTime)
        {
            spriteManager.Position = new Vector2(body.Position.X, body.Position.Y);
            spriteManager.Update(gameTime);
        }

        public override bool WillCollide(AbstractEntity collidedWith)
        {
            return false;
        }
        public bool Collided
        {
            get
            {
                return _collided;
            }
            set
            {
                _collided = value;
            }
        }
        public bool PlasmaCollided
        {
            get
            {
                return _plasmaCollided;
            }
            set
            {
                _plasmaCollided = value;
            }
        }
    }
}
