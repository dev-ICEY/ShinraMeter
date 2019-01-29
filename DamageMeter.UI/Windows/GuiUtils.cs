using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace DamageMeter.UI
{
    public static class GuiUtils
    {
        public static ICollectionViewLiveShaping InitLiveView<T>(Predicate<object> predicate, IEnumerable<T> source,
            string[] filters, SortDescription[] sortFilters)
        {
            var cv = new CollectionViewSource { Source = source }.View;
            cv.Filter = predicate;
            var liveView = cv as ICollectionViewLiveShaping;
            if (liveView != null && !liveView.CanChangeLiveFiltering) return null;
            if (filters.Length > 0)
            {
                foreach (var filter in filters)
                {
                    liveView?.LiveFilteringProperties.Add(filter);
                }

                if (liveView != null) liveView.IsLiveFiltering = true;
            }

            if (sortFilters.Length > 0)
            {
                foreach (var filter in sortFilters)
                {
                    (liveView as ICollectionView)?.SortDescriptions.Add(filter);
                    liveView.LiveSortingProperties.Add(filter.PropertyName);
                }

                if (liveView != null) liveView.IsLiveSorting = true;
            }

            return liveView;
        }


    }
}