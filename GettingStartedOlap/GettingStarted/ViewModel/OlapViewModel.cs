using System.ServiceModel;
using GettingStarted.OlapManagerService;
using Syncfusion.Olap.UWP.Manager;
using Syncfusion.Olap.UWP.Reports;

namespace GettingStarted
{
    class OlapViewModel
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets the client channel.
        /// </summary>
        IOlapDataProvider clientChannel;

        /// <summary>
        /// Gets or sets the OlapDataManager.
        /// </summary>
        OlapDataManager olapDataManager;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the OlapDataManager.
        /// </summary>
        public OlapDataManager OlapDataManager
        {
            get
            {
                this.olapDataManager = this.olapDataManager ?? this.SetDataManager();
                return this.olapDataManager;
            }
            set { this.olapDataManager = value; }
        }

        #endregion

        #region Setting Connection to Service

        private void SetConnection()
        {
            BasicHttpsBinding basicHttpBinding = new BasicHttpsBinding();
            basicHttpBinding.MaxReceivedMessageSize = 2147483647;
            basicHttpBinding.MaxBufferSize = 2147483647;
            EndpointAddress address = new EndpointAddress("https://bi.syncfusion.com/OlapUWPTestService/OlapManager.svc/");
            ChannelFactory<IOlapDataProvider> clientFactory = new ChannelFactory<IOlapDataProvider>(basicHttpBinding, address);
            this.clientChannel = clientFactory.CreateChannel();
        }

        #endregion

        #region Setting OLAP Data Manager

        private OlapDataManager SetDataManager()
        {
            this.olapDataManager = new OlapDataManager();
            this.olapDataManager.OlapDataChanged += this.OlapDataManager_OlapDataChanged;
            this.olapDataManager.GetCubeSchema += this.OlapDataManager_GetCubeSchema;
            this.olapDataManager.GetCubeInfoCollection += this.OlapDataManager_GetCubeInfoCollection;
            this.olapDataManager.SetCurrentReport(this.CreateOlapReport());
            return this.olapDataManager;
        }

        #endregion

        #region OLAP Report Creation

        private OlapReport CreateOlapReport()
        {
            OlapReport olapReport = new OlapReport();
            olapReport.Name = "OLAP Report";
            olapReport.CurrentCubeName = "Adventure Works";

            DimensionElement dimensionElementColumn = new DimensionElement();
            dimensionElementColumn.Name = "Customer";
            dimensionElementColumn.HierarchyName = "Customer Geography";
            dimensionElementColumn.AddLevel("Customer Geography", "Country");

            MeasureElements measureElementColumn = new MeasureElements();
            measureElementColumn.Elements.Add(new MeasureElement { Name = "Internet Sales Amount" });

            DimensionElement dimensionElementRow = new DimensionElement();
            dimensionElementRow.Name = "Date";
            dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");

            olapReport.CategoricalElements.Add(dimensionElementColumn);
            olapReport.CategoricalElements.Add(measureElementColumn);
            olapReport.SeriesElements.Add(dimensionElementRow);

            return olapReport;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// The event handler method is hooked when the data manager was changed.
        /// </summary>
        private void OlapDataManager_OlapDataChanged(object sender, OlapDataChangedEventArgs args)
        {
            if (args.MDXQuery != null && sender is OlapDataManager)
            {
                this.SetConnection();
                (sender as OlapDataManager).JSONData = this.clientChannel.GetJSONDataAsync(args.MDXQuery, args.SerializedReport, args.AllowMdxToOlapReportParse).Result;
            }
        }

        /// <summary>
        /// The event handler method is hooked to get the current cube schema.
        /// </summary>
        private string OlapDataManager_GetCubeSchema(object sender, GetCubeSchemaEventArgs args)
        {
            if (args.CubeName != null && sender is OlapDataManager)
            {
                this.SetConnection();
                return this.clientChannel.GetJSONCubeSchemaAsync(args.CubeName).Result;
            }
            return null;
        }

        /// <summary>
        /// The event handler method is hooked to get the collection of cube information.
        /// </summary>
        private string OlapDataManager_GetCubeInfoCollection(object sender, GetCubeInfoCollectionEventArgs args)
        {
            if (sender is OlapDataManager)
            {
                this.SetConnection();
                return this.clientChannel.GetJSONCubesAsync().Result;
            }
            return null;
        }

        #endregion
    }
}
