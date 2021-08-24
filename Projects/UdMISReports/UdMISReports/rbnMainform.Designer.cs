using System.Windows.Forms;

namespace UdMISReports
{
    partial class rbnMainform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbiPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.bbiRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.bbiPrint = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExport = new DevExpress.XtraBars.BarButtonItem();
            this.bsubExport = new DevExpress.XtraBars.BarSubItem();
            this.bbiExportToXls = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToXlsx = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToHTML = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToPDF = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToRTF = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToText = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExportToCSV = new DevExpress.XtraBars.BarButtonItem();
            this.bsubEmail = new DevExpress.XtraBars.BarSubItem();
            this.bbiEmailToXls = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEmailToXlsx = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEmailToPdf = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEmailToCSV = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEmailToRTF = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEmailToText = new DevExpress.XtraBars.BarButtonItem();
            this.bbiOpen = new DevExpress.XtraBars.BarButtonItem();
            this.bbiSave = new DevExpress.XtraBars.BarButtonItem();
            this.bbiChart = new DevExpress.XtraBars.BarButtonItem();
            this.bbiTopNValues = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExit = new DevExpress.XtraBars.BarButtonItem();
            this.bsiView = new DevExpress.XtraBars.BarSubItem();
            this.bciShowRowTotal = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowRowGrandTotal = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowRowHeader = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowColTotal = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowColGrandTotal = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowColHeader = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowDataHeader = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowFilterHeader = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowHoriLines = new DevExpress.XtraBars.BarCheckItem();
            this.bciShowVertLines = new DevExpress.XtraBars.BarCheckItem();
            this.srgbiSkin = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.bsiUserName = new DevExpress.XtraBars.BarStaticItem();
            this.bsiRecords = new DevExpress.XtraBars.BarStaticItem();
            this.bbiClear = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDelete = new DevExpress.XtraBars.BarButtonItem();
            this.bbiData = new DevExpress.XtraBars.BarButtonItem();
            this.bbiBoth = new DevExpress.XtraBars.BarButtonItem();
            this.rbpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpRefresh = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpPrint = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpView = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpExport = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpEmail = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpAppearance = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpLayout = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpChart = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpTopNValue = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpExit = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpOptions = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl3 = new DevExpress.XtraEditors.SplitContainerControl();
            this.icbField = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.lblfldValue = new DevExpress.XtraEditors.LabelControl();
            this.seTop = new DevExpress.XtraEditors.SpinEdit();
            this.lblShowTop = new DevExpress.XtraEditors.LabelControl();
            this.comboChartType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.pgMain = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.chartMain = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icbField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboChartType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.bbiPrintPreview,
            this.bbiRefresh,
            this.bbiPrint,
            this.bbiExport,
            this.bsubExport,
            this.bbiExportToXls,
            this.bbiExportToXlsx,
            this.bbiExportToHTML,
            this.bbiExportToPDF,
            this.bbiExportToRTF,
            this.bbiExportToText,
            this.bbiExportToCSV,
            this.bsubEmail,
            this.bbiEmailToXls,
            this.bbiEmailToXlsx,
            this.bbiEmailToPdf,
            this.bbiEmailToCSV,
            this.bbiEmailToRTF,
            this.bbiEmailToText,
            this.bbiOpen,
            this.bbiSave,
            this.bbiChart,
            this.bbiTopNValues,
            this.bbiExit,
            this.bsiView,
            this.bciShowRowTotal,
            this.bciShowRowGrandTotal,
            this.bciShowRowHeader,
            this.bciShowColTotal,
            this.bciShowColGrandTotal,
            this.bciShowColHeader,
            this.bciShowDataHeader,
            this.bciShowFilterHeader,
            this.bciShowHoriLines,
            this.bciShowVertLines,
            this.srgbiSkin,
            this.bsiUserName,
            this.bsiRecords,
            this.bbiClear,
            this.bbiDelete,
            this.bbiData,
            this.bbiBoth});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 106;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rbpHome});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.Size = new System.Drawing.Size(898, 143);
            this.ribbonControl.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // bbiPrintPreview
            // 
            this.bbiPrintPreview.Caption = "Print Preview";
            this.bbiPrintPreview.Id = 14;
            this.bbiPrintPreview.ImageUri.Uri = "Preview";
            this.bbiPrintPreview.Name = "bbiPrintPreview";
            this.bbiPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintPreview_ItemClick);
            // 
            // bbiRefresh
            // 
            this.bbiRefresh.Caption = "Refresh";
            this.bbiRefresh.Id = 19;
            this.bbiRefresh.ImageUri.Uri = "Refresh";
            this.bbiRefresh.Name = "bbiRefresh";
            this.bbiRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRefresh_ItemClick);
            // 
            // bbiPrint
            // 
            this.bbiPrint.Caption = "Print";
            this.bbiPrint.Id = 20;
            this.bbiPrint.ImageUri.Uri = "Print";
            this.bbiPrint.Name = "bbiPrint";
            this.bbiPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrint_ItemClick);
            // 
            // bbiExport
            // 
            this.bbiExport.Caption = "Export";
            this.bbiExport.Id = 45;
            this.bbiExport.ImageUri.Uri = "ExportFile";
            this.bbiExport.Name = "bbiExport";
            // 
            // bsubExport
            // 
            this.bsubExport.Caption = "Export";
            this.bsubExport.Id = 46;
            this.bsubExport.ImageUri.Uri = "ExportFile";
            this.bsubExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToXls),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToXlsx),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToHTML),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToPDF),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToRTF),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToText),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExportToCSV)});
            this.bsubExport.Name = "bsubExport";
            // 
            // bbiExportToXls
            // 
            this.bbiExportToXls.Caption = "Export To Xls";
            this.bbiExportToXls.Id = 49;
            this.bbiExportToXls.ImageUri.Uri = "ExportToXLS";
            this.bbiExportToXls.Name = "bbiExportToXls";
            this.bbiExportToXls.Tag = "XLS";
            // 
            // bbiExportToXlsx
            // 
            this.bbiExportToXlsx.Caption = "Export To Xlsx";
            this.bbiExportToXlsx.Id = 50;
            this.bbiExportToXlsx.ImageUri.Uri = "ExportToXLSX";
            this.bbiExportToXlsx.Name = "bbiExportToXlsx";
            this.bbiExportToXlsx.Tag = "XLSX";
            // 
            // bbiExportToHTML
            // 
            this.bbiExportToHTML.Caption = "Export To HTML";
            this.bbiExportToHTML.Id = 51;
            this.bbiExportToHTML.ImageUri.Uri = "ExportToHTML";
            this.bbiExportToHTML.Name = "bbiExportToHTML";
            this.bbiExportToHTML.Tag = "HTML";
            // 
            // bbiExportToPDF
            // 
            this.bbiExportToPDF.Caption = "Export To PDF";
            this.bbiExportToPDF.Id = 52;
            this.bbiExportToPDF.ImageUri.Uri = "ExportToPDF";
            this.bbiExportToPDF.Name = "bbiExportToPDF";
            this.bbiExportToPDF.Tag = "PDF";
            // 
            // bbiExportToRTF
            // 
            this.bbiExportToRTF.Caption = "Export to RTF";
            this.bbiExportToRTF.Id = 53;
            this.bbiExportToRTF.ImageUri.Uri = "ExportToRTF";
            this.bbiExportToRTF.Name = "bbiExportToRTF";
            this.bbiExportToRTF.Tag = "RTF";
            // 
            // bbiExportToText
            // 
            this.bbiExportToText.Caption = "Export to Text";
            this.bbiExportToText.Id = 54;
            this.bbiExportToText.ImageUri.Uri = "ExportToTXT";
            this.bbiExportToText.Name = "bbiExportToText";
            this.bbiExportToText.Tag = "TXT";
            // 
            // bbiExportToCSV
            // 
            this.bbiExportToCSV.Caption = "Export to CSV";
            this.bbiExportToCSV.Id = 56;
            this.bbiExportToCSV.ImageUri.Uri = "ExportToCSV";
            this.bbiExportToCSV.Name = "bbiExportToCSV";
            this.bbiExportToCSV.Tag = "CSV";
            // 
            // bsubEmail
            // 
            this.bsubEmail.Caption = "E-mail";
            this.bsubEmail.Id = 57;
            this.bsubEmail.ImageUri.Uri = "Previous";
            this.bsubEmail.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToXls),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToXlsx),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToPdf),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToCSV),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToRTF),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiEmailToText)});
            this.bsubEmail.Name = "bsubEmail";
            // 
            // bbiEmailToXls
            // 
            this.bbiEmailToXls.Caption = "Excel File(.XLS)";
            this.bbiEmailToXls.Id = 60;
            this.bbiEmailToXls.ImageUri.Uri = "SendXLS";
            this.bbiEmailToXls.Name = "bbiEmailToXls";
            this.bbiEmailToXls.Tag = "XLS";
            // 
            // bbiEmailToXlsx
            // 
            this.bbiEmailToXlsx.Caption = "Excel File(.XLSX)";
            this.bbiEmailToXlsx.Id = 61;
            this.bbiEmailToXlsx.ImageUri.Uri = "SendXLSX";
            this.bbiEmailToXlsx.Name = "bbiEmailToXlsx";
            this.bbiEmailToXlsx.Tag = "XLSX";
            // 
            // bbiEmailToPdf
            // 
            this.bbiEmailToPdf.Caption = "Pdf File(.PDF)";
            this.bbiEmailToPdf.Id = 62;
            this.bbiEmailToPdf.ImageUri.Uri = "SendPDF";
            this.bbiEmailToPdf.Name = "bbiEmailToPdf";
            this.bbiEmailToPdf.Tag = "PDF";
            // 
            // bbiEmailToCSV
            // 
            this.bbiEmailToCSV.Caption = "CSV File(.CSV)";
            this.bbiEmailToCSV.Id = 64;
            this.bbiEmailToCSV.ImageUri.Uri = "SendCSV";
            this.bbiEmailToCSV.Name = "bbiEmailToCSV";
            this.bbiEmailToCSV.Tag = "CSV";
            // 
            // bbiEmailToRTF
            // 
            this.bbiEmailToRTF.Caption = "RTF File(.RTF)";
            this.bbiEmailToRTF.Id = 65;
            this.bbiEmailToRTF.ImageUri.Uri = "SendRTF";
            this.bbiEmailToRTF.Name = "bbiEmailToRTF";
            this.bbiEmailToRTF.Tag = "RTF";
            // 
            // bbiEmailToText
            // 
            this.bbiEmailToText.Caption = "Text File(.TXT)";
            this.bbiEmailToText.Id = 66;
            this.bbiEmailToText.ImageUri.Uri = "SendTXT";
            this.bbiEmailToText.Name = "bbiEmailToText";
            this.bbiEmailToText.Tag = "TXT";
            // 
            // bbiOpen
            // 
            this.bbiOpen.Caption = "Open";
            this.bbiOpen.Id = 69;
            this.bbiOpen.ImageUri.Uri = "Open";
            this.bbiOpen.Name = "bbiOpen";
            this.bbiOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiOpen_ItemClick);
            // 
            // bbiSave
            // 
            this.bbiSave.Caption = "Save";
            this.bbiSave.Id = 70;
            this.bbiSave.ImageUri.Uri = "Save";
            this.bbiSave.Name = "bbiSave";
            this.bbiSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiSave_ItemClick);
            // 
            // bbiChart
            // 
            this.bbiChart.Caption = "Show Chart";
            this.bbiChart.Id = 71;
            this.bbiChart.ImageUri.Uri = "Chart";
            this.bbiChart.Name = "bbiChart";
            this.bbiChart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiChart_ItemClick);
            // 
            // bbiTopNValues
            // 
            this.bbiTopNValues.Caption = "Top N Values";
            this.bbiTopNValues.Id = 74;
            this.bbiTopNValues.ImageUri.Uri = "Filter";
            this.bbiTopNValues.Name = "bbiTopNValues";
            this.bbiTopNValues.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiTopNValues_ItemClick);
            // 
            // bbiExit
            // 
            this.bbiExit.Caption = "Exit";
            this.bbiExit.Id = 75;
            this.bbiExit.ImageUri.Uri = "Close";
            this.bbiExit.Name = "bbiExit";
            this.bbiExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiExit_ItemClick);
            // 
            // bsiView
            // 
            this.bsiView.Caption = "&View";
            this.bsiView.Id = 87;
            this.bsiView.ImageUri.Uri = "MoreLayoutOptions";
            this.bsiView.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowRowTotal),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowRowGrandTotal),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowRowHeader),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowColTotal),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowColGrandTotal),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowColHeader),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowDataHeader),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowFilterHeader),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowHoriLines),
            new DevExpress.XtraBars.LinkPersistInfo(this.bciShowVertLines)});
            this.bsiView.Name = "bsiView";
            // 
            // bciShowRowTotal
            // 
            this.bciShowRowTotal.Caption = "Show Row Total";
            this.bciShowRowTotal.Id = 89;
            this.bciShowRowTotal.Name = "bciShowRowTotal";
            // 
            // bciShowRowGrandTotal
            // 
            this.bciShowRowGrandTotal.Caption = "Show Row Grand Total";
            this.bciShowRowGrandTotal.Id = 90;
            this.bciShowRowGrandTotal.Name = "bciShowRowGrandTotal";
            // 
            // bciShowRowHeader
            // 
            this.bciShowRowHeader.Caption = "Show Row Header";
            this.bciShowRowHeader.Id = 91;
            this.bciShowRowHeader.Name = "bciShowRowHeader";
            // 
            // bciShowColTotal
            // 
            this.bciShowColTotal.Caption = "Show Column Total";
            this.bciShowColTotal.Id = 92;
            this.bciShowColTotal.Name = "bciShowColTotal";
            // 
            // bciShowColGrandTotal
            // 
            this.bciShowColGrandTotal.Caption = "Show Column Grand Total";
            this.bciShowColGrandTotal.Id = 93;
            this.bciShowColGrandTotal.Name = "bciShowColGrandTotal";
            // 
            // bciShowColHeader
            // 
            this.bciShowColHeader.Caption = "Show Column Header";
            this.bciShowColHeader.Id = 94;
            this.bciShowColHeader.Name = "bciShowColHeader";
            // 
            // bciShowDataHeader
            // 
            this.bciShowDataHeader.Caption = "Show Data Header";
            this.bciShowDataHeader.Id = 95;
            this.bciShowDataHeader.Name = "bciShowDataHeader";
            // 
            // bciShowFilterHeader
            // 
            this.bciShowFilterHeader.Caption = "Show Filter Header";
            this.bciShowFilterHeader.Id = 96;
            this.bciShowFilterHeader.Name = "bciShowFilterHeader";
            // 
            // bciShowHoriLines
            // 
            this.bciShowHoriLines.Caption = "Show Horizontal Lines";
            this.bciShowHoriLines.Id = 97;
            this.bciShowHoriLines.Name = "bciShowHoriLines";
            // 
            // bciShowVertLines
            // 
            this.bciShowVertLines.Caption = "Show Vertical Lines";
            this.bciShowVertLines.Id = 98;
            this.bciShowVertLines.Name = "bciShowVertLines";
            // 
            // srgbiSkin
            // 
            this.srgbiSkin.Caption = "Appearance";
            this.srgbiSkin.Id = 100;
            this.srgbiSkin.Name = "srgbiSkin";
            this.srgbiSkin.GalleryItemClick += new DevExpress.XtraBars.Ribbon.GalleryItemClickEventHandler(this.srgbiSkin_GalleryItemClick);
            // 
            // bsiUserName
            // 
            this.bsiUserName.Caption = "Username : ";
            this.bsiUserName.Id = 101;
            this.bsiUserName.Name = "bsiUserName";
            this.bsiUserName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bsiRecords
            // 
            this.bsiRecords.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bsiRecords.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.bsiRecords.Caption = "Records : 0";
            this.bsiRecords.Id = 15;
            this.bsiRecords.Name = "bsiRecords";
            this.bsiRecords.TextAlignment = System.Drawing.StringAlignment.Near;
            this.bsiRecords.Width = 100;
            // 
            // bbiClear
            // 
            this.bbiClear.Caption = "Clear";
            this.bbiClear.Id = 102;
            this.bbiClear.ImageUri.Uri = "ClearFormatting";
            this.bbiClear.Name = "bbiClear";
            this.bbiClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiClear_ItemClick);
            // 
            // bbiDelete
            // 
            this.bbiDelete.Caption = "Delete";
            this.bbiDelete.Id = 103;
            this.bbiDelete.ImageUri.Uri = "Delete";
            this.bbiDelete.Name = "bbiDelete";
            this.bbiDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiDelete_ItemClick);
            // 
            // bbiData
            // 
            this.bbiData.Caption = "Show Data";
            this.bbiData.Id = 104;
            this.bbiData.ImageUri.Uri = "Summary";
            this.bbiData.Name = "bbiData";
            this.bbiData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiData_ItemClick);
            // 
            // bbiBoth
            // 
            this.bbiBoth.Caption = "Show Both";
            this.bbiBoth.Id = 105;
            this.bbiBoth.ImageUri.Uri = "Show";
            this.bbiBoth.Name = "bbiBoth";
            this.bbiBoth.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiBoth_ItemClick);
            // 
            // rbpHome
            // 
            this.rbpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpRefresh,
            this.rbpPrint,
            this.rbpView,
            this.rbpExport,
            this.rbpEmail,
            this.rbpAppearance,
            this.rbpLayout,
            this.rbpChart,
            this.rbpTopNValue,
            this.rbpExit});
            this.rbpHome.MergeOrder = 0;
            this.rbpHome.Name = "rbpHome";
            this.rbpHome.Text = "Home";
            // 
            // rbpRefresh
            // 
            this.rbpRefresh.AllowTextClipping = false;
            this.rbpRefresh.ItemLinks.Add(this.bbiRefresh);
            this.rbpRefresh.Name = "rbpRefresh";
            this.rbpRefresh.ShowCaptionButton = false;
            this.rbpRefresh.Tag = "";
            // 
            // rbpPrint
            // 
            this.rbpPrint.AllowTextClipping = false;
            this.rbpPrint.ItemLinks.Add(this.bbiPrintPreview);
            this.rbpPrint.ItemLinks.Add(this.bbiPrint);
            this.rbpPrint.Name = "rbpPrint";
            this.rbpPrint.ShowCaptionButton = false;
            this.rbpPrint.Text = "Print";
            // 
            // rbpView
            // 
            this.rbpView.ItemLinks.Add(this.bsiView);
            this.rbpView.Name = "rbpView";
            // 
            // rbpExport
            // 
            this.rbpExport.ItemLinks.Add(this.bsubExport);
            this.rbpExport.Name = "rbpExport";
            // 
            // rbpEmail
            // 
            this.rbpEmail.ItemLinks.Add(this.bsubEmail);
            this.rbpEmail.Name = "rbpEmail";
            // 
            // rbpAppearance
            // 
            this.rbpAppearance.ItemLinks.Add(this.srgbiSkin);
            this.rbpAppearance.Name = "rbpAppearance";
            this.rbpAppearance.Text = "Appearance";
            // 
            // rbpLayout
            // 
            this.rbpLayout.ItemLinks.Add(this.bbiOpen);
            this.rbpLayout.ItemLinks.Add(this.bbiSave);
            this.rbpLayout.ItemLinks.Add(this.bbiDelete);
            this.rbpLayout.ItemLinks.Add(this.bbiClear);
            this.rbpLayout.Name = "rbpLayout";
            this.rbpLayout.Text = "Layout";
            // 
            // rbpChart
            // 
            this.rbpChart.ItemLinks.Add(this.bbiChart);
            this.rbpChart.ItemLinks.Add(this.bbiData);
            this.rbpChart.ItemLinks.Add(this.bbiBoth);
            this.rbpChart.Name = "rbpChart";
            // 
            // rbpTopNValue
            // 
            this.rbpTopNValue.ItemLinks.Add(this.bbiTopNValues);
            this.rbpTopNValue.Name = "rbpTopNValue";
            // 
            // rbpExit
            // 
            this.rbpExit.ItemLinks.Add(this.bbiExit);
            this.rbpExit.Name = "rbpExit";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.bsiUserName);
            this.ribbonStatusBar1.ItemLinks.Add(this.bsiRecords);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 464);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(898, 31);
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.AllowTextClipping = false;
            this.ribbonPageGroup4.ItemLinks.Add(this.bbiRefresh);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.ShowCaptionButton = false;
            this.ribbonPageGroup4.Tag = "";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.AllowTextClipping = false;
            this.ribbonPageGroup5.ItemLinks.Add(this.bbiRefresh);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.ShowCaptionButton = false;
            this.ribbonPageGroup5.Text = "Tasks";
            // 
            // rbpOptions
            // 
            this.rbpOptions.AllowTextClipping = false;
            this.rbpOptions.Name = "rbpOptions";
            this.rbpOptions.ShowCaptionButton = false;
            this.rbpOptions.Text = "Options";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 143);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl3);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(898, 321);
            this.splitContainerControl1.SplitterPosition = 34;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl3.Name = "splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add(this.icbField);
            this.splitContainerControl3.Panel1.Controls.Add(this.lblfldValue);
            this.splitContainerControl3.Panel1.Controls.Add(this.seTop);
            this.splitContainerControl3.Panel1.Controls.Add(this.lblShowTop);
            this.splitContainerControl3.Panel1.Text = "Panel1";
            this.splitContainerControl3.Panel2.Controls.Add(this.comboChartType);
            this.splitContainerControl3.Panel2.Controls.Add(this.labelControl2);
            this.splitContainerControl3.Panel2.Text = "Panel2";
            this.splitContainerControl3.Size = new System.Drawing.Size(898, 34);
            this.splitContainerControl3.SplitterPosition = 444;
            this.splitContainerControl3.TabIndex = 0;
            this.splitContainerControl3.Text = "splitContainerControl3";
            // 
            // icbField
            // 
            this.icbField.EditValue = "imageComboBoxEdit1";
            this.icbField.Location = new System.Drawing.Point(262, 7);
            this.icbField.Name = "icbField";
            this.icbField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.icbField.Size = new System.Drawing.Size(168, 20);
            this.icbField.TabIndex = 10;
            this.icbField.Visible = false;
            this.icbField.SelectedIndexChanged += new System.EventHandler(this.icbField_SelectedIndexChanged);
            // 
            // lblfldValue
            // 
            this.lblfldValue.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblfldValue.Location = new System.Drawing.Point(152, 10);
            this.lblfldValue.Name = "lblfldValue";
            this.lblfldValue.Size = new System.Drawing.Size(99, 13);
            this.lblfldValue.TabIndex = 12;
            this.lblfldValue.Text = "Values for the Field: ";
            this.lblfldValue.Visible = false;
            // 
            // seTop
            // 
            this.seTop.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.seTop.Location = new System.Drawing.Point(74, 7);
            this.seTop.Name = "seTop";
            this.seTop.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seTop.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.seTop.Properties.IsFloatValue = false;
            this.seTop.Properties.Mask.EditMask = "N00";
            this.seTop.Properties.MaxValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.seTop.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seTop.Size = new System.Drawing.Size(72, 20);
            this.seTop.TabIndex = 9;
            this.seTop.Visible = false;
            this.seTop.EditValueChanged += new System.EventHandler(this.seTop_EditValueChanged);
            // 
            // lblShowTop
            // 
            this.lblShowTop.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblShowTop.Location = new System.Drawing.Point(14, 10);
            this.lblShowTop.Name = "lblShowTop";
            this.lblShowTop.Size = new System.Drawing.Size(51, 13);
            this.lblShowTop.TabIndex = 11;
            this.lblShowTop.Text = "Show Top:";
            this.lblShowTop.Visible = false;
            // 
            // comboChartType
            // 
            this.comboChartType.EditValue = "Line";
            this.comboChartType.Location = new System.Drawing.Point(81, 6);
            this.comboChartType.Name = "comboChartType";
            this.comboChartType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboChartType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboChartType.Size = new System.Drawing.Size(154, 20);
            this.comboChartType.TabIndex = 5;
            this.comboChartType.SelectedIndexChanged += new System.EventHandler(this.comboChartType_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 9);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(58, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Chart Type:";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.pgMain);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.chartMain);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(898, 282);
            this.splitContainerControl2.SplitterPosition = 444;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // pgMain
            // 
            this.pgMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMain.Location = new System.Drawing.Point(0, 0);
            this.pgMain.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.pgMain.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pgMain.Name = "pgMain";
            this.pgMain.OptionsLayout.AddNewGroups = true;
            this.pgMain.OptionsLayout.Columns.StoreAllOptions = true;
            this.pgMain.OptionsLayout.Columns.StoreAppearance = true;
            this.pgMain.OptionsLayout.StoreAllOptions = true;
            this.pgMain.OptionsLayout.StoreAppearance = true;
            this.pgMain.OptionsLayout.StoreFormatRules = true;
            this.pgMain.OptionsLayout.StoreLayoutOptions = true;
            this.pgMain.Size = new System.Drawing.Size(444, 282);
            this.pgMain.TabIndex = 3;
            // 
            // chartMain
            // 
            this.chartMain.DataBindings = null;
            this.chartMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMain.Legend.Name = "Default Legend";
            this.chartMain.Location = new System.Drawing.Point(0, 0);
            this.chartMain.Name = "chartMain";
            this.chartMain.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartMain.Size = new System.Drawing.Size(449, 282);
            this.chartMain.TabIndex = 0;
            // 
            // rbnMainform
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 495);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl);
            this.MaximizeBox = false;
            this.Name = "rbnMainform";
            this.Ribbon = this.ribbonControl;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Udyog MIS Reports";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.rbnMainform_FormClosed);
            this.Load += new System.EventHandler(this.rbnMainform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).EndInit();
            this.splitContainerControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.icbField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboChartType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pgMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private DevExpress.XtraBars.BarButtonItem bbiPrintPreview;
        private DevExpress.XtraBars.BarButtonItem bbiRefresh;
        private DevExpress.XtraBars.BarButtonItem bbiPrint;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpRefresh;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpPrint;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarStaticItem bsiRecords;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpExport;
        private DevExpress.XtraBars.BarButtonItem bbiExport;
        private DevExpress.XtraBars.BarSubItem bsubExport;
        private DevExpress.XtraBars.BarButtonItem bbiExportToXls;
        private DevExpress.XtraBars.BarButtonItem bbiExportToXlsx;
        private DevExpress.XtraBars.BarButtonItem bbiExportToPDF;
        private DevExpress.XtraBars.BarButtonItem bbiExportToRTF;
        private DevExpress.XtraBars.BarButtonItem bbiExportToText;
        private DevExpress.XtraBars.BarButtonItem bbiExportToCSV;
        private DevExpress.XtraBars.BarSubItem bsubEmail;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpEmail;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToXls;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToXlsx;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToPdf;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToCSV;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToRTF;
        private DevExpress.XtraBars.BarButtonItem bbiEmailToText;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpLayout;
        private DevExpress.XtraBars.BarButtonItem bbiOpen;
        private DevExpress.XtraBars.BarButtonItem bbiSave;
        private DevExpress.XtraBars.BarButtonItem bbiChart;
        private DevExpress.XtraBars.BarButtonItem bbiTopNValues;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpChart;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpTopNValue;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpExit;
        private DevExpress.XtraBars.BarButtonItem bbiExit;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpAppearance;
        private DevExpress.XtraBars.BarButtonItem bbiExportToHTML;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpView;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpOptions;
        private DevExpress.XtraBars.BarSubItem bsiView;
        private DevExpress.XtraBars.BarCheckItem bciShowRowTotal;
        private DevExpress.XtraBars.BarCheckItem bciShowRowGrandTotal;
        private DevExpress.XtraBars.BarCheckItem bciShowRowHeader;
        private DevExpress.XtraBars.BarCheckItem bciShowColTotal;
        private DevExpress.XtraBars.BarCheckItem bciShowColGrandTotal;
        private DevExpress.XtraBars.BarCheckItem bciShowColHeader;
        private DevExpress.XtraBars.BarCheckItem bciShowDataHeader;
        private DevExpress.XtraBars.BarCheckItem bciShowFilterHeader;
        private DevExpress.XtraBars.BarCheckItem bciShowHoriLines;
        private DevExpress.XtraBars.BarCheckItem bciShowVertLines;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem srgbiSkin;
        private DevExpress.XtraBars.BarStaticItem bsiUserName;
        private DevExpress.XtraBars.BarButtonItem bbiClear;
        private DevExpress.XtraBars.BarButtonItem bbiDelete;
        private DevExpress.XtraBars.BarButtonItem bbiData;
        private DevExpress.XtraBars.BarButtonItem bbiBoth;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraPivotGrid.PivotGridControl pgMain;
        private DevExpress.XtraCharts.ChartControl chartMain;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboChartType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ImageComboBoxEdit icbField;
        private DevExpress.XtraEditors.LabelControl lblfldValue;
        private DevExpress.XtraEditors.SpinEdit seTop;
        private DevExpress.XtraEditors.LabelControl lblShowTop;
    }
}