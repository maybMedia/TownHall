using System.Xml.Linq;

namespace TownHall
{ 
	public abstract class PageWithNavBar : ContentPage
	{
		protected PageWithNavBar()
		{
			var grid = new Grid
			{
				Padding = new Thickness(10),
				RowSpacing = 0,
				ColumnSpacing = 5,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto }
				}
			};

			var pageTypes = new[] { typeof(Messages), typeof(Buy), typeof(Sell), typeof(Account) };

			int col = 0;
			foreach (var type in pageTypes)
			{
				var button = new Button
				{
					Text = type.Name[..1], // just using first letter for now
					WidthRequest = 30,
					HeightRequest = 30,
					FontSize = 10
				};

				button.Clicked += async (s, e) =>
				{
					if (type.Name == nameof(Buy))
						await Shell.Current.GoToAsync("///Buy");
					else
						await Shell.Current.GoToAsync(type.Name);
				};

				grid.Add(button);
				Grid.SetRow(button, 0);
				Grid.SetColumn(button, col);

				col++;
			}

			Content = grid;
		}

	}
}
