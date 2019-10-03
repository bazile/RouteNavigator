using System;
using System.Collections.Generic;

namespace RouteNavigator
{
    interface IRouteDisplay
    {
        List<Route> Routes { get; set; }

        event DisplayCountChange DisplayCountChanged;

        void DisplayRoutes(string filter = null);
    }
}