namespace SmartRide.src.Utilities.DefaultMap;

public static class GenerateDefaultMap
{
    public static void InitializeMap(SmartRideDbContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
        }

        var mapService = new MapService(context);

        // Add nodes
        AddNodes(mapService);
        // Add edges
        AddEdges(mapService);

        // Save changes to the database
        context.SaveChanges();
    }

    private static void AddNodes(MapService mapService)
    {
        mapService.AddNode("Ghazali", "Hostel");
        mapService.AddNode("Rahmat", "Hostel");
        mapService.AddNode("Zakriya", "Hostel");
        mapService.AddNode("NICE", "Department/Ground");
        mapService.AddNode("Fatima", "Hostel");
        mapService.AddNode("Gate-4", "Town-Gate");
        mapService.AddNode("Bolan Chowk", "Chowk");
        mapService.AddNode("C-2", "Cafeteria");
        mapService.AddNode("Snakesian", "Department/Parking");
        mapService.AddNode("Supreme Court", "Main office");
        mapService.AddNode("Masjid", "Mosque");
        mapService.AddNode("Wadera Chowk", "Chowk");
        mapService.AddNode("Gate-2", "Town-Gate");
        mapService.AddNode("Iqbal Circle", "Chowk");
        mapService.AddNode("C-1", "Cafeteria");
        mapService.AddNode("Ug-Girls Hostels", "Hostels");
        mapService.AddNode("Gate-1", "Town-Gate");
    }

    private static void AddEdges(MapService mapService)
    {
        // Edges for Ghazali
        mapService.AddEdge("Ghazali", "Rahmat", 2.0, isOneWay: false);

        // Edges for Zakriya
        mapService.AddEdge("Zakriya", "Rahmat", 1.0, isOneWay: false);
        mapService.AddEdge("Zakriya", "NICE", 1.0, isOneWay: false);

        // Edges for Rahmat
        mapService.AddEdge("Rahmat", "Fatima", 1.0, isOneWay: false);

        // Edges for NICE
        mapService.AddEdge("NICE", "Gate-4", 3.0, isOneWay: false);
        mapService.AddEdge("NICE", "Bolan Chowk", 1.0, isOneWay: false);

        // Edges for Fatima
        mapService.AddEdge("Fatima", "Bolan Chowk", 1.0, isOneWay: false);

        // Edges for Gate-4
        mapService.AddEdge("Gate-4", "Wadera Chowk", 8.0, isOneWay: false);

        // Edges for Bolan Chowk
        mapService.AddEdge("Bolan Chowk", "C-2", 2.0, isOneWay: false);
        mapService.AddEdge("Bolan Chowk", "Iqbal Circle", 5.0, isOneWay: false);
        mapService.AddEdge("Bolan Chowk", "Masjid", 3.0, isOneWay: true);

        // Edges for C-2
        mapService.AddEdge("C-2", "Snakesian", 2.0, isOneWay: false);
        mapService.AddEdge("C-2", "Supreme Court", 3.0, isOneWay: false);

        // Edges for Supreme Court
        mapService.AddEdge("Supreme Court", "Snakesian", 1.0, isOneWay: true);
        mapService.AddEdge("Supreme Court", "Ug-Girls Hostels", 3.0, isOneWay: false);

        // Edges for Masjid
        mapService.AddEdge("Masjid", "Snakesian", 2.0, isOneWay: true);

        // Edges for Wadera Chowk
        mapService.AddEdge("Wadera Chowk", "Gate-2", 1.0, isOneWay: false);

        // Edges for Gate-2
        mapService.AddEdge("Gate-2", "Iqbal Circle", 5.0, isOneWay: false);

        // Edges for Iqbal Circle
        mapService.AddEdge("Iqbal Circle", "Masjid", 2.0, isOneWay: true);
        mapService.AddEdge("Iqbal Circle", "C-1", 3.0, isOneWay: false);
        mapService.AddEdge("Iqbal Circle", "Gate-1", 5.0, isOneWay: false);

        // Edges for C-1
        mapService.AddEdge("C-1", "Ug-Girls Hostels", 1.0, isOneWay: false);
    }
}
