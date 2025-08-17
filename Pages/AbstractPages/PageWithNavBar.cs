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

			var pageNames = new[] { "Messages", "Buy", "Sell", "Account" };

			foreach (var name in pageNames)
			{
				var button = new Button { Text = name };
				button.Clicked += async (s, e) => await Shell.Current.GoToAsync(name);
				layout.Children.Add(button);
			}

			Content = layout;
		}
	}
}
