using DataStore.Entities;
using DataStore.Repositories;
using DataStore.Repositories.ProjectRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Logic
{
    public class NeuralNetworkModel
    {
        private readonly ProjectsRepository projectsRepository = null;
        private readonly GenericRepository<Tag> tagsRepository = null;

        private const int tagsLength = 10;
        private const int userTagsLength = 5;

        public NeuralNetworkModel(BusinessUniversityContext context)
        {
            projectsRepository = new ProjectsRepository(context);
            tagsRepository = new GenericRepository<Tag>(context);
        }

        public NeuralNetworkModel()
        {

        }

        public void PreTrain()
        {
            var generatedTrainData = new List<NeuralNetwork.NeuralNetworkData>();
            var tags = tagsRepository.GetAll().OrderBy(x => x.Id).ToArray();
            double maxId = tags.Max(x => x.Id);
            long startDateMax = DateTime.Now.AddDays(60).Ticks;
            long finishDateMax = DateTime.Now.AddDays(80).Ticks;
            double maxCurrentCost = (double)projectsRepository.GetAll().Max(x => x.CostCurrent);
            double maxFullCost = (double)projectsRepository.GetAll().Max(x => x.CostFull);
            double maxInitId = projectsRepository.GetAll().Max(x => x.Initializer.Id);

            var rnd = new Random();
            
            var preTrainDataSize = 1275;
            for (var i = 0; i < preTrainDataSize; i++)
            {
                var features = new double[NeuralNetwork.NeuralNetwork.inputDim];

                features[0] = rnd.Next(0, 5) / 4d;

                if (rnd.NextDouble() > 0.5)
                {
                    var minDate = DateTime.Now.AddDays(40).Ticks;

                    var minVal = (int)(minDate >> 32);
                    var maxVal = (int)(startDateMax >> 32);
                    features[1] = ((long)rnd.Next(minVal, maxVal + 1) << 32) / (double)startDateMax;
                }
                else
                {
                    features[1] = -1;
                }

                if (rnd.NextDouble() > 0.5)
                {
                    var minDate = DateTime.Now.AddDays(60).Ticks;

                    var minVal = (int)(minDate >> 32);
                    var maxVal = (int)(finishDateMax >> 32);
                    features[2] = ((long)rnd.Next(minVal, maxVal + 1) << 32) / (double)finishDateMax;
                }
                else
                {
                    features[2] = -1;
                }

                var curPrice = rnd.Next(0, (int)maxCurrentCost);

                features[3] = curPrice / maxCurrentCost;
                features[4] = rnd.Next(curPrice, (int) maxFullCost) / maxFullCost;
                features[5] = rnd.Next(0, (int)maxInitId + 1) / (double)maxInitId;

                var userIndexes = new HashSet<int>(15);

                for (int j = 0; j < userTagsLength; j++)
                {
                    var randomIndex = rnd.Next(0, tags.Length);
                    while (userIndexes.Contains(randomIndex))
                    {
                        if (++randomIndex >= tags.Length)
                        {
                            randomIndex = 0;
                        }
                    }
                    userIndexes.Add(randomIndex);

                    features[j + 6] = tags[randomIndex].Id / maxId;
                }

                var projectIndexes = new HashSet<int>(15);

                for (int j = 0; j < tagsLength; j++)
                {
                    var randomIndex = rnd.Next(0, tags.Length);
                    while (projectIndexes.Contains(randomIndex))
                    {
                        if (++randomIndex >= tags.Length)
                        {
                            randomIndex = 0;
                        }
                    }
                    projectIndexes.Add(randomIndex);

                    features[j + 11] = tags[randomIndex].Id / maxId;

                    if (j >= 3)
                        break;
                }

                userIndexes.IntersectWith(projectIndexes);

                var labels = new double[] { ((double)userIndexes.Count) / ((double)userTagsLength) };

                generatedTrainData.Add(new NeuralNetwork.NeuralNetworkData
                {
                    Features = features,
                    Labels = labels
                });
            }
            var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();

            foreach (var item in generatedTrainData)
            {
                nn.Train(item, true);
            }

            nn.Train(generatedTrainData.Last());
        }

        public void Train(int id, int interest, User user)
        {
            var project = projectsRepository.Get(id);
            var maxId = projectsRepository.GetAll().Max(x => x.Id);
            var startDateMax = projectsRepository.GetAll().Max(x => x.StartDate)?.Ticks ?? -1;
            var finishDateMax = projectsRepository.GetAll().Max(x => x.FinishDate)?.Ticks ?? -1;
            var maxCurrentCost = projectsRepository.GetAll().Max(x => x.CostCurrent);
            var maxFullCost = projectsRepository.GetAll().Max(x => x.CostFull);
            var maxInitId = projectsRepository.GetAll().Max(x => x.Initializer.Id);

            if (project != null)
            {
                double[] features = ExtractFeatures(user, project, maxId, startDateMax, 
                    finishDateMax, (double)maxCurrentCost, (double)maxFullCost, maxInitId);

                var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();
                nn.Train(new NeuralNetwork.NeuralNetworkData
                {
                    Features = features,
                    Labels = new[] { (double)interest }
                });
            }
        }

        public static double[] ExtractFeatures(User user, Project project, double maxTagId, 
            double startDateMax, double finishDateMax, double maxCurrentCost, double maxFullCost, double maxInitializerId)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];

            var startDate = project.StartDate?.Ticks ?? -1d;
            var finishDate = project.FinishDate?.Ticks ?? -1d;

            features[0] = (double)project.Stage / 4d;
            features[1] = startDate < 0 ? startDate : startDate / startDateMax;
            features[2] = finishDate < 0 ? finishDate : finishDate / finishDateMax;
            features[3] = (double)project.CostCurrent / maxCurrentCost;
            features[4] = (double)project.CostFull / maxFullCost;
            features[5] = project.Initializer.Id / maxInitializerId;

            var orderedUserTags = user.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < userTagsLength && i < orderedUserTags.Length; i++)
            {
                features[i + 6] = orderedUserTags[i].TagId / maxTagId;
            }

            var orderedTags = project.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 11] = orderedTags[i].TagId / maxTagId;
            }

            return features;
        }

        public static double[] ExtractFeatures(User user, ProjectViewModel project, double maxTagId,
            double startDateMax, double finishDateMax, double maxCurrentCost, double maxFullCost, double maxInitializerId)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];

            var startDate = project.StartDate?.Ticks ?? -1d;
            var finishDate = project.FinishDate?.Ticks ?? -1d;

            features[0] = (double)Enum.Parse(typeof(Project.ProjectStage), project.Stage) / 4d;
            features[1] = startDate < 0 ? startDate : startDate / startDateMax;
            features[2] = finishDate < 0 ? finishDate : finishDate / finishDateMax;
            features[3] = (double)project.CostCurrent / maxCurrentCost;
            features[4] = (double)project.CostFull / maxFullCost;
            features[5] = project.Initializer.Id / maxInitializerId;

            var orderedUserTags = user.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < userTagsLength && i < orderedUserTags.Length; i++)
            {
                features[i + 6] = orderedUserTags[i].TagId / maxTagId;
            }

            var orderedTags = project.Tags.OrderBy(x => x.Id).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 11] = orderedTags[i].Id / maxTagId;
            }

            return features;
        }

        //public IEnumerable<ProjectViewModel> SortProjects(User user, IEnumerable<ProjectViewModel> items)
        //{
        //    var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();
        //    var maxId = projectsRepository.GetAll().Max(x => x.Id);

        //    return items.OrderByDescending(x => nn.Evaluate(new NeuralNetwork.NeuralNetworkData
        //    {
        //        Features = ExtractFeatures(user, x, maxId)
        //    }));
        //}
    }
}
