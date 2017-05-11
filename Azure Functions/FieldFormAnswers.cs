// Class for Creating FieldFormAnswers Objects
public class FieldFormAnswers
{
    public string ReportSubmissionTime { get; set; }

    public string RecipientEmail { get; set; }
    public string PartyMembers { get; set; }
    public string PartySize { get; set; }
    public string CountCircleCode { get; set; }
    public string CountType { get; set; }       // For a Field Form, this will always be "FIELD"

    public string TeamLeaderName { get; set; }
    public string TeamLeaderPhone { get; set; }
    public string TeamLeaderEmail { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public string StartTime1 { get; set; }
    public string EndTime1 { get; set; }
    public string StartTime2 { get; set; }
    public string EndTime2 { get; set; }
    public string HoursDriven { get; set; }
    public string MilesDriven { get; set; }
    public string HoursWalked { get; set; }
    public string MilesWalked { get; set; }
    public string HoursOwling { get; set; }
    public string OptionalNotes { get; set; }

    public string TotalBirdSpeciesSeen { get; set; }
    public string TotalBirdsSeen { get; set; }

    public FieldFormAnswers() { }

    public FieldFormAnswers
    (
        string reportSubmissionTime,
        string recipientEmail,
        string partyMembers,
        string partySize,
        string countCircleCode,
        string countType,
        string teamLeaderName,
        string teamLeaderPhone,
        string teamLeaderEmail,
        string street,
        string city,
        string state,
        string zip,
        string startTime1,
        string endTime1,
        string startTime2,
        string endTime2,
        string hoursDriven,
        string milesDriven,
        string hoursWalked,
        string milesWalked,
        string hoursOwling,
        string optionalNotes,
        string totalBirdSpeciesSeen,
        string totalBirdsSeen
    )
    {
        ReportSubmissionTime = reportSubmissionTime;
        RecipientEmail = recipientEmail;
        PartyMembers = partyMembers;
        PartySize = partySize;
        CountCircleCode = countCircleCode;
        CountType = countType;
        TeamLeaderName = teamLeaderName;
        TeamLeaderPhone = teamLeaderPhone;
        TeamLeaderEmail = teamLeaderEmail;
        Street = street;
        City = city;
        State = state;
        Zip = zip;
        StartTime1 = startTime1;
        EndTime1 = endTime1;
        StartTime2 = startTime2;
        EndTime2 = endTime2;
        HoursDriven = hoursDriven;
        MilesDriven = milesDriven;
        HoursWalked = hoursWalked;
        MilesWalked = milesWalked;
        HoursOwling = hoursOwling;
        OptionalNotes = optionalNotes;
        TotalBirdSpeciesSeen = totalBirdSpeciesSeen;
        TotalBirdsSeen = totalBirdsSeen;
    }
}