using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ShortenURL_API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ShortenURLController : ApiController
    {
        readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        public string GetAll()
        {
            return "Hello";
        }

        public string GetShortenURL(string OriginalURL)
        {
            return DoProcess(OriginalURL);
        }

        private string DoProcess(string OriginalURL)
        {
            ValidateURL(OriginalURL);

            string shortURL = FindExistingURL(OriginalURL);

            if (shortURL == String.Empty)
            {
                shortURL = ConvertURL(OriginalURL);
            }

            return shortURL;
        }

        private void ValidateURL(string OriginalURL)
        {
            bool result = Uri.IsWellFormedUriString(OriginalURL, UriKind.Absolute);

            if (!result)
            {
                throw new Exception("Invalid URL");
            }
        }

        private string FindExistingURL(string OriginalURL)
        {
            string shortURL = String.Empty;

            SqlCommand query = new SqlCommand(String.Format("SELECT ShortenURL FROM URL WHERE OriginalURL = '{0}'", OriginalURL), conn);
            conn.Open();
            object result = query.ExecuteScalar();
            conn.Close();


            if (result == null)
            {
                shortURL = String.Empty;
            }
            else
            {
                shortURL = result.ToString();
            }

            return shortURL;
        }

        private string ConvertURL(string OriginalURL)
        {
            string shortURL = "https://shortenURL/";
            StringBuilder str = new StringBuilder();
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char newLetter;

            for (int i = 0; i < 10; i++)
            {
                newLetter = chars[random.Next(chars.Length)];
                str.Append(newLetter);
            }

            shortURL = String.Concat(shortURL, str);

            if (IsExistShortURL(shortURL))
            {
                ConvertURL(OriginalURL);
            }

            SaveNewURL(OriginalURL, shortURL);

            return shortURL;
        }

        private bool IsExistShortURL(string shortURL)
        {
            bool isExist = true;

            SqlCommand query = new SqlCommand(String.Format("SELECT ShortenURL FROM URL WHERE ShortenURL = '{0}'", shortURL), conn);
            conn.Open();
            object result = query.ExecuteScalar();
            conn.Close();

            if (result == null)
            {
                isExist = false;
            }

            return isExist;
        }

        private void SaveNewURL(string OriginalURL, string ShortenURL)
        {
            StringBuilder sqlQueryBuilder = new StringBuilder();

            sqlQueryBuilder.Append("INSERT INTO URL ");
            sqlQueryBuilder.Append("(OriginalURL, ShortenURL) values ");
            sqlQueryBuilder.Append("('{0}', '{1}')");

            SqlCommand query = new SqlCommand(String.Format(sqlQueryBuilder.ToString(), OriginalURL, ShortenURL), conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
        }
    }
}
