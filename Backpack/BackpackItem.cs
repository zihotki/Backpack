using System.Collections.Generic;

namespace Backpack
{
    public class BackpackItem
	{
	    private readonly Dictionary<string, BackpackItem> _container;

        public string Name { get; }
		public string Value { get; }

		internal BackpackItem(string name, string value, Dictionary<string, BackpackItem>  container)
		{
			Name = name;
			Value = value;
			_container = container;
		}

	    public void Remove()
	    {
	        _container.Remove(Name);
	    }
	}
}
