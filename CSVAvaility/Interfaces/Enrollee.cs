using System;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace CSVAvaility.Interfaces
{
    public class Enrollee
    {
        public Enrollee() { }

        [Name("User Id")]
        public string UserID { get; set; }

        [Name("First Name")]
        public string FirstName { get; set; }

        [Name("Last Name")]
        public string LastName { get; set; }

        [Name("Version")]
        public int Version { get; set; }

        [Name("Insurance Company")]
        public string InsuranceCompany { get; set; }
    }

    public class EnrolleesClassMap : ClassMap<Enrollee>
    {
        public EnrolleesClassMap()
        {
            Map(m => m.UserID).Name("User Id");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Version).Name("Version");
            Map(m => m.InsuranceCompany).Name("Insurance Company");
        }
    }
}
