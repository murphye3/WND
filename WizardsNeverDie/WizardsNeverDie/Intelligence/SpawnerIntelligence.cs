using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Entities;
namespace WizardsNeverDie.Intelligence
{
    public class SpawnerIntelligence : AbstractIntelligence
    {

        public AbstractCreature target;
        public AbstractEntity spawner;
        public SpawnerIntelligence(AbstractEntity spawner, AbstractCreature target)
        {
            this.target = target;
            this.spawner = spawner;
        }

        public override void Update(GameTime gameTime)
        {
            if (target == null)
                return;
            SpawnerAnimation animation = (SpawnerAnimation)this.spawner.SpriteManager;
            float targetDistance = (float)Math.Sqrt(Math.Pow((spawner.Position.X - target.Position.X), 2) + Math.Pow((spawner.Position.Y - target.Position.Y), 2));

            if (targetDistance < 12)
            {
                animation.SetAnimationState(AnimationState.Spawning);
            }
            else
            {
                animation.SetAnimationState(AnimationState.NotSpawning);
            }
        }
    }
}