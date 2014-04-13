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
    class KeyAnimation : SpriteAnimation
    {
        public KeyAnimation()
        {

        }

        public KeyAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            this.TimeToUpdate = 6F;
        }

        public KeyAnimation(Texture2D texture, StreamReader sr, float timeToUpdate)
            : base(texture, sr)
        {
            this.TimeToUpdate = timeToUpdate;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.GetAnimationState() == AnimationState.Rotating)
            {
                this.Rotating(gameTime);
            }
            
        }
        private void Rotating(GameTime gameTime)
        {
            this.TimeToUpdate = 9f;

            if (timeElapsed > this.TimeToUpdate)
            {
                _frameIndex++;

                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    _frameIndex = 0;
                }
                else
                {
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }
            }
        }
        public override void SetAnimationState(AnimationState state)
        {
            //the illusion of choice
            AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "rotating";
        }
        public override AnimationState GetAnimationState()
        {

            string state = AnimationName.Split('_')[2];
            if (state == "rotating")
            {
                return AnimationState.Rotating;
            }
            else
            {
                //It's always rotating, why is this here? WHO KNOWS
                return AnimationState.Rotating;
            }
        }
    }
}
