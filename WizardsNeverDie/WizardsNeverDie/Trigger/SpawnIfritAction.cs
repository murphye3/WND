using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Entities;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Animation;
using WizardsNeverDie.ScreenSystem;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace WizardsNeverDie.Trigger
{
    class SpawnIfritAction : IAction
    {
        private List<Explosion> _explosions;
        List<ExplosionAnimation> _explosionAnimation;
        private List<MeleeRedIfrit> _ifrits;
        private Wizard _player;
        private ScreenManager _screenManager;
        private bool _isDead;
        private bool _firstTime = true;
        private List<Vector2> _spawnVectors;
        public SpawnIfritAction(List<ExplosionAnimation> explosionAnimation, ref List<Explosion> explosion, Wizard player, ScreenManager screenManager, List<Vector2> spawnVectors, ref List<MeleeRedIfrit> creatures)
        {
            this._ifrits = creatures;
            this._spawnVectors = spawnVectors;
            this._explosions = explosion;
            this._explosionAnimation = explosionAnimation;
            this._screenManager = screenManager;
            this._player = player;
        }

        public void Update(GameTime gameTime)
        {
            if (_firstTime)
            {
                for (int i = 0; i < _explosionAnimation.Count; i++)
                {
                    _explosions.Add(new Explosion(_explosionAnimation[i], new Vector2(_spawnVectors[i].X,_spawnVectors[i].Y-2) , .01f, .01f));
                    _explosions[_explosions.Count - 1].SpriteManager.Animations[_explosionAnimation[i].AnimationName].Scale = 2f;
                }
                _firstTime = false;
            }

                for (int i = 0; i < _explosionAnimation.Count; i++)
                {
                    if (_explosionAnimation[i].SpawnIfrit == true)
                    {
                        MeleeRedIfritAnimation _creatureAnimation = new MeleeRedIfritAnimation(_screenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                        _creatureAnimation.AnimationName = "ifrit_d_walk";
                        _ifrits.Add(new MeleeRedIfrit(_creatureAnimation, _player, _spawnVectors[i] + new Vector2(1, 0), 1.5f, 1.5f, 30F));
                        _isDead = true;
                    }
                }
            

                

           
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

        public void Draw()
        {
            ;
        }
    }
}
