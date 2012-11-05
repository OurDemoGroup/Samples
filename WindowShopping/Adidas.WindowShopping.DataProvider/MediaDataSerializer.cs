using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Adidas.WindowShopping.DataProvider.DataTypes;
using Adidas.WindowShopping.DataProvider.Helpers;

namespace Adidas.WindowShopping.DataProvider
{
	public class MediaDataSerializer
	{
		public MediaInfo Deserialize(string resourceName, Assembly assembly)
		{
			MediaInfo mediaInfo;
			var mediaData = ResourceHelper.ReadToString(assembly, resourceName);
			var serializer = new XmlSerializer(typeof(MediaInfo));
			using (var sr = new StringReader(mediaData))
			{
				mediaInfo = (MediaInfo)serializer.Deserialize(sr);
			}
			return mediaInfo;
		}
	}
}
