using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarshipSearchAPI.Data
{
	public class Ship
	{
		private string? wikiLink;
		private string? navyLink;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }


		[Required]
		public string Nation { get; set; }

		[Required]
		public string ClassName { get; set; }

		[Required]
		public WarshipType ClassType { get; set; }

		[Required]
		public string ClassAbbreviation { get; set; }

		public int? ShipClass { get; set; }

		public double? Units { get; set; }

		public int? NumberInClass { get; set; }

		public int? SpeedIrcwcc { get; set; }

		public double? SpeedKnots { get; set; }


		public int? Length { get; set; }
		public int? Beam { get; set; }
		public int? StandardWeight { get; set; }
		public int? FullWeight { get; set; }
		public int? Guns { get; set; }
		public double? GunDiameter { get; set; }
		public double? Armor { get; set; }
		public int? Rudders { get; set; }
		public string? RudderType { get; set; }
		public string? RudderStyle { get; set; }
		public int? Shafts { get; set; }


		public int? ShiplistKey { get; set; }
		public string? Comment { get; set; }
		public string? WikiLink { get => wikiLink; set => wikiLink = value?.Replace(@"\/", "/"); }
		public string? NavyLink { get => navyLink; set => navyLink = value?.Replace(@"\/", "/"); }
		public string? AdditionalLink { get; set; }
		public string? Notes { get; set; }
		public string? Unknown0 { get; set; }

		public int? Launched { get; set; }
		public int? Completed { get; set; }

	}
}
