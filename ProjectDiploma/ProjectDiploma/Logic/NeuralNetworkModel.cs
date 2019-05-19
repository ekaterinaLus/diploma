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
        private const int userTagsLength = 5;

        public NeuralNetworkModel(BusinessUniversityContext context)
        {
            projectsRepository = new ProjectsRepository(context);
        }

        public NeuralNetworkModel()
        {

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
