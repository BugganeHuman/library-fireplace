namespace LibraryFireplace;

public class Reader
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long PhoneNumber { get; set; }
    public string Citizenship { get; set; }
    public long PersonalID { get; set; }
    public Reader (string firstName, string lastName, long phoneNumber, string citizenship, long personalID)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Citizenship = citizenship;
        PersonalID = personalID;
    }
}
