using AnimalKingdom.Shared.Extensions;
using System.Text.Json.Serialization;

namespace AnimalKingdom.Shared.Models
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(EnumJsonConverter<AnimalCategory>))]
        public AnimalCategory Category { get; set; }
        public string Image { get; set; }
    }

    public enum AnimalCategory
    {
        Herbivore,
        Carnivore,
        Omnivore,
    }
}
