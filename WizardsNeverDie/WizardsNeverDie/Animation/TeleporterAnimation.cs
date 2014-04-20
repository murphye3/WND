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
    public class TeleporterAnimation : SpriteAnimation
    {
        private bool _isDead = false;
        private bool _onSpawner = false;
        private int i = 0;
        public TeleporterAnimation()
        {

        }

        public TeleporterAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
            this.TimeToUpdate = 6F;
        }

        public TeleporterAnimation(Texture2D texture, StreamReader sr, float timeToUpdate)
            : base(texture, sr)
        {
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
            this.TimeToUpdate = timeToUpdate;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);
            Teleporting(gameTime);

        }
        private void Teleporting(GameTime gameTime)
        {
            if (timeElapsed > TimeToUpdate)
            {

                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    timeElapsed -= TimeToUpdate;
                    FrameIndex = 0;
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

            //this.FramesPerSecond = 5f;
            AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "open";
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
        }

        public override AnimationState GetAnimationState()
        {
            return AnimationState.Open;
        }

    }
}
