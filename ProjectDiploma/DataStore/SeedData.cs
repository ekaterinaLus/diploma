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
using static DataStore.Entities.Project;

namespace DataStore
{
    static class DBHelper
    {
        private static readonly Dictionary<Type, dynamic> _comparersList = new Dictionary<Type, dynamic>
        {
            { typeof(Tag), new GenericComparer<Tag> { GetComparableField = x => x.Name } },
            { typeof(News), new GenericComparer<News> { GetComparableField = x => x.Header } },
            { typeof(Event), new GenericComparer<Event> { GetComparableField = x => x.Title } },
            { typeof(NewsType), new GenericComparer<NewsType> { GetComparableField = x => x.Name } },
            { typeof(Project), new GenericComparer<Project> { GetComparableField = x => x.Name }},
            { typeof(University), new GenericComparer<University> { GetComparableField = x => x.Name } }
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
                await EnsureRoleCreated(serviceProvider, adminId, nameof(BusinessUniversityContext.RoleValues.ADMIN));
                await EnsureRoleCreated(serviceProvider, string.Empty, nameof(BusinessUniversityContext.RoleValues.BUSINESS));
                await EnsureRoleCreated(serviceProvider, string.Empty, nameof(BusinessUniversityContext.RoleValues.UNIVERSITY));

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
            var univers = await FillUnivers(context);
            //сохраняем базу
            await context.SaveChangesAsync();

            await FillEvents(context, tags);
            await FillNews(context, tags, newsTypes);
            await FillProjects(context, tags, univers);

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
                new Tag { Name = "бизнес"},
                new Tag { Name = "IT-технологии"},
                new Tag { Name = "менеджмент"},
                new Tag { Name = "торговля"},
                new Tag { Name = "франчайзинг"},
                new Tag { Name = "электроника"},
                new Tag { Name = "информация и СМИ"},
                new Tag { Name = "блокчейн"}
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

        private static async Task<IEnumerable<University>> FillUnivers(BusinessUniversityContext context)
        {
            List<University> items = new List<University>
            {
                new University
                {
                    Name = "АГУ",
                    ContactInformation = "ЮРИДИЧЕСКИЙ АДРЕС: 414056, Россия, г. Астрахань, ул. Татищева, 20а, Астраханский государственный университет.Телефоны: 8(8512) 24 - 64 - 00.Факс: 8(8512) 49 - 41 - 57.E - mail: asu @asu.edu.ru"

                },
                new University
                {
                    Name = "АГТУ",
                    ContactInformation = "АГТУ', 'г.Астрахань, ул.Татищева, 16. Приемная ректора: тел. (8512) 61-41-19. Общий отдел: тел./факс 61-43-66; e-mail: astu@astu.org"
                }           
            };

            await context.Universities.AddUniqueElementsAsync(items);
            return await context.Universities.GetDatabaseElements(items);

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
                                .Select(x => new EventsTags { Tag = x }).ToList(),
                    Description = "Тренинги профессионального и личностного развития для преподавателей и сотрудников университета",
                    Address = "ул. Татищева, 20а"
                },
                new Event
                {
                    Date = new DateTime(2018, 7, 5),
                    Title = "Мероприятие",
                    Tags = tags.Where(tag =>
                            tag.Name == "семинар" || tag.Name == "развитие")
                                .Select(x => new EventsTags { Tag = x }).ToList(),
                    Description = "Социальная адаптация на рынке труда студентов выпускных групп колледжа по специальностям «Фармация» и «Ветеринария» совместно с ОГКУ «Центр занятости населения г. Астрахани",
                    Address = "ул. Татищева, 20а"
                },
                 new Event
                {
                    Date = new DateTime(2019, 6, 5),
                    Title = "Курсы",
                    Tags = tags.Where(tag =>
                            tag.Name == "университеты" || tag.Name == "развитие")
                                .Select(x => new EventsTags { Tag = x }).ToList(),
                    Description = "Курсы повышения квалификации для преподавателей университета",
                    Address = "ул. Татищева, 20а"
                },
                 new Event
                {
                    Date = new DateTime(2019, 6, 5),
                    Title = "Социальная адаптация на рынке труда",
                    Tags = tags.Where(tag =>
                            tag.Name == "университеты" || tag.Name == "развитие"|| tag.Name == "семинар")
                                .Select(x => new EventsTags { Tag = x }).ToList(),
                    Description = "Социальная адаптация на рынке труда студентов выпускных групп колледжа по специальностям «Информатика» совместно с ОГКУ «Центр занятости населения г. Астрахани»",
                    Address = "ул. Татищева, 20а"
                },
                   new Event
                {
                    Date = new DateTime(2019, 6, 5),
                    Title = "Тренинги для сотрудников организаций",
                    Tags = tags.Where(tag =>
                            tag.Name == "тренинг" || tag.Name == "развитие")
                                .Select(x => new EventsTags { Tag = x }).ToList(),
                    Description = "Тренинги профессионального и личностного развития для преподавателей и сотрудников университета",
                    Address = "ул. Татищева, 20а"
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
                                .Select(x => new NewsTags { Tag = x }).ToList(),
                    Header = "Российские ученые приблизились к разгадке генетической болезни Хантингтона",
                    Annotation = "Биологи Новосибирского государственного университета и Института цитологии и генетики СО РАН создали  клеточную линию, " +
                    "которая моделирует неизлечимую на сегодня болезнь",
                    Date = DateTime.Now.AddDays(-5), 
                    Link = "http://s1.1zoom.me/big0/930/Coast_Sunrises_and_sunsets_Waves_USA_Ocean_Kaneohe_521540_1280x775.jpg",
                    Section = nt.Where(x => x.Name == "Наука").FirstOrDefault(),
                    Text = @"Что такое болезнь Хантингтона? Хорея Хантингтона (или Гентингтона — взависимости отпроизношения имени врача, ее описавшего) —  тяжелое, неизлечимое, наследственное заболевание. Недуг кроется вгенах иможет проявить себя влюбой момент, чтообычно становится тяжелым ударом для человека, который дотого ипонятия неимел, что обречен.Болезнь поражает нервную систему, прежде всего — стриатум, область головного мозга, которую также называют полосатым телом. Этоприводит кразвитию хореического гиперкинеза — синдрома, прикотором учеловека развиваются неконтролируемые беспорядочные отрывистые движения различной амплитуды иинтенсивности. Внешне онинапоминают танец — отсюда иназвание «хорея», что в переводе с греческого означает «пляска» (унамного более невинного слова «хореография» аналогичное происхождение). В народе это состояние также получило  название «пляска святого Вита». Болезнь Хантингтона проявляется у больных в возрасте 35 - 45 лет двигательных и умственных расстройств и в течение 15 - 20 лет приводит к распаду личности и смерти.Чтобы найти метод лечения, российские ученые с помощью современной технологии «отредактировали» геном CRISPR\Cas9 и получили клеточную модель,  которая позволит исследовать молекулярные механизмы заболевания."
                },
                new News
                {                    
                    Section = nt.Where( n => n.Name == "Обучающие курсы и семинары").FirstOrDefault(),
                    Tags = tags.Where(tag =>
                            tag.Name == "курсы" || tag.Name == "университеты" || tag.Name == "бизнес")
                                .Select(x => new NewsTags { Tag = x }).ToList(),
                    Header = "Обучающие курсы помогут усовершенствовать работу компании",
                    Date = DateTime.Now.AddDays(-1),
                    Text = @"На сегодняшний день конкурентоспособными оказываются компании, постоянно работающие над своим усовершенствованием. В частности, перспективы развития светят тем, кто уделяет должное внимание обучению всего штата сотрудников, в том числе и руководящего состава. Приоритетным в работе таких компаний является системный подход к решению всех вопросов. За счет этого занятия любого формата строятся таким образом, что их восприятие слушателями оказывается простым и при этом эффективным, а продолжение самосовершенствования каждым участником в самостоятельном режиме становится беспроблемным. Ассортимент предложений профессионалов очень широк и позволяет подобрать наиболее оптимальное решение в полном соответствии с поставленными задачами. Многие программы рассчитаны на реализацию в течение весьма продолжительного периода времени, например, за 1-3 года, и именно такие тренинги дают самые солидные и стабильные плоды в виде формирования отлично подкованного во всех вопросах штата. Огромное значение уделяет мотивации учени ков, перед которыми заблаговременно ставятся четкие цели и задачи. Корректировка обучения может осуществляться по ходу продвижения его участников вперед, при этом могут применяться и практические инструменты для проверки и закрепления полученных знаний и навыков. Идеальным форматом сотрудничества между двумя сторонами становится интеграция программы в организацию заказчика на всех уровнях. Это позволяет в режиме реального времени отслеживать полученные результаты и выявлять степень эффективности проведенных мероприятий."
                },

                new News
                {
                    Section = nt.Where( n => n.Name == "Обучающие курсы и семинары").FirstOrDefault(),
                    Tags = tags.Where(tag =>
                            tag.Name == "курсы" ||  tag.Name == "бизнес")
                                .Select(x => new NewsTags { Tag = x }).ToList(),
                    Header = "Пять способов для стартапа вызвать доверие у клиентов",
                    Annotation = "Доверие является ключевым фактором в процессе установления профессиональных отношений и увеличения количество конверсий. Согласно исследованиям, 1 из 3 потребителей оценили «доверие к бренду» как одну из трех главных причин, которые влияют на их решение о покупке у конкретного продавца.",
                    Date = DateTime.Now.AddDays(-1),
                    Text = @"1. Ценный контент:
                    одним из наиболее эффективных способов для стартапа привлечь и удержать клиентов может стать получение их доверия путем предоставления им ценного образовательного контента. Вы не только представите свою компанию в качестве ведущего специалиста в своей отрасли, предлагая полезные сообщения в блогах, но и сможете решить проблемы своей целевой аудитории.
                    Раздел комментариев ваших сообщений в блогах также является отличным местом для развития ваших отношений с потребителями. 2. Быть доступным для запросов через чат: Большинство потребителей сегодня хотят быстрого, почти немедленного ответа и предпочитают общаться с клиентами через чат, а не по телефону или электронной почте. 3. Истории успеха: Истории успеха являются социальным доказательством, а социальное доказательство - ключ к укреплению доверия с потребителями. Когда клиент не уверен в чем-то, он смотрит на социальное доказательство, чтобы понять, что думают и говорят другие. 
                    Фактически, 90% покупателей, которые читали положительные истории и отзывы других клиентов, утверждали, что это повлияло на их решение о покупке. 4. Политика возврата: Хорошая политика возврата показывает, что вы заботитесь о своих клиентах и уверены в своем продукте. "
                },
                new News
                {
                    Section = nt.Where( n => n.Name == "Обучающие курсы и семинары").FirstOrDefault(),
                    Tags = tags.Where(tag =>
                            tag.Name == "развитие")
                                .Select(x => new NewsTags { Tag = x }).ToList(),
                    Header = "«Инвестируйте в свой опыт, а не в имущество», - Томас Гилович",
                    Annotation = "Результаты исследования профессор психологии Корнельского университета о ценности инвестиций в опыт",
                    Date = DateTime.Now.AddDays(-1),
   
                    Text = @"Профессор психологии Корнельского университета доктор Томас Гилович в течении 20-ти лет проводил исследование, которое привело его к одному простому выводу: не стоит тратить деньги на вещи, то есть инвестировать в имущество. Проблема в том, что счастье, которое оно обеспечивает, быстро исчезает. Для этого есть три важных причины:
                    Мы привыкаем к новым вещам. То, что когда-то казалось новым и захватывающим, быстро становится нормой.
                    Мы продолжаем поднимать планку. Новые покупки приводят к новым ожиданиям. Как только мы немного привыкаем к новой вещи, начинаем искать что-то лучше.
                    Имущество, по своей природе, способствует сравнению. Мы покупаем новый автомобиль и в восторге от него, пока друг не купит лучше.
                    «Один из врагов счастья - адаптация, - говорит Гилович. Мы покупаем вещи, чтобы сделать себя счастливыми, и нам это удается. Но только на время. Сначала новые вещи нам интересны, но мы быстро привыкаем к этому».

                    Парадокс имущества состоит в том, что мы предполагаем, что счастье, которое мы получаем от покупки чего-либо, будет продолжаться до тех пор, пока у нас есть сама вещь. На уровне интуиции кажется, что инвестирование в то, что мы можем видеть, слышать и к чему прикасаться, имеет большую ценность. Но это не так.  "
                }

            };

            await context.News.AddUniqueElementsAsync(items);
        }


        private static async Task FillProjects(BusinessUniversityContext context, IEnumerable<Tag> tags, IEnumerable<University> univ)
        {
            List<Project> items = new List<Project>
            {
                //new Project
                //{
                //    Name = "Платформа. Бизнес-тренажеры для онлайн обучения",
                //    Description = "Платформа позволяет: - Быстро и легко создавать многопользовательские бизнес-тренажеры и реалистичные обучающие бизнес-игры - Создавать, продавать и проводить онлайн курсы по менеджменту и онлайн тренинги нового поколения, построенные по схеме «ТЕОРИЯ (контент) + ПРАКТИКА (в бизнес-симуляции) = НАВЫКИ» - Проводить автоматизированную онлайн оценку компетенций сотрудников и кандидатов в бизнес-тренажерах - Проводить онлайн турниры по менеджменту - Интегрировать бизнес-симуляции в любые образовательные платформы",
                //    Risks = "Рыночная среда: Worldwide E-Learning Market by 2016 - $51,5 B,Annual worldwide growth rate over theperiod 2012-2016 - 7,9%Одним из самых мощных трендов последних лет в индустрии онлайн обучения и тренингов является движение в сторону геймификации и использования обучающих игр и симуляций.Самые высокие темпы роста в индустрии e-learning сейчас показывает сегмент решений simulation-based - 23% в год. С $2,4B в 2012 году до $6,6 в 2017.",
                //    CostCurrent = 50000,
                //    CostFull = 100000,
                //    Date = DateTime.Now.AddDays(-1),
                //    IsClosed = false,
                //    Stage = ProjectStage.Founding,
                //    Initializer = univ
                //                .Where(n => n.Name == "АГУ")
                //                    .FirstOrDefault()

                //},

                //new Project
                //{
                //    Name = "Аналитическая система визуального анализа данных",
                //    Description = "Система реализована на авторских теориях: - искусственный интеллект; -теория СУБД;-теория графов.Свойства: -интеллектуальный анализ с помощью глубокой интерактивности, поддерживающая рассуждения исследователя;-встроенные Объектная База данных и База знаний;-аналитические возможности усиленны технологией 'нейронные сети'(начальная стадия для прогнозирования процессов);-не требующий навыков программирования визуальный язык GSQL",
                //    Risks = "Российский рынок информационных технологий находится под контролем дилеров западных продуктов. Их гонорары порядка 2-3 млрд $. И они делают всё возможное, чтобы на этот рынок не допустить отечественные продукты. ",
                //    CostCurrent = 10000,
                //    CostFull = 100000,
                //    Date = DateTime.Now.AddDays(-1),
                //    IsClosed = false,
                //    Stage = ProjectStage.Founding,
                //    Initializer = univ
                //                .Where(n => n.Name == "АГУ")
                //                    .FirstOrDefault()
                //},
                //new Project
                //{
                //    Name = "Доставка свежеприготовленного горячего обеда или ужина",
                //    Description = "Действующий бизнес с монетизацией.- Портал работает;-ежедневно заключаются договоры с точками общественного питания;-укомплектован штат сотрудников;-разработана юридическая и правовая основа проекта;-совместно с процессинговой компанией разработана логика платежей, которая интегрирована в портал и дает возможность всем участникам бизнес - процесса получать свои средства в автоматическом режиме при оплате заказа покупателем;-разработана упаковка;-отгружается наша фирменная упаковка в точки общественного питания;-заключены договоры с партнерами и разными участниками бизнес - процесса;-продукт выводится на рынок согласно маркетинговому плану;-разработана и автоматизирована программа лояльности для покупателей и поставщиков, которая присутствует в логике платежей;-закончено тестирование мобильного приложения.",
                //    Risks = "Российский рынок информационных технологий находится под контролем дилеров западных продуктов. Их гонорары порядка 2-3 млрд $. И они делают всё возможное, чтобы на этот рынок не допустить отечественные продукты. ",
                //    CostCurrent = 15000,
                //    CostFull = 50000,
                //    Date = DateTime.Now.AddDays(-1),
                //    IsClosed = false,
                //    Stage = ProjectStage.Founding,
                //    Initializer = univ
                //                .Where(n => n.Name == "АГТУ")
                //                    .FirstOrDefault(),
                    
                //},

                //new Project
                //{
                //    Name = "Реклама будущего, которая производит wow эффект",
                //    Description = "Готов прототип. Проекционная система для показа включает в себя мобильную платформу на базе Лада Ларгус, оснащённую автоматизированным подъемником - позиционером и лазерным проекторомна нём. По заказу клиента, создаётся изображение или видеоролик на специализированном программном обеспечении, и отправляется на проекционную систему. В результате получаем мега-широкоформатную рекламу нового поколения, котоорая имеет повышеннгое время удержания целевой аудитории и, как следствие, обеспечивает повышене конверсии компаний.",
                //    Risks = "Риски практически отстуствуют",
                //    CostCurrent = 50000,
                //    CostFull = 300000,
                //    Date = DateTime.Now.AddDays(-1),
                //    IsClosed = false,
                //    Stage = ProjectStage.Founding,
                //    Initializer = univ
                //                .Where(n => n.Name == "АГУ")
                //                    .FirstOrDefault(),
                     
                //},
                new Project
                {
                    Name = "Сервис коллективного принятия решений",
                    Description = "Сервис опросов, позволяет в реальном времени проводить голосование в прямом эфире ТВ передач, одновременно суммируя голоса с сайтов, соцсетей и других площадок. Подходит для соцопросов и закрытых групп (конференции, выборы, ТСЖ, АО и прочее) с одновременным участием онлайн-голосующих и оффлайн (присутствующих в зале). Планируется большое расширение функций именно для коллективного принятия решений - участники могут инициировать обсуждения, голосования, добавлять ответы, комментировать, объединяться в группы, загружать видео, устанавливать расписание и т.д.",
                    Risks = "1. Изменения в законодательстве о хранении персональных данных 2. Появление систем сбора данных у крупных игроков - Яндекс, Гугл. ",
                    CostCurrent = 50000,
                    CostFull = 300000,
                    Date = DateTime.Now.AddDays(-1),
                    IsClosed = false,
                    Stage = ProjectStage.Founding,
                    Initializer = univ
                                .Where(n => n.Name == "АГУ")
                                    .FirstOrDefault(),
                    
                }
            };
            //items[0].Tags = tags.Where(tag =>
            //              tag.Name == "бизнес" || tag.Name == "менеджмент" || tag.Name == "IT-технологии")
            //                    .Select(x => new ProjectsTags { Project = items[0], Tag = x }).ToList();
            //items[0].Tags = tags.Where(tag =>
            //               tag.Name == "IT-технологии")
            //                    .Select(x => new ProjectsTags {Project = items[0], Tag = x }).ToList();
            //items[0].Tags = tags.Where(tag =>
            //               tag.Name == "IT-технологии" || tag.Name == "франчайзинг" || tag.Name == "торговля")
            //                   .Select(x => new ProjectsTags { Tag = x }).ToList();

            //items[0].Tags = tags.Where(tag =>
            //                tag.Name == "IT-технологии" || tag.Name == "информация и СМИ" || tag.Name == "электроника")
            //                    .Select(x => new ProjectsTags { Tag = x }).ToList();

            items[0].Tags = tags.Where(tag =>
                           tag.Name == "IT-технологии" || tag.Name == "информация и СМИ" || tag.Name == "блокчейн")
                               .Select(x => new ProjectsTags { Tag = x }).ToList();

            await context.Projects.AddUniqueElementsAsync(items);
        }
    }
}
