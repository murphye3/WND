using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using WizardsNeverDie.ScreenSystem;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Level 
{
    public abstract class BaseLevel : PhysicsGameScreen, IDemoScreen
    {
        protected KeyboardState keyboardState, lastKeyBoardState;
        protected String levelDetails, levelName, backgroundTextureStr;
        private LevelScreen _background;
        private BackgroundScreen test;
        public Vector2 ifritPosition = new Vector2(0, -20);

        public override void LoadContent()
        {
            base.LoadContent();

            if (backgroundTextureStr != null)
            {
                _background = new LevelScreen(backgroundTextureStr, this.Camera);
                test = new BackgroundScreen("Materials/Level1_Background");
                this.ScreenManager.AddScreen(test);
                this.ScreenManager.AddScreen(_background);
                this.AttachScreen(test);
                this.AttachScreen(_background);
            }
            base.DebugView = new DebugViewXNA(World);
            DebugView.AppendFlags(DebugViewFlags.Shape);
            DebugView.AppendFlags(DebugViewFlags.DebugPanel);
            DebugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            DebugView.AppendFlags(DebugViewFlags.Joint);
            DebugView.AppendFlags(DebugViewFlags.ContactPoints);
            DebugView.AppendFlags(DebugViewFlags.ContactNormals);
            DebugView.AppendFlags(DebugViewFlags.Controllers);
            DebugView.AppendFlags(DebugViewFlags.CenterOfMass);
            DebugView.AppendFlags(DebugViewFlags.AABB);

            DebugView.DefaultShapeColor = Color.White;
            DebugView.SleepingShapeColor = Color.LightGray;
            DebugView.LoadContent(ScreenManager.GraphicsDevice, ScreenManager.Content);
            DebugView.Enabled = false;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            lastKeyBoardState = keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Z) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z)) // toggle debugg view
                DebugView.Enabled = !DebugView.Enabled;
        }

        public string GetTitle()
        {
            return levelName;
        }

        public string GetDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(levelDetails);
            return sb.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            //Draw stuff in here
            ScreenManager.SpriteBatch.End();

            Matrix proj = Camera.SimProjection;
            Matrix view = Camera.SimView;
            DebugView.RenderDebugData(ref proj, ref view);

            base.Draw(gameTime);
        }
    }
}
