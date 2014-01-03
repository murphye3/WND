using Microsoft.Xna.Framework.Graphics;

namespace WizardsNeverDie.Animation
{
    class FrameAnimation : SpriteManager
    {
        public FrameAnimation(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
        }

        public void SetFrame(int frame)
        {
            if (frame < Animations[AnimationName].NumOfFrames)
                _frameIndex = frame;
        }
    }
}
