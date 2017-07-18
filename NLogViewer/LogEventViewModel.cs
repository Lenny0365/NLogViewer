using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogViewer
{
    public class LogEventViewModel
    {
        private LogEventInfo logEventInfo;

        public LogEventViewModel(LogEventInfo logEventInfo)
        {
            // TODO: Complete member initialization
            this.logEventInfo = logEventInfo;

            ToolTip = logEventInfo.FormattedMessage;
            Level = logEventInfo.Level.ToString();
            FormattedMessage = logEventInfo.FormattedMessage;
            Exception = logEventInfo.Exception;
            LoggerName = logEventInfo.LoggerName;
            Time = logEventInfo.TimeStamp.ToString(CultureInfo.InvariantCulture);

            SetupColors(logEventInfo);
        }


        public string Time { get; private set; }

        [DisplayName("Logger")]
        public string LoggerName { get; private set; }
        public string Level { get; private set; }

        [DisplayName("Message")]
        public string FormattedMessage { get; private set; }
        public Exception Exception { get; private set; }
        public string ToolTip { get; private set; }
        public SolidBrush Background { get; private set; }
        public SolidBrush Foreground { get; private set; }
        public SolidBrush BackgroundMouseOver { get; private set; }
        public SolidBrush ForegroundMouseOver { get; private set; }

        private void SetupColors(LogEventInfo logEventInfo)
        {
            if (logEventInfo.Level == LogLevel.Warn)
            {
                Background = Brushes.Yellow as SolidBrush;
                BackgroundMouseOver = Brushes.GreenYellow as SolidBrush;
            }
            else if (logEventInfo.Level == LogLevel.Error)
            {
                Background = Brushes.Tomato as SolidBrush;
                BackgroundMouseOver = Brushes.IndianRed as SolidBrush;
            }
            else
            {
                Background = Brushes.White as SolidBrush;
                BackgroundMouseOver = Brushes.LightGray as SolidBrush;
            }
            Foreground = Brushes.Black as SolidBrush;
            ForegroundMouseOver = Brushes.Black as SolidBrush;
        }
    }
}
