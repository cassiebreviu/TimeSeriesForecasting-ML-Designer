using System;

namespace IgniteAimlDataApp.Model
{
    public class Rdpi
    {
        public DateTime time { get; set; }
        public decimal rdpi { get; set; }

        public static Rdpi FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Rdpi rdpi = new Rdpi();
            rdpi.time = Convert.ToDateTime(values[0]);
            rdpi.rdpi = Convert.ToDecimal(values[1]);
            return rdpi;
        }
    }
}
