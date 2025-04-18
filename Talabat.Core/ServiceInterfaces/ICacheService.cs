using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.ServiceInterfaces
{
	public interface ICacheService
	{
		Task CacheResponseAsync(string key, object response, TimeSpan timeOut);

		Task<string?> GetCachedResponseAsync(string key);
	}
}
