using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Web.Script.Serialization;

namespace GoogleBookApi.Test
{
    public class GoogleBookApiExploration
    {
        private string isbn = "9780439785969";//Harry Potter and the Half-Blood Prince (Book 6) ISBN
        public string execute(string isbn)
        {
            return new RawCommunicationWithApi().Google_Book_Api_exploration(isbn);

        }
        [Fact]
        public void return_response()
        {
            string results=execute(isbn);
            Assert.NotEmpty(results);
        }
        [Fact]
        public void returns_json_response()
        {
            string results = execute(isbn);
            var json = Record.Exception(() => Deserialize_Json(results));
            Assert.Null(json);

        }
        [Fact]
        public void returns_HarryPotterISBN_from_other_ISBNs()
        {
            string json = execute(isbn);
            dynamic search_results = Deserialize_Json(json);
            Assert.Equal(1, search_results["totalItems"]); //unique ISBN so its only 1 result
        }
        [Fact]
        public void returns_if_book_exists_and_check_id_of_book_when_ISBN_is_given()
        {
            string json = execute(isbn);
            dynamic deserialize = Deserialize_Json(json);
            dynamic[] items = deserialize["items"];
            Assert.True(items.Length>0);
            dynamic id = items.SingleOrDefault(x => x["id"] =="9pB5BgAAQBAJ");
           
        }
        //testing with other class json convert to c# objects
        [Fact]
        public void returns_details_of_HP_half_blood()
        {
            
            string json = execute(isbn);
            dynamic deserialize = new JavaScriptSerializer().Deserialize<HPHalBlood.RootObject>(json);
            HPHalBlood.RootObject obj = deserialize;
            Assert.Equal("books#volumes", obj.kind);
            Assert.Equal("Rowling, J.K.", obj.items[0].volumeInfo.authors[0]);
            Assert.Equal("Harry Potter and the Half-Blood Prince (Book 6)", obj.items[0].volumeInfo.title);
            Assert.Equal(652, obj.items[0].volumeInfo.pageCount);
            Assert.Equal("BOOK", obj.items[0].volumeInfo.printType);
        }

        private static dynamic Deserialize_Json(string results)
        {
            return new JavaScriptSerializer().Deserialize<dynamic>(results);
        }
    }
}
