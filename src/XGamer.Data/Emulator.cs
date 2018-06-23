namespace XGamer.Data.Entities
{
    public class Emulator
    {
        public string FileName { get; set; }

        public string Parameters { get; set; }
        
        public int IdRomType { get; set; }
        
        public RomType RomType { get; set; }
    }
}
