using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHostJsonConf
{
	public class ScopeSample
	{
		private readonly IServiceProvider provider;

		public ScopeSample(IServiceProvider serviceProvider)
		{
			this.provider = serviceProvider;
		}
	}
}
