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
                    .AddChoices("\n  Show all books", "Select one book",
                                "Update", "Delete", "Back"));
            if (action_menu_choice == "Back") { break; }

            else if (action_menu_choice == "\n  Show all books")
            {
                List<BookDTO> allBooks = BookRepository.GetAllBooks();

                var table = new Table();
                table.ShowRowSeparators();
                table.AddColumns("ID", "Created At", "Name", "Author",
                    "Year of publication", "Language of book text", "Genre");

                foreach (BookDTO book in allBooks)
                {
                    table.AddRow(book.Id.ToString(), book.CreatedAt, book.Name,
                                 book.Author, book.YearOfPublication.ToString(),
                                 book.LanguageOfBookText, book.Genre);
                }
                AnsiConsole.Write(table);
            }

        }
                
    }

}