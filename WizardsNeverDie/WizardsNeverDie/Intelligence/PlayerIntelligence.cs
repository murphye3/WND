using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WizardsNeverDie.Animation;
using WizardsNeverDie.Level;
using WizardsNeverDie.Physics;
using WizardsNeverDie.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
namespace WizardsNeverDie.Intelligence
{
    class PlayerIntelligence : AbstractIntelligence
    {

        protected Orientation lastOrientation;
        protected TimeSpan swapTimer;
        protected TimeSpan swapCooldown = TimeSpan.FromSeconds(.03d);
        public Wizard player;
        float speed;
        KeyboardState keyboardState, lastKeyBoardState;
        GamePadState gamepadState, lastGamepadState;
        public PlayerIntelligence(AbstractCreature player, float speed)
        {
            this.player = (Wizard)player;
            this.speed = speed;
            lastOrientation = Orientation.None;
        }

        public bool canSwap(Orientation current)
        {
            bool canSwap = true;
            if (swapTimer < swapCooldown)
            {
                switch (current)
                {
                    case Orientation.Down: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.DownRight) canSwap = false; break;
                    case Orientation.Up: if (lastOrientation == Orientation.UpLeft || lastOrientation == Orientation.UpRight) canSwap = false; break;
                    case Orientation.Left: if (lastOrientation == Orientation.DownLeft || lastOrientation == Orientation.UpLeft) canSwap = false; break;
                    case Orientation.Right: if (lastOrientation == Orientation.DownRight || lastOrientation == Orientation.UpRight) canSwap = false; break;
                    case Orientation.DownLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Down) canSwap = false; break;
                    case Orientation.DownRight: if (lastOrientation == Orientation.Down || lastOrientation == Orientation.Right) canSwap = false; break;
                    case Orientation.UpLeft: if (lastOrientation == Orientation.Left || lastOrientation == Orientation.Up) canSwap = false; break;
                    case Orientation.UpRight: if (lastOrientation == Orientation.Up || lastOrientation == Orientation.Right) canSwap = false; break;
                    default: break;
                }
            }
            return canSwap;
        }

        public void swapWalkOrientation(Orientation current)
        {
            if (current == Orientation.Right)
            {
                ;
            }
            WizardAnimation animation = (WizardAnimation)player.SpriteManager;
            if (current == Orientation.None)
                animation.SetAnimationState(AnimationState.Stop);
            else if(canSwap(current))
            {
                animation.SetAnimationState(AnimationState.Walk);
                animation.SetOrientation(current);
                lastOrientation = current;
                swapTimer = TimeSpan.Zero;
            }
        }

        public void spell()
        {
            SpriteAnimation animation = (SpriteAnimation)player.SpriteManager;
            animation.SetAnimationState(AnimationState.Spell1);
        }

        public void potionexplode()
        {
            SpriteAnimation animation = (SpriteAnimation)player.SpriteManager;
            animation.SetAnimationState(AnimationState.PotionExplosion);
        }

        public override void Update(GameTime gameTime)
        {
            lastGamepadState = gamepadState;
            gamepadState = GamePad.GetState(PlayerIndex.One);
            swapTimer = swapTimer.Add(gameTime.ElapsedGameTime);
            lastKeyBoardState = keyboardState;
            keyboardState = Keyboard.GetState();
            PhysicsBody body = player.getBody();
            if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space) && (lastKeyBoardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                || (gamepadState.Buttons.X == ButtonState.Released && lastGamepadState.Buttons.X == ButtonState.Pressed))
            {
                    spell();
            }
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)
                || (gamepadState.IsConnected && (gamepadState.DPad.Down == ButtonState.Pressed && gamepadState.DPad.Left == ButtonState.Pressed)))
            {
                swapWalkOrientation(Orientation.DownLeft);
                body.Move(new Vector2(-speed, speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)
                || (gamepadState.IsConnected && (gamepadState.DPad.Down == ButtonState.Pressed && gamepadState.DPad.Right == ButtonState.Pressed)))
            {
                swapWalkOrientation(Orientation.DownRight);
                body.Move(new Vector2(speed, speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)
                || (gamepadState.IsConnected && (gamepadState.DPad.Up == ButtonState.Pressed && gamepadState.DPad.Left == ButtonState.Pressed)))
            {
                swapWalkOrientation(Orientation.UpLeft);
                body.Move(new Vector2(-speed, -speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)
                || (gamepadState.IsConnected && (gamepadState.DPad.Up == ButtonState.Pressed && gamepadState.DPad.Right == ButtonState.Pressed)))
            {
                swapWalkOrientation(Orientation.UpRight);
                body.Move(new Vector2(speed, -speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) || (gamepadState.IsConnected && gamepadState.DPad.Up == ButtonState.Pressed))
            {
                swapWalkOrientation(Orientation.Up);
                body.Move(new Vector2(0, -speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) || (gamepadState.IsConnected && gamepadState.DPad.Down == ButtonState.Pressed))
            {
                swapWalkOrientation(Orientation.Down);
                body.Move(new Vector2(0, speed));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) || (gamepadState.IsConnected && gamepadState.DPad.Left == ButtonState.Pressed))
            {
                swapWalkOrientation(Orientation.Left);
                body.Move(new Vector2(-speed, 0));
            }
            else if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) || (gamepadState.IsConnected && gamepadState.DPad.Right == ButtonState.Pressed))
            {
                swapWalkOrientation(Orientation.Right);
                body.Move(new Vector2(speed, 0));
            }
            else
            {
                swapWalkOrientation(Orientation.None);
            }
        }
    }
}
