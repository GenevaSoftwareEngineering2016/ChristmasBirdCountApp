// (c) 2017 Geneva College Senior Software Project Team
using System;
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
    public class FeederFormActivity : Activity
    {
        // Text Fields and UI Objects
        private EditText _recipientEmail;
        private EditText _countCircleCode;
        private EditText _observerName;
        private EditText _observerPhone;
        private EditText _observerEmail;
        private EditText _street;
        private EditText _city;
        private Spinner _state;
        private EditText _zip;
        private EditText _hoursObserving;
        private EditText _optionalNotes;
        private Button _sendButton;

        // ImageButtons for Clearing/Resetting Fields
        private ImageButton _ibEmailClearField;
        private ImageButton _ibCountCircleClearField;

        private ImageButton _ibObserverNameClearField;
        private ImageButton _ibObserverPhoneClearField;
        private ImageButton _ibObserverEmailClearField;
        private ImageButton _ibStreetClearField;
        private ImageButton _ibCityClearField;
        private ImageButton _ibStateClearField;
        private ImageButton _ibZipClearField;
        private ImageButton _ibHoursObservingClearField;
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

            SetContentView(Resource.Layout.FeederForm);        // Display the Feeder Count Type Form

            // Find Fields and Buttons
            _recipientEmail = FindViewById<EditText>(Resource.Id.txtRecipientEmail);
            _countCircleCode = FindViewById<EditText>(Resource.Id.txtCountCircle);

            _observerName = FindViewById<EditText>(Resource.Id.txtObserverName);
            _observerPhone = FindViewById<EditText>(Resource.Id.txtObserverPhone);
            _observerEmail = FindViewById<EditText>(Resource.Id.txtObserverEmail);
            _street = FindViewById<EditText>(Resource.Id.txtStreet);
            _city = FindViewById<EditText>(Resource.Id.txtCity);
            _state = FindViewById<Spinner>(Resource.Id.spinnerState);
            _zip = FindViewById<EditText>(Resource.Id.txtZIPCode);

            _hoursObserving = FindViewById<EditText>(Resource.Id.txtHoursObserving);
            _optionalNotes = FindViewById<EditText>(Resource.Id.txtNotes);

            // Find Buttons that Clear Text Fields
            _ibEmailClearField = FindViewById<ImageButton>(Resource.Id.ibEmailClearField);
            _ibCountCircleClearField = FindViewById<ImageButton>(Resource.Id.ibCountCircleClearField);

            _ibObserverNameClearField = FindViewById<ImageButton>(Resource.Id.ibObserverNameClearField);
            _ibObserverPhoneClearField = FindViewById<ImageButton>(Resource.Id.ibObserverPhoneClearField);
            _ibObserverEmailClearField = FindViewById<ImageButton>(Resource.Id.ibObserverEmailClearField);
            _ibStreetClearField = FindViewById<ImageButton>(Resource.Id.ibStreetClearField);
            _ibCityClearField = FindViewById<ImageButton>(Resource.Id.ibCityClearField);
            _ibStateClearField = FindViewById<ImageButton>(Resource.Id.ibStateClearField);
            _ibZipClearField = FindViewById<ImageButton>(Resource.Id.ibZIPCodeClearField);

            _ibHoursObservingClearField = FindViewById<ImageButton>(Resource.Id.ibHoursObservingClearField);
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

            _ibObserverNameClearField.Click += ClearObserverName_OnClick;
            _ibObserverPhoneClearField.Click += ClearObserverPhone_OnClick;
            _ibObserverEmailClearField.Click += ClearObserverEmail_OnClick;
            _ibStreetClearField.Click += ClearStreet_OnClick;
            _ibCityClearField.Click += ClearCity_OnClick;
            _ibStateClearField.Click += ClearState_OnClick;
            _ibZipClearField.Click += ClearZip_OnClick;

            _ibHoursObservingClearField.Click += ClearHoursObservingField_OnClick;
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

            _ibObserverNameClearField.Click -= ClearObserverName_OnClick;
            _ibObserverPhoneClearField.Click -= ClearObserverPhone_OnClick;
            _ibObserverEmailClearField.Click -= ClearObserverEmail_OnClick;
            _ibStreetClearField.Click -= ClearStreet_OnClick;
            _ibCityClearField.Click -= ClearCity_OnClick;
            _ibStateClearField.Click -= ClearState_OnClick;
            _ibZipClearField.Click -= ClearZip_OnClick;

            _ibHoursObservingClearField.Click -= ClearHoursObservingField_OnClick;
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

        private void ClearObserverName_OnClick(object sender, EventArgs e)
        {
            _observerName.Text = "";
        }

        private void ClearObserverPhone_OnClick(object sender, EventArgs e)
        {
            _observerPhone.Text = "";
        }

        private void ClearObserverEmail_OnClick(object sender, EventArgs e)
        {
            _observerEmail.Text = "";
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

        private void ClearHoursObservingField_OnClick(object sender, EventArgs e)
        {
            _hoursObserving.Text = "";
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
            //emailBodyText.AppendLine("Count Type: Feeder");
            //emailBodyText.AppendLine("--------------------------------------\n");

            //emailBodyText.AppendLine("\n");
            //emailBodyText.AppendLine("Observer: " + _observerName.Text + "\n");
            //emailBodyText.AppendLine("Phone: " + _observerPhone.Text + "\n");
            //emailBodyText.AppendLine("Email: " + _observerEmail.Text + "\n");
            //emailBodyText.AppendLine("Address:" + "\n");
            //emailBodyText.AppendLine(_street.Text + "\n");
            //emailBodyText.AppendLine(_city.Text + ", " + _state.SelectedItem.ToString() + " " + _zip.Text + "\n");
            //emailBodyText.AppendLine("\n");

            //emailBodyText.AppendLine("Hours Observing: " + _hoursObserving.Text + "\n");
            //emailBodyText.AppendLine("Total Number of Bird Species Seen: " + MainActivity.totalSpeciesSeen.ToString() + "\n");
            //emailBodyText.AppendLine("Total Number of All Birds Seen: " + MainActivity.totalBirdsSeen.ToString() + "\n");
            //emailBodyText.AppendLine("Notes: " + _optionalNotes.Text);

            //// Create and Send the Email Message
            //Email emailToSend = new Email();

            //emailToSend.CreateEmailMessage(_recipientEmail.Text, "Christmas Bird Count Results: " + currentDateTime + " " + _countCircleCode.Text + " Feeder", emailBodyText);

            //// Send the Email - We Are Adding an Attachment
            //emailSent = emailToSend.SendEmail(true, BirdListFile.FilePath);

            // Put all the answers typed into the GUI form into an object (a list of sorts)
            FeederFormAnswers feederFormAnswers = new FeederFormAnswers
            (
                currentDateTime.ToString(),
                _recipientEmail.Text,
                _countCircleCode.Text,
                "Feeder",
                _observerName.Text,
                _observerPhone.Text,
                _observerEmail.Text,
                _street.Text,
                _city.Text,
                _state.SelectedItem.ToString(),
                _zip.Text,
                _hoursObserving.Text,
                _optionalNotes.Text,
                MainActivity.totalSpeciesSeen.ToString(),
                MainActivity.totalBirdsSeen.ToString()
            );

            // Submit the Data to Azure
            AzureDataPOSTer dataPOSTer = new AzureDataPOSTer(this.ApplicationContext, "feeder", null, feederFormAnswers, null);
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