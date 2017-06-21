using System;
using System.Collections.Generic;

namespace BackpackCore
{
	public class BackpackItem
	{
	    private readonly Dictionary<string, BackpackItem> _container;
		
        public string Name { get; }

		public ItemType ItemType { get; }
	    public bool IsHidden { get; }
        
        public string StringValue { get; }
	    public long? LongValue { get; }
		public int? IntValue { get; }
		public short? ShortValue { get; }
		public byte? ByteValue { get; }
		public Guid? GuidValue { get; }
		public bool? BoolValue { get; }

		internal BackpackItem(string name, string value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			StringValue = value;
		    ItemType = ItemType.String;
		    IsHidden = isHidden;
            _container = container;
		}

		internal BackpackItem(string name, long value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			LongValue = value;
			ItemType = ItemType.Long;
		    IsHidden = isHidden;
			_container = container;
		}

		internal BackpackItem(string name, int value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			IntValue = value;
			ItemType = ItemType.Int;
		    IsHidden = isHidden;
			_container = container;
		}

		internal BackpackItem(string name, short value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			ShortValue = value;
			ItemType = ItemType.Short;
		    IsHidden = isHidden;
			_container = container;
		}

		internal BackpackItem(string name, byte value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			ByteValue = value;
			ItemType = ItemType.Byte;
		    IsHidden = isHidden;
			_container = container;
		}

		internal BackpackItem(string name, Guid value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			GuidValue = value;
			ItemType = ItemType.Guid;
		    IsHidden = isHidden;
			_container = container;
		}

		internal BackpackItem(string name, bool value, bool isHidden, Dictionary<string, BackpackItem> container)
		{
			Name = name;
			BoolValue = value;
			ItemType = ItemType.Bool;
		    IsHidden = isHidden;
			_container = container;
		}

		public void Remove()
	    {
	        _container.Remove(Name);
	    }
	}
}
