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

namespace WizardsNeverDie.Entities
{
    class Enemy: AbstractCreature
    {
        private bool _isDead = false;
        public Enemy(EnemyAnimation spriteManager, AbstractCreature target, Vector2 position)
        {
            this.spriteManager = spriteManager;
            this.body = new BasicBody(this, position, 1f);
            this.intelligence = new CreatureIntelligence(this, target, .05f);
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
                return ((CreatureIntelligence)intelligence).target;
            }
            set
            {
                ((CreatureIntelligence)intelligence).target = value;
            }
        }
        public override bool WillCollide(AbstractEntity collidedWith)
        {
            if (collidedWith is Player)
            {
                Player player = (Player)collidedWith;
                EnemyAnimation animation = (EnemyAnimation)this.SpriteManager;
                animation.SetAnimationState(AnimationState.Attack);
                if (player.Health == HealthAnimation.HealthState.Health100)
                    player.Health = HealthAnimation.HealthState.Health75;
                else if (player.Health == HealthAnimation.HealthState.Health75)
                    player.Health = HealthAnimation.HealthState.Health50;
                else if (player.Health == HealthAnimation.HealthState.Health50)
                    player.Health = HealthAnimation.HealthState.Health25;
                else if (player.Health == HealthAnimation.HealthState.Health25)
                    player.Health = HealthAnimation.HealthState.Health0;
            }
            return false;       
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
