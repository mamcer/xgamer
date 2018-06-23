namespace XGamer.Data.Entities
{
    public class Rom
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        
        public string GameName { get; set; }
        
        public int IdRomType { get; set; }
        
        public string Poster1FileName { get; set; }
        
        public string Poster2FileName { get; set; }
        
        public RomType RomType { get; set; }
    }
}
