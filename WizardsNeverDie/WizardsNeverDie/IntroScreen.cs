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
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
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

            Level1_0 stage0 = new Level1_0();
            Level1_1 stage1 = new Level1_1();
            Level1_2 stage2 = new Level1_2();
            Level1_3 stage3 = new Level1_3();
            Level1_4 stage4 = new Level1_4();
            MenuScreen menuScreen = new MenuScreen("", _graphics);

            menuScreen.AddMenuItem(stage0.GetTitle(), EntryType.Screen, stage0);
            menuScreen.AddMenuItem(stage1.GetTitle(), EntryType.Screen, stage1);
            menuScreen.AddMenuItem(stage2.GetTitle(), EntryType.Screen, stage2);
            menuScreen.AddMenuItem(stage3.GetTitle(), EntryType.Screen, stage3);
            menuScreen.AddMenuItem(stage4.GetTitle(), EntryType.Screen, stage4);
            List<string> resolutions = new List<string>();
            resolutions.Add(Resolution.Res1280x720.ToString());
            resolutions.Add(Resolution.Res1600x900.ToString());
            resolutions.Add(Resolution.Res1920x1080.ToString());
            List<string> fullscreen = new List<string>();
            fullscreen.Add(Display.NotFullScreen.ToString());
            fullscreen.Add(Display.FullScreen.ToString());

            menuScreen.AddMenuItem(resolutions, EntryType.Resolution, null);
            menuScreen.AddMenuItem(fullscreen, EntryType.Display, null);
            menuScreen.AddMenuItem("Exit", EntryType.ExitItem, null);

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
