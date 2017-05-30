using System.Collections.Generic;
using System.Threading;

namespace Backpack
{
    public static class Backpack
    {
        private static readonly AsyncLocal<BackpackScope> CurrentScope = new AsyncLocal<BackpackScope>();

		public static BackpackScope CreateScope()
		{
		    var newScope = new BackpackScope(CurrentScope.Value);
		    CurrentScope.Value = newScope;

			return newScope;
		}

		public static BackpackItem Add(string name, string value)
		{
		    var data = CurrentScope.Value.Data;
            var item = new BackpackItem(name, value, data);
		    data[name] = item;
		    return item;
		}

		internal static void Clean(BackpackScope currentItem)
		{
			CurrentScope.Value = currentItem.Parent;

			do
			{
				currentItem.Data.Clear();

				currentItem = currentItem.Child;
			}
			while (currentItem != null);
		}

        public static BackpackItem Get(string name)
        {
            var scope = CurrentScope.Value;
            var currentData = scope.Data;

            do
            {
                if (currentData.ContainsKey(name))
                {
                    return currentData[name];
                }

                currentData = scope.Parent?.Data;
            } while (currentData != null);

            return null;
        }

        public static IEnumerable<BackpackItem> GetAll()
        {
            var scope = CurrentScope.Value;
            var currentData = scope.Data;

            do
            {
                foreach (var item in currentData.Values)
                {
                    yield return item;
                }

                currentData = scope.Parent?.Data;
            } while (currentData != null);
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
