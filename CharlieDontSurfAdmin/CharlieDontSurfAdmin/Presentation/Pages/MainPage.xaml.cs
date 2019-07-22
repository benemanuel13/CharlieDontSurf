using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI;

using CharlieDontSurfAdmin.Presentation.ViewModels;
using CharlieDontSurfAdmin.Models.Orders;
using CharlieDontSurfAdmin.Infrastructure;
using BensJson;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CharlieDontSurfAdmin.Presentation.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainPageView view;

        Dictionary<int, Border> borders = new Dictionary<int, Border>();

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(1240, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;

            view = new MainPageView();
            view.OrdersRecieved += View_OrdersRecieved;
            view.CurrentOrderChanged += View_CurrentOrderChanged;

            view.Start();
        }

        private void View_CurrentOrderChanged(object sender, Events.CurrentOrderChangedEventArgs e)
        {
            AddressLine1.Text = e.CurrentOrder.AddressLine1;
            AddressLine2.Text = e.CurrentOrder.AddressLine2;
            City.Text = e.CurrentOrder.City;
            County.Text = e.CurrentOrder.County;
            Postcode.Text = e.CurrentOrder.Postcode;
            Country.Text = e.CurrentOrder.Country;
        }

        private void View_OrdersRecieved(object sender, Events.OrdersRecievedEventArgs e)
        {
            ListOrders(e.Orders);
        }

        private void ListOrders(List<Order> orders)
        {
            foreach (Order order in orders)
            {
                OrderList.RowDefinitions.Add(new RowDefinition());
                OrderList.RowDefinitions[OrderList.RowDefinitions.Count - 1].Height = GridLength.Auto;

                Border newBorder = new Border();
                OrderList.Children.Add(newBorder);
                newBorder.SetValue(Grid.ColumnProperty, 0);
                newBorder.SetValue(Grid.RowProperty, OrderList.RowDefinitions.Count - 1);
                newBorder.SetValue(Grid.ColumnSpanProperty, 2);

                borders.Add(order.Id, newBorder);

                TextBlock orderId = new TextBlock();
                orderId.Tag = order.Id;
                orderId.Width = 160;
                orderId.PointerEntered += Order_PointerEntered;
                orderId.PointerExited += Order_PointerExited;
                orderId.PointerPressed += Order_PointerPressed;
                orderId.Text = order.Id.ToString();
                orderId.FontSize = 30;
                orderId.Padding = new Thickness(5, 0, 0, 5);
                OrderList.Children.Add(orderId);
                orderId.SetValue(Grid.ColumnProperty, 0);
                orderId.SetValue(Grid.RowProperty, OrderList.RowDefinitions.Count - 1);

                TextBlock orderName = new TextBlock();
                orderName.Tag = order.Id;
                orderName.Width = 350;
                orderName.PointerEntered += Order_PointerEntered;
                orderName.PointerExited += Order_PointerExited;
                orderName.PointerPressed += Order_PointerPressed;
                orderName.Text = order.Recipient;
                orderName.FontSize = 30;
                orderName.Padding = new Thickness(5, 0, 0, 5);
                OrderList.Children.Add(orderName);
                orderName.SetValue(Grid.ColumnProperty, 1);
                orderName.SetValue(Grid.RowProperty, OrderList.RowDefinitions.Count - 1);
            }
        }

        private void Order_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            TextBlock text = (TextBlock)sender;
            view.CurrentOrder = view.Orders[(int)text.Tag];
        }

        private void Order_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TextBlock text = (TextBlock)sender;
            Border thisBorder = borders[(int)text.Tag];
            thisBorder.Background = new SolidColorBrush(Colors.Beige);
        }

        private void Order_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TextBlock text = (TextBlock)sender;
            Border thisBorder = borders[(int)text.Tag];
            thisBorder.Background = new SolidColorBrush(Colors.White);
        }
    }
}
