using System;

namespace BackpackCore.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
		    Console.WriteLine("Starting scope 1");
			var backpack = Backpack.CreateScope();
			try
			{
			    Console.WriteLine("Adding name:Hello to the scope");
				var cleaner = Backpack.Add("name", "Hello");

			    PrintBackpackValue("name");

			    M1();
                
                cleaner.Remove();

				backpack.Clear();
			}
			catch (Exception e)
			{
				// log error
				Console.WriteLine(e.ToString());

				backpack.Clear(e);
			}

		    Console.ReadLine();
		}

	    private static void M1()
	    {
	        PrintBackpackValue("name");

	        CreateChildScope();
	    }

	    private static void CreateChildScope()
	    {
	        Console.WriteLine("Starting scope 2");

            var backpack = Backpack.CreateScope();
	        try
	        {
	            Console.WriteLine("Adding surname:yellow to the child scope");
	            var c = Backpack.Add("surname", "yellow");

	            PrintBackpackValue("name");
	            PrintBackpackValue("surname");

	            Console.WriteLine("Overriding name:brown in child scope");
	            var c2 = Backpack.Add("name", "brown");

	            PrintBackpackValue("name");

	            Console.WriteLine("Removing overriden name:hello in child scope");
	            c2.Remove();

	            PrintBackpackValue("name");
	        }
            catch (Exception e)
	        {
	            // log error with data in backpack
	            Console.WriteLine(e.ToString());
	        }
	        finally
	        {
	            backpack.Clear();
	        }

	        PrintBackpackValue("surname");
	    }

	    private static void PrintBackpackValue(string itemName)
	    {
	        var item = Backpack.Get(itemName);
	        Console.WriteLine($"Read {itemName}:{item?.StringValue} from context");
	    }
	}
}
