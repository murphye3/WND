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
    class ExplosionAnimation : SpriteAnimation
    {
        private bool _isDead = false;
        private bool _onSpawner = false;
        private bool _spawnIfrit = false;
        public ExplosionAnimation()
        {

        }

        public ExplosionAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
            _frameIndex = 0;
            IsMoving = true;
            InChainAnimation = true;
            this.TimeToUpdate = 6F;
        }

        public ExplosionAnimation(Texture2D texture, StreamReader sr, float timeToUpdate)
            : base(texture, sr)
        {
            this.TimeToUpdate = timeToUpdate;
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
            this.TimeToUpdate = 12f;
            
            if (timeElapsed > this.TimeToUpdate)
            { 
                _frameIndex++;
                if (this.FrameIndex == 20)
                {
                    this.IsDead = true;
                }
                if (_frameIndex == 5)
                {
                    this.SpawnIfrit = true;
                }
                else
                {
                    this.SpawnIfrit = false;
                }
                if (_frameIndex > Animations[AnimationName].NumOfFrames - 1)
                {
                    
                    InChainAnimation = false;
                    timeElapsed -= TimeToUpdate;
                    this.IsDead = true;
                }
                else
                {
                    if (this.ExplodeOnSpawner)
                    {
                        this.TimeToUpdate = 12f;
                    }
                    timeElapsed -= TimeToUpdate;
                    _frameIndex %= Animations[AnimationName].NumOfFrames;
                }
            }
        }
        public override void SetAnimationState(AnimationState state)
        {
            if (state != GetAnimationState() && !InChainAnimation)
            {
                if (state == AnimationState.Explosion)
                {
                    //this.FramesPerSecond = 5f;
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "explosion";
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
            return AnimationState.Explosion;
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

        public bool ExplodeOnSpawner
        {
            get
            {
                return _onSpawner;
            }
            set
            {
                _onSpawner = value;
            }
        }

        public bool SpawnIfrit
        {
            get
            {
                return _spawnIfrit;
            }
            set
            {
                _spawnIfrit = value;
            }
        }

    }
}
