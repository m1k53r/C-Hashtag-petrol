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
        public Petrol P95 = new (6, 45, 4.99);
        public Petrol LPG = new (9, 30, 2.29);
        public int totalLPGDays = 0;
        public DateTime? foundDay;
        public class LPGDays
        {
            public List<double> Before = new ();
            public List<double> After = new ();
        }
        LPGDays lpgDays = new ();
        public double installationCost = 1600;
        public double savedInstallationCost = 1600;
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
                LPG.totalUsedLiters += (LPG.burning * km) / 100;
                totalLPGDays++;
            }
            else
            {
                LPG.remainingSpace -= (LPG.burning * km / 2) / 100;
                LPG.totalUsedLiters += (LPG.burning * km / 2) / 100;
                P95.remainingSpace -= (P95.burning * km / 2) / 100;
                P95.totalUsedLiters += (P95.burning * km / 2) / 100;
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
            Console.WriteLine("Pick a solution: ");
            Console.WriteLine("[ 1 ] Times tanked for each petrol type");
            Console.WriteLine("[ 2 ] Total LPG days");
            Console.WriteLine("[ 3 ] First day of LPG being <5.25");
            Console.WriteLine("[ 4 ] Used LPG on certain day");
            Console.WriteLine("[ 5 ] LPG and P95 cost");

            var input = Console.ReadLine();
            Int32.TryParse(input, out int parsedInput);

            Console.WriteLine(PickSolution(parsedInput));
        }
        public decimal RoundTwo(double number) => Math.Ceiling(100 * (decimal)number) / 100;
        public string PickSolution(int input) =>
            input switch
            {
                1 => $"P95: {P95.timesTanked}\nLPG: {LPG.timesTanked}",
                2 => $"Total LPG days: {totalLPGDays}",
                3 => $"Found day: {foundDay}",
                4 => "JSON file saved to ../../../../palwio.Shared/Resources/data.json",
                5 => $"LPG and installation: {RoundTwo(LPG.CalculatePrice() + installationCost)}\nP95: {RoundTwo(P95.CalculatePrice())}",
                _ => "Solution not found"
            };
        public void SetPrices(double p95Price, double lpgPrice, double installationCost)
        {
            P95.price = p95Price;
            LPG.price = lpgPrice;
            this.installationCost = installationCost;
        }
        public string GetUsedLPGOnDays()
        {
            string text = "";

            for(int i = 0; i < lpgDays.Before.Count; ++i)
            {
                text += $"Day {i + 1}, before: {RoundTwo(lpgDays.Before[i])}\n";
                text += $"Day {i + 1}, after: {RoundTwo(lpgDays.After[i])}\n\n";
            }

            return text;
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
            lpgDays.Before.Add(LPG.remainingSpace);
            func();
            newData.After = LPG.remainingSpace;
            lpgDays.After.Add(LPG.remainingSpace);
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