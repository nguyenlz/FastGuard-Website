using System;
using System.Collections.Generic;

namespace FastGuard.Data
{
    public partial class Location
    {
        public Location()
        {
            PickLocations = new HashSet<PickLocation>();
            RouteLocationId1Navigations = new HashSet<Route>();
            RouteLocationId2Navigations = new HashSet<Route>();
        }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }

        public virtual ICollection<PickLocation> PickLocations { get; set; }
        public virtual ICollection<Route> RouteLocationId1Navigations { get; set; }
        public virtual ICollection<Route> RouteLocationId2Navigations { get; set; }
    }
}
