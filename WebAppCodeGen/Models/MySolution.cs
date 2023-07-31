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
        public List<MoSolution> Solutions = new();

        public MySolution()
        {
            this.ReadAll();
        }

        public void ReadAll()
        {
            var di = new DirectoryInfo(MyApp.DataDirectory);
            var solutionDirectories = di.GetDirectories(".", SearchOption.TopDirectoryOnly);

            foreach (var dir in solutionDirectories)
            {
                var file = dir.GetFiles("_.dat", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (file != null)
                {
                    var solution = JsonSerializer.Deserialize<MoSolution>(File.ReadAllText(file.FullName)) ?? new MoSolution();
                    this.Solutions.Add(solution);
                }
            }
        }

        public MoSolution GetByName(string name)
        {
            return this.Solutions.First(f => f.Name == name);
        }
    }
    #endregion
}
