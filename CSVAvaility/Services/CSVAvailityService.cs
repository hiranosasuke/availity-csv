using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using CSVAvaility.Interfaces;

namespace CSVAvaility.Services
{
    public static class CSVAvailityService
    {
        private static Dictionary<String, List<Enrollee>> diction;

        public static Dictionary<String, List<Enrollee>> ParseCSV(List<Enrollee> enrollees)
        {
            Dictionary<String, List<Enrollee>> result = new Dictionary<String, List<Enrollee>>();
            diction = new Dictionary<String, List<Enrollee>>();

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
                var sortedEnrollees = item.Value.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
                result[item.Key] = sortedEnrollees;
            }

            return result;
        }

        public static HttpResponseMessage WriteCSV(string filePath = "")
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("Hello, World!");
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }
    }
}
