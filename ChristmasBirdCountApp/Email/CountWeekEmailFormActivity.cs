// (c) 2017 Geneva College Senior Software Project Team
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
    public class CountWeekEmailFormActivity : Activity
    {
        // Text Fields and UI Objects
        private EditText _recipientEmail;
        private EditText _countCircleCode;
        private EditText _counterName;
        private EditText _counterPhone;
        private EditText _counterEmail;
        private EditText _street;
        private EditText _city;
        private Spinner _state;
        private EditText _zip;
        private EditText _optionalNotes;
        private Button _sendButton;

        // ImageButtons for Clearing/Resetting Fields
        private ImageButton _ibEmailClearField;
        private ImageButton _ibCountCircleClearField;

        private ImageButton _ibCounterNameClearField;
        private ImageButton _ibCounterPhoneClearField;
        private ImageButton _ibCounterEmailClearField;
        private ImageButton _ibStreetClearField;
        private ImageButton _ibCityClearField;
        private ImageButton _ibStateClearField;
        private ImageButton _ibZipClearField;
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

            SetContentView(Resource.Layout.CountWeekEmailForm);        // Display the Count Week Count Type Form

            // Find Fields and Buttons
            _recipientEmail = FindViewById<EditText>(Resource.Id.txtRecipientEmail);
            _countCircleCode = FindViewById<EditText>(Resource.Id.txtCountCircle);

            _counterName = FindViewById<EditText>(Resource.Id.txtCounterName);
            _counterPhone = FindViewById<EditText>(Resource.Id.txtCounterPhone);
            _counterEmail = FindViewById<EditText>(Resource.Id.txtCounterEmail);
            _street = FindViewById<EditText>(Resource.Id.txtStreet);
            _city = FindViewById<EditText>(Resource.Id.txtCity);
            _state = FindViewById<Spinner>(Resource.Id.spinnerState);
            _zip = FindViewById<EditText>(Resource.Id.txtZIPCode);

            _optionalNotes = FindViewById<EditText>(Resource.Id.txtNotes);

            // Find Buttons that Clear Text Fields
            _ibEmailClearField = FindViewById<ImageButton>(Resource.Id.ibEmailClearField);
            _ibCountCircleClearField = FindViewById<ImageButton>(Resource.Id.ibCountCircleClearField);

            _ibCounterNameClearField = FindViewById<ImageButton>(Resource.Id.ibCounterNameClearField);
            _ibCounterPhoneClearField = FindViewById<ImageButton>(Resource.Id.ibCounterPhoneClearField);
            _ibCounterEmailClearField = FindViewById<ImageButton>(Resource.Id.ibCounterEmailClearField);
            _ibStreetClearField = FindViewById<ImageButton>(Resource.Id.ibStreetClearField);
            _ibCityClearField = FindViewById<ImageButton>(Resource.Id.ibCityClearField);
            _ibStateClearField = FindViewById<ImageButton>(Resource.Id.ibStateClearField);
            _ibZipClearField = FindViewById<ImageButton>(Resource.Id.ibZIPCodeClearField);

            _ibNotesClearField = FindViewById<ImageButton>(Resource.Id.ibNotesClearField);

            // Find the Button that Submits/Sends Emails
            _sendButton = FindViewById<Button>(Resource.Id.btnSend);

            // Set Up State Selection Spinner Using Spinner Adapter and Strings.xml Resource
            var stateSpinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.state_array, Android.Resource.Layout.SimpleSpinnerItem);
            stateSpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

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
            _ibCountCircleClearField.Click += ClearCountCircleField_OnClick;

            _ibCounterNameClearField.Click += ClearCounterName_OnClick;
            _ibCounterPhoneClearField.Click += ClearCounterPhone_OnClick;
            _ibCounterEmailClearField.Click += ClearCounterEmail_OnClick;
            _ibStreetClearField.Click += ClearStreet_OnClick;
            _ibCityClearField.Click += ClearCity_OnClick;
            _ibStateClearField.Click += ClearState_OnClick;
            _ibZipClearField.Click += ClearZip_OnClick;

            _ibNotesClearField.Click += ClearNotesField_OnClick;
        }

        protected override void OnStop()
        {
            // Deregister event handlers
            _sendButton.Click -= SendButton_OnClick;
            _llAdd.Click -= LlAdd_OnClick;

            // Deregister Event Handlers for Buttons that Clear Text Fields
            _ibEmailClearField.Click -= ClearEmailField_OnClick;
            _ibCountCircleClearField.Click -= ClearCountCircleField_OnClick;

            _ibCounterNameClearField.Click -= ClearCounterName_OnClick;
            _ibCounterPhoneClearField.Click -= ClearCounterPhone_OnClick;
            _ibCounterEmailClearField.Click -= ClearCounterEmail_OnClick;
            _ibStreetClearField.Click -= ClearStreet_OnClick;
            _ibCityClearField.Click -= ClearCity_OnClick;
            _ibStateClearField.Click -= ClearState_OnClick;
            _ibZipClearField.Click -= ClearZip_OnClick;

            _ibNotesClearField.Click -= ClearNotesField_OnClick;

            base.OnStop();
        }

        private void ClearEmailField_OnClick(object sender, EventArgs e)
        {
            _recipientEmail.Text = "";
        }

        private void ClearCountCircleField_OnClick(object sender, EventArgs e)
        {
            _countCircleCode.Text = "";
        }

        private void ClearCounterName_OnClick(object sender, EventArgs e)
        {
            _counterName.Text = "";
        }

        private void ClearCounterPhone_OnClick(object sender, EventArgs e)
        {
            _counterPhone.Text = "";
        }

        private void ClearCounterEmail_OnClick(object sender, EventArgs e)
        {
            _counterEmail.Text = "";
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

            // Create the Body of the Email Message
            StringBuilder emailBodyText = new StringBuilder();
            emailBodyText.AppendLine("Christmas Bird Count Results\n");
            emailBodyText.AppendLine(currentDateTime + "\n");
            emailBodyText.AppendLine("Count Circle: " + _countCircleCode.Text + "\n");
            emailBodyText.AppendLine("Count Type: Count Week");
            emailBodyText.AppendLine("--------------------------------------\n");

            emailBodyText.AppendLine("\n");
            emailBodyText.AppendLine("Counter: " + _counterName.Text + "\n");
            emailBodyText.AppendLine("Phone: " + _counterPhone.Text + "\n");
            emailBodyText.AppendLine("Email: " + _counterEmail.Text + "\n");
            emailBodyText.AppendLine("Address:" + "\n");
            emailBodyText.AppendLine(_street.Text + "\n");
            emailBodyText.AppendLine(_city.Text + ", " + _state.SelectedItem.ToString() + " " + _zip.Text + "\n");
            emailBodyText.AppendLine("\n");

            emailBodyText.AppendLine("Total Number of Bird Species Seen: " + MainActivity.totalSpeciesSeen.ToString() + "\n");
            emailBodyText.AppendLine("Total Number of All Birds Seen: " + MainActivity.totalBirdsSeen.ToString() + "\n");
            emailBodyText.AppendLine("Notes: " + _optionalNotes.Text);

            // Create and Send the Email Message
            Email emailToSend = new Email();

            emailToSend.CreateEmailMessage(_recipientEmail.Text, "Christmas Bird Count Results: " + currentDateTime + " " + _countCircleCode.Text + " Count Week", emailBodyText);

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
        }
    }
}