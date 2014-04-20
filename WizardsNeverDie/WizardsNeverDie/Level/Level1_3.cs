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
        private bool _openBecauseISaySo;
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
        private List<Switcher> _switch = new List<Switcher>();
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
        private List<List<TriggerBody>> _allTriggers = new List<List<TriggerBody>>();
        private List<TriggerBody> _checkGate = new List<TriggerBody>();
        private SpriteManager _topOfLog;
        private List<Gate> _redGates = new List<Gate>();
        GateAnimation redGateAnimation;
        private Body _blockGate;
        World world = Farseer.Instance.World;
        private List<TriggerBody> _triggers2 = new List<TriggerBody>();
        private List<IAction> _actions = new List<IAction>();
        private List<IAction> _actions2 = new List<IAction>();
        private List<Key> _greenKey = new List<Key>();
        private List<Key> _fakeGreenKey = new List<Key>();
        private KeyAnimation keyAnimation;
        public Level1_3()
        {
            random = new Random();
            int test = initialTime.Seconds;
            levelDetails = "Level 3";
            levelName = "Level: 3";
            this.backgroundTextureStr = "Materials/Level1_3";
        }

        public override void LoadContent()
        {
            _topOfLog = new SpriteAnimation(ScreenManager.Content.Load<Texture2D>(("Materials\\MapSprites\\TopLog")), new StreamReader(@"Content/Materials/MapSprites/TopLog.txt"));
            _topOfLog.AnimationName = "toplog_d_yolo";

            base.LoadContent();
            Farseer.Instance.World.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;
            _wizard = new WizardAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wizard\\wizard"), new StreamReader(@"Content/Sprites/Wizard/wizard.txt"));
            _wizard.AnimationName = "wizard_dr_walk";
            _player = new Wizard(_wizard, ConvertUnits.ToSimUnits(-(2048 / 2) + 80 + (50 / 2), -(2048 / 2) + 990 + (50 / 2)), _plasma);
            _topOfLog.Position = _player.Position + new Vector2(64.8f, .15f);
            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _gameover = ScreenManager.Content.Load<Texture2D>("Common\\gameover");
            _plasmaSprite1 = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            _plasmaSprite1.AnimationName = "plasma_d_attack";
            GenerateSwitches();
            GenerateWalls();
            GenereateCreatures();
            GenereateSpawners();
            GeneratePotions();
            GenerateTrees();
            GenerateTeleporters();
            GenerateGates();
            GenerateKeys();
            this.Camera.EnableTracking = true;
            this.Camera.TrackingBody = _player.getBody().Bodies[0];
        }
        private void GenerateSwitches()
        {
            SwitcherAnimation switcherAnimation = new SwitcherAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Switch\\switch"), new StreamReader(@"Content/Sprites/Switch/switch.txt"), 1f);
            switcherAnimation.AnimationName = "switch_d_off";
            SwitcherAnimation switcherAnimation1 = new SwitcherAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Switch\\switch"), new StreamReader(@"Content/Sprites/Switch/switch.txt"), 1f);
            switcherAnimation1.AnimationName = "switch_d_off";
            SwitcherAnimation switcherAnimation2 = new SwitcherAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Switch\\switch"), new StreamReader(@"Content/Sprites/Switch/switch.txt"), 1f);
            switcherAnimation2.AnimationName = "switch_d_off";
            SwitcherAnimation switcherAnimation3 = new SwitcherAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Switch\\switch"), new StreamReader(@"Content/Sprites/Switch/switch.txt"), 1f);
            switcherAnimation3.AnimationName = "switch_d_off";
            _switch.Add(new Switcher(switcherAnimation, ConvertUnits.ToSimUnits(-(2048 / 2) + 200 + (50 / 2), -(2048 / 2) + 755 + (50 / 2))));//yellow swithc, first switch
            _switch.Add(new Switcher(switcherAnimation3, ConvertUnits.ToSimUnits(-(2048 / 2) + 1028 + (50 / 2), -(2048 / 2) + 1265 + (50 / 2))));//blue switch, second switch
            _switch.Add(new Switcher(switcherAnimation1, ConvertUnits.ToSimUnits(-(2048 / 2) + 1020 + (50 / 2), -(2048 / 2) + 755 + (50 / 2))));//green switch, third switch
            _switch.Add(new Switcher(switcherAnimation2, ConvertUnits.ToSimUnits(-(2048 / 2) + 197 + (50 / 2), -(2048 / 2) + 1265 + (50 / 2))));//red switch, fourth switch
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
            World world = Farseer.Instance.World;
            Body bottomLeftTrees1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(444), ConvertUnits.ToSimUnits(85), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (444 / 2), -(2048 / 2) + (85 / 2) + 1066)));
            Body bottomLeftTrees2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(66), ConvertUnits.ToSimUnits(263), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 378 + (66 / 2), -(2048 / 2) + (263 / 2) + 1150)));
            Body bottomLeftTrees3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(60), ConvertUnits.ToSimUnits(287), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (60 / 2), -(2048 / 2) + (287 / 2) + 1130)));
            Body topLeftTrees1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(375), ConvertUnits.ToSimUnits(81), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (375 / 2), -(2048 / 2) + (81 / 2) + 900)));
            Body topLeftTrees2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(72), ConvertUnits.ToSimUnits(335), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 374 + (72 / 2), -(2048 / 2) + (335 / 2) + 611)));
            Body topLeftTrees3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(67), ConvertUnits.ToSimUnits(287), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (67 / 2), -(2048 / 2) + (287 / 2) + 612)));
            Body bottomRighTrees1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(67), ConvertUnits.ToSimUnits(350), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 825 + (67 / 2), -(2048 / 2) + (350 / 2) + 1065)));
            Body bottomRightTrees2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(316), ConvertUnits.ToSimUnits(82), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 893 + (316 / 2), -(2048 / 2) + (82 / 2) + 1069)));
            Body bottomRightTrees3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(447), ConvertUnits.ToSimUnits(352), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1210 + (447 / 2), -(2048 / 2) + (352 / 2) + 1065)));
            Body topRightTrees1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(67), ConvertUnits.ToSimUnits(361), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 823 + (67 / 2), -(2048 / 2) + (361 / 2) + 606)));
            Body topRightTrees2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(311), ConvertUnits.ToSimUnits(78), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 891 + (311 / 2), -(2048 / 2) + (78 / 2) + 901)));
            Body topRightTrees3 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(456), ConvertUnits.ToSimUnits(356), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1200 + (456 / 2), -(2048 / 2) + (356 / 2) + 612)));
            Body yellowTree = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(65), ConvertUnits.ToSimUnits(78), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 475 + (65 / 2), -(2048 / 2) + (78 / 2) + 667)));
            Body greenTree = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(65), ConvertUnits.ToSimUnits(78), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 729 + (65 / 2), -(2048 / 2) + (78 / 2) + 665)));
            Body redTree = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(65), ConvertUnits.ToSimUnits(78), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 479 + (65 / 2), -(2048 / 2) + (78 / 2) + 1267)));
            Body blueTree = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(65), ConvertUnits.ToSimUnits(78), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 731 + (65 / 2), -(2048 / 2) + (78 / 2) + 1270)));
            Body statue1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(54), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 619 + (32 / 2), -(2048 / 2) + (54 / 2) + 728)));
            Body statue2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(54), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 619 + (32 / 2), -(2048 / 2) + (54 / 2) + 1240)));
            Body bottomTrees = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2050), ConvertUnits.ToSimUnits(634), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (2050 / 2), -(2048 / 2) + (634 / 2) + 1414)));
            Body topTrees = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2050), ConvertUnits.ToSimUnits(308), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (2050 / 2), -(2048 / 2) + (308 / 2) + 316)));
            PlasmaWall rock = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 757 + (34 / 2), -(2048 / 2) + (32 / 2) + 1148)), ConvertUnits.ToSimUnits(34), ConvertUnits.ToSimUnits(32));
            PlasmaWall rock2 = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1747 + (34 / 2), -(2048 / 2) + (32 / 2) + 916)), ConvertUnits.ToSimUnits(34), ConvertUnits.ToSimUnits(32));
            PlasmaWall rock3 = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1810 + (34 / 2), -(2048 / 2) + (32 / 2) + 1087)), ConvertUnits.ToSimUnits(34), ConvertUnits.ToSimUnits(32));
            PlasmaWall rockWall = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1898 + (34 / 2), -(2048 / 2) + (32 / 2) + 890)), ConvertUnits.ToSimUnits(34), ConvertUnits.ToSimUnits(32));
            Body topLog = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(87), ConvertUnits.ToSimUnits(30), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1610 + (87 / 2), -(2048 / 2) + (30 / 2) + 963)));
            Body bottomLog = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(84), ConvertUnits.ToSimUnits(36), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1612 + (84 / 2), -(2048 / 2) + (36 / 2) + 1047)));
            Body topTempleWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(381), ConvertUnits.ToSimUnits(19), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1654 + (381 / 2), -(2048 / 2) + (19 / 2) + 862)));
            Body bottomTempleWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(381), ConvertUnits.ToSimUnits(19), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1654 + (381 / 2), -(2048 / 2) + (19 / 2) + 1162)));
            BouncingWall bouncingTempleWall = new BouncingWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 2033 + (20 / 2), -(2048 / 2) + (315 / 2) + 863)), ConvertUnits.ToSimUnits(20), ConvertUnits.ToSimUnits(315));
            _checkGate.Add(new TriggerBody(1f, 4f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1400 + (16 / 2), -(2048 / 2) + (227 / 2) + 900)), 1f, _plasma));
            MessageAction wAction = new MessageAction(ScreenManager, ScreenManager.Content.Load<Texture2D>(@"Avatar\WizardAvatar"), "conversation8.xml");
            _actions.Add(wAction);
            MessageAction nAction = new MessageAction(ScreenManager, ScreenManager.Content.Load<Texture2D>(@"Avatar\ScrollAvatar"), "conversation9.xml");
            _actions2.Add(nAction);
            _triggers2.Add(new TriggerBody(1f, 4f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1600 + (16 / 2), -(2048 / 2) + (227 / 2) + 900)), 1f, _actions2, _plasma));
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
        private void GenerateGates()
        {
            redGateAnimation = new GateAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Gates\\gates"), new StreamReader(@"Content/Sprites/Gates/gates.txt"), 1f);
            redGateAnimation.AnimationName = "redgate_dl_lock";
            _redGates.Add(new Gate(redGateAnimation, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1270 + (16 / 2), -(2048 / 2) + (227 / 2) + 922)), 1f, 4f, .75f));
            _redGates[_redGates.Count - 1].SpriteManager.Animations[redGateAnimation.AnimationName].Scale = .75f;
            _redGates[_redGates.Count - 1].SpriteManager.Animations["redgate_dl_open"].Scale = .75f;
            _redGates[_redGates.Count - 1].SpriteManager.Animations["redgate_dl_close"].Scale = .75f;
        }

        private void GenerateTeleporters()
        {
            TeleporterAnimation teleporterAnimation = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation.AnimationName = "standardteleporter_d_open";
            _blueTeleporters.Add(new Teleporter(teleporterAnimation, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1140 + (50 / 2), -(2048 / 2) + (50 / 2) + 1290)), 1f, 1f));

            TeleporterAnimation teleporterAnimation2 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation2.AnimationName = "blueteleporter_d_open";
            _blueTeleporters.Add(new Teleporter(teleporterAnimation2, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 477 + (50 / 2), -(2048 / 2) + (50 / 2) + 850)), 1f, 1f));

            TeleporterAnimation teleporterAnimation3 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation3.AnimationName = "standardteleporter_d_open";
            _redTeleporters.Add(new Teleporter(teleporterAnimation3, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 200 + (50 / 2), -(2048 / 2) + (50 / 2) + 1116)), 1f, 1f));

            TeleporterAnimation teleporterAnimation4 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation4.AnimationName = "redteleporter_d_open";
            _redTeleporters.Add(new Teleporter(teleporterAnimation4, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 750 + (50 / 2), -(2048 / 2) + (50 / 2) + 1065)), 1f, 1f));

            TeleporterAnimation teleporterAnimation5 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation5.AnimationName = "standardteleporter_d_open";
            _greenTeleporters.Add(new Teleporter(teleporterAnimation5, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1010 + (50 / 2), -(2048 / 2) + (50 / 2) + 635)), 1f, 1f));

            TeleporterAnimation teleporterAnimation6 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation6.AnimationName = "greenteleporter_d_open";
            _greenTeleporters.Add(new Teleporter(teleporterAnimation6, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 476 + (50 / 2), -(2048 / 2) + (50 / 2) + 1065)), 1f, 1f));

            TeleporterAnimation teleporterAnimation7 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation7.AnimationName = "standardteleporter_d_open";
            _yellowTeleporters.Add(new Teleporter(teleporterAnimation7, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 80 + (50 / 2), -(2048 / 2) + (50 / 2) + 641)), 1f, 1f));

            TeleporterAnimation teleporterAnimation8 = new TeleporterAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Teleporter\\teleporter"), new StreamReader(@"Content/Sprites/Teleporter/teleporter.txt"), 15f);
            teleporterAnimation8.AnimationName = "yellowteleporter_d_open";
            _yellowTeleporters.Add(new Teleporter(teleporterAnimation8, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 745 + (50 / 2), -(2048 / 2) + (50 / 2) + 850)), 1f, 1f));
        }
        private void GenerateKeys()
        {
            keyAnimation = new KeyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Keys\\keys"), new StreamReader(@"Content/Sprites/Keys/keys.txt"), 9f);
            keyAnimation.AnimationName = "keygreen_d_rotating";
            KeyAnimation keyAnimation2 = new KeyAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Keys\\keys"), new StreamReader(@"Content/Sprites/Keys/keys.txt"), 9f);
            keyAnimation2.AnimationName = "keygreen_d_rotating";
            _fakeGreenKey.Add(new Key(keyAnimation2, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 2015 + (16 / 2), -(2048 / 2) + (227 / 2) + 910)), .5f, .5f));
        }
        public void PotionExplosion(Wizard player)
        {

        }

        public void Spell(Wizard player, int forcePower)
        {
            Vector2 plasmaPosition = plasmaPosition = new Vector2(0, _player.Position.Y + 2.2f);
            WizardPlasmaAnimation plasmaSprite = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            plasmaSprite.AnimationName = "plasma_d_attack";
            SpriteAnimation animation = (SpriteAnimation)player.SpriteManager;
            Vector2 force = new Vector2();
            if (animation.GetOrientation() == Orientation.Down)
            {
                force = new Vector2(0, forcePower);
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y + 1);
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
                plasmaPosition = new Vector2(_player.Position.X - 1, _player.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Right)
            {
                force = new Vector2(forcePower, 0);
                plasmaPosition = new Vector2(_player.Position.X + 1, _player.Position.Y);
            }
            else if (animation.GetOrientation() == Orientation.Up)
            {
                plasmaPosition = new Vector2(_player.Position.X, _player.Position.Y - 1);
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
            for (int i = 0; i < _checkGate.Count; i++)
            {
                if (_checkGate[i].IsDead)
                {
                    Farseer.Instance.World.RemoveBody(_checkGate[i].Bodies[0]);
                    _blockGate = BodyFactory.CreateRectangle(world, 1f, 4f, 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1270 + (16 / 2), -(2048 / 2) + (227 / 2) + 922)));
                    _checkGate[i].IsDead = false;
                    redGateAnimation.AnimationName = _redGates[0].SpriteManager.AnimationName.Split('_')[0] + '_' + _redGates[0].SpriteManager.AnimationName.Split('_')[1] + '_' + "close";
                    redGateAnimation.SetAnimationState(AnimationState.Close);
                    redGateAnimation.TimeToUpdate = 50f;
                    redGateAnimation.FrameIndex = 0;
                    redGateAnimation.Update(gameTime);
                    _redGates[0].Update(gameTime);
                    _checkGate.Remove(_checkGate[i]);
                    _triggers2.Add(new TriggerBody(1f, 4f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1270 + (16 / 2), -(2048 / 2) + (227 / 2) + 922)), 1f, _actions, _plasma));

                }
                else
                {
                    _checkGate[i].Update(gameTime);
                }
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

            for (int i = 0; i < _switch.Count; i++)
            {
                SwitcherAnimation switchAnim = (SwitcherAnimation)_switch[0].SpriteManager;
                SwitcherAnimation switchAnim1 = (SwitcherAnimation)_switch[1].SpriteManager;
                SwitcherAnimation switchAnim2 = (SwitcherAnimation)_switch[2].SpriteManager;
                SwitcherAnimation switchAnim3 = (SwitcherAnimation)_switch[3].SpriteManager;
                //check first switch
                if (!_switch[0].IsOn)
                {
                    switchAnim.AnimationName = "switch_d_off";
                    switchAnim.SetAnimationState(AnimationState.Off);
                }
                else
                {
                    switchAnim.AnimationName = "switch_d_on";
                    switchAnim.SetAnimationState(AnimationState.On);
                }
                //check second switch
                if (!_switch[0].IsOn && _switch[1].IsOn)
                {
                    _switch[1].IsOn = false;
                    switchAnim1.AnimationName = "switch_d_off";
                    switchAnim1.SetAnimationState(AnimationState.Off);
                }
                else if (_switch[0].IsOn && _switch[1].IsOn)
                {
                    switchAnim1.AnimationName = "switch_d_on";
                    switchAnim1.SetAnimationState(AnimationState.On);
                }
                //check third switch
                if (!_switch[1].IsOn && _switch[2].IsOn)
                {
                    _switch[2].IsOn = false;
                    switchAnim2.AnimationName = "switch_d_off";
                    switchAnim2.SetAnimationState(AnimationState.Off);
                    _switch[1].IsOn = false;
                    switchAnim1.AnimationName = "switch_d_off";
                    switchAnim1.SetAnimationState(AnimationState.Off);
                    _switch[0].IsOn = false;
                    switchAnim.AnimationName = "switch_d_off";
                    switchAnim.SetAnimationState(AnimationState.Off);
                }
                else if (_switch[1].IsOn && _switch[2].IsOn)
                {
                    switchAnim2.AnimationName = "switch_d_on";
                    switchAnim2.SetAnimationState(AnimationState.On);
                }
                //check 4th switch
                if (!_switch[2].IsOn && _switch[3].IsOn)
                {
                    _switch[3].IsOn = false;
                    switchAnim3.AnimationName = "switch_d_off";
                    switchAnim3.SetAnimationState(AnimationState.Off);
                    _switch[2].IsOn = false;
                    switchAnim2.AnimationName = "switch_d_off";
                    switchAnim2.SetAnimationState(AnimationState.Off);
                    _switch[1].IsOn = false;
                    switchAnim1.AnimationName = "switch_d_off";
                    switchAnim1.SetAnimationState(AnimationState.Off);
                    _switch[0].IsOn = false;
                    switchAnim.AnimationName = "switch_d_off";
                    switchAnim.SetAnimationState(AnimationState.Off);
                }
                else if (_switch[2].IsOn && _switch[3].IsOn)
                {
                    switchAnim3.AnimationName = "switch_d_on";
                    switchAnim3.SetAnimationState(AnimationState.On);
                    _openBecauseISaySo = true;

                }

                //if (_switch[i].IsOn == true)
                //{
                //    switchAnim.AnimationName = "switch_d_on";
                //    switchAnim.SetAnimationState(AnimationState.On);
                //}
                //else if (_switch[i].IsOn == false)
                //{
                //    
                //}

                _switch[i].Update(gameTime);
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
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Up);
                    _blueTeleporters[0].Collided = false;
                }
                if (_blueTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _blueTeleporters[0].Position + new Vector2(0, 3);
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Up);
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
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
                }
                else if (_redTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _redTeleporters[0].Position + new Vector2(0, 3);
                    _redTeleporters[1].Collided = false;
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
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
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
                }
                else if (_yellowTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _yellowTeleporters[0].Position + new Vector2(0, 3);
                    _yellowTeleporters[1].Collided = false;
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
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
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
                }
                else if (_greenTeleporters[1].Collided == true)
                {
                    _player.getBody().Bodies[0].Position = _greenTeleporters[0].Position + new Vector2(0, 3);
                    _greenTeleporters[1].Collided = false;
                    WizardAnimation wizAnim = (WizardAnimation)_player.SpriteManager;
                    wizAnim.SetOrientation(Orientation.Down);
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
            for (int i = 0; i < _triggers2.Count; i++)
            {
                if (_triggers2[i].IsDead == true)
                {
                    for (int j = 0; j < _triggers2[i].ActionList.Count; j++)
                    {
                        if (_triggers2[i].ActionList[j].IsDead == false)
                            _triggers2[i].ActionList[j].Update(gameTime);
                    }
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

            for (int i = 0; i < _triggers2.Count; i++)
            {
                bool removeTrigger = true;
                for (int j = 0; j < _triggers2[i].ActionList.Count; j++)
                {
                    if (_triggers2[i].ActionList[j].IsDead == false)
                    {
                        removeTrigger = false;
                    }
                }
                if (removeTrigger)
                {
                    Farseer.Instance.World.RemoveBody(_triggers2[i].Bodies[0]);
                    _triggers2.Remove(_triggers2[i]);
                }
                else
                {
                    _triggers2[i].Update(gameTime);
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

            for (int i = 0; i < _redGates.Count; i++)
            {
                if ((_redGates[i].Unlock && _redKeyCount > 0) || _openBecauseISaySo)
                {

                    redGateAnimation.SetAnimationState(AnimationState.Open);
                    redGateAnimation.Update(gameTime);
                    if (_redGates[i].GateCollideFirstTime)
                    {
                        Farseer.Instance.World.RemoveBody(_redGates[i].getBody().Bodies[0]);
                        _redGates[i].GateCollideFirstTime = false;
                        _redKeyCount--;
                    }
                }
                else
                {
                    _redGates[i].Update(gameTime);
                }
            }

            for (int i = 0; i < _redGates.Count; i++)
            {
                if ((_redGates[i].Unlock && _redKeyCount > 0) || _openBecauseISaySo)
                {
                    redGateAnimation.SetAnimationState(AnimationState.Open);
                    redGateAnimation.Update(gameTime);
                    if (_redGates[i].GateCollideFirstTime)
                    {
                        Farseer.Instance.World.RemoveBody(_redGates[i].getBody().Bodies[0]);
                        _redGates[i].GateCollideFirstTime = false;
                        _redKeyCount--;
                    }
                }
                else
                {
                    _redGates[i].Update(gameTime);
                }
            }
            for (int i = 0; i < _greenKey.Count; i++)
            {
                if (_greenKey[i].IsCollected == true && _greenKey[i].CollideWithPlasma == true)
                {
                    _greenKeyCount++;
                    Farseer.Instance.World.RemoveBody(_greenKey[i].getBody().Bodies[0]);
                    _greenKey.Remove(_greenKey[i]);
                    _redGates[0].SpriteManager.AnimationName = "redgate_dl_open";
                    Farseer.Instance.World.RemoveBody(_blockGate);
                }
                else if (_greenKey[i].CollideWithPlasma == false)
                {
                    SpriteAnimation animation = (SpriteAnimation)_player.SpriteManager;
                    if (animation.GetOrientation() == Orientation.Down)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(0, 1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.DownLeft)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(-1.5f, 1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.DownRight)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(1.5f, 1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.Up)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(0, -1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.UpLeft)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(-1.5f, -1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.UpRight)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(1.5f, -1.5f);
                    }
                    if (animation.GetOrientation() == Orientation.Left)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(-1.5f, 0);
                    }
                    if (animation.GetOrientation() == Orientation.Right)
                    {
                        _greenKey[i].getBody().Bodies[0].Position = _player.getBody().Bodies[0].Position - new Vector2(1.5f, 0);
                    }

                    _greenKey[i].Update(gameTime);
                }
                else
                {
                    _greenKey[i].Update(gameTime);
                }

            }

            for (int i = 0; i < _fakeGreenKey.Count; i++)
            {
                if (_fakeGreenKey[i].IsCollected == true)
                {
                    Farseer.Instance.World.RemoveBody(_fakeGreenKey[i].getBody().Bodies[0]);
                    _fakeGreenKey.Remove(_fakeGreenKey[i]);
                    _greenKey.Add(new Key(keyAnimation, _player.Position - new Vector2(1.5f, 0), ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(15)));
                }
                else
                {
                    _fakeGreenKey[i].Update(gameTime);
                }
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            //Draw stuff in here
            foreach (Switcher s in _switch)
            {
                s.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }

            foreach (Key g in _greenKey)
            {
                //if (g.CollideWithPlasma == true)
                //{
                g.SpriteManager.Draw(ScreenManager.SpriteBatch);
                //}
            }
            foreach (Key g in _fakeGreenKey)
            {

                g.SpriteManager.Draw(ScreenManager.SpriteBatch);

            }

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
            foreach (Gate g in _redGates)
            {
                g.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }

            if (!isGameOver)
            {
                _player.SpriteManager.Draw(ScreenManager.SpriteBatch);
                _health.SpriteManager.Draw(ScreenManager.SpriteBatch);
            }
            _topOfLog.Draw(ScreenManager.SpriteBatch);
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
            for (int i = 0; i < _triggers2.Count; i++)
            {
                if (_triggers2[i].IsDead == true)
                {
                    for (int j = 0; j < _triggers2[i].ActionList.Count; j++)
                    {
                        _triggers2[i].ActionList[j].Draw();
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
