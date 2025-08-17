using System.Xml.Linq;

namespace TownHall
{ 
	public abstract class PageWithNavBar : ContentPage
	{
		protected PageWithNavBar(string pageTitle)
		{
			Title = pageTitle;

			var layout = new VerticalStackLayout
			{
				Padding = 20,
				Spacing = 10
			};

			layout.Children.Add(new Label
			{
				Text = pageTitle,
				FontSize = 24,
				HorizontalOptions = LayoutOptions.Center
			});

			var pageTypes = new[] { typeof(Messages), typeof(Buy), typeof(Sell), typeof(Account) };

			foreach (var type in pageTypes)
			{
				var button = new Button { Text = type.Name };
				button.Clicked += async (s, e) =>
				{
					// have to treat Buy differently to other pages as its defined as a ShellContent in order to be the start page
					if (type.Name == nameof(Buy))
						await Shell.Current.GoToAsync("///Buy");
					else
						await Shell.Current.GoToAsync(type.Name);
				};
				layout.Children.Add(button);
			}

			Content = layout;
		}
	}
}
