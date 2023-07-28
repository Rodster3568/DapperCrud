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
        private readonly SqlConnection _connection;

        public ContactBookController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactBook>>> GetAllContactBooks()
        {
            IEnumerable<ContactBook> contacts = await SelectAllContacts();
            return Ok(contacts);
        }

        [HttpGet("{contactId}")]
        public async Task<ActionResult<List<ContactBook>>> GetContact(int contactId)
        {
            var contact = await _connection.QueryFirstAsync<ContactBook>("select * from ContactBooks where id=@Id",
                new { Id = contactId });
            return Ok(contact);
        }
        [HttpPost]
        public async Task<ActionResult<List<ContactBook>>> CreateContact(ContactBook contact)
        {
            await _connection.ExecuteAsync("insert into ContactBooks (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", contact);
            return Ok(await SelectAllContacts());
        }
        [HttpPut]
        public async Task<ActionResult<List<ContactBook>>> UpdateContact(ContactBook contact)
        {
            await _connection.ExecuteAsync("update ContactBooks set name = @Name, " +
                "firstname = @FirstName, lastname=@LastName, place=@Place where id=@Id", contact);
            return Ok(await SelectAllContacts());
        }
        [HttpDelete("{contactId}")]
        public async Task<ActionResult<List<ContactBook>>> DeleteContact(int contactId)
        {
            await _connection.ExecuteAsync("delete from ContactBooks where id= @Id", new {Id= contactId});
            return Ok(await SelectAllContacts());
        }
        private async Task<IEnumerable<ContactBook>> SelectAllContacts()
        {
            return await _connection.QueryAsync<ContactBook>("select * from ContactBooks");
        }
    }
}