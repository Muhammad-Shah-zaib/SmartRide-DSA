global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.Services;
global using SmartRide.src.DataStructures;
global using Microsoft.EntityFrameworkCore;
global using SmartRide.src.Utilities.GraphAlgos;

using SmartRide.CLI;

var context = new SmartRideDbContext();

Main mainCli = new(context);
mainCli.Run();