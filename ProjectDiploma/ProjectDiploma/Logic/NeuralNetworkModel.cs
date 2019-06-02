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
            var rnd = new Random();
            
            var preTrainDataSize = 850;
            for (var i = 0; i < preTrainDataSize; i++)
            {
                var features = new double[NeuralNetwork.NeuralNetwork.inputDim];

                features[0] = 0;
                features[1] = 0;
                features[2] = 0;
                features[3] = 0;
                features[4] = 0;
                features[5] = 0;

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
                nn.Train(item);
            }
        }

        public void Train(int id, int interest, User user)
        {
            var project = projectsRepository.Get(id);

            if (project != null)
            {
                double[] features = ExtractFeatures(user, project);

                var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();
                nn.Train(new NeuralNetwork.NeuralNetworkData
                {
                    Features = features,
                    Labels = new[] { (double)interest }
                });
            }
        }

        public static double[] ExtractFeatures(User user, Project project)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];
            
            features[0] = (double)project.Stage;
            features[1] = project.StartDate?.Ticks ?? -1d;
            features[2] = project.FinishDate?.Ticks ?? -1d;
            features[3] = (double)project.CostCurrent;
            features[4] = (double)project.CostFull;
            features[5] = project.Initializer.Id;

            var orderedUserTags = user.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < userTagsLength && i < orderedUserTags.Length; i++)
            {
                features[i + 6] = orderedUserTags[i].TagId;
            }

            var orderedTags = project.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 11] = orderedTags[i].TagId;
            }

            return features;
        }

        public static double[] ExtractFeatures(User user, ProjectViewModel project)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];

            features[0] = (double)Enum.Parse(typeof(Project.ProjectStage), project.Stage);
            features[1] = project.StartDate?.Ticks ?? -1d;
            features[2] = project.FinishDate?.Ticks ?? -1d;
            features[3] = (double)project.CostCurrent;
            features[4] = (double)project.CostFull;
            features[5] = project.Initializer.Id;

            var orderedUserTags = user.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < userTagsLength && i < orderedUserTags.Length; i++)
            {
                features[i + 6] = orderedUserTags[i].TagId;
            }

            var orderedTags = project.Tags.OrderBy(x => x.Id).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 11] = orderedTags[i].Id;
            }

            return features;
        }

        public IEnumerable<ProjectViewModel> SortProjects(User user, IEnumerable<ProjectViewModel> items)
        {
            var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();

            return items.OrderByDescending(x => nn.Evaluate(new NeuralNetwork.NeuralNetworkData
            {
                Features = ExtractFeatures(user, x)
            }));
        }
    }
}
