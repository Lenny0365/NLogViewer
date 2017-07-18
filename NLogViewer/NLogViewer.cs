using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace NLogViewer
{
    public partial class NLogViewer : UserControl
    {
        /// <summary>
        /// LogEntiesBind - collection of LogeventViewModel objects.
        /// </summary>
        public IBindingList LogEntiesBind { get; private set; }

        /// <summary>
        /// IsTargetConfigured 
        /// </summary>
        public bool IsTargetConfigured { get; private set; }

        [Description("Width of time column in pixels"), Category("Data")]
        //[TypeConverterAttribute(typeof(LengthConverter))]
        public double TimeWidth { get; set; }

        [Description("Width of Logger column in pixels, or auto if not specified"), Category("Data")]
        //[TypeConverterAttribute(typeof(LengthConverter))]
        public double LoggerNameWidth { set; get; }

        [Description("Width of Level column in pixels"), Category("Data")]
        //[TypeConverterAttribute(typeof(LengthConverter))]
        public double LevelWidth { get; set; }
        [Description("Width of Message column in pixels"), Category("Data")]
        // [TypeConverterAttribute(typeof(LengthConverter))]
        public double MessageWidth { get; set; }
        [Description("Width of Exception column in pixels"), Category("Data")]
        // [TypeConverterAttribute(typeof(LengthConverter))]
        public double ExceptionWidth { get; set; }

        [Description("Name of associated logger"), Category("Data")]
        [Browsable(true)]
        public string LoggerName { get; set; }

        [Description("Number of log entries to keep"), Category("Data")]
        [Browsable(true)]
        [System.ComponentModel.DefaultValue(50)]
        public int NumberOfEntries
        {
            get { return _NumberOfEntries; }
            set { _NumberOfEntries = value; }
        }
        private int _NumberOfEntries = 50;

        public NLogViewer()
        {
            IsTargetConfigured = false;
            LogEntiesBind = new BindingList<LogEventViewModel>();

            InitializeComponent();
        }

        private void NLogViewer_Load(object sender, EventArgs e)
        {
            if (DesignMode == false)
            {
                foreach (NlogViewerTarget target in NLog.LogManager.Configuration.AllTargets.Where(t => t is NlogViewerTarget).Cast<NlogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                }
            }

            this.ugrLogEntries.DataSource = LogEntiesBind;
            FormatLogEntriesGrid();
        }

        protected void LogReceived(NLog.Common.AsyncLogEventInfo log)
        {
            LogEventViewModel vm = new LogEventViewModel(log.LogEvent);
            if (IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (this.LoggerName == vm.LoggerName)
                    {
                        if (LogEntiesBind.Count >= NumberOfEntries)
                            LogEntiesBind.RemoveAt(0);

                        LogEntiesBind.Add(vm);
                    }
                }));
            }
        }

        #region Grid Events/ methods
        private void FormatLogEntriesGrid()
        {
            List<string> VisibleColumns = new List<string>();

            VisibleColumns.Clear();
            VisibleColumns.Add("Time");
            VisibleColumns.Add("LoggerName");
            VisibleColumns.Add("Level");
            VisibleColumns.Add("FormattedMessage");
            VisibleColumns.Add("Exception");
            SetAllGridColumnVisibility(this.ugrLogEntries, VisibleColumns);
            ugrLogEntries.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
        }
        /// <summary>
        /// SetAllGridColumnVisibility
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="ColumnsToShow"></param>
        public void SetAllGridColumnVisibility(UltraGrid grid, List<string> ColumnsToShow)
        {
            foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].Columns)
            {
                if (ColumnsToShow.Any(p => p == col.Key))
                {
                    col.Hidden = false;
                }
                else
                {
                    col.Hidden = true;
                }
            }
        }
        private void ugrLogEntries_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            LogEventViewModel temp = (LogEventViewModel)LogEntiesBind[e.Row.Index];
            e.Row.Appearance.BackColor = temp.Background.Color;
            e.Row.Appearance.ForeColor = temp.Foreground.Color;
            ugrLogEntries.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
        }

        private void ugrLogEntries_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.HotTrackRowCellAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
        }

        Color originalBackColor;
        private void ugrLogEntries_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            UltraGridRow row;
            row = (UltraGridRow)e.Element.GetContext(typeof(UltraGridRow));
            if (row != null)
            {

                SolidBrush backBrushMouse = (SolidBrush)row.GetCellValue("BackgroundMouseOver");
                SolidBrush backBrush = (SolidBrush)row.GetCellValue("Background");
                row.Appearance.BackColor = backBrushMouse.Color;
                originalBackColor = backBrush.Color;
            }
        }

        private void ugrLogEntries_MouseLeaveElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            UltraGridRow row;
            row = (UltraGridRow)e.Element.GetContext(typeof(UltraGridRow));
            if (row != null)
            {
                row.Appearance.BackColor = originalBackColor;
            }
        }
        #endregion
    }
    
}
