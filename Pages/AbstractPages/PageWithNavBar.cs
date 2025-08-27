namespace TownHall
{
	public abstract class PageWithNavBar : ContentPage
	{
		protected PageWithNavBar()
		{
			Shell.SetNavBarIsVisible(this, true);
		}
	}
}