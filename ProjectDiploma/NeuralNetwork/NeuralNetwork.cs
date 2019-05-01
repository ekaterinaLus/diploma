using CNTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNetwork
{
    [Serializable]
    public class NeuralNetworkData
    {
        public double[] Features { get; set; }
        public double[] Labels { get; set; }
    }

    public class NeuralNetwork
    {
        private static NeuralNetwork currentNetwork = null;

        private static double? EmptyFunction(string param) => null;

        private static double? DoubleParse(string param) => double.Parse(param, System.Globalization.CultureInfo.InvariantCulture);

        private readonly Func<string, double?>[] ParseFunctions = new Func<string, double?>[]
        {
            EmptyFunction,                                      //1
            c1 => DateTime.Parse(c1).Ticks / 1_000_000_000,     //2
            EmptyFunction,                                      //3
            DoubleParse,                                         //4
            DoubleParse,                                         //5
            DoubleParse,                                         //6
            DoubleParse,                                         //7
            EmptyFunction,                                      //8
            DoubleParse,                                         //9
            DoubleParse,                                         //10
            DoubleParse,                                         //11
            c11 => c11 == "conventional" ? -1d : 1d,            //12
            EmptyFunction,                                      //13
            c13 => c13.GetHashCode()                            //14
        };

        private const string SAVED_MODEL_FILE_NAME = "businessUniversity.bin";
        private const string SAVED_BATCH_FILE_NAME = "currentNNBatch.tmp";

        private Random rnd = new Random();

        private const int inputDim = 17;
        private const int outputDim = 1;
        private const int hiddenDim = 24;
        private const int batchSize = 60;

        private Trainer trainer;
        private Function model;
        private readonly DeviceDescriptor device = DeviceDescriptor.CPUDevice;
        private readonly NDShape inputShape = new NDShape(1, inputDim);
        private readonly NDShape outputShape = new NDShape(1, outputDim);

        private List<NeuralNetworkData> dataCache = new List<NeuralNetworkData>(batchSize + 1);

        private NeuralNetwork() { }

        ~NeuralNetwork()
        {
            try
            {
                Serializer.Save(SAVED_BATCH_FILE_NAME, dataCache);
            }
            catch { }
        }

        private IEnumerable<NeuralNetworkData> ReadFromFile(string fileName)
        {
            var file = File.ReadLines(fileName);
            var result = file
                            .Skip(1)
                            .Select(x => x.Split(','))
                            .Select(x => new NeuralNetworkData
                            {
                                Labels = new[] { double.Parse(x[2], System.Globalization.CultureInfo.InvariantCulture) },
                                Features = x
                                            .Select((param, indx) => ParseFunctions[indx].Invoke(param))
                                            .Where(param => param != null)
                                            .Select(param => param.Value)
                                            .ToArray()
                            })
                            .ToArray();

            var vector = new double[result[0].Features.Length];

            foreach (var item in result)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    if (Math.Abs(item.Features[i]) > vector[i])
                    {
                        vector[i] = Math.Abs(item.Features[i]);
                    }
                }
            }

            foreach (var item in result)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    if (Math.Abs(item.Features[i]) != 1d)
                    {
                        item.Features[i] /= vector[i] + 0.1d;
                    }
                }
            }

            return result;
        }

        private Function AddLayer(Function previousLayer, int outputDimension)
        {
            var glorotInit = CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1);

            var castedPrevLayer = (Variable)previousLayer;

            var w = new Parameter(new int[] { outputDimension, castedPrevLayer.Shape[0] }, DataType.Double, glorotInit, device, "w");
            var b = new Parameter(new int[] { outputDimension }, DataType.Double, 0, device, "b");

            return CNTKLib.Times(w, previousLayer) + b;
        }

        private Function AddActivationFunction(Function layer)
        {
            return CNTKLib.ReLU(layer);
        }

        public static NeuralNetwork GetNeuralNetwork()
        {
            NeuralNetwork result = new NeuralNetwork();

            if (currentNetwork == null)
            {
                result.CreateNN();
                if (File.Exists(SAVED_MODEL_FILE_NAME))
                {
                    result.trainer.RestoreFromCheckpoint(SAVED_MODEL_FILE_NAME);
                }
            }
            else
            {
                result = currentNetwork;
            }

            result.dataCache = Serializer.Load<List<NeuralNetworkData>>(SAVED_BATCH_FILE_NAME);

            return result;
        }

        private void CreateNN()
        {
            var features = Variable.InputVariable(inputShape, DataType.Double);
            var label = Variable.InputVariable(outputShape, DataType.Double);

            model = AddLayer(features, hiddenDim);
            model = AddActivationFunction(model);

            model = AddLayer(model, hiddenDim);
            model = AddActivationFunction(model);

            model = AddLayer(model, hiddenDim);
            model = AddActivationFunction(model);

            model = AddLayer(model, outputDim);

            var loss = CNTKLib.SquaredError(model, label);
            var evalError = CNTKLib.SquaredError(model, label);
            //0.002525
            var learningRatePerSample = new TrainingParameterScheduleDouble(0.002525, 1);
            var parameterLearners = new List<Learner>() { Learner.SGDLearner(model.Parameters(), learningRatePerSample) };

            trainer = Trainer.CreateTrainer(model, loss, evalError, parameterLearners);
        }

        public void Train()
        {
            var trainData = ReadFromFile("avocado.csv").ToArray();

            //var trainData = GetTrainData(500).ToList();

            

            

            var epoch = 1000;
            int j = 0, i = 0;
            var batchSize = 20;

            while (epoch > -1)
            {
                if (j + batchSize > trainData.Count())
                {
                    j = 0;
                }

                var featuresData = trainData.Skip(j).Take(batchSize).Select(x => x.Features);
                var labelsData = trainData.Skip(j).Take(batchSize).Select(x => x.Labels);

                j += batchSize;

                var arguments = new Dictionary<Variable, Value>
                {
                    { features, Value.CreateBatchOfSequences(inputShape, featuresData, device) },
                    { label, Value.CreateBatchOfSequences(outputShape, labelsData, device) }
                };

                trainer.TrainMinibatch(arguments, false, device);

                PrintTrainingProgress(trainer, i++, 10);

                epoch--;
            }
        }

        public double Evaluate(NeuralNetworkData data)
        {

            var feature = model.Arguments[0];
            var label = model.Output;

            var featuresData = data.Features;

            var inputDataMap = new Dictionary<Variable, Value>()
            {
                { feature, Value.CreateBatch(inputShape, featuresData, device)}
            };

            var outputDataMap = new Dictionary<Variable, Value>()
            {
                { label, null }
            };

            model.Evaluate(inputDataMap, outputDataMap, device);
            var outputData = outputDataMap[label].GetDenseData<double>(label);
            var actualLabels = outputData[0][0];

            return actualLabels;
        }

        private static void PrintTrainingProgress(Trainer trainer, int minibatchIdx, int outputFrequencyInMinibatches)
        {
            if ((minibatchIdx % outputFrequencyInMinibatches) == 0 && trainer.PreviousMinibatchSampleCount() != 0)
            {
                float trainLossValue = (float)trainer.PreviousMinibatchLossAverage();
                float evaluationValue = (float)trainer.PreviousMinibatchEvaluationAverage();
                Console.WriteLine($"Minibatch: {minibatchIdx} CrossEntropyLoss = {trainLossValue}, EvaluationCriterion = {evaluationValue}");
            }
        }
    }
}
