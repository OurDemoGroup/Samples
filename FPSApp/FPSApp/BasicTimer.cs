using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Media;

namespace FPSApp
{
    /// <summary>
    /// Computes current frames per second(FPS) for Application
    /// </summary>
    public class BasicTimer
    {
        #region Fileds

        public double FPS { get { return frameRate; } }

        private TimeSpan delta;
        private Stopwatch stopwatch;
        private int frameCounter;
        private double frameRate;

        #endregion

        public BasicTimer()
        {
            delta = TimeSpan.FromSeconds(1);
            frameRate = 0;
            frameCounter = 0;
            stopwatch = Stopwatch.StartNew();

            System.Timers.Timer timer = new System.Timers.Timer();
            // raise per second
            timer.Interval = 1;
            timer.Elapsed += new ElapsedEventHandler(Update);
            timer.Start();

            // This event handler method gets called each time that WPF marshals the persisted rendering data 
            // in the visual tree across to the composition scene graph. 
            CompositionTarget.Rendering += Render;
        }

        #region Frame Rate Computation

        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (stopwatch.Elapsed > delta)
            {
                double elapsed = stopwatch.Elapsed.TotalSeconds;
                frameRate = (double)((double)frameCounter / elapsed);

                stopwatch.Reset();
                stopwatch.Start();
                Interlocked.Exchange(ref frameCounter, 0);
            }
        }

        // Called just before frame is rendered
        private void Render(object sender, EventArgs e)
        {
            Interlocked.Increment(ref frameCounter);
        }

        #endregion
        
    }
}
