using GeneGenie.Geocoder;
using GeneGenie.Geocoder.Models;
using GeneGenie.Geocoder.Models.Geo;
using GeneGenie.Geocoder.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geomapping
{
    public class GeocodeService
    {
        private GeocodeManager geocodeManager;

        public GeocodeService(string key, GeocoderNames geocoder)
        {
            var geocoderSettings = new List<GeocoderSettings>
            {
                new GeocoderSettings { ApiKey = key, GeocoderName = geocoder },
            };

            geocodeManager = GeocodeManager.Create(geocoderSettings);
        }

        public async Task<List<GeocodeResponseLocation>> GetLocationAsync(string address)
        {
            var response = await geocodeManager.GeocodeAddressAsync(address);
            return response.Locations;
        }

        public bool IsLocationIncludes(Bounds bounds, LocationPair innerLocation, double radius)
        {
            var middle = GetMiddle(bounds);
            var distance = GetDistance(middle, innerLocation);
            return distance < radius;
        }

        public double GetRadius(GeocodeResponseLocation location)
        {
            var middle = GetMiddle(location.Bounds);
            var distanceToNorthEast = GetDistance(middle, location.Bounds.NorthEast);
            var distanceToSouthWest = GetDistance(middle, location.Bounds.SouthWest);

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

        private LocationPair GetMiddle(Bounds bounds)
        {
            var lat1 = bounds.NorthEast.Latitude;
            var lon1 = bounds.NorthEast.Longitude;

            var lat2 = bounds.SouthWest.Latitude;
            var lon2 = bounds.SouthWest.Longitude;

            double dLon = DegToRad(lon2 - lon1);

            lat1 = DegToRad(lat1);
            lat2 = DegToRad(lat2);
            lon1 = DegToRad(lon1);

            double Bx = Math.Cos(lat2) * Math.Cos(dLon);
            double By = Math.Cos(lat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);

            return new LocationPair
            {
                Latitude = RadToDeg(lat3),
                Longitude = RadToDeg(lon3)
            };
        }

        private double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        private double RadToDeg(double rad)
        {
            return (180 / Math.PI) * rad;
        }
    }
}
