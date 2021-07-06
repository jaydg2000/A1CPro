using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using A1CPro.Domain;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace A1CPro
{
    [Activity(Label = "DiaryEntryActivity", Theme = "@style/AppTheme")]
    public class DiaryEntryActivity : Activity
    {
        private TextView _textViewDateOfReading;
        private EditText _editTextSugar;
        private EditText _editTextWeight;
        private EditText _editTextBloodPressureSystolic;
        private EditText _editTextBloodPressureDiastolic;
        private CheckBox _checkBoxTookMorningMeds;
        private CheckBox _checkBoxTookEveningMeds;
        private Button _buttonSave;

        private DiaryEntry _entry;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_entry_details);

            _textViewDateOfReading = FindViewById<TextView>(Resource.Id.textViewDetailsReadingDate);
            _editTextSugar = FindViewById<EditText>(Resource.Id.editTextGlucoseReading);
            _editTextWeight = FindViewById<EditText>(Resource.Id.editTextWeight);
            _editTextBloodPressureSystolic = FindViewById<EditText>(Resource.Id.editTextSystolic);
            _editTextBloodPressureDiastolic = FindViewById<EditText>(Resource.Id.editTextDiastolic);
            _checkBoxTookMorningMeds = FindViewById<CheckBox>(Resource.Id.checkBoxTookMorningMeds);
            _checkBoxTookEveningMeds = FindViewById<CheckBox>(Resource.Id.checkBoxTookEveningMeds);
            _buttonSave = FindViewById<Button>(Resource.Id.buttonSave);

            _buttonSave.Click += _buttonSave_Click;

            InitDiaryEntry();
            LoadForm();
        }

        private void InitDiaryEntry()
        {
            if (this.Intent.HasExtra(Constants.EXTRA_DIARY_ENTRY))
            {                
                var intentPayload = this.Intent.GetStringExtra(Constants.EXTRA_DIARY_ENTRY);
                _entry = JsonConvert.DeserializeObject<DiaryEntry>(intentPayload);
            }
            else
            {
                _entry = new DiaryEntry()
                {
                    DateOfReading = DateTime.Now
                };
            }
        }

        private void LoadForm()
        {
            if (_entry == null || _entry.IsNew())
            {
                _textViewDateOfReading.Text = DateTime.Now.ToShortDateString();
                return;
            }

            _editTextSugar.Text = _entry.Sugar.ToString();
            if (_entry.Weight > 0)
            {
                _editTextWeight.Text = _entry.Weight.ToString();
            }
            if (_entry.BloodPressure.SystolicBloodPressue > 0)
            {
                _editTextBloodPressureSystolic.Text = _entry.BloodPressure.SystolicBloodPressue.ToString();
                _editTextBloodPressureDiastolic.Text = _entry.BloodPressure.DiastolicBloodPressure.ToString();
            }
            _checkBoxTookMorningMeds.Checked = _entry.MorningMedsTaken;
            _checkBoxTookEveningMeds.Checked = _entry.EveningMedsTaken;
        }

        private void PatchEntryFromForm()
        {
            if (_entry.IsNew())
            {
                _entry.DateOfReading = DateTime.Now;
            }

            var sugarText = _editTextSugar.Text;
            var weight = _editTextWeight.Text;
            var systolic = _editTextBloodPressureSystolic.Text;
            var diastolic = _editTextBloodPressureDiastolic.Text;

            if (string.IsNullOrWhiteSpace(sugarText))
            {
                return;
            }

            _entry.Sugar = int.Parse(sugarText);
            _entry.Weight = string.IsNullOrWhiteSpace(weight) ? 0 : double.Parse(weight);
            _entry.MorningMedsTaken = _checkBoxTookMorningMeds.Checked;
            _entry.EveningMedsTaken = _checkBoxTookEveningMeds.Checked;
            if (!string.IsNullOrWhiteSpace(diastolic) && !string.IsNullOrWhiteSpace(diastolic))
            {
                _entry.BloodPressure = new BloodPressureReading
                {
                    SystolicBloodPressue = int.Parse(systolic),
                    DiastolicBloodPressure = int.Parse(diastolic)
                };
            }
        }

        private void _buttonSave_Click(object sender, EventArgs e)
        {
            PatchEntryFromForm();
            if (_entry.Sugar > 0)
            {
                var data = new Intent();
                var entryJson = JsonConvert.SerializeObject(_entry);
                data.PutExtra(Constants.EXTRA_DIARY_ENTRY, entryJson);
                SetResult(Android.App.Result.Ok, data);
            }
            else
            {
                SetResult(Android.App.Result.Canceled);
            }
            Finish();
        }
    }
}