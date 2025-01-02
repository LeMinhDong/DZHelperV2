using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace DZHelper.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddItemsNotExits<T>(this ObservableCollection<T> list1, IEnumerable<T> list2)
        {
            var itemsToAdd = list2.Where(item2 => !list1.Any(item1 => item1.Equals(item2)));
            Application.Current.Dispatcher.Invoke(new Action(() => {
                foreach (var item in itemsToAdd)
                    list1.Add(item);
            }));
        }
        public static void RemoveItemsExits<T>(this ObservableCollection<T> list1, IEnumerable<T> list2)
        {
            var itemsToRemove = list1.Where(item1 => !list2.Any(item2 => item1.Equals(item2))).ToList();
            Application.Current.Dispatcher.Invoke(new Action(() => {
                foreach (var item in itemsToRemove)
                    list1.Remove(item);
            }));
        }

        public static void SortList<T, TKey>(this ObservableCollection<T> collection, Func<T, TKey> keySelector) where TKey : IComparable
        {
            // Kiểm tra tham số đầu vào
            if (collection == null || keySelector == null)
                return;

            // Sắp xếp danh sách tạm
            var sorted = collection.OrderBy(keySelector).ToList();

            // Cập nhật lại ObservableCollection
            Application.Current.Dispatcher.Invoke(new Action(() => {
                for (int i = 0; i < sorted.Count; i++)
                {
                    if (!EqualityComparer<T>.Default.Equals(collection[i], sorted[i]))
                    {
                        collection.Move(collection.IndexOf(sorted[i]), i);
                    }
                }
            }));
        }
    }
}
