
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

namespace WizardsNeverDie.Level
{
    class IfritDemo : BaseLevel
    {

        private Player _player;
        private WizardAnimation _wizard;
        private List<Brick> _bricks;
        private List<Enemy> _creatures = new List<Enemy>();
        private List<SpriteAnimation> _wallSprites;
        private List<Plasma> _plasma = new List<Plasma>();

        private HealthAnimation _healthSprite;
        private Health _health;

        private Texture2D _gameover;
        private Vector2 _gameOverVector;
        private bool isGameOver = false;
        private int _creaturesKilled = 0;

        public IfritDemo()
        {
            levelDetails = "Level 1";
            levelName = "Start Game";
            this.backgroundTextureStr = "Materials/ground";
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
            for(int i = 0; i< w_size; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j+i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j+i], new Vector2(x, y)));
                x+=.7F;
            }
            j = _wallSprites.Count;
            for (int i = 0; i < h_size; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j+i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j+i], new Vector2(x, y)));
                y -= .7F;
            }
            j = _wallSprites.Count;
            for (int i = 0; i < w_size+1; i++)
            {
                _wallSprites.Add(new SpriteAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Wall\\wall"), new StreamReader(@"Content/Sprites/Wall/wall.txt")));
                _wallSprites[j + i].AnimationName = "wall_n_normal";
                _bricks.Add(new Brick(_wallSprites[j + i], new Vector2(x, y)));
                x -= .7F;
            }

        }

        public void Spell(Player player)
        {
            PlasmaAnimation plasmaSprite = new PlasmaAnimation(ScreenManager.Content.Load<Texture2D>("Sprites\\Plasma\\plasma"), new StreamReader(@"Content/Sprites/Plasma/plasma.txt"));
            plasmaSprite.AnimationName = "plasma_d_attack";
            SpriteAnimation animation = (SpriteAnimation)player.SpriteManager;
            Vector2 force = new Vector2();
            int forcePower = 500;
            if (animation.GetOrientation() == Orientation.Down)
                force = new Vector2(0, forcePower);
            else if (animation.GetOrientation() == Orientation.DownLeft)
                force = new Vector2(0, forcePower);
            else if (animation.GetOrientation() == Orientation.DownRight)
                force = new Vector2(0, forcePower);
            else if (animation.GetOrientation() == Orientation.Left)
                force = new Vector2(-forcePower, 0);
            else if (animation.GetOrientation() == Orientation.Right)
                force = new Vector2(forcePower, 0);
            else if (animation.GetOrientation() == Orientation.Up)
                force = new Vector2(0, -forcePower);
            else if (animation.GetOrientation() == Orientation.UpLeft)
                force = new Vector2(0, -forcePower);
            else if (animation.GetOrientation() == Orientation.UpRight)
                force = new Vector2(0, -forcePower);
            _plasma.Add(new Plasma(plasmaSprite, _player.Position, force));
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (Brick b in _bricks)
            {
                b.Update(gameTime);
            }
            if (!isGameOver)
            {
                _player.Update(gameTime);
                _health.Update(gameTime);
            }
            if (_player.Health == HealthAnimation.HealthState.Health0 && isGameOver==false)
            {
                isGameOver = true;
                _gameOverVector = new Vector2((int)ConvertUnits.ToDisplayUnits(_player.Position.X - 8), (int)ConvertUnits.ToDisplayUnits(_player.Position.Y - 5.5F));
            }
            for (int i = 0; i<_creatures.Count; i++)//(Enemy e in _creatures)
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
                _creatureAnimation.AnimationName = "ifrit_d_attack";
                _creatures.Add(new Enemy(_creatureAnimation, _player, new Vector2(0, -20)));
            }
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                if(!isGameOver)
                    Spell(_player);
            }
            for(int i =0; i<_plasma.Count; i++)//(Plasma p in _plasma)
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
