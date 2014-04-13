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
    class OdinAnimation : SpriteAnimation
    {
        private AnimationState _previousAnimation;
        public OdinAnimation()
        {

        }

        public OdinAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            this.TimeToUpdate = 10F;
        }

        public OdinAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
            this.TimeToUpdate = 6F;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);

            if (this.GetAnimationState() == AnimationState.Attack)
            {
                Attack(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Death)
            {
                Death(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Stop)
            {
                Stop(gameTime);
            }
            if (this.GetAnimationState() == AnimationState.Revived)
            {
                Revived(gameTime);
            }
        }
        public void Attack(GameTime gameTime)
        {
            this.TimeToUpdate = 30f;
            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    this.SetAnimationState(AnimationState.Walk);
                    _frameIndex = 0;
                    timeElapsed -= TimeToUpdate;
                }
                else
                {
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
        public void Revived(GameTime gameTime)
        {

            if (timeElapsed > TimeToUpdate)
            {
                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    this.SetAnimationState(AnimationState.Walk);
                    
                    timeElapsed -= TimeToUpdate;
                }
                else
                {
                    InChainAnimation = true;
                    if (_frameIndex < 4)
                    {
                        this.TimeToUpdate = 10f;
                    }
                    else
                    {
                        this.TimeToUpdate = 5f;
                    }
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

        public void Death(GameTime gameTime)
        {
            
            if (timeElapsed > TimeToUpdate)
            {
                IsMoving = true;
                InChainAnimation = true;
                _frameIndex++;
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    _frameIndex = 0;
                    this.SetAnimationState(AnimationState.Revived);
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "revive";
                }
                else
                {
                    if (_frameIndex < 4)
                    {
                        this.TimeToUpdate = 5f;
                    }else
                    {
                        this.TimeToUpdate = 5f;
                    }
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

        public void Stop(GameTime gameTime)
        {
            _frameIndex = 0;
        }
        public override void SetAnimationState(AnimationState state)
        {
            
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.Attack)
                {
                    this.PreviousAnimationState = this.GetAnimationState();
                    this.TimeToUpdate = 30f;
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "attack";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                    this.PreviousAnimationState = AnimationState.Attack;
                }
                else  if (state == AnimationState.Death)
                {
                    this.PreviousAnimationState = this.GetAnimationState();
                    this.TimeToUpdate = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "death";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                    this.PreviousAnimationState = AnimationState.Death;
                }
                else if (state == AnimationState.Revived)
                {
                    this.PreviousAnimationState = this.GetAnimationState();
                    this.TimeToUpdate = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "revive";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                    
                }
                else
                {
                    this.TimeToUpdate = 10f;
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
        public override AnimationState GetAnimationState()
        {
            if (!IsMoving)
            {
                return AnimationState.Stop;
            }
            string state = AnimationName.Split('_')[2];
            if (state == "attack")
            {
                return AnimationState.Attack;
            }
            else if (state == "death")
            {
                return AnimationState.Death;
            }
            else if (state == "revive")
            {
                return AnimationState.Revived;
            }
            else
            {
                return base.GetAnimationState(state);
            }
        }
    }
}
