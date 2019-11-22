using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RouteNavigator
{
    internal static class ListViewExtensions
    {
        public static void SelectAllItems(this ListView listview)
        {
            SetItemState(listview, -1, LVIS_SELECTED, 2);
        }

        /// <summary>
        /// Set the item state on the given item
        /// </summary>
        /// <param name="list">The listview whose item's state is to be changed</param>
        /// <param name="itemIndex">The index of the item to be changed</param>
        /// <param name="mask">Which bits of the value are to be set?</param>
        /// <param name="value">The value to be set</param>
        private static void SetItemState(ListView list, int itemIndex, int mask, int value)
        {
            if (!list.IsHandleCreated) return;

            var lvItem = new LVITEM { stateMask = mask, state = value };
            SendMessageLVItem(list.Handle, LVM_SETITEMSTATE, itemIndex, ref lvItem);
        }

        #region P/Invoke stuff

        private const int LVIS_SELECTED = 0x0002;
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageLVItem(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);

        #endregion    
    }
}