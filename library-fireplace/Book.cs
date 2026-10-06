namespace LibraryFireplace;

public class Book
{
    public string Name { get; set; } 
    public string Author { get; set; }
    public int YearOfPublication { get; set; }
    public string LanguageOfBookText { get; set; }
    public string Genre { get; set; }
    
    public Book (string name, string author, int yearOfPublication,
                 string languageOfBookText, string genre)
    {
        Name = name;
        Author = author;
        YearOfPublication = yearOfPublication;
        LanguageOfBookText = languageOfBookText;
        Genre = genre;
    }


}
