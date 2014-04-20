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
using WizardsNeverDie.ScreenSystem;
using WizardsNeverDie.Trigger;
using WizardsNeverDie.Dialog;
using WizardsNeverDie.Entities;

namespace WizardsNeverDie.Physics
{
    public class TriggerBody : PhysicsBody
    {
        private Vector2 _position;
        private float _size;
        private bool _isDead = false;
        private List<IAction> _actions;
        private List<WizardPlasma> _plasma;

        public TriggerBody(float width, float height, Vector2 position, float size, List<IAction>actions, List<WizardPlasma> plasma)
            : base()
        {
            this._position = position;
            this._size = size;
            this._actions = actions;
            _plasma = plasma;

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


        public TriggerBody(float width, float height, Vector2 position, float size, List<WizardPlasma> plasma)
            : base()
        {
            this._position = position;
            this._size = size;

            _plasma = plasma;
            World world = Farseer.Instance.World;
            Body body = BodyFactory.CreateRectangle(world, width, height, 1f, position);
            Bodies.Add(body);
            for (int k = 0; k < _plasma.Count; k++)
            {
                this.Bodies[0].IgnoreCollisionWith(_plasma[k].getBody().Bodies[0]);
            }
            //body.UserData = entity;
            body.Position = position;
            body.Friction = float.MaxValue;
            body.Restitution = 0.3f;
            body.BodyType = BodyType.Static;
            body.SleepingAllowed = false;
            body.CollisionCategories = Category.Cat1;
            body.CollidesWith = Category.Cat1;
            body.Awake = true;
            body.OnCollision += new OnCollisionEventHandler(onCollision2);
        }
        public void Update(GameTime gameTime)
        {
                for (int k = 0; k < _plasma.Count; k++)
                {
                    this.Bodies[0].IgnoreCollisionWith(_plasma[k].getBody().Bodies[0]);
                }
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
        
        public List<IAction> ActionList
        {
            get
            {
                return _actions;
            }
            set
            {
                _actions = value;
            }
        }
        bool onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        { 
            _isDead = true;
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i] is MessageAction)
                {
                    MessageAction test = (MessageAction)_actions[i];
                    Conversation.Avatars.Clear();
                    Conversation.Avatars.Add(test.Avatar);
                    Conversation.Avatars.Add(test.Avatar);
                    Conversation.LoadConversation(test.FileName);
                    Conversation.StartConversation();
                }
                this.Bodies[0].CollidesWith = Category.None;
            }
            return false;
        }

        bool onCollision2(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            _isDead = true;
            this.Bodies[0].CollidesWith = Category.None;
            return false;
        }
    }
}
