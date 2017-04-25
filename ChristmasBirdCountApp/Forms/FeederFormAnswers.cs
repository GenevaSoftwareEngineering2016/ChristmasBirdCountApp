// (c) 2017 Geneva College Senior Software Project Team
namespace ChristmasBirdCountApp.Forms
{
    public class FeederFormAnswers
    {
        public string ReportSubmissionTime { get; set; }

        public string RecipientEmail { get; set; }

        public string CountCircleCode { get; set; }
        public string CountType { get; set; }       // For a Feeder Form, this will always be "FEEDER"

        public string ObserverName { get; set; }
        public string ObserverPhone { get; set; }
        public string ObserverEmail { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string HoursObserving { get; set; }
        public string OptionalNotes { get; set; }

        public string TotalBirdSpeciesSeen { get; set; }
        public string TotalBirdsSeen { get; set; }

        public FeederFormAnswers() { }

        public FeederFormAnswers
        (
            string reportSubmissionTime,
            string recipientEmail,
            string countCircleCode,
            string countType,
            string observerName,
            string observerPhone,
            string observerEmail,
            string street,
            string city,
            string state,
            string zip,
            string hoursObserving,
            string optionalNotes,
            string totalBirdSpeciesSeen,
            string totalBirdsSeen
        )
        {
            ReportSubmissionTime = reportSubmissionTime;
            RecipientEmail = recipientEmail;
            CountCircleCode = countCircleCode;
            CountType = countType;
            ObserverName = observerName;
            ObserverPhone = observerPhone;
            ObserverEmail = observerEmail;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
            HoursObserving = hoursObserving;
            OptionalNotes = optionalNotes;
            TotalBirdSpeciesSeen = totalBirdSpeciesSeen;
            TotalBirdsSeen = totalBirdsSeen;
        }
    }
}