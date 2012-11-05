using Adidas.WindowShopping.DataProvider.DataTypes;

namespace Adidas.WindowShopping.DataProvider.Interfaces
{
	public interface IMediaNode
	{
		int X { get; set; }
		int Y { get; set; }
		MediaNodeType MediaType { get; }
		string MovieId { get; }
		string Source { get; }
	}
}
