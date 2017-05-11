// Class for Creating CountWeekFormAnswers Objects
public class CountWeekFormAnswers
{
    public string ReportSubmissionTime { get; set; }

    public string RecipientEmail { get; set; }

    public string CountCircleCode { get; set; }
    public string CountType { get; set; }       // For a Count Week Form, this will always be "Count Week"

    public string CounterName { get; set; }
    public string CounterPhone { get; set; }
    public string CounterEmail { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public string OptionalNotes { get; set; }

    public string TotalBirdSpeciesSeen { get; set; }
    public string TotalBirdsSeen { get; set; }

    public CountWeekFormAnswers() { }

    public CountWeekFormAnswers
    (
        string reportSubmissionTime,
        string recipientEmail,
        string countCircleCode,
        string countType,
        string counterName,
        string counterPhone,
        string counterEmail,
        string street,
        string city,
        string state,
        string zip,
        string optionalNotes,
        string totalBirdSpeciesSeen,
        string totalBirdsSeen
    )
    {
        ReportSubmissionTime = reportSubmissionTime;
        RecipientEmail = recipientEmail;
        CountCircleCode = countCircleCode;
        CountType = countType;
        CounterName = counterName;
        CounterPhone = counterPhone;
        CounterEmail = counterEmail;
        Street = street;
        City = city;
        State = state;
        Zip = zip;
        OptionalNotes = optionalNotes;
        TotalBirdSpeciesSeen = totalBirdSpeciesSeen;
        TotalBirdsSeen = totalBirdsSeen;
    }
}