using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Entities;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Trigger
{
    public class SpawnerAction : IAction
    {
        private bool _isDead = false;
        private List<Spawner> _spawners;
        public SpawnerAction(List<Spawner> spawners)
        {
            _spawners = spawners;
        }
        public void Draw()
        {
            ;
        }
        public void Update(GameTime gameTime)
        {
            if (!_isDead)
            {
                for (int i = 0; i < _spawners.Count; i++)
                {
                    _spawners[i].IsActivated = true;
                }
            }
            _isDead = true;
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
