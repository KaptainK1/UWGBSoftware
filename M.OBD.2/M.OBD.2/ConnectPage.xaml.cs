﻿#region Using Statements
using System;
using M.OBD._2;
using M.OBD2;
using Xamarin.Forms;
#endregion

namespace M.OBD
{
    public partial class ConnectPage : ContentPage
    {
        #region Declarations

        private Bluetooth oBluetooth;
        private UserSetting oUserSetting;
        private bool isPickerActive;
        private bool isSelected;

        private enum STATE // Selection state enum
        {
            NONE,
            SELECT
        }
        private STATE State;

        #endregion

        #region Main Control

        public ConnectPage()
        {
            InitializeComponent();

            LoadUserSetting();
            InitControls();
        }

        private void ConnectPage_Appearing(object sender, EventArgs e)
        {
            SetSelectState(STATE.NONE);
        }

        private void LoadUserSetting()
        {
            oUserSetting = null;
            oUserSetting = new UserSetting();
        }

        private void InitControls()
        {
            Appearing += ConnectPage_Appearing;
            pkrDevices.SelectedIndexChanged += pkrDevices_SelectedIndexChanged;
            pkrDevices.Unfocused += pkrDevices_Unfocused;
            btnSelect.Clicked += btnSelect_Clicked;

            SetPickerSelection();
            SetSelectState(STATE.NONE);
        }

        #endregion

        #region Selection Related

        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            SetSelectState(STATE.SELECT);
        }

        private void SetSelectState(STATE state)
        {
            State = state;
            IsEnabled = false;

            if (State == STATE.SELECT && App.isBluetoothDisconnected() && !isPickerActive)
            {
                if (!CheckBluetooth())
                    return;

                btnSelect.IsEnabled = false;
                LoadPicker();
            }
            else
                btnSelect.IsEnabled = App.isBluetoothDisconnected();

            State = STATE.NONE;
            IsEnabled = true;
        }

        #endregion

        #region Picker Related

        private void SetPickerSelection()
        {
            // ToDo: set current user device selection
            bool isUserDevice = oUserSetting.isUserDevice();
        }

        private void LoadPicker()
        {
            isPickerActive = true;

            try
            {
                if (oBluetooth != null && oBluetooth.CheckConnection())
                    oBluetooth.CloseConnection();

                oBluetooth = null;
                oBluetooth = new Bluetooth(true, false);

                oBluetooth.LoadPairedDevices();

                if (oBluetooth.Count == 0)
                    throw new Exception("No devices found");

                pkrDevices.ItemsSource = oBluetooth;

                pkrDevices.Focus();
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }

        private void pkrDevices_Unfocused(object sender, FocusEventArgs e)
        {
            isPickerActive = false;
            SetSelectState(STATE.NONE);
        }

        private void pkrDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CheckPickerSelection()) return;
                SetPickerSelection(((Picker) sender).SelectedIndex);
                isSelected = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void SetPickerSelection(int index)
        {
            if (index == -1 || pkrDevices.Items.Count == 0) 
                return;

            BluetoothConnection oBc = (BluetoothConnection) pkrDevices.SelectedItem;

            if (oBc != null && oBluetooth != null)
            {
                CheckDevice(oBc);
            }
        }

        private bool CheckPickerSelection()
        {
            isSelected = !isSelected;
            return !isSelected;
        }

        #endregion

        #region Bluetooth

        private bool CheckBluetooth()
        {
            if (!Bluetooth.CheckAdapterPresent()) // Check if bluetooth is available on this device: display message and return on failure
            {
                DisplayMessage(Bluetooth.GetStatusMessage());
                return false;
            }

            if (!Bluetooth.CheckAdapterEnabled()) // Check if bluetooth is enabled on this device: display message and return on failure
            {
                // ToDo: open OS settings page?
                DisplayMessage(Bluetooth.GetStatusMessage());
                return false;
            }
            return true;
        }

        private async void CheckDevice(BluetoothConnection oBc)
        {
            if (oBluetooth.CheckConnection())
                oBluetooth.CloseConnection();

            lblState.Text = "Checking Device ...";

            bool isValid = await oBluetooth.OpenPairedDevice(oBc.device_name, oBc.device_address, false);

            lblState.Text = (isValid) ? "Device Connected" : "Invalid Device";
        }

        #endregion
        
        #region Misc

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            //TODO
        }

        private async void DisplayMessage(string message)
        {
            await DisplayAlert("Alert", message, "OK");
        }

        #endregion
    }
}