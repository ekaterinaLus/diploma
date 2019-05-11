using DataStore.Entities;
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

        private const int tagsLength = 10;

        public NeuralNetworkModel(BusinessUniversityContext context)
        {
            projectsRepository = new ProjectsRepository(context);
        }

        public NeuralNetworkModel()
        {

        }

        public void Train(int id, int interest, string userId)
        {
            var project = projectsRepository.Get(id);

            if (project != null)
            {
                double[] features = ExtractFeatures(userId, project);

                var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();
                nn.Train(new NeuralNetwork.NeuralNetworkData
                {
                    Features = features,
                    Labels = new[] { (double)interest }
                });
            }
        }

        public static double[] ExtractFeatures(string userId, Project project)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];
            features[0] = userId.GetHashCode();
            features[1] = (double)project.Stage;
            features[2] = project.StartDate?.Ticks ?? -1d;
            features[3] = project.FinishDate?.Ticks ?? -1d;
            features[4] = (double)project.CostCurrent;
            features[5] = (double)project.CostFull;
            features[6] = project.Initializer.Id;

            var orderedTags = project.Tags.OrderBy(x => x.TagId).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 7] = orderedTags[i].TagId;
            }

            return features;
        }

        public static double[] ExtractFeatures(string userId, ProjectViewModel project)
        {
            var features = new double[NeuralNetwork.NeuralNetwork.inputDim];
            features[0] = userId.GetHashCode();
            features[1] = (double)Enum.Parse(typeof(Project.ProjectStage), project.Stage);
            features[2] = project.StartDate?.Ticks ?? -1d;
            features[3] = project.FinishDate?.Ticks ?? -1d;
            features[4] = (double)project.CostCurrent;
            features[5] = (double)project.CostFull;
            features[6] = project.Initializer.Id;

            var orderedTags = project.Tags.OrderBy(x => x.Id).ToArray();

            for (int i = 0; i < tagsLength && i < orderedTags.Length; i++)
            {
                features[i + 7] = orderedTags[i].Id;
            }

            return features;
        }

        public IEnumerable<ProjectViewModel> SortProjects(string userId, IEnumerable<ProjectViewModel> items)
        {
            var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();

            return items.OrderByDescending(x => nn.Evaluate(new NeuralNetwork.NeuralNetworkData
            {
                Features = ExtractFeatures(userId, x)
            }));
        }
    }
}
