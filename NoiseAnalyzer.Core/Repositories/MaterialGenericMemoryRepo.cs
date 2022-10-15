using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoiseAnalyzer.Core.Enttities;
using NoiseAnalyzer.Core.Extensions;

namespace NoiseAnalyzer.Core.Repositories
{
    public abstract class MaterialGenericMemoryRepo
    {
        protected abstract List<Material> Materials { get; set; }

        public void Add(string name, string description, double sizeFrom, double sizeTo, double[] valuesArray)
        {
            Materials.Add(new Material
            {
                Name = name,
                SizeFrom = sizeFrom,
                SizeTo = sizeTo,
                Description = $"{description} [MATERIAŁ UŻYTKOWNIKA]",
                ValuesArray = valuesArray
            });
        }

        public Material GetMaterialByName(string name)
        {
            return Materials.First(m => m.Name == name);
        }

        public Material GetMaterialByNameAndSize(string name, double size)
        {
            return Materials.Find(m => m.Name.Equals(name) && size > m.SizeFrom && size <= m.SizeTo);
        }

        public void Remove(string name)
        {
            if (!Materials.Any(material => material.Name == name))
            {
                throw new ArgumentException("Invalid parameter", name);
            }

            Materials.RemoveAll(material => material.Name == name);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var material in Materials)
            {
                builder.AppendLine($"  {material.Name.PadRight(25)}  {material.ValuesArray.Format().PadRight(55)}  {material.Description.PadRight(10)}  ");
            }

            return builder.ToString();
        }
    }
}