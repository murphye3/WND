using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Level;
using WizardsNeverDie.ScreenSystem;
using FarseerPhysics.Factories;
using WizardsNeverDie.Physics;

namespace WizardsNeverDie.Trigger
{
    public class SwitchLevelAction: IAction
    {
        private BaseLevel _nextLevel;
        private BaseLevel _currentLevel;
        private bool _isDead = false;
        private ScreenManager _screenManager;
        public SwitchLevelAction(ScreenManager screenManager, BaseLevel currentLevel, BaseLevel nextLevel)
        {
            _currentLevel = currentLevel;
            _nextLevel = nextLevel;
            _screenManager = screenManager;
        }
        public void Draw()
        {
            ;
        }
        public void Update(GameTime gameTime)
        {
            if (!_isDead)
            {
                for(int i = 0; i<Farseer.Instance.World.BodyList.Count; i++)
                {
                    Farseer.Instance.World.RemoveBody(Farseer.Instance.World.BodyList[i]);
                }
                _screenManager.RemoveScreen(_currentLevel);
                _screenManager.AddScreen(_nextLevel);
                
            }
            //_isDead = true;
        }
        bool IAction.IsDead
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
    }
}
