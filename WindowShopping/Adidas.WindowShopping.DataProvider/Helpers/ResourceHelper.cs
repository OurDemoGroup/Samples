using System;
using System.IO;
using System.Reflection;

namespace Adidas.WindowShopping.DataProvider.Helpers
{
	public class ResourceHelper
	{
		public static string ReadToString(Assembly assembly, string resourceName)
		{
			if (assembly == null || string.IsNullOrEmpty(resourceName))
			{
				return null;
			}

			var stream = assembly.GetManifestResourceStream(resourceName);
			try
			{
				if (stream == null)
				{
					throw new Exception(string.Format("Failed to open resource with name {0} in assembly {1}", (object) resourceName,
					                                  (object) assembly.FullName));
				}
				using (var streamReader = new StreamReader(stream))
					return streamReader.ReadToEnd();
			}
			finally
			{
				if (stream != null)
					stream.Dispose();
			}
		}
	}
}
