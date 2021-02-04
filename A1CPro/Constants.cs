using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace A1CPro
{
    public static class Constants
    {
        public static int REQUEST_CODE_ADD_ENTRY = 100;
        public static int REQUEST_CODE_MODIFY_ENTRY = 101;

        public static string EXTRA_DIARY_ENTRY = "diary-entry";
        public static string EXTRA_DIARY_ENTRY_ID = "diary-entry-id";
        public static string EXTRA_DIARY_ENTRY_SUGAR = "diary-entry-sugar";
        public static string EXTRA_DIARY_ENTRY_DATE_OF_READING = "diary-entry-date-of-reading";
        public static string EXTRA_DIARY_ENTRY_BLOOD_PRESSURE_SYSTOLIC = "diary-entry-blood-pressure-systolic";
        public static string EXTRA_DIARY_ENTRY_BLOOD_PRESSURE_DIASTOLIC = "diary-entry-blood-pressure-diastolic";
        public static string EXTRA_DIARY_ENTRY_WEIGHT = "diary-entry-weight";
        public static string EXTRA_DIARY_ENTRY_MEDS_TAKEN_AM = "diary-entry-meds-taken-am";
        public static string EXTRA_DIARY_ENTRY_MEDS_TAKEN_PM = "diary-entry-meds-taken-pm";
    }
}