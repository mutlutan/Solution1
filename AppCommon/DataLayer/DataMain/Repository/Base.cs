using AppCommon.DataLayer.DataMain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppCommon.DataLayer.DataMain.Repository
{

    //public interface IDto
    //{
    //    DataContext  DataContext { get; set; }
    //}

    public class BaseDmo
    {
        protected readonly MainDataContext dataContext;

        public BaseDmo(MainDataContext context)
        {
            this.dataContext = context;
        }
    }



}