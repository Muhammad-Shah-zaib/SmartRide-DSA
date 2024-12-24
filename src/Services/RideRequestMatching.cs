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
        private readonly RideRequestDto _request = new RideRequestDto();
        public RideRequestMatching() { }
        public void RequestMatching(Graph<T> map,T driver,T user) 
        {
            var shortestpath = new ShortestPath<T>();
            var path = shortestpath.Dijsktra(map, driver, user);

            double distance = 0;
            if (path != null && path.Count > 1)
            {
                for(int i = 0; i < path.Count -1; i++)
                {
                    distance += map.GetEdgeWeight(path[i], path[i + 1]);
                }
            }


            Console.WriteLine("The path is : "+distance);
        }
    }
}
