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
    class PotionExplosionAnimation : SpriteAnimation
    {
        private bool _isDead = false;
        private bool _onSpawner = false;
        private int i = 0;
        public PotionExplosionAnimation()
        {

        }

        public PotionExplosionAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
            this.TimeToUpdate = 6F;
        }

        public PotionExplosionAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
            this.TimeToUpdate = 6F;
        }

        //update current frame 
        public override void Update(GameTime gameTime)
        {
            //if(timeElapsed>TimeToUpdate)
            base.Update(gameTime);
            this.Explosion(gameTime);

        }
        private void Explosion(GameTime gameTime)
        {
            if (timeElapsed > TimeToUpdate)
            {
                this.TimeToUpdate = 10f;
                _frameIndex++;
                if (_frameIndex == 7)
                {
                    if(i < 20)
                    {
                        _frameIndex-=3;
                    }
                    i++;
                }
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    InChainAnimation = false;
                    timeElapsed -= TimeToUpdate;
                    this.IsDead = true;
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
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "potionexplosion";
                    _frameIndex = 0;
                    IsMoving = true;
                    InChainAnimation = true;
        }

        public override AnimationState GetAnimationState()
        {
            return AnimationState.PotionExplosion;
        }
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }

    }
}
