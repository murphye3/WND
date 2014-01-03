using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsNeverDie.Animation
{
    public class AnimationClass
    {
        public Rectangle[] Rectangles;
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public bool IsLooping = true;

        public AnimationClass Copy()
        {
            AnimationClass ac = new AnimationClass();
            ac.Rectangles = Rectangles;
            ac.Color = Color;
            ac.Origin = Origin;
            ac.Rotation = Rotation;
            ac.Scale = Scale;
            ac.SpriteEffect = SpriteEffect;
            ac.IsLooping = IsLooping;
            return ac;
        }
        public int NumOfFrames
        {
            get { return Rectangles.Length; }
        }
    }
}
