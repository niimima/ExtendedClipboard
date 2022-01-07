using ExtendedClipboard.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uwpDataTransfer = Windows.ApplicationModel.DataTransfer;

namespace ExtendedClipboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <remarks>
    /// 以下の記事のWpfSampleをベースに作成。
    /// https://codezine.jp/article/detail/11229
    /// </remarks>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += async (s, e) =>
            {
                await ClipboardHistoryData.TryUpdateAsync();
                this.ListView.ItemsSource = ClipboardHistoryData.Items;
            };

            uwpDataTransfer.Clipboard.HistoryChanged += async (s, e) =>
            {
                await ClipboardHistoryData.TryUpdateAsync();
                // WPF では、フォーカスが無くても、最小化されていても、構わず取得できる
                this.ListView.ItemsSource = ClipboardHistoryData.Items;
            };
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.ListView.ItemsSource = ClipboardHistoryData.Items.Where(i => i.TextHead.Contains(this.TextBox.Text));
        }

        private void ListView_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ClipboardHistoryData)ListView.SelectedItem;
            uwpDataTransfer.Clipboard.SetHistoryItemAsContent(item.HistoryItem);
        }
    }
}
