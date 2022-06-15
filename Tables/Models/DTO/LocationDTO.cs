namespace Tables.Data
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeskDTO> Desks { get; set; }

    }
}