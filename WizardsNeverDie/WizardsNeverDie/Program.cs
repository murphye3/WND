using System;

namespace WizardsNeverDie
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (IntroScreen game = new IntroScreen())
            {
                game.Run();
            }
        }
    }
#endif
}

