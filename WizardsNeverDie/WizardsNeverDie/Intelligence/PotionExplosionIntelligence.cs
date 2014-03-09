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
    class PotionExplosionIntelligence : AbstractIntelligence
    {

        private AbstractCreature player;
        private List<MeleeRedIfrit> enemy;
        public PotionExplosionIntelligence(AbstractCreature player, List<MeleeRedIfrit> enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        public override void Update(GameTime gameTime)
        {
            if (enemy == null)
                return;
            
            for (int i = 0; i < enemy.Count; i++)
            {
                MeleeRedIfritAnimation animation = (MeleeRedIfritAnimation)this.enemy[i].SpriteManager;
                float targetDistance = (float)Math.Sqrt(Math.Pow((player.Position.X - enemy[i].Position.X), 2) + Math.Pow((player.Position.Y - enemy[i].Position.Y), 2));
                    if (targetDistance < 4 && !enemy[i].IsDead)
                    {
                        enemy[i].IsDead = true;
                        animation.SetAnimationState(AnimationState.Death);
                    }
                    else
                    {
                        animation.SetAnimationState(AnimationState.Stop);
                    }       
            }
        }
    }
}