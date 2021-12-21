using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CSVAvaility.Interfaces;
using CsvHelper;
using System.Globalization;
using CSVAvaility.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSVAvaility.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CSVAvailityController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok("Success!");
        }

        // POST api/values
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post()
        {
            var file = Request.Form.Files[0];

            if (file.ContentType != "text/csv")
            {
                return BadRequest("Please upload a csv file");
            }

            if (file.Length > 0)
            {
                using (var reader = new System.IO.StreamReader(file.OpenReadStream()))
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<EnrolleesClassMap>();
                        var records = csvReader.GetRecords<Enrollee>().ToList();
                        var insuredEnrollees = CSVAvailityService.ParseCSV(records);
                        var zip = CSVAvailityService.ConvertToCSVs(insuredEnrollees);
                        return File(zip.ToArray(), "application/zip");
                    }
                }
            }
            else
            {
                return BadRequest("File is empty");
            }
        }
    }
}
