using GeneGenie.Geocoder;
using GeneGenie.Geocoder.Models;
using GeneGenie.Geocoder.Models.Geo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Geomapping
{
    public class GeocodeService
    {
        private GeocodeManager geocodeManager;

        public GeocodeService(string key)
        {
            var geocoderSettings = new List<GeocoderSettings>
            {
                new GeocoderSettings { ApiKey = key, GeocoderName = GeneGenie.Geocoder.Services.GeocoderNames.Google },
            };

            geocodeManager = GeocodeManager.Create(geocoderSettings);
        }

        public async Task<List<GeocodeResponseLocation>> GetLocationAsync(string address)
        {
            var response = await geocodeManager.GeocodeAddressAsync(address);
            return response.Locations;
        }

        public double GetRadius(GeocodeResponseLocation location)
        {
            var distanceToNorthEast = GetDistanceFromLatLonInKm(location.Location.Latitude, location.Location.Longitude, location.Bounds.NorthEast.Latitude, location.Bounds.NorthEast.Longitude);
            var distanceToSouthWest = GetDistanceFromLatLonInKm(location.Location.Latitude, location.Location.Longitude, location.Bounds.SouthWest.Latitude, location.Bounds.SouthWest.Longitude);

            return Math.Max(distanceToNorthEast, distanceToSouthWest);
        }

        public double GetDistance(LocationPair location, LocationPair innerLocation)
        {
            return GetDistanceFromLatLonInKm(location.Latitude, location.Longitude, innerLocation.Latitude, innerLocation.Longitude);
        }

        private double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var radiusOfEarthInKm = 6371;
            var dLat = DegToRad(lat2 - lat1);
            var dLon = DegToRad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = radiusOfEarthInKm * c;
            return distance;
        }

        private double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
