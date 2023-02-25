using System;
using System.IO;
using System.Threading.Tasks;

namespace Reportyy
{
	public interface IReportyyApiClient
	{
		Task<Stream> GeneratePDF(string templateId, object data);
	}
}

