using NoiseAnalyzer.Core.AcousticModels;
using NoiseAnalyzer.Core.HydraulicModels;
using NoiseAnalyzer.Core.Repositories;
using NoiseAnalyzer.Core.RoomModels;
using NoiseAnalyzer.Core.Utils;
using SoundDoc.Core;
using System;

namespace NoiseAnalyzer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            UseExample();
        }

        static void UseExample()
        {
            // Suppose we have a noisy fan, 20m of duct, silencer, bend and room outlet.
            // Our goal is to determine noise level at the end of the line.

            // 1. Lets start from defining a flow and geometry properties of our system components:
            var flowRate = 6000.0 /*m3/h*/ / 3600.0; // m3/s
            var ductSideA = 0.5;                     // m
            var ductSideB = 0.5;                     // m    
            var ductLength = 20;                     // m
            var ductGeometry = HydraulicsDuct.Create(flowRate, ductLength, ductSideA, ductSideB);

            var bendAngle = 90.0;                    // deg    
            var bendGeometry = HydraulicsBend.Create(flowRate, bendAngle, ductSideA, ductSideB);

            var fanSpeed = 1000.0;                   // RPM
            var fanType = "HP_RG";                   // -    
            var fanDeltaP = 1200.0;                  // Pa
            var fanHydr = HydraulicsFan.Create(flowRate, fanDeltaP, fanType, fanSpeed, ductSideA, ductSideB);

            var plenuumInletDiameter = 0.3;           // m
            var dischargeCoef = 10.0;                 // -  
            var grossDischArea = 0.05;                // m    
            var terminalPressureLoss = 20.0;          // Pa
            var terminalHydr = HydraulicsTerminal.Create(flowRate, dischargeCoef, grossDischArea, terminalPressureLoss, plenuumInletDiameter);

            /* 
               Note that we do not define geometry for a silencer. Silencer is not implemented yet, but we can
               still include it as a generic sound source using data from any silencer catalogue. As for the fan, typically
               your fan is already selected and you know its properties, based on AHU data sheet, but for this example
               generate our fan based on VDI default fan properties.
            */

            // 2. Defining acoustic properties and acoustic models
            var claddingsRepo = new AbsorbingMaterialsMemoryRepo();
            var mineralWoolCladding = claddingsRepo.GetMaterialByName("blachaOcynk");
            var duct = AcuDuct.FromHydraulics(ductGeometry).WithCladingMaterial(mineralWoolCladding).WithName("duct");
            var bend = AcuBend.FromHydraulics(bendGeometry).WithName("bend");
            var fan = AcuFan.FromHydraulics(fanHydr).WithName("fan");
            var terminal = AcuTerminal.FromHydraulics(terminalHydr).WithLocation(OutletLocation.MiddleOfTheRoom).WithName("terminal");

            // 3. Let's define a silencer as a custom sound source, based on catalogue data
            var name = "Silencer";
            var inputLw = Defaults.InputLwMinAray;                           // default input, it will be override by incoming input noise
            var sourceDa = new double[] { 6, 17, 34, 36, 38, 29, 19, 15 };   // silencer dampening properties in each octave dB
            var sourceLw = new double[] { 59, 55, 50, 46, 42, 38, 35, 32 };  // silencer internal noise sound power, dB
            var silencer = new AcuItem(name, inputLw, sourceLw, sourceDa);

            // 4. Now we can setup analysis matrix and construct our analysis system.
            var analysisMatrix = new AnalysisMatrix();
            analysisMatrix.AddAll(fan, duct, silencer, bend, terminal);
            analysisMatrix.RecalculateAll();

            // 5. Checking the results. Last item in our line is a silencer. Lets check first the noise level leaving the fan,
            // and next, noise level after the last item (bend) to determine total noise reduction.
            Console.WriteLine(fan);
            Console.WriteLine(duct);
            Console.WriteLine(silencer);
            Console.WriteLine(bend);

            // We can observe that duct and bends have some minimal dampening properties and big impact of a silencer.
            // Note that output of each element is input for another in a analyzed system line.

            // 6. Lets create a typical room composed of equipment with absorption properties and next we put our terminal inside
            // At first we need to construct internal equipment and provide information on their area and material finishes.

            AbsorbingMaterialsMemoryRepo materialList = new AbsorbingMaterialsMemoryRepo();
            var coating = materialList.GetMaterialByName("wykladzina");
            var glassPanel = materialList.GetMaterialByName("elementySzklane");
            var conrete = materialList.GetMaterialByName("murBetonSur");
            var humanBody = materialList.GetMaterialByName("czlowiekSiedz");

            var materialCoatingItems = AbsorbingPart.FromMaterialAndArea(coating, 100);
            var glassPanelItems = AbsorbingPart.FromMaterialAndArea(glassPanel, 10);
            var concreteItems = AbsorbingPart.FromMaterialAndArea(conrete, 30);
            var officeOccupant = AbsorbingPart.FromMaterialAndArea(humanBody, 12.0);

            var wallWithWindow = AbsorbingItemFromMaterial.FromParts(concreteItems, glassPanelItems);
            var ceiling = AbsorbingItemFromMaterial.FromParts(concreteItems);
            var floor = AbsorbingItemFromMaterial.FromParts(concreteItems, materialCoatingItems);
            var occupants = AbsorbingItemFromMaterial.FromParts(officeOccupant);

            // 7. Now it is time to setup a room acoustic model:
            var distanceFromObservationPointToTerminal = 3; // m
            var officeRoom = RoomModel.Create()
                .WithName("Office")
                .WithEquipment(wallWithWindow, ceiling, floor, occupants)
                .WithTerminals(terminal)
                .WithMinDistance(distanceFromObservationPointToTerminal);

            // 8. We can check the results now for the room, to observe that room absorption interior had further impact
            // on reducing noise level from our system.

            Console.WriteLine(officeRoom);

        }

    }
}