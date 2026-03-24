using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EWTSS_DESKTOP.Helpers
{
    public static class ClockHelper
    {
        public static DispatcherTimer StartClock(TextBlock textBlock)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            // show time immediately
            textBlock.Text = DateTime.Now.ToString("HH:mm:ss");

            timer.Tick += (s, e) =>
            {
                textBlock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
  
            timer.Start();
            return timer;
        }
    }
}