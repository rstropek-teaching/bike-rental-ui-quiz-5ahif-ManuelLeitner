using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.Presentation {
    public static class Utils {
        public static void AddRange<T>(this ObservableCollection<T> c, IEnumerable<T> i)
        {
            foreach (var n in i)
            {
                c.Add(n);
            }
        }
    }
}
