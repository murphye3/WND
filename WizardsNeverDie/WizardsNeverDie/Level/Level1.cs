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
        private List<Explosion> _explosions = new List<Explosion>();
        private List<SpriteAnimation> _wallSprites;
        private List<Plasma> _plasma = new List<Plasma>();
        private List<Spawner> _spawner = new List<Spawner>();
        private List<TriggerBody> _triggers = new List<TriggerBody>();

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
            _player = new Player(_wizard, ConvertUnits.ToSimUnits(-(2048/2) + 430, -(2048/2)+135));

            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _gameover = ScreenManager.Content.Load<Texture2D>("Common\\gameover");

            GenerateWalls();
            GenereateCreatures();
            GenereateSpawners();

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
            Body wallMiddle = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(1600), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1159 + (32 / 2), -(2048 / 2) + (1600 / 2) + 416)));
            Body trees1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(64), ConvertUnits.ToSimUnits(196), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1142 + (64 / 2), -(2048 / 2) + (196 / 2) + 220)));
            Body trees2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(384), ConvertUnits.ToSimUnits(368), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1207 + (384 / 2), -(2048 / 2) + (368 / 2) + 230)));
            Body trees3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(329), ConvertUnits.ToSimUnits(598), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1719 + (329 / 2), -(2048 / 2) + (598 / 2) + 0)));
            Body trees4 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(905), ConvertUnits.ToSimUnits(140), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1142 + (905 / 2), -(2048 / 2) + (140 / 2) + 0)));
            Body trees5 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(256), ConvertUnits.ToSimUnits(324), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1207 + (256 / 2), -(2048 / 2) + (324 / 2) + 598)));
            Body trees6 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(513), ConvertUnits.ToSimUnits(1126), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1207 + (513 / 2), -(2048 / 2) + (1126 / 2) + 922)));
            Body trees7 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(329), ConvertUnits.ToSimUnits(721), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1719 + (329 / 2), -(2048 / 2) + (721 / 2) + 1327)));
            Body trees8 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(201), ConvertUnits.ToSimUnits(640), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1847 + (201 / 2), -(2048 / 2) + (640 / 2) + 598)));

            _triggers.Add(new TriggerBody(ConvertUnits.ToSimUnits(552), ConvertUnits.ToSimUnits(50), ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 608 + (552 / 2), -(2048 / 2) + (50 / 2) + 1582)), 1f, _spawner));
        }

        private void GenereateCreatures()
        {
            EnemyAnimation _creatureAnimation = new EnemyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
            _creatureAnimation.AnimationName = "ifrit_d_walk";
            _creatures.Add(new Enemy(_creatureAnimation, _player, ConvertUnits.ToSimUnits(-(2048/2) + 435, -(2048/2) + 740), 1.5f, 1.5f, 10F));
        }

        private void GenereateSpawners()
        {
            SpawnerAnimation _spawnerAnimation = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation.AnimationName = "portal_dr_spawning";
            _spawner.Add(new Spawner(_spawnerAnimation, ConvertUnits.ToSimUnits(-(2048 / 2) + 520, -(2048 / 2) + 1840), _player, 1.5f, 1.5f, true));

            SpawnerAnimation _spawnerAnimation2 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation2.AnimationName = "portal_d_spawning";
            _spawner.Add(new Spawner(_spawnerAnimation2, ConvertUnits.ToSimUnits(-(2048 / 2) + 752, -(2048 / 2) + 1289), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation3 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation3.AnimationName = "portal_d_spawning";
            _spawner.Add(new Spawner(_spawnerAnimation3, ConvertUnits.ToSimUnits(-(2048 / 2) + 1032, -(2048 / 2) + 1032), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation4 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation4.AnimationName = "portal_d_spawning";
            _spawner.Add(new Spawner(_spawnerAnimation4, ConvertUnits.ToSimUnits(-(2048 / 2) + 749, -(2048 / 2) + 791), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation5 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation5.AnimationName = "portal_d_spawning";
            _spawner.Add(new Spawner(_spawnerAnimation5, ConvertUnits.ToSimUnits(-(2048 / 2) + 1034, -(2048 / 2) + 507), _player, 1.5f, 1.5f, false));
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
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _creatures[i].Position + new Vector2(0, -1), .01f, .01f));
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
                        _creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(0, 3), 1.5f, 1.5f, 15F));
                    }
                    else if (orientation == Orientation.DownRight)
                    {
                        _creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(-3, 0), 1.5f, 1.5f, 15F));
                    }
                    else if (orientation == Orientation.DownLeft)
                    {
                        _creatures.Add(new Enemy(_creatureAnimation, _player, _spawner[i].Position + new Vector2(3, 0), 1.5f, 1.5f, 15F));
                    }


                }
                spawnerAnimation.CheckIfrit = false;
            }


            //SpawnerAnimation _spawnerAnimation = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            //_spawnerAnimation.AnimationName = "spawner_d_spawning";
            //_spawner.Add(new Spawner(_spawnerAnimation, new Vector2(0,0)));
            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X))
            {
                EnemyAnimation _creatureAnimation = new EnemyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                _creatureAnimation.AnimationName = "ifrit_d_walk";
                _creatures.Add(new Enemy(_creatureAnimation, _player, ifritPosition, 1.5f, 1.5f, 15F));
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
            for (int i = 0; i < _triggers.Count; i++)
            {
                if (_triggers[i].IsDead)
                {
                    Farseer.Instance.World.RemoveBody(_triggers[i].Bodies[0]);
                    _triggers.Remove(_triggers[i]);
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
