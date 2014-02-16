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
    public class SpawnerAnimation : SpriteAnimation
    {
        private bool _checkIfrit;
        public SpawnerAnimation()
        {

        }

        public SpawnerAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            this.TimeToUpdate = 6F;
        }

        public SpawnerAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
            this.TimeToUpdate = 6F;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.GetAnimationState() == AnimationState.NotSpawning)
            {
                this.NotSpawning(gameTime);
            }
            else if (this.GetAnimationState() == AnimationState.Spawning)
            {
                this.Spawning(gameTime);
            }
            
        }
        private void Spawning(GameTime gameTime)
        {
            this.TimeToUpdate = 12f;

            if (timeElapsed > this.TimeToUpdate)
            {
                _frameIndex++;

                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    _frameIndex = 0;
                    timeElapsed -= TimeToUpdate;
                    this.CheckIfrit = false;
                }
                else
                {
                    if (_frameIndex == 12)
                    {
                        this.CheckIfrit = true;
                    }
                    else
                    {
                        this.CheckIfrit = false;
                    }
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }
            }
        }
        public bool CheckIfrit
        {
            get
            {
            return _checkIfrit;
            }
            set
            {
                _checkIfrit = value;
            }
        }
        private void NotSpawning(GameTime gameTime)
        {
            this.TimeToUpdate = 6f;

            if (timeElapsed > this.TimeToUpdate)
            {
                _frameIndex++;

                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    _frameIndex = 0;
                    timeElapsed -= TimeToUpdate;
                    this.CheckIfrit = false;
                }
                else
                {
                    this.CheckIfrit = false;
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }
            }
        }
        public override void SetAnimationState(AnimationState state)
        {
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.Spawning)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "spawning";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
                    
                }
                else if (state == AnimationState.NotSpawning)
                {
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "notspawning";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = false;
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

            string state = AnimationName.Split('_')[2];
            if (state == "spawning")
            {
                return AnimationState.Spawning;
            }
            else if (state == "notspawning")
            {
                return AnimationState.NotSpawning;
            }
            else
            {
                return base.GetAnimationState(state);
            }
        }
    }
}
