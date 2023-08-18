using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace simplesoilModel
{
    //! Simulates pools of water in or on the soil, or on the crop canopy
    class pool
    {
       //! Current evaporation from this pool (millimetres)		
         double evaporation; //Attribute data member
       //! Current rate of transpiration from this pool (millimetres)		
         double transpiration; //Attribute data member
       //! Current input of water via capillary transport to this pool (millimetres)		
         double capillary; //Attribute data member
       //! Current loss of water from the pool via drainage (millimetres)		
         double drainage; //Attribute data member
       //! Capacity of the pool to hold water (millimetres)		
         double maxVolume; //Attribute data member
       /*! Rate at which water in excess of the pool capacity drains to the next pool (millimetres per day).
    Only valid for soil pools.
    */		
         double drainageConst; //Attribute data member
       //! Name of the pool		
         string name; //Attribute data member        

        //! Current amount of water in the pool (millimetres)
          double volume;
        //! Return volume of water in pool (mm)  as double
        public double getvolume()  { return volume;}
        //! Return maximum volume of pool (mm) as double
          public double getmaxVolume() { return maxVolume; }
        //! Set volume of pool
        /*!
         * \param avolume volume to set (mm) as double
         */
           public void setvolume(double avolume) { volume = avolume; }
        //!Set transpiration
        /*!
            * \param avolume volume of transpiration to set (mm) as double
            */
        public void settranspiration(double atranspiration) { transpiration = atranspiration; }
        //! Set maximum volume of pool
        /*!
        * \param avolume a maximum volume to set (mm) as double
        */
        public void setmaxVolume(double amaxVolume) { maxVolume = amaxVolume; }
        //! Set a drainage constant
        /*!
            * \param adrainageConst a drainage constant to set (mm) as double
            */
        public void setdrainageConst(double adrainageConst) { drainageConst = adrainageConst; }
        //! Set name of pool
        /*!
        * \param aname to set as a string
        */
        public void setname(string aname) { name = aname; }
        /*! 
        \param waterInAbove water entering the pool from above (millimetres)
        \param waterInCapillary water entering the pool by capillary transport (millimetres)
        \param theevaporation water leaving the pool by evaporation (millimetres)
        \param thetranspiration water leaving the pool by transpiration (millimetres)
        */
        public double update(double waterInAbove, double waterInCapillary, double theevaporation, double thetranspiration)  
        {
            drainage = 0.0;
	        evaporation = theevaporation;
	        transpiration = thetranspiration;
	        capillary = waterInCapillary;
            double waterIn = waterInAbove + waterInCapillary;
            double waterOut = theevaporation + thetranspiration;
            double currentVol = volume;
            volume = currentVol + waterIn - waterOut;
	        //volume += waterInAbove + waterInCapillary - (theevaporation + thetranspiration);
           if (volume > maxVolume)
           {
               if (maxVolume > 0)
               {
                   drainage = drainageConst * (volume - maxVolume);
                   volume -= drainage;
               }
               else
               {
                   drainage = volume;
                   volume = 0;
               }
           }
           if (volume < 0)
               Console.Write("");
           return drainage;
        }
        //!Initialise a pool
        /*! 
        \param thevolume the initial volume of water in the pool (millimetres)
        \param themaxVolume the initial capacity of the pool (millimetres)
        \param thedrainageConst the initial drainage constant (millimetres per day)
        */
        public void Initialise(double thevolume, double themaxVolume, double thedrainageConst)  
        {
         volume = thevolume;
         maxVolume = themaxVolume;
         drainageConst = thedrainageConst;
         evaporation = 0.0;
         transpiration = 0.0;
         capillary = 0.0;
        }
        //!Simulates the dynamics of snow melting and subtracts evaporation from the snow from the potential evaporation.
        /*! 
        \param potEvap pointer to the potential evaporation
        \param precip precipitation as snow (millimetres of water)
        \param Tmean mean daily temperature (Celsius)
        */
        public double dailySnow(ref double potEvap, double precip, double Tmean)  
        {
           drainage = precip;
           double melt = 0.0;
           double newSnow = 0.0;
           if (potEvap > volume)  //limit evaporation if greater than volume
           {
   	          evaporation = volume;
              potEvap -= evaporation;   //adjust pot evap to reflect loss from pool
              volume = 0.0;
           }
           else
           {
   	        evaporation = potEvap;
   	        volume-=evaporation;
            potEvap = 0.0;
           }

           if (Tmean > 0.0)    //melt some snow if there is any
	        {
              if (volume>0.0)
              {
                 melt = 2.0 * Tmean;
                 if (melt > volume)
                    melt = volume;
              }
	          drainage = melt + precip;   //precipitation drops through pool, add meltwater if necessary
           }
           else
           {
   	          newSnow = precip;
              drainage = 0.0;
           }
           volume+=newSnow - melt;  //do water balance
           return drainage;
        }
        //!Outputs state variables to file
        public void outputDetails()  
        {
            GlobalVars.Instance.writeStartTab("pools");
            GlobalVars.Instance.writeInformationToFiles("name", " name", "-", name, "");
            GlobalVars.Instance.writeInformationToFiles("volume", " volume", "-", volume, "");
            GlobalVars.Instance.writeInformationToFiles("evaporation", " evaporation", "-", evaporation, "");
            GlobalVars.Instance.writeInformationToFiles("transpiration", " transpiration", "-", transpiration, "");
            GlobalVars.Instance.writeInformationToFiles("capillary", " capillary", "-", capillary, "");
            GlobalVars.Instance.writeInformationToFiles("drainage", " drainage", "-", drainage, "");
            GlobalVars.Instance.writeInformationToFiles("maxVolume", " maxVolume", "-", maxVolume, "");
            GlobalVars.Instance.writeInformationToFiles("drainageConst", " drainageConst", "-", drainageConst, "");
            GlobalVars.Instance.writeEndTab();
        }

        //!Outputs header information to debug file0
        /*!
        \param line as integer, = 1 for variable name, = 2 for units
        */
        public void OutputDebugHeader(int line)
        {
            if (line==1)
            GlobalVars.Instance.theZoneData.WriteToDebug("name" + " " + "volume" + " " + "evaporation" + " " + "transpiration" + " " + "capillary" + " "
            + "drainage" + " " + "maxVolume" + " " + "drainageConst" + " ");
            if (line==2)
            GlobalVars.Instance.theZoneData.WriteToDebug("-" + " " + "mm" + " " + "mm" + " " + "mm" + " " + "mm" + " "
            + "mm" + " " + "mm" + " " + "?" + " ");
        }
        //!Outputs information to debug file

        public void OutputDebug()
        {
            GlobalVars.Instance.theZoneData.WriteToDebug(name + " " + volume + " " + evaporation + " " + transpiration + " " + capillary + " "
            + drainage + " " + maxVolume + " " + drainageConst + " ");
        }

        //! Return the deficit between the capacity of the pool and the current volume
        /*!
        \return deficit in mm
        */
        public double getDeficit()  
        {
	        double aVal;
           if ((maxVolume - volume)>0.0)
   	        aVal = (maxVolume - volume);
           else
   	        aVal = 0.0;
	        return aVal;
        }
    }
}
