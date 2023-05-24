using Accord.Statistics.Testing;
using StudyWatcherProject.Models;

namespace StudyWatcherFormsAdmin;

public class AnovaAlgorithm
{
    public List<ProcessAnova> processList;
    public string[] processArray;
    public double[] rowSums;
    public OneWayAnova anovaResult;

    public AnovaAlgorithm(List<ProcessWs> processWsList)
    {
        this.processList = ConvertToProcesAnova(processWsList);
        anovaCollectionFromation();
    }

    public void anovaCollectionFromation()
    {
        List<string> distinctProcesses = processList.Select(p => p.NameProcess).Distinct().ToList();
        List<string> distinctLocations = processList.Select(p => p.NameLocation).Distinct().ToList();

        List<List<double>> groups = new List<List<double>>();
        double[,] matrix = new double[distinctProcesses.Count, distinctLocations.Count];

        foreach (string process in distinctProcesses)
        {
            List<double> processGroup = new List<double>();

            foreach (string location in distinctLocations)
            {
                int countLaunch =
                    processList.FirstOrDefault(p => p.NameProcess == process && p.NameLocation == location)
                        ?.CountLaunch ?? 0;
                processGroup.Add(countLaunch);

                matrix[distinctProcesses.IndexOf(process), distinctLocations.IndexOf(location)] = countLaunch;
            }

            groups.Add(processGroup);
        }
        foreach (List<double> processGroup in groups)
        {
            Console.WriteLine(string.Join(", ", processGroup));
        }
        
        this.processArray = distinctProcesses.ToArray();
        this.rowSums = SumRows(groups);
        this.anovaResult = ApplyAnova(groups);
    }


        public double[] SumRows(List<List<double>> matrix)
        {
            int rowsCount = matrix.Count;
            double[] rowSums = new double[rowsCount];

            for (int i = 0; i < rowsCount; i++)
            {
                double sum = matrix[i].Sum();
                rowSums[i] = sum;
            }

            return rowSums;
        }

        public OneWayAnova ApplyAnova(List<List<double>> groups)
        {
            double[][] data = groups.Select(g => g.Select(Convert.ToDouble).ToArray()).ToArray();

            OneWayAnova anova = new OneWayAnova(data);

            return anova;
        }
        
        public List<ProcessAnova> ConvertToProcesAnova(List<ProcessWs> processWsList)
        {
            List<ProcessAnova> result = new List<ProcessAnova>();
            var groupedProcesses = processWsList.GroupBy(p => new { p.NameProcess, p.NameLocation });

            foreach (var group in groupedProcesses)
            {
                ProcessAnova processAnova = new ProcessAnova
                {
                    NameProcess = group.Key.NameProcess,
                    NameLocation = group.Key.NameLocation,
                    CountLaunch = group.Count()
                };
                result.Add(processAnova);
            }
            return result;
        }
}