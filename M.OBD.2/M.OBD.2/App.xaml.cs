﻿using System;
using M.OBD2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using M.OBD.2.Services;
//using M.OBD.2.Views;

namespace M.OBD._2
{
    public partial class App : Application
    {
        //string to hold the location of the sql-lite database
        //init to empty
        public static string Database = string.Empty;
        private static Bluetooth.BLUETOOTH_STATE Bluetooth_State;

        public App()
        {
            InitializeComponent();

            SetBluetoothState(Bluetooth.BLUETOOTH_STATE.DISCONNECTED);

            //setup navigation for the rest of the project
            //make the MainPage the first page that opens
            MainPage = new NavigationPage(new MainPage());
        }

        public App(string database)
        {
            InitializeComponent();


            MainPage = new NavigationPage(new MainPage());

            Database = database;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static Bluetooth.BLUETOOTH_STATE GetBluetoothState()
        {
            return Bluetooth_State;
        }

        public static void SetBluetoothState(Bluetooth.BLUETOOTH_STATE _Bluetooth_State)
        {
            Bluetooth_State = _Bluetooth_State;
        }

        public static bool isBluetoothConnected()
        {
            return Bluetooth_State == Bluetooth.BLUETOOTH_STATE.CONNECTED;
        }

        public static bool isBluetoothDisconnected()
        {
            return Bluetooth_State == Bluetooth.BLUETOOTH_STATE.DISCONNECTED;
        }
    }
}
