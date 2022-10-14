using SoundDoc.Abstractions.Models;
using SoundDoc.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundDoc.Core.Data.RoomModels
{
    /**
    * AbsorbingItem: used when object equivalent sound absorbing area is provided
    */
    public class AbsorbingItem
    {
        public string ItemName { get; set; }
        public double[] ItemAbsEquivArea { get; private set; }
        public int ItemQuantity { get; private set; }

        protected AbsorbingItem(double[] itemEqArea, int itemQty=1, string itemName="new item") 
        {
            ItemName = itemName;
            SetItemAbsEquivArea(itemEqArea);
            SetItemQuantity(itemQty);
        }
        
        public static AbsorbingItem FromEquivArea(double[] itemEqArea) 
        {
            return new(itemEqArea);
        }

        public AbsorbingItem WithQuantity(int itemQty) 
        {
            SetItemQuantity(itemQty);
            return this;
        }

        public AbsorbingItem WithName(string itemName)
        {
            ItemName = itemName;
            return this;
        }

        public void SetItemAbsEquivArea(double[] itemEqArea) 
        {
            ItemAbsEquivArea = itemEqArea ?? throw new ArgumentNullException(nameof(itemEqArea) + " Item equivalent area cannot be null");
        }

        public void SetItemQuantity(int itemQty)
        {
            if (itemQty < 0)
                throw new ArgumentException(nameof(itemQty) + "Item quantity cannot be negative value.");
           
            ItemQuantity = itemQty;
        }
    }

    /**
    * AbsorbingItemFromMaterial: used when object equivalent sound absorbing area must be calculated from finishing material properties
    * Each object can be made of many parts, where each of these parts can bade from different material.
    */
    public class AbsorbingItemFromMaterial : AbsorbingItem
    {
        public List<AbsorbingPart> AbsorbingPartList { get; private set; }

        private AbsorbingItemFromMaterial() : base(Defaults.EmptyOctaveArray)
        {
            AbsorbingPartList = new List<AbsorbingPart>();
        }
 
        public static AbsorbingItemFromMaterial Create()
        {
            return new();
        }

        public static AbsorbingItemFromMaterial FromParts(params AbsorbingPart[] values) 
        {
            var absItem = Create();
            values.ToList().ForEach(part => absItem.AddAbsorbingPart(part));
            return absItem;
        }

        public void AddAbsorbingPart(AbsorbingPart part) 
        {
            AbsorbingPartList.Add(part);
            UpdateEquivArea();
        }

        public void RemoveAbsorbingPart(int index) 
        {
            AbsorbingPartList.RemoveAt(index);
            UpdateEquivArea();
        }

        private void UpdateEquivArea() 
        {
            for (int i = 0; i < ItemAbsEquivArea.Length; i++) 
                ItemAbsEquivArea[i] = AbsorbingPartList.Select(part => part.PartEquivArea[i]).Sum();      
        }

        public void EditPartAtIndex(int index, Material newMaterial, double newArea) 
        {
            AbsorbingPartList[index].SetPartMaterial(newMaterial);
            AbsorbingPartList[index].SetPartArea(newArea);
            UpdateEquivArea();
        }

        public void EditPartOfName(string name, Material newMaterial, double newArea)
        {
            int partIndex = AbsorbingPartList.FindIndex(part => part.PartName.Equals(name));
            EditPartAtIndex(partIndex, newMaterial, newArea);
        }
    }

    /**
    * AbsorbingPart: Single part made of one type of material (or set of exactly the same parts).
    */
    public class AbsorbingPart
    {
        public string PartName { get; set; }
        public Material PartMaterial { get; private set; }
        public double PartArea { get; private set; }
        public double[] PartEquivArea { get; private set; }
        public int PartQuantity { get; private set; }

        private AbsorbingPart(Material partMaterial, double partArea, string partName = "new part", int partQty = 1)
        {
            if (partArea < 0 || partQty < 0)
                throw new ArgumentException("Negative value exception. " + nameof(partArea) + " = " + partArea
                                                                         + nameof(partQty) + " = " + partQty);
            PartMaterial = partMaterial ?? Defaults.MaterialDef;
            PartArea = partArea;
            PartQuantity = partQty;
            PartName = partName;
            UpdatePartSoundAbsEquivAreaArray();
        }

        public static AbsorbingPart FromMaterialAndArea(Material partMaterial, double partArea)
        {
            return new(partMaterial, partArea);
        }

        public AbsorbingPart WithName(string name)
        {
            PartName = name;
            return this;
        }

        public AbsorbingPart WithQuantity(int qty)
        {
            SetPartQuantity(qty);
            return this;
        }

        private void UpdatePartSoundAbsEquivAreaArray() 
        {
            PartEquivArea = PartMaterial.ValuesArray.Select(alphaCoef => PhysicsRoom.CalcRoomEquivSoundAbsArea(alphaCoef, PartArea)*PartQuantity).ToArray();
        }
            
        public void SetPartMaterial(Material material) 
        {
            PartMaterial = material ?? throw new ArgumentNullException(nameof(material) + " Material cannot be null.");
            UpdatePartSoundAbsEquivAreaArray();
        }

        public void SetPartArea(double area)
        {
            if (area < 0)
                throw new ArgumentException(nameof(area) + "Area cannot be negative value.");

            PartArea = area;
            UpdatePartSoundAbsEquivAreaArray();
        }

        public void SetPartQuantity(int qty) 
        {
            if (qty < 0)
                throw new ArgumentException(nameof(qty) + "Quantity cannot be negative value.");

            PartQuantity = qty;
            UpdatePartSoundAbsEquivAreaArray();
        }
    }
}
