namespace LibraryFireplace;

public record BookDTO(
    int Id,
    string CreatedAt,
    string Name,
    string Author,
    int YearOfPublication,
    string LanguageOfBookText,
    string Genre,
    bool IsLent = false,
    int? Borower = null,
    string? DueDate  = null
    );
