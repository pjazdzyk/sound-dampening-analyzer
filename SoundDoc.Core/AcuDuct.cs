using SoundDoc.Abstractions.Models;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Physics;

namespace SoundDoc.Core
{
    /**
    *  ACOUSTIC PROPERTIES CLASS - VENTILATION RECTANGULAR OR CIRCULAR DUCT
    */
    public class AcuDuct : AcuBasic<HydraulicsDuct>
    {
        protected MaterialAbstractList MaterialDataBase { get; }                    // Duct material database initialization
        protected Material DuctMaterial { get; private set; }                   // Duct material

        private AcuDuct() : this(HydraulicsDuct.FromDefaults()) { }

        public static AcuDuct FromDefaults()
        {
            return new();
        }

        private AcuDuct(HydraulicsDuct hydraulics) 
            : base(hydraulics)
        {
            MaterialDataBase = new AbsorbingMaterials();
            DuctMaterial = MaterialDataBase.GetMaterialByName(Defaults.Material);
            UpdateAcousticProperties();
        }

        public static AcuDuct FromHydraulics(HydraulicsDuct hydraulics)
        {
            return new(hydraulics);
        }

        public AcuDuct WithName(string name)
        {
            Name = name;
            return this;
        }

        public AcuDuct WithInputLw(double[] inputLw)
        {
            SetInputLw(inputLw);
            return this;
        }

        public AcuDuct WithCladingMaterial(Material claddingType)
        {
            SetMaterialInstance(claddingType);
            return this;
        }

        protected override void CalculateSourceDa() 
        {
            SourceDa = PhysicsDuct.CalcDuctSourceDa(Hydraulics,DuctMaterial);
        }

        protected override void CalculateSourceLw() 
        {
            SourceLw = PhysicsDuct.CalcDuctSourceLw(Hydraulics);
        }

        public void SetDuctLength(double length) 
        {
            Hydraulics.SetDuctLength(length);
            UpdateAcousticProperties();
        }

        public void SetMaterialByKeyName(string materialKey)
        {
            DuctMaterial = MaterialDataBase.GetMaterialByName(materialKey);
            UpdateAcousticProperties();
        }

        public void SetMaterialInstance(Material material)
        {
            DuctMaterial = material;
            UpdateAcousticProperties();
        }
    }
}