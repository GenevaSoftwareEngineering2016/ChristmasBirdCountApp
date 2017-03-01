// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace ChristmasBirdCountApp.Email
{
    [Activity(Label = "Bird Counter", Icon = "@drawable/audubon_society2", Theme = "@style/CustomActionBarTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Portrait)]
    public class FieldEmailFormActivity : Activity
    {
        private EditText _recipientEmail;
        private EditText _partyMembers;
        private EditText _partySize;
        private EditText _countCircleCode;
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
        private ImageButton _ibEmailClearField;
        private ImageButton _ibPartyClearField;
        private ImageButton _ibPartySizeClearField;
        private ImageButton _ibCountCircleClearField;
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
        private LinearLayout _llAdd;
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

            _txtBirdLabel.Text = "Return to List";

            _llClear.Visibility = Android.Views.ViewStates.Invisible;
            _llSubmit.Visibility = Android.Views.ViewStates.Invisible;

            SetContentView(Resource.Layout.FieldEmailForm);

            // Find Fields and Buttons
            _recipientEmail = FindViewById<EditText>(Resource.Id.txtRecipientEmail);
            _partyMembers = FindViewById<EditText>(Resource.Id.txtPartyMembers);
            _partySize = FindViewById<EditText>(Resource.Id.txtPartySize);
            _countCircleCode = FindViewById<EditText>(Resource.Id.txtCountCircle);
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

            _startTime1.Adapter = timeSpinnerAdapter;
            _endTime1.Adapter = timeSpinnerAdapter;
            _startTime2.Adapter = optionalTimeSpinnerAdapter;   // The second start time is optional (users may not have taken a break)
            _endTime2.Adapter = optionalTimeSpinnerAdapter;     // The second end time is optional (users may not have taken a break)
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

            // Deregister Event Handlers for Buttons that Clear Text Fields
            _ibEmailClearField.Click -= ClearEmailField_OnClick;
            _ibPartyClearField.Click -= ClearPartyField_OnClick;
            _ibPartySizeClearField.Click -= ClearPartySizeField_OnClick;
            _ibCountCircleClearField.Click -= ClearCountCircleField_OnClick;
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
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            // Return to the Main App Screen
            // By overriding the function of the device's "Back" button
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            //SetContentView(Resource.Layout.Main);
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

            // Create the Body of the Email Message
            StringBuilder emailBodyText = new StringBuilder();
            emailBodyText.AppendLine("Christmas Bird Count Results\n");
            emailBodyText.AppendLine(currentDateTime + "\n");
            emailBodyText.AppendLine("Count Circle: " + _countCircleCode.Text + "\n");
            emailBodyText.AppendLine("Count Type: Field");
            emailBodyText.AppendLine("--------------------------------------\n");
            emailBodyText.AppendLine("Party Members: " + _partyMembers.Text + "\n");
            emailBodyText.AppendLine("Number in Party: " + _partySize.Text + "\n");
            emailBodyText.AppendLine("Start Time 1: " + _startTime1.SelectedItem.ToString());
            emailBodyText.AppendLine("End Time 1: " + _endTime1.SelectedItem.ToString() + "\n");
            emailBodyText.AppendLine("Start Time 2: " + _startTime2.SelectedItem.ToString());
            emailBodyText.AppendLine("End Time 2: " + _endTime2.SelectedItem.ToString() + "\n");
            emailBodyText.AppendLine("Hours Driven: " + _hoursDriven.Text);
            emailBodyText.AppendLine("Miles Driven: " + _milesDriven.Text + "\n");
            emailBodyText.AppendLine("Hours Walked: " + _hoursWalked.Text);
            emailBodyText.AppendLine("Miles Walked: " + _milesWalked.Text + "\n");
            emailBodyText.AppendLine("Hours Owling: " + _hoursOwling.Text + "\n");
            emailBodyText.AppendLine("Total Number of Bird Species Seen: " + MainActivity.totalSpeciesSeen.ToString() + "\n");
            emailBodyText.AppendLine("Total Number of All Birds Seen: " + MainActivity.totalBirdsSeen.ToString() + "\n");
            emailBodyText.AppendLine("Notes: " + _optionalNotes.Text);

            // Create and Send the Email Message
            Email emailToSend = new Email();

            emailToSend.CreateEmailMessage(_recipientEmail.Text, "Christmas Bird Count Results: " + currentDateTime + " " + _countCircleCode.Text + " Field", emailBodyText);

            // Send the Email - We Are Adding an Attachment
            emailSent = emailToSend.SendEmail(true, BirdListFile.FilePath);

            if (emailSent)
            {
                Toast.MakeText(this, "Email sent!", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Unable to send email.", ToastLength.Short).Show();
            }

            // Return to the Main App Screen
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            //SetContentView(Resource.Layout.Main);
        }
    }
}