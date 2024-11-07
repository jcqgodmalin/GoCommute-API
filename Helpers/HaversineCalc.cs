using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCommute.Helpers
{
    public static class HaversineCalc
    {
        private const double EarthRadiusInKM = 6371.0;

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2){

            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            // Calculate differences
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            // Haversine formula
            double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Pow(Math.Sin(deltaLon / 2), 2);
            
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            return EarthRadiusInKM * c;

        }

        private static double DegreesToRadians(double degrees){
            return degrees * Math.PI / 180.0;
        }
    }
}