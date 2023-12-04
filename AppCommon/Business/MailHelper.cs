
using System;
using AppCommon.DataLayer.DataMain.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AppCommon.Business
{
	//gmail göndeririken hata alınıyor olabilir, Google hesabınızın dışarıdan harici bir uygulama tarafından kullanılmasına izin verilmiyor olmasıdır.
	//aşağıdaki linke girip devam et demek yeterli
	// https://accounts.google.com/DisplayUnlockCaptcha

	public enum EnmEmailPoolStatus
	{
		Waiting = 0, /*Gönderilecek, bekliyor*/
		Sending = 1, /*Gönderim başladı*/
		Sent = 2,    /*Gönderildi*/
		Error = 3,   /*Hata oluştu*/
		Cancel = 4   /*İptal edildi*/
	}

	public enum EnmMailSablon
	{
		SifreBildirim = 101,
		UyeOnayBildirim = 102,
		SifreSifirlamaBildirim = 103,
		JobHataBildirim = 104
	}

	public class MyMailAccount
	{
		public string Host { get; set; } = "";
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public string UserName { get; set; } = "";
		public string Password { get; set; } = "";
	}

	public class MailHelper : IDisposable
	{
		private readonly MainDataContext dataContext;


		public MailHelper(MainDataContext _dataContext)
		{
			dataContext = _dataContext;
		}

		public Parameter GetParameter()
		{
			Parameter rV = new();
			try
			{
				var data = dataContext.Parameter.AsNoTracking()
					.Where(c => c.Id == 1).FirstOrDefault();

				if (data != null)
				{
					rV = data;
				}
			}
			catch { }

			return rV;
		}


		public MyMailAccount GetDefaultAccount()
		{
			var parametre = this.GetParameter();
			return new MyMailAccount()
			{
				Host = parametre.EmailHost.MyToTrim(),
				Port = parametre.EmailPort,
				EnableSsl = parametre.EmailEnableSsl,
				UserName = parametre.EmailUserName.MyToTrim(),
				Password = parametre.EmailPassword.MyToDecryptPassword()
			};
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
		public MimeKit.MimeMessage MesajOlustur(string _fromAdress, string _fromDisplayName, List<string> _to, List<string> _cc, List<string> _bcc, string _subject, string _body)
		{
			var message = new MimeKit.MimeMessage() { };

			message.From.Add(new MimeKit.MailboxAddress(_fromDisplayName, _fromAdress));

			if (_to != null)
			{
				foreach (var item in _to)
				{
					foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
					{
						if (adres.MyToTrim().Length > 0)
						{
							message.To.Add(new MimeKit.MailboxAddress(adres, adres));
						}
					}
				}
			}

			if (_cc != null)
			{
				foreach (var item in _cc)
				{
					foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
					{
						if (adres.MyToTrim().Length > 0)
						{
							message.Cc.Add(new MimeKit.MailboxAddress(adres, adres));
						}
					}
				}
			}

			if (_bcc != null)
			{
				foreach (var item in _bcc)
				{
					foreach (var adres in item.MyToTrim().Split(new char[] { ',', ';' }))
					{
						if (adres.MyToTrim().Length > 0)
						{
							message.Bcc.Add(new MimeKit.MailboxAddress(adres, adres));
						}
					}
				}
			}

			message.Subject = _subject;
			message.Body = new MimeKit.BodyBuilder()
			{
				HtmlBody = _body
			}.ToMessageBody();

			return message;
		}

		public MoResponse<object> SendMailWithMailKit(MimeKit.MimeMessage message)
		{
			MoResponse<object> response = new();

			try
			{
				var smtpClient = new MailKit.Net.Smtp.SmtpClient();
				var mailAccount = GetDefaultAccount();

				//Host; smtp.yandex.com smtp-mail.outlook.com yandex.com
				//Port;  outlook.com:587 ssl false // gmail port : 465 ssl true
				smtpClient.Connect(mailAccount.Host, mailAccount.Port, mailAccount.EnableSsl);
				smtpClient.Authenticate(mailAccount.UserName, mailAccount.Password);
				smtpClient.Send(message);
				smtpClient.Disconnect(true);

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Message.Add(ex.MyLastInner().Message);
			}

			return response;
		}

		#region Sablon gönderici
		public bool SendMailForSablon(int _sablonId, List<string> _toMailList, Dictionary<string, string> _data)
		{
			bool rTF = false;

			_data.Add("[#DateTime.Today#]", DateTime.Today.Date.ToString("d"));
			_data.Add("[#DateTime.Now#]", DateTime.Now.ToString("g"));

			EmailTemplate postaSablon = new();
			var itemMailSablon = dataContext.EmailTemplate
				.Where(c => c.Id == _sablonId)
				.Include(i => i.EmailLetterhead)
				.FirstOrDefault();
			if (itemMailSablon != null)
			{
				postaSablon = itemMailSablon;
			}

			string antetIcerik = postaSablon.EmailLetterhead.Content.MyToTrim();
			string mailsubject = postaSablon.EmailSubject.MyToStr() + " " + DateTime.Now.ToString("g");
			string mailBody = postaSablon.EmailContent.MyToStr();

			//body replace
			foreach (var item in _data)
			{
				antetIcerik = antetIcerik.Replace(item.Key, item.Value.MyToStr());
				mailsubject = mailsubject.Replace(item.Key, item.Value.MyToStr());
				mailBody = mailBody.Replace(item.Key, item.Value.MyToStr());
			}

			// antet geçişi
			if (antetIcerik.Length > 0 && antetIcerik.Contains("[#AntetContent#]"))
			{
				mailBody = antetIcerik.Replace("[#AntetContent#]", mailBody);
			}

			//log ekle
			EmailPool modelMailHareket = new()
			{
				Id = dataContext.GetNextSequenceValue("sqMailHareket"),
				CreateDate = DateTime.Now,
				EmailTemplateId = _sablonId,
				EmailPoolStatusId = (int)EnmEmailPoolStatus.Waiting,
				TryQuantity = 1,
				LastTryDate = DateTime.Now,
				EmailTo = string.Join(",", _toMailList),
				EmailCc = postaSablon.EmailCc,
				EmailBcc = postaSablon.EmailBcc,
				EmailSubject = mailsubject,
				EmailContent = mailBody
			};
			dataContext.Add(modelMailHareket);
			dataContext.SaveChanges();

			//gönder
			try
			{
				var parametre = this.GetParameter();
				//mesaj oluştur
				var mesaj = MesajOlustur(
					parametre.EmailUserName.MyToStr(),
					parametre.EmailUserName.MyToStr(),
					_toMailList,
					postaSablon.EmailCc.MyToStr().Split(",").ToList(),
					postaSablon.EmailBcc.MyToStr().Split(",").ToList(),
					mailsubject,
					mailBody
				);

				rTF = SendMailWithMailKit(mesaj).Success;

				//log update
				modelMailHareket.LastTryDate = DateTime.Now;
				modelMailHareket.EmailPoolStatusId = (int)EnmEmailPoolStatus.Sent;
				dataContext.SaveChanges();
			}
			catch (Exception ex)
			{
				//log update
				modelMailHareket.LastTryDate = DateTime.Now;
				modelMailHareket.Description = ex.Message;
				modelMailHareket.EmailPoolStatusId = (int)EnmEmailPoolStatus.Error;
				dataContext.SaveChanges();

				// Exception devam etsin yola...
				throw new Exception(ex.MyLastInner().Message, ex);
			}

			return rTF;
		}
		#endregion


		#region MailHareket metodları : Kaydet, Gönder

		public MoResponse<int> MailHareketKaydet(int _sablonId, List<string> _toMailList, Dictionary<string, string> _data)
		{
			MoResponse<int> response = new();

			_data.Add("[#DateTime.Today#]", DateTime.Today.Date.ToString("d"));
			_data.Add("[#DateTime.Now#]", DateTime.Now.ToString("g"));

			var emailTemplate = dataContext.EmailTemplate
				.Where(c => c.Id == _sablonId)
				.Include(i => i.EmailLetterhead)
				.FirstOrDefault();

			string antetIcerik = emailTemplate?.EmailLetterhead.Content.MyToTrim() ?? "";
			string mailsubject = emailTemplate?.EmailSubject.MyToStr() ?? "" + " " + DateTime.Now.ToString("g");
			string mailBody = emailTemplate?.EmailContent.MyToStr() ?? "";

			//body replace
			foreach (var item in _data)
			{
				antetIcerik = antetIcerik.Replace(item.Key, item.Value.MyToStr());
				mailsubject = mailsubject.Replace(item.Key, item.Value.MyToStr());
				mailBody = mailBody.Replace(item.Key, item.Value.MyToStr());
			}

			// antet geçişi
			if (antetIcerik.Length > 0 && antetIcerik.Contains("[#AntetContent#]"))
			{
				mailBody = antetIcerik.Replace("[#AntetContent#]", mailBody);
			}

			//log ekle
			EmailPool emailPool = new()
			{
				Id = dataContext.GetNextSequenceValue("sqEmailPool"),
				CreateDate = DateTime.Now,
				EmailTemplateId = _sablonId,
				EmailPoolStatusId = (int)EnmEmailPoolStatus.Waiting,
				TryQuantity = 0,
				EmailTo = string.Join(",", _toMailList),
				EmailCc = emailTemplate?.EmailCc ?? "",
				EmailBcc = emailTemplate?.EmailBcc ?? "",
				EmailSubject = mailsubject,
				EmailContent = mailBody
			};
			dataContext.Add(emailPool);

			dataContext.SaveChanges();

			response.Data = emailPool.Id;

			return response;
		}

		public MoResponse<object> SendMailForMailHareket(int mailHareketId)
		{
			MoResponse<object> response = new();

			//gönderildi ise gönderili statüsene çeker kaydı
			//gönderilemedi ise hata statüsüne çeker, hata sayısını 1 artırır

			var emailPool = dataContext.EmailPool.Where(c => c.Id == mailHareketId).FirstOrDefault();
			if (emailPool != null)
			{
				var parametre = this.GetParameter();
				//mesaj oluştur
				var mesaj = MesajOlustur(
					_fromAdress: parametre.EmailUserName.MyToStr(),
					_fromDisplayName: parametre.EmailUserName.MyToStr(),
					_to: emailPool.EmailTo.MyToStr().Split(",").ToList(),
					_cc: emailPool.EmailCc.MyToStr().Split(",").ToList(),
					_bcc: emailPool.EmailBcc.MyToStr().Split(",").ToList(),
					_subject: emailPool.EmailSubject.MyToStr(),
					_body: emailPool.EmailContent.MyToStr()
				);

				response = SendMailWithMailKit(mesaj);
				emailPool.LastTryDate = DateTime.Now;
				emailPool.TryQuantity += 1;

				if (response.Success)
				{
					emailPool.EmailPoolStatusId = (int)EnmEmailPoolStatus.Sent;
				}
				else
				{
					emailPool.LastTryDate = DateTime.Now;
					emailPool.Description = string.Join(' ', response.Message);
					emailPool.EmailPoolStatusId = (int)EnmEmailPoolStatus.Error;
				}
				dataContext.SaveChanges();
			}

			return response;
		}

		#endregion

		#region varsayılan gönderimler - önce kayıt eder sonra gönderim sağlar

		public bool SendMail_SifreBildirim(string toMail, string yeniSifre)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#Kullanici_Ad#]", toMail.MyToStr());
			data.Add("[#Kullanici_Sifre#]", yeniSifre);

			var to = new List<string>() { toMail.MyToStr() };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_UyeMailOnay(string toMail, string adSoyad, string onayLinkValue, string onayLinkText)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#AdSoyad#]", adSoyad);
			data.Add("[#OnayLinkValue#]", onayLinkValue);
			data.Add("[#OnayLinkText#]", onayLinkText);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.UyeOnayBildirim, to, data).Data;
			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_SifreSifirlamaBildirim(string toMail, string adSoyad, string linkValue, string linkText)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#AdSoyad#]", adSoyad);
			data.Add("[#LinkValue#]", linkValue);
			data.Add("[#LinkText#]", linkText);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_JobHataBildirim(string toMail, string islemAdi, string aciklama)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", DateTime.Now.ToString("G"));
			data.Add("[#IslemAdi#]", islemAdi);
			data.Add("[#Aciklama#]", aciklama);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.JobHataBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_UyeOdemeBildirim(string toMail, string adSoyad, DateTime islemZamani, decimal yatirimTutari, decimal bakiye)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", islemZamani.ToString("G"));
			data.Add("[#AdSoyad#]", adSoyad);
			data.Add("[#YatirimTutari#]", yatirimTutari.ToString("F"));
			data.Add("[#Bakiye#]", bakiye.ToString("F"));

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}
		public bool SendMail_UyeSurusBildirim(string toMail, string adSoyad, string aracAd, DateTime baslangicTarih, DateTime bitisTarih, int sure, decimal tutar, decimal bakiye)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#AdSoyad#]", adSoyad);
			data.Add("[#AracAd#]", aracAd);
			data.Add("[#BaslangicTarih#]", String.Format("{0:U}", baslangicTarih));
			data.Add("[#BitisTarih#]", String.Format("{0:U}", bitisTarih));
			data.Add("[#ToplamSure#]", sure.ToString());
			data.Add("[#Tutar#]", tutar.ToString("F"));
			data.Add("[#Bakiye#]", bakiye.ToString("F"));

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_UyeAracYasakliBolgeBildirim(string toMail, string adSoyad, string aracAd, string yasakliBolgeAd, DateTime tarih)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", tarih.ToString("G"));
			data.Add("[#AdSoyad#]", adSoyad);
			data.Add("[#AracAd#]", aracAd);
			data.Add("[#YasakliBolgeAd#]", yasakliBolgeAd);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_AracSarjBildirim(string toMail, string imeiNo, string aracAd, string kullanimDurumu, decimal sarjOrani)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", DateTime.Now.ToString("G"));
			data.Add("[#ImeiNo#]", imeiNo);
			data.Add("[#AracAd#]", aracAd);
			data.Add("[#KullanimDurumu#]", kullanimDurumu);
			data.Add("[#SarjOrani#]", sarjOrani.ToString());

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_AracYasakliBolgeBildirim(string toMail, string imeiNo, string aracAd, string yasakliBolgeAd, DateTime tarih)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", tarih.ToString("G"));
			data.Add("[#ImeiNo#]", imeiNo);
			data.Add("[#AracAd#]", aracAd);
			data.Add("[#YasakliBolgeAd#]", yasakliBolgeAd);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}

		public bool SendMail_AracHataliKullanimBildirim(string toMail, string imeiNo, string aracAd, string aciklama, DateTime tarih)
		{
			Dictionary<string, string> data = new() { };
			data.Add("[#IslemZamani#]", tarih.ToString("G"));
			data.Add("[#ImeiNo#]", imeiNo);
			data.Add("[#AracAd#]", aracAd);
			data.Add("[#Aciklama#]", aciklama);

			var to = new List<string>() { toMail };

			var mailHareketId = MailHareketKaydet((int)EnmMailSablon.SifreSifirlamaBildirim, to, data).Data;

			return SendMailForMailHareket(mailHareketId).Success;
		}


		#endregion

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

	}

}
