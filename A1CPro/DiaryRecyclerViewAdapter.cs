using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using A1CPro.Domain;

namespace A1CPro
{
    public class DiaryRecyclerViewAdapter : RecyclerView.Adapter
    {
        private Android.Graphics.Color _goodColor = Android.Graphics.Color.DarkGreen;
        private Android.Graphics.Color _badColor = Android.Graphics.Color.Red;
        private Android.Graphics.Color _blackColor = Android.Graphics.Color.Black;

        public event EventHandler<DiaryRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<DiaryRecyclerViewAdapterClickEventArgs> ItemLongClick;
        DiaryEntry[] items;

        public DiaryRecyclerViewAdapter(DiaryEntry[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.diary_entry_item;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new DiaryRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];
            bool hasBloodPressure = item.BloodPressure != null;
            bool hasWeight = item.Weight > 0;
            bool hasSugarReading = item.Sugar > 0;
            
            var holder = viewHolder as DiaryRecyclerViewAdapterViewHolder;
            holder.textViewDateOfReading.Text = item.DateOfReading.ToShortDateString();
            holder.textViewMorningReading.Text = hasSugarReading ? item.Sugar.ToString() : "--";
            holder.textViewBloodPressure.Text = hasBloodPressure ? item.BloodPressure.ToString() : "--";
            holder.textViewWeight.Text = hasWeight ? item.Weight.ToString() : "--";
            holder.textViewMorningMedsTaken.Text = item.MorningMedsTaken ? "Yes" : "No";
            holder.textViewEveningMedsTaken.Text = item.EveningMedsTaken ? "Yes" : "No";

            bool isSugarReadingGood = item.Sugar <= 120;
            bool isBloodPressureGood = hasBloodPressure ? (item.BloodPressure.SystolicBloodPressue <= 120) && (item.BloodPressure.DiastolicBloodPressure <= 80) : false;
            bool isWeightGood = IsWeightGood(item, position);

            holder.textViewMorningReading.SetTextColor(GetCorrectColor(hasSugarReading, isSugarReadingGood));
            holder.textViewMorningMedsTaken.SetTextColor(item.MorningMedsTaken ? _goodColor : _badColor);
            holder.textViewEveningMedsTaken.SetTextColor(item.EveningMedsTaken ? _goodColor : _badColor);
            holder.textViewBloodPressure.SetTextColor(GetCorrectColor(hasBloodPressure, isBloodPressureGood));
            holder.textViewWeight.SetTextColor(GetCorrectColor(hasWeight, isWeightGood));
        }

        private Android.Graphics.Color GetCorrectColor(bool hasData, bool isGood)
        {
            if (!hasData)
            {
                return _blackColor;
            }
            return isGood ? _goodColor : _badColor;
        }

        private bool IsWeightGood(DiaryEntry item, int position)
        {
            var isWeightGood = true;
            if (position < items.Length-1)
            {
                var todaysWeight = item.Weight;
                var lastRecordedWeight = FindPreviousRecordedWeight(position+1);
                isWeightGood = todaysWeight <= lastRecordedWeight;
            }
            return isWeightGood;
        }

        private double FindPreviousRecordedWeight(int position)
        {
            int maxIndex = items.Length-1;

            if (position >= maxIndex)
                return items[position].Weight;

            while(position < maxIndex)
            {
                if (items[position].Weight > 0)
                {
                    return items[position].Weight;
                }
                position++;
            }
            return 0;
        }

        public override int ItemCount => items.Length;

        void OnClick(DiaryRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DiaryRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class DiaryRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView textViewDateOfReading { get; set; }
        public TextView textViewMorningReading { get; set; }
        public TextView textViewBloodPressure { get; set; }
        public TextView textViewWeight { get; set; }
        public TextView textViewMorningMedsTaken { get; set; }
        public TextView textViewEveningMedsTaken { get; set; }

        public DiaryRecyclerViewAdapterViewHolder(View itemView, Action<DiaryRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<DiaryRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            textViewDateOfReading = itemView.FindViewById<TextView>(Resource.Id.textViewDate);
            textViewMorningReading = itemView.FindViewById<TextView>(Resource.Id.textViewMorningReading);
            textViewBloodPressure = itemView.FindViewById<TextView>(Resource.Id.textViewBloodPressure);
            textViewWeight = itemView.FindViewById<TextView>(Resource.Id.textViewWeight);
            textViewMorningMedsTaken = itemView.FindViewById<TextView>(Resource.Id.textViewMorningMeds);
            textViewEveningMedsTaken = itemView.FindViewById<TextView>(Resource.Id.textViewEveningMeds);

            itemView.Click += (sender, e) => clickListener(new DiaryRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DiaryRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DiaryRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}