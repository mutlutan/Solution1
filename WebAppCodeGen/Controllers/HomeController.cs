using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebApp1.Codes;
using WebApp1.Models;
using WebAppCodeGen.Models;

namespace WebApp1.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }


        public IActionResult Index()
        {
            ViewBag.Solutions = new MySolution().Solutions;
            
            return View();
        }

        public IActionResult TableList(string solutionName)
        {
            var myCodeGen = new MyCodeGen(solutionName);

            var myCodeGenInfoList = new List<MyCodeGenInfo>();
            foreach (var table in myCodeGen.GetTablesOrViews())
            {
                var tableOptions = myCodeGen.GetTableOptions(table.Name);

                myCodeGenInfoList.Add(
                    new MyCodeGenInfo
                    {
                        TableName = table.Name,
                        TableOptions = tableOptions,
                        DataTransferObjects = myCodeGen.GetDataTransferObjects(table.Name),
                        DataManipulationObjects = myCodeGen.GetDataManipulationObjects(table.Name),
                        Controllers = myCodeGen.GetControllers(table.Name),
                        Dictionaries = myCodeGen.GetDictionaries(table.Name),
                        FormViews = myCodeGen.GetFormViews(table.Name),
                        GridViews = myCodeGen.GetGridViews(table.Name),
                        TreeLists = myCodeGen.GetTreeLists(table.Name),
                        SearchViews = myCodeGen.GetSearchViews(table.Name)
                    }
                );
            }

            ViewBag.MyCodeGenInfoList = myCodeGenInfoList;
            ViewBag.SolutionName = solutionName;

			return View();
        }



        #region ReadLocks
        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult ReadLocks(string _solutionName, string _TableName, string _TableOptionName)
        {
            Boolean rError = false;
            string rMessage = "";
            var rLockList = new List<string>();
            try
            {
                var props = new[] {
                    "DataTransferObjectLock",
                    "DataManipulationObjectLock",
                    "ControllerLock",
                    "DictionaryLock",
                    "FormViewLock",
                    "GridViewLock",
                    "TreeListLock",
                    "SearchViewLock"
                };


                var myCodeGen = new MyCodeGen(_solutionName);
                var tableOption = myCodeGen.FnTableOptionRead(_TableName, _TableOptionName);

                foreach (var prop in props)
                {
                    var p = typeof(MyTableOption).GetProperty(prop);
                    if (p != null)
                    {
                        object? val = p.GetValue(tableOption);
                        if (val != null)
                        {
                            if ((Boolean)val == true)
                            {
                                rLockList.Add(prop);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }
            return Json(new
            {
                bError = rError,
                sMessage = rMessage,
                oLockList = rLockList
            });
        }
        #endregion


        #region translator
        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TranslateWithApi(string _text, string _target, string _source, Boolean _titleCase)
        {
            Boolean rError = false;
            string rMessage = "";
            string rTranslatedText = "";
            try
            {
                rTranslatedText = MyApp.TranslateWithApi(_text, _target, _source, _titleCase);
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage, sTranslatedText = rTranslatedText });
        }
        #endregion

        #region lookup
        public IActionResult GetColumns(string _solutionName, string _TableName)
        {
            var rList = new List<string>();
            if (_TableName != null)
            {
                var myCodeGen = new MyCodeGen(_solutionName);
                rList = myCodeGen.GetTableOrViewColumns(_TableName).Select(s => s.Name).ToList();
            }

            return Json(new { data = rList });
        }
        #endregion

        #region Table Options

        public IActionResult TableOption(string _solutionName, string _TableName, string _TableOptionName)
        {
            ViewBag.SolutionName = _solutionName;
            ViewBag.TableName = _TableName;
            ViewBag.TableOptionName = _TableOptionName;
            if (_TableName == _TableOptionName)
            {
                ViewBag.TableOptionName = _TableOptionName + "_";
            }

            var myCodeGen = new MyCodeGen(_solutionName);

            List<string> tableAndViews = myCodeGen.GetTablesOrViews().Select(s => s.Name).ToList();
            ViewBag.tableList = tableAndViews;

            ViewBag.DataTransferObjectCount = myCodeGen.GetDataTransferObjects(_TableName).Count;
            ViewBag.DataManipulationObjectCount = myCodeGen.GetDataManipulationObjects(_TableName).Count;
            ViewBag.ControllerCount = myCodeGen.GetControllers(_TableName).Count;
            ViewBag.DictionaryCount = myCodeGen.GetDictionaries(_TableName).Count;
            ViewBag.FormViewCount = myCodeGen.GetFormViews(_TableName).Count;
            ViewBag.GridViewCount = myCodeGen.GetGridViews(_TableName).Count;
            ViewBag.TreeListCount = myCodeGen.GetTreeLists(_TableName).Count;
            ViewBag.SearchViewCount = myCodeGen.GetSearchViews(_TableName).Count;

            return View();
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TableOptionRead(string _solutionName, string _TableName, string _TableOptionName)
        {
			Boolean rError = false;
            string rMessage = "";
            bool rExists = false;
            var rTableOption = new MyTableOption();
            try
            {
                var myCodeGen = new MyCodeGen(_solutionName);

                //dosya varmı?
                if (System.IO.File.Exists(myCodeGen.GetTableOptionsFullPathFileName(_TableOptionName)))
                {
                    rExists = true;
                }

                //option read
                rTableOption = myCodeGen.FnTableOptionRead(_TableName, _TableOptionName);
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { bError = rError, sMessage = rMessage, bExists = rExists, oTableOption = rTableOption });
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TableOptionSave(string _solutionName, string _TableOptionName, string _TableOptionText)
        {
			bool rError;
            string rMessage = string.Empty;

			try
            {
                //option read
                var myCodeGen = new MyCodeGen(_solutionName);
                var result = myCodeGen.FnTableOptionSave(_TableOptionName, _TableOptionText);
                rError = result.Error;
                rMessage = result.Message;
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }

            return Json(new { bError = rError, sMessage = rMessage });
        }

        #endregion

        #region CodeWrite

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult CodeWrite(string _solutionName, string _TableOptionNames, string _CodeName)
        {
			var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(_solutionName);
            foreach (var tableOptionName in _TableOptionNames.Split(","))
            {
                MyCustomResult rV = myCodeGen.CodeWriteAll(tableOptionName, _CodeName);
                result.Error = rV.Error;
                result.Message += rV.Message;
            }
            return Json(new { bError = result.Error, sMessage = result.Message });
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult ReTableOptionSave(string _solutionName, string _TableOptionNames)
        {
			var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(_solutionName);
            foreach (var tableOptionName in _TableOptionNames.Split(","))
            {
                //tümü geldiğinde, tüm options dosyalarının defaultlarıyla read ve write yapılması
                string optionsFileName = myCodeGen.GetTableOptionsFullPathFileName(tableOptionName); 
                if (System.IO.File.Exists(optionsFileName))
                {
                    var tableOption = myCodeGen.FnTableOptionRead("", tableOptionName);
                    string jsonText = JsonSerializer.Serialize(tableOption);
                    myCodeGen.FnTableOptionSave(tableOptionName, jsonText);
                    result.Message += tableOptionName + " kaydedildi." + Environment.NewLine;
                }
            }
            return Json(new { bError = result.Error, sMessage = result.Message });
        }

        #endregion


    }
}