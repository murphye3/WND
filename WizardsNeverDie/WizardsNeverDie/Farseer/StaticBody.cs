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

namespace WizardsNeverDie.Physics
{
    public class StaticBody : PhysicsBody
    {
        BaseLevel level;
        AbstractEntity entity;
        Vector2 position;
        float size;
        public StaticBody(AbstractEntity entity, Vector2 position, float size)
            : base()
        {
            this.entity = entity;
            this.position = position;
            this.size = size;

            World world = Farseer.Instance.World;
            Body body = BodyFactory.CreateRectangle(world, size, 1f, 1f);
            Bodies.Add(body);
            foreach (Fixture fixture in body.FixtureList)
                fixture.UserData = entity;
            body.UserData = entity;
            body.Position = position;
            body.Friction = float.MaxValue;
            body.Restitution = 0.3f;
            body.BodyType = BodyType.Static;
            body.CollisionCategories = Category.Cat1;
            body.CollidesWith = Category.Cat1;
            body.OnCollision += new OnCollisionEventHandler(onCollision);
        }
        bool onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            AbstractEntity collided = fixtureB.UserData as AbstractEntity;
            return entity.WillCollide(collided);
        }
    }
}
