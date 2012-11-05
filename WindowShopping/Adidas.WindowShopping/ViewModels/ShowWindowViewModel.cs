using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Adidas.WindowShopping.DataProvider;
using Adidas.WindowShopping.DataProvider.DataTypes;
using Adidas.WindowShopping.DataProvider.Interfaces;
using Adidas.WindowShopping.Framework;
using Caliburn.Micro;

namespace Adidas.WindowShopping.ViewModels
{
	[Export(typeof(IShell))]
	public class ShopWindowViewModel : PropertyChangedBase, IShell
	{

		#region Fields

		private readonly MediaDataProvider _mediaDataProvider;
		private readonly MediaInfo _mediaMetaData;

		#endregion

		public ShopWindowViewModel()
		{
			_mediaDataProvider = new MediaDataProvider();
			_mediaMetaData = _mediaDataProvider.GetMediaInfo();
			//Show root movie
			SetMediaLayout(_mediaMetaData.MovieGroups[0], 
						   _mediaMetaData.MovieGroups[0].Movies.FirstOrDefault(mov => mov.IsRootMovie));
		}

		#region Properites

		public int LayoutHeight
		{
			get { return _mediaMetaData.LayoutHeight; }
		}

		public int LayoutWidth
		{
			get { return _mediaMetaData.LayoutWidth; }
		}

		public string SelectedMovieId { get; set; }

		public ObservableCollection<IMediaNode> MediaNodes { get; set; }

		public MovieGroup MoviesGroup { get; set; }

		public bool CanShowNextMovieGroup
		{
			get { return _mediaMetaData.MovieGroups.FindIndex(mov => mov == MoviesGroup) < (_mediaMetaData.MovieGroups.Count - 1); }
		}

		public bool CanShowPreviousMovieGroup
		{
			get { return _mediaMetaData.MovieGroups.FindIndex(mov => mov == MoviesGroup) > 0; }
		}

		#endregion

		#region Public properties

		public void ShowMediaAction(string movieActionId)
		{
			if (string.IsNullOrEmpty(movieActionId))
				return;
			
			var movieAction = MoviesGroup.Movies.FirstOrDefault(mov => mov.Id == movieActionId);
			SetMediaLayout(MoviesGroup, movieAction);
		}

		public void ShowNextMovieGroup()
		{
			var movieGroupIndex = _mediaMetaData.MovieGroups.FindIndex(mov => mov == MoviesGroup);
			var movieGroup = _mediaMetaData.MovieGroups[movieGroupIndex + 1];
			SetMediaLayout(movieGroup, movieGroup.Movies.FirstOrDefault(mov => mov.IsRootMovie));
		}

		public void ShowPreviousMovieGroup()
		{
			var movieGroupIndex = _mediaMetaData.MovieGroups.FindIndex(mov => mov == MoviesGroup);
			var movieGroup = _mediaMetaData.MovieGroups[movieGroupIndex - 1];
			SetMediaLayout(movieGroup, movieGroup.Movies.FirstOrDefault(mov => mov.IsRootMovie));
		}

		#endregion

		#region Private methods

		private void SetMediaLayout(MovieGroup movieGroup, MovieAction movieAction)
		{
			if (movieGroup == null || movieAction == null)
			{
				return;
			}
			
			var mediaNodes = new List<IMediaNode>();
			mediaNodes.AddRange(movieAction.ActionNodes);
			mediaNodes.AddRange(movieAction.InfoNodes);
			MediaNodes = new ObservableCollection<IMediaNode>(mediaNodes);
			
			SelectedMovieId = movieAction.Id;
			MoviesGroup = movieGroup;

			NotifyAllPropertiesChange();
		}

		private void NotifyAllPropertiesChange()
		{
			NotifyOfPropertyChange(() => MediaNodes);
			NotifyOfPropertyChange(() => MoviesGroup);
			NotifyOfPropertyChange(() => SelectedMovieId);
			NotifyOfPropertyChange(() => CanShowNextMovieGroup);
			NotifyOfPropertyChange(() => CanShowPreviousMovieGroup);
		}

		#endregion

	}
}