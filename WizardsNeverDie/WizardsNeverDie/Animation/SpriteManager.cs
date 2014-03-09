using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using WizardsNeverDie.Utility;
using WizardsNeverDie.Level;

namespace WizardsNeverDie.Animation
{
    public abstract class SpriteManager
    {
        IfritDemo ifrit = new IfritDemo();
        protected Texture2D _texture;
        public Vector2 Position;
        private Dictionary<string, AnimationClass> _animations = new Dictionary<string, AnimationClass>();
        protected int _frameIndex = 0;
        protected Vector2 Origin;
        private int height;
        private int width;
        public float xCoord;
        public float yCoord;
        private string animation;

        public Dictionary<string, AnimationClass> Animations
        {
            get
            {
                return _animations;
            }
            set
            {
                _animations = value;
            }
        }

        public int FrameIndex
        {
            get
            {
                return _frameIndex;
            }
            set
            {
                _frameIndex = value;
            }
        }


        public string AnimationName
        {
            get { return animation; }
            set
            {

                animation = value;
                //FrameIndex = 0;
            }
        }
        protected SpriteManager()
        {

        }
        protected SpriteManager(Texture2D Texture, int Frames, int animations)
        {
            this.Position = ifrit.ifritPosition;
            this._texture = Texture;
            width = Texture.Width / Frames;
            height = Texture.Height / animations;
            Origin = new Vector2(width / 2, height / 2);
        }

        protected SpriteManager(Texture2D Texture, StreamReader sr)
        {
            this.Position = ifrit.ifritPosition;
            this._texture = Texture;
            AddAnimation(sr);
        }

        public void AddAnimation(string name, int row,
            int frames, AnimationClass animation)
        {
            Rectangle[] recs = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                recs[i] = new Rectangle(i * width,
                    (row - 1) * height, width, height);
            }
            //animation.Frames = frames;
            animation.Rectangles = recs;
            _animations.Add(name, animation);
        }

        public void AddAnimation(string name, AnimationClass animation)
        {
            _animations.Add(name, animation);
        }

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }
        public void AddAnimation(StreamReader sr)
        {
            List<Rectangle> recs = new List<Rectangle>();
            string line, name, currentName = null;
            string[] strs;
            int x, y, width, height;
            AnimationClass animation = new AnimationClass();
            //line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                strs = line.Split(' ');

                //get the name  <name>_<orientation>_<Action>-<FrameNumber>
                name = strs[0].Substring(0, strs[0].LastIndexOf('-'));
                if (currentName == null)
                    currentName = name;
                else if (name != currentName)    // we got all the frames for the animation
                {
                    animation.Rectangles = recs.ToArray();
                    AddAnimation(currentName, animation);

                    animation = new AnimationClass();
                    recs = new List<Rectangle>();
                    currentName = name;
                }
                x = Convert.ToInt32(strs[2]);
                y = Convert.ToInt32(strs[3]);
                width = Convert.ToInt32(strs[4]);
                height = Convert.ToInt32(strs[5]);
                recs.Add(new Rectangle(x, y, width, height));
            }
            animation.Rectangles = recs.ToArray();
            AddAnimation(currentName, animation);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

                if (_frameIndex < _animations[AnimationName].Rectangles.Count())
                {
                    Rectangle rec = _animations[AnimationName].Rectangles[_frameIndex];
                    Vector2 pos = new Vector2((int)ConvertUnits.ToDisplayUnits(Position.X), (int)ConvertUnits.ToDisplayUnits(Position.Y));
                    pos = new Vector2(pos.X - (rec.Width / 2), pos.Y - (rec.Height / 2));

                    spriteBatch.Draw(_texture, pos,
                        rec,
                        _animations[AnimationName].Color,
                        _animations[AnimationName].Rotation, Origin,
                        _animations[AnimationName].Scale,
                        _animations[AnimationName].SpriteEffect, 0f);
                }
            
        }
    }
}
