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
using WizardsNeverDie.Entities;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;

namespace WizardsNeverDie.Physics
{
    class TriggerBody : PhysicsBody
    {
        private Vector2 _position;
        private float _size;
        private List<Spawner> _spawners;
        private bool _isDead = false;

        public TriggerBody(float width, float height, Vector2 position, float size, List<Spawner> spawners)
            : base()
        {
            this._position = position;
            this._size = size;
            this._spawners = spawners;

            World world = Farseer.Instance.World;
            Body body = BodyFactory.CreateRectangle(world, width, height, 1f, position);
            Bodies.Add(body);
           
            //body.UserData = entity;
            body.Position = position;
            body.Friction = float.MaxValue;
            body.Restitution = 0.3f;
            body.BodyType = BodyType.Static;
            body.SleepingAllowed = false;
            body.CollisionCategories = Category.Cat1;
            body.CollidesWith = Category.Cat1;
            body.Awake = true;
            body.OnCollision += new OnCollisionEventHandler(onCollision);
        }
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }
        bool onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        { 
            foreach (Spawner s in _spawners)
            {
                s.IsActivated = true;
            }
            _isDead = true;
            return false;
        }

    }
}
