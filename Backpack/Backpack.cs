using System.Threading;

namespace Backpack
{
    public static class Backpack
    {
        static readonly AsyncLocal<LinkedItem> Data = new AsyncLocal<LinkedItem>();

		public static BackpackCleaner CreateScope()
		{
		    var newScope = new LinkedItem {Parent = Data.Value};

		    Data.Value = newScope;

			return new BackpackCleaner(newScope);
		}

		public static BackpackItem Add(string name, string value)
		{
		    var data = Data.Value.Data;
            var item = new BackpackItem(name, value, data);
		    data[name] = item;
		    return item;
		}

		internal static void Clean(LinkedItem currentItem)
		{
			Data.Value = currentItem.Parent;

			do
			{
				currentItem.Data.Clear();

				currentItem = currentItem.Child;
			}
			while (currentItem != null);
		}

        public static BackpackItem Get(string name)
        {
            var item = Data.Value;
            var currentData = item.Data;

            do
            {
                if (currentData.ContainsKey(name))
                {
                    return currentData[name];
                }

                currentData = item.Parent?.Data;
            } while (currentData != null);

            return null;
        }

       /*

		#if ASYNCLOCAL

				static List<BackpackItem> Enrichers
				{
					get
					{
						return Data.Value;
					}
					set
					{
						Data.Value = value;
					}
				}

		#elif REMOTING

				static ImmutableStack<BackpackItem> Enrichers
				{
					get
					{
						var objectHandle = CallContext.LogicalGetData(DataSlotName) as ObjectHandle;

						return objectHandle?.Unwrap() as List<BackpackItem>;
					}
					set
					{
						CallContext.LogicalSetData(DataSlotName, new ObjectHandle(value));
					}
				}

		#else // DOTNET_51

				static List<BackpackItem> Enrichers
				{
					get
					{
						return Data;
					}
					set
					{
						Data = value;
					}
				}
		#endif
		*/

    }
}
