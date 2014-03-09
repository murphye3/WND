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

namespace WizardsNeverDie.Physics
{
    class TriggerBody : PhysicsBody
    {
        private Vector2 _position;
        private float _size;
        private bool _isDead = false;
        private List<IAction> _actions;

        public TriggerBody(float width, float height, Vector2 position, float size, List<IAction>actions)
            : base()
        {
            this._position = position;
            this._size = size;
            this._actions = actions;


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
                    Conversation.ClearConversation();
                    Conversation.Avatars.Clear();
                    Conversation.Avatars.Add(test.Avatar);
                    Conversation.Avatars.Add(test.Avatar);
                    Conversation.StartConversation(test.Str1, test.Str2);
                }
                this.Bodies[0].CollidesWith = Category.None;
            }
            return false;
        }

    }
}
