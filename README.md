# Getting Started with UWP Pivot Chart (SfPivotChart)

This repository contains getting started samples for the Syncfusion® [UWP Pivot Chart](https://www.syncfusion.com/uwp-ui-controls/pivot-chart) control (`SfPivotChart`). It includes two projects:

- **GettingStartedRelational** — Binds the `SfPivotChart` to a relational data source (IList/IEnumerable).
- **GettingStartedOlap** — Binds the `SfPivotChart` to an OLAP data source via a WCF service and `OlapDataManager`.

> **Important:** Starting with v16.2.0.x, if you refer to Syncfusion® assemblies from a trial setup or from the NuGet feed, you must include a license key in your projects. Refer to [this link](https://help.syncfusion.com/common/essential-studio/licensing/license-key) to learn about registering the Syncfusion® license key in your UWP application.

---

## Table of Contents

- [Requirements](#requirements)
- [NuGet Packages](#nuget-packages)
- [Part 1: Getting Started – Relational](#part-1-getting-started--relational)
  - [Create a New UWP Project](#1-create-a-new-uwp-project)
  - [Add NuGet Package Reference](#2-add-nuget-package-reference-relational)
  - [Add the SfPivotChart Namespace](#3-add-the-sfpivotchart-namespace)
  - [Create the ViewModel with Relational Data](#4-create-the-viewmodel-with-relational-data)
  - [Bind Data to SfPivotChart via XAML](#5-bind-data-to-sfpivotchart-via-xaml)
  - [Output – Relational](#output--relational)
- [Part 2: Getting Started – OLAP](#part-2-getting-started--olap)
  - [Create a New UWP Project](#1-create-a-new-uwp-project-1)
  - [Add NuGet Package Reference](#2-add-nuget-package-reference-olap)
  - [Add Service Reference](#3-add-service-reference)
  - [Add the SfPivotChart Namespace](#4-add-the-sfpivotchart-namespace)
  - [Create the OlapViewModel](#5-create-the-olapviewmodel)
  - [Bind OlapDataManager to SfPivotChart via XAML](#6-bind-olapdatamanager-to-sfpivotchart-via-xaml)
  - [Output – OLAP](#output--olap)

---

## Requirements

- Visual Studio 2022 or later
- Windows 10 SDK (Target: 10.0.26100.0 / Minimum: 10.0.17763.0)
- Syncfusion.SfPivotChart.UWP NuGet package (latest)

---

## NuGet Packages

The following NuGet packages are used in this sample:

| Package | Version |
|---|---|
| `Syncfusion.SfPivotChart.UWP` | * |
| `Syncfusion.SfChart.UWP` | * |
| `Microsoft.NETCore.UniversalWindowsPlatform` | * |

---

## Part 1: Getting Started – Relational

This section explains how to create a simple `SfPivotChart` control bound to a relational data source.

### 1. Create a New UWP Project

Open Visual Studio and select **File > New > Project**. Choose **Installed > Templates > Visual C# > Windows > Universal > Blank App (Universal Windows)** and name the project **GettingStarted**.

In the target version dialog, set:
- **Target version:** Windows 10 (10.0; Build 10240) or later
- **Minimum version:** Windows 10 (10.0; Build 17763)

### 2. Add NuGet Package Reference (Relational)

Right-click the project in Solution Explorer and select **Manage NuGet Packages**. Search for and install:

- `Syncfusion.SfPivotChart.UWP`

This package automatically brings in all the required dependencies (`Syncfusion.Data.UWP`, `Syncfusion.Olap.UWP`, `Syncfusion.PivotAnalysis.UWP`, `Syncfusion.SfBusyIndicator.UWP`, `Syncfusion.SfChart.UWP`).

### 3. Add the SfPivotChart Namespace

Open `MainPage.xaml` and add the following XML namespaces:

```xml
xmlns:PivotChart="using:Syncfusion.UI.Xaml.PivotChart"
xmlns:pivot="using:Syncfusion.PivotAnalysis.UWP"
```

### 4. Create the ViewModel with Relational Data

Add a new class file `ViewModel.cs` under the `ViewModel` folder and define the relational data model and view model as follows:

```csharp
using System;
using System.Collections.ObjectModel;

namespace GettingStarted
{
    public class ProductSalesViewModel
    {
        private ProductSalesCollection productSales;

        public ProductSalesCollection ProductSales
        {
            get { return this.productSales; }
            set { this.productSales = value; }
        }

        public ProductSalesViewModel()
        {
            this.productSales = GetSalesData();
        }

        public static ProductSalesCollection GetSalesData()
        {
            // Geography
            string[] countries = new string[] { "Germany", "Canada", "United States" };

            // Time
            string[] dates = new string[] { "FY 2008", "FY 2009", "FY 2010", "FY 2012" };

            // Products
            string[] products = new string[] { "Bike", "Car" };
            Random r = new Random(123345);

            int numberOfRecords = 1000;
            ProductSalesCollection listOfProductSales = new ProductSalesCollection();
            for (int i = 0; i < numberOfRecords; i++)
            {
                ProductSale sales = new ProductSale();
                sales.Country = countries[r.Next(1, countries.GetLength(0))];
                sales.Amount = (3000 * r.Next(1, 12));
                sales.Date = dates[r.Next(r.Next(dates.GetLength(0) + 1))];
                sales.Product = products[r.Next(r.Next(products.GetLength(0) + 1))];
                listOfProductSales.Add(sales);
            }
            return listOfProductSales;
        }
    }

    public class ProductSale
    {
        public string Product { get; set; }
        public string Date { get; set; }
        public string Country { get; set; }
        public double Amount { get; set; }
    }

    public class ProductSalesCollection : ObservableCollection<ProductSale>
    {
    }
}
```

### 5. Bind Data to SfPivotChart via XAML

Open `MainPage.xaml` and bind the `ProductSalesViewModel` to the `SfPivotChart` control using the `ItemSource` property. Define the `PivotAxis`, `PivotLegend`, and `PivotCalculations`:

```xml
<Page
    x:Class="GettingStarted.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:GettingStarted"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:pivot="using:Syncfusion.PivotAnalysis.UWP"
    xmlns:PivotChart="using:Syncfusion.UI.Xaml.PivotChart"
    mc:Ignorable="d">

    <Grid>
        <Grid.DataContext>
            <local:ProductSalesViewModel/>
        </Grid.DataContext>
        <PivotChart:SfPivotChart ItemSource="{Binding ProductSales}">
            <PivotChart:SfPivotChart.PivotAxis>
                <pivot:PivotItem FieldCaption="Product" FieldMappingName="Product" TotalHeader="Total"/>
                <pivot:PivotItem FieldCaption="Country" FieldMappingName="Country" TotalHeader="Total"/>
            </PivotChart:SfPivotChart.PivotAxis>
            <PivotChart:SfPivotChart.PivotLegend>
                <pivot:PivotItem FieldCaption="Date" FieldMappingName="Date" TotalHeader="Total"/>
            </PivotChart:SfPivotChart.PivotLegend>
            <PivotChart:SfPivotChart.PivotCalculations>
                <pivot:PivotComputationInfo FieldCaption="Amount" FieldName="Amount" Format="#.##" SummaryType="DoubleTotalSum"/>
            </PivotChart:SfPivotChart.PivotCalculations>
        </PivotChart:SfPivotChart>
    </Grid>
</Page>
```

**Property descriptions:**

| Property | Description |
|---|---|
| `ItemSource` | Binds the relational data collection to the `SfPivotChart`. |
| `PivotAxis` | Defines the fields displayed along the X-axis (categorical axis). |
| `PivotLegend` | Defines the fields used to differentiate series (legend axis). |
| `PivotCalculations` | Defines the measure fields and their summary type for chart values. |

### Output – Relational

Run the application to see the `SfPivotChart` populated with relational data showing product sales by country and fiscal year.
<img width="1919" height="1005" alt="Screenshot 2026-03-31 165715" src="https://github.com/user-attachments/assets/ab0a4a55-c41b-4242-a5b3-a4d7a0bef064" />

---

## Part 2: Getting Started – OLAP

This section explains how to create a simple `SfPivotChart` control bound to an OLAP data source through a WCF service and `OlapDataManager`.

### 1. Create a New UWP Project

Open Visual Studio and select **File > New > Project**. Choose **Installed > Templates > Visual C# > Windows > Universal > Blank App (Universal Windows)** and name the project **GettingStarted**.

In the target version dialog, set:
- **Target version:** Windows 10 (10.0; Build 10240) or later
- **Minimum version:** Windows 10 (10.0; Build 17763)

### 2. Add NuGet Package Reference (OLAP)

Right-click the project in Solution Explorer and select **Manage NuGet Packages**. Search for and install:

- `Syncfusion.SfPivotChart.UWP`

This package automatically brings in all the required dependencies (`Syncfusion.Data.UWP`, `Syncfusion.Olap.UWP`, `Syncfusion.PivotAnalysis.UWP`, `Syncfusion.SfBusyIndicator.UWP`, `Syncfusion.SfChart.UWP`).

### 3. Add Service Reference

> **Note:** This step explains how to refer to the online WCF service for binding cube information to the `SfPivotChart`. The service URL used here refers to the demo cube **"Adventure Works"**, used for illustration purposes.

Right-click the project in Solution Explorer and select **Add > Service Reference…**.

In the **Add Service Reference** dialog, enter the following address and click **Go**:

```
https://bi.syncfusion.com/OlapUWPTestService/OlapManager.svc
```

Change the default namespace from `ServiceReference1` to `OlapManagerService`, then click **OK**.

### 4. Add the SfPivotChart Namespace

Open `MainPage.xaml` and add the following XML namespace:

```xml
xmlns:PivotChart="using:Syncfusion.UI.Xaml.PivotChart"
```

### 5. Create the OlapViewModel

Add a new class file `OlapViewModel.cs` under the `ViewModel` folder. This class sets up the `OlapDataManager` and `OlapReport`, and communicates with the WCF service:

```csharp
using System.ServiceModel;
using GettingStarted.OlapManagerService;
using Syncfusion.Olap.UWP.Manager;
using Syncfusion.Olap.UWP.Reports;

namespace GettingStarted
{
    class OlapViewModel
    {
        #region Private Fields

        IOlapDataProvider clientChannel;
        OlapDataManager olapDataManager;

        #endregion

        #region Public Properties

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
            // Hook OlapDataChanged before assigning the current report
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

        private void OlapDataManager_OlapDataChanged(object sender, OlapDataChangedEventArgs args)
        {
            if (args.MDXQuery != null && sender is OlapDataManager)
            {
                this.SetConnection();
                (sender as OlapDataManager).JSONData = this.clientChannel.GetJSONDataAsync(args.MDXQuery, args.SerializedReport, args.AllowMdxToOlapReportParse).Result;
            }
        }

        private string OlapDataManager_GetCubeSchema(object sender, GetCubeSchemaEventArgs args)
        {
            if (args.CubeName != null && sender is OlapDataManager)
            {
                this.SetConnection();
                return this.clientChannel.GetJSONCubeSchemaAsync(args.CubeName).Result;
            }
            return null;
        }

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
```

**Key points:**

- The `OlapDataChanged` event must be hooked **before** calling `SetCurrentReport` on the `OlapDataManager`.
- The `OlapDataManager` communicates with the WCF service to fetch MDX query results, cube schemas, and cube info collections.
- The `OlapReport` defines `CategoricalElements` (column axis) and `SeriesElements` (row axis).

### 6. Bind OlapDataManager to SfPivotChart via XAML

Open `MainPage.xaml` and bind the `OlapViewModel` to the `SfPivotChart` control using the `OlapDataManager` property:

```xml
<Page
    x:Class="GettingStarted.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:GettingStarted"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:PivotChart="using:Syncfusion.UI.Xaml.PivotChart"
    mc:Ignorable="d"
    Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

    <Grid>
        <Grid.DataContext>
            <local:OlapViewModel/>
        </Grid.DataContext>
        <PivotChart:SfPivotChart OlapDataManager="{Binding OlapDataManager}"/>
    </Grid>
</Page>
```

**Property description:**

| Property | Description |
|---|---|
| `OlapDataManager` | Binds the `OlapDataManager` instance that connects the `SfPivotChart` to the OLAP data source via WCF service. |

### Output – OLAP

Run the application to see the `SfPivotChart` populated with OLAP data from the Adventure Works cube, showing Internet Sales Amount by Customer Geography Country and Fiscal Year.
<img width="1919" height="1005" alt="Screenshot 2026-03-31 163536" src="https://github.com/user-attachments/assets/d42a8ed7-e62a-49d2-9d5a-bb5c5bfe62f0" />

---

## References

- [UWP Pivot Chart – Relational Getting Started](https://help.syncfusion.com/uwp/pivot-chart/relational/getting-started)
- [UWP Pivot Chart – OLAP Getting Started](https://help.syncfusion.com/uwp/pivot-chart/olap/getting-started)
- [Syncfusion UWP Controls](https://www.syncfusion.com/uwp-ui-controls)
- [Syncfusion License Key Registration](https://help.syncfusion.com/common/essential-studio/licensing/license-key)
