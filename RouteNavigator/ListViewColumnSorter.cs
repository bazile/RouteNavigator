using System.Collections;
using System.Globalization;
using System.Windows.Forms;

namespace RouteNavigator
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>Case insensitive comparer object</summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn { set; get; }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order { set; get; }

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            SortColumn = 0;
            Order = SortOrder.None;
            _objectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            var itemX = (ListViewItem)x;
            var itemY = (ListViewItem)y;

            if (itemX.ListView.Columns[SortColumn].Tag is string tag && tag == "number")
            {
                return CompareNumbers(itemX, itemY);
            }

            return CompareText(itemX, itemY);
        }

        private int CompareNumbers(ListViewItem itemX, ListViewItem itemY)
        {
            int x = int.Parse(itemX.SubItems[SortColumn].Text, NumberStyles.AllowThousands);
            int y = int.Parse(itemY.SubItems[SortColumn].Text, NumberStyles.AllowThousands);
            int compareResult = x.CompareTo(y);
            if (Order == SortOrder.Ascending)
            {
                return compareResult;
            }
            if (Order == SortOrder.Descending)
            {
                return (-compareResult);
            }

            return 0;
        }

        private int CompareText(ListViewItem itemX, ListViewItem itemY)
        {
            int compareResult = _objectCompare.Compare(itemX.SubItems[SortColumn].Text, itemY.SubItems[SortColumn].Text);
            if (Order == SortOrder.Ascending)
            {
                return compareResult;
            }
            if (Order == SortOrder.Descending)
            {
                return (-compareResult);
            }

            return 0;
        }
    }
}