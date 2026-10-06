namespace LibraryFireplace;

using Microsoft.Data.Sqlite;
public static class BookRepository
{
    private static SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection("Data Source=library_fireplace_db.db");
        connection.Open();
        return connection;
    }
    public static void Start()
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS books (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                name TEXT,
                author TEXT,
                year_of_publication INTEGER,
                language_of_book_text TEXT,
                genre TEXT
            );
            """;
        cmd.ExecuteNonQuery();
    }

    public static void AddBook (Book book)
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            INSERT INTO books 
            (name, author, year_of_publication, language_of_book_text, genre)
            VALUES 
            ($name, $author, $year_of_publication, $language_of_book_text, $genre)

            """;
        cmd.Parameters.AddWithValue("$name", book.Name);
        cmd.Parameters.AddWithValue("$author", book.Author);
        cmd.Parameters.AddWithValue("$year_of_publication", book.YearOfPublication);
        cmd.Parameters.AddWithValue("$language_of_book_text", book.LanguageOfBookText);
        cmd.Parameters.AddWithValue("$genre", book.Genre);
        cmd.ExecuteNonQuery();
    }

    public static List<BookDTO> GetAllBooks ()
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT * FROM books
            """;
        using var reader = cmd.ExecuteReader();
        List<BookDTO> books = [];
        while (reader.Read())
        {
            books.Add(new BookDTO(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    reader.GetString(5),
                    reader.GetString(6)
                    ));
        }
        return books;
    }



}
