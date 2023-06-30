using AppData.Main.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppData.Main.Repository
{

    //public interface IDto
    //{
    //    DataContext  DataContext { get; set; }
    //}

    public class BaseDmo
    {
        protected readonly DataContext dataContext;

        public BaseDmo(DataContext context)
        {
            this.dataContext = context;
        }
    }



}