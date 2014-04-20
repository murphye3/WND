using System;
using System.Collections.Generic;
using System.Linq;
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
using WizardsNeverDie.Entities;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace WizardsNeverDie.Entities
{
    public class RangedPurpleIfrit : AbstractCreature
    {
        private bool _isDead = false;

        public RangedPurpleIfrit(RangedPurpleIfritAnimation spriteManager, AbstractCreature target, Vector2 position, float width, float height, float targetDistance, float attackDistance, bool onlyAttack)
        {
            this.spriteManager = spriteManager;
            this.body = new BasicBody(this, position, 1f);
            this.intelligence = new PurpleCreatureIntelligence(this, target, .1f, targetDistance, attackDistance);
        }
        public void Update(GameTime gameTime)
        {
            this.getBody().Bodies[0].ResetDynamics();
            intelligence.Update(gameTime);
            spriteManager.Position = body.Position;
            spriteManager.Update(gameTime);
        }
        public AbstractCreature Target
        {
            get
            {
                return ((PurpleCreatureIntelligence)intelligence).target;
            }
            set
            {
                ((PurpleCreatureIntelligence)intelligence).target = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {

            RangedPurpleIfritAnimation animation = (RangedPurpleIfritAnimation)this.SpriteManager;
            if (collidedWith is WizardPlasma)
            {
                WizardPlasma plasma = (WizardPlasma)collidedWith;
                animation.SetAnimationState(AnimationState.Death);
                return false;
            }
            if (collidedWith is MeleeRedIfrit)
            {
                return true;
            }
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
