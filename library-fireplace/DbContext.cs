namespace LibraryFireplace;

using Microsoft.Data.Sqlite;
public static class DbContext
{
    public static SqliteConnection OpenConnection()
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
                genre TEXT,
                is_lent BOOL DEFAULT FALSE,
                borrower INTEGER DEFAULT NULL,
                due_date TEXT DEFAULT NULL,
                FOREIGN KEY (borrower) REFERENCES readers(id)
            );

            CREATE TABLE IF NOT EXISTS readers (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                first_name TEXT,
                last_name TEXT,
                phone_number INTEGER,
                citizenship TEXT,
                personal_id INTEGER
            );
            """;
        cmd.ExecuteNonQuery();
    }
}
