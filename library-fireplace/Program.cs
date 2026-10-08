using Spectre.Console;
using LibraryFireplace;
using System.ComponentModel.Design;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Welcome, it's panel of Library Fireplace\n");
DbContext.Start();

while (true)
{
    string main_menu_choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .HighlightStyle(new Style(Color.DarkOrange))
            .AddChoices("Books", "Lend a Book", "Readers", "Escape"));
    if (main_menu_choice == "Escape") { break; }

    else if (main_menu_choice == "Lend a Book")
    {
        int bookID = AnsiConsole.Ask<int>("Book ID: ");
        int readerID = AnsiConsole.Ask<int>("Reader ID: ");
        BookRepository.LendBook(bookID, readerID);
        AnsiConsole.MarkupLine("\n[green]DONE[/]\n");
    }

    else if (main_menu_choice == "Books")
    {
        while (true)
        {
            string book_menu_choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(new Style(Color.GreenYellow))
                    .AddChoices("\n  Show all books", "Add Book", "Find books",
                                "Update", "Delete", "Back"));
            if (book_menu_choice == "Back") { break; }

            else if (book_menu_choice == "\n  Show all books")
            {
                List<BookDTO> allBooks = BookRepository.GetAllBooks();
                ShowTableOFBooks(allBooks);
            }

            else if (book_menu_choice == "Add Book")
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


            else if (book_menu_choice == "Find books") //can accept only Latin characters
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

            else if (book_menu_choice == "Update")
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

            else if (book_menu_choice == "Delete")
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

    else if (main_menu_choice == "Readers")
    {
        while(true)
        {
            string reader_menu_choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(new Style(Color.IndianRed1))
                    .AddChoices("\n  Show all readers", "Add Reader", "Find readers",
                                "Update", "Delete", "Back"));
            if (reader_menu_choice == "Back") { break; }

            else if (reader_menu_choice == "\n  Show all readers")
            {
                List<ReaderDTO> allReaders = ReaderRepository.GetAllReaders();
                ShowTableOfReaders(allReaders);
            }

            else if (reader_menu_choice == "Add Reader")
            {
                string firstName = AnsiConsole.Ask<string>("First Name: ");
                string lastName = AnsiConsole.Ask<string>("Last Name: ");
                long phoneNumber = AnsiConsole.Ask<long>("Phone Number: ");
                string citizenship = AnsiConsole.Ask<string>("Citizenship: ");
                long personalID = AnsiConsole.Ask<long>("Personal ID: ");
                ReaderRepository.AddReader(new Reader (firstName, lastName, phoneNumber, citizenship, personalID));
                AnsiConsole.MarkupLine("\n[green]DONE[/]\n");
            }


        }
    }
}

static void ShowTableOFBooks (List<BookDTO> books)
{
    var table = new Table();
    table.ShowRowSeparators();
    table.AddColumns("ID", "Created At", "Name", "Author","Year of publication",
                     "Language of book text", "Genre", "Is Lent",
                     "Borrower", "Due Date");

    foreach (BookDTO book in books)
    {
        table.AddRow(book.Id.ToString(), book.CreatedAt, book.Name, book.Author,
                     book.YearOfPublication.ToString(), book.LanguageOfBookText,
                     book.Genre, book.IsLent.ToString(), book.Borower.ToString() ?? "None",
                     book.DueDate ?? "None");
    }
    AnsiConsole.Write(table);
}

static void ShowTableOfReaders (List<ReaderDTO> readers)
{
    var table = new Table();
    table.ShowRowSeparators();
    table.AddColumns("ID", "Created At", "First Name", "Last Name",
    "Phone Number", "Citizenship", "Personal ID");
    foreach (ReaderDTO reader in readers)
    {
        table.AddRow(reader.ID.ToString(), reader.CreatedAt, reader.FirstName,
                     reader.LastName, reader.PhoneNumber.ToString(),
                     reader.Citizenship, reader.PersonalID.ToString());
    }
    AnsiConsole.Write(table);
}