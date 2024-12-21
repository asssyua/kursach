using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using kursach.Model; 
using DevExpress.Mvvm.Native;

namespace kursach.ViewModel
{
    public static class SearchHelper
    {
        /// <summary>
        /// Универсальный метод для поиска по коллекции.
        /// </summary>
        public static ObservableCollection<CardInformation> SearchCards(ObservableCollection<CardInformation> items, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return items;  

            Regex regex = new Regex(query, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var filteredItems = items.Where(item => regex.IsMatch(item.Title ?? string.Empty) || regex.IsMatch(item.Description ?? string.Empty))
                                     .ToObservableCollection();

            return filteredItems;
        }
    }
}
