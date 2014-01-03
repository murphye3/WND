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
using WizardsNeverDie.Level;

namespace WizardsNeverDie.Physics
{
    public abstract class PhysicsBody
    {
        private List<Body> bodies = new List<Body>();
        public PhysicsBody() 
        { 
        }
        public Vector2 DisplayPosition
        {
            get { return Utility.ConvertUnits.ToDisplayUnits(Position); }
        }
        public List<Body> Bodies
        {
            get { return bodies; }
        }
        public virtual Vector2 Position
        {
            get 
            {
                Vector2 position = Vector2.Zero;
                foreach (Body body in bodies)
                    position += body.Position;
                position /= bodies.Count;
                return position;
            }
            set 
            {
                Vector2 position = Vector2.Zero;
                foreach (Body body in bodies)
                    position += body.Position;
                position /= bodies.Count;
            }
        }
        public virtual Vector2 Velocity
        {
            get 
            {
                Vector2 velocity = Vector2.Zero;
                foreach (Body body in bodies)
                    velocity += body.LinearVelocity;
                velocity /= bodies.Count;
                return velocity;
            }
        }
        public virtual float Rotation
        {
            get
            {
                float rotation = 0f;
                foreach (Body body in bodies)
                    rotation += body.Rotation;
                rotation /= bodies.Count;
                return rotation;
            }
        }
        public void Move(Vector2 direction)
        {
            foreach (Body body in bodies)
                body.Position += direction;
            
        }
        public float GetArea()
        {
            float area = 0;
            foreach (Body body in bodies)
                foreach (Fixture fixture in body.FixtureList)

                    area += fixture.Shape.MassData.Area;
            return area;
        }
        public bool isInArea(Vector2 point)
        {
            foreach (Body body in bodies)
                foreach (Fixture fixture in body.FixtureList)
                    if (!fixture.TestPoint(ref point))
                        return false;
            return true;
        }
    }
}
