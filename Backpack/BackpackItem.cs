using System.Collections.Generic;

namespace Backpack
{
    public class BackpackItem
	{
		public string Name { get; }
		public string Value { get; }

		private readonly Dictionary<string, BackpackItem> _container;

		internal BackpackItem(string name, string value, Dictionary<string, BackpackItem>  container)
		{
			Name = name;
			Value = value;
			_container = container;
		}

		public void Remove()
		{
			if (_container.ContainsKey(Name))
			{
			    _container.Remove(Name);
			}
		}
	}
}
