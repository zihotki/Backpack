using System.Collections.Generic;

namespace Backpack
{
    public class LinkedItem
	{
		private LinkedItem _parent;

		public Dictionary<string, BackpackItem> Data { get; private set; }

		public LinkedItem Parent
		{
			get => _parent;
		    set
			{
				if (value == null)
				{
					return;
				}

				_parent = value;
				_parent.Child = this;
			}
		}

		public LinkedItem Child { get; private set; }

		public LinkedItem()
		{
			Data = new Dictionary<string, BackpackItem>();
		}

	}
}
