using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace WizardsNeverDie.Animation
{
    public class HealthAnimation : SpriteManager
    {
        public enum HealthState
        {
            Health100 = 100,
            Health80 = 80,
            Health75 = 75,
            Health60 = 60,
            Health50 = 50,
            Health40 = 40,
            Health25 = 25,
            Health20 = 20,
            Health0 = 0
        }

        protected float framesPerSecond = 2f;
        protected float timeElapsed = 0;
        protected bool InChainAnimation = false, left = false, right = false;
        private bool _isMoving = false;

        public HealthAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
        }

        public HealthAnimation(Texture2D texture, StreamReader sr)
            : base(texture, sr)
        {
        }

        public float FramesPerSecond
        {
            set
            {
                framesPerSecond = value;
            }
        }

        public float TimeToUpdate
        {
            get
            {
                return 1 / framesPerSecond;
            }
            set
            {
                framesPerSecond = value;
            }
        }
        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }


        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)
                            gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > this.TimeToUpdate)
            {
                // Keep the Frame between 0 and the total frames, minus one.
                _frameIndex++;
                _frameIndex %= Animations[AnimationName].NumOfFrames;
                timeElapsed -= TimeToUpdate;
            }
        }
        public virtual void SetHealthState(HealthState state)
        {
            switch (state)
            {
                case HealthState.Health100:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health100";
                    _isMoving = true;
                    break;
                case HealthState.Health80:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health80";
                    _isMoving = true;
                    break;
                case HealthState.Health75:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health75";
                    _isMoving = true;
                    break;
                case HealthState.Health60:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health60";
                    _isMoving = true;
                    break;
                case HealthState.Health50:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health50";
                    _isMoving = false;
                    break;
                case HealthState.Health40:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health40";
                    _isMoving = true;
                    break;
                case HealthState.Health25:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health25";
                    _isMoving = false;
                    break;
                case HealthState.Health20:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health20";
                    _isMoving = true;
                    break;
                case HealthState.Health0:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "health0";
                    _isMoving = false;
                    break;
            }
        }
    }
}
