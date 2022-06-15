namespace Tables.Data
{
    public class LocationResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeskDTO> Desks { get; set; }

    }
}