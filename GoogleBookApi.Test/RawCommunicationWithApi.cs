using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBookApi.Test
{
    public class RawCommunicationWithApi
    {
        public string Google_Book_Api_exploration(string isbn)
        {
            return GetHttp(isbn);
        }

        private static string GetHttp(string isbn)
        {
            string url = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + isbn;
            return new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
