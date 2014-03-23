using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.ScreenSystem;
using WizardsNeverDie.Dialog;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WizardsNeverDie.Trigger
{
    public class MessageAction : IAction
    {
        private bool _isDead = false;
        private ScreenManager _screenManager;
        private bool _firstTime = true;
        private string _fileName;

        private Texture2D _avatar;

        public MessageAction(ScreenManager screenManager, Texture2D avatar, string fileName)
        {
            _screenManager = screenManager;
            _fileName = fileName;
            Conversation.Initialize(_screenManager.Content.Load<SpriteFont>(@"Fonts\Segoe"),
                _screenManager.Content.Load<SoundEffect>(@"SoundEffects\ContinueDialogue"),
                _screenManager.Content.Load<Texture2D>(@"Common\DialogueBoxBackground"),
                new Rectangle(50, 50, 400, 100),
                _screenManager.Content.Load<Texture2D>(@"Common\BorderImage"),
                5,
                Color.Black,
                _screenManager.Content.Load<Texture2D>(@"Common\ConversationContinueIcon"),
                _screenManager.Content.RootDirectory + @"\Conversations\");
            _avatar = avatar;
        }
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        public Texture2D Avatar
        {
            get
            {
                return _avatar;
            }
            set
            {
                _avatar = value;
            }
        }
        public void Draw()
        {
            Conversation.Draw(_screenManager);
        }
        public void Update(GameTime gameTime)
        {
            Conversation.Update(gameTime);
            if (Conversation.Expired == true)
                _isDead = true;
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
    }
}
