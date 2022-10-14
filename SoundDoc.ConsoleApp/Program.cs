using SoundDoc.Core;
using System;
using SoundDoc.Core.Data;
using SoundDoc.Core.Data.HydraulicModels;
using SoundDoc.Core.Physics;
using SoundDoc.Core.Data.RoomModels;

namespace SoundDoc.ConsoleApp
{
    class Program
    {
       static void Main(string[] args)
        {
            AcuClassesUseExample();
            NumericsClassesUseExample();
        }
       static void AcuClassesUseExample()
       {
            var bends = new MaterialBendsLists();
            //   Console.WriteLine(bends.ToString());

            var materials = new AbsorbingMaterials();
            //  Console.WriteLine(materials.ToString());

            var flow = 2.0;                 // m3/s
            var brFlow = flow / 3.0;        // m3/s
            var sideA = 0.5;                // m
            var sideB = 0.5;                // m    
            var brSideA = 0.2;              // m
            var brSideB = 0.2;              // m
            var angle = 90.0;               // deg    
            var brAngle = 90.0;             // deg    
            var length = 10.0;              // m
            var dischargeCoef = 10.0;       // -  
            var filetRadius = 0.01;         // m
            var bladeHeight = 0.2;          // m         
            bool damperType = HydraulicsDamper.TypeOpposedBlades;
            var deltaP = 20.0;              // Pa
            var grossDischArea = 0.05;      // m    
            var inDIm3 = 0.3;               // m
            var flowTerminal = 0.25;        // m3/s
            var fanSpeed = 1000.0;          // RPM
            var fanType = "HP_RG";
            var fanDeltaP = 1200.0;         // Pa


            var ductHydr = HydraulicsDuct.Create(flow, length, sideA, sideB);
            var bendHydr = HydraulicsBend.Create(brFlow, angle, sideA, brSideA, sideB, brSideB);
            var branchHydr = HydraulicsBranch.Create(flow, brFlow, brAngle, filetRadius, sideA, brSideA, sideA, sideB, brSideB, sideB);
            var damperHydr = HydraulicsDamper.Create(flow, dischargeCoef, bladeHeight, damperType, sideA, sideB);
            var terminalHydr = HydraulicsTerminal.Create(flowTerminal, dischargeCoef, grossDischArea, deltaP, inDIm3);
            var fanHydr = HydraulicsFan.Create(flow, fanDeltaP, fanType, fanSpeed, sideA, sideB);

            Console.WriteLine("*******************HYDRAULICS TEST*******************");

            Console.WriteLine(ductHydr.ToString());
            Console.WriteLine(bendHydr.ToString());
            Console.WriteLine(branchHydr.ToString());
            Console.WriteLine(damperHydr.ToString());
            Console.WriteLine(terminalHydr.ToString());
            Console.WriteLine(fanHydr.ToString());

            Console.WriteLine("\n*******************ACU TESTS*******************");

            var duct = AcuDuct.FromHydraulics(ductHydr).WithName("duct");
            Console.WriteLine(duct.ToString());

            var bend = AcuBend.FromHydraulics(bendHydr).WithName("bend");
            Console.WriteLine(bend.ToString());

            var branch = AcuBranch.FromHydraulics(branchHydr).WithName("branch");
            Console.WriteLine(branch.ToString());

            var damper = AcuDamper.FromHydraulics(damperHydr).WithName("damper");
            Console.WriteLine(damper.ToString());

            var terminal = AcuTerminal.FromHydraulics(terminalHydr).WithName("terminal");

            Console.WriteLine(terminal.ToString());

            var fan = AcuFan.FromHydraulics(fanHydr).WithName("fan");
            Console.WriteLine(fan.ToString());

            Console.WriteLine("*******************CALC MATRIX TEST*******************");

            var calcMatrix = new CalcMatrix();

            calcMatrix.AddItem(fan);
            calcMatrix.AddItem(duct);
            calcMatrix.AddItem(bend);
            calcMatrix.AddItem(branch);
            calcMatrix.AddItem(damper);
            calcMatrix.AddItem(terminal);

            calcMatrix.RecalculateAll();

            Console.WriteLine(calcMatrix.ToString());


        }
       static void NumericsClassesUseExample() 
        {
            double angle1 = 1;
            double result1 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle1);
            Console.WriteLine("For angle= " + angle1 + "  Zeta= " + result1);

            double angle2 = 10;
            double result2 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle2);
            Console.WriteLine("For angle= " + angle2 + "  Zeta= " + result2);

            double angle3 = 42;
            double result3 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle3);
            Console.WriteLine("For angle= " + angle3 + "  Zeta= " + result3);

            double angle4 = 56;
            double result4 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle4);
            Console.WriteLine("For angle= " + angle4 + "  Zeta= " + result4);

            double angle5 = 75;
            double result5 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle5);
            Console.WriteLine("For angle= " + angle5 + "  Zeta= " + result5);

            double angle6 = 82;
            double result6 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle6);
            Console.WriteLine("For angle= " + angle6 + "  Zeta= " + result6);

            double angle7 = 84;
            double result7 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle7);
            Console.WriteLine("For angle= " + angle7 + "  Zeta= " + result7);

            double angle8 = 85;
            double result8 = PhysicsDamper.ChartsData.CalcOpposedBladesZetaCoef(angle8);
            Console.WriteLine("For angle= " + angle8 + "  Zeta= " + result8);

        }
 
    }
}