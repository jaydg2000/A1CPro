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
        private FloatingActionButton _fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            _fab = FindViewById<FloatingActionButton>(Resource.Id.fabAddEdit);
            _fab.Click += Fab_Click;
            _chartView = FindViewById<ChartView>(Resource.Id.chartViewSugarChart);
            _recyclerViewReadings = FindViewById<RecyclerView>(Resource.Id.recyclerViewHistory);
            _repo = new DiaryRepository();
            LoadChart();
            LoadReadingHistory();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == Constants.REQUEST_CODE_ADD_ENTRY)
            {
                if (resultCode == Result.Ok)
                {
                    var entry = GetDiaryEntryFromIntent(data);
                    if (entry != null)
                    {
                        _repo.SaveDiaryEntry(entry);
                        LoadChart();
                        LoadReadingHistory();

                        Toast.MakeText(this, "Entry saved.", ToastLength.Short);
                        return;
                    }
                    Toast.MakeText(this, "Entry NOT saved.", ToastLength.Short);
                }
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

        private void LoadChart()
        {
            var colorGreen = new SkiaSharp.SKColor(0, 255, 0);
            var colorRed = new SkiaSharp.SKColor(255, 0, 0);

            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            {
                _diary = _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-90));
            }
            else
            {
                _diary = _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-14));
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

        private void LoadReadingHistory()
        {
            var entries = _repo.GetDiary(DateTime.Now, DateTime.Now.AddDays(-90));
            _diaryRecyclerViewAdapter = new DiaryRecyclerViewAdapter(entries.ToArray());
            _recyclerViewReadings.SetLayoutManager(new LinearLayoutManager(this));
            _recyclerViewReadings.SetAdapter(_diaryRecyclerViewAdapter);
        }

        private int GetHighestSugarReading()
        {
            return _diary.Max(e => e.Sugar);
        }

        private int GetLowestSugarReading()
        {
            return _diary.Min(e => e.Sugar > 0 ? e.Sugar : 50);
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            StartActivityForResult(typeof(DiaryEntryActivity), Constants.REQUEST_CODE_ADD_ENTRY);
        }

    }
}