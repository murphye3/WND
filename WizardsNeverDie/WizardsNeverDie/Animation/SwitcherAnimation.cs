using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Animation
{
    class SwitcherAnimation : SpriteAnimation
    {
        public SwitcherAnimation(Texture2D texture, StreamReader sr, float timeToUpdate)
            : base(texture, sr)
        {
            this.TimeToUpdate = timeToUpdate;
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = false;
            this.TimeToUpdate = 6F;
        }

        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);

            if (this.GetAnimationState() == AnimationState.On)
            {
                On(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Off)
            {
                Off(gameTime);
            }
        }

        private void On(GameTime gameTime)
        {
            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    _frameIndex = Animations[AnimationName].NumOfFrames - 1;
                    InChainAnimation = true;
                    timeElapsed -= TimeToUpdate;
                }
                else
                {
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }

            }
        }
        private void Off(GameTime gameTime)
        {
            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    _frameIndex = Animations[AnimationName].NumOfFrames - 1;
                    _frameIndex = 0;
                    InChainAnimation = true;
                    timeElapsed -= TimeToUpdate;
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
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.On)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "on";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                }
                else if (state == AnimationState.Off)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "off";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                }
                else
                {
                    InChainAnimation = false;
                    //this.FramesPerSecond = 5f;
                    base.SetAnimationState(state);
                }
            }
        }

        public override AnimationState GetAnimationState()
        {
            if (!IsMoving)
            {
                return AnimationState.Stop;
            }
            string state = AnimationName.Split('_')[2];
            if (state == "talking")
            {
                return AnimationState.Talking;
            }
            else
            {
                return base.GetAnimationState(state);
            }
        }
    }
}
