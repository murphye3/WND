using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Level;
using WizardsNeverDie.Entities;
using WizardsNeverDie.Intelligence;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;
using FarseerPhysics.Factories;
using WizardsNeverDie.ScreenSystem;
using WizardsNeverDie.Dialog;
using Microsoft.Xna.Framework.Audio;
using WizardsNeverDie.Trigger;

namespace WizardsNeverDie.Level
{
    class Level1_3 : BaseLevel
    {
        public static Random random;
        private int checkRandom = 0;
        private List<TrippinTrees> _trippinTrees = new List<TrippinTrees>();
        private List<TrippinTreesAnimation> _trippinTreesAnimation = new List<TrippinTreesAnimation>();
        private RangedPurpleIfrit _purple;
        private Wizard _player;
        private WizardAnimation _wizard;
        private List<Brick> _bricks = new List<Brick>();
        private List<MeleeRedIfrit> _creatures = new List<MeleeRedIfrit>();
        private List<RangedPurpleIfrit> _purpleCreatures = new List<RangedPurpleIfrit>();
        private List<Explosion> _explosions = new List<Explosion>();
        private List<SpriteAnimation> _wallSprites;
        private List<WizardPlasma> _plasma = new List<WizardPlasma>();
        private List<RangedPurplePlasma> _purplePlasma = new List<RangedPurplePlasma>();
        private List<Teleporter> _blueTeleporters = new List<Teleporter>();
        private List<Teleporter> _yellowTeleporters = new List<Teleporter>();
        private List<Teleporter> _greenTeleporters = new List<Teleporter>();
        private List<Teleporter> _redTeleporters = new List<Teleporter>();
        private List<Spawner> _spawners = new List<Spawner>();
        private List<TriggerBody> _triggers = new List<TriggerBody>();
        private List<Potions> _potion = new List<Potions>();
        private List<PotionExplosion> _potionExplosion = new List<PotionExplosion>();
        private List<IAction> _actions4 = new List<IAction>();
        private HealthAnimation _healthSprite;
        PotionExplosionAnimation potionExplosionAnimation;
        private Health _health;
        private SpawnIfritAction _spawnIfritAction;
        private MessageBoxScreen _mbs;
        private List<ExplosionAnimation> _explosionAnimation = new List<ExplosionAnimation>();
        private List<Explosion> _spawnedExplosions = new List<Explosion>();
        private SpriteAnimation _potionSprite;
        private WizardPlasmaAnimation _plasmaSprite1;
            
        private List<Vector2> _spawnedExplosionVectors = new List<Vector2>();
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

        public Level1_3()
        {
            random = new Random();
            int test = initialTime.Seconds;
            levelDetails = "Level 1";
            levelName = "Start Game: 1";
            this.backgroundTextureStr = "Materials/Level1_Background";
        }

        public override void LoadContent()
        {
            
            base.LoadContent();
            Farseer.Instance.World.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;
            _wizard = new WizardAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wizard\\wizard"), new StreamReader(@"Content/Sprites/Wizard/wizard.txt"));
            _wizard.AnimationName = "wizard_d_walk";
            _player = new Wizard(_wizard, ConvertUnits.ToSimUnits(-(2048 / 2) + 430, -(2048 / 2) + 135));
            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _gameover = ScreenManager.Content.Load<Texture2D>("Common\\gameover");
            _plasmaSprite1 = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            _plasmaSprite1.AnimationName = "plasma_d_attack";
            GenerateWalls();
            GenereateCreatures();
            GenereateSpawners();
            GeneratePotions();
            GenerateTrees();
            GenerateTeleporters();
            this.Camera.EnableTracking = true;
            this.Camera.TrackingBody = _player.getBody().Bodies[0];
        }
        private void GenerateTrees()
        {

            //for (int i = 600; i < 1800; i++)
            //{

            //    for (int j = 0; j < 1700; j++)
            //    {
            //        if (i % (random.Next(20) + 20) == 0 && j % (random.Next(20) + 20) == 0)
            //        {
            //            _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), (float)random.Next(10) + 10));
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = random.Next(27);
            //            _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + random.Next(1148) + 600, -(2048 / 2) + j + 174))));
            //        }
            //    }
            //}
            //for (int i = 600; i < 1800; i++)
            //{

            //    for (int j = 0; j < 1700; j++)
            //    {
            //        if (i % (random.Next(20) + 20) == 0 && j % (random.Next(20) + 20) == 0)
            //        {
            //            _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), (float)random.Next(10) + 10));
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = random.Next(27);
            //            _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + random.Next(1148) + 600, -(2048 / 2) + j + 174))));
            //        }
            //    }
            //}
            //for (int i = 0; i < 1800; i++)
            //{

            //    //for (int j = 0; j < 1700; j++)
            //    //{
            //    //    if (i % (random.Next(20) + 20) == 0 && j % (random.Next(20) + 20) == 0)
            //    //    {
            //    //        _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), (float)random.Next(10) + 10));
            //    //        checkRandom = random.Next(3);
            //    //        if (checkRandom == 1 || checkRandom == 2)
            //    //        {
            //    //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //    //        }
            //    //        else
            //    //        {
            //    //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttrandomcolors_d_forward";
            //    //        }
            //    //        _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = random.Next(27);
            //    //        _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + random.Next(1348) + 400, -(2048 / 2) + j + 174))));
            //    //        _trippinTrees[_trippinTrees.Count - 1].SpriteManager.Animations[_trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName].Scale = (float)random.NextDouble() * 2 + .5f;
            //    //    }
            //    //}
            //}
        //    for (int i = 50; i < 2048; i++)
        //    {
        //        for (int j = 50; j < 2048; j++)
        //        {
        //            if (i % 100 == 0 && j % 100 == 0)
        //            {
        //                _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), 15f));
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttrandomcolors_d_forward";
        //                _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(1948 / 2) + i, -(2048 / 2) + j) - new Vector2(50,50))));
        //            }
        //        }
        //    }
        //    for (int i = 0; i < 2048; i++)
        //    {
        //        for (int j = 0; j < 2048; j++)
        //        {
        //            if (i % 75 == 0 && j % 75 == 0)
        //            {
        //                _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), 20f));
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = 12;
        //                _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + i, -(2048 / 2) + j))));
        //            }
        //        }
        //    }
        //    for (int i = 0; i < 2048; i++)
        //    {
        //        for (int j = 0; j < 2048; j++)
        //        {
        //            if (i % 90 == 0 && j % 75 == 0)
        //            {
        //                _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), 20f));
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = 5;
        //                _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + i, -(2048 / 2) + j))));
        //            }
        //        }
        //    }
        //    for (int i = 500; i < 1200; i++)
        //    {
        //        for (int j = 500; j < 1200; j++)
        //        {
        //            if (i % 90 == 0 && j % 75 == 0)
        //            {
        //                _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), 20f));
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
        //                _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = 8;
        //                _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + i, -(2048 / 2) + j))));
        //            }
        //        }
        //    }
            //_trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), 5f));
            //_trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //_trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = 0;
            //_trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 800, -(2048 / 2) + 400))));
            //for (int i = 600; i < 1800; i++)
            //{

            //    for (int j = 0; j < 1700; j++)
            //    {
            //        if (i % 80 == 0 && j % 100 == 0)
            //        {
            //            _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), (float)random.Next(15) + 5));
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = random.Next(12);
            //            _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + i, -(2048 / 2) + j + 174))));
            //        }
            //    }
            //}
            //for (int i = 600; i < 1800; i++)
            //{

            //    for (int j = 0; j < 1700; j++)
            //    {
            //        if (i % 90 == 0 && j % 120 == 0)
            //        {
            //            _trippinTreesAnimation.Add(new TrippinTreesAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\TrippinTrees\\trippintrees"), new StreamReader(@"Content/Sprites/TrippinTrees/trippintrees.txt"), (float)random.Next(15) + 5));
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].AnimationName = "ttcolorspectrum_d_forward";
            //            _trippinTreesAnimation[_trippinTreesAnimation.Count - 1].FrameIndex = random.Next(12);
            //            _trippinTrees.Add(new TrippinTrees(_trippinTreesAnimation[_trippinTreesAnimation.Count - 1], ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + i, -(2048 / 2) + j + 174))));
            //        }
            //    }
            //}
        }
        private void GenerateWalls()
        {
            
        }

        private void GenereateCreatures()
        {

            
        }

        private void GenereateSpawners()
        {
            
        }

        private void GeneratePotions()
        {
        }

        private void GenerateTeleporters()
        {
            TeleporterAnimation teleporterAnimation = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation.AnimationName = "blueteleporter_d_open";
            _blueTeleporters.Add(new Teleporter(teleporterAnimation, _player.Position + new Vector2(-5, -5), 1f, 1f));

            TeleporterAnimation teleporterAnimation2 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation2.AnimationName = "blueteleporter_d_open";
            _blueTeleporters.Add(new Teleporter(teleporterAnimation2, _player.Position + new Vector2(-15, -5), 1f, 1f));

            TeleporterAnimation teleporterAnimation3 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation3.AnimationName = "redteleporter_d_open";
            _redTeleporters.Add(new Teleporter(teleporterAnimation3, _player.Position + new Vector2(20, 10), 1f, 1f));
            
            TeleporterAnimation teleporterAnimation4 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation4.AnimationName = "redteleporter_d_open";
            _redTeleporters.Add(new Teleporter(teleporterAnimation4, _player.Position + new Vector2(80, 20), 1f, 1f));

            TeleporterAnimation teleporterAnimation5 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation5.AnimationName = "greenteleporter_d_open";
            _greenTeleporters.Add(new Teleporter(teleporterAnimation5, _player.Position + new Vector2(30, 60), 1f, 1f));

            TeleporterAnimation teleporterAnimation6 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation6.AnimationName = "greenteleporter_d_open";
            _greenTeleporters.Add(new Teleporter(teleporterAnimation6, _player.Position + new Vector2(100, 40), 1f, 1f));

            TeleporterAnimation teleporterAnimation7 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation7.AnimationName = "yellowteleporter_d_open";
            _yellowTeleporters.Add(new Teleporter(teleporterAnimation7, _player.Position + new Vector2(10, 40), 1f, 1f));

            TeleporterAnimation teleporterAnimation8 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation8.AnimationName = "yellowteleporter_d_open";
            _yellowTeleporters.Add(new Teleporter(teleporterAnimation8, _player.Position + new Vector2(100, 20), 1f, 1f));
        }

        public void PotionExplosion(Wizard player)
        {
            
        }

        public void Spell(Wizard player, int forcePower)
        {
            Vector2 plasmaPosition = plasmaPosition = new Vector2(0, _player.Position.Y + 2);
            WizardPlasmaAnimation plasmaSprite = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
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
            _plasma.Add(new WizardPlasma(plasmaSprite, plasmaPosition, force));
        }

        public void PurpleSpell(RangedPurpleIfrit enemy, int forcePower)
        {
            forcePower = 500;
            Vector2 plasmaPosition = plasmaPosition = new Vector2(0, _player.Position.Y + 2);
            WizardPlasmaAnimation plasmaSprite = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            plasmaSprite.AnimationName = "plasma_d_attack";
            SpriteAnimation animation = (SpriteAnimation)enemy.SpriteManager;
            Vector2 force = new Vector2();
            //for (int i = 0; i < _purpleCreatures.Count; i++)
            //{
            if (animation.GetOrientation() == Orientation.Down)
            {
                force = new Vector2(0, forcePower);
                plasmaPosition = new Vector2(enemy.Position.X, enemy.Position.Y + 2);
            }
            else if (animation.GetOrientation() == Orientation.DownLeft)
            {
                force = new Vector2(-forcePower, forcePower);
                plasmaPosition = new Vector2(enemy.Position.X - 1, enemy.Position.Y + 1);
            }
            else if (animation.GetOrientation() == Orientation.DownRight)
            {
                force = new Vector2(forcePower, forcePower);
                plasmaPosition = new Vector2(enemy.Position.X + 1, enemy.Position.Y + 1);
            }
            else if (animation.GetOrientation() == Orientation.Left)
            {
                force = new Vector2(-forcePower, 0);
                plasmaPosition = new Vector2(enemy.Position.X - 2, enemy.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Right)
            {
                force = new Vector2(forcePower, 0);
                plasmaPosition = new Vector2(enemy.Position.X + 2, enemy.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Up)
            {
                plasmaPosition = new Vector2(enemy.Position.X, enemy.Position.Y - 2);
                force = new Vector2(0, -forcePower);
            }
            else if (animation.GetOrientation() == Orientation.UpLeft)
            {
                force = new Vector2(-forcePower, -forcePower);
                plasmaPosition = new Vector2(enemy.Position.X - 1, enemy.Position.Y - 1);
            }
            else if (animation.GetOrientation() == Orientation.UpRight)
            {
                force = new Vector2(forcePower, -forcePower);
                plasmaPosition = new Vector2(enemy.Position.X + 1, enemy.Position.Y - 1);
            }
            _purplePlasma.Add(new RangedPurplePlasma(plasmaSprite, plasmaPosition, force));
            //}


        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            WizardAnimation wizard = (WizardAnimation)_player.SpriteManager;
            lastGamepadState = gamepadState;
            gamepadState = GamePad.GetState(PlayerIndex.One);
            ExplosionAnimation explosionSprite = new ExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Explosion\\explosion"), new StreamReader(@"Content/Sprites/Explosion/explosion.txt"), 12f);
            ExplosionAnimation metalSlugExplosionSprite =
                new ExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Explosion\\metalslugexplosion"), new StreamReader(@"Content/Sprites/Explosion/metalslugexplosion.txt"), 12f);
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
            if (_player.Health == HealthAnimation.HealthState.Health0 && !isGameOver && !wizard.CheckEndRevive)
            {
                wizard.SetAnimationState(AnimationState.Revived);
                _player.getBody().Bodies[0].Position = (ConvertUnits.ToSimUnits(-(2048 / 2) + 430, -(2048 / 2) + 135));

                _player.Update(gameTime);

            }
            if (wizard.CheckEndRevive)
            {
                _player.Health = HealthAnimation.HealthState.Health100;
                _health.Update(gameTime);
                wizard.SetAnimationState(AnimationState.Walk);
                _player.Update(gameTime);
            }
            for (int i = 0; i < _triggers.Count; i++)
            {
                if (_triggers[i].IsDead == true)
                {
                    for (int j = 0; j < _triggers[i].ActionList.Count; j++)
                    {
                        if (_triggers[i].ActionList[j].IsDead == false)
                            _triggers[i].ActionList[j].Update(gameTime);
                    }
                }
            }

            for (int i = 0; i < _creatures.Count; i++)//(Enemy e in _creatures)
            {
                MeleeRedIfritAnimation enemy = (MeleeRedIfritAnimation)_creatures[i].SpriteManager;
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
            for (int i = 0; i < _purpleCreatures.Count; i++)//(Enemy e in _creatures)
            {
                RangedPurpleIfritAnimation enemy = (RangedPurpleIfritAnimation)_purpleCreatures[i].SpriteManager;
                AnimationState s = enemy.GetAnimationState();
                if (s == AnimationState.Disentegrated && enemy.PreviousAnimationState == AnimationState.Death)
                {
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _purpleCreatures[i].Position + new Vector2(0, -1), .01f, .01f));
                    Farseer.Instance.World.RemoveBody(_purpleCreatures[i].getBody().Bodies[0]);
                    _purpleCreatures.Remove(_purpleCreatures[i]);
                    _creaturesKilled = _creaturesKilled++;
                }
                else if (enemy.CheckEndSpell == true)
                {
                    PurpleSpell(_purpleCreatures[i], 500);
                    _purpleCreatures[i].Update(gameTime);
                }
                else
                {
                    _purpleCreatures[i].Update(gameTime);
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
            for (int i = 0; i < _spawnedExplosions.Count; i++)
            {
                ExplosionAnimation explosion = (ExplosionAnimation)_spawnedExplosions[i].SpriteManager;
                if (explosion.IsDead)
                {
                    _spawnedExplosions.Remove(_spawnedExplosions[i]);
                }
                else
                {
                    _spawnedExplosions[i].Update(gameTime);
                }
            }
            if (_mbs != null)
            {
                _mbs.TextPosition = new Vector2(ScreenManager.Game.Window.ClientBounds.Width / 2, ScreenManager.Game.Window.ClientBounds.Height - 200);
            }
            for (int i = 0; i < _spawners.Count; i++)
            {
                SpawnerAnimation spawnerAnimation = (SpawnerAnimation)_spawners[i].SpriteManager;
                if (spawnerAnimation.CheckIfrit == true)
                {
                    Orientation orientation;
                    MeleeRedIfritAnimation _creatureAnimation = new MeleeRedIfritAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                    _creatureAnimation.AnimationName = "ifrit_d_walk";
                    orientation = spawnerAnimation.GetOrientation();
                    if (orientation == Orientation.Down)
                    {
                        _creatures.Add(new MeleeRedIfrit(_creatureAnimation, _player, _spawners[i].Position + new Vector2(0, 3), 1.5f, 1.5f, 15F));
                    }
                    else if (orientation == Orientation.DownRight)
                    {
                        _creatures.Add(new MeleeRedIfrit(_creatureAnimation, _player, _spawners[i].Position + new Vector2(-3, 0), 1.5f, 1.5f, 15F));
                    }
                    else if (orientation == Orientation.DownLeft)
                    {
                        _creatures.Add(new MeleeRedIfrit(_creatureAnimation, _player, _spawners[i].Position + new Vector2(3, 0), 1.5f, 1.5f, 15F));
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
                MeleeRedIfritAnimation _creatureAnimation = new MeleeRedIfritAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
                _creatureAnimation.AnimationName = "ifrit_d_walk";
                _creatures.Add(new MeleeRedIfrit(_creatureAnimation, _player, ifritPosition, 1.5f, 1.5f, 15F));
            }
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Y) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
            {
                _mbs = new MessageBoxScreen("TESTTESTTEST");
                _mbs.ScreenManager = this.ScreenManager;
                SpriteFont font = ScreenManager.Fonts.DetailsFont;
                _mbs.TextSize = font.MeasureString("TESTTESTTEST");
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


            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Q) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q) && potionCount > 0)
            {
                PotionExplosion(_player);
                _potionExplosion.Add(new PotionExplosion(potionExplosionAnimation, _player, _creatures, _player.Position));
                potionCount--;
            }
            for (int i = 0; i < _potionExplosion.Count; i++)
            {
                PotionExplosionAnimation potionExplosionAnim = (PotionExplosionAnimation)_potionExplosion[i].SpriteManager;
                if (potionExplosionAnim.IsDead)
                {
                    _potionExplosion.Remove(_potionExplosion[i]);
                }
                else
                {
                    _potionExplosion[i].Update(gameTime);
                }
            }
            for (int i = 0; i < _trippinTrees.Count; i++)
            {
                _trippinTrees[i].Update(gameTime);
            }
            for (int i = 0; i < _blueTeleporters.Count; i++)
            {
                if (_blueTeleporters[0].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _blueTeleporters[1].Position + new Vector2(0, 3);
                    _blueTeleporters[0].Collided = false;
                }
                if (_blueTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _blueTeleporters[0].Position + new Vector2(0, 3);
                    _blueTeleporters[1].Collided = false;
                }
                if (_blueTeleporters[0].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _blueTeleporters[1].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _blueTeleporters[0].PlasmaCollided = false;
                }
                if (_blueTeleporters[1].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _blueTeleporters[0].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _blueTeleporters[1].PlasmaCollided = false;
                }
                _blueTeleporters[i].Update(gameTime);
            }

            for (int i = 0; i < _redTeleporters.Count; i++)
            {
                if (_redTeleporters[0].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _redTeleporters[1].Position + new Vector2(0, 3);
                    _redTeleporters[0].Collided = false;
                }
                else if (_redTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _redTeleporters[0].Position + new Vector2(0, 3);
                    _redTeleporters[1].Collided = false;
                }
                if (_redTeleporters[0].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _redTeleporters[1].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _redTeleporters[0].PlasmaCollided = false;
                }
                if (_redTeleporters[1].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _redTeleporters[0].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _redTeleporters[1].PlasmaCollided = false;
                }
                _redTeleporters[i].Update(gameTime);
            }
            for (int i = 0; i < _yellowTeleporters.Count; i++)
            {
                if (_yellowTeleporters[0].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _yellowTeleporters[1].Position + new Vector2(0, 3);
                    _yellowTeleporters[0].Collided = false;
                }
                else if (_yellowTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _yellowTeleporters[0].Position + new Vector2(0, 3);
                    _yellowTeleporters[1].Collided = false;
                }
                if (_yellowTeleporters[0].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _yellowTeleporters[1].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _yellowTeleporters[0].PlasmaCollided = false;
                }
                if (_yellowTeleporters[1].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _yellowTeleporters[0].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _yellowTeleporters[1].PlasmaCollided = false;
                }
                _yellowTeleporters[i].Update(gameTime);
            }
            for (int i = 0; i < _greenTeleporters.Count; i++)
            {
                if (_greenTeleporters[0].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _greenTeleporters[1].Position + new Vector2(0, 3);
                    _greenTeleporters[0].Collided = false;
                }
                else if (_greenTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _greenTeleporters[0].Position + new Vector2(0, 3);
                    _greenTeleporters[1].Collided = false;
                }
                if (_greenTeleporters[0].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _greenTeleporters[1].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _greenTeleporters[0].PlasmaCollided = false;
                }
                if (_greenTeleporters[1].PlasmaCollided == true)
                {
                    _plasma.Add(new WizardPlasma(new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt")), _greenTeleporters[0].Position + new Vector2(0, 1.1f), new Vector2(0, forcePower)));
                    _plasma[_plasma.Count - 1].SpriteManager.AnimationName = "plasma_d_attack";
                    _greenTeleporters[1].PlasmaCollided = false;
                }
                _greenTeleporters[i].Update(gameTime);
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
            for (int i = 0; i < _purplePlasma.Count; i++)//(Plasma p in _plasma)
            {
                if (_purplePlasma[i].IsDead)
                {
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _purplePlasma[i].Position + new Vector2(0, -1), .01f, .01f));
                    Farseer.Instance.World.RemoveBody(_purplePlasma[i].getBody().Bodies[0]);
                    _purplePlasma.Remove(_purplePlasma[i]);
                }
                else if (_purplePlasma[i].IsDeadOnEnemy)
                {
                    Farseer.Instance.World.RemoveBody(_purplePlasma[i].getBody().Bodies[0]);
                    _purplePlasma.Remove(_purplePlasma[i]);
                }
                else
                    _purplePlasma[i].Update(gameTime);
            }
            for (int i = 0; i < _spawners.Count; i++)
            {
                if (_spawners[i].IsDead)
                {
                    metalSlugExplosionSprite.ExplodeOnSpawner = true;
                    metalSlugExplosionSprite.AnimationName = "metalslug_d_explosion";
                    _explosions.Add(new Explosion(metalSlugExplosionSprite, _spawners[i].Position + new Vector2(-2, -5), .01f, .01f));
                    _explosions[_explosions.Count - 1].SpriteManager.Animations[metalSlugExplosionSprite.AnimationName].Scale = 2f;
                    _explosions[_explosions.Count - 1].Update(gameTime);
                    Farseer.Instance.World.RemoveBody(_spawners[i].getBody().Bodies[0]);
                    _spawners.Remove(_spawners[i]);
                }
                else
                {
                    _spawners[i].Update(gameTime);
                }
            }

            //SWITCH NEXT LEVEL

            for (int i = 0; i < _triggers.Count; i++)
            {
                bool removeTrigger = true;
                for (int j = 0; j < _triggers[i].ActionList.Count; j++)
                {
                    if (_triggers[i].ActionList[j].IsDead == false)
                    {
                        removeTrigger = false;
                    }
                }
                if (removeTrigger)
                {
                    Farseer.Instance.World.RemoveBody(_triggers[i].Bodies[0]);
                    _triggers.Remove(_triggers[i]);
                }
            }
            for (int i = 0; i < _potion.Count; i++)
            {
                if (_potion[i].IsCollected == true)
                {
                    potionCount++;
                    Farseer.Instance.World.RemoveBody(_potion[i].getBody().Bodies[0]);
                    _potion.Remove(_potion[i]);
                }
                else
                {
                    _potion[i].Update(gameTime);
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
            foreach (Teleporter t in _blueTeleporters)
            {
                t.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Teleporter t in _redTeleporters)
            {
                t.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Teleporter t in _greenTeleporters)
            {
                t.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Teleporter t in _yellowTeleporters)
            {
                t.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Potions s in _potion)
            {
                s.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Spawner s in _spawners)
            {
                s.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (MeleeRedIfrit e in _creatures)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (RangedPurpleIfrit e in _purpleCreatures)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (WizardPlasma p in _plasma)
            {
                p.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (RangedPurplePlasma p in _purplePlasma)
            {
                p.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (PotionExplosion pE in _potionExplosion)
            {
                pE.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            
            if (!isGameOver)
            {
                _player.SpriteManager.Draw(ScreenManager.SpriteBatch);
                _health.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Explosion e in _explosions)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (Explosion e in _spawnedExplosions)
            {
                e.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            foreach (TrippinTrees t in _trippinTrees)
            {
                t.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }

            //if (isGameOver)
            //{
            //    ScreenManager.SpriteBatch.Draw(_gameover, _gameOverVector, Color.White);
            //}


            //ScreenManager.SpriteBatch.Draw(_gameover, new Rectangle((int)_player.Position.X, (int)_player.Position.X, 400, 266), Color.White);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            for (int i = 0; i < _triggers.Count; i++)
            {
                if (_triggers[i].IsDead == true)
                {
                    for (int j = 0; j < _triggers[i].ActionList.Count; j++)
                    {
                        _triggers[i].ActionList[j].Draw();
                    }
                }
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void MyOnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2)
        {
            ;
        }
    }
}
