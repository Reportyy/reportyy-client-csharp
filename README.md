# reportyy-client-dotnet
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Reportyy.net)

C#/.NET client for Reportyy API

## Installation
Using the [.NET Core command-line interface (CLI) tools](https://docs.microsoft.com/en-us/dotnet/core/tools/):

```sh
$ dotnet add package Reportyy.net
```

Using the [NuGet Command Line Interface (CLI)](https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference):

```sh
$ nuget install Stripe.net
```

Using the [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```powershell
Install-Package Stripe.net
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on a project within your solution.
3. Click on *Manage NuGet Packages...*
4. Click on the *Browse* tab and search for "Stripe.net".
5. Click on the Stripe.net package, select the appropriate version in the
   right-tab and click *Install*.

## Documentation
Please see the [Reportyy API documentation](https://docs.reportyy.com/quickstart) for more information.

## Usage

```csharp
using Reportyy;
using System.IO;

// Obtain API key from Reportyy Dashboard (Settings -> API Keys)
var apiKey = "rpty_ktS0rU...";

IReportyyApiClient client = new ReportyyApiClient(apiKey);

Stream response = await client.GeneratePDF(
    "cleakim7c00129882ha9ct56d",
    new
    {
        date = "February 18th 2023",
        reportType = "Daily report",
        reportHeader = "Day",
        merchant = new
        {
            id = "c45d1500-2184",
            name = "Reportyy Limited",
            address = new
            {
                line1 = "Line 1",
                town = "London",
                postCode = "E2 000",
            }
        },
        saleItems = new List<object>()
        {
            new { date = "February 7, 2023", value = "£14.00" }
        },
        lineItems = new List<object>()
        {
            new { title =  "Gross sales", value =  "£14.00" },
            new { title =  "Net sales", value =  "£14.00" },
            new { title =  "Cost of goods", value =  "£-2.00" },
            new { title =  "Fees", value =  "£0.00" },
            new { title =  "Gross profit", value =  "£12.00" }
        }
    }
);

using (var fileStream = File.Create("sales_report.pdf"))
{
    response.CopyTo(fileStream);
}
```