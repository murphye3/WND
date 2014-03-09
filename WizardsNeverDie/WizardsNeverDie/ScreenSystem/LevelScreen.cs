using System;
using WizardsNeverDie;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardsNeverDie.ScreenSystem
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    public class LevelScreen : GameScreen
    {
        private string _background;
        private Texture2D _backgroundTexture;
        private Camera2D _camera;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public LevelScreen()
            : this("Materials/blank", null)
        { }
        public LevelScreen(String background, Camera2D newCamera)
        {
            _background = background;
            _camera = newCamera;
        }

        public override void LoadContent()
        {
            //_logoTexture = ScreenManager.Content.Load<Texture2D>("Common/logo");
            _backgroundTexture = ScreenManager.Content.Load<Texture2D>(_background);
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_backgroundTexture, new Vector2(0,0), Color.Black);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, _camera.View);
            ScreenManager.SpriteBatch.Draw(_backgroundTexture, new Vector2(-_backgroundTexture.Width / 2, -_backgroundTexture.Height / 2), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}