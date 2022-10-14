using SoundDoc.Core.Data.RoomModels;
using SoundDoc.Core.Extensions;
using SoundDoc.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundDoc.Core.Data.RoomModels
{
    public class RoomModel
    {
        public string RoomName { get; set; }
        public double[] RoomSourceLw { get; private set; }
        public double[] RoomSourceLp { get; private set; }
        public List<AbsorbingItem> RoomEquipmentList { get; private set; }
        public double[] RoomAbsEquivArea { get; private set; }
        public List<IRoomTerminal> RoomTerminalList { get; private set; }
        public double[] RoomOutLp { get; private set; }
        public double RoomSumOutLp { get; private set; }
        public double MinDistR { get; private set; }

        private const double DefaultDistance = 3.0; //m

        private RoomModel(string roomName = "new room")
        {
            RoomName = roomName;
            RoomEquipmentList = new(50);
            RoomAbsEquivArea = Defaults.EmptyOctaveArray;
            RoomSourceLw = Defaults.EmptyOctaveArray;
            RoomSourceLp = Defaults.EmptyOctaveArray;
            RoomOutLp = Defaults.EmptyOctaveArray;
            RoomSumOutLp = 0.0;
            RoomTerminalList = new(10);
            MinDistR = DefaultDistance;
        }

        public static RoomModel Create()
        {
            return new();
        }

        public RoomModel WithName(string roomName)
        {
            RoomName = roomName;
            return this;
        }

        public RoomModel WithTerminals(params IRoomTerminal[] terminals)
        {
            RoomTerminalList = new List<IRoomTerminal>(terminals);
            UpdateAcousticProperties();
            return this;
        }

        public RoomModel WithTerminals(List<IRoomTerminal> terminals)
        {
            RoomTerminalList = terminals;
            UpdateAcousticProperties();
            return this;
        }

        public RoomModel WithRoomSourceLw(double[] roomSrcLw)
        {
            RoomSourceLw = roomSrcLw;
            UpdateAcousticProperties();
            return this;
        }

        public RoomModel WithRoomSourceLp(double[] roomSrcLp)
        {
            RoomSourceLp = roomSrcLp;
            UpdateAcousticProperties();
            return this;
        }

        public RoomModel WithEquipment(params AbsorbingItem[] equipment)
        {
            RoomEquipmentList = new List<AbsorbingItem>(equipment);
            UpdateRoomEquivArea();
            return this;
        }

        public RoomModel WithEquipment(List<AbsorbingItem> equipment)
        {
            RoomEquipmentList = equipment;
            UpdateRoomEquivArea();
            return this;
        }

        public RoomModel WithMinDistance(double exposureDistance)
        {
            SetExposureDistance(exposureDistance);
            return this;
        }

        public void AddAbsorbingItem(AbsorbingItem absItem)
        {
            RoomEquipmentList.Add(absItem);
            UpdateRoomEquivArea();
        }

        public void RemoveAbsorbingItem(int index = -1)
        {
            if (index < 0)
                RoomEquipmentList.RemoveAt(RoomEquipmentList.Count - 1);
            else
                RoomEquipmentList.RemoveAt(index);

            UpdateRoomEquivArea();
        }

        public void AddRoomTerminal(IRoomTerminal terminal)
        {
            RoomTerminalList.Add(terminal);
            UpdateAcousticProperties();
        }

        public void RemoveTerminal(int index = -1)
        {
            if (index < 0)
                RoomTerminalList.RemoveAt(RoomTerminalList.Count - 1);
            else
                RoomTerminalList.RemoveAt(index);

            UpdateAcousticProperties();
        }

        public void SetExposureDistance(double exposureDistance) 
        {
            if (exposureDistance < 0)
                throw new ArgumentException("Distance cannot be negative value.");

            MinDistR = exposureDistance;
            UpdateAcousticProperties();
        }

        public void SetSourceLp(double[] inSourceLp) 
        {
            if (inSourceLp.ContainsNegative())
                throw new ArgumentException("Invalid argument. Negative values detected in passed array.");

            RoomSourceLp = inSourceLp;
            UpdateAcousticProperties();
        }

        public void SetSourceLw(double[] inSourceLw)
        {
            if (inSourceLw.ContainsNegative())
                throw new ArgumentException("Invalid argument. Negative values detected in passed array.");

            RoomSourceLw = inSourceLw;
            UpdateAcousticProperties();
        }

        private void UpdateRoomEquivArea()
        {
            for (int i = 0; i < RoomAbsEquivArea.Length; i++)
                RoomAbsEquivArea[i] = RoomEquipmentList.Select(absItem => absItem.ItemAbsEquivArea[i]).Sum();
            UpdateAcousticProperties();
        }

        private void UpdateAcousticProperties() 
        {
            RoomOutLp = PhysicsRoom.CalcRoomSoundPressureArray(RoomTerminalList, RoomAbsEquivArea, RoomSourceLw, RoomSourceLp, MinDistR);
            RoomSumOutLp = RoomOutLp.LogSum();
        }
    }
}
