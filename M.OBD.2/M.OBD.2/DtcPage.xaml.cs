﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M.OBD._2;
using M.OBD2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace M.OBD
{

    public partial class DtcPage : ContentPage, IPageLoad
    {
        #region Declarations

        private readonly Bluetooth oBluetooth;
        private UserSetting oUserSetting;

        #endregion

        #region Page Initialization and Update

        public DtcPage()
        {
            InitializeComponent();

            InitBluetooth(out oBluetooth);
            InitUserSettings();
            InitControls();
        }

        public void InitBluetooth(out Bluetooth bluetooth)
        {
            bluetooth = App.GetBluetooth();
        }

        public void InitControls()
        {
            Appearing += Page_Appearing;
            btnClear.Clicked += btnClear_Clicked;
        }

        public void InitUserSettings()
        {
            oUserSetting = null;
            oUserSetting = new UserSetting();
        }

        public void Page_Appearing(object sender, EventArgs e)
        {
            UpdateControls();
            UpdateUserSettings();
        }

        public void UpdateControls()
        {
            btnClear.IsEnabled = Bluetooth.isBluetoothDisconnected();
        }

        public void UpdateUserSettings()
        {

        }

        #endregion

        private void btnClear_Clicked(object sender, EventArgs e)
        {
        }
    }
}