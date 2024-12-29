using Microsoft.EntityFrameworkCore.Query;
using SmartRide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.CLI
{
    public class DriverMenu
    {

        public SmartRideDbContext _context;
        public DriverDto Driver;
        public DriverService _driverService;
        public DriverRatingService _driverRatingService;

        public DriverMenu(SmartRideDbContext context, DriverDto driver)
        {
            _context = context;
            Driver = driver;
            _driverService = new DriverService(context);
            _driverRatingService = new DriverRatingService(context);
        }
        public void Run()
        {
            // Loop until driver decides to log out
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to SmartRide!");

                // Check if the driver is logged in
                if (Driver != null)
                {
                    Console.WriteLine($"Hello, {Driver.Name}! Ready to get started?");
                }
                else
                {
                    Console.WriteLine("Please log in. Press any key to continue.");
                    Console.ReadLine();
                    return; // Exit the menu if no driver is logged in
                }

                // Main menu with options
                Console.WriteLine("\nWhat would you like to do today? ");
                Console.WriteLine("1. Change your email.");
                Console.WriteLine("2. Change your License number.");
                Console.WriteLine("3. Have a look at your ratings.");
                Console.WriteLine("4. Check out your rides.");
                Console.WriteLine("5. Log out");
                string choice = Console.ReadLine()?.Trim() ?? "";
                
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Change email:");
                        this.emailchange(Driver);
                        break;

                    case "2":
                        Console.WriteLine("Change your license number: ");
                        this.change_license_number(Driver);
                        break;

                    case "3":
                        Console.WriteLine("Your Ratings: ");
                        this.Ratings();
                        break;

                    case "4":
                        Console.WriteLine("Rides:");
                        Driver = null;
                        break;

                    case "5":
                        Console.WriteLine("Logging out... Stay Safe!");
                        Driver = null;
                        return;
                }
            }
        }

        private void emailchange(DriverDto Driver)

        {
            Console.WriteLine("Enter new email:");
            string newEmail = Console.ReadLine()?.Trim().ToUpper();  // Use ToLower to avoid casing issues.

            // Validate the new email
            if (string.IsNullOrEmpty(newEmail))
            {
                Console.WriteLine("Invalid email entered.");
                return;
            }

            // Ensure the email doesn't already exist (optional validation)

            if (_driverService.GetDriver(newEmail) != null)
            {
                Console.WriteLine("Email already in use.");
                return;
            }

            // Update the driver's email in the database
            _driverService.UpdateDriverEmail(Driver.Email, newEmail, _context);

            // Fetch the updated driver to display the changes
            var updatedDriver = _driverService.GetDriver(newEmail);

            if (updatedDriver == null)
            {
                Console.WriteLine("Error: Unable to update driver.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---------------------");
            Console.WriteLine($"ID: {updatedDriver.Id}");
            Console.WriteLine($"Name: {updatedDriver.Name}");
            Console.WriteLine($"Email: {updatedDriver.Email}");
            Console.WriteLine($"License Number: {updatedDriver.LicenseNumber}");
            Console.WriteLine("---------------------");
            Console.ResetColor();
            Prompt.PressKeyToContinue();

        }

        private void change_license_number(DriverDto Driver)
        {
            DriverDto d = _driverService.GetDriver(Driver.Email);

            if (d == null)
            {
                Console.WriteLine("Driver not found.");
                return;
            }

            // Prompt the user to enter a new license number
            Console.WriteLine("Enter new license number:");
            string newLicenseNumber = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(newLicenseNumber))
            {
                Console.WriteLine("Invalid license number entered.");
                return;
            }

            // Update the driver's license number
            d.LicenseNumber = newLicenseNumber;

            // Save the changes to the database
            _driverService.UpdateDriver(_context, d);

            // Fetch the updated driver to display the changes
            var updatedDriver = _driverService.GetDriver(d.Email);

            if (updatedDriver == null)
            {
                Console.WriteLine("Error: Unable to update driver's license number.");
                return;
            }

            // Display the updated driver details
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---------------------");
            Console.WriteLine($"ID: {updatedDriver.Id}");
            Console.WriteLine($"Name: {updatedDriver.Name}");
            Console.WriteLine($"Email: {updatedDriver.Email}");
            Console.WriteLine($"License Number: {updatedDriver.LicenseNumber}");
            Console.WriteLine("---------------------");
            Console.ResetColor();
            Prompt.PressKeyToContinue();
        }


        private void Ratings()
        {
            var rating = new DriverRatingService(_context).GetDriverRating(Driver.Id);
            if (rating != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n---------------------");
                Console.WriteLine("Here’s your current rating!");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Average Rating: {rating.AverageRating} ({rating.RatingCount} ratings)");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nCustomer Comments:");
                Prompt.PressKeyToContinue();
                Console.ResetColor();
                if(rating.Comment != null)
                {
                    
                }
                else
                {
                    Console.WriteLine("No comments yet.");
                }
                if (rating == null)
                {
                    Console.WriteLine("No ratings yet.");
                    return;
                }
            }
        }

        private void Rides()
        {
            Console.WriteLine("Rides: ");

        }
    }
}

