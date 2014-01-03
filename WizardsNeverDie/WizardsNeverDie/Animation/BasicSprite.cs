using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsNeverDie.Animation
{
    public struct BasicSprite
    {
        public Vector2 Origin;
        public Texture2D Texture;

        public BasicSprite(Texture2D texture, Vector2 origin)
        {
            this.Texture = texture;
            this.Origin = origin;
        }

        public BasicSprite(Texture2D sprite)
        {
            Texture = sprite;
            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
}
