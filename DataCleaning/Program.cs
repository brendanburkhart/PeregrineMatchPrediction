using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MLExploration.DataCleaning
{
    class Program
    {
        private static List<T> LoadData<T>(string dataPath)
        {
            using StreamReader file = File.OpenText(dataPath);
            using JsonTextReader json = new JsonTextReader(file);
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<List<T>>(json);
        }

        private static Schema LoadSchema(string schemaPath)
        {
            using StreamReader file = File.OpenText(schemaPath);
            using JsonTextReader json = new JsonTextReader(file);
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<Schema>(json);
        }

        private static Dictionary<string, Dictionary<string, double>> ProcessTeamStats(List<TeamStats> teamStats, Schema schema)
        {
            Dictionary<string, Dictionary<string, double>> processedStats = new Dictionary<string, Dictionary<string, double>>();

            foreach (var stats in teamStats)
            {
                processedStats.Add(stats.TeamNumber, new Dictionary<string, double>());

                foreach (var stat in stats.Stats)
                {
                    var statName = stat.Name.Replace(" ", "");
                    processedStats[stats.TeamNumber].Add(statName + "Max", stat.Max);
                    processedStats[stats.TeamNumber].Add(statName + "Avg", stat.Average);
                }
            }

            return processedStats;
        }

        private static string BuildTeamStats(Dictionary<string, double> stats, List<string> statNames)
        {
            return statNames.Select(name => stats[name]).Aggregate("", (prev, next) => $"{prev}\t{next}");
        }

        static void Main(string[] args)
        {
            List<TeamStats> teamStats = LoadData<TeamStats>(Configuration.statsPath);
            List<Match> matches = LoadData<Match>(Configuration.matchesPath);
            Schema schema = LoadSchema(Configuration.schemaPath);

            Dictionary<string, Dictionary<string, double>> processedTeamStats = ProcessTeamStats(teamStats, schema);

            string header = "didRedWin\tdata";

            List<string> statNames = schema.stats.Select(description => description.Name.Replace(" ", "") + "Max")
                                     .Union(schema.stats.Select(description => description.Name.Replace(" ", "") + "Avg"))
                                     .ToList();

            using (StreamWriter outputFile = File.CreateText(Configuration.trainingDataPath))
            {
                outputFile.WriteLine(header);

                foreach (var match in matches)
                {
                    int didRedWin = (match.RedScore > match.BlueScore) ? 1 : 0;

                    string data = $"{didRedWin}";

                    foreach (var teamNumber in match.RedAlliance)
                    {
                        data += BuildTeamStats(processedTeamStats[teamNumber], statNames);
                    }

                    foreach (var teamNumber in match.BlueAlliance)
                    {
                        data += BuildTeamStats(processedTeamStats[teamNumber], statNames);
                    }

                    outputFile.WriteLine(data);
                }
            }
        }
    }
}
