using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WizardsNeverDie.Entities;

namespace WizardsNeverDie.Animation
{
    public class RangedPurpleIfritAnimation : MeleeRedIfritAnimation
    {
        private bool _checkEndSpell = false;
        public RangedPurpleIfritAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            this.TimeToUpdate = 10F;
        }

        public RangedPurpleIfritAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
            this.TimeToUpdate = 6F;
        }

        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);

            if (this.GetAnimationState() == AnimationState.PurpleSpell)
            {
                PurpleSpell(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Death)
            {
                Death(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Stop)
            {
                Stop(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Disentegrated)
            {
                ExplosionAnimation explosion = new ExplosionAnimation();
                explosion.SetAnimationState(AnimationState.Explosion);
            }
        }



        private void PurpleSpell(GameTime gameTime)
        {
            TimeToUpdate = 7f;
            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex == Animations[AnimationName].NumOfFrames - 2)
                {
                    this.CheckEndSpell = true;
                }
                else if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    this.CheckEndSpell = false;
                    InChainAnimation = false;
                    _frameIndex = 0;
                    timeElapsed -= TimeToUpdate;
                }
                else
                {
                    this.CheckEndSpell = false;
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }

                //else
                //{
                //    //FrameIndex %= Animations[this.Animation].NumOfFrames;
                //    // Keep the Frame between 0 and the total frames, minus one.
                //    FrameIndex++;
                //    timeElapsed -= 1 / this.FramesPerSecond;
                //}
            }
        }

            public bool CheckEndSpell
        {
            get
            {
                return _checkEndSpell;
            }
            set
            {
                _checkEndSpell = value;
            }
        }
            public override AnimationState GetAnimationState()
            {
                if (!IsMoving)
                {
                    return AnimationState.Stop;
                }
                string state = AnimationName.Split('_')[2];
                if (state == "purplespell")
                {
                    return AnimationState.PurpleSpell;
                }
                else if (state == "death")
                {
                    return AnimationState.Death;
                }
                else if (state == "disentegrated")
                {
                    return AnimationState.Disentegrated;
                }
                else
                {
                    return base.GetAnimationState(state);
                }
            }
        
    }
}
