using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using WizardsNeverDie.Trigger;

namespace WizardsNeverDie.Level
{
    public class Level1_1: BaseLevel
    {
        Body oracleCutSceneBlock;
        private Wizard _player;
        private WizardAnimation _wizard;
        private OracleAnimation _oracleAnimation;
        private List<Oracle> _oracle = new List<Oracle>();
        private HealthAnimation _healthSprite;
        private Health _health;
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
        Stopwatch timer = new Stopwatch();
        TimeSpan initialTime;
        TimeSpan animationTimeSpan;
        int forcePower = 500;
        Stopwatch animationTimer = new Stopwatch();
        Boolean animationFinished = true;
        private bool isGameOver = false;
        private int _creaturesKilled = 0;
        private TriggerBody _trigger;

        public Level1_1()
        {
            int test = initialTime.Seconds;
            levelDetails = "Level 1";
            levelName = "Start Game: 2";
            this.backgroundTextureStr = "Materials/Level1_1";
        }
        public override void LoadContent()
        {
            base.LoadContent();
            _wizard = new WizardAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wizard\\wizard"), new StreamReader(@"Content/Sprites/Wizard/wizard.txt"));
            _wizard.AnimationName = "wizard_d_walk";
            _player = new Wizard(_wizard, ConvertUnits.ToSimUnits(-(2048 / 2) + 47, -(2048 / 2) + 1025));
            _healthSprite = new HealthAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Health\\health"), new StreamReader(@"Content/Sprites/Health/health.txt"));
            _healthSprite.AnimationName = "health_n_health25";
            _health = new Health(_healthSprite, _player, _player.Position);
            _oracleAnimation = new OracleAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Oracle\\oracle"), new StreamReader(@"Content/Sprites/Oracle/oracle.txt"), 6f);
            _oracleAnimation.AnimationName = "oracle_d_walk";
            _oracle.Add(new Oracle(_oracleAnimation,  ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1020 + (1f / 2), -(2048 / 2) + (1f / 2) + 1019))));
            GenerateWalls();
            GenereateCreatures();
            GenereateSpawners();
            GeneratePotions();

            this.Camera.EnableTracking = true;
            this.Camera.TrackingBody = _player.getBody().Bodies[0];
        }
        public void GenerateWalls()
        {
            World world = Farseer.Instance.World;
            Body treesTopLeft1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(961), ConvertUnits.ToSimUnits(546), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (961 / 2), -(2048 / 2) + (546 / 2) + 265)));
            Body treesTopLeft2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(769), ConvertUnits.ToSimUnits(174), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (769 / 2), -(2048 / 2) + (174 / 2) + 811)));
            Body treesBottomLeft1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(961), ConvertUnits.ToSimUnits(810), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (961 / 2), -(2048 / 2) + (810 / 2) + 1238)));
            Body treesBottomLeft2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(769), ConvertUnits.ToSimUnits(174), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 0 + (769 / 2), -(2048 / 2) + (174 / 2) + 1065)));
            Body treesTopRight1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(961), ConvertUnits.ToSimUnits(546), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1086 + (961 / 2), -(2048 / 2) + (546 / 2) + 265)));
            Body treesTopRight2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(769), ConvertUnits.ToSimUnits(174), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1279 + (769 / 2), -(2048 / 2) + (174 / 2) + 811)));
            Body treesBottomRight1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(961), ConvertUnits.ToSimUnits(810), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1087 + (961 / 2), -(2048 / 2) + (810 / 2) + 1238)));
            Body treesBottomRight2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(769), ConvertUnits.ToSimUnits(174), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1279 + (769 / 2), -(2048 / 2) + (174 / 2) + 1065)));
            Body leftBridgeWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(227), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 968 + (16 / 2), -(2048 / 2) + (227 / 2) + 86)));
            Body rightBridgeWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(227), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1064 + (16 / 2), -(2048 / 2) + (227 / 2) + 86)));
            oracleCutSceneBlock = BodyFactory.CreateRectangle( world, ConvertUnits.ToSimUnits(24), ConvertUnits.ToSimUnits(80), 1f,  ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 761 + (24 / 2), -(2048 / 2) + (80 / 2) + 986)));
            //Body topPlatformWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(17), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 975 + (96 / 2), -(2048 / 2) + (17 / 2) + 961)));
            //Body bottomPlatformWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(17), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 975 + (96 / 2), -(2048 / 2) + (17 / 2) + 1072)));
            //Body backPlatformWall = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(17), ConvertUnits.ToSimUnits(128), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1055 + (17 / 2), -(2048 / 2) + (128 / 2) + 961)));
            //Body bottomPlatformWall2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(23), 1f, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 967 + (25 / 2), -(2048 / 2) + (23 / 2) + 1049)));
            PlasmaWall backPlatformWall = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1055 + (17 / 2), -(2048 / 2) + (128 / 2) + 961)), ConvertUnits.ToSimUnits(17), ConvertUnits.ToSimUnits(128));
            PlasmaWall bottomPlatformWall2 = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 967 + (25 / 2), -(2048 / 2) + (23 / 2) + 1049)), ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(23));
            PlasmaWall bottomPlatformWall = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 975 + (96 / 2), -(2048 / 2) + (17 / 2) + 1072)), ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(17));
            PlasmaWall topPlatformWall = new PlasmaWall(ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 975 + (96 / 2), -(2048 / 2) + (17 / 2) + 961)), ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(17));
            _trigger = new TriggerBody(ConvertUnits.ToSimUnits(24), ConvertUnits.ToSimUnits(80),ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 761 + (24 / 2), -(2048 / 2) + (80 / 2) + 986)), 1f);
            List<IAction> _actions1 = new List<IAction>();
            //I got it set to stun
            MessageAction oAction = new MessageAction(ScreenManager, ScreenManager.Content.Load<Texture2D>(@"Avatar\OracleAvatar"), "conversation3.xml");
            _actions1.Add(oAction);
            _triggers.Add(new TriggerBody(ConvertUnits.ToSimUnits(24), ConvertUnits.ToSimUnits(80), ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 761 + (24 / 2), -(2048 / 2) + (80 / 2) + 986)), 1f, _actions1));
        }


        public void GenereateCreatures()
        {
            ;
        }
        public void GenereateSpawners()
        {
            ;
        }
        public void GeneratePotions()
        {
            ;
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
            }


        //}

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
                    PurpleSpell(_purpleCreatures[i], 300);
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
                //PotionExplosion(_player);
                //_potionExplosion.Add(new PotionExplosion(potionExplosionAnimation, _player, _creatures, _player.Position));
                //potionCount--;
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
            //oracle stufffff
            if (_trigger.IsDead)
            {
                WizardPlasmaAnimation plasmaSprite = new WizardPlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
                plasmaSprite.AnimationName = "plasma_d_attack";
                _oracleAnimation.SetAnimationState(AnimationState.Talking);
                _oracleAnimation.Update(gameTime);
                _purplePlasma.Add(new RangedPurplePlasma(plasmaSprite,  ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1470 + (1f / 2), -(2048 / 2) + (1f / 2) + 1019)), new Vector2(-85, 0)));
                Farseer.Instance.World.RemoveBody(_trigger.Bodies[0]);
                _trigger.IsDead = false;
            }
            for (int i = 0; i < _oracle.Count; i++)
            {
                if (_oracle[i].IsDead)
                {
                    oracleCutSceneBlock.Dispose();
                    explosionSprite.AnimationName = "ifrit_d_explosion";
                    _explosions.Add(new Explosion(explosionSprite, _oracle[i].Position + new Vector2(0, -1), .01f, .01f));
                    Farseer.Instance.World.RemoveBody(_oracle[i].getBody().Bodies[0]);
                    _oracle.Remove(_oracle[i]);
                    RangedPurpleIfritAnimation _purpleCreatureAnimation = new RangedPurpleIfritAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\PurpleIfrit\\ifrit"),
                        new StreamReader(@"Content/Sprites/PurpleIfrit/ifrit.txt"));
                    _purpleCreatureAnimation.AnimationName = "ifrit_d_walk";
                    _purpleCreatures.Add(new RangedPurpleIfrit(_purpleCreatureAnimation, _player, ConvertUnits.ToSimUnits(new Vector2(-(2048 / 2) + 1470 + (1f / 2), -(2048 / 2) + (1f / 2) + 1019)), 1.5f, 1.5f, 100F, 20f));
                }else
                {
                    _oracle[i].Update(gameTime);
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
            foreach(Oracle o in _oracle)
            {
                o.SpriteManager.Draw(ScreenManager.SpriteBatch);
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
                if (_triggers[i].IsDead)
                {
                    if (_oracle.Count == 1)
                    {
                        for (int j = 0; j < _triggers[i].ActionList.Count; j++)
                        {
                            _triggers[i].ActionList[j].Draw();
                        }
                    }
                }
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
