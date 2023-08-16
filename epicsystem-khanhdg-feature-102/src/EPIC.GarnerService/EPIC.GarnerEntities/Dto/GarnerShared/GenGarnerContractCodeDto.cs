namespace EPIC.GarnerEntities.Dto.GarnerShared
{
    public class GenGarnerContractCodeDto
    {
        public DataEntities.GarnerOrder Order { get; set; }
        public DataEntities.GarnerProduct Product { get; set; }
        public DataEntities.GarnerPolicy Policy { get; set; }
    }
}
