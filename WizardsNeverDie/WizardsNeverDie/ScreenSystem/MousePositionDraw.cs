using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Globalization;
using Microsoft.Xna.Framework.Input;

namespace WizardsNeverDie.ScreenSystem
{
    class DisplayData : DrawableGameComponent
    {
        private NumberFormatInfo _format; 
        private double _mouseXSim, _mouseYSim, _rotation;
        private ScreenManager _screenManager;
        private Vector2 _position;
        private MouseState _mouseState;
        public DisplayData(ScreenManager screenManager)
            : base(screenManager.Game)
        {
            _screenManager = screenManager;
            _format = new NumberFormatInfo();
            _format.NumberGroupSeparator = ".";
            _position = new Vector2(30, 45);
        }
        public override void Update(GameTime gameTime)
        {
          
            _mouseState = Mouse.GetState();
            //_mouseXReal = _mouseState.X;
            //_mouseYReal = _mouseState.Y;
            //ConvertUnits.ToSimUnits(viewport.Width) / 2f - 0.75f;
            //ConvertUnits.ToSimUnits(viewport.Height) / 2f - 0.75f;
            _mouseXSim = ((ConvertUnits.ToSimUnits(_screenManager.GraphicsDevice.Viewport.Width) / 2f - 0.75f) - ConvertUnits.ToSimUnits(_mouseState.X))*-1;
            _mouseXSim = Math.Truncate(_mouseXSim * 100) / 100;
            _mouseYSim = ((ConvertUnits.ToSimUnits(_screenManager.GraphicsDevice.Viewport.Height) / 2f - 0.75f) - ConvertUnits.ToSimUnits(_mouseState.Y))*-1;
            _mouseYSim = Math.Truncate(_mouseYSim * 100) / 100;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            string mouseXSStr = string.Format(_format, "{0} fps", _mouseXSim);
            string mouseYSStr = string.Format(_format, "{0} fps", _mouseYSim);
            string output = "Sim X:"+mouseXSStr+" Y:"+mouseYSStr;

            _screenManager.SpriteBatch.Begin();

            //_screenManager.SpriteBatch.DrawString(_screenManager.Fonts.DetailsFont, output, 
            //                                      _position + Vector2.One, Color.White);
            _screenManager.SpriteBatch.DrawString(_screenManager.Fonts.DetailsFont, output,
                                                  _position, Color.Black);
            _screenManager.SpriteBatch.End();
        }
    }
}
