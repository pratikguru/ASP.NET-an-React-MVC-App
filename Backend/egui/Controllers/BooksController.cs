using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using egui.Models;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace egui.Controllers
{

    [Route("api/[controller]")]
    public class BooksController : Controller
    {

        // GET: api/books
        [HttpGet]
        public IEnumerable<Book> Get()
        { 
            dbHandler db = new dbHandler();
            var output = db.LoadData();
            List<Book> bk = new List<Book>();
            while (output.Read())
            {
                bk.Add(new Book { Id = Convert.ToInt32(output["Id"]), Title = (output["Title"].ToString()), Author = (output["Author"].ToString()), Year = Convert.ToInt32(output["Year"])});
            }
            return bk;
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            dbHandler db = new dbHandler();
            return db.FindBook(id).ToString();
        }

        // POST api/books
        [HttpPost]
        public string Post([FromQuery]string author, string title, int year)
        {
            Book book = new Book() { Title = title, Author = author, Year = year };
            dbHandler db = new dbHandler();
            db.insertBook(book);
            return book.ToString();
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public string Put(int id, [FromQuery]string author, string title, int year)
        {
            Book book = new Book() { Title = title, Author = author, Year = year };
            dbHandler db = new dbHandler();
            db.EditBook(id, author, title, year);
            return book.ToString();
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            dbHandler db = new dbHandler();
            db.DeleteRecord(id);
        }

        [HttpPost("Filter")]
        public IEnumerable<Book> Filter([FromQuery]string author = "", string title = "", int year = 0)
        {
            dbHandler db = new dbHandler();
            List<Book> bk = new List<Book>();

            if (year == 0)
                bk = db.loadFilters($"SELECT * FROM BOOK where LOWER(BOOK.Author) LIKE '%{author}%' AND LOWER(BOOK.Title) LIKE '%{title}%' ");
            else
                bk = db.loadFilters($"SELECT * FROM BOOK where LOWER(BOOK.Author) LIKE '%{author}%' AND LOWER(BOOK.Title) LIKE '%{title}%' AND Year='{year}'");

            /*
            string sql = "";

            if (title != null && author != null && (year) > 0)
            {
                Console.WriteLine("Filtering YEAR, AUTHOR AND TITLE");
                sql = $"SELECT rowid, *FROM BOOK WHERE AUTHOR ='{author}' AND YEAR ={year} AND TITLE='{title}'";
            }
            else if (title != null && author != null)
            {
                Console.WriteLine("Filtering TITLE AND AUTHOR");
                sql = $"SELECT rowid,* FROM BOOK WHERE AUTHOR='{author}' and TITLE='{title}'";
            }
            else if (author != null && year > 0)
            {
                Console.WriteLine("Filtering AUTHOR and YEAR.");
                sql = $"SELECT rowid,* FROM BOOK WHERE AUTHOR='{author}' and YEAR={year}";
            }
            else if (title != null && year > 0)
            {
                Console.WriteLine("Filtering TITLE and YEAR");
                sql = $"SELECT rowid,* FROM BOOK WHERE TITLE='{title}' and YEAR={year}";
            }
            else if (title != null)
            {
                sql = $"SELECT rowid,* FROM BOOK WHERE TITLE='{title}'";
            }
            else if (author != null)
            {
                Console.WriteLine("Filtering AUTHOR");
                sql = $"SELECT rowid,* FROM BOOK WHERE AUTHOR='{author}'";
            }
            else if (year > 0)
            {
                Console.WriteLine("Filtering YEAR");
                sql = $"SELECT rowid,* FROM BOOK WHERE YEAR={year}";
            }
            else
            {
                Console.WriteLine("Filtering not set.");
                
            }
            bk = db.loadFilters(sql);
            */
            return bk;
        }
    }
}
