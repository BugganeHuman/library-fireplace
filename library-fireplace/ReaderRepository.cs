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

    public static List<ReaderDTO> FindReaders (int? id, string? firstName,
                                                string? lastName, long? phoneNumber,
                                                string? citizenship, long? personalID)
    {
        using var connection = DbContext.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT * FROM readers WHERE
            id = COALESCE ($id, id)
            AND LOWER(first_name) = COALESCE (LOWER($first_name), LOWER(first_name))
            AND LOWER(last_name) = COALESCE (LOWER($last_name), LOWER(last_name))
            AND phone_number = COALESCE($phone_number, phone_number)
            AND LOWER(citizenship) = COALESCE (LOWER($citizenship), LOWER(citizenship))
            AND personal_id = COALESCE ($personal_id, personal_id)
            """;
        cmd.Parameters.AddWithValue("$id", (object?)id ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$first_name", (object?)firstName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$last_name", (object?)lastName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$phone_number", (object?)phoneNumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$citizenship", (object?)citizenship ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$personal_id", (object?)personalID ?? DBNull.Value);
        using var reader = cmd.ExecuteReader();
        List<ReaderDTO> foundReaders = [];
        while (reader.Read())
        {
            foundReaders.Add(new ReaderDTO(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetInt64(4),
                reader.GetString(5),
                reader.GetInt64(6)
                ));
        }
        return foundReaders;

    }
}
