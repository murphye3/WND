using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardsNeverDie.ScreenSystem;

namespace WizardsNeverDie.Dialog
{
    public static class Conversation
    {
        #region Declarations

        public static List<Speaker> ConversationSpeakers = new List<Speaker>();
        private static int currentSpeakerIndex = 0;
        public static string ConversationFileLocation;

        public static SpriteFont spriteFont;

        public static SoundEffect soundEffect;

        private static Rectangle textRectangle;
        private static string message;

        private static string revealedMessage;
        private static float messageSpeed = 0.008f;
        private static float messageTimer = 0.0f;
        private static int stringIndex;
        public static bool MessageShown = false;

        private static Texture2D splitIcon;
        private static float splitIconSpeed = 0.4f;
        private static float splitIconTimer = 0.0f;
        private static int splitIconOffsetValue = 5;
        private static bool splitIconOffset = false;

        private static Texture2D backgroundImage;
        private static Rectangle boxRectangle;

        private static Texture2D borderImage;
        private static int borderWidth;
        private static Color borderColor;

        public static List<Texture2D> Avatars = new List<Texture2D>();
        private static Rectangle avatarRectangle;

        public static bool Expired = false;

        #endregion

        #region Properties

        public static Vector2 BoxPosition
        {
            get { return new Vector2(boxRectangle.X, boxRectangle.Y); }
        }

        public static Vector2 StringPosition
        {
            get { return new Vector2(textRectangle.X, textRectangle.Y); }
        }

        public static float MessageDelay
        {
            get { return messageSpeed; }
            set { messageSpeed = value; }
        }

        public static string Message
        {
            get { return message; }
            set { message = constrainText(value); }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes the Conversation Class
        /// </summary>
        /// <param name="font">Font to display text with</param>
        /// <param name="background">Window Background Image</param>
        /// <param name="initialRectangle">Window Background Rectangle</param>
        /// <param name="bImage">Window Border Image</param>
        /// <param name="bWidth">Window Border Width</param>
        /// <param name="bColor">Window Border Color</param>
        /// <param name="sIcon">Continue Reading Icon</param>
        public static void Initialize(SpriteFont font, SoundEffect sound, Texture2D background, Rectangle initialRectangle, Texture2D bImage, int bWidth, Color bColor, Texture2D sIcon, string path)
        {
            spriteFont = font;
            soundEffect = sound;
            backgroundImage = background;
            boxRectangle = initialRectangle;
            textRectangle = new Rectangle(initialRectangle.X + 10, initialRectangle.Y + 10, initialRectangle.Width - 20, initialRectangle.Height - 20);
            borderImage = bImage;
            borderWidth = bWidth;
            borderColor = bColor;
            splitIcon = sIcon;
            ConversationFileLocation = path;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts a new Conversation
        /// </summary>
        /// <param name="conversationID">Conversation ID to use</param>
        public static void StartConversation(string str1, string str2)
        {
            // TODO: Load ID, then run createbox etc
            currentSpeakerIndex = 0;
            ConversationSpeakers.Add(new Speaker(0, constrainText(str1)));
            ConversationSpeakers.Add(new Speaker(1, str2));
            //ConversationSpeakers.Add(new Speaker(0, constrainText("rofl who the hell are these fools voting woot \nas the best demoman?  At what point did smart \nplay and good aim become overlooked in favor of \nrandom mouse flicking and laying stickies ")));
            //ConversationSpeakers.Add(new Speaker(1,
            //"everywhere in between constant failed jumps on the enemy medic." +
            //"  Obviously a better teammate than destro or solid, " +
            //"who actually attempt to do damage rather than just cleanup garbage frags." +

            //"here is a fun game: download a woot demo or stv of his team and attempt to count the number of shots which are aimed. " +
            //"Place bets with friends in mumble over how many times he will stickyjump " +
            //"into the other team to make up for his lack of dming ability. I know it's tempting to spec sureshot " +
            //"carrying him but take the time to truly observe this demoman and try to decipher what exactly he is " +
            //"attempting to do for his team at any given time. and if you still think he is good go to steam, " +
            //"right click tf2, select delete local content and take up a mw2 gaming career."));
            CreateBox(ConversationSpeakers[currentSpeakerIndex].Message,
                new Rectangle(100, 200, 600, 150),
                new Rectangle(250, 215, 445, 115),
                new Rectangle(120, 215, 115, 115));
        }

        /// <summary>
        /// Creates a new Conversation Window
        /// </summary>
        /// <param name="msg">Speaker Message</param>
        /// <param name="msgBox">Window Rectangle</param>
        /// <param name="textBox">Text Rectangle</param>
        /// <param name="avatarBox">Avatar Rectangle</param>
        /// <param name="background">Background Image</param>
        public static void CreateBox(string msg, Rectangle msgBox, Rectangle textBox, Rectangle avatarBox, Texture2D background)
        {
            boxRectangle = msgBox;
            textRectangle = textBox;
            avatarRectangle = avatarBox;
            backgroundImage = background;
            Expired = false;
            stringIndex = 0;
            revealedMessage = "";
            MessageShown = false;

            // Set last so it breaks appropriately to fit the box (textRectangle MUST be set first)
            Message = msg;
        }

        /// <summary>
        /// Creates a new Conversation Window
        /// </summary>
        /// <param name="msg">Speaker Message</param>
        /// <param name="msgBox">Window Rectangle</param>
        /// <param name="textBox">Text Rectangle</param>
        /// <param name="avatarBox">Avatar Rectangle</param>
        public static void CreateBox(string msg, Rectangle msgBox, Rectangle textBox, Rectangle avatarBox)
        {
            CreateBox(msg, msgBox, textBox, avatarBox, backgroundImage);
        }

        /// <summary>
        /// Creates a new Conversation Window
        /// </summary>
        /// <param name="msg">Speaker Message</param>
        /// <param name="msgBox">Window Rectangle</param>
        /// <param name="textBox">Text Rectangle</param>
        public static void CreateBox(string msg, Rectangle msgBox, Rectangle textBox)
        {
            CreateBox(msg, msgBox, textBox, avatarRectangle, backgroundImage);
        }

        /// <summary>
        /// Creates a new Conversation Window
        /// </summary>
        /// <param name="msg">Speaker Message</param>
        /// <param name="msgBox">Window Rectangle</param>
        public static void CreateBox(string msg, Rectangle msgBox)
        {
            CreateBox(msg, msgBox, textRectangle, avatarRectangle, backgroundImage);
        }

        /// <summary>
        /// Creates a new Conversation Window
        /// </summary>
        /// <param name="msg">Speaker Message</param>
        public static void CreateBox(string msg)
        {
            CreateBox(msg, boxRectangle, textRectangle, avatarRectangle, backgroundImage);
        }

        /// <summary>
        /// Removes the Conversation Box
        /// </summary>
        public static void RemoveBox()
        {
            Expired = true;
        }

        /// <summary>
        /// Updates an existing Conversation Box
        /// </summary>
        /// <param name="msg">Message String</param>
        public static void UpdateBox(string msg)
        {
            Message = msg;
            Expired = false;
            stringIndex = 0;
            revealedMessage = "";
            MessageShown = false;
        }

        #endregion

        #region Saving and Loading Conversations

        /// <summary>
        /// Saves a Conversation File
        /// </summary>
        public static void SaveConversation()
        {
            // TOOD: File Location
            XmlSerializer serializer = new XmlSerializer(typeof(List<Speaker>));
            using (TextWriter textWriter = new StreamWriter(ConversationFileLocation + @"conversation.xml"))
            {
                serializer.Serialize(textWriter, ConversationSpeakers);
            }
        }

        /// <summary>
        /// Loads a Conversation File
        /// </summary>
        public static void LoadConversation()
        {
            // TODO: File Location
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Speaker>));
                using (TextReader textReader = new StreamReader(ConversationFileLocation + @"conversation.xml"))
                {
                    ConversationSpeakers = (List<Speaker>)deserializer.Deserialize(textReader);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Could not load conversations.");
                ClearConversation();
            }
        }

        /// <summary>
        /// Resets the Conversation Speakers List
        /// </summary>
        public static void ClearConversation()
        {
            ConversationSpeakers.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Breaks up a string so it fits in the box. Will split long messages into two or three if necessary
        /// </summary>
        /// <param name="message">Speaker Message String</param>
        /// <returns>Formatted String</returns>
        private static string constrainText(String message)
        {
            bool filled = false;
            string line = "";
            string returnString = "";
            string[] wordArray = message.Split(' ');

            // Go through each word in string
            foreach (string word in wordArray)
            {
                // If we add the next word to the current line and go beyond the width...
                if (spriteFont.MeasureString(line + word).X > textRectangle.Width)
                {
                    // If adding a new line doesn't put us beyond height
                    if (spriteFont.MeasureString(returnString + line + "\n").Y < textRectangle.Height)
                    {
                        returnString += line + "\n";
                        line = "";
                    }
                    // If adding a new line does put us beyond height
                    else if (!filled)
                    {
                        filled = true;
                        returnString += line;
                        line = "";
                    }
                }
                line += word + " ";
            }

            // We need to add another Speaker Object first
            if (filled)
            {
                ConversationSpeakers.Insert(currentSpeakerIndex + 1, new Speaker(ConversationSpeakers[currentSpeakerIndex].AvatarIndex, line));
                return returnString;
            }
            else
            {
                return returnString + line;
            }
        }

        #endregion

        #region Input Handling

        /// <summary>
        /// Handles User Input during a Conversation
        /// </summary>
        /// <param name="keyboardState">KeyboardState</param>
        private static void HandleKeyboardInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.E))
            {
                if (currentSpeakerIndex + 1 < ConversationSpeakers.Count)
                {
                    soundEffect.Play();
                    currentSpeakerIndex++;
                    revealedMessage = "";
                    stringIndex = 0;
                    ConversationSpeakers[currentSpeakerIndex].Message = constrainText(ConversationSpeakers[currentSpeakerIndex].Message);
                }
                else
                {
                    RemoveBox();
                }
                MessageShown = false;
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Update the Conversation Message Box
        /// </summary>
        /// <param name="gameTime">XNA GameTime</param>
        public static void Update(GameTime gameTime)
        {
            if (!Expired)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                messageTimer += elapsed;
                splitIconTimer += elapsed;

                if (messageTimer >= messageSpeed)
                {
                    // Typewriter Effect
                    if (stringIndex < ConversationSpeakers[currentSpeakerIndex].Message.Length)
                    {
                        revealedMessage += ConversationSpeakers[currentSpeakerIndex].Message[stringIndex];
                        stringIndex++;
                    }
                    // Full message displayed, handle input
                    else
                    {
                        MessageShown = true;
                        KeyboardState keyboardState = Keyboard.GetState();
                        HandleKeyboardInput(keyboardState);
                    }
                    messageTimer = 0.0f;
                }

                // Update Continue Reading Icon
                if (splitIconTimer >= splitIconSpeed)
                {
                    splitIconOffset = !splitIconOffset;
                    splitIconTimer = 0.0f;
                }
            }
        }

        /// <summary>
        /// Draws the Conversation Box to the Screen
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch</param>
        public static void Draw(ScreenManager screenManager)
        {
            if (!Expired)
            {
                SpriteBatch spriteBatch = screenManager.SpriteBatch;
                int x = (screenManager.Game.Window.ClientBounds.Width / 2) - (boxRectangle.Width / 2);
                int y = (screenManager.Game.Window.ClientBounds.Height / 2) + (100);
                // Only draw border if specified
                if (borderImage != null)
                {
                    spriteBatch.Draw(borderImage,
                        new Rectangle(x - borderWidth, y - borderWidth, boxRectangle.Width + 2 * borderWidth, boxRectangle.Height + 2 * borderWidth),
                        borderColor);
                }

                // Only draw Background if specified
                if (backgroundImage != null)
                {
                    spriteBatch.Draw(backgroundImage, new Rectangle(x, y, boxRectangle.Width, boxRectangle.Height), Color.White);
                }

                // Check to make sure we have the Avatar
                if (ConversationSpeakers[currentSpeakerIndex].AvatarIndex < Avatars.Count())
                {
                    spriteBatch.Draw(Avatars[ConversationSpeakers[currentSpeakerIndex].AvatarIndex], 
                        new Rectangle(x+20,y+15,avatarRectangle.Width,avatarRectangle.Height), Color.White);
                }


                // Draw the Message
                spriteBatch.DrawString(spriteFont, revealedMessage, new Vector2(x + 155, y), Color.White);

                // Check to see if we need to draw the Continue Reading icon
                if (MessageShown && currentSpeakerIndex + 1 < ConversationSpeakers.Count())
                {
                    Rectangle splitRectangle = new Rectangle(x + boxRectangle.Width - 2 * splitIcon.Width + splitIcon.Width / 2,
                                                             y + boxRectangle.Height - 2 * splitIcon.Height + splitIcon.Height / 2,
                                                             splitIcon.Width,
                                                             splitIcon.Height);

                    if (splitIconOffset)
                    {
                        splitRectangle.Y += splitIconOffsetValue;
                    }

                    spriteBatch.Draw(splitIcon, splitRectangle, Color.White);
                }
            }
        }

        #endregion
    }
}