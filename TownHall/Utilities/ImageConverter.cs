using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is byte[] bytes && bytes.Length > 0)
			{
				return ImageSource.FromStream(() => new MemoryStream(bytes));
			}
			return null;
		}

		// interface defines so need to implement this method, but we don't actually need to use it
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
