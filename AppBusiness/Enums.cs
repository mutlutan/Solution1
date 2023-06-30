using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBusiness
{
    #region Araç Dijital Data Type Enumları
    public enum EnmDijitalDataType
    {
        // Kablo Kilit Bağlantısı. Error, True meaning=OK
        [Description("Cable-Lock-Connection")]
        CableLockConnection,

        // Harici Güç. False Meaning=No, True meaning=Yes
        [Description("External-Power")]
        ExternalPower,

        // GPS Durumu. False Meaning=Invalid, True meaning=Valid
        [Description("GPS-State")]
        GpsState,

        // Hub Kilit Sinyali Hatası. False Meaning=OK, True meaning=Error
        [Description("Hub-Lock-Signal-Error")]
        HubLockSignalError,

        // Ignition. False Meaning=Off, True meaning=On
        [Description("Ignition")]
        Ignition,

        // Düşük Pil. False Meaning=No, True meaning=Yes
        [Description("Low-Battery")]
        LowBattery,

        // On-side. False Meaning=Normal, True meaning=Yes
        [Description("On-Side")]
        OnSide,

        // Tamper Alarm. False Meaning=No, True meaning=Yes
        [Description("Tamper-Alarm")]
        TamperAlarm,

        // Trip Start. False Meaning=No, True meaning=Yes
        [Description("Trip-Start")]
        TripStart,

        // Trip End. False Meaning=No, True meaning=Yes
        [Description("Trip-End")]
        TripEnd,

        // Yetkisiz sürücü. False Meaning=No, True meaning=Yes
        [Description("Unauthorised-Driver")]
        UnauthorisedDriver,

        // Araç şarjı. False Meaning=No, True meaning=Yes
        [Description("Vehicle-Charging")]
        VehicleCharging

    }
    #endregion

    #region Araç Analog Data Type Enumları
    public enum EnmAnalogDataType
    {
        // Rakım. (metre)
        [Description("Altitude")]
        Altitude,

        // Pil Seviyesi (%)
        [Description("Battery")]
        Battery,

        // Batarya Voltajı (V)
        [Description("Battery-Voltage")]
        BatteryVoltage,

        // Elektrikli Araç Bataryası (%)
        [Description("Electric-Vehicle-Battery")]
        ElectricVehicleBattery,

        // Elektrikli Araç Akü Akımı (A)
        [Description("Electric-Vehicle-Battery-Current")]
        ElectricVehicleBatteryCurrent,

        // Elektrikli Araç Akü Voltajı (V)
        [Description("Electric-Vehicle-Battery-Voltage")]
        ElectricVehicleBatteryVoltage,

        // Etkinlik Kodu
        [Description("Event-Code")]
        EventCode,

        // GSM Sinyal Gücü
        [Description("Gsm-Signal-Strength")]
        GsmSignalStrength,

        // Rota
        [Description("Heading")]
        Heading,

        // İç Sıcaklık (°C)
        [Description("Internal-Temperature")]
        InternalTemperature,

        // Kilometre sayacı (km)
        [Description("Odometer-Vehicle")]
        OdometerVehicle,

        // Uydu Sayısı
        [Description("Satellite-Count")]
        SatelliteCount,

        // Hız (km/h)
        [Description("Speed")]
        Speed,

        // Yolculuk Mesafesi (km)
        [Description("Trip-Distance")]
        TripDistance,

        // Tekerlek Tabanlı Hız (km/h)
        [Description("Wheel-Based-Speed")]
        WheelBasedSpeed

    }
    #endregion

    #region Arac Text Data Type Enumları
    public enum EnmTextDataType
    {
        // GSM Hücre Kimliği
        [Description("Gsm-Cell-Id")]
        GsmCellId,

        // GSM Konum Alan Kodu
        [Description("Gsm-Location-Area-Code")]
        GsmLocationAreaCode,

        // GSM Mobil Ülke Kodu
        [Description("Gsm-Mobile-Country-Code")]
        GsmMobileCountryCode,

        // GSM Mobil Şebeke Kodu
        [Description("Gsm-Mobile-Network-Code")]
        GsmMobileNetworkCode
    }
    #endregion
}
