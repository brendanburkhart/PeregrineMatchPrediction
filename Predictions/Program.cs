using Microsoft.ML;
using Shared;
using System;

namespace MLExploration.Predictions
{
    class Program
    {
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            // Load saved model
            var trainedModel = mlContext.Model.Load(Configuration.modelPath, out DataViewSchema modelSchema);

            var testData = mlContext.Data.LoadFromTextFile<DataRow>(Configuration.testDataPath, hasHeader: true);

            var predictions = trainedModel.Transform(testData);

            var metrics = mlContext.BinaryClassification.EvaluateNonCalibrated(data: predictions, labelColumnName: "RedWon", scoreColumnName: "Score");

            Console.WriteLine($"F1: {metrics.F1Score} Accuracy: {metrics.Accuracy}");
        }
    }
}
