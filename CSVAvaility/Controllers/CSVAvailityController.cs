using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSVAvaility.Interfaces;
using CsvHelper;
using System.Globalization;
using CSVAvaility.Services;
using System.Net.Http;

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
            return View();
        }

        // POST api/values
        [HttpPost, DisableRequestSizeLimit]
        public HttpResponseMessage Post([FromForm] IFormFile filee)
        {
            var file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                using (var reader = new System.IO.StreamReader(file.OpenReadStream()))
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<EnrolleesClassMap>();
                        var records = csvReader.GetRecords<Enrollee>().ToList();
                        var asd = CSVAvailityService.ParseCSV(records);
                    }
                }


                var qwe = CSVAvailityService.WriteCSV();
                return qwe;

                //return Ok();
            }
            else
            {
                return new HttpResponseMessage();
            }
        }
    }
}
