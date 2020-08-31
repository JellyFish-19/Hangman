using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace LESSON_SEVEN
{
    class Soundtrack
    {
        public void MainGame()
        {
            SoundPlayer mainTheme = new SoundPlayer();
            mainTheme.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Super Mario Bros - Castle Theme.wav";
            mainTheme.Play();
        }

        public void Victory()
        {
            SoundPlayer victorySound = new SoundPlayer();
            victorySound.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Mario Stage Win Sound Effect.wav";
            victorySound.Play();
        }

        public void Death()
        {
            SoundPlayer gameOverSound = new SoundPlayer();
            gameOverSound.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Mario Death Sound Effect.wav";
            gameOverSound.Play();
        }
    }
}
