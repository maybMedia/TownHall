namespace TownHall
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

			Routing.RegisterRoute(nameof(Messages), typeof(Messages));
			Routing.RegisterRoute(nameof(Buy), typeof(Buy));
			Routing.RegisterRoute(nameof(Sell), typeof(Sell));
			Routing.RegisterRoute(nameof(Account), typeof(Account));
			Routing.RegisterRoute(nameof(Listings), typeof(Listings));
		}
	}
}
