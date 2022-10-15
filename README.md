## NOISE-DAMPENING-ANALYZER

---

### Noise generation, propagation and dampening calculation library for ventilation ducts based on VDI-2081 standard.

> VERSION: 1.0 - SNAPSHOT <br>
> AUTHOR: PIOTR JAŻDŻYK<br>
> LINKEDIN: https://www.linkedin.com/in/pjazdzyk <br>

---

<span style="font-weight:700;font-size:15px">

[USER GUIDE](USER_GUIDE.md) | [DESCRIPTION](#short-description) | [TECH](#tech) | [FUNCTIONALITY](#current-functionality) |
[COLLABORATION](#collaboration) | [LEGAL DISCLAIMER](#legal-disclaimer) | [REFERENCE SOURCES](#reference-sources) |
[ACKNOWLEDGMENTS](#acknowledgments)

</span>

---

### SHORT DESCRIPTION
This is a C# implementation of noise generation, propagation, and dampening (attenuation) calculation methodology introduced in the german VDI 2081 
standard. The calculation procedure is dedicated to ventilation ducts for typical HVAC air speeds and near-like atmospheric pressures. This tool allows 
for extensive noise propagation and attenuation analysis to ensure that noise reaching the room is within acceptable limits. Each item at the same time 
can have noise veneration and dampening properties (for example, a sound attenuator produces noise despite its dampening properties, therefore in case 
of high velocities,  dampening properties might be consumed by internal noise generation). Calculations are carried out in full noise octave spectrum. 
An additional module has been included allowing for detailed calculation of room sound absorption properties based on internal finish and equipment.<br>
This is an engineering library. It is meant to be used in development of other applications.

### DEVELOPMENT STATUS
This project has been temporarily suspended. I tried to build a marble palace with using my bare hands, where this projects need a team of 5 engineers and skilled programmers. 
If you have a good pair of eyes and can C# and you are interested in participation in development of this project, please contact me!
Check the Architecture folder for presentation, physical concepts explanation I have prepared and concept GUI I have proposed. Originally this project
was meant to be desktop application, but current preference is to go web.

### TECH
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white) &nbsp;
![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) &nbsp;
![image](https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white) &nbsp;
![image](https://img.shields.io/badge/Xunit-25A162?style=for-the-badge) &nbsp;
![image](https://img.shields.io/badge/Math-Net.Numerics-696d74?style=for-the-badge) &nbsp;

### CURRENT FUNCTIONALITY
SYSTEM BUILDING BLOCKS:
- straight duct
- bends / elbows
- branches (tees)
- fans or generic noise sources
- dampers / louvers
- air terminals

INFLOW NOISE ANALYSIS:
- 8 octave sound spectrum analysis
- implemented VDI predefined values, data sets, properties and others,
- automatic airflow distribution and velocity calculation,
- included impact of material of construction or cladding and its sound dampening properties,
- natural dampening effect based on element length and geometry,
- system is being built from smaller blocks added to the calculation matrix, and analyzed as a whole.

ROOM NOISE ANALYSIS:
- typical material sound absorption coefficients list,
- classes for defining absorbing part or item as composed of dampening materials,
- equivalent room sound absorption area calculation,
- included impact of ventilation terminal location inside room

### COLLABORATION
If you are interested in getting involved, these are just a few of items to be developed:
1. I am looking for MEP engineers with programming skills to help in development
2. Factory pattern for easier creation of system building blocks,
3. Factory pattern with predefined typical rooms and absorbing equipment,
4. Database implementation based on Entity Framework,
5. Implementation of silencer module based on VDI,
6. Implementation of external noise propagation through ducting walls,
7. Add noise correction characteristics A,B,C.

### LEGAL DISCLAIMER

This code is my intellectual property. Please have respect for this. You can use it freely in any academic or
non-commercial use if you
properly include the source and author in your references or documentation. For commercial use, please contact me first.

### REFERENCE SOURCES

* [1] - VDI 2081 Part 1 / 2019: "Noise generation and noise reduction"
* [2] - https://www.engineeringtoolbox.com/hydraulic-diameter-rectangular-ducts-d_1004.html
* [3] - A.Pełech. Wentylacja i klimatyzacja - podstawy. ISBN: 978-83-7493-780-1.

## ACKNOWLEDGMENTS
* [Shields.io](https://img.shields.io)
* [Badges 4 README.md](https://github.com/alexandresanlim/Badges4-README.md-Profile)
