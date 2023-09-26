using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using System.Reflection;
using System.Drawing;
using WebApp.Panel.Codes;
using AppCommon;

#nullable disable

namespace WebApp.Panel.Controllers
{
    public class DosyalarController : BaseController
    {
        public DosyalarController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

        [HttpGet]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_R.")]
        public IActionResult ReadDirectoryList(string id)
        {
            DataSourceResult dsr = new();

            try
            {
                //thumbs dir
                string thumbsDirectoryPath = MyApp.Env?.WebRootPath + "\\" + MyApp.AppThumbsDirectory;
                if (!System.IO.Directory.Exists(thumbsDirectoryPath))
                {
                    var newDir = System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
                    newDir.Attributes = System.IO.FileAttributes.Hidden;
                }

                //files dir
                string rootDirectoryPath = MyApp.Env?.WebRootPath + "\\" + MyApp.AppFilesDirectory;
                if (!System.IO.Directory.Exists(rootDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(rootDirectoryPath);
                }

                string[] dirList = new string[] {
                    "Resimler",
                    "Dokumanlar"
                };

                foreach (var dir in dirList)
                {
                    string path = rootDirectoryPath + "\\" + dir;
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                }

                if (!string.IsNullOrEmpty(id))
                {
                    rootDirectoryPath = id;
                }

                var directories = new System.IO.DirectoryInfo(rootDirectoryPath).GetDirectories()
                    .Where(c => !c.Attributes.HasFlag(System.IO.FileAttributes.Hidden))
                    .Select(dir => new
                    {
                        id = dir.FullName,
                        hasChildren = new System.IO.DirectoryInfo(dir.FullName).GetDirectories().Where(c => !c.Attributes.HasFlag(System.IO.FileAttributes.Hidden)).Any(),
                        text = dir.Name,
                        expanded = false
                    });

                dsr.Data = directories;
                dsr.Total = directories.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }

            return Json(dsr);
        }

        [HttpGet]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_R.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
		public IActionResult ReadFilesInDirectory(string _DirectoryName)
        {
            DataSourceResult dsr = new();

            try
            {
                string filePath = MyApp.Env?.WebRootPath + "\\" + MyApp.AppFilesDirectory;

                if (!string.IsNullOrEmpty(_DirectoryName))
                {
                    filePath = _DirectoryName;
                }

                if (System.IO.Directory.Exists(filePath))
                {
                    List<MyFile> myFileList = new();
                    System.IO.DirectoryInfo directoryInfo = new(filePath);

                    //if (directoryInfo.Attributes != System.IO.FileAttributes.Hidden)
                    //{
                    System.IO.FileInfo[] files = directoryInfo
                        .EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                        .OrderBy(p => p.Name).ToArray();

                    foreach (System.IO.FileInfo file in files)
                    {
                        string fileId = file.DirectoryName?.Replace(MyApp.Env.WebRootPath, "") + "\\" + file.Name;
                        string fileUrl = fileId.Replace("\\", "/");
                        string fileVersion = "";

                        string thumbsRootDir = MyApp.Env?.WebRootPath + "\\" + MyApp.AppFilesDirectory;
                        string thumbFileName = file.FullName.Replace(MyApp.Env?.WebRootPath + "\\" + MyApp.AppFilesDirectory, MyApp.Env?.WebRootPath + "\\" + MyApp.AppThumbsDirectory);
                        string thumbUrl = thumbFileName.Replace(MyApp.Env.WebRootPath, "");

                        string extension = file.Extension.MyToLower().Replace(".", "");// ".xxx" => "xxx"
                        string[] imgExtensions = new string[] { "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "apng", "webp", "gif", "svg" };
                        string[] imgExtensionsForVersion = new string[] { "jpg", "jpeg", "png", "gif" };
                        string[] imgExtensionsForthumb = new string[] { "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "apng", "gif" }; //thumbu oluşacak olan dosyalar, webp nin kini oluşturamıyor boşa ekleme


                        if (!imgExtensions.Contains(extension))
                        {
                            thumbUrl = $"/img/file/{extension}.png";
                        }
                        else
                        if (!imgExtensionsForthumb.Contains(extension))
                        {
                            thumbUrl = fileUrl;
                        }
                        else
                        {
                            //thumb yok ise
                            //if (!System.IO.File.Exists(thumbFileName))
                            if (true) //her durumda yapsın dosya güncellemeleri başka bir kanaldan olunca thumb eski kalıyor
                            {
                                try
                                {
                                    
                                    using var originalImage = Image.FromFile(file.FullName);
                                    var newWidth = 100;
                                    using var resizedImage = originalImage.GetThumbnailImage(newWidth, newWidth * originalImage.Height / originalImage.Width, null, IntPtr.Zero);
                                    string thumbPath = System.IO.Path.GetDirectoryName(thumbFileName);
                                    if (!System.IO.Directory.Exists(thumbPath))
                                    {
                                        System.IO.Directory.CreateDirectory(thumbPath);
                                    }
                                    resizedImage.Save(thumbFileName);
                                }
                                catch { }
                            }
                        }

                        if (imgExtensionsForVersion.Contains(extension))
                        {
                            //version set ediliyor, image olanlara version set ediliyor
                            fileVersion = "?v." + file.LastWriteTime.Ticks.ToString();
                        }

                        myFileList.Add(new MyFile()
                        {
                            Id = fileId,
                            Name = file.Name,
                            Extension = file.Extension,
                            FileUrl = fileUrl.Replace("\\", "/"),
                            FileViewUrl = thumbUrl.Replace("\\", "/") + fileVersion,
                            FileVersion = fileVersion,
                            ModifiedDate = file.LastWriteTime,
                            Size = file.Length,
                            SizeText = (file.Length / 1024).MyToStr() + " KB"
                        });
                    }

                    dsr.Data = myFileList; // myFileList.Skip(skip).Take(take);
                    dsr.Total = myFileList.Count;
                    //}
                }
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_C.")]
        public IActionResult AddDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (!System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.Directory.CreateDirectory(_directoryName);

                    System.IO.DirectoryInfo di = new(_directoryName);
                    if (di.Parent.Name == "tr-TR" || di.Parent.Name == "en-US")
                    {
                        System.IO.Directory.CreateDirectory(_directoryName + "//Anlatim");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Klavuz");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Kodlar");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Tanitim");
                    }

                    rMessage += this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DizinEklendi");
                }
                else
                {
                    rMessage += this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.EklemekIstediginizDizinMevcut");
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_C.")]
        public IActionResult DownloadDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            string rUrl = "";
            try
            {
                if (System.IO.Directory.Exists(_directoryName))
                {
                    string targetDirectory = MyApp.Env?.WebRootPath + "\\temp";
                    if (!System.IO.Directory.Exists(targetDirectory))
                    {
                        System.IO.Directory.CreateDirectory(targetDirectory);
                    }
                    else
                    {
                        //burada belirli bir süre önce eklenen dosyalar varsa onlar temizlenebilir
                    }

                    DirectoryInfo di = new(_directoryName);

                    string zipDirectory = targetDirectory + "\\";
                    string zipFileName = di.Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".zip";

                    System.IO.Compression.ZipFile.CreateFromDirectory(_directoryName, targetDirectory + "\\" + zipFileName);

                    rUrl = "/" + "temp" + "/" + zipFileName;
                    rMessage = "The directory has been compressed and is being downloaded.";
                }
                else
                {
                    rMessage = "The directory you want to download is not found";
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage, sUrl = rUrl });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_D.")]
        public IActionResult DeleteDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.DirectoryInfo directoryInfo = new(_directoryName);
                    var files = directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);
                    if (!files.Any())
                    {
                        directoryInfo.Delete(false);
                        rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DizinSilindi");
                    }
                    else
                    {
                        rError = true;
                        rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.SilmekIstediginizDizinBosDegil");
                    }
                }
                else
                {
                    rError = true;
                    rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.SilmekIstediginizDizinBulunamadı");
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_D.")]
        public IActionResult ClearDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.DirectoryInfo directoryInfo = new(_directoryName);
                    var dirs = directoryInfo.GetDirectories("*.*", System.IO.SearchOption.TopDirectoryOnly);

                    if (!dirs.Any())
                    {
                        directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                            .ToList().ForEach(f => f.Delete());
                        rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DizinTemizlendi");
                    }
                    else
                    {
                        rError = true;
                        rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.TemizlemekIstediginizDizinIcindeDizinOlmamaz");
                    }
                }
                else
                {
                    rError = true;
                    rMessage = this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.TemizlemekIstediginizDizinBulunamadı");
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }


        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_C.")]
        //[RequestFormLimits(ValueLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        public IActionResult UploadFile(string _directoryName, string _fileName, string _fileContent)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                string imgFullFileName = _directoryName + "/" + _fileName;

                System.IO.File.WriteAllBytes(imgFullFileName, Convert.FromBase64String(_fileContent));

                #region .webp dosyasından png copyasını oluşturmak
                if (_fileName.MyToLower().EndsWith(".webp"))
                {
                    var outFileName = $"{Path.GetDirectoryName(imgFullFileName)}\\{Path.GetFileNameWithoutExtension(imgFullFileName)}.png";
                    
                    System.Diagnostics.ProcessStartInfo psi = new()
                    {
                        FileName = $@"{MyApp.Env.ContentRootPath}\dwebp.exe",
                        Arguments = $@" {imgFullFileName} -o {outFileName}"
                    };

                    using var exeProcess = System.Diagnostics.Process.Start(psi);
                    exeProcess.WaitForExit();
                    exeProcess.Kill();
                }
                #endregion

                rMessage += this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DosyaGonderildi");
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Dosyalar.D_D.")]
        public IActionResult RemoveFile(string _fileId)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                string fileFullName = MyApp.Env.WebRootPath + _fileId;

                //delete normal file
                if (System.IO.File.Exists(fileFullName))
                {
                    System.IO.File.Delete(fileFullName);
                    rMessage += this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DosyaSilindi");
                }
                else
                {
                    rError = true;
                    rMessage += this.business.repository.dataContext.TranslateTo("xLng.viewDosyalar.DosyaBulunamadi");
                }

                //delete thumb
                string thumbFileName = fileFullName.Replace(MyApp.Env.WebRootPath + "\\" + MyApp.AppFilesDirectory, MyApp.Env.WebRootPath + "\\" + MyApp.AppThumbsDirectory);

                if (System.IO.File.Exists(thumbFileName))
                {
                    System.IO.File.Delete(thumbFileName);
                }

            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { bError = rError, sMessage = rMessage });
        }

    }
}