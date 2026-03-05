using API_Project_dotnetcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Project_dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonAPI : ControllerBase
    {
        #region Global Declaration
        BussinessLogic bl = new BussinessLogic();
        EncryptionHelper EH = new EncryptionHelper();

        #endregion

        protected internal virtual JsonTextActionResult JsonText(string jsonText)
        {
            return new JsonTextActionResult(jsonText);
        }
        // GET: api/<CommonAPI>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CommonAPI>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CommonAPI>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CommonAPI>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommonAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("testdata")]
        public IActionResult GetProjectGroup()
        {
            var data = bl.GetDataTableJsonDataSql("usp_test", "", "connectionstring");
            return JsonText(data);
        }
        [HttpPost]
        [Route("CategoryMaster")]
        public IActionResult CategoryMaster(GetAll objdata)
        {
            var data = bl.GetDataTableJsonDataSql("usp_IUDCategory", objdata.FilterParameter.ToString(), "connectionstring");
            return JsonText(data);

        }

        [HttpPost]
        [Route("UserMaster")]
        public IActionResult UserMaster(GetAll objdata)
        {
            var data = bl.GetDataTableJsonDataSql("usp_IUDUserMaster", objdata.FilterParameter.ToString(), "connectionstring");
            return JsonText(data);

        }
        
        [HttpPost]
        [Route("Products")]
        public IActionResult Products(GetAll objdata)
        {
            var data = bl.GetDataTableJsonDataSql("usp_IUDProductMaster", objdata.FilterParameter.ToString(), "connectionstring");
            return JsonText(data);

        }
    }
}
