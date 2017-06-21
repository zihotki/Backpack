using System;
using System.Collections.Generic;
using System.Threading;

namespace BackpackCore
{
	public static class Backpack
    {
        private static readonly AsyncLocal<BackpackScope> LocalCurrentScope = new AsyncLocal<BackpackScope>();

		private static readonly List<IBackpackCleaner> Cleaners = new List<IBackpackCleaner>();

	    public static BackpackScope CreateScope(string name = null, BackpackScope parentScope = null)
	    {
		    var newScope = new BackpackScope(LocalCurrentScope.Value, name);
		    LocalCurrentScope.Value = newScope;

		    return newScope;
	    }

	    public static BackpackScope CurrentScope => LocalCurrentScope.Value;

	    #region Cleaner

	    public static void AddCleaner(IBackpackCleaner cleaner)
	    {
		    Cleaners.Add(cleaner);
	    }

	    public static void RemoveCleaner(IBackpackCleaner cleaner)
	    {
		    Cleaners.Remove(cleaner);
	    }

	    internal static IEnumerable<IBackpackCleaner> GetCleaners()
	    {
		    return Cleaners;
	    }

	    #endregion

	    #region Add

		public static BackpackItem Add(string name, string value)
		{
			ThrowIfNotInitialized();

			return LocalCurrentScope.Value.Add(name, value);
		}

		public static BackpackItem Add(string name, bool value)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Add(name, value);
	    }

	    public static BackpackItem Add(string name, long value)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Add(name, value);
	    }

	    public static BackpackItem Add(string name, int value)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Add(name, value);
	    }

	    public static BackpackItem Add(string name, byte value)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Add(name, value);
	    }

	    public static BackpackItem Add(string name, Guid value)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Add(name, value);
	    }

	    #endregion

	    #region Get

	    public static BackpackItem Get(string name)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name);
	    }

	    public static long Get(string name, long defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static byte Get(string name, byte defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static long Get(string name, int defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static short Get(string name, short defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static Guid Get(string name, Guid defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static bool Get(string name, bool defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static string Get(string name, string defaultValue)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.Get(name, defaultValue);
	    }

	    public static IEnumerable<BackpackItem> GetAll(bool includeHidden = false)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.GetAll(includeHidden);
	    }

	    public static IEnumerable<BackpackItem> GetUnique(bool includeHidden = false)
	    {
		    ThrowIfNotInitialized();

		    return LocalCurrentScope.Value.GetUnique(includeHidden);
	    }

		#endregion

		private static void ThrowIfNotInitialized()
		{
			if (LocalCurrentScope.Value == null)
			{
				throw new BackpackNotInitializedException();
			}
		}

		internal static void ResetToParent(BackpackScope currentItem)
		{
			LocalCurrentScope.Value = currentItem.Parent;
		}
    }
}
