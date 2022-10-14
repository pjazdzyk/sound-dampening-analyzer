using System.Threading.Tasks;
using SoundDoc.Abstractions.Models;

namespace SoundDoc.Abstractions.Data
{
    public interface IMaterialRepository
    {
        Task<Material> GetMaterialByName(string name, double sizeFrom, double sizeTo);
    }
}