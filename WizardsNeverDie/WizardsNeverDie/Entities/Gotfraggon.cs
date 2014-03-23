using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Intelligence;
using WizardsNeverDie.Animation;

namespace WizardsNeverDie.Entities
{
    public class Gotfraggon : AbstractCreature
    {
        private bool _isDead;

        public Gotfraggon(GotfraggonAnimation spriteManager, AbstractCreature target, Vector2 position, float width, float height, float targetDistance)
        {
            this.spriteManager = spriteManager;
            this.body = new BasicBody(this, position, 5f);
            this.intelligence = new GotfraggonIntelligence(this, target, .05f, targetDistance);
        }
        public void Update(GameTime gameTime)
        {
            intelligence.Update(gameTime);
            spriteManager.Position = body.Position;
            spriteManager.Update(gameTime);
        }
        public AbstractCreature Target
        {
            get
            {
                return ((GotfraggonIntelligence)intelligence).target;
            }
            set
            {
                ((GotfraggonIntelligence)intelligence).target = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {
            return true;
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
    }
}
