using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BackpackCore
{
	[DebuggerDisplay("Name = {" + nameof(Name) + "}")]
	public class BackpackScope
	{
		private BackpackScope _parent;

		private volatile bool _disposed;
		private BackpackScope _child;

		private Dictionary<string, BackpackItem> Data { get; }

		internal BackpackScope Parent
		{
			get
			{
				ThrowIfDisposed();

				return _parent;
			}
		    private set
		    {
			    ThrowIfDisposed();

				if (value == null)
				{
					if (_parent != null)
					{
						_parent.Child = null;
						_parent = null;
					}

					return;
				}

				_parent = value;
				_parent.Child = this;
			}
		}

		public string Name { get; }

		internal BackpackScope Child
		{
			get
			{
			    ThrowIfDisposed();

				return _child;
			}
			private set
			{
				ThrowIfDisposed();

				_child = value;
			}
		}

		internal BackpackScope(BackpackScope parent, string name = null)
		{
			Data = new Dictionary<string, BackpackItem>();
		    Parent = parent;
			Name = name;
		}

		public void Clear(Exception e = null)
		{
			ThrowIfDisposed();

			Backpack.ResetToParent(this);

			// find the innermost scope
			var currentScope = this;
			while (currentScope.Child != null)
			{
				currentScope = currentScope.Child;
			}

			var parentScope = _parent;
			var cleaners = Backpack.GetCleaners() ?? Enumerable.Empty<IBackpackCleaner>();
			// unwind from innermost to parent
			do
			{
				// ReSharper disable once PossibleMultipleEnumeration
				// Justification: cleaners should run for each scope we are clearing
				foreach (var backpackCleaner in cleaners)
				{
					backpackCleaner.Cleanup(currentScope, e);
				}

				var scopeToClear = currentScope;
				currentScope = currentScope.Parent;

				scopeToClear.ClearScope();
			} while (currentScope != parentScope);
		}

		private void ClearScope()
		{
			Data.Clear();

			Parent = null;
			Child = null;

			_disposed = true;
		}

		#region Add

		public BackpackItem Add(string name, string value, bool isHidden = false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		public BackpackItem Add(string name, bool value, bool isHidden= false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		public BackpackItem Add(string name, long value, bool isHidden = false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		public BackpackItem Add(string name, int value, bool isHidden = false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		public BackpackItem Add(string name, byte value, bool isHidden = false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		public BackpackItem Add(string name, Guid value, bool isHidden = false)
		{
			ThrowIfDisposed();

			var item = new BackpackItem(name, value, isHidden, Data);
			Data[name] = item;
			return item;
		}

		#endregion

		#region GetLocal

		public BackpackItem GetLocal(string name)
		{
			ThrowIfDisposed();

			if (Data.ContainsKey(name))
			{
				return Data[name];
			}

			return null;
		}

		public long GetLocal(string name, long defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Long)
			{
				return defaultValue;
			}

			return item.LongValue.Value;
		}

		public byte GetLocal(string name, byte defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Byte)
			{
				return defaultValue;
			}

			return item.ByteValue.Value;
		}

		public long GetLocal(string name, int defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Int)
			{
				return defaultValue;
			}

			return item.IntValue.Value;
		}

		public short GetLocal(string name, short defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Short)
			{
				return defaultValue;
			}

			return item.ShortValue.Value;
		}

		public Guid GetLocal(string name, Guid defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Guid)
			{
				return defaultValue;
			}

			return item.GuidValue.Value;
		}

		public bool GetLocal(string name, bool defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.Bool)
			{
				return defaultValue;
			}

			return item.BoolValue.Value;
		}

		public string GetLocal(string name, string defaultValue)
		{
			var item = GetLocal(name);
			if (item == null || item.ItemType != ItemType.String)
			{
				return defaultValue;
			}

			return item.StringValue;
		}

		#endregion

		#region Get

		public BackpackItem Get(string name)
		{
			ThrowIfDisposed();

			var currentScope = this;

			do
			{
				var currentData = currentScope.Data;

				if (currentData.ContainsKey(name))
				{
					return currentData[name];
				}

				currentScope = currentScope.Parent;
			} while (currentScope != null);

			return null;
		}

		public long Get(string name, long defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Long)
			{
				return defaultValue;
			}

			return item.LongValue.Value;
		}

		public byte Get(string name, byte defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Byte)
			{
				return defaultValue;
			}

			return item.ByteValue.Value;
		}

		public long Get(string name, int defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Int)
			{
				return defaultValue;
			}

			return item.IntValue.Value;
		}

		public short Get(string name, short defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Short)
			{
				return defaultValue;
			}

			return item.ShortValue.Value;
		}

		public Guid Get(string name, Guid defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Guid)
			{
				return defaultValue;
			}

			return item.GuidValue.Value;
		}

		public bool Get(string name, bool defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.Bool)
			{
				return defaultValue;
			}

			return item.BoolValue.Value;
		}

		public string Get(string name, string defaultValue)
		{
			var item = Get(name);
			if (item == null || item.ItemType != ItemType.String)
			{
				return defaultValue;
			}

			return item.StringValue;
		}

		#endregion

		public IEnumerable<BackpackItem> GetAll(bool includeHidden = false)
		{
			ThrowIfDisposed();

			var currentScope = this;

			do
			{
				var currentData = currentScope.Data;

				foreach (var item in currentData.Values)
				{
					if (item.IsHidden && !includeHidden)
					{
						continue;
					}

					yield return item;
				}

				currentScope = currentScope.Parent;
			} while (currentScope != null);
		}


		public IEnumerable<BackpackItem> GetUnique(bool includeHidden = false)
		{
			ThrowIfDisposed();

			var currentScope = this;
			var uniqueNames = new HashSet<string>();

			do
			{
				var currentData = currentScope.Data;

				foreach (var item in currentData.Values)
				{
					if ((item.IsHidden && !includeHidden) || uniqueNames.Contains(item.Name))
					{
						continue;
					}

					uniqueNames.Add(item.Name);
					yield return item;
				}

				currentScope = currentScope.Parent;
			} while (currentScope != null);
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new BackpackScopeDisposedException($"Scope {Name} has already been disposed");
			}
		}
	}
}
