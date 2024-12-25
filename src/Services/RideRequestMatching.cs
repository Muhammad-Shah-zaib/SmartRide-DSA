global using SmartRide.src.DataStructures;
using SmartRide.src.Dtos;
using SmartRide.Models;
using SmartRide.src.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRide.src.Utilities.GraphAlgos;
namespace SmartRide.src.Services
{
    public class RideRequestMatching<T> where T : IComparable<T>//input the node of the driver and the user
    {
        private readonly RideRequestDto _request ;
        public RideRequestMatching() {
            _request = new RideRequestDto();
        }
        private T ConvertToT(string input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public DriverDto RequestMatching(Graph<T> map, T user, List<DriverDto> drivers)
        {
            var shortestpath = new ShortestPath<T>();

            //Tracks the minimum distance
            double minimumdistance = double.MaxValue;

            //Priority Queue based on the distance
            PriorityQueues<DriverDto> NearestDrivers = new PriorityQueues<DriverDto>();


            //Calculates distance from all the drivers
            foreach (var driver in drivers)
            {
                
                var path = shortestpath.Dijsktra(map,ConvertToT(driver.CurrentPosition),user);
                double distance = 0;

                //if path exists
                if(path.Count != null && path.Count > 1)
                {
                    for(int i = 0; i < path.Count - 1; i++)
                    {
                        distance += map.GetEdgeWeight(path[i], path[i + 1]);

                    }


                    //storing in the queue
                    NearestDrivers.Enqueue(driver, distance);
                }
            }

            //returns the driver with the least distance
            var (ur_driver,dist) = NearestDrivers.Dequeue();
            return ur_driver;
        }
    }
}
