// using System;
// using System.Net;
// using System.Collections.Generic;
// using System.Linq;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Navigation;
// using Microsoft.Phone.Controls;
// using Microsoft.Phone.Shell;
// using Windows.Devices.Geolocation;
// using System.Device.Location;

// namespace commanderscall.ui
// {
//     public partial class MainPage : PhoneApplicationPage
//     {
//         Geolocator locator;
//         public MainPage()
//         {
//             InitializeComponent();

//             locator = new Geolocator();
//             CenterOnUser();
//         }

//         // Load data for the ViewModel Items
//         protected override void OnNavigatedTo(NavigationEventArgs e)
//         {
//             if (!App.ViewModel.IsDataLoaded)
//             {
//                 App.ViewModel.LoadData();
//             }
//         }

//         private async void CenterOnUser()
//         {
//             var location = await locator.GetGeopositionAsync();
//             BattleMap.Center = new GeoCoordinate(location.Coordinate.Latitude, location.Coordinate.Longitude);
//             BattleMap.ZoomLevel = 11;
//         }

//         private void FindMeButton(object sender, EventArgs e)
//         {
//             CenterOnUser();
//         }
//     }
// }