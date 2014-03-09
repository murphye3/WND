using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsNeverDie.Animation
{
    public class WizardAnimation : SpriteAnimation
    {
        private bool _checkEndSpell = false;
        private AnimationState _previousAnimation;
        private bool spell1 = false, spell2 = false;
        public WizardAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            this.TimeToUpdate = 7F;
        }

        public WizardAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
            this.TimeToUpdate = 7F;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);

            if (this.GetAnimationState() == AnimationState.Spell1)
            {
                Spell1(gameTime);
            }
        }
        private void Spell1(GameTime gameTime)
        {
            TimeToUpdate = 15f;
            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex == Animations[AnimationName].NumOfFrames - 1)
                {
                    this.CheckEndSpell = true;
                }
                else if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    this.CheckEndSpell = false;
                    InChainAnimation = false;
                    _frameIndex = 0;
                    timeElapsed -= TimeToUpdate;
                    this.SetAnimationState(AnimationState.Walk);
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
        public override void SetAnimationState(AnimationState state)
        {
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.Spell1)
                {
                    
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "spell1";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true; 
                }
                else
                {
                    this.PreviousAnimationState = AnimationState.Walk;
                    InChainAnimation = false;
                    //this.FramesPerSecond = 5f;
                    base.SetAnimationState(state);
                }
            }
        }

        public AnimationState PreviousAnimationState
        {
            get
            {
                return _previousAnimation;
            }
            set
            {
                _previousAnimation = value;
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
             if (state == "spell1")
            {
                return AnimationState.Spell1;
            }
            else
            {
                 return base.GetAnimationState(state);
            }
        }
    }
}
