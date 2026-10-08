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
                    reader.GetString(6),
                    reader.GetBoolean(7),
                    reader.IsDBNull(8) ? null : reader.GetInt32(8),
                    reader.IsDBNull(9) ? null : reader.GetString(9) 
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
                    reader.GetString(6),
                    reader.GetBoolean(7),
                    reader.GetInt32(8),
                    reader.GetString(9)
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

    public static void LendBook(int bookID, int readerID)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            UPDATE books SET 
            is_lent = $is_lent,
            borrower = $borrower,
            due_date = $due_date
            WHERE id = $id
            """;
        cmd.Parameters.AddWithValue("$is_lent", true);
        cmd.Parameters.AddWithValue("$borrower", readerID);
        cmd.Parameters.AddWithValue("$id", bookID);
        cmd.Parameters.AddWithValue("$due_date", DateTime.Now.AddDays(21).ToString("dd.MM.yyyy"));
        cmd.ExecuteNonQuery();
    }


}

