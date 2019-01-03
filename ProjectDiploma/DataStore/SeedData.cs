using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    public static class SeedData
    {

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BusinessUniversityContext(
                serviceProvider.GetRequiredService<DbContextOptions<BusinessUniversityContext>>()))
            {
                //создаем админа
                var adminId = await EnsureUserCreated(serviceProvider, "1234", "ekaterinatimofeeva20@gmail.com");

                //создаем роли + добавляем админа в роль админ
                await EnsureRoleCreated(serviceProvider, adminId, BusinessUniversityContext.RoleName.ADMIN_ROLE_NAME);
                await EnsureRoleCreated(serviceProvider, string.Empty, BusinessUniversityContext.RoleName.BUSINESS_ROLE_NAME);
                await EnsureRoleCreated(serviceProvider, string.Empty, BusinessUniversityContext.RoleName.UNIVERSITY_ROLE_NAME);

                //создаем элементы таблиц
                await SeedDB(context);
            }
        }

        private static async Task<string> EnsureUserCreated(IServiceProvider serviceProvider,
                                                            string userPassword, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new User { UserName = userName };
                await userManager.CreateAsync(user, userPassword);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRoleCreated(IServiceProvider serviceProvider, string uid, string role)
        {
            IdentityResult ir = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetService<UserManager<User>>();
            var user = await userManager.FindByIdAsync(uid);

            if (!await roleManager.RoleExistsAsync(role))
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (user != null)
            {
                ir = await userManager.AddToRoleAsync(user, role);
            }

            return ir;
        }

        /// <summary>
        /// Заполняет таблицы первоначальными сведениями
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task SeedDB(BusinessUniversityContext context)
        {
            var tags = await FillTags(context);

            var newsTypes = await FillNewsType(context);

            //await FillEvents(context, tags);

            //сохраняем базу
            await context.SaveChangesAsync();
        }

        private static async Task<IEnumerable<Tag>> FillTags(BusinessUniversityContext context)
        {
            List<Tag> items = new List<Tag>
            {
                new Tag { Name = "тренинг" },
                new Tag { Name = "семинар" },
                new Tag { Name = "развитие" },
                new Tag { Name = "конференция" },
                new Tag { Name = "занятость" },
                new Tag { Name = "студенты" },

                new Tag { Name = "биология"},
                new Tag { Name = "болезнь"},
                new Tag { Name = "новосибирск"},
                new Tag { Name = "генетика"},

                new Tag { Name = "курсы"},
                new Tag { Name = "университеты"},
                new Tag { Name = "бизнесс"}
            };

            await context.Tags.AddRangeAsync(items);

            return items;
        }

        private static async Task<IEnumerable<NewsType>> FillNewsType(BusinessUniversityContext context)
        {
            List<NewsType> items = new List<NewsType>
            {
                //....
            };

            await context.NewsTypes.AddRangeAsync(items);

            return items;
        }

        private static async Task FillEvents(BusinessUniversityContext context, IEnumerable<Tag> tags)
        {
            List<Event> items = new List<Event>
            {
                new Event
                {
                    Title = "",
                    Tags = tags.Where(tag => 
                            tag.Name == "тренинг" || tag.Name == "")
                                .Select(x => new EventsTags { Tags = x }).ToList(),
                },
            };

            await context.Events.AddRangeAsync(items);
        }

        //NewsTypes

        //Events
        //Тренинги профессионального и личностного развития для преподавателей и сотрудников университета
        //Социальная адаптация на рынке труда студентов выпускных групп колледжа по специальностям «Фармация» и «Ветеринария» совместно с ОГКУ «Центр занятости населения г. Астрахани»

        //News
        //Project -
    }
}
