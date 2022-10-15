using System;
using NoiseAnalyzer.Core.Enttities;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Physics;
using NoiseAnalyzer.Core.Utils;
using NoiseAnalyzer.Core.Repositories;

namespace NoiseAnalyzer.Core.AcousticModels
{
    /**
    *  ACOUSTIC PROPERTIES CLASS - VENTILATION BEND / ELBOW
    */

    public class AcuBend : AcuBasic<HydraulicsBend>
    {
        protected MaterialGenericMemoryRepo BendNaturalDmpDataBase { get; private set; }       // baza danych z naturalnymi tlumiennosciami lukow 
        protected Material BendNaturalDamp { get; private set; }                               // tlumiennosc naturalna luku

        private AcuBend() : this(HydraulicsBend.FromDefaults()) { }

        public static AcuBend FromDefaults()
        {
            return new();
        }

        private AcuBend(HydraulicsBend hydraulics) : base(hydraulics)
        {
            BendNaturalDmpDataBase = new MaterialBendsLists();
            BendNaturalDamp = BendNaturalDmpDataBase.GetMaterialByNameAndSize(Defaults.BendKey, Hydraulics.InDim1);
            UpdateAcousticProperties();
        }

        public static AcuBend FromHydraulics(HydraulicsBend hydraulics)
        {
            return new(hydraulics);
        }

        public AcuBend WithName(string name)
        {
            Name = name;
            return this;
        }

        public AcuBend WithInputLw(double[] inputLw)
        {
            SetInputLw(inputLw);
            return this;
        }

        public AcuBend WithBendMaterial(Material bendMaterial)
        {
            SetBendNatDampeningInstance(bendMaterial);
            return this;
        }

        protected override void CalculateSourceDa()                                   // Obliczenie arraya sourceDa babazujac na geometrii łuku, materiale i przeplywie 
        {
            var bendDampeningValues = Math.Abs(Hydraulics.BrAngle) <= 60
                ? new double[8]
                : BendNaturalDamp.ValuesArray;

            SourceDa = bendDampeningValues;

            CalculateOutputLw();
        }

        protected override void CalculateSourceLw()                                   // Obliczenie arraya sourceDa babazujac na geometrii łuku, materiale i przeplywie 
        {
            SourceLw = PhysicsBranch.CalcBendSourceLw(Hydraulics);
        }

        public void SetBendNatDampeningByKeyName(string materialKey)
        {
            BendNaturalDamp = BendNaturalDmpDataBase.GetMaterialByNameAndSize(materialKey, Hydraulics.InDim1);
            UpdateAcousticProperties();
        }

        public void SetBendNatDampeningInstance(Material material)
        {
            BendNaturalDamp = material ?? throw new ArgumentNullException(nameof(material) + "Material argument cannot be null");
            UpdateAcousticProperties();
        }

        public void SetBendAngle(double angle)
        {
            Hydraulics.SetBranchAngle(angle);
            UpdateAcousticProperties();

        }
    }
}
