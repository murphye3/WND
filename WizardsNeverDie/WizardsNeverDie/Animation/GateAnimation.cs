using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Animation
{
    public class GateAnimation : SpriteAnimation
    {
        public GateAnimation(Texture2D texture, StreamReader sr, float timeToUpdate)
            : base(texture, sr)
        {
            this.TimeToUpdate = 15f;
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = false;
            
        }

        public override void Update(GameTime gameTime)
        {
            
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);

            if (this.GetAnimationState() == AnimationState.Open)
            {
                Open(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Close)
            {
                Close(gameTime);
            }
        }

        private void Open(GameTime gameTime)
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
        private void Close(GameTime gameTime)
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

        public override void SetAnimationState(AnimationState state)
        {
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.Open)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "open";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                }else if (state == AnimationState.Close)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "close";
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
            if (state == "open")
            {
                return AnimationState.Open;
            }
            else if (state == "close")
            {
                return AnimationState.Close;
            }
            else
            {
                return AnimationState.Lock;
            }
        }
    }
}
