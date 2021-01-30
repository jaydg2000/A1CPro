using A1CPro.Data;
using A1CPro.Domain;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Microcharts;
using Microcharts.Droid;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            _chartView = FindViewById<ChartView>(Resource.Id.chartViewSugarChart);
            _recyclerViewReadings = FindViewById<RecyclerView>(Resource.Id.recyclerViewHistory);
            _repo = new DiaryRepository();
            LoadChart();
            LoadReadingHistory();
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
                MinValue = 70,
                MaxValue = 250
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
    }
}