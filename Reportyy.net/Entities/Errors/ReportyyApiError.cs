using System.Text.Json.Serialization;

namespace Reportyy
{
	public class ReportyyApiError
	{
		[JsonPropertyName("status")]
        public int Status;

		[JsonPropertyName("code")]
		public int Code;

		[JsonPropertyName("message")]
		public string Message;
	}
}

