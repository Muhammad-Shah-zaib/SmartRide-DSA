global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.Services;
global using SmartRide.src.DataStructures;
global using Microsoft.EntityFrameworkCore;
global using SmartRide.src.Utilities.GraphAlgos;


var context = new SmartRideDbContext();

var carPoolService = new CarpoolService(context);
var mapService = new MapService(context);

carPoolService.GetAllCarpools();

var userSrc = "MASJID";

var result = carPoolService.FindAndAssignNearestCarpool(userSrc: userSrc, graph: mapService._graph);

if (result.HasValue)
{
    var (carpool, route) = result.Value;

    Console.WriteLine($"carpoolId: {carpool.CarpoolId} -> maxPassengers: {carpool.MaxPassengers} -> status: {carpool.Status} -> src: {carpool.Src}");
    Console.WriteLine(string.Join("->", route));
}