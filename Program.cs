using System;
using System.Collections.Generic;

class City
{
    public string Name { get; set; }
    public int OutbreakLevel { get; set; }
    public List<int> Contacts { get; set; }

    public City(string name)
    {
        Name = name;
        OutbreakLevel = 0;
        Contacts = new List<int>();
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter the number of cities: ");
        int numCities = int.Parse(Console.ReadLine());

        List<City> cities = new List<City>();

        for (int i = 0; i < numCities; i++)
        {
            Console.Write("City {0} name: ", i);
            string cityName = Console.ReadLine();

            City city = new City(cityName);

            Console.Write("Number of cities in contact with {0}: ", cityName);
            int numContacts = int.Parse(Console.ReadLine());

            for (int j = 0; j < numContacts; j++)
            {
                int contactCity;
                do
                {
                    Console.Write("City {0} contacts (enter city number): ", cityName);
                    contactCity = int.Parse(Console.ReadLine());
                } while (contactCity < 0 || contactCity >= numCities);

                city.Contacts.Add(contactCity);
            }

            cities.Add(city);
        }

        Console.WriteLine("\nCity\t\tCOVID-19 Outbreak Level");
        Console.WriteLine("-------------------------------");
        foreach (City city in cities)
        {
            Console.WriteLine("{0}\t\t{1}", city.Name, city.OutbreakLevel);
        }
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter the event (Outbreak, Vaccinate, Lockdown, Spread, or Exit): ");
            string eventType = Console.ReadLine();

            if (eventType.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                break;

            Console.Write("Enter the city number where the event occurred: ");
            int eventCity = int.Parse(Console.ReadLine());

            if (eventCity < 0 || eventCity >= numCities)
            {
                Console.WriteLine("Invalid city number. Please try again.\n");
                continue;
            }

            switch (eventType.ToLower())
            {
                case "outbreak":
                    ProcessOutbreakEvent(cities, eventCity);
                    break;
                case "vaccinate":
                    ProcessVaccinateEvent(cities, eventCity);
                    break;
                case "lockdown":
                    ProcessLockdownEvent(cities, eventCity);
                    break;
                case "spread":
                    ProcessSpreadEvent(cities);
                    break;
                default:
                    Console.WriteLine("Invalid event. Please try again.\n");
                    break;
            }

            Console.WriteLine("\nCity\t\tCOVID-19 Outbreak Level");
            Console.WriteLine("-------------------------------");
            foreach (City city in cities)
            {
                Console.WriteLine("{0}\t\t{1}", city.Name, city.OutbreakLevel);
            }
            Console.WriteLine();
        }
    }

    static void ProcessOutbreakEvent(List<City> cities, int eventCity)
    {
        cities[eventCity].OutbreakLevel += 2;

        foreach (int contactCity in cities[eventCity].Contacts)
        {
            if (cities[contactCity].OutbreakLevel < 3)
                cities[contactCity].OutbreakLevel += 1;
        }
    }

    static void ProcessVaccinateEvent(List<City> cities, int eventCity)
    {
        cities[eventCity].OutbreakLevel = 0;
    }
    static void ProcessLockdownEvent(List<City> cities, int eventCity)
    {
        if (cities[eventCity].OutbreakLevel > 0)
            cities[eventCity].OutbreakLevel -= 1;

        foreach (int contactCity in cities[eventCity].Contacts)
        {
            if (cities[contactCity].OutbreakLevel > 0)
                cities[contactCity].OutbreakLevel -= 1;
        }
    }

    static void ProcessSpreadEvent(List<City> cities)
    {
        foreach (City city in cities)
        {
            bool hasHigherOutbreak = false;
            foreach (int contactCity in city.Contacts)
            {
                if (cities[contactCity].OutbreakLevel > city.OutbreakLevel)
                {
                    hasHigherOutbreak = true;
                    break;
                }
            }

            if (hasHigherOutbreak)
                city.OutbreakLevel += 1;
        }
    }
}