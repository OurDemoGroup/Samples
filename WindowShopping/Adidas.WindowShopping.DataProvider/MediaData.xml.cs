using System.Reflection;

namespace Adidas.WindowShopping.DataProvider
{
	public class MediaData
	{
		private const string Extension = ".xml";

		public static string ResourceName
		{
			get { return typeof(MediaData).FullName + Extension; }
		}

		public static Assembly Assembly
		{
			get { return typeof(MediaData).Assembly; }
		}
	}
}
