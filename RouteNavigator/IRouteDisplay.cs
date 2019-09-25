using System.Collections.Generic;

namespace RouteNavigator
{
    interface IRouteDisplay
    {
        void DisplayRoutes(string filter = null);

        List<Route> Routes { get; set; }
    }
}