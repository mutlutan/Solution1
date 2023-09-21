using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json;
using AppCommon;
using AppCommon.DataLayer.DataMain.Models;

#nullable disable

namespace WebApp.Portal.Codes
{
    public class MyApp
	{
		public static IWebHostEnvironment Env { get; set; } = null;
		public static string SupportedCultures { get; set; } = "tr-TR,en-US";

		#region AppDirectory
		public static string AppClientDirectory { get; set; } = "client";
		public static string AppComponentsDirectory { get; set; } = "components";
		public static string GetAppComponentsPath()
		{
			return MyApp.Env?.WebRootPath + "\\" + MyApp.AppClientDirectory + "\\" + MyApp.AppComponentsDirectory;
		}

		#endregion

		#region files
		public static string AppFilesDirectory { get; set; } = "Data\\Files";
		public static string AppThumbsDirectory { get; set; } = "Data\\Thumbs";
		#endregion

		#region app Version
		public static string AppName1 { get; set; } = "Solution1";
		public static string AppName2 { get; set; } = "Portal";
		public static string AppName
		{
			get
			{
				string rS = MyApp.AppName1;
				if (!string.IsNullOrEmpty(MyApp.AppName2))
				{
					rS += " " + MyApp.AppName2;
				}
				return rS;
			}
		}

		public static string Version
		{
			get
			{
				string Version = "1.0.0.01";
				if (MyApp.Env?.EnvironmentName == "Development")
				{
					Version += "_";
					Version += DateTime.Now.ToString("MMddHHmmss");
				}
				return Version;
			}
		}
		#endregion

		#region Load dictionary 
		public static Dictionary<string, Dictionary<string, string>> AppDictionary { get; set; } = new Dictionary<string, Dictionary<string, string>>();

		public static Dictionary<string, Dictionary<string, string>> GetDictionary()
		{
			if (MyApp.AppDictionary.Count == 0)
			{
				var sozluk = new Dictionary<string, Dictionary<string, string>>();
				var fileList = new List<string>();

				var dirDictionary = new System.IO.DirectoryInfo(MyApp.Env?.WebRootPath + "\\client\\dictionary");
				IEnumerable<System.IO.FileInfo> filesDictionary = dirDictionary.EnumerateFiles("*" + ".json", SearchOption.TopDirectoryOnly);
				fileList.AddRange(filesDictionary.Select(s => s.FullName));

				var dirViewDictionary = new System.IO.DirectoryInfo(MyApp.Env?.WebRootPath + "\\client\\views");
				IEnumerable<System.IO.FileInfo> filesViewDictionary = dirViewDictionary.EnumerateFiles("*" + ".json", SearchOption.AllDirectories);
				fileList.AddRange(filesViewDictionary.Select(s => s.FullName));

				foreach (var filePath in fileList)
				{
					if (System.IO.File.Exists(filePath))
					{
						string jsonString = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
						if (!string.IsNullOrEmpty(jsonString))
						{
							var dynamicObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
							if (dynamicObject != null)
							{
								if (dynamicObject.Count > 0)
								{
									foreach (var itemObject in dynamicObject)
									{
										var itemDict = new Dictionary<string, string>();
										foreach (var item in itemObject.value)
										{
											itemDict.Add(item.Name, item.Value.Value);
										}
										sozluk[itemObject.key.Value] = itemDict;
									}
								}
							}
						}
					}
				}

				AppDictionary = sozluk;
			}

			return AppDictionary;
		}
		#endregion

		#region load Authority
		public static List<string> AuthorityJsonTextList { get; set; } = new();
		public static List<string> GetAuthorityJsonTextList()
		{
			if (MyApp.AuthorityJsonTextList.Count == 0)
			{
				var dir = new System.IO.DirectoryInfo(MyApp.Env?.WebRootPath + "\\client\\authority");
				IEnumerable<System.IO.FileInfo> files = dir.EnumerateFiles("*" + ".js", SearchOption.TopDirectoryOnly);

				foreach (var file in files)
				{
					if (System.IO.File.Exists(file.FullName))
					{
						var authorityJsonLines = System.IO.File.ReadAllLines(file.FullName, System.Text.Encoding.UTF8);
						var query = authorityJsonLines
							.Where(c => c.StartsWith("window.AppAuthority") == false)
							.Where(c => c.Trim().StartsWith("//") == false);

						var authorityJsonText = string.Join(Environment.NewLine, query.ToArray());
						if (!string.IsNullOrEmpty(authorityJsonText))
						{
							MyApp.AuthorityJsonTextList.Add(authorityJsonText);
						}
					}
				}
			}
			return MyApp.AuthorityJsonTextList;
		}

		public static List<string> AuthorityJsonLangKeyList { get; set; } = new();

		public static List<string> GetAuthorityJsonLangKeyList()
		{
			if (MyApp.AuthorityJsonLangKeyList.Count == 0)
			{
				foreach (var authorityJsonText in GetAuthorityJsonTextList())
				{
					foreach (var s in authorityJsonText.Split("mnLang.f"))
					{
						if (s.StartsWith("(\""))
						{
							var indx = s.IndexOf("\")");
							if (indx >= 0)
							{
								string key = s[2..indx];
								MyApp.AuthorityJsonLangKeyList.Add(key);
							}
						}
					}
				}
			}

			return MyApp.AuthorityJsonLangKeyList;
		}

		public static MoResponse<MoAuthority> GetAuthorityTemplate(MainDataContext dataContext)
		{
			MoResponse<MoAuthority> response = new();

			foreach (string jsText in MyApp.GetAuthorityJsonTextList())
			{
				var jsonText = jsText.Replace("window.AppAuthority =", "");
				var indx = jsonText.LastIndexOf(";");
				jsonText = jsonText[..indx];

				foreach (var keyText in MyApp.GetAuthorityJsonLangKeyList())
				{
					string oldText = $"mnLang.f(\"{keyText}\")";
					string trnsText = "\"" + dataContext.TranslateTo(keyText) + "\"";
					jsonText = jsonText.Replace(oldText, trnsText);
				}

				response.Data = JsonConvert.DeserializeObject<MoAuthority>(jsonText);
				response.Success = true;
			}

			return response;
		}

		#endregion


	}
}
