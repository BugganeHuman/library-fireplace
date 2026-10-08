namespace LibraryFireplace;

using Microsoft.Data.Sqlite;
public static class BookRepository
{
    public static void AddBook (Book book)
    {
        using var connection = DbContext.OpenConnection();
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
        using var connection = DbContext.OpenConnection();
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

    public static List<BookDTO> FindBooks (int? id, string? name, string? author,
                  int? yearOfPublication, string? languageOfBookText, string? genre)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT * FROM books WHERE
            id = COALESCE($id, id)
            AND LOWER(name) = COALESCE(LOWER($name), LOWER(name))
            AND LOWER(author) = COALESCE(LOWER($author), LOWER(author))
            AND year_of_publication = COALESCE($year_of_publication, year_of_publication)
            AND LOWER(language_of_book_text) = COALESCE(LOWER($language_of_book_text),
                                                        LOWER(language_of_book_text))
            AND LOWER(genre) = COALESCE(LOWER($genre), LOWER(genre))
            """;
        cmd.Parameters.AddWithValue("$id", (object?)id ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$name", (object?)name ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$author", (object?)author ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$year_of_publication",
                                     (object?)yearOfPublication ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$language_of_book_text",
                                    (object?)languageOfBookText ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$genre", (object?)genre ?? DBNull.Value);
        using var reader = cmd.ExecuteReader();
        List<BookDTO> found_books = [];
        while (reader.Read())
        {
            found_books.Add(new BookDTO(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    reader.GetString(5),
                    reader.GetString(6)
                ));
        }
        return found_books;
    }

    public static int UpdateBook(int id, string? name = null, string? author = null,
                                 int? yearOfPublication = null,
                                 string? languageOfBookText = null, string? genre = null)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            UPDATE books set
            name = COALESCE($name, name),
            author = COALESCE($author, author),
            year_of_publication = COALESCE($year_of_publication,
                                  year_of_publication),
            language_of_book_text = COALESCE($language_of_book_text,
                                    language_of_book_text),
            genre = COALESCE($genre, genre)
            WHERE id = $id
            """;
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$name", (object?)name ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$author", (object?)author ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$year_of_publication",
                                     (object?)yearOfPublication ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$language_of_book_text",
                                    (object?)languageOfBookText ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$genre", (object?)genre ?? DBNull.Value);
        int rowsUpdated = cmd.ExecuteNonQuery();
        return rowsUpdated;
    }

    public static bool DeleteBook(int id)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            DELETE FROM books WHERE id = $id
            """;
        cmd.Parameters.AddWithValue("$id", id);
        int deletedBooks = cmd.ExecuteNonQuery();
        bool isDeleted = deletedBooks > 0;
        return isDeleted;
    }


}

