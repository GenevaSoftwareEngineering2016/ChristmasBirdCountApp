using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Widget;

namespace ChristmasBirdCountApp
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
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Register event handlers
            _recipientEmail.TextChanged += RecipientEmailOnTextChanged;
            _regionSelection.ItemSelected += RegionSelectionOnItemSelected;
            _hoursDriven.TextChanged += HoursDrivenOnTextChanged;
            _milesDriven.TextChanged += MilesDrivenOnTextChanged;
            _hoursWalked.TextChanged += HoursWalkedOnTextChanged;
            _milesWalked.TextChanged += MilesWalkedOnTextChanged;
            _hoursOwling.TextChanged += HoursOwlingOnTextChanged;
            _partyMembers.TextChanged += PartyMembersOnTextChanged;
            _optionalNotes.TextChanged += OptionalNotesOnTextChanged;
            _returnButton.Click += ReturnButtonOnClick;
            // Send Button Event Handler

        }

        protected override void OnStop()
        {
            // Deregister event handlers
            _recipientEmail.TextChanged -= RecipientEmailOnTextChanged;
            _regionSelection.ItemSelected-= RegionSelectionOnItemSelected;
            _hoursDriven.TextChanged -= HoursDrivenOnTextChanged;
            _milesDriven.TextChanged -= MilesDrivenOnTextChanged;
            _hoursWalked.TextChanged -= HoursWalkedOnTextChanged;
            _milesWalked.TextChanged -= MilesWalkedOnTextChanged;
            _hoursOwling.TextChanged -= HoursOwlingOnTextChanged;
            _partyMembers.TextChanged -= PartyMembersOnTextChanged;
            _optionalNotes.TextChanged -= OptionalNotesOnTextChanged;
            _returnButton.Click -= ReturnButtonOnClick;
            // Send Button Event Handler

            base.OnStop();
        }

        private void RecipientEmailOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegionSelectionOnItemSelected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HoursDrivenOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MilesDrivenOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HoursWalkedOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MilesWalkedOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HoursOwlingOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PartyMembersOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OptionalNotesOnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ReturnButtonOnClick(object sender, EventArgs e)
        {
            // Return to the Main App Screen
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            SetContentView(Resource.Layout.Main);
        }
    }
}