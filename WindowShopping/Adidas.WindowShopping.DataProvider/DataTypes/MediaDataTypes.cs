using System.Collections.Generic;
using System.Xml.Serialization;
using Adidas.WindowShopping.DataProvider.Interfaces;

namespace Adidas.WindowShopping.DataProvider.DataTypes
{
	[XmlRoot("MediaInfo")]
	public class MediaInfo
	{
		[XmlAttribute("LayoutWidth")]
		public int LayoutWidth { get; set; }

		[XmlAttribute("LayoutHeight")]
		public int LayoutHeight { get; set; }

		[XmlArray("MovieGroups")]
		[XmlArrayItem("MovieGroup", typeof(MovieGroup))]
		public List<MovieGroup> MovieGroups { get; set; }
	}

	[XmlRoot("MovieGroup")]
	public class MovieGroup
	{
		[XmlAttribute("title")]
		public string Title { get; set; }

		[XmlArray("Movies")]
		[XmlArrayItem("Movie", typeof(MovieAction))]
		public List<MovieAction> Movies { get; set; }
	}

	[XmlRoot("Movie")]
	public class MovieAction : IMediaNode
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("moveId")]
		public string MovieId { get; set; }

		[XmlAttribute("path")]
		public string Source { get; set; }

		[XmlAttribute("isRoot")]
		public bool IsRootMovie { get; set; }

		[XmlAttribute("X")]
		public int X { get; set; }

		[XmlAttribute("Y")]
		public int Y { get; set; }

		[XmlArray("InfoNodes")]
		[XmlArrayItem("Info", typeof(Info))]
		public List<Info> InfoNodes { get; set; }

		[XmlArray("ActionNodes")]
		[XmlArrayItem("MovieAction", typeof(MovieAction))]
		public List<MovieAction> ActionNodes { get; set; }

		[XmlIgnore]
		public MediaNodeType MediaType
		{
			get { return MediaNodeType.MovieAction; }
		}
	}

	[XmlRoot("Info")]
	public class Info : IMediaNode
	{
		[XmlAttribute("img")]
		public string Source { get; set; }

		[XmlAttribute("qr")]
		public string QrImagePath { get; set; }

		[XmlAttribute("desc")]
		public string Description { get; set; }

		[XmlAttribute("X")]
		public int X { get; set; }

		[XmlAttribute("Y")]
		public int Y { get; set; }

		[XmlIgnore]
		public string MovieId { get; set; }

		[XmlIgnore]
		public MediaNodeType MediaType
		{
			get { return MediaNodeType.Info; }
		}
	}
}
