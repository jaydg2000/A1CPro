using A1CPro.Data;
using A1CPro.Domain;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Microcharts;
using Microcharts.Droid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace A1CPro
{
    [Activity(Label = "DashboardActivity", Theme = "@style/AppTheme", MainLauncher = true)]
    public class DashboardActivity : Activity
    {
        private DiaryRepository _repo;
        private IReadOnlyList<DiaryEntry> _diary;
        private Chart _chart;
        private ChartView _chartView;
        private RecyclerView _recyclerViewReadings;
        private DiaryRecyclerViewAdapter _diaryRecyclerViewAdapter;
        private TextView _textViewA1CPrediction;
        private FloatingActionButton _fab;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            _textViewA1CPrediction = FindViewById<TextView>(Resource.Id.textViewA1CPrediction);
            _fab = FindViewById<FloatingActionButton>(Resource.Id.fabAddEdit);
            _fab.Click += Fab_Click;
            _chartView = FindViewById<ChartView>(Resource.Id.chartViewSugarChart);
            _recyclerViewReadings = FindViewById<RecyclerView>(Resource.Id.recyclerViewHistory);
            _repo = new DiaryRepository();
            await LoadChart();
            await LoadReadingHistory();
            LoadA1CPrediction();
        }

        private void OnRecyclerViewReadings_LongClick(object sender, DiaryRecyclerViewAdapterClickEventArgs e)
        {
            if (e.Position < 0 || e.Position >= _diary.Count)
            {
                return;
            }

            var diaryEntryToEdit = _diary[e.Position];
            var intent = new Intent(this, typeof(DiaryEntryActivity));
            intent.PutExtra(Constants.EXTRA_DIARY_ENTRY, JsonConvert.SerializeObject(diaryEntryToEdit));
            StartActivityForResult(intent, Constants.REQUEST_CODE_MODIFY_ENTRY);
        }

        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == Constants.REQUEST_CODE_ADD_ENTRY
                || requestCode == Constants.REQUEST_CODE_MODIFY_ENTRY)
            {
                if (resultCode == Result.Ok)
                {
                    var entry = GetDiaryEntryFromIntent(data);
                    if (entry != null)
                    {
                        await _repo.SaveDiaryEntry(entry);
                        await LoadChart();
                        await LoadReadingHistory();

                        Toast.MakeText(this, "Entry saved.", ToastLength.Short).Show();
                        return;
                    }
                    Toast.MakeText(this, "Entry NOT saved.", ToastLength.Short).Show();
                }
                Toast.MakeText(this, "Cancelled.", ToastLength.Short).Show();
            }
        }

        private DiaryEntry GetDiaryEntryFromIntent(Intent data)
        {
            if (data == null || data.Extras == null || !data.HasExtra(Constants.EXTRA_DIARY_ENTRY))
            {
                return null;
            }

            var entryJson = data.GetStringExtra(Constants.EXTRA_DIARY_ENTRY);
            var entry = JsonConvert.DeserializeObject<DiaryEntry>(entryJson);
            return entry;
        }

        private async Task LoadChart()
        {
            var colorGreen = new SkiaSharp.SKColor(0, 255, 0);
            var colorRed = new SkiaSharp.SKColor(255, 0, 0);

            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            {
                _diary = await _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-90));
            }
            else
            {
                _diary = await _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-14));
            }
            var entries = _diary.Select(entry =>
            {
                return new ChartEntry(entry.Sugar)
                {
                    Label = entry.DateOfReading.ToShortDateString(),
                    ValueLabel = entry.Sugar.ToString(),
                    Color = entry.Sugar > 120 ? colorRed : colorGreen
                };
            });

            _chart = new LineChart
            {
                Entries = entries,
                MinValue = GetLowestSugarReading(),
                MaxValue = GetHighestSugarReading()
            };
            _chartView.Chart = _chart;
        }

        private async Task LoadReadingHistory()
        {
            // todo: the start and end dates are ignored, currently.
            var entries = await _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-90));
            _diaryRecyclerViewAdapter = new DiaryRecyclerViewAdapter(entries.ToArray());
            _recyclerViewReadings.SetLayoutManager(new LinearLayoutManager(this));
            _recyclerViewReadings.SetAdapter(_diaryRecyclerViewAdapter);
            _diaryRecyclerViewAdapter.ItemLongClick += OnRecyclerViewReadings_LongClick;
        }

        private int GetHighestSugarReading()
        {
            if (_diary.Count < 1)
            {
                return 150; // TODO: magic number.
            }
            return _diary.Max(e => e.Sugar);
        }

        private int GetLowestSugarReading()
        {
            if (_diary.Count < 1)
            {
                return 50; // TODO: magic number.
            }
            return _diary.Min(e => e.Sugar > 0 ? e.Sugar : 50);
        }

        private void LoadA1CPrediction()
        {
            var a1c = new A1CCalculator();
            var estimate = a1c.Estimate(_diary.ToList());
            var value = $"Estimated A1C:   {estimate:0.0}";
            _textViewA1CPrediction.Text = value;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            StartActivityForResult(typeof(DiaryEntryActivity), Constants.REQUEST_CODE_ADD_ENTRY);
        }
    }
}