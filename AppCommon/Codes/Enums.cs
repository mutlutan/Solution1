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
        Personnel = 21,
		Member = 31
	}

	public enum EnmClaimType //EnmAccessType
	{
		User,
		Member
	}

	#endregion

	#region Üye Enums
	public enum EnmUserStatus
	{
        Passive = 0,
        Active = 1,
        Blocked = 2,
		Deleted = 3
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

	#endregion
}
