using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon
{
    #region genel enums
    public enum EnmLogTur
    {
        Hata = 11,
        Genel = 12,
        Istek = 13,
        Middleware = 14,
        Uyari = 15
    }

    public enum EnmYetkiGrup
    {
        Admin = 11,
        Personel = 21,
        Uye = 31
    }

    public enum EnmClaimType
    {
        User,
        Member
    }

    #endregion

    #region Üye Enums
    public enum EnmUyeDurum
    {
        Pasif = 0,
        Aktif = 1,
        Bloke = 2,
        Deleted = 3
    }
    public enum EnmUyeSmsTur
    {
        Login = 1,
        Register = 2,
        NewPassword = 3
    }

    public enum EnmUyeSurusGecmisi
    {
        TumZaman = 0,
        BirHafta = 1,
        BirAy = 2,
        UcAy = 3,
        AltiAy = 4,
        BirYil = 5
    }
    public enum EnmUyeCuzdanHareketTur
    {
        KarttanYukleme = 1,
        Odeme = 2,
        Iade = 3,
        CaridenIade = 4
    }

    public enum EnmUyeCariHareketTur
    {
        KarttanYukleme = 1,
        CuzdandanOdeme = 2,
        CuzdanaIade = 3,
        Tahakkuk = 4
    }
    public enum EnmVehicleStation
    {
        [Description("vehicle")]
        Vehicle,

        [Description("station")]
        Station
    }
    public enum EnmAracRezervasyonDurum
    {
        Rezerve = 11,
        Kullanildi = 12,
        Kullanilmadi = 13,
        IptalEdildi = 14
    }

    #endregion
}
