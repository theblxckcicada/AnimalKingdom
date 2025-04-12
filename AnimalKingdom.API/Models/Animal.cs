namespace AnimalKingdom.API.Models
{
    public class Animal : IEntityBase<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
