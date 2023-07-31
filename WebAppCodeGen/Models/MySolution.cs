using System.Runtime.CompilerServices;
using System.Text.Json;
using WebApp1.Codes;

namespace WebAppCodeGen.Models
{
    #region MySolution

    public class MoSolution
    {
        public string Name { get; set; } = "";
        public string Directory { get; set; } = "";
        public string MainConnectionString { get; set; } = "";
        public string LogConnectionString { get; set; } = "";
    }


    public class MySolution
    {
        public string FileName { get; set; } = MyApp.Env?.ContentRootPath + "/Solution.dat";
        public List<MoSolution> Solutions = new();

        public MySolution()
        {
            this.ReadAll();
        }

        public void SaveAll()
        {
            File.WriteAllText(this.FileName, JsonSerializer.Serialize(this.Solutions));
        }

        public void ReadAll()
        {
            try
            {
                if (!File.Exists(this.FileName))
                {
                    this.Insert(new MoSolution()
                    {
                        Name = "Solution1",
                        Directory = "C:\\Yedek\\Projeler\\Solution1",
                        MainConnectionString = "Server=.;Database=solution1_main;Trusted_Connection=True;Max Pool Size=500;",
                        LogConnectionString = "Server=.;Database=solution1_log;Trusted_Connection=True;Max Pool Size=500;"
                    });
                }

                this.Solutions = JsonSerializer.Deserialize<List<MoSolution>>(File.ReadAllText(this.FileName)) ?? new List<MoSolution>();
            }
            catch { }
        }

        public MoSolution GetByName(string name)
        {
            return this.Solutions.First(f => f.Name == name);
        }

        public void Insert(MoSolution model)
        {
            this.Solutions.Add(model);
            this.SaveAll();
        }

        public void Delete(MoSolution model)
        {
            this.Solutions.Remove(model);
            this.SaveAll();
        }

        public void Update(MoSolution model)
        {
            this.Solutions.Remove(this.GetByName(model.Name));
            this.Solutions.Add(model);
            this.SaveAll();
        }
    }
    #endregion
}
