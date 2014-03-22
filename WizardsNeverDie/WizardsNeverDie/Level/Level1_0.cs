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
    class Level1_0 : BaseLevel
    {
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
        private List<Spawner> _spawners = new List<Spawner>();
        private List<TriggerBody> _triggers = new List<TriggerBody>();
        private List<Potions> _potion = new List<Potions>();
        private List<PotionExplosion> _potionExplosion = new List<PotionExplosion>();
        private HealthAnimation _healthSprite;
        PotionExplosionAnimation potionExplosionAnimation;
        private Health _health;

        private MessageBoxScreen _mbs;

        private SpriteAnimation _potionSprite;
        private SpriteAnimation _potionSprite2;
        private SpriteAnimation _potionSprite3;
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

        public Level1_0()
        {
            int test = initialTime.Seconds;
            levelDetails = "Level 1";
            levelName = "Start Game: 1";
            this.backgroundTextureStr = "Materials/Level1_0";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Farseer.Instance.World.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;
            _wizard = new WizardAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wizard\\wizard"), new StreamReader(@"Content/Sprites/Wizard/wizard.txt"));
            _wizard.AnimationName = "wizard_d_walk";
            _player = new Wizard(_wizard, ConvertUnits.ToSimUnits(-(2048/2) + 430, -(2048/2)+135));
            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _gameover = ScreenManager.Content.Load<Texture2D>("Common\\gameover");

            GenerateWalls();
            GenereateCreatures();
            GenereateSpawners();
            GeneratePotions();

            this.Camera.EnableTracking = true;
            this.Camera.TrackingBody = _player.getBody().Bodies[0];
        }

        private void GenerateWalls()
        {
            World world = Farseer.Instance.World;
            Body wallLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(2048), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 16, 0)));
            //Body wallRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(2048), 1f, ConvertUnits.ToSimUnits(new Vector2((2048 / 2) - 16, 0)));
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
            List<IAction> _actions1 = new List<IAction>();
            List<IAction> _actions2 = new List<IAction>();
            List<IAction> _actions3 = new List<IAction>();

            SwitchLevelAction nextLevel = new SwitchLevelAction(ScreenManager, this, new Level1_1());
            _actions3.Add(nextLevel);

            //string str1 = "Oh No, I can't believe <INSERT PLOT HERE>!!!\nI better use my magic powers.";
            //string str2 = "Don't worry I have my staff set to stun.";
            //string str3 = "rofl who the hell are these fools voting woot \nas the best demoman?  At what point did smart \nplay and good aim become overlooked in favor of \nrandom mouse flicking and laying stickies ";
            //string str4 = "everywhere in between constant failed jumps on the enemy medic." +
            //"  Obviously a better teammate than destro or solid, " +
            //"who actually attempt to do damage rather than just cleanup garbage frags." +

            //"here is a fun game: download a woot demo or stv of his team and attempt to count the number of shots which are aimed. " +
            //"Place bets with friends in mumble over how many times he will stickyjump " +
            //"into the other team to make up for his lack of dming ability. I know it's tempting to spec sureshot " +
            //"carrying him but take the time to truly observe this demoman and try to decipher what exactly he is " +
            //"attempting to do for his team at any given time. and if you still think he is good go to steam, " +
            //"right click tf2, select delete local content and take up a mw2 gaming career.";

            //I got it set to stun
            MessageAction wAction = new MessageAction(ScreenManager, ScreenManager.Content.Load<Texture2D>(@"Avatar\WizardAvatar"), "conversation1.xml");

            //Best Demoman NA and Enable Spawners
            MessageAction mAction = new MessageAction(ScreenManager, ScreenManager.Content.Load<Texture2D>(@"Avatar\IfritAvatar"), "conversation2.xml");
            SpawnerAction sAction = new SpawnerAction(_spawners);


            _actions1.Add(mAction);
            _actions1.Add(sAction);
            _actions2.Add(wAction);
            _triggers.Add(new TriggerBody(ConvertUnits.ToSimUnits(552), ConvertUnits.ToSimUnits(50), ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 608 + (552 / 2), -(2048 / 2) + (50 / 2) + 1582)), 1f, _actions1));
            _triggers.Add(new TriggerBody(ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(31), ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 111 + (96 / 2), -(2048 / 2) + (31 / 2) + 465)), 1f, _actions2));
            _triggers.Add(new TriggerBody(ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(2048), ConvertUnits.ToSimUnits(new Vector2((2048 / 2) - 16, 0)), 1f, _actions3));
        }

        private void GenereateCreatures()
        {

            MeleeRedIfritAnimation _creatureAnimation = new MeleeRedIfritAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Ifrit\\ifrit"), new StreamReader(@"Content/Sprites/Ifrit/ifrit.txt"));
            _creatureAnimation.AnimationName = "ifrit_d_walk";
            _creatures.Add(new MeleeRedIfrit(_creatureAnimation, _player, ConvertUnits.ToSimUnits(-(2048 / 2) + 435, -(2048 / 2) + 740), 1.5f, 1.5f, 10F));

            //RangedPurpleIfritAnimation _purpleCreatureAnimation = new RangedPurpleIfritAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\PurpleIfrit\\ifrit"),
            //    new StreamReader(@"Content/Sprites/PurpleIfrit/ifrit.txt"));
            //_purpleCreatureAnimation.AnimationName = "ifrit_d_walk";
            //_purpleCreatures.Add(new RangedPurpleIfrit(_purpleCreatureAnimation, _player, ConvertUnits.ToSimUnits(-(2048 / 2) + 435, -(2048 / 2) + 740), 1.5f, 1.5f, 15F, 10f));
        }

        private void GenereateSpawners()
        {
            SpawnerAnimation _spawnerAnimation = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation.AnimationName = "portal_dr_spawning";
            _spawners.Add(new Spawner(_spawnerAnimation, ConvertUnits.ToSimUnits(-(2048 / 2) + 520, -(2048 / 2) + 1840), _player, 1.5f, 1.5f, true));

            SpawnerAnimation _spawnerAnimation2 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation2.AnimationName = "portal_d_spawning";
            _spawners.Add(new Spawner(_spawnerAnimation2, ConvertUnits.ToSimUnits(-(2048 / 2) + 752, -(2048 / 2) + 1289), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation3 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation3.AnimationName = "portal_d_spawning";
            _spawners.Add(new Spawner(_spawnerAnimation3, ConvertUnits.ToSimUnits(-(2048 / 2) + 1032, -(2048 / 2) + 1032), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation4 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation4.AnimationName = "portal_d_spawning";
            _spawners.Add(new Spawner(_spawnerAnimation4, ConvertUnits.ToSimUnits(-(2048 / 2) + 749, -(2048 / 2) + 791), _player, 1.5f, 1.5f, false));

            SpawnerAnimation _spawnerAnimation5 = new SpawnerAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Spawner\\portal"), new StreamReader(@"Content/Sprites/Spawner/portal.txt"));
            _spawnerAnimation5.AnimationName = "portal_d_spawning";
            _spawners.Add(new Spawner(_spawnerAnimation5, ConvertUnits.ToSimUnits(-(2048 / 2) + 1034, -(2048 / 2) + 507), _player, 1.5f, 1.5f, false));
        }

        private void GeneratePotions()
        {
            _potionSprite = new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Potions\\potions"), new StreamReader(@"Content/Sprites/Potions/potions.txt"));
            _potionSprite.AnimationName = "potion_d_orange";
            _potion.Add(new Potions(_potionSprite, ConvertUnits.ToSimUnits(-(2048 / 2) + 891, -(2048 / 2) + 125), 1f, 1f));

            //_potionSprite2 = new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Potions\\potions"), new StreamReader(@"Content/Sprites/Potions/potions.txt"));
            //_potionSprite2.AnimationName = "potion_d_orange";
            //_potion.Add(new Potions(_potionSprite2, ConvertUnits.ToSimUnits(-(2048 / 2) + 900, -(2048 / 2) + 127), 1f, 1f));

            //_potionSprite3 = new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Potions\\potions"), new StreamReader(@"Content/Sprites/Potions/potions.txt"));
            //_potionSprite3.AnimationName = "potion_d_orange";
            //_potion.Add(new Potions(_potionSprite3, ConvertUnits.ToSimUnits(-(2048 / 2) + 910, -(2048 / 2) + 127), 1f, 1f));
        }

        public void PotionExplosion(Wizard player)
        {
            potionExplosionAnimation = new PotionExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\PotionExplosion\\potionexplosion"),
                new StreamReader(@"Content/Sprites/PotionExplosion/potionexplosion.txt"));
            potionExplosionAnimation.AnimationName = "potionexplosion_d_kaboom";
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
            ExplosionAnimation explosionSprite = new ExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Explosion\\explosion"), new StreamReader(@"Content/Sprites/Explosion/explosion.txt"));
            ExplosionAnimation metalSlugExplosionSprite = 
                new ExplosionAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Explosion\\metalslugexplosion"), new StreamReader(@"Content/Sprites/Explosion/metalslugexplosion.txt"));
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
            for (int i = 0; i < _triggers.Count; i++)
            {
                if (_triggers[i].IsDead == true)
                {
                    for (int j = 0; j < _triggers[i].ActionList.Count; j++)
                    {
                        if(_triggers[i].ActionList[j].IsDead==false)
                            _triggers[i].ActionList[j].Update(gameTime);
                    }
                }
            }
            if (_player.Health == HealthAnimation.HealthState.Health0 && isGameOver == false)
            {
                isGameOver = true;
                _gameOverVector = new Vector2((int)ConvertUnits.ToDisplayUnits(_player.Position.X - 8), (int)ConvertUnits.ToDisplayUnits(_player.Position.Y - 5.5F));
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
            for (int i = 0; i < _spawners.Count; i++ )
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
