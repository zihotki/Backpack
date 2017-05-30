using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ASYNCLOCAL
using System.Collections.Generic;
using System.Threading;
#elif REMOTING
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#endif

namespace Backpack
{

	public class BackpackCleaner
	{
        public LinkedItem CurrentItem { get; }

        public BackpackCleaner(LinkedItem currentItem)
		{
			CurrentItem = currentItem;
		}

		public void Clean()
		{
			Backpack.Clean(CurrentItem);
		}
	}
}
