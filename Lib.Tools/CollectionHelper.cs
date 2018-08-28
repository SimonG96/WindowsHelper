using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace Lib.Tools
{
    public static class CollectionHelper
    {
        public static void EnableCollectionSynchronization<T>(this ObservableCollection<T> collection)
        {
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
                throw new InvalidOperationException("EnableCollectionSynchronization can only be called from Dispatcher Thread");

            BindingOperations.EnableCollectionSynchronization(collection, collection);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ObservableCollection<T> newCollection = new ObservableCollection<T>();

            foreach (var item in source)
            {
                newCollection.Add(item);
            }

            return newCollection;
        }
    }
}
