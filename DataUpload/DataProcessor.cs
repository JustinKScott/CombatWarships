using AutoMapper;
using DataUpload.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace DataUpload
{
    internal class DataProcessor
    {

        public async Task Process()
        {
            using (var context = new Database())
            {
                Task dropDb;
                try
                {
                    var tablename = nameof(context.Ships);
                    var dropCmd = $"DROP TABLE IF EXISTS {tablename}";
                    dropDb = context.Database.ExecuteSqlRawAsync(dropCmd);
                }
                catch (Exception ex) { return; }



                var shipData = await GetShiplist();
                if (shipData == null)
                    throw new InvalidOperationException("No shipdata");


            Parallel.ForEach(shipData, (ship) =>
            {
               WikiDataAnalyzer wiki = null;
               if (!string.IsNullOrEmpty(ship.WikiLink))
               {
                  wiki = new WikiDataAnalyzer(ship.WikiLink);
               }
               else
               {
                  List<string> wikiUrls = new List<string>();
                  wikiUrls.AddRange(WikiFinder.FindWikiUrls(ship));

                  foreach (var url in wikiUrls)
                  {
                     var potentialWiki = new WikiDataAnalyzer(url);
                     if (potentialWiki.ValidateShip(ship))
                     {
                        Console.WriteLine($"Found wiki for {ship.ClassName}");
                        ship.WikiLink = potentialWiki.Url;
                        wiki = potentialWiki;
                        break;
                     }
                  }

                  // No wiki article found.
                  if (wiki == null)
                     return;
               }


               ship.SpeedKnots = wiki.Speed;

               var wikiArmor = wiki.Armor;
               if (wikiArmor != null)
               {
                  ship.Armor = wikiArmor;
               }
            });



            await dropDb;
                await context.Database.EnsureCreatedAsync();

                foreach (var ship in shipData)
                    context.Ships.Add(ship);

                context.SaveChanges();
            }
        }

        private async Task<List<Ship>?> GetShiplist()
        {
            string jsonShipData = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("https://ircwcc.org/common/shiplist/ships.php");
                jsonShipData = await result.Content.ReadAsStringAsync();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var shipData = JsonSerializer.Deserialize<List<JsonShip>>(jsonShipData, options);


            var mapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<JsonShip, Ship>()
                .ForMember(dest => dest.SpeedIrcwcc, opt => opt.MapFrom(src => src.Speed))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Loa))
                .ForMember(dest => dest.ClassAbbreviation, opt => opt.MapFrom(src => src.ClassType))
                .ForMember(dest => dest.ClassType, opt => opt.MapFrom(src => WarshipTypeHelper.Lookup(src.ClassType)))
                .ForMember(dest => dest.Launched, opt => opt.MapFrom(src => RegExHelper.FindYear(src.Launched)))
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => RegExHelper.FindYear(src.Completed)))
            );

            var mapper = mapperConfig.CreateMapper();

            var ships = new List<Ship>();
            foreach (var jsonShip in shipData)
                ships.Add(mapper.Map<Ship>(jsonShip));

            return ships;
        }

    }
}