using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.Enttities;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.Repositories;

namespace NoiseAnalyzer.Core.AcousticModels
{
    /**
    *  ACOUSTIC PROPERTIES CLASS - VENTILATION RECTANGULAR OR CIRCULAR DUCT
    */
    public class AcuDuct : AcuBasic<HydraulicsDuct>
    {
        protected MaterialGenericMemoryRepo MaterialDataBase { get; }                    // Duct material database initialization
        protected Material DuctMaterial { get; private set; }                            // Duct material

        private AcuDuct() : this(HydraulicsDuct.FromDefaults()) { }

        public static AcuDuct FromDefaults()
        {
            return new();
        }

        private AcuDuct(HydraulicsDuct hydraulics)
            : base(hydraulics)
        {
            MaterialDataBase = new AbsorbingMaterialsMemoryRepo();
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
            SourceDa = PhysicsDuct.CalcDuctSourceDa(Hydraulics, DuctMaterial);
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