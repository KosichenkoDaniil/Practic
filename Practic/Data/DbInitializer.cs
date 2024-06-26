using Microsoft.Extensions.Hosting;
using Practic.Models;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practic.Data
{
    public class DbInitializer
    {
        private static Random random = new Random();

        private static List<string> NameofService1 = new()
            {
                "Электрики"
            };

        private static List<string> Department1 = new()
            {
                "СтПЦ-1",
                "СтПЦ-2",
                "СтПЦ-3",
                "СПЦ-1",
                "СПЦ-2",
                "ТПЦ",
                "ЭСПЦ-1",
                "ЭСПЦ-2"
            };
        private static List<string> Name1 = new()
            {
               "Трансформатор",
                "Реактор",
                "Конденсатор",
                "Диод",
                "Тиристор",
                "Транзистор",
                "Резистор",
                "Индуктивность",
                "Стабилитрон",
                "Фоторезистор",
                "Реле",
                "Выпрямитель",
                "Стабилизатор напряжения",
                "Инвертор",
                "Автоматический выключатель",
                "Преобразователь частоты",
                "Генератор",
                "Мотор",
                "Датчик температуры",
                "Датчик давления",
                "Ультразвуковой датчик",
                "Оптический датчик",
                "Силовой предохранитель",
                "Электромагнит",
                "Контактор",
                "Силовой полупроводниковый модуль",
                "Промышленный контроллер",
                "Сервопривод",
                "Интерфейсный модуль",
                "Тепловое реле",
                "Термопара",
                "Измерительный трансформатор",
                "Преобразователь тока",
                "Импульсный источник питания",
                "Шина питания",
                "Электропривод",
                "Магнитный пускатель",
                "Электромеханическое реле",
                "Блок управления",
                "Панель оператора"
            };



        public static void Initialize(PracticdataContext db)
        {
            db.Database.EnsureCreated();

            if (!db.Currencies.Any())
            {
                var currencies = new List<Currency>
                {
                    new Currency { NameofCurrency = "Доллар США", CountryofCurrency = "Соединенные Штаты" },
                    new Currency { NameofCurrency = "Евро", CountryofCurrency = "Германия" }, // Германия использует Евро
                    new Currency { NameofCurrency = "Японская иена", CountryofCurrency = "Япония" },
                    new Currency { NameofCurrency = "Французский евро", CountryofCurrency = "Франция" }, // Франция использует Евро
                    new Currency { NameofCurrency = "Швейцарский франк", CountryofCurrency = "Швейцария" },
                    new Currency { NameofCurrency = "Тайваньский доллар", CountryofCurrency = "Тайвань" },
                    new Currency { NameofCurrency = "Бразильский реал", CountryofCurrency = "Бразилия" },
                    new Currency { NameofCurrency = "Датская крона", CountryofCurrency = "Дания" },
                    new Currency { NameofCurrency = "Южнокорейская вона", CountryofCurrency = "Южная Корея" }
                };

                db.AddRange(currencies);
                db.SaveChanges();
            }

            if (!db.ForWhats.Any())
            {
                var forWhats = new List<ForWhat>
                {
                    new ForWhat { TypeofWork = "Замена" },
                    new ForWhat { TypeofWork = "Ремонт" },
                    new ForWhat { TypeofWork = "Обслуживание" }
                };

                db.AddRange(forWhats);
                db.SaveChanges();
            }

            if (!db.Firms.Any())
            {
                var firms = new List<Firm>
                {
                    new Firm { NameofFirm = "ABB", СountryofFirm  = "Швейцария" },
                    new Firm { NameofFirm = "Siemens", СountryofFirm  = "Германия" },
                    new Firm { NameofFirm = "Schneider Electric", СountryofFirm  = "Франция" },
                    new Firm { NameofFirm = "General Electric", СountryofFirm  = "Соединенные Штаты" },
                    new Firm { NameofFirm = "Mitsubishi Electric", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Eaton", СountryofFirm  = "Соединенные Штаты" },
                    new Firm { NameofFirm = "Honeywell", СountryofFirm  = "Соединенные Штаты" },
                    new Firm { NameofFirm = "Rockwell Automation", СountryofFirm  = "Соединенные Штаты" },
                    new Firm { NameofFirm = "Emerson Electric", СountryofFirm  = "Соединенные Штаты" },
                    new Firm { NameofFirm = "Bosch Rexroth", СountryofFirm  = "Германия" },
                    new Firm { NameofFirm = "Hitachi", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Fuji Electric", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Yokogawa Electric", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Omron", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Toshiba", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Panasonic", СountryofFirm  = "Япония" },
                    new Firm { NameofFirm = "Delta Electronics", СountryofFirm  = "Тайвань" },
                    new Firm { NameofFirm = "WEG", СountryofFirm  = "Бразилия" },
                    new Firm { NameofFirm = "Danfoss", СountryofFirm  = "Дания" },
                    new Firm { NameofFirm = "LG Electronics", СountryofFirm  = "Южная Корея" }
                };

                db.AddRange(firms);
                db.SaveChanges();

            }

            if (!db.ServiceNames.Any())
            {
                var random = new Random();
                var uniqueCombinations = new List<(string NameofService, string Department)>();

                foreach (var service in NameofService1)
                {
                    foreach (var department in Department1)
                    {
                        uniqueCombinations.Add((service, department));
                    }
                }

                
                for (int i = uniqueCombinations.Count - 1; i > 0; i--)
                {
                    int j = random.Next(0, i + 1);
                    var temp = uniqueCombinations[i];
                    uniqueCombinations[i] = uniqueCombinations[j];
                    uniqueCombinations[j] = temp;
                }

                
                var selectedCombinations = uniqueCombinations.Take(24).ToList();

                foreach (var combination in selectedCombinations)
                {
                    db.Add(new ServiceName() { NameofService = combination.NameofService, Department = combination.Department });
                }

                db.SaveChanges();
            }

            if (!db.Fabrics.Any())
            {
                var serviceNames = db.ServiceNames.Select(s => s.Id).ToList();
                var forWhats = db.ForWhats.Select(f => f.Id).ToList();

                for (int i = 0; i < 100; i++)
                {
                    string Name = Name1[random.Next(0, Name1.Count)];
                    int SetviceNameid = serviceNames[random.Next(0, serviceNames.Count)];
                    int ForWhatid = forWhats[random.Next(0, forWhats.Count)];
                    string CodeTnved = Math.Abs(random.Next()) % 10000 + " " + Math.Abs(random.Next()) % 100 + " " + Math.Abs(random.Next()) % 1000 + " " + Math.Abs(random.Next()) % 10;
                    string CodeOkrb = "27." + Math.Abs(random.Next()) % 100 + "." + Math.Abs(random.Next()) % 100 + "." + Math.Abs(random.Next()) % 1000;
                    db.Add(new Fabric() { Name = Name, SetviceNameId = SetviceNameid, ForWhatId = ForWhatid, CodeTnved = CodeTnved, CodeOkrb = CodeOkrb });
                }
                db.SaveChanges();
            }

            if (!db.Applications.Any())
            {
                var fabrics = db.Fabrics.ToList();
                var firms = db.Firms.ToList();
                var currencies = db.Currencies.ToList();
                var forWhats = db.ForWhats.ToList();
                var serviceNames = db.ServiceNames.ToList();
                var random = new Random();

                for (int i = 0; i < 100; i++)
                {
                    var fabric = fabrics[random.Next(0, fabrics.Count)];
                    int fabricId = fabric.Id;

                    var forWhat = forWhats.FirstOrDefault(fw => fw.Id == fabric.ForWhatId);
                    if (forWhat == null)
                    {
                        continue; // Пропустить итерацию, если соответствующий forWhat не найден
                    }
                    string typeOfWork = forWhat.TypeofWork;

                    var serviceName = serviceNames.FirstOrDefault(sn => sn.Id == fabric.SetviceNameId);
                    if (serviceName == null)
                    {
                        continue; // Пропустить итерацию, если соответствующий serviceName не найден
                    }
                    string department = serviceName.Department;

                    string shortDescription = $"{typeOfWork} - {fabric.Name} - {department}";

                    var firm = firms[random.Next(0, firms.Count)];
                    int firmId = firm.Id;
                    string firmCountry = firm.СountryofFirm;

                    var matchingCurrencies = currencies.Where(c => c.CountryofCurrency == firmCountry).ToList();
                    if (matchingCurrencies.Any())
                    {
                        int currencyId = matchingCurrencies[random.Next(0, matchingCurrencies.Count)].Id;
                        decimal price = random.Next(100, 10000);
                        int quantity = random.Next(1, 1000);

                        db.Add(new Application()
                        {
                            FabricId = fabricId,
                            ShortDescription = shortDescription,
                            FirmId = firmId,
                            CurrencyId = currencyId,
                            Price = price,
                            Quantity = quantity
                        });
                    }
                }
                db.SaveChanges();
            }

        }

    }
}
