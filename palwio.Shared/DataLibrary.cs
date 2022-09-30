using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.Text.Json;

namespace palwio.Shared
{
    public class DataLibrary
    {
        IEnumerable<InputData>? records;
        public Petrol P95 = new Petrol(6, 45);
        public Petrol LPG = new Petrol(9, 30);
        public int totalLPGDays = 0;
        public DateTime? foundDay;
        public void Read()
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t"
            };

            var reader = new StreamReader("../../../../palwio.Shared/Resources/lpg.txt");
            var csv = new CsvReader(reader, configuration);
            records = csv.GetRecords<InputData>();
        }
        public void BurnLiters(int km)
        {
            if (LPG.remainingSpace > 15)
            {
                LPG.remainingSpace -= (LPG.burning * km) / 100;
                totalLPGDays++;
            }
            else
            {
                LPG.remainingSpace -= (LPG.burning * km / 2) / 100;
                P95.remainingSpace -= (P95.burning * km / 2) / 100;
            }
        }
        public void TankPetrol(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Thursday && P95.remainingSpace < 40)
            {
                P95.remainingSpace = P95.totalSpace;
                P95.timesTanked++;
            }
            if (LPG.remainingSpace < 5)
            {
                LPG.remainingSpace = LPG.totalSpace;
                LPG.timesTanked++;
            }
        }
        public void PrintSolution()
        {
            Console.WriteLine($"P95: {P95.timesTanked}");
            Console.WriteLine($"LPG: {LPG.timesTanked}");
            Console.WriteLine($"Total LPG days: {totalLPGDays}");
            Console.WriteLine($"Found day: {foundDay}");
        }
        public void FindDay(DateTime data)
        {
            if (foundDay == null && LPG.remainingSpace <= 5.25)
            {
                foundDay = data;
            }
        }
        public JsonData SaveData(Action func, DateTime data)
        {
            JsonData newData = new JsonData()
            {
                Data = data,
                Before = LPG.remainingSpace,
            };
            func();
            newData.After = LPG.remainingSpace;
            return newData;
        }
        public void WriteToFile(List<JsonData> json)
        {
            var data = JsonSerializer.Serialize(json);
            File.WriteAllText("../../../../palwio.Shared/Resources/data.json", data);
        }
        public void Drive()
        {
            List<JsonData> json = new List<JsonData>();
            foreach (var r in records)
            {
                var data = SaveData(() =>
                {
                    FindDay(r.data);
                    BurnLiters(r.km);
                    TankPetrol(r.data);
                }, r.data);
                json.Add(data);
            }
            PrintSolution();
            WriteToFile(json);
        }
    }
    public class InputData
    {
        public DateTime data { get; set; }
        public int km { get; set; }
    }
    public class JsonData
    {
        public DateTime Data { get; set; }
        public double Before { get; set; }
        public double After { get; set; }
    }
}