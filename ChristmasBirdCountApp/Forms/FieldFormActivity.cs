// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using ChristmasBirdCountApp.Azure;
using ChristmasBirdCountApp.Forms;

namespace ChristmasBirdCountApp.Email
{
    [Activity(Label = "Birdubon", Icon = "@drawable/audubon_society2", Theme = "@style/CustomActionBarTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Portrait)]
    public class FieldFormActivity : Activity
    {
        // Text Fields and UI Objects
        private EditText _recipientEmail;
        private EditText _partyMembers;
        private EditText _partySize;
        private EditText _countCircleCode;

        private EditText _teamLeaderName;
        private EditText _teamLeaderPhone;
        private EditText _teamLeaderEmail;
        private EditText _street;
        private EditText _city;
        private Spinner _state;
        private EditText _zip;

        private Spinner _startTime1;
        private Spinner _endTime1;
        private Spinner _startTime2;
        private Spinner _endTime2;
        private EditText _hoursDriven;
        private EditText _milesDriven;
        private EditText _hoursWalked;
        private EditText _milesWalked;
        private EditText _hoursOwling;
        private EditText _optionalNotes;
        private Button _sendButton;

        // ImageButtons for Clearing/Resetting Fields
        private ImageButton _ibEmailClearField;
        private ImageButton _ibPartyClearField;
        private ImageButton _ibPartySizeClearField;
        private ImageButton _ibCountCircleClearField;

        private ImageButton _ibTeamLeaderNameClearField;
        private ImageButton _ibTeamLeaderPhoneClearField;
        private ImageButton _ibTeamLeaderEmailClearField;
        private ImageButton _ibStreetClearField;
        private ImageButton _ibCityClearField;
        private ImageButton _ibStateClearField;
        private ImageButton _ibZipClearField;

        private ImageButton _ibStartTime1ClearField;
        private ImageButton _ibEndTime1ClearField;
        private ImageButton _ibStartTime2ClearField;
        private ImageButton _ibEndTime2ClearField;
        private ImageButton _ibHDClearField;
        private ImageButton _ibMDClearField;
        private ImageButton _ibHWClearField;
        private ImageButton _ibMWClearField;
        private ImageButton _ibHOClearField;
        private ImageButton _ibNotesClearField;

        private LinearLayout _llClear;
        private LinearLayout _llAdd;        // The "Return to List" button
        private LinearLayout _llSubmit;
        private TextView _txtBirdLabel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetCustomView(Resource.Layout.actionBar);
            ActionBar.SetDisplayShowCustomEnabled(true);

            _llClear = FindViewById<LinearLayout>(Resource.Id.llClear);
            _llAdd = FindViewById<LinearLayout>(Resource.Id.llAdd);
            _llSubmit = FindViewById<LinearLayout>(Resource.Id.llSubmit);
            _txtBirdLabel = FindViewById<TextView>(Resource.Id.lblAdd);

            _llClear.Visibility = Android.Views.ViewStates.Invisible;
            _llSubmit.Visibility = Android.Views.ViewStates.Invisible;

            _txtBirdLabel.Text = "Return to List";

            SetContentView(Resource.Layout.FieldForm);     // Display the Field Count Type Form

            // Find Fields and Buttons
            _recipientEmail = FindViewById<EditText>(Resource.Id.txtRecipientEmail);
            _partyMembers = FindViewById<EditText>(Resource.Id.txtPartyMembers);
            _partySize = FindViewById<EditText>(Resource.Id.txtPartySize);
            _countCircleCode = FindViewById<EditText>(Resource.Id.txtCountCircle);

            _teamLeaderName = FindViewById<EditText>(Resource.Id.txtTeamLeaderName);
            _teamLeaderPhone = FindViewById<EditText>(Resource.Id.txtTeamLeaderPhone);
            _teamLeaderEmail = FindViewById<EditText>(Resource.Id.txtTeamLeaderEmail);
            _street = FindViewById<EditText>(Resource.Id.txtStreet);
            _city = FindViewById<EditText>(Resource.Id.txtCity);
            _state = FindViewById<Spinner>(Resource.Id.spinnerState);
            _zip = FindViewById<EditText>(Resource.Id.txtZIPCode);

            _startTime1 = FindViewById<Spinner>(Resource.Id.spinnerStartTime1);
            _endTime1 = FindViewById<Spinner>(Resource.Id.spinnerEndTime1);
            _startTime2 = FindViewById<Spinner>(Resource.Id.spinnerStartTime2);
            _endTime2 = FindViewById<Spinner>(Resource.Id.spinnerEndTime2);
            _hoursDriven = FindViewById<EditText>(Resource.Id.txtHoursDriven);
            _milesDriven = FindViewById<EditText>(Resource.Id.txtMilesDriven);
            _hoursWalked = FindViewById<EditText>(Resource.Id.txtHoursWalked);
            _milesWalked = FindViewById<EditText>(Resource.Id.txtMilesWalked);
            _hoursOwling = FindViewById<EditText>(Resource.Id.txtHoursOwling);
            _optionalNotes = FindViewById<EditText>(Resource.Id.txtNotes);

            // Find Buttons that Clear Text Fields
            _ibEmailClearField = FindViewById<ImageButton>(Resource.Id.ibEmailClearField);
            _ibPartyClearField = FindViewById<ImageButton>(Resource.Id.ibPartyClearField);
            _ibPartySizeClearField = FindViewById<ImageButton>(Resource.Id.ibPartySizeClearField);
            _ibCountCircleClearField = FindViewById<ImageButton>(Resource.Id.ibCountCircleClearField);

            _ibTeamLeaderNameClearField = FindViewById<ImageButton>(Resource.Id.ibTeamLeaderNameClearField);
            _ibTeamLeaderPhoneClearField = FindViewById<ImageButton>(Resource.Id.ibTeamLeaderPhoneClearField);
            _ibTeamLeaderEmailClearField = FindViewById<ImageButton>(Resource.Id.ibTeamLeaderEmailClearField);
            _ibStreetClearField = FindViewById<ImageButton>(Resource.Id.ibStreetClearField);
            _ibCityClearField = FindViewById<ImageButton>(Resource.Id.ibCityClearField);
            _ibStateClearField = FindViewById<ImageButton>(Resource.Id.ibStateClearField);
            _ibZipClearField = FindViewById<ImageButton>(Resource.Id.ibZIPCodeClearField);

            _ibStartTime1ClearField = FindViewById<ImageButton>(Resource.Id.ibStart1ClearField);
            _ibEndTime1ClearField = FindViewById<ImageButton>(Resource.Id.ibEnd1ClearField);
            _ibStartTime2ClearField = FindViewById<ImageButton>(Resource.Id.ibStart2ClearField);
            _ibEndTime2ClearField = FindViewById<ImageButton>(Resource.Id.ibEnd2ClearField);
            _ibHDClearField = FindViewById<ImageButton>(Resource.Id.ibHDClearField);
            _ibMDClearField = FindViewById<ImageButton>(Resource.Id.ibMDClearField);
            _ibHWClearField = FindViewById<ImageButton>(Resource.Id.ibHWClearField);
            _ibMWClearField = FindViewById<ImageButton>(Resource.Id.ibMWClearField);
            _ibHOClearField = FindViewById<ImageButton>(Resource.Id.ibHOClearField);
            _ibNotesClearField = FindViewById<ImageButton>(Resource.Id.ibNotesClearField);

            // Find the Button that Submits/Sends Emails
            _sendButton = FindViewById<Button>(Resource.Id.btnSend);

            // Set Up Time Selection Spinners Using Spinner Adapter and Strings.xml Resource
            var timeSpinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.times_array, Android.Resource.Layout.SimpleSpinnerItem);
            timeSpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            // Set Up Time Selection Spinner for Optional Times (Start and End Times 2)
            var optionalTimeSpinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.time_optional_array, Android.Resource.Layout.SimpleSpinnerItem);
            optionalTimeSpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            // Set Up State Selection Spinner Using Spinner Adapter and Strings.xml Resource
            var stateSpinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.state_array, Android.Resource.Layout.SimpleSpinnerItem);
            stateSpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            _startTime1.Adapter = timeSpinnerAdapter;
            _endTime1.Adapter = timeSpinnerAdapter;
            _startTime2.Adapter = optionalTimeSpinnerAdapter;   // The second start time is optional (users may not have taken a break)
            _endTime2.Adapter = optionalTimeSpinnerAdapter;     // The second end time is optional (users may not have taken a break)
            _state.Adapter = stateSpinnerAdapter;
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Register event handlers
            _sendButton.Click += SendButton_OnClick;
            _llAdd.Click += LlAdd_OnClick;

            // Register Event Handlers for Buttons that Clear Text Fields
            _ibEmailClearField.Click += ClearEmailField_OnClick;
            _ibPartyClearField.Click += ClearPartyField_OnClick;
            _ibPartySizeClearField.Click += ClearPartySizeField_OnClick;
            _ibCountCircleClearField.Click += ClearCountCircleField_OnClick;

            _ibTeamLeaderNameClearField.Click += ClearTeamLeaderName_OnClick;
            _ibTeamLeaderPhoneClearField.Click += ClearTeamLeaderPhone_OnClick;
            _ibTeamLeaderEmailClearField.Click += ClearTeamLeaderEmail_OnClick;
            _ibStreetClearField.Click += ClearStreet_OnClick;
            _ibCityClearField.Click += ClearCity_OnClick;
            _ibStateClearField.Click += ClearState_OnClick;
            _ibZipClearField.Click += ClearZip_OnClick;

            _ibStartTime1ClearField.Click += ClearStartTime1_OnClick;
            _ibEndTime1ClearField.Click += ClearEndTime1_OnClick;
            _ibStartTime2ClearField.Click += ClearStartTime2_OnClick;
            _ibEndTime2ClearField.Click += ClearEndTime2_OnClick;
            _ibHDClearField.Click += ClearHoursDrivenField_OnClick;
            _ibMDClearField.Click += ClearMilesDrivenField_OnClick;
            _ibHWClearField.Click += ClearHoursWalkedField_OnClick;
            _ibMWClearField.Click += ClearMilesWalkedField_OnClick;
            _ibHOClearField.Click += ClearHoursOwlingField_OnClick;
            _ibNotesClearField.Click += ClearNotesField_OnClick;
        }

        protected override void OnStop()
        {
            // Deregister event handlers
            _sendButton.Click -= SendButton_OnClick;
            _llAdd.Click -= LlAdd_OnClick;

            // Deregister Event Handlers for Buttons that Clear Text Fields
            _ibEmailClearField.Click -= ClearEmailField_OnClick;
            _ibPartyClearField.Click -= ClearPartyField_OnClick;
            _ibPartySizeClearField.Click -= ClearPartySizeField_OnClick;
            _ibCountCircleClearField.Click -= ClearCountCircleField_OnClick;

            _ibTeamLeaderNameClearField.Click -= ClearTeamLeaderName_OnClick;
            _ibTeamLeaderPhoneClearField.Click -= ClearTeamLeaderPhone_OnClick;
            _ibTeamLeaderEmailClearField.Click -= ClearTeamLeaderEmail_OnClick;
            _ibStreetClearField.Click -= ClearStreet_OnClick;
            _ibCityClearField.Click -= ClearCity_OnClick;
            _ibStateClearField.Click -= ClearState_OnClick;
            _ibZipClearField.Click -= ClearZip_OnClick;

            _ibStartTime1ClearField.Click -= ClearStartTime1_OnClick;
            _ibEndTime1ClearField.Click -= ClearEndTime1_OnClick;
            _ibStartTime2ClearField.Click -= ClearStartTime2_OnClick;
            _ibEndTime2ClearField.Click -= ClearEndTime2_OnClick;
            _ibHDClearField.Click -= ClearHoursDrivenField_OnClick;
            _ibMDClearField.Click -= ClearMilesDrivenField_OnClick;
            _ibHWClearField.Click -= ClearHoursWalkedField_OnClick;
            _ibMWClearField.Click -= ClearMilesWalkedField_OnClick;
            _ibHOClearField.Click -= ClearHoursOwlingField_OnClick;
            _ibNotesClearField.Click -= ClearNotesField_OnClick;

            base.OnStop();
        }

        private void ClearEmailField_OnClick(object sender, EventArgs e)
        {
            _recipientEmail.Text = "";
        }

        private void ClearPartyField_OnClick(object sender, EventArgs e)
        {
            _partyMembers.Text = "";
        }

        private void ClearPartySizeField_OnClick(object sender, EventArgs e)
        {
            _partySize.Text = "";
        }

        private void ClearCountCircleField_OnClick(object sender, EventArgs e)
        {
            _countCircleCode.Text = "";
        }

        private void ClearTeamLeaderName_OnClick(object sender, EventArgs e)
        {
            _teamLeaderName.Text = "";
        }

        private void ClearTeamLeaderPhone_OnClick(object sender, EventArgs e)
        {
            _teamLeaderPhone.Text = "";
        }

        private void ClearTeamLeaderEmail_OnClick(object sender, EventArgs e)
        {
            _teamLeaderEmail.Text = "";
        }

        private void ClearStreet_OnClick(object sender, EventArgs e)
        {
            _street.Text = "";
        }

        private void ClearCity_OnClick(object sender, EventArgs e)
        {
            _city.Text = "";
        }

        private void ClearState_OnClick(object sender, EventArgs e)
        {
            _state.SetSelection(0);
        }

        private void ClearZip_OnClick(object sender, EventArgs e)
        {
            _zip.Text = "";
        }

        private void ClearStartTime1_OnClick(object sender, EventArgs e)
        {
            _startTime1.SetSelection(0);
        }

        private void ClearEndTime1_OnClick(object sender, EventArgs e)
        {
            _endTime1.SetSelection(0);
        }

        private void ClearStartTime2_OnClick(object sender, EventArgs e)
        {
            _startTime2.SetSelection(0);
        }

        private void ClearEndTime2_OnClick(object sender, EventArgs e)
        {
            _endTime2.SetSelection(0);
        }

        private void ClearHoursDrivenField_OnClick(object sender, EventArgs e)
        {
            _hoursDriven.Text = "";
        }

        private void ClearMilesDrivenField_OnClick(object sender, EventArgs e)
        {
            _milesDriven.Text = "";
        }

        private void ClearHoursWalkedField_OnClick(object sender, EventArgs e)
        {
            _hoursWalked.Text = "";
        }

        private void ClearMilesWalkedField_OnClick(object sender, EventArgs e)
        {
            _milesWalked.Text = "";
        }

        private void ClearHoursOwlingField_OnClick(object sender, EventArgs e)
        {
            _hoursOwling.Text = "";
        }

        private void ClearNotesField_OnClick(object sender, EventArgs e)
        {
            _optionalNotes.Text = "";
        }

        private void LlAdd_OnClick(object sender, EventArgs e)
        {
            // Return to the Main App Screen
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            // Return to the Main App Screen
            // By overriding the function of the device's "Back" button
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void SendButton_OnClick(object sender, EventArgs e)
        {
            bool emailSent = false;
            DateTime currentDateTime = DateTime.Now;

            // Check to ensure that user entered an email address for the recipient of the report (i.e. the Compiler)
            if (string.IsNullOrEmpty(_recipientEmail.Text))
            {
                Toast.MakeText(this, "You must enter a recipient email address.", ToastLength.Short).Show();
                return;
            }

            //// Create the Body of the Email Message
            //StringBuilder emailBodyText = new StringBuilder();
            //emailBodyText.AppendLine("Christmas Bird Count Results\n");
            //emailBodyText.AppendLine(currentDateTime + "\n");
            //emailBodyText.AppendLine("Count Circle: " + _countCircleCode.Text + "\n");
            //emailBodyText.AppendLine("Count Type: Field");
            //emailBodyText.AppendLine("--------------------------------------\n");
            //emailBodyText.AppendLine("Party Members: " + _partyMembers.Text + "\n");
            //emailBodyText.AppendLine("Number in Party: " + _partySize.Text + "\n");

            //emailBodyText.AppendLine("\n");
            //emailBodyText.AppendLine("Team Leader: " + _teamLeaderName.Text + "\n");
            //emailBodyText.AppendLine("Phone: " + _teamLeaderPhone.Text + "\n");
            //emailBodyText.AppendLine("Email: " + _teamLeaderEmail.Text + "\n");
            //emailBodyText.AppendLine("Address:" + "\n");
            //emailBodyText.AppendLine(_street.Text + "\n");
            //emailBodyText.AppendLine(_city.Text + ", " + _state.SelectedItem.ToString() + " " + _zip.Text + "\n");
            //emailBodyText.AppendLine("\n");

            //emailBodyText.AppendLine("Start Time 1: " + _startTime1.SelectedItem.ToString());
            //emailBodyText.AppendLine("End Time 1: " + _endTime1.SelectedItem.ToString() + "\n");
            //emailBodyText.AppendLine("Start Time 2: " + _startTime2.SelectedItem.ToString());
            //emailBodyText.AppendLine("End Time 2: " + _endTime2.SelectedItem.ToString() + "\n");
            //emailBodyText.AppendLine("Hours Driven: " + _hoursDriven.Text);
            //emailBodyText.AppendLine("Miles Driven: " + _milesDriven.Text + "\n");
            //emailBodyText.AppendLine("Hours Walked: " + _hoursWalked.Text);
            //emailBodyText.AppendLine("Miles Walked: " + _milesWalked.Text + "\n");
            //emailBodyText.AppendLine("Hours Owling: " + _hoursOwling.Text + "\n");

            //emailBodyText.AppendLine("Total Number of Bird Species Seen: " + MainActivity.totalSpeciesSeen.ToString() + "\n");
            //emailBodyText.AppendLine("Total Number of All Birds Seen: " + MainActivity.totalBirdsSeen.ToString() + "\n");
            //emailBodyText.AppendLine("Notes: " + _optionalNotes.Text);

            //// Create and Send the Email Message
            //Email emailToSend = new Email();

            //emailToSend.CreateEmailMessage(_recipientEmail.Text, "Christmas Bird Count Results: " + currentDateTime + " " + _countCircleCode.Text + " Field", emailBodyText);

            //// Send the Email - We Are Adding an Attachment
            //emailSent = emailToSend.SendEmail(true, BirdListFile.FilePath);

            // Put all the answers typed into the GUI form into an object (a list of sorts)
            FieldFormAnswers fieldFormAnswers = new FieldFormAnswers
            (
                currentDateTime.ToString(),
                _recipientEmail.Text,
                _partyMembers.Text,
                _partySize.Text,
                _countCircleCode.Text,
                "Field",
                _teamLeaderName.Text,
                _teamLeaderPhone.Text,
                _teamLeaderEmail.Text,
                _street.Text,
                _city.Text,
                _state.SelectedItem.ToString(),
                _zip.Text,
                _startTime1.SelectedItem.ToString(),
                _endTime1.SelectedItem.ToString(),
                _startTime2.SelectedItem.ToString(),
                _endTime2.SelectedItem.ToString(),
                _hoursDriven.Text,
                _milesDriven.Text,
                _hoursWalked.Text,
                _milesWalked.Text,
                _hoursOwling.Text,
                _optionalNotes.Text,
                MainActivity.totalSpeciesSeen.ToString(),
                MainActivity.totalBirdsSeen.ToString()
            );

            // Submit the Data to Azure
            AzureDataPOSTer dataPOSTer = new AzureDataPOSTer(this.ApplicationContext, "field", fieldFormAnswers, null, null);
            bool dataSentToAzure = dataPOSTer.PerformPostAgainstAzureFunctionApi();

            if (dataSentToAzure) //if (emailSent && dataSentToAzure)
            {
                Toast.MakeText(this, "Report submitted!", ToastLength.Short).Show();
            }
            //else if (!emailSent)
            //{
            //    Toast.MakeText(this, "Unable to send email.", ToastLength.Short).Show();
            //}
            else
            {
                Toast.MakeText(this, "Unable to send data to Azure API.", ToastLength.Short).Show();
            }

            // Return to the Main App Screen
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}