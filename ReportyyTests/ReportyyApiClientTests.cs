using FluentAssertions;
using Reportyy;

namespace ReportyyTests
{
    public class ReportyyApiClientTests
    {
        private static readonly string baseUrl = Environment.GetEnvironmentVariable("REPORTYY_BASE_URL");
        private static readonly string apiKey = Environment.GetEnvironmentVariable("REPORTYY_API_KEY");

        [Fact]
        public async Task ReportyyApiClient_Should_Throw_401_With_Invalid_API_Key()
        {
            var client = new ReportyyApiClient("invalid_api_key", baseUrl);

            var act = () => client.GeneratePDF("cleakim7c00129882ha9ct56d", new { });
            await act.Should().ThrowAsync<ReportyyApiException>().WithMessage("Reportyy HTTP API Error: Unauthorized");
        }

        [Fact]
        public async Task ReportyyApiClient_Should_Return_PDF()
        {
            var client = new ReportyyApiClient(apiKey, baseUrl);

            var response = await client.GeneratePDF(
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


            response.Should().NotBeNull();

            using (var fileStream = File.Create("generated.pdf"))
            {
                response.Seek(0, SeekOrigin.Begin);
                response.CopyTo(fileStream);
            }
        }
    }
}
