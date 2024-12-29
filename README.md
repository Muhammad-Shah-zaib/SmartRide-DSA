# SmartRide - Ride Hailing Service

Welcome to the **SmartRide** repository! This project simulates a ride-hailing service where users can book rides, drivers accept requests, and the system efficiently manages operations to ensure minimal wait times and optimized routes.

## **Overview**
The SmartRide project is designed to implement the core functionalities of a ride-hailing platform. It focuses on designing and implementing custom data structures to manage operations efficiently. The repository contains all the code, documentation, and test cases required to simulate a functional ride-hailing system.

## Current File structure

``` md
SmartRide.ConsoleApp/
├── Program.cs         # Entry point of the application
├── Models             # DataBase context (via scaffold)
├── src/
│   |	├── CLI/				 # Contains CLI related Classes
│   │   ├── Dtos/                # Contains your data models (e.g., User, Driver, Ride)
│   │   ├── Services/            # Contains services for handling business logic
│   │   ├── DataStructures/      # Custom implementations of data structures
│   │   ├── Utilities/           # Helper utilities (e.g., algorithms for pathfinding)
│   │   ├── AppSettings.json     # Configuration file (optional)
│   ├── SmartRide.Tests/         # Unit test project
│       ├── UnitTests/           # Unit tests for each feature
├── docs/                        # Documentation folder
├── .gitignore                   # Ignored files/folders for version control
├── README.md                    # Project overview
```
