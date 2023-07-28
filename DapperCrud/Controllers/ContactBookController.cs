using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactBookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ContactBookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<List<ContactBook>>> GetAllContactBooks()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var contacts = await connection.QueryAsync<ContactBook>("select * from ContactBooks");
            return Ok(contacts);
        }
    } 
}
