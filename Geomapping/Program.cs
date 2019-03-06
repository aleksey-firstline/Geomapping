using System;

namespace Geomapping
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new GeocodeService("");
            Console.WriteLine("Input first location address:");
            Console.WriteLine();
            var address1 = Console.ReadLine();
            var locations = worker.GetLocationAsync(address1).Result;
            foreach (var location in locations)
            {
                Console.WriteLine($"Address of {location.FormattedAddress} has a location of {location.Location.Latitude} / {location.Location.Longitude}");
                Console.WriteLine();

                Console.WriteLine("Input second location address:");
                Console.WriteLine();

                var radius = worker.GetRadius(location);
                var address2 = Console.ReadLine();
                var innerLocations = worker.GetLocationAsync(address2).Result;
                foreach (var innerLocation in innerLocations)
                {
                    Console.WriteLine($"Address of {innerLocation.FormattedAddress} has a location of {innerLocation.Location.Latitude} / {innerLocation.Location.Longitude}");
                    Console.WriteLine();

                    var isLocatiomExist = worker.IsLocationIncludes(location.Location, innerLocation.Location, radius);
                    Console.WriteLine($"Does {address1} include {address2} in radius of {radius}: {isLocatiomExist}");
                }
            }

            Console.ReadLine();
        }
    }
}
