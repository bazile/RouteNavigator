﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace RouteNavigator
{
    public partial class ListViewScreen : UserControl, IRouteDisplay
    {
        private readonly ListViewColumnSorter _columnSorter;

        [Browsable(false)]
        public List<Route> Routes { get; set; }

        public event DisplayCountChange DisplayCountChanged;

        private IObservable<object> _mySequence;

        public ListViewScreen()
        {
            InitializeComponent();

            _columnSorter = new ListViewColumnSorter();
            lvRoutes.ListViewItemSorter = _columnSorter;
        }

        private void ListViewScreen_Load(object sender, EventArgs e)
        {
            // Debounce TextChanged event using Rx library
            _mySequence = Observable.FromEventPattern(
                h => tbFilter.TextChanged += h,
                h => tbFilter.TextChanged -= h
            );
            var uiThread = Scheduler.CurrentThread;
            _mySequence.Throttle(TimeSpan.FromMilliseconds(200), uiThread).Subscribe(obj => DisplayRoutes(tbFilter.Text));
        }

        public void DisplayRoutes(string filter = null)
        {
            if (filter == null)
            {
                filter = tbFilter.Text;
            }

            lvRoutes.BeginUpdate();
            lvRoutes.Items.Clear();
            var filteredRoutes = FilterRoutes(Routes, filter);
            foreach (var route in filteredRoutes)
            {
                var lvi = new ListViewItem(route.RequiresAuthorization ? "Yes\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0" : "No");
                lvi.SubItems.Add(route.HttpMethod);
                lvi.SubItems.Add(route.FullRoute);
                lvi.SubItems.Add(route.ClassName);
                lvi.SubItems.Add(route.MethodName);
                lvRoutes.Items.Add(lvi);
            }
            lvRoutes.AutoResizeColumns(lvRoutes.Items.Count > 0 ? ColumnHeaderAutoResizeStyle.ColumnContent : ColumnHeaderAutoResizeStyle.HeaderSize);
            lvRoutes.EndUpdate();

            DisplayCountChanged?.Invoke(lvRoutes.Items.Count);
        }

        private static IEnumerable<Route> FilterRoutes(IEnumerable<Route> routes, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return routes;
            else
            {
                if (Uri.TryCreate(filter, UriKind.Absolute, out Uri uri))
                {
                    filter = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                }

                filter = filter.Trim().TrimStart('/');
                string normalizedFilter = Route.Normalize(filter);
                return routes.Where(r =>
                    r.FullRoute.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1
                    || r.NormalizedFullRoute.IndexOf(normalizedFilter, StringComparison.OrdinalIgnoreCase) != -1
                    || r.ClassName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1
                    || r.MethodName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1
                );
            }
        }

        private void OnListViewRoutes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _columnSorter.SortColumn)
            {
                _columnSorter.Order = _columnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                _columnSorter.SortColumn = e.Column;
                _columnSorter.Order = SortOrder.Ascending;
            }

            ((ListView)sender).Sort();
        }

        private void lvRoutes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelectionToClipboard(e);
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                lvRoutes.SelectAllItems();
            }
        }

        private void CopySelectionToClipboard(KeyEventArgs e)
        {
            if (lvRoutes.SelectedItems.Count == 0) return;

            e.Handled = true;
            string[] array = new string[lvRoutes.SelectedItems[0].SubItems.Count + 1];

            string text;
            if (lvRoutes.SelectedItems.Count == 1)
            {
                var lvi = lvRoutes.SelectedItems[0];
                array[0] = lvi.Text;
                for (int i = 0; i < lvi.SubItems.Count; i++)
                {
                    array[i + 1] = lvi.SubItems[i].Text;
                }
                text = string.Join(" ", array);
            }
            else
            {
                var sb = new StringBuilder();
                foreach (ListViewItem lvi in lvRoutes.SelectedItems)
                {
                    array[0] = lvi.Text;
                    for (int i = 0; i < lvi.SubItems.Count; i++)
                    {
                        array[i + 1] = lvi.SubItems[i].Text;
                    }
                    sb.AppendLine(string.Join(" ", array));
                }
                text = sb.ToString();
            }
            Clipboard.SetText(text);
        }
    }
}
