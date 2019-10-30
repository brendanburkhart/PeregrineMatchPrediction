using Microsoft.ML;
using Shared;
using System;
using static Microsoft.ML.DataOperationsCatalog;

namespace MLExploration
{
    class Program
    {
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            // Load training and validation data from TSV file
            IDataView dataView = mlContext.Data.LoadFromTextFile<DataRow>(path: Configuration.trainingDataPath, hasHeader: true, separatorChar: '\t');

            // Split data into training and validation sets, 70% training, 30% validation
            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.3);
            IDataView trainingData = dataSplit.TrainSet;
            IDataView validationData = dataSplit.TestSet;

            // Normalize all team stats
            var dataProcessPipeline = mlContext.Transforms.NormalizeLogMeanVariance("Features");

            // RedWon is boolean label column, Features is array of all normalized team stats
            var trainer = mlContext.BinaryClassification.Trainers.LinearSvm(labelColumnName: "RedWon", featureColumnName: "Features", numberOfIterations: 25);
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // Train model
            var model = trainingPipeline.Fit(trainingData);

            // Apply trained model to validation data
            var predictions = model.Transform(validationData);

            var metrics = mlContext.BinaryClassification.EvaluateNonCalibrated(data: predictions, labelColumnName: "RedWon", scoreColumnName: "Score");

            Console.WriteLine($"F1: {metrics.F1Score} Accuracy: {metrics.Accuracy}");

            // Save trained model
            mlContext.Model.Save(model, trainingData.Schema, Configuration.modelPath);

            Console.WriteLine($"Saved model to {Configuration.modelPath}");
        }
    }
}
