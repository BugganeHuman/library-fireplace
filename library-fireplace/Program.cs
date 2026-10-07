using Spectre.Console;
using LibraryFireplace;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Welcome, it's panel of Library Fireplace\n");
BookRepository.Start();

while (true)
{
    string main_menu_choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .HighlightStyle(new Style(Color.DarkOrange))
            .AddChoices("Add Book", "Actions with Books", "Escape"));
    if (main_menu_choice == "Escape") { break; }

    else if (main_menu_choice == "Add Book")
    {
        string name = AnsiConsole.Prompt(
            new TextPrompt<string>("\nBook's name: ")
            );
        string author = AnsiConsole.Prompt(
            new TextPrompt<string>("\nAuthor: ")
            );
        int yearOfPublication = AnsiConsole.Prompt(
            new TextPrompt<int>("\nYear of publication: ")
            );
        string languageOfBookText = AnsiConsole.Prompt(
            new TextPrompt<string>("Language of book text: ")
            );
        string genre = AnsiConsole.Prompt(
            new TextPrompt<string>("\nGenre of book: ")
            );
        Book new_book = new Book(name, author, yearOfPublication,
                             languageOfBookText, genre);
        BookRepository.AddBook(new_book);
        AnsiConsole.MarkupLine("\n[green]DONE[/]\n");
    }

    else if (main_menu_choice == "Actions with Books")
    {
        while (true)
        {
            string action_menu_choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(new Style(Color.GreenYellow))
                    .AddChoices("\n  Show all books", "Find books",
                                "Update", "Delete", "Back"));
            if (action_menu_choice == "Back") { break; }

            else if (action_menu_choice == "\n  Show all books")
            {
                List<BookDTO> allBooks = BookRepository.GetAllBooks();
                ShowTableOFBooks(allBooks);
            }
            else if (action_menu_choice == "Find books") //can accept only Latin characters
            {
                int? id = AnsiConsole.Prompt(
                    new TextPrompt<int?>("\nID, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? name = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Name, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? author = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Author, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                int? yearOfPublication = AnsiConsole.Prompt(
                    new TextPrompt<int?>("Year of publiacation, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? languageOfBookText = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Language of book text, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? genre = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Genre, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                List<BookDTO> foundBooks = BookRepository.FindBooks(id, name, author,
                                        yearOfPublication, languageOfBookText, genre);
                ShowTableOFBooks(foundBooks);
            }

            else if (action_menu_choice == "Update")
            {
                int id = AnsiConsole.Ask<int>("id of book: ");
                string? name = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Name to update, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? author = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Author, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                int? yearOfPublication = AnsiConsole.Prompt(
                    new TextPrompt<int?>("Year of publiacation, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? languageOfBookText = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Language of book text," +
                    " or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                string? genre = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Genre, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue());
                int updatedRows = BookRepository.UpdateBook(id, name, author,
                                yearOfPublication, languageOfBookText, genre);
                AnsiConsole.MarkupLine("\n[green]DONE[/]");
                AnsiConsole.MarkupLine($"[green]UPDATED {updatedRows} BOOKS[/]\n");
            }

            else if (action_menu_choice == "Delete")
            {
                int id = AnsiConsole.Ask<int>("id of book: ");
                bool isSure = AnsiConsole.Confirm("Are you sure?", false);
                if (isSure)
                {
                    bool isDeleted = BookRepository.DeleteBook(id);
                    if (isDeleted) { AnsiConsole.MarkupLine("\n[green]DONE[/]\n"); }
                    else { AnsiConsole.MarkupLine("\n[orange1]DONE, NOTHING DELETED[/]\n"); }
                }
                else
                {
                    AnsiConsole.MarkupLine("\n[red]REJECTED[/]\n");
                }
            }
        }     
    }
}

static void ShowTableOFBooks (List<BookDTO> books)
{
    var table = new Table();
    table.ShowRowSeparators();
    table.AddColumns("ID", "Created At", "Name", "Author",
        "Year of publication", "Language of book text", "Genre");

    foreach (BookDTO book in books)
    {
        table.AddRow(book.Id.ToString(), book.CreatedAt, book.Name,
                     book.Author, book.YearOfPublication.ToString(),
                     book.LanguageOfBookText, book.Genre);
    }
    AnsiConsole.Write(table);
}