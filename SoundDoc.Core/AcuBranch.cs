using System;
using System.Diagnostics;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Physics;

namespace SoundDoc.Core
{
    /**
    *  ACOUSTIC PROPERTIES CLASS - VENTILATION DUCT BRANCH
    */
    public class AcuBranch : AcuBasic<HydraulicsBranch>
    {
        private AcuBranch() : this(HydraulicsBranch.FromDefaults()) { }

        public static AcuBranch FromDefaults() 
        {
            return new();
        }

        private AcuBranch(HydraulicsBranch hydraulics)
            : base(hydraulics)
        {
            UpdateAcousticProperties();
        }

        public static AcuBranch FromHydraulics(HydraulicsBranch hydraulics) 
        {
            return new(hydraulics);
        }

        public AcuBranch WithName(string name) 
        {
            Name = name;
            return this;
        }

        public AcuBranch WithInputLw(double[] inputLw) 
        {
            SetInputLw(inputLw);
            return this;
        }

        protected override void CalculateSourceDa() 
        {
            // throw new NotImplementedException("Do zakodowania");
        }

        protected override void CalculateSourceLw()                          
        {
            SourceLw = PhysicsBranch.CalcBranchSourceLw(Hydraulics);
        }

        public void SetBranchGeometry(double angle, double filetRad, double brDim1, double brDim2 = 0)
        {
            Hydraulics.SetBranchGeometry(angle, filetRad, brDim1, brDim2);
            UpdateAcousticProperties();
        }

        public void SetBranchFlows(double branchVolFlow)
        {
            Hydraulics.SetBranchFlow(branchVolFlow);
            UpdateAcousticProperties();
        }
    }
}