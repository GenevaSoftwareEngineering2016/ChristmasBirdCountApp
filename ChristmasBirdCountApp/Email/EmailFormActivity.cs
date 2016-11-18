// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ChristmasBirdCountApp.Email
{
    [Activity(Label = "Bird Counter", Icon = "@drawable/audubon_society2")]
    public class EmailFormActivity : Activity
    {
        private EditText _recipientEmail;
        private Spinner _regionSelection;
        private EditText _hoursDriven;
        private EditText _milesDriven;
        private EditText _hoursWalked;
        private EditText _milesWalked;
        private EditText _hoursOwling;
        private EditText _partyMembers;
        private EditText _optionalNotes;
        private Button _returnButton;
        private Button _sendButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmailForm);

            // Register Fields and Buttons
            _recipientEmail = FindViewById<EditText>(Resource.Id.txtRecipientEmail);
            _regionSelection = FindViewById<Spinner>(Resource.Id.spnRegion);
            _hoursDriven = FindViewById<EditText>(Resource.Id.txtHoursDriven);
            _milesDriven = FindViewById<EditText>(Resource.Id.txtMilesDriven);
            _hoursWalked = FindViewById<EditText>(Resource.Id.txtHoursWalked);
            _milesWalked = FindViewById<EditText>(Resource.Id.txtMilesWalked);
            _hoursOwling = FindViewById<EditText>(Resource.Id.txtHoursOwling);
            _partyMembers = FindViewById<EditText>(Resource.Id.txtPartyMembers);
            _optionalNotes = FindViewById<EditText>(Resource.Id.txtNotes);
            _returnButton = FindViewById<Button>(Resource.Id.btnReturn);
            _sendButton = FindViewById<Button>(Resource.Id.btnSend);

            var spinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.RegionList,
                Android.Resource.Layout.SimpleSpinnerItem);

            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            _regionSelection.Adapter = spinnerAdapter;
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Register event handlers
            //_recipientEmail.TextChanged += RecipientEmailOnTextChanged;
            //_regionSelection.ItemSelected += RegionSelectionOnItemSelected;
            //_hoursDriven.TextChanged += HoursDrivenOnTextChanged;
            //_milesDriven.TextChanged += MilesDrivenOnTextChanged;
            //_hoursWalked.TextChanged += HoursWalkedOnTextChanged;
            //_milesWalked.TextChanged += MilesWalkedOnTextChanged;
            //_hoursOwling.TextChanged += HoursOwlingOnTextChanged;
            //_partyMembers.TextChanged += PartyMembersOnTextChanged;
            //_optionalNotes.TextChanged += OptionalNotesOnTextChanged;
            _returnButton.Click += ReturnButtonOnClick;
            _sendButton.Click += SendButtonOnClick;

        }

        protected override void OnStop()
        {
            // Deregister event handlers
            //_recipientEmail.TextChanged -= RecipientEmailOnTextChanged;
            //_regionSelection.ItemSelected-= RegionSelectionOnItemSelected;
            //_hoursDriven.TextChanged -= HoursDrivenOnTextChanged;
            //_milesDriven.TextChanged -= MilesDrivenOnTextChanged;
            //_hoursWalked.TextChanged -= HoursWalkedOnTextChanged;
            //_milesWalked.TextChanged -= MilesWalkedOnTextChanged;
            //_hoursOwling.TextChanged -= HoursOwlingOnTextChanged;
            //_partyMembers.TextChanged -= PartyMembersOnTextChanged;
            //_optionalNotes.TextChanged -= OptionalNotesOnTextChanged;
            _returnButton.Click -= ReturnButtonOnClick;
            _sendButton.Click -= SendButtonOnClick;

            base.OnStop();
        }

        //private void RecipientEmailOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void RegionSelectionOnItemSelected(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void HoursDrivenOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void MilesDrivenOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void HoursWalkedOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void MilesWalkedOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void HoursOwlingOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void PartyMembersOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void OptionalNotesOnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void ReturnButtonOnClick(object sender, EventArgs e)
        {
            // Return to the Main App Screen
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            SetContentView(Resource.Layout.Main);
        }

        private void SendButtonOnClick(object sender, EventArgs e)
        {
            bool emailSent = false;
            DateTime currentDateTime = DateTime.Now;

            // Create the Body of the Email Message
            StringBuilder emailBodyText = new StringBuilder();
            emailBodyText.AppendLine("Christmas Bird Count Results\n");
            emailBodyText.AppendLine(currentDateTime + "\n");
            emailBodyText.AppendLine("Region: " + _regionSelection.SelectedItem + "\n");
            emailBodyText.AppendLine("--------------------------------------\n");
            emailBodyText.AppendLine("Party Members: " + _partyMembers.Text + "\n");
            emailBodyText.AppendLine("Hours Driven: " + _hoursDriven.Text + "\n");
            emailBodyText.AppendLine("Miles Driven: " + _milesDriven.Text + "\n");
            emailBodyText.AppendLine("Hours Walked: " + _hoursWalked.Text + "\n");
            emailBodyText.AppendLine("Miles Walked: " + _milesWalked.Text + "\n");
            emailBodyText.AppendLine("Hours Owling: " + _hoursOwling.Text + "\n");
            emailBodyText.AppendLine("Notes: " + _optionalNotes.Text + "\n");
            emailBodyText.AppendLine("--------------------------------------\n");

            // Create and Send the Email Message
            Email emailToSend = new Email();

            emailToSend.CreateEmailMessage(_recipientEmail.Text, "Christmas Bird Count Results: " + currentDateTime + " " + _regionSelection.SelectedItem, emailBodyText);

            // Send the Email - We Are Adding an Attachment
            Context appContext = this;  // We need to get and pass in the app context so that we can access the email address/password saved as an app resource
            emailSent = emailToSend.SendEmail(appContext, true, BirdListFile.FilePath);

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
            SetContentView(Resource.Layout.Main);
        }
    }
}