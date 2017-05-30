using System.Collections.Generic;

namespace Backpack
{
    public class BackpackScope
	{
		private BackpackScope _parent;

		public Dictionary<string, BackpackItem> Data { get; }

		public BackpackScope Parent
		{
			get => _parent;
		    private set
			{
				if (value == null)
				{
					return;
				}

				_parent = value;
				_parent.Child = this;
			}
		}

		public BackpackScope Child { get; private set; }

		public BackpackScope(BackpackScope parent)
		{
			Data = new Dictionary<string, BackpackItem>();
		    Parent = parent;
		}

	    public void Clean()
	    {
	        Backpack.Clean(this);
	    }
    }
}
