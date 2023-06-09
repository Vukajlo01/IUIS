using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Data
{
	public class ComboBoxItemsData
	{
		public static Dictionary<string, string> entityTypes = new Dictionary<string, string>()
		{
            { "Elektronski", "pack://application:,,,/NetworkService;component/Images/elektronski.jpeg" },
            { "Turbinski", "pack://application:,,,/NetworkService;component/Images/turbinski.jpg" },
            { "Zapreminski", "pack://application:,,,/NetworkService;component/Images/zapreminski.jpg" }
        };
    }
}
