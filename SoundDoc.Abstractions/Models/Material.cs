namespace SoundDoc.Abstractions.Models
{
    public class Material
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double SizeFrom { get; set; }
        public double SizeTo { get; set; }
        public double[] ValuesArray { get; set; }
    }
}