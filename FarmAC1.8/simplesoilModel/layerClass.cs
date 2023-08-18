using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace simplesoilModel
{
//! describes a single layer in the soil water model
    class soilWaterLayerClass
    {
      //! Depth below the soil surface of the lower boundary of the layer (millimetres)
      double z_lower;
      //! Water content of the layer at field capacity, mm/mm
      double fieldCapacity;
      //! Thickness of the layer (metres)
      double thickness;  
      //! Water below permanent wilting point, mm/mm
      double capacityAtPWP;
        //! Get the depth below the soil surface of the lower boundary of the layer (millimetres)
        /*
         \return depth (mm)
         */
        public double getz_lower()  { return z_lower;}
        //! Get water content of the layer at field capacity, mm/mm
        /*
         \return water content at field capacity (mm/mm)
         */
        public double getfieldCapacity()  { return fieldCapacity;}
        //! Get thickness of the layer
        /*
         \return thickness (mm)
         */
        public double getthickness() { return thickness; }
        //! Set field capacity
        /*
        \param field capacity (mm/mm)
        */
        public void setfieldCapacity(double afieldCapacity) { fieldCapacity = afieldCapacity; }
        //! Set water below permanent wilting point, mm/mm
        /*
        \param permanent wilting point (mm/mm)
        */
        public void setPWP(double aVal) { capacityAtPWP = aVal; }
        //! Get water below permanent wilting point, mm/mm
        /*
         \return permanent wilting point (mm/mm)
         */
        public double getPWP() { return capacityAtPWP; }
        //Get mm of water when below permanent wilting point
        /*
         \return amount of water in the layer that is below the permanent wilting point (mm)
         */
        public double GetWaterBelowPWP() { return capacityAtPWP * thickness; }
        //Get mm of plant-available water
        /*
         \return amount of water in the layer that is available to plants (mm)
         */
        public double GetPlantAvailableWater() { return (fieldCapacity - capacityAtPWP) * thickness; }
        //! Copy constructor
        /*
        \param instance of soilWaterLayerClass to copy
        */
        public soilWaterLayerClass(soilWaterLayerClass alayerClass)  
            {
             z_lower = alayerClass.z_lower;
             fieldCapacity = alayerClass.fieldCapacity;
             thickness = alayerClass.thickness;
            }
            //! Default constructor
          public soilWaterLayerClass()
          {
          }
        //! Initialise an instance of this class
        /*! 
        \param z_upper depth below the soil surface of the upper boundary of the layer
        \param az_lower depth below the soil surface of the lower boundary of the layer
        \param afieldCapacity field capacity (mm/mm) of the soil in the layer
        \param aPWP permanent wilting point (mm/mm) of the soil in the layer
        */
        public void Initialise(double z_upper, double az_lower, double afieldCapacity, double aPWP)  
        {
            z_lower = az_lower;
            fieldCapacity = afieldCapacity/100.0;
            capacityAtPWP = aPWP / 100;
            thickness = z_lower - z_upper;
        }
    }
}
