## NOISE-DAMPENING-ANALYZER - USER GUIDE WITH EXAMPLES

> VERSION: 1.0 - SNAPSHOT <br>
> AUTHOR: PIOTR JAŻDŻYK<br>
> LINKEDIN: https://www.linkedin.com/in/pjazdzyk <br>

---

<span style="font-weight:700;font-size:15px">

[HOME](README.md) | [DUCT NETWORK MODEL](#duct-network-model) | [ROOM MODEL](#room-model) |  [USE EXAMPLE](#use-example)

</span>

---

### DUCT NETWORK MODEL

This library is dedicated to noise propagation and dampening analysis inside ventilation ducts. The physical phenomena
of sound propagation inside a closed system are complex, counter-intuitive, and independent of flow direction. This software
allows for full analysis of selected parts of the system, what we would call from here on a dampening line. Before
conducting any calculations, you must carefully analyze your network and decide which line is potentially closest to the
fan or any other source installed in the system. The dampening line will consist set of components of a given system, starting from
the outside, through the fans, sources, duct, bends, branches louvers, or any other equipment, and ending at the terminal inside a room.
The 1D topology node-network concept has been adopted for calculation algorithms as the closest analogy to the real component behavior.
Each of the components is composed of two parts: the hydraulic part which is a composition of flow and geometrical properties, and the acoustic
part which represents noise generation, dampening, and propagation concepts. Both hydraulic and acoustic terms behave as a node,
having their own input and output. A simple example is shown below:
![IMAGE](/Architecture/Images/Connectivity.png)
This allows us to build a chain of components, where the output of the previous element is an input to the next element,
representing the simple model of flow and noise propagation concept. Noise will be modified by the acoustic properties of
a given element and sent through output port to another element.
![IMAGE](/Architecture/Images/ConnectivityLarge.png)
Plenty of typical ventilation duct equipment both in terms of geometry and acoustic properties has been already implemented
based on VDI-2081 data. In the case of non-standard elements, the library provides generic classes for implementing custom components.
Elements must be added to the calculation matrix in the correct order, starting from the main noise source up to the terminal or room.

### ROOM MODEL

One of the most crucial aspects of the ventilation system design is to prevent excessive noise propagation to the room.
The noise inside the room the human ear can experience is not a sound power itself but the sound pressure. Sound pressure
depends strongly on room absorption properties (equivalent absorption area), noise source location, and distance from this source.
Room equipment and materials used for finishes have a strong impact on what extent room can absorb some portion of the noise.
Each material can have different acoustic-dampening properties. Because of this property of equipment to reduce some noise, we will
call them AbsorbingItems. Each piece of AbsorbingItem can be constructed of AbsorbingParts. Each absorbing part is composed of
some materials with predefined acoustic properties and information on the net area of this part. See the drawing below for a better illustration:
![IMAGE](/Architecture/Images/RoomModelAnalogy1.png)
The location of the terminal plays important role in noise propagation. Location is defined by the direction coefficient. Distance from the source
should be carefully determined, specific to your analysis target. If no other guidelines are provided, analyze the room layout and
measure the real spatial distance between the terminal to the nearest working place.
It is also possible to define room background noise as uniformly dispersed sound power or sound pressure field if you have any on-site
measurements' data.
![IMAGE](/Architecture/Images/RoomModelAnalogy2.png)
The room model might sound a bit complicated, therefore I have provided a simple dependency diagram of classes as shown below.
The architecture of this library always delegates calculations to a physics module, which is a collection of static methods, to
be reused by any component at any time, similarly as the Java Math class works.
![IMAGE](/Architecture/Images/RoomModelDiagram.png)
I hope I was able to cover the most relevant information, which should help you to understand better the example part. If you have any questions
about this model functionality do not hesitate to ask me.


### USE EXAMPLE

Suppose we have a noisy fan, 20m of duct, silencer, bend, and a room outlet composed of a few absorbing items. Now we would like to
perform noise analysis to determine if our silencer is good enough.<br>

Let's start by defining the flow and geometry properties of our system components:

```
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
```
Note that we do not define geometry for a silencer. The silencer is not implemented yet, but we can
still include it as a generic sound source using data from any silencer catalog. As for the fan, typically
your fan is already selected, and you know its properties, based on the AHU datasheet, but for this example
generate our fan based on VDI default fan properties. The next step is defining acoustic properties and acoustic models:

```
var claddingsRepo = new AbsorbingMaterialsMemoryRepo();
var mineralWoolCladding = claddingsRepo.GetMaterialByName("blachaOcynk");
var duct = AcuDuct.FromHydraulics(ductGeometry).WithCladingMaterial(mineralWoolCladding).WithName("duct");
var bend = AcuBend.FromHydraulics(bendGeometry).WithName("bend");
var fan = AcuFan.FromHydraulics(fanHydr).WithName("fan");
var terminal = AcuTerminal.FromHydraulics(terminalHydr).WithLocation(OutletLocation.MiddleOfTheRoom).WithName("terminal");
```
Let's define a silencer as a custom sound source, based on catalogue data.

```
var name = "Silencer";
var inputLw = Defaults.InputLwMinAray;  // default input, it will be override by incoming input noise
var sourceDa = new double[] { 6, 17, 34, 36, 38, 29, 19, 15 };   // silencer dampening properties in each octave dB
var sourceLw = new double[] { 59, 55, 50, 46, 42, 38, 35, 32 };  // silencer internal noise sound power, dB
var silencer = new AcuItem(name, inputLw, sourceLw, sourceDa);
```
Now we can set up analysis matrix and construct our analysis system:

```
var analysisMatrix = new AnalysisMatrix();
analysisMatrix.AddAll(fan, duct, silencer, bend, terminal);
analysisMatrix.RecalculateAll();
```
Checking the results. The last item in our line is a silencer. Let's check first the noise level leaving the fan,
and next, the noise level after the last item (bend) to determine total noise reduction.

```
fan:
InputLw  [10  10  10  10  10  10  10  10]
SourceLw [93.4  91.5  88.8  85  80.4  74.9  68.5  61.2]
SourceDa [0  0  0  0  0  0  0  0]
OutputLw [93.4  91.5  88.8  85  80.4  74.9  68.5  61.2]

duct:
InputLw  [93.4  91.5  88.8  85  80.4  74.9  68.5  61.2]
SourceLw [52.8  47.5  41.7  35.5  29.1  22.7  16.2  9.7]
SourceDa [1.8  1.8  1.8  2.3  2.7  2.7  2.7  2.7]
OutputLw [91.7  89.8  87  82.8  77.8  72.3  65.9  58.6]

Silencer:
InputLw  [91.7  89.8  87  82.8  77.8  72.3  65.9  58.6]
SourceLw [59  55  50  46  42  38  35  32]
SourceDa [6  17  34  36  38  29  19  15]
OutputLw [85.7  72.9  54.8  49.4  44.1  44.4  47.2  43.9]

bend:
InputLw  [85.7  72.9  54.8  49.4  44.1  44.4  47.2  43.9]
SourceLw [55.5  51  45.8  39.9  33.6  26.8  19.7  12.2]
SourceDa [0  0  6  8  4  3  3  3]
OutputLw [85.7  72.9  50.5  43.8  41  41.6  44.2  40.9]
```
We can observe that ducts and bends have some minimal dampening properties and a big impact on a silencer.
Note that the output of each element is input for another in the analyzed system line.

Let's create a typical room composed of equipment with absorption properties and next we put our terminal inside
At first, we need to construct internal equipment and provide information on their area and material finishes.

```
AbsorbingMaterialsMemoryRepo materialList = new AbsorbingMaterialsMemoryRepo();
var coating = materialList.GetMaterialByName("wykladzina");
var glassPanel = materialList.GetMaterialByName("elementySzklane");
var concrete = materialList.GetMaterialByName("murBetonSur");
var humanBody = materialList.GetMaterialByName("czlowiekSiedz");

var materialCoatingItems = AbsorbingPart.FromMaterialAndArea(coating, 100);
var glassPanelItems = AbsorbingPart.FromMaterialAndArea(glassPanel, 10);
var concreteItems = AbsorbingPart.FromMaterialAndArea(concrete, 30);
var officeOccupant = AbsorbingPart.FromMaterialAndArea(humanBody, 12.0);

var wallWithWindow = AbsorbingItemFromMaterial.FromParts(concreteItems, glassPanelItems);
var ceiling = AbsorbingItemFromMaterial.FromParts(concreteItems);
var floor = AbsorbingItemFromMaterial.FromParts(concreteItems, materialCoatingItems);
var occupants = AbsorbingItemFromMaterial.FromParts(officeOccupant);
```
Now it is time to set up a room acoustic model:

```
var distanceFromObservationPointToTerminal = 3; // m
var officeRoom = RoomModel.Create()
          .WithName("Office")
          .WithEquipment(wallWithWindow, ceiling, floor, occupants)
          .WithTerminals(terminal)
          .WithMinDistance(distanceFromObservationPointToTerminal);
```
When we print room models results, following output is expected:

```
Office:
Room internal source Lw [0  0  0  0  0  0  0  0] dB
Room internal source Lp [0  0  0  0  0  0  0  0] dB
Room resulting Lp [70.7  54.2  32.6  25.1  20.6  19.9  22.4  19] dB
Total room equivalent absorbption area [3.9  10.4  8.9  10.8  17.6  26.7  28.2  29.2] m2
Distance from terminal 3 m
Total room sound power Lp 70.8 dB
```
As it can be observed that room dampening has a strong impact on the acoustic environment.

