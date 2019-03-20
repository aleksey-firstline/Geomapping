using FluentAssertions;
using GeneGenie.Geocoder.Services;
using Geomapping;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class GeocodeServiceTest
    {
        [SetUp]
        public void StartUp()
        {
        }

        [Test]
        public void IsLocationIncludes_Bing_ShouldBeTrue()
        {
            var service = new GeocodeService("AinYG_Q_noj6lfOQmtIBK49BEWWi1D_QSBt5fFCHxGQZFfKNSfwAM30W2mGlwhFj", GeocoderNames.Bing);
            var location = service.GetLocationAsync("35801 usa").Result.FirstOrDefault();
            var innerLocation = service.GetLocationAsync("1405 Big Cove Rd SE").Result.FirstOrDefault();
            var radius = service.GetRadius(location);
            var isIncludes = service.IsLocationIncludes(location.Bounds, innerLocation.Location, radius);

            isIncludes.Should().BeTrue();
        }

        [Test]
        public void IsLocationIncludes_Bing_ShouldBeFalse()
        {
            var service = new GeocodeService("AinYG_Q_noj6lfOQmtIBK49BEWWi1D_QSBt5fFCHxGQZFfKNSfwAM30W2mGlwhFj", GeocoderNames.Bing);
            var location = service.GetLocationAsync("72201 usa").Result.FirstOrDefault();
            var innerLocation = service.GetLocationAsync("521 Jack Stephens Dr, Little Rock, AR 72205, США").Result.FirstOrDefault();
            var radius = service.GetRadius(location);
            var isIncludes = service.IsLocationIncludes(location.Bounds, innerLocation.Location, radius);

            isIncludes.Should().BeFalse();
        }

        [Test]
        public void IsLocationIncludes_Google_ShouldBeTrue()
        {
            var service = new GeocodeService("AIzaSyDXrRQME2vYtdi94sWJV33Cl3A4mESSAXA", GeocoderNames.Google);
            var location = service.GetLocationAsync("35801 usa").Result.FirstOrDefault();
            var innerLocation = service.GetLocationAsync("1405 Big Cove Rd SE").Result.FirstOrDefault();
            var radius = service.GetRadius(location);
            var isIncludes = service.IsLocationIncludes(location.Bounds, innerLocation.Location, radius);

            isIncludes.Should().BeTrue();
        }

        [Test]
        public void IsLocationCross_Google_ShouldBeTrue()
        {
            var service = new GeocodeService("AIzaSyDXrRQME2vYtdi94sWJV33Cl3A4mESSAXA", GeocoderNames.Google);
            var location = service.GetLocationAsync("35801 usa").Result.FirstOrDefault();
            var innerLocation = service.GetLocationAsync("35811 usa").Result.FirstOrDefault();
            var radius = service.GetRadius(location);
            var innerRadius = service.GetRadius(innerLocation);
            var isIncludes = service.IsLocationCross(location.Bounds, innerLocation.Bounds, radius, innerRadius);

            isIncludes.Should().BeTrue();
        }

        [Test]
        public void IsLocationIncludes_Google_ShouldBeFalse()
        {
            var service = new GeocodeService("AIzaSyDXrRQME2vYtdi94sWJV33Cl3A4mESSAXA", GeocoderNames.Google);
            var location = service.GetLocationAsync("72201 usa").Result.FirstOrDefault();
            var innerLocation = service.GetLocationAsync("521 Jack Stephens Dr, Little Rock, AR 72205, США").Result.FirstOrDefault();
            var radius = service.GetRadius(location);
            var isIncludes = service.IsLocationIncludes(location.Bounds, innerLocation.Location, radius);

            isIncludes.Should().BeFalse();
        }
    }
}
