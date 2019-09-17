using IgniteAimlDataApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IgniteAimlDataApp.DataLogic
{
    // Get data from local csv datasets.
    public static class DataAccess
    {
        public static List<ForecastingData> GetForecastingDataFromLocal()
        {
            string sourceFile = $"{Environment.CurrentDirectory}\\Datasets\\ForecastingData.csv";
            return File.ReadAllLines(sourceFile)
                                           .Skip(1)
                                           .Select(v => ForecastingData.FromCsv(v))
                                           .ToList();
        }

        public static List<Rdpi> GetRdpiDataFromLocal()
        {
            string sourceFile = $"{Environment.CurrentDirectory}\\Datasets\\RdpiData.csv";
            return File.ReadAllLines(sourceFile)
                    .Skip(1)
                    .Select(v => Rdpi.FromCsv(v))
                    .ToList();
        }
    }
}
