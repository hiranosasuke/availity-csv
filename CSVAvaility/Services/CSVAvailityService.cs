using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CSVAvaility.Interfaces;
using CsvHelper;

namespace CSVAvaility.Services
{
    public static class CSVAvailityService
    {
        private static Dictionary<string, List<Enrollee>> diction;

        // Takes a list of all enrollees, then separates them into their own list of enrolles
        // by their insurance companies.
        public static Dictionary<string, List<Enrollee>> ParseCSV(List<Enrollee> enrollees)
        {
            Dictionary<string, List<Enrollee>> result = new Dictionary<string, List<Enrollee>>();
            diction = new Dictionary<string, List<Enrollee>>();

            foreach (var item in enrollees)
            {
                if (diction.ContainsKey(item.InsuranceCompany))
                {
                    var enrolleesInsured = diction[item.InsuranceCompany];

                    var user = enrolleesInsured.Find(x => x.UserID == item.UserID);

                    if (user != null)
                    {
                        if (user.Version < item.Version)
                        {
                            user.FirstName = item.FirstName;
                            user.LastName = item.LastName;
                            user.Version = item.Version;
                        }
                    }
                    else
                    {
                        enrolleesInsured.Add(item);
                    }
                }
                else
                {
                    List<Enrollee> newList = new List<Enrollee>();
                    newList.Add(item);
                    diction[item.InsuranceCompany] = newList;
                }
            }

            foreach (var item in diction)
            {
                var sortedEnrollees = item.Value;
                result[item.Key] = sortedEnrollees.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
            }

            return result;
        }

        public static MemoryStream ConvertToCSVs(Dictionary<string, List<Enrollee>> diction)
        {
            var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in diction)
                {
                    var demoFile = archive.CreateEntry(item.Key + ".csv");

                    using (var entryStream = demoFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(item.Value);
                        }
                    }
                }
            }

            return memoryStream;
        }
    }
}
