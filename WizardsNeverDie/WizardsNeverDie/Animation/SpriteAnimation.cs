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
    public enum AnimationState
    {
        Walk,
        Run,
        Attack,
        Spell1,
        Stop,
        Death,
        Disentegrated,
        Explosion,
        Spawning,
        NotSpawning,
        PotionExplosion,
        PurpleSpell,
        Talking,
        Open,
        Close,
        Lock,
        Rotating,
        Revived,
        OdinDeath,
        TrippinForward,
        Yolo
    }

    [Flags]
    public enum Orientation
    {
        None,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        DownLeft = 5,
        DownRight = 6,
        UpLeft = 9,
        UpRight = 10
    }

    public class SpriteAnimation : SpriteManager
    {
        
        protected float framesPerSecond = 2f;

        // default to 20 frames per second
        //private float  = 1/10f; // framespersecond
        protected float timeElapsed = 0;
        protected bool  InChainAnimation = false, left = false, right = false;
        private bool _isMoving = false;
        public SpriteAnimation()
        {

        }
        public SpriteAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
        }

        public SpriteAnimation(Texture2D texture, StreamReader sr)
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
            if (GetAnimationState() == AnimationState.Stop)
            {
                //FrameIndex %= Animations[Animation].NumOfFrames;
                ////timeElapsed -= timeToUpdate;
                if (timeElapsed > this.TimeToUpdate)
                    timeElapsed -= TimeToUpdate;
                return;
            }
            else if (GetAnimationState() == AnimationState.Walk
                && timeElapsed>this.TimeToUpdate)
            {
                // Keep the Frame between 0 and the total frames, minus one.
                _frameIndex++;
                _frameIndex %= Animations[AnimationName].NumOfFrames;
                timeElapsed -= TimeToUpdate;
            }
        }
        public String GetOrientation(Orientation orientation)
        {
            if (orientation == Orientation.Left)
                return "dl";
            else if (orientation == Orientation.Right)
                return "dr";
            else if (orientation == Orientation.Up)
                return "u";
            else if (orientation == Orientation.Down)
                return "d";
            else if (orientation == Orientation.UpLeft)
                return "ul";
            else if (orientation == Orientation.UpRight)
                return "ur";
            else if (orientation == Orientation.DownLeft)
                return "dl";
            else if (orientation == Orientation.DownRight)
                return "dr";
            else
                return "";
        }
        public Orientation GetOrientation()
        {
            if (left)
                return Orientation.Left;
            else if (right)
                return Orientation.Right;

            string orientation = AnimationName.Split('_')[1];
            //if(this.AnimationName
            if (orientation == "u")
                return Orientation.Up;
            else if (orientation == "ul")
                return Orientation.UpLeft;
            else if (orientation == "ur")
                return Orientation.UpRight;
            else if (orientation == "dl")
                return Orientation.DownLeft;
            else if (orientation == "d")
                return Orientation.Down;
            else if (orientation == "dr")
                return Orientation.DownRight;
            else
                return Orientation.None;
        }
        public void SetOrientation(Orientation orentation)
        {
            string o = AnimationName.Split('_')[0] + '_' + GetOrientation(orentation) + '_' + AnimationName.Split('_')[2];
            if (GetOrientation(orentation) != o && IsMoving)
            {
                if (orentation == Orientation.Left)
                {
                    left = true;
                    right = false;
                }
                else if (orentation == Orientation.Right)
                {
                    right = true;
                    left = false;
                }
                else
                {
                    left = false;
                    right = false;
                }
                this.AnimationName = AnimationName.Split('_')[0] + '_' + GetOrientation(orentation) + '_' + AnimationName.Split('_')[2];
                _frameIndex %= Animations[AnimationName].Rectangles.Count();
            }
        }
        public virtual void SetAnimationState(AnimationState state)
        {
            switch (state)
            {
                case AnimationState.Attack:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "attack";
                    _isMoving = false;
                    break;
                case AnimationState.Walk:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "walk";
                    _isMoving = true;
                    break;
                case AnimationState.Stop:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "walk";
                    _isMoving = false;
                    break;
                case AnimationState.Death:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "death";
                    _isMoving = false;
                    break;
                case AnimationState.Explosion:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "explosion";
                    break;
                case AnimationState.Spawning:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "spawning";
                    break;
                case AnimationState.NotSpawning:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "notspawning";
                    break;
                case AnimationState.PotionExplosion:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "potionexplosion";
                    break;
                case AnimationState.PurpleSpell:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "purplespell";
                    break;
                case AnimationState.Talking:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "talking";
                    break;
                case AnimationState.Lock:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "lock";
                    break;
                case AnimationState.Open:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "open";
                    break;
                case AnimationState.Close:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "close";
                    break;
                case AnimationState.Rotating:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "rotating";
                    break;
                case AnimationState.Revived:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "revive";
                    break;
                case AnimationState.OdinDeath:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "odindeath";
                    break;
                case AnimationState.TrippinForward:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "forward";
                    break;
                case AnimationState.Yolo:
                    AnimationName = AnimationName.Split('_')[0] + '_' + AnimationName.Split('_')[1] + '_' + "yolo";
                    break;
            }
        }

        public virtual AnimationState GetAnimationState()
        {
            return GetAnimationState(AnimationName.Split('_')[2]);
        }
        protected AnimationState GetAnimationState(string state)
        {
            //if (state == "attack")
            //    return AnimationState.Attack;
            //else (state == "walk")
            if(state == "walk" && _isMoving)
            {
                return AnimationState.Walk;
            }
            else
            {
                return AnimationState.Stop;
            }
        }

        //public void SetOrientation()
        //{
        //    Animation.Split('_')
        //}
    }
}
