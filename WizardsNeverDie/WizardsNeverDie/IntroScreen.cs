using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WizardsNeverDie.Level;
using WizardsNeverDie.ScreenSystem;

namespace WizardsNeverDie
{
    public class IntroScreen : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private static IntroScreen instance;

        public IntroScreen()
        {
            Window.Title = "Wizards Never Die";
            instance = this;

            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferMultiSampling = true;
#if WINDOWS || XBOX
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            IsFixedTimeStep = true;
#elif WINDOWS_PHONE
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(16f);
            IsFixedTimeStep = false;
#endif
#if WINDOWS
            _graphics.IsFullScreen = false;
#elif XBOX || WINDOWS_PHONE
            _graphics.IsFullScreen = true;
#endif
            Content.RootDirectory = "Content";

            //new-up components and add to Game.Components
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            FrameRateCounter frameRateCounter = new FrameRateCounter(ScreenManager);
            frameRateCounter.DrawOrder = 101;
            Components.Add(frameRateCounter);

            //added coordinates
            DisplayData displayInfo = new DisplayData(ScreenManager);
            displayInfo.DrawOrder = 101;
            Components.Add(displayInfo);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //load textures
            //Content.RootDirectory = "Content";
            //_textures.Add("Cannon", Content.Load<Texture2D>("Sprite\\cannon"));
            //_textures.Add("CannonBall", Content.Load<Texture2D>("Sprite\\ball")); 

            IfritDemo ifritDemo = new IfritDemo();
            Level1_0 level1 = new Level1_0();
            Level1_2 level3 = new Level1_2();
            Level1_1 level2 = new Level1_1();
            Level1_3 level4 = new Level1_3();
            MenuScreen menuScreen = new MenuScreen("");

            //menuScreen.AddMenuItem("Levels", EntryType.Separator, null);
            menuScreen.AddMenuItem(level1.GetTitle(), EntryType.Screen, level1);
            menuScreen.AddMenuItem(level2.GetTitle(), EntryType.Screen, level2);
            menuScreen.AddMenuItem(level3.GetTitle(), EntryType.Screen, level3);
            menuScreen.AddMenuItem(level4.GetTitle(), EntryType.Screen, level4);

            ScreenManager.AddScreen(new BackgroundScreen("Common/title_screen"));
            ScreenManager.AddScreen(menuScreen);
            ScreenManager.AddScreen(new LogoScreen(TimeSpan.FromSeconds(3.0)));
            base.Initialize();
        }

        //public static IntroScreen getInstance
        //{
        //    get{ return instance;}
        //}

        public ScreenManager ScreenManager { get; set; }
    }
}
