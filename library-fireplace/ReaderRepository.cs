namespace LibraryFireplace;

using Microsoft.Data.Sqlite;

public static class ReaderRepository
{
    public static void AddReader (Reader newReader)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            INSERT INTO readers
            (first_name, last_name, phone_number, citizenship, personal_id)
            VALUES
            ($first_name, $last_name, $phone_number, $citizenship, $personal_id)
            """;
        cmd.Parameters.AddWithValue("$first_name", newReader.FirstName);
        cmd.Parameters.AddWithValue("$last_name", newReader.LastName);
        cmd.Parameters.AddWithValue("$phone_number", newReader.PhoneNumber);
        cmd.Parameters.AddWithValue("$citizenship", newReader.Citizenship);
        cmd.Parameters.AddWithValue("$personal_id", newReader.PersonalID);
        cmd.ExecuteNonQuery();
    }

    public static List<ReaderDTO> GetAllReaders ()
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT * FROM readers
            """;
        using var sqliteReader = cmd.ExecuteReader();
        List<ReaderDTO> allReaders = [];
        while (sqliteReader.Read())
        {
            allReaders.Add(new ReaderDTO(
                sqliteReader.GetInt32(0),
                sqliteReader.GetString(1),
                sqliteReader.GetString(2),
                sqliteReader.GetString(3),
                sqliteReader.GetInt64(4),
                sqliteReader.GetString(5),
                sqliteReader.GetInt64(6)
                ));
        }
        return allReaders;
    }
}
