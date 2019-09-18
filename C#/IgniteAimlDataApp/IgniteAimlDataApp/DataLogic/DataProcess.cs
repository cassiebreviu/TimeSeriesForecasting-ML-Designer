using IgniteAimlDataApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace IgniteAimlDataApp.DataLogic
{
    public class DataProcess
    {
        public List<ForecastingData> GetProcessedDataForScore(int id1, int id2, int weeksToPredict)
        {
            // Load forecasting data and filter by ids needed for prediction.
            var forecastingData = DataAccess.GetForecastingDataFromLocal()
                                            .Where(item => item.ID1 == id1 && item.ID2 == id2)
                                            .ToList();
            // Load rdpi data.
            var rdpiData = DataAccess.GetRdpiDataFromLocal();

            // Add dates for weeks to predict.
            var latestDate = forecastingData.Max(data => data.Time);
            for (int i = 0; i < weeksToPredict; i++)
            {
                latestDate = latestDate.AddDays(7);
                var forcastingDataItem = new ForecastingData
                {
                    ID1 = id1,
                    ID2 = id2,
                    Time = latestDate,
                    Value = 0,
                    RDPI = rdpiData.Last().rdpi
                };
                forecastingData.Add(forcastingDataItem);
            }



            // Create Time Features
            forecastingData = CreateTimeFeatures(forecastingData, rdpiData);
            // Create Fourier Features
            forecastingData = CreateFourierFeatures(forecastingData);
            // Create Lag Features
            forecastingData = CreateLagFeatures(forecastingData);
            return forecastingData;

        }

        public List<ForecastingData> CreateFourierFeatures(List<ForecastingData> forecastData)
        {
            // Set seasonality to 52 which is the number of weeks in a year.

            var seasonality = 52;
            foreach (var item in forecastData)
            {
                item.FreqCos1 = Math.Cos(item.WeekOfYear * 2 * Math.PI * 1 / seasonality);
                item.FreqSin1 = Math.Sin(item.WeekOfYear * 2 * Math.PI * 1 / seasonality);

                item.FreqCos2 = Math.Cos(item.WeekOfYear * 2 * Math.PI * 2 / seasonality);
                item.FreqSin2 = Math.Sin(item.WeekOfYear * 2 * Math.PI * 2 / seasonality);

                item.FreqCos3 = Math.Cos(item.WeekOfYear * 2 * Math.PI * 3 / seasonality);
                item.FreqSin3 = Math.Sin(item.WeekOfYear * 2 * Math.PI * 3 / seasonality);

                item.FreqCos4 = Math.Cos(item.WeekOfYear * 2 * Math.PI * 4 / seasonality);
                item.FreqSin4 = Math.Sin(item.WeekOfYear * 2 * Math.PI * 4 / seasonality);

            }
            return forecastData;
        }

        public List<ForecastingData> CreateTimeFeatures(List<ForecastingData> forecastData, List<Rdpi> rdpiData)
        {
            var cultureInfo = new CultureInfo("en-US");
            var calendar = cultureInfo.Calendar;
            var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            foreach (var item in forecastData)
            {
                item.Year = item.Time.Year;
                item.Month = item.Time.Month;
                item.WeekOfMonth = Convert.ToInt32(Math.Ceiling(item.Time.Day / 7.0));
                item.WeekOfYear = calendar.GetWeekOfYear(item.Time, calendarWeekRule, firstDayOfWeek);

                // 4th Friday in November
                item.IsBlackFriday = item.DatesInWeek.Any(date => date.Month == 11 &&
                                                                 date.DayOfWeek == DayOfWeek.Friday
                                                                 && date.Day > 22
                                                                 && date.Day < 29);

                // 1st Monday in September
                item.IsUsLaborDay = item.DatesInWeek.Any(date => date.Month == 9
                                                         && date.DayOfWeek == DayOfWeek.Monday
                                                         && date.Day < 8);
                // 25th of December
                item.IsChristmasDay = item.DatesInWeek.Any(date => date.Month == 12 && date.Day == 25);
                // 1st of January
                item.IsUsNewYearsDay = item.DatesInWeek.Any(date => date.Month == 1 && date.Day == 1);
            }
            return forecastData;
        }

        public List<ForecastingData> CreateLagFeatures(List<ForecastingData> forecastData)
        {
            // Populate Lag features for previous 26 weeks.

            for (int i = forecastData.Count - 1; i > 26; i--)
            {
                forecastData[i].Lag1 = forecastData[i - 1].Value;
                forecastData[i].Lag2 = forecastData[i - 2].Value;
                forecastData[i].Lag3 = forecastData[i - 3].Value;
                forecastData[i].Lag4 = forecastData[i - 4].Value;
                forecastData[i].Lag5 = forecastData[i - 5].Value;
                forecastData[i].Lag6 = forecastData[i - 6].Value;
                forecastData[i].Lag7 = forecastData[i - 7].Value;
                forecastData[i].Lag8 = forecastData[i - 8].Value;
                forecastData[i].Lag9 = forecastData[i - 9].Value;
                forecastData[i].Lag10 = forecastData[i - 10].Value;
                forecastData[i].Lag11 = forecastData[i - 11].Value;
                forecastData[i].Lag12 = forecastData[i - 12].Value;
                forecastData[i].Lag13 = forecastData[i - 13].Value;
                forecastData[i].Lag14 = forecastData[i - 14].Value;
                forecastData[i].Lag15 = forecastData[i - 15].Value;
                forecastData[i].Lag16 = forecastData[i - 16].Value;
                forecastData[i].Lag17 = forecastData[i - 17].Value;
                forecastData[i].Lag18 = forecastData[i - 18].Value;
                forecastData[i].Lag19 = forecastData[i - 19].Value;
                forecastData[i].Lag20 = forecastData[i - 20].Value;
                forecastData[i].Lag21 = forecastData[i - 21].Value;
                forecastData[i].Lag22 = forecastData[i - 22].Value;
                forecastData[i].Lag23 = forecastData[i - 23].Value;
                forecastData[i].Lag24 = forecastData[i - 24].Value;
                forecastData[i].Lag25 = forecastData[i - 25].Value;
                forecastData[i].Lag26 = forecastData[i - 26].Value;
            }


            return forecastData;
        }
    }
}
