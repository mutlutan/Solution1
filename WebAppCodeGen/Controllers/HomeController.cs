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
            var solution = new MySolution().GetByName(solutionName);

            var myCodeGen = new MyCodeGen(solution.MainConnectionString);

            var MyCodeGenInfoList = new List<MyCodeGenInfo>();
            foreach (var table in myCodeGen.GetTablesOrViews())
            {
                var tableOptions = MyCodeGen.GetTableOptions(table.Name);

                MyCodeGenInfoList.Add(
                    new MyCodeGenInfo
                    {
                        TableName = table.Name,
                        TableOptions = tableOptions,
                        DataTransferObjects = MyCodeGen.GetDataTransferObjects(table.Name),
                        DataManipulationObjects = MyCodeGen.GetDataManipulationObjects(table.Name),
                        Controllers = MyCodeGen.GetControllers(table.Name),
                        Dictionaries = MyCodeGen.GetDictionaries(table.Name),
                        FormViews = MyCodeGen.GetFormViews(table.Name),
                        GridViews = MyCodeGen.GetGridViews(table.Name),
                        TreeLists = MyCodeGen.GetTreeLists(table.Name),
                        SearchViews = MyCodeGen.GetSearchViews(table.Name)
                    }
                );
            }

            ViewBag.MyCodeGenInfoList = MyCodeGenInfoList;
            ViewBag.SolutionName = solution.Name;

			return View();
        }



        #region ReadLocks
        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult ReadLocks(string _solutionName, string _TableName, string _TableOptionName)
        {
            var solution = new MySolution().GetByName(_solutionName);

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


                var myCodeGen = new MyCodeGen(solution.MainConnectionString);
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
            var solution = new MySolution().GetByName(_solutionName);

            var rList = new List<string>();
            if (_TableName != null)
            {
                var myCodeGen = new MyCodeGen(solution.MainConnectionString);
                rList = myCodeGen.GetTableOrViewColumns(_TableName).Select(s => s.Name).ToList();
            }

            return Json(new { data = rList });
        }
        #endregion

        #region Table Options

        public IActionResult TableOption(string _solutionName, string _TableName, string _TableOptionName)
        {
            var solution = new MySolution().GetByName(_solutionName);

            ViewBag.SolutionName = solution.Name;
            ViewBag.TableName = _TableName;
            ViewBag.TableOptionName = _TableOptionName;
            if (_TableName == _TableOptionName)
            {
                ViewBag.TableOptionName = _TableOptionName + "_";
            }

            var myCodeGen = new MyCodeGen(solution.MainConnectionString);

            List<string> tableAndViews = myCodeGen.GetTablesOrViews().Select(s => s.Name).ToList();
            ViewBag.tableList = tableAndViews;

            ViewBag.DataTransferObjectCount = MyCodeGen.GetDataTransferObjects(_TableName).Count;
            ViewBag.DataManipulationObjectCount = MyCodeGen.GetDataManipulationObjects(_TableName).Count;
            ViewBag.ControllerCount = MyCodeGen.GetControllers(_TableName).Count;
            ViewBag.DictionaryCount = MyCodeGen.GetDictionaries(_TableName).Count;
            ViewBag.FormViewCount = MyCodeGen.GetFormViews(_TableName).Count;
            ViewBag.GridViewCount = MyCodeGen.GetGridViews(_TableName).Count;
            ViewBag.TreeListCount = MyCodeGen.GetTreeLists(_TableName).Count;
            ViewBag.SearchViewCount = MyCodeGen.GetSearchViews(_TableName).Count;

            return View();
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TableOptionRead(string _solutionName, string _TableName, string _TableOptionName)
        {
			var solution = new MySolution().GetByName(_solutionName);
			Boolean rError = false;
            string rMessage = "";
            bool rExists = false;
            var rTableOption = new MyTableOption();
            try
            {
                //dosya varmı?
                if (System.IO.File.Exists(MyCodeGen.GetTableOptionsFullPathFileName(_TableOptionName)))
                {
                    rExists = true;
                }

                //option read
                var myCodeGen = new MyCodeGen(solution.MainConnectionString);
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
			var solution = new MySolution().GetByName(_solutionName);
			bool rError;
            string rMessage = string.Empty;

			try
            {
                //option read
                var myCodeGen = new MyCodeGen(solution.MainConnectionString);
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
			var solution = new MySolution().GetByName(_solutionName);
			var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(solution.MainConnectionString);
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
			var solution = new MySolution().GetByName(_solutionName);
			var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(solution.MainConnectionString);
            foreach (var tableOptionName in _TableOptionNames.Split(","))
            {
                //tümü geldiğinde, tüm options dosyalarının defaultlarıyla read ve write yapılması
                string optionsFileName = MyCodeGen.GetTableOptionsFullPathFileName(tableOptionName); 
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