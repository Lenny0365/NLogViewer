using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Infragistics.Win.UltraWinGrid;
using System.Diagnostics;
using Infragistics.Win;

namespace NLogViewer
{
    public partial class NLogViewer : UserControl
    {
        /// <summary>
        /// LogEntiesBind - collection of LogeventViewModel objects.
        /// </summary>
        public IBindingList LogEntiesBind { get; private set; }
        Timer ClipboardTimer = new Timer();

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
            Debug.WriteLine("NLogViewer - constractor");
            ClipboardTimer.Interval = 30000;
            ClipboardTimer.Tick += ClipboardTimer_Tick;
            ClipboardTimer.Start();
        }

        private void ClipboardTimer_Tick(object sender, EventArgs e)
        {
            ClipboardTimer.Stop();
            if (Clipboard.ContainsText())
            {
                Clipboard.Clear();
            }
        }


        private void NLogViewer_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Load - DesignMode: " + DesignMode);
            if (DesignMode == false)
            {
                foreach (NlogViewerTarget target in NLog.LogManager.Configuration.AllTargets.Where(t => t is NlogViewerTarget).Cast<NlogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                    Debug.WriteLine("Load - target.Name" + target.Name);
                }
            }
            Debug.WriteLine("Load - LogEntiesBind.Count: " + LogEntiesBind.Count);
            this.ugrLogEntries.DataSource = LogEntiesBind;
            FormatLogEntriesGrid();
        }

        /// <summary>
        /// LogReceived
        /// If LoggerName of the control is not set then we display entries form all loggers.
        /// If LoggerName  of the control is set the we will dispaly entries only for this specific logger.
        /// </summary>
        /// <param name="log"></param>
        protected void LogReceived(NLog.Common.AsyncLogEventInfo log)
        {
            LogEventViewModel vm = new LogEventViewModel(log.LogEvent);
            Debug.WriteLine("LogReceived: " + vm.LoggerName);
            if (IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if ((string.IsNullOrEmpty(this.LoggerName) == true) || (this.LoggerName == vm.LoggerName))
                    {
                        if (LogEntiesBind.Count >= NumberOfEntries)
                            LogEntiesBind.RemoveAt(0);

                        LogEntiesBind.Add(vm);
                        lblTotalEntries.Text = LogEntiesBind.Count.ToString();
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
            Debug.WriteLine("Grid formatted");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            LogEntiesBind.Clear();
            lblTotalEntries.Text = LogEntiesBind.Count.ToString();
        }

        private void ugrLogEntries_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                UIElement el = ugrLogEntries.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                UltraGridCell cell = GetCell(el);// Get cell from point coordinates
                if (cell != null)//if there is a cell
                {
                    object value = cell.Value;// get cell vale
                    //cell.Value = value;// set cell value
                    if ((value != null) && (string.IsNullOrEmpty(value.ToString()) == false))
                    {
                        string msg = string.Format("Following data will be copied to the clipboard{0} and will be cleared in 30 seconds:{0}{1}", Environment.NewLine, value.ToString());
                        if (MessageBox.Show(msg, "Data copied to clipboard", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            Clipboard.SetText(value.ToString());
                            ClipboardTimer.Start();
                        }
                    }
                }
            }
        }

        private UltraGridCell GetCell(UIElement element)
        {
            if (element == null || element.Parent == null)
                return null;
            if (element.Parent is CellUIElement)
                return ((CellUIElement)element.Parent).Cell;
            else
                return GetCell(element.Parent);
        }
    }

}
