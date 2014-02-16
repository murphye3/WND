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
using FarseerPhysics.Factories;

namespace WizardsNeverDie.Level
{
    class Level1 : BaseLevel
    {

        private Player _player;
        private WizardAnimation _wizard;
        private List<Brick> _bricks = new List<Brick>();
        private List<Enemy> _creatures = new List<Enemy>();
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
        TimeSpan animationTimeSpan;
        int forcePower = 500;
        Stopwatch animationTimer = new Stopwatch();
        Boolean animationFinished = true;

        public Level1()
        {
            int test = initialTime.Seconds;
            levelDetails = "Level 0";
            levelName = "Start Game: 0";
            this.backgroundTextureStr = "Materials/Level1_0";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Farseer.Instance.World.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;
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
            World world = Farseer.Instance.World;
            Body wallLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(2048), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 16, 0)));
            Body wallRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(2048), 1f, ConvertUnits.ToSimUnits(new Vector2((2048 / 2) - 16, 0)));
            Body wallTop = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2048), ConvertUnits.ToSimUnits(32), 1f, ConvertUnits.ToSimUnits(new Vector2(0, -(2048 / 2) + 16)));
            Body wallBottom = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2048), ConvertUnits.ToSimUnits(32), 1f, ConvertUnits.ToSimUnits(new Vector2(0, (2048 / 2) - 16)));
            Body wall1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(1600), 1f, ConvertUnits.ToSimUnits(new Vector2(-(1024 - 576 - 16), -((2048 - 1600) / 2) + 32)));
            Body houseTop = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(544), ConvertUnits.ToSimUnits(81), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 32 + (544 / 2), -(2048 / 2) + (81 / 2) + 32)));
            Body bridgeRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(59), ConvertUnits.ToSimUnits(90), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 517 + (59 / 2), -(2048 / 2) + (89 / 2) + 1001)));
            Body bridgeLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(373), ConvertUnits.ToSimUnits(90), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 32 + (373 / 2), -(2048 / 2) + (89 / 2) + 1001)));
            Body railingLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(208), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 405 + (16 / 2), -(2048 / 2) + (208 / 2) + 939)));
            Body railingRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(208), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 501 + (16 / 2), -(2048 / 2) + (208 / 2) + 939)));
            Body houseWall1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(434), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 32 + (16 / 2), -(2048 / 2) + (434 / 2) + 32)));
            Body houseWall2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(434), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 560 + (16 / 2), -(2048 / 2) + (434 / 2) + 32)));
            Body houseWall3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(64), ConvertUnits.ToSimUnits(161), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 272 + (64 / 2), -(2048 / 2) + (161 / 2) + 32)));
            Body houseWall4 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(81), ConvertUnits.ToSimUnits(31), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 32 + (81 / 2), -(2048 / 2) + (31 / 2) + 466)));
            Body houseWall5 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(368), ConvertUnits.ToSimUnits(31), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 208 + (368 / 2), -(2048 / 2) + (31 / 2) + 466)));
            Body houseWall6 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(64), ConvertUnits.ToSimUnits(160), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 272 + (64 / 2), -(2048 / 2) + (160 / 2) + 305)));
            Body houseWall7 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(87), ConvertUnits.ToSimUnits(57), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 116 + (87 / 2), -(2048 / 2) + (57 / 2) + 286)));
        }

        public void Spell(Player player, int forcePower)
        {
            animationFinished = false;
            Thread.Sleep(400);
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
                force = new Vector2(0, forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y + 2);
            }
            else if (animation.GetOrientation() == Orientation.DownRight)
            {
                force = new Vector2(0, forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y + 2);
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
                force = new Vector2(0, -forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y - 2);
            }
            else if (animation.GetOrientation() == Orientation.UpRight)
            {
                force = new Vector2(0, -forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y - 2);
            }
            _plasma.Add(new Plasma(plasmaSprite, plasmaPosition, force));


        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!animationTimer.IsRunning)
            {
                animationTimer.Start();
            }

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                if (!timer.IsRunning)
                {
                    timer.Start();
                    initialTime = timer.Elapsed;
                }
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
                if (_creatures[i].IsDead)
                {
                    Farseer.Instance.World.RemoveBody(_creatures[i].getBody().Bodies[0]);
                    _creatures.Remove(_creatures[i]);
                    _creaturesKilled = _creaturesKilled++;
                }
                else
                    _creatures[i].Update(gameTime);
            }

            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                EnemyAnimation _creatureAnimation = new EnemyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                _creatureAnimation.AnimationName = "ifrit_d_walk";
                _creatures.Add(new Enemy(_creatureAnimation, _player, ifritPosition, 1.5F, 1.5F));
            }
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Thread thread = new Thread(() => Spell(_player, forcePower));

                animationTimeSpan = animationTimer.Elapsed;
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
                if (!isGameOver)
                {

                    if (animationTimeSpan.TotalMilliseconds > 908 || animationFinished == true)
                    {
                        thread.Start();
                        animationTimer.Stop();
                        animationTimer.Reset();
                        animationTimeSpan = animationTimer.Elapsed;
                    }
                }
            }
            for (int i = 0; i < _plasma.Count; i++)//(Plasma p in _plasma)
            {
                if (_plasma[i].IsDead)
                {
                    Farseer.Instance.World.RemoveBody(_plasma[i].getBody().Bodies[0]);
                    _plasma.Remove(_plasma[i]);
                }
                else
                    _plasma[i].Update(gameTime);
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
            foreach (Enemy e in _creatures)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Plasma p in _plasma)
            {
                p.SpriteManager.Draw(ScreenManager.SpriteBatch);
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
