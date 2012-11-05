using Adidas.WindowShopping.DataProvider.DataTypes;

namespace Adidas.WindowShopping.DataProvider
{
	public class MediaDataProvider
	{
		private readonly MediaDataSerializer _metaDataSerializer;
		private MediaInfo _mediaInfo;

		public MediaDataProvider()
		{
			_metaDataSerializer = new MediaDataSerializer();
		}

		public MediaInfo GetMediaInfo()
		{
			return _mediaInfo ?? (_mediaInfo = _metaDataSerializer.Deserialize(MediaData.ResourceName, MediaData.Assembly));
		}
	}
}
