using System.Data;
using Microsoft.Data.Sqlite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new List<CheepViewModel>();

    public List<CheepViewModel> GetCheeps()
    {
        string value = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        string tempPath;

        if (value == "")
        {
            tempPath = Path.GetTempPath();
            string tmp = Path.Combine(tempPath, "chirp.db");
            Environment.SetEnvironmentVariable("CHIRPDBPATH", tmp);
            value = Environment.GetEnvironmentVariable("CHIRPDBPATH");
            Console.WriteLine(value);
            File.Move("/data/chirp.db", value);
        }

        var sqlQuery = 
        @"SELECT M.text, U.username, M.pub_date FROM message M
        JOIN user U ON M.author_id = U.user_id";
        using (var connection = new SqliteConnection($"Data Source={value}"))
        {   
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read()){
                string message = reader[0].ToString();
                string author = reader[1].ToString();
                long timestamp = Convert.ToInt64(reader[2]);
               CheepViewModel someCheep = new CheepViewModel(author,message,UnixTimeStampToDateTimeString(timestamp));
                _cheeps.Add(someCheep); 
            }
        }
        return _cheeps;
    }
    
    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
