using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Adidas.WindowShopping.DataProvider.DataTypes;
using Adidas.WindowShopping.DataProvider.Interfaces;

namespace Adidas.WindowShopping.Controls
{
	public class MediaGrid: Grid
	{
		private readonly List<Button> _mediaBtns;
		private readonly MediaElement _videoBackround;
		private readonly Dictionary<string, MediaElement> _backgroundMedia = new Dictionary<string, MediaElement>();
		private MediaElement _previousBackgroundMovie;

		#region DependencyProperties

		public static readonly DependencyProperty MediaNodesProperty =
			DependencyProperty.Register("MediaNodes", typeof(ICollection<IMediaNode>), typeof(MediaCanvas),
										new PropertyMetadata(default(ICollection<IMediaNode>)));

		public static DependencyProperty MoviesGroupProperty =
			DependencyProperty.Register("MoviesGroup", typeof(MovieGroup), typeof(MediaCanvas), new PropertyMetadata(null));

		public static DependencyProperty SelectedMovieIdProperty =
			DependencyProperty.Register("SelectedMovieId", typeof(string), typeof(MediaCanvas), new PropertyMetadata(null));

		public static readonly DependencyProperty ColumnsCountProperty =
			DependencyProperty.Register("ColumnsCount", typeof(int), typeof(MediaGrid), new PropertyMetadata(100));

		public static readonly DependencyProperty RowsCountProperty =
			DependencyProperty.Register("RowsCount", typeof(int), typeof(MediaGrid), new PropertyMetadata(100));

		public ICollection<IMediaNode> MediaNodes
		{
			get { return (ICollection<IMediaNode>)GetValue(MediaNodesProperty); }
			set { SetValue(MediaNodesProperty, value); }
		}

		public string SelectedMovieId
		{
			get { return (string)GetValue(SelectedMovieIdProperty); }
			set { SetValue(SelectedMovieIdProperty, value); }
		}

		public int ColumnsCount
		{
			get { return (int)GetValue(ColumnsCountProperty); }
			set { SetValue(ColumnsCountProperty, value); }
		}

		public int RowsCount
		{
			get { return (int)GetValue(RowsCountProperty); }
			set { SetValue(RowsCountProperty, value); }
		}

		public MovieGroup MoviesGroup
		{
			get { return (MovieGroup)GetValue(MoviesGroupProperty); }
			set { SetValue(MoviesGroupProperty, value); }
		}


		#endregion

		#region Events

		public static readonly RoutedEvent ActionBtnClickEvent = EventManager.RegisterRoutedEvent(
			"ActionBtnClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaGrid));

		public MediaGrid(List<Button> mediaBtns, MediaElement videoBackround)
		{
			_mediaBtns = mediaBtns;
			_videoBackround = videoBackround;
		}

		public event RoutedEventHandler ActionBtnClick
		{
			add { AddHandler(ActionBtnClickEvent, value); }
			remove { RemoveHandler(ActionBtnClickEvent, value); }
		}

		#endregion

		#region Override methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			
			if (e.Property == RowsCountProperty)
			{
				OnRowsCountPropertyChanged();
			}
			if (e.Property == ColumnsCountProperty)
			{
				OnColumnsCountPropertyChanged();
			}

			if (e.Property == SelectedMovieIdProperty ||
				e.Property == MoviesGroupProperty)
			{
				if (e.Property == MoviesGroupProperty)
				{
					_backgroundMedia.Clear();
				}

				ClearMediaNodesLayout();
				SetMediaLayout();
			}
		}

		#endregion

		#region Private methods

		private void SetMediaLayout()
		{
			var selectedMovie = !string.IsNullOrEmpty(SelectedMovieId)
										? MoviesGroup.Movies.FirstOrDefault(mov => mov.Id == SelectedMovieId)
										: null;

			if (selectedMovie == null)
				return;

			if (!_backgroundMedia.ContainsKey(SelectedMovieId))
				_backgroundMedia.Add(SelectedMovieId, CreateMediaElement(selectedMovie.Source));

			PlayBackgroundMovie(_backgroundMedia[SelectedMovieId]);

			//Add all movies to the background (perfomance issue: need to the investigations in next steps
			foreach (var movie in MoviesGroup.Movies.Where(movie => !_backgroundMedia.ContainsKey(movie.Id)))
			{
				_backgroundMedia.Add(movie.Id, CreateMediaElement(movie.Source));
			}
		}

		private void PlayBackgroundMovie(MediaElement mediaElement)
		{
			if (!Children.Contains(mediaElement))
			{
				Children.Insert(0, mediaElement);
			}
			_backgroundMedia[SelectedMovieId].Stop();
			_backgroundMedia[SelectedMovieId].Play();
		}


		private MediaElement CreateMediaElement(string moviePath)
		{
			var mediaElement = new MediaElement
			{
				Source = new Uri(moviePath, UriKind.Relative),
				Width = Width,
				Height = Height,
				LoadedBehavior = MediaState.Manual
			};
			mediaElement.MediaEnded += OnBackgroundVideoEnded;
			mediaElement.MediaOpened += OnBackgroundVideoOpened;
			return mediaElement;
		}

		private void ShowMediaNodes()
		{
			if (MediaNodes == null)
				return;

			foreach (var mediaNode in MediaNodes)
			{
				var btn = mediaNode.MediaType == MediaNodeType.MovieAction
							? CreateActionBtn(mediaNode)
							: CreateInfoBtn();
				btn.SetValue(RowProperty, mediaNode.Y);
				btn.SetValue(ColumnProperty, mediaNode.X);
				_mediaBtns.Add(btn);
				Children.Add(btn);
			}
		}

		private Button CreateInfoBtn()
		{
			return new Button
			       	{
						Height = Height / RowsCount, 
						Width = Width / ColumnsCount,
						Background = Brushes.Blue
			       	};
		}

		private Button CreateActionBtn(IMediaNode mediaNode)
		{
			var btn = new ActionButton
			          	{
							Height = Height / RowsCount,
							Width = Width / ColumnsCount, 
							Foreground = Brushes.Azure,
							Background = Brushes.DarkRed
			          	};
			btn.SetValue(ActionButton.MovieIdProperty, mediaNode.MovieId);
			btn.Click += OnActionBtnClick;
			return btn;
		}

		private void OnColumnsCountPropertyChanged()
		{
			ColumnDefinitions.Clear();
			for (var index = 0; index < ColumnsCount; index++)
			{
				ColumnDefinitions.Add(new ColumnDefinition());
			}
			_videoBackround.SetValue(ColumnSpanProperty, ColumnsCount);
		}

		private void OnRowsCountPropertyChanged()
		{
			RowDefinitions.Clear();
			for (var index = 0; index < RowsCount; index++)
			{
				RowDefinitions.Add(new RowDefinition());
			}
			_videoBackround.SetValue(RowSpanProperty, RowsCount);
		}

		private void OnActionBtnClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as ActionButton;
			if (btn == null)
				return;

			SelectedMovieId = btn.MovieId;
			RaiseEvent(new RoutedEventArgs(ActionBtnClickEvent));
		}

		private void OnBackgroundVideoEnded(object sender, RoutedEventArgs e)
		{
			//Check that we still stay in current action
			if (_previousBackgroundMovie == _backgroundMedia[SelectedMovieId])
				ShowMediaNodes();
		}

		private void OnBackgroundVideoOpened(object sender, RoutedEventArgs e)
		{
			//Uses for smooth movie loading (removed previous media element only after new movie will be loaded)
			Children.Remove(_previousBackgroundMovie);
			_previousBackgroundMovie = _backgroundMedia[SelectedMovieId];
		}

		private void ClearMediaNodesLayout()
		{
			foreach (var btn in _mediaBtns)
			{
				btn.Click -= OnActionBtnClick;
				Children.Remove(btn);
			}
			_mediaBtns.Clear();
		}

		#endregion

	}
}
