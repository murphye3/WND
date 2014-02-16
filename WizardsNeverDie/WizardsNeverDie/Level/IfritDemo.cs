using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Level;
using WizardsNeverDie.Entities;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;

namespace WizardsNeverDie.Level
{
    class IfritDemo : BaseLevel
    {
        bool firstTime = true;
        private Player _player;
        private WizardAnimation _wizard;
        private SpawnerAnimation _spawnerSprite;
        private SpawnerAnimation _spawnerSprite2;
        private SpawnerAnimation _spawnerSprite3;
        private SpawnerAnimation _spawnerSprite4;
        private List<Brick> _bricks;
        private List<Enemy> _creatures = new List<Enemy>();
        private List<Explosion> _explosions = new List<Explosion>();
        private List<Spawner> _spawner = new List<Spawner>();
        private List<SpriteAnimation> _wallSprites;
        private List<Plasma> _plasma = new List<Plasma>();

        private HealthAnimation _healthSprite;
        private Health _health;

        private Texture2D _gameover;
        private Vector2 _gameOverVector;
        private bool isGameOver = false;
        private int _creaturesKilled = 0;
        Stopwatch timer = new Stopwatch();
        TimeSpan initialTime;
        int forcePower = 500;
        Stopwatch animationTimer = new Stopwatch();

        public IfritDemo()
        {
            int test = initialTime.Seconds;
            levelDetails = "Level 1";
            levelName = "Start Game";
            this.backgroundTextureStr = "Materials/ground";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Farseer.Instance.World.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;
            _spawnerSprite = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerSprite2 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerSprite3 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerSprite4 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerSprite.AnimationName = "portal_d_spawning";
            _spawnerSprite2.AnimationName = "portal_d_spawning";
            _spawnerSprite3.AnimationName = "portal_dr_spawning";
            _spawnerSprite4.AnimationName = "portal_dl_spawning";
            _wizard = new WizardAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wizard\\wizard"), new StreamReader(@"Content/Sprites/Wizard/wizard.txt"));
            _wizard.AnimationName = "wizard_d_walk";
            _player = new Player(_wizard, new Vector2(0, 0));
            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _gameover = ScreenManager.Content.Load<Texture2D>("Common\\gameover");

            GenerateWalls();

            this.Camera.EnableTracking = true;
            this.Camera.TrackingBody = _player.getBody().Bodies[0];
        }

        private void GenerateWalls()
        {
            int w_size = 30;
            int h_size = 80;
            float x = -10;
            float y = 5;
            _bricks = new List<Brick>();
            _bricks.Clear();
            _wallSprites = new List<SpriteAnimation>();
            for (int i = 0; i < h_size; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[i], new Vector2(x, y)));
                y -= .7F;
            }
            int j = _wallSprites.Count;
            y = 5;
            for (int i = 0; i < w_size; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j + i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j + i], new Vector2(x, y)));
                x += .7F;
            }
            j = _wallSprites.Count;
            for (int i = 0; i < h_size; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j + i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j + i], new Vector2(x, y)));
                y -= .7F;
            }
            j = _wallSprites.Count;
            for (int i = 0; i < w_size + 1; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j + i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j + i], new Vector2(x, y)));
                x -= .7F;
            }

        }

        public void Spell(Player player, int forcePower)
        {
            Vector2 plasmaPosition = plasmaPosition = new Vector2(0, _player.Position.Y + 2);
            PlasmaAnimation plasmaSprite = new PlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            plasmaSprite.AnimationName = "plasma_d_attack";
            SpriteAnimation animation = (SpriteAnimation)player.SpriteManager;
            Vector2 force = new Vector2();
            if (animation.GetOrientation() == Orientation.Down)
            {
                force = new Vector2(0, forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y + 2);
            }
            else if (animation.GetOrientation() == Orientation.DownLeft)
            {
                force = new Vector2(-forcePower, forcePower);
                plasmaPosition = new Vector2(_player.Position.X - 1, _player.Position.Y + 1);
            }
            else if (animation.GetOrientation() == Orientation.DownRight)
            {
                force = new Vector2(forcePower, forcePower);
                plasmaPosition = new Vector2(_player.Position.X + 1, _player.Position.Y + 1);
            }
            else if (animation.GetOrientation() == Orientation.Left)
            {
                force = new Vector2(-forcePower, 0);
                plasmaPosition = new Vector2(_player.Position.X - 2, _player.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Right)
            {
                force = new Vector2(forcePower, 0);
                plasmaPosition = new Vector2(_player.Position.X + 2, _player.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Up)
            {
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y - 2);
                force = new Vector2(0, -forcePower);
            }
            else if (animation.GetOrientation() == Orientation.UpLeft)
            {
                force = new Vector2(-forcePower, -forcePower);
                plasmaPosition = new Vector2(_player.Position.X - 1, _player.Position.Y - 1);
            }
            else if (animation.GetOrientation() == Orientation.UpRight)
            {
                force = new Vector2(forcePower, -forcePower);
                plasmaPosition = new Vector2(_player.Position.X + 1, _player.Position.Y - 1);
            }
            _plasma.Add(new Plasma(plasmaSprite, plasmaPosition, force));


        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (firstTime == true)
            {
                _spawner.Add(new Spawner(_spawnerSprite, new Vector2(0, -10), _player, 3f, 6f));
                _spawner.Add(new Spawner(_spawnerSprite2, new Vector2(0, -20), _player, 3f, 6f));
                _spawner.Add(new Spawner(_spawnerSprite3, new Vector2(5, -15), _player, 3f, 6f));
                _spawner.Add(new Spawner(_spawnerSprite4, new Vector2(-5, -15), _player, 3f, 6f));
            }
            firstTime = false;
            WizardAnimation wizard = (WizardAnimation)_player.SpriteManager;
            lastGamepadState = gamepadState;
            gamepadState = GamePad.GetState(PlayerIndex.One);
            ExplosionAnimation explosionSprite = new ExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Explosion\\explosion"), new StreamReader(@"Content/Sprites/Explosion/explosion.txt"));
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) || gamepadState.Buttons.X == ButtonState.Pressed)
            {
         
                if (!timer.IsRunning)
                {
                    timer.Start();
                    initialTime = timer.Elapsed;
                }
            }
            for (int i = 0; i < _spawner.Count; i++)
            {
                SpawnerAnimation spawnerAnimation = (SpawnerAnimation)_spawner[i].SpriteManager;
                if (spawnerAnimation.CheckIfrit == true)
                {
                    Orientation orientation;
                    EnemyAnimation _creatureAnimation = new EnemyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                    _creatureAnimation.AnimationName = "ifrit_d_walk";
                    orientation = spawnerAnimation.GetOrientation();
                    if (orientation == Orientation.Down)
                    {
                        //_creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(0, 3), 1.5f, 1.5f));
                    }
                    else if (orientation == Orientation.DownRight)
                    {
                        _creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(-3, 0), 1.5f, 1.5f));
                    }
                    else if (orientation == Orientation.DownLeft)
                    {
                        _creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(3, 0), 1.5f, 1.5f));
                    }
                    
                    
                }
                spawnerAnimation.CheckIfrit = false;
            }

            foreach (Brick b in _bricks)
            {
                b.Update(gameTime);
            }
            if (!isGameOver)
            {
                _player.Update(gameTime);
                _health.Update(gameTime);
            }
            if (_player.Health == HealthAnimation.HealthState.Health0 && isGameOver == false)
            {
                isGameOver = true;
                _gameOverVector = new Vector2((int)ConvertUnits.ToDisplayUnits(_player.Position.X - 8), (int)ConvertUnits.ToDisplayUnits(_player.Position.Y - 5.5F));
            }
            for (int i = 0; i < _creatures.Count; i++)//(Enemy e in _creatures)
            {
                EnemyAnimation enemy = (EnemyAnimation)_creatures[i].SpriteManager;
                AnimationState s = enemy.GetAnimationState();
                if (s == AnimationState.Disentegrated && enemy.PreviousAnimationState == AnimationState.Death)
                {
                    
                    Farseer.Instance.World.RemoveBody(_creatures[i].getBody().Bodies[0]);
                    _creatures.Remove(_creatures[i]);
                    _creaturesKilled = _creaturesKilled++;
                }
                else
                {
                    _creatures[i].Update(gameTime);
                }
            }
            for (int i = 0; i < _explosions.Count; i++)
            {
                ExplosionAnimation explosion = (ExplosionAnimation)_explosions[i].SpriteManager;
                if (explosion.IsDead)
                {
                    _explosions.Remove(_explosions[i]);
                }
                else
                {
                    _explosions[i].Update(gameTime);
                }
            }


            //SpawnerAnimation _spawnerAnimation = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            //_spawnerAnimation.AnimationName = "spawner_d_spawning";
            //_spawner.Add(new Spawner(_spawnerAnimation, new Vector2(0,0)));
            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                EnemyAnimation _creatureAnimation = new EnemyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                _creatureAnimation.AnimationName = "ifrit_d_walk";
                _creatures.Add(new Enemy(_creatureAnimation, _player, ifritPosition, 1.5f, 1.5f));
            }


            if (wizard.CheckEndSpell == true)
            {

                timer.Stop();
                TimeSpan endTime = timer.Elapsed;
                int totalTime = endTime.Seconds - initialTime.Seconds;
                timer.Reset();

                initialTime = timer.Elapsed;
                if (totalTime < 1)
                {
                    forcePower = 500;
                }
                else if (totalTime >= 1 && totalTime < 2)
                {
                    forcePower = 1000;
                }
                else if (totalTime >= 2 && totalTime < 4)
                {
                    forcePower = 2000;
                }
                else if (totalTime >= 4)
                {
                    forcePower = 3000;
                }
                AnimationState s = wizard.GetAnimationState();
                if (!isGameOver)
                {
                    Spell(_player, forcePower);
                }
            }
            for (int i = 0; i < _plasma.Count; i++)//(Plasma p in _plasma)
            {

                if (_plasma[i].IsDead)
                {
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _plasma[i].Position + new Vector2(0, -1), .01f, .01f));
                    Farseer.Instance.World.RemoveBody(_plasma[i].getBody().Bodies[0]);
                    _plasma.Remove(_plasma[i]);
                }
                else if (_plasma[i].IsDeadOnEnemy)
                {
                    Farseer.Instance.World.RemoveBody(_plasma[i].getBody().Bodies[0]);
                    _plasma.Remove(_plasma[i]);
                }
                else
                    _plasma[i].Update(gameTime);
            }


            for (int i = 0; i < _spawner.Count; i++)
            {
                if (_spawner[i].IsDead)
                {
                    explosionSprite.ExplodeOnSpawner = true;
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _spawner[i].Position + new Vector2(-2, -3), .01f, .01f));
                    _explosions[_explosions.Count - 1].SpriteManager.Animations[explosionSprite.AnimationName].Scale = 2f;
                    _explosions[_explosions.Count - 1].Update(gameTime);
                    Farseer.Instance.World.RemoveBody(_spawner[i].getBody().Bodies[0]);
                    _spawner.Remove(_spawner[i]);
                }
                else
                {
                    _spawner[i].Update(gameTime);
                }
            }

                base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            //Draw stuff in here
            foreach (Brick b in _bricks)
            {
                b.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            if (!isGameOver)
            {
                _player.SpriteManager.Draw(ScreenManager.SpriteBatch);
                _health.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Spawner s in _spawner)
            {
                s.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Enemy e in _creatures)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Plasma p in _plasma)
            {
                p.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Explosion e in _explosions)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }

            if (isGameOver)
            {
                ScreenManager.SpriteBatch.Draw(_gameover, _gameOverVector, Color.White);
            }

            //ScreenManager.SpriteBatch.Draw(_gameover, new Rectangle((int)_player.Position.X, (int)_player.Position.X, 400, 266), Color.White);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void MyOnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2)
        {
            ;
        }
    }
}
