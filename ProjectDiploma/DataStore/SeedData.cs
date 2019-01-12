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
    static class DBHelper
    {
        private static readonly Dictionary<Type, dynamic> _comparersList = new Dictionary<Type, dynamic>
        {
            { typeof(Tag), new GenericComparer<Tag> { GetComparableField = x => x.Name } },
            { typeof(News), new GenericComparer<News> { GetComparableField = x => x.Header } },
            { typeof(Event), new GenericComparer<Event> { GetComparableField = x => x.Title } },
            { typeof(NewsType), new GenericComparer<NewsType> { GetComparableField = x => x.Name } }
        };

        public static async Task AddUniqueElementsAsync<T>(this DbSet<T> @this, IEnumerable<T> addingElements)
            where T: class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = await @this.AsNoTracking().ToArrayAsync();
            await @this.AddRangeAsync(addingElements.Except(elementsInDb.Intersect(addingElements, comparer), comparer));
        }

        public static async Task<IEnumerable<T>> GetDatabaseElements<T>(this DbSet<T> @this, IEnumerable<T> localElements)
            where T: class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = await @this.ToArrayAsync();
            return elementsInDb.Where(item => localElements.Contains(item, comparer));
        }
    }

    class GenericComparer<TItem> : IEqualityComparer<TItem>
    {
        public delegate dynamic Field(TItem obj);
        public Field GetComparableField { get; set; }

        public bool Equals(TItem x, TItem y)
        {
            return GetComparableField(x) == GetComparableField(y);
        }

        public int GetHashCode(TItem obj)
        {
            return GetComparableField(obj).GetHashCode();
        }
    }

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
            //сохраняем базу
            await context.SaveChangesAsync();

            await FillEvents(context, tags);
            await FillNews(context, tags, newsTypes);

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
                new Tag { Name = "бизнес"}
            };

            await context.Tags.AddUniqueElementsAsync(items);

            return await context.Tags.GetDatabaseElements(items);
        }

        private static async Task<IEnumerable<NewsType>> FillNewsType(BusinessUniversityContext context)
        {
            List<NewsType> items = new List<NewsType>
            {
                new NewsType {Name = "Наука" },
                new NewsType {Name = "Культура" },
                new NewsType {Name = "Промышленность" },
                new NewsType {Name = "Обучающие курсы и семинары" }
            };

            await context.NewsTypes.AddUniqueElementsAsync(items);

            return await context.NewsTypes.GetDatabaseElements(items);
        }

        private static async Task FillEvents(BusinessUniversityContext context, IEnumerable<Tag> tags)
        {
            List<Event> items = new List<Event>
            {
                new Event
                {
                    Date = new DateTime(2018, 5, 1),
                    Title = "Тренинг саморазвития",
                    Tags = tags.Where(tag => 
                            tag.Name == "тренинг" || tag.Name == "развитие")
                                .Select(x => new EventsTags { Tags = x }).ToList(),
                    Description = "Тренинги профессионального и личностного развития для преподавателей и сотрудников университета",
                    Adress = "ул. Татищева, 20а"
                },
                new Event
                {
                    Date = new DateTime(2018, 7, 5),
                    Title = "Мероприятие",
                    Tags = tags.Where(tag =>
                            tag.Name == "семинар" || tag.Name == "развитие")
                                .Select(x => new EventsTags { Tags = x }).ToList(),
                    Description = "Социальная адаптация на рынке труда студентов выпускных групп колледжа по специальностям «Фармация» и «Ветеринария» совместно с ОГКУ «Центр занятости населения г. Астрахани",
                    Adress = "ул. Татищева, 20а"
                }
            };
            
            await context.Events.AddUniqueElementsAsync(items);
           
        }

        ////NewsTypes

        ////Events
        ////Тренинги профессионального и личностного развития для преподавателей и сотрудников университета
        ////Социальная адаптация на рынке труда студентов выпускных групп колледжа по специальностям «Фармация» и «Ветеринария» совместно с ОГКУ «Центр занятости населения г. Астрахани»

        //News
        //Project -
        private static async Task FillNews(BusinessUniversityContext context, IEnumerable<Tag> tags, IEnumerable<NewsType> nt)
        {
            List<News> items = new List<News>
            {
                new News
                {
                    Tags = tags.Where(tag =>
                            tag.Name == "биология" || tag.Name == "болезнь" || tag.Name == "новосибирск" || tag.Name == "генетика")
                                .Select(x => new NewsTags { Tags = x }).ToList(),
                    Header = "Российские ученые приблизились к разгадке генетической болезни Хантингтона",
                    Annotation = "Биологи Новосибирского государственного университета и Института цитологии и генетики СО РАН создали  клеточную линию, " +
                    "которая моделирует неизлечимую на сегодня болезнь",
                    Date = DateTime.Now.AddDays(-5),           
                    Section = nt.Where(x => x.Name == "Наука").FirstOrDefault(),
                    Text = @"Что такое болезнь Хантингтона? Хорея Хантингтона (или Гентингтона — взависимости отпроизношения имени врача, ее описавшего) —  тяжелое, неизлечимое, наследственное заболевание. Недуг кроется вгенах иможет проявить себя влюбой момент, чтообычно становится тяжелым ударом для человека, который дотого ипонятия неимел, что обречен.Болезнь поражает нервную систему, прежде всего — стриатум, область головного мозга, которую также называют полосатым телом. Этоприводит кразвитию хореического гиперкинеза — синдрома, прикотором учеловека развиваются неконтролируемые беспорядочные отрывистые движения различной амплитуды иинтенсивности. Внешне онинапоминают танец — отсюда иназвание «хорея», что в переводе с греческого означает «пляска» (унамного более невинного слова «хореография» аналогичное происхождение). В народе это состояние также получило  название «пляска святого Вита». Болезнь Хантингтона проявляется у больных в возрасте 35 - 45 лет двигательных и умственных расстройств и в течение 15 - 20 лет приводит к распаду личности и смерти.Чтобы найти метод лечения, российские ученые с помощью современной технологии «отредактировали» геном CRISPR\Cas9 и получили клеточную модель,  которая позволит исследовать молекулярные механизмы заболевания."
                },
                new News
                {                    
                    Section = nt.Where( n => n.Name == "Обучающие курсы и семинары").FirstOrDefault(),
                    Tags = tags.Where(tag =>
                            tag.Name == "курсы" || tag.Name == "университеты" || tag.Name == "бизнес")
                                .Select(x => new NewsTags { Tags = x }).ToList(),
                    Header = "Обучающие курсы помогут усовершенствовать работу компании",
                    Date = DateTime.Now.AddDays(-1),
                    Text = @"На сегодняшний день конкурентоспособными оказываются компании, постоянно работающие над своим усовершенствованием. В частности, перспективы развития светят тем, кто уделяет должное внимание обучению всего штата сотрудников, в том числе и руководящего состава. Приоритетным в работе таких компаний является системный подход к решению всех вопросов. За счет этого занятия любого формата строятся таким образом, что их восприятие слушателями оказывается простым и при этом эффективным, а продолжение самосовершенствования каждым участником в самостоятельном режиме становится беспроблемным. Ассортимент предложений профессионалов очень широк и позволяет подобрать наиболее оптимальное решение в полном соответствии с поставленными задачами. Многие программы рассчитаны на реализацию в течение весьма продолжительного периода времени, например, за 1-3 года, и именно такие тренинги дают самые солидные и стабильные плоды в виде формирования отлично подкованного во всех вопросах штата. Огромное значение уделяет мотивации учени ков, перед которыми заблаговременно ставятся четкие цели и задачи. Корректировка обучения может осуществляться по ходу продвижения его участников вперед, при этом могут применяться и практические инструменты для проверки и закрепления полученных знаний и навыков. Идеальным форматом сотрудничества между двумя сторонами становится интеграция программы в организацию заказчика на всех уровнях. Это позволяет в режиме реального времени отслеживать полученные результаты и выявлять степень эффективности проведенных мероприятий."
                }
            };

            await context.News.AddUniqueElementsAsync(items);
        }
    }
}
