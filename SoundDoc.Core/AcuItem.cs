using SoundDoc.Core.Data;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;
using System;
using System.Linq;
using System.Text;
using SoundDoc.Core.Data.HydraulicModels;

namespace SoundDoc.Core
{
    /**
    *  ACOUSTIC PROPERTIES CLASS - GENERIC ITEM, MANUAL INPUT FOR SOURCELW AND SOURCE DA
    */

    public class AcuItem
    {
        public string Name { get; protected set; }                       
        public double[] SourceLw { get; protected set; }                          
        public double[] SourceDa { get; protected set; }               
        public double[] InputLw { get; protected set; }                 
        public double[] OutputLw { get; private set; }            

        public AcuItem(string name = null, double[] inputLw = null, double[] sourceLw = null, double[] sourceDa = null)
        {
            Name = name ?? Defaults.Name;
            InputLw = inputLw ?? Defaults.InputLwArray;
            SourceLw = sourceLw ?? Defaults.InputLwMinAray;
            SourceDa = sourceDa ?? Defaults.InputLwMinAray;

            ChecklIfContainsNegative(InputLw);
            ChecklIfContainsNegative(SourceDa);
            ChecklIfContainsNegative(SourceLw);

            EnsureMinBackgroundLw();
            CalculateOutputLw();

        }

        protected void EnsureMinBackgroundLw()
        {
            for (var i = 0; i < InputLw.Length; i++)
            {
                InputLw[i] = Math.Max(InputLw[i], Defaults.LwBackr);
                InputLw[i] = Math.Min(InputLw[i], Defaults.LwMax);
            }
        }

        protected static void ChecklIfContainsNegative(double[] array) 
        {
            foreach(double value in array)
            {
                if (value < 0)
                    throw new ArgumentException(nameof(array) + $"Negative value present inside the array at index {Array.IndexOf(array, value)}: {value}.");
            }    
        }

        protected void CalculateOutputLw()
        {
            OutputLw = PhysicsCommon.CalcuOutLwFromSourceLwDa(InputLw, SourceLw, SourceDa);
        }

        public void SetInputLw(double[] inputLw)
        {
            if (inputLw == null)
                throw new ArgumentNullException(nameof(inputLw));

            if (inputLw.Length != 8)
                throw new ArgumentException(nameof(inputLw));

            ChecklIfContainsNegative(inputLw);
            InputLw = inputLw.ToArray();
            EnsureMinBackgroundLw();
            CalculateOutputLw();
        }
        
        public void SetSourceLw(double[] sourceLw) 
        {
            if (sourceLw == null)
                throw new ArgumentNullException(nameof(sourceLw));

            if (sourceLw.Length != Defaults.NumberOfOctaves )
                throw new ArgumentException(nameof(sourceLw));

            ChecklIfContainsNegative(sourceLw);
            SourceLw = sourceLw.ToArray();
            CalculateOutputLw();
        }

        public void SetSourceDa(double[] sourceDa) 
        {
            if (sourceDa == null)
                throw new ArgumentNullException(nameof(sourceDa));

            if (sourceDa.Length != 8 )
                throw new ArgumentException(nameof(sourceDa));

            ChecklIfContainsNegative(sourceDa);
            SourceDa = sourceDa.ToArray();
            CalculateOutputLw();
        }

        public override string ToString()
        {
            return ToString(precision: 1);
        }

        public string ToString(int precision)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"\n{Name}:");
            builder.AppendLine($"InputLw  {InputLw.Format(precision)}");
            builder.AppendLine($"SourceLw {SourceLw.Format(precision)}");
            builder.AppendLine($"SourceDa {SourceDa.Format(precision)}");
            builder.AppendLine($"OutputLw {OutputLw.Format(precision)}");

            return builder.ToString();
        }
    }

    public abstract class AcuBasic<THydraulics> : AcuItem where THydraulics : Hydraulics
    {
        protected AcuBasic(THydraulics hydraulics, string name = null, double[] inputLw = null)
        {
            Name = name ?? Defaults.Name;
            InputLw = inputLw ?? Defaults.InputLwArray;
            Hydraulics = hydraulics ?? throw new ArgumentException();
            EnsureMinBackgroundLw();
        }

        public THydraulics Hydraulics { get; protected set; }

        public void SetHydraulics(THydraulics hydraulics)
        {
            THydraulics Hydraulics = hydraulics ?? throw new ArgumentNullException(nameof(hydraulics));
            UpdateAcousticProperties();
        }
        
        public void SetUpStreamGeometry(double dim1, double dim2 = 0)
        {
            Hydraulics.SetUpStreamGeometry(dim1, dim2);
            UpdateAcousticProperties();
        }

        public virtual void SetUpstreamFlow(double inletVolFlow)
        {
            Hydraulics.SetUpStreamFlow(inletVolFlow);
            UpdateAcousticProperties();
        }

        protected void UpdateAcousticProperties()
        {
            CalculateSourceLw();
            CalculateSourceDa();
            CalculateOutputLw();
        }

        protected abstract void CalculateSourceDa();
        protected abstract void CalculateSourceLw();

    }
}