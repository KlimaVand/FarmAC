using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
/*! ctool2 models carbon and nitrogen dynamics in the soil */
/*!
 Based on CTool model, expanded to include N
*/
public class ctool2
{
    //! Decomposition rate of fresh organic matter (per year)
    double FOMdecompositionrate = 0;
    //! Decomposition rate of humic organic matter (per year)
    double HUMdecompositionrate = 0;
    //! Decomposition rate of resistant organic matter (per year)
    double ROMdecompositionrate = 0;
    //! Transport function (dimensionless)
    double tF = 0;
    //! Proportion of degraded HUM that is converted to ROM
    double ROMificationfraction = 0;
    //! Proportion of degraded FOM, HUM or ROM that is emitted as CO2
    double fCO2 = 0;
    //! C:N ratio of the HUM pool
    double CtoNHUM = 10.0;// will be overwritten with value from parameter file
    //! C:N ratio of the ROM pool
    double CtoNROM = 10.0;// will be overwritten with value from parameter file
    //! Clay fraction in the soil (should be made layer-specific but is not at the moment)
    double Clayfraction = 0;
    //! Number of soil layers. Currently only works with 2 layers but number is read from parameter file, in case someone wants to have more layers in the future
    int numberOfLayers = 0;
    //! FOMn = N in fresh organic matter, kg N/ha
    public double FOMn = 0;
    //! Maximum depth of soil (m)
    double maxSoilDepth = 0;
    //! Timestep of simulation (yr)
    double timeStep = 1/365.25;
    //!Total CO2 emission (kg/ha)
    double totalCO2Emission = 0;
    //! Total C input (kg/ha)
    double CInput = 0;
    //! Input of N in fresh organic matter (kg/ha)
    double FOMNInput = 0;
    //! Input of N in humic organic matter (kg/ha)
    double HUMNInput = 0;
    //! Total N lost (kg/ha)
    double Nlost = 0;
    //! Time lag of temperature fluctuation (days)
    double offset = 0;
    //! Amplitude of the annual variation in air temperature (Celsius)
    double amplitude = 0;
    //! constant characterizing the decrease in amplitude with an increase in distance from the soil surface
    double dampingDepth = 0;
    //!< a string containing information about the farm and scenario number.
    string parens; 
    //! 
    private bool pauseBeforeExit = false;
    //! C input in FOM (including at start of simulation) (kg/ha)
    double FOMcInput = 0;
    //! C lost as CO2 from FOM (kg/ha)
    double FOMcCO2 = 0;
    //! FOM C degraded into HUM (kg/ha)
    double FOMcToHUM = 0;
    //! C input in HUM  (including at start of simulation) (kg/ha)
    double HUMcInput = 0;
    //! C lost as CO2 from HUM (kg/ha)
    double HUMcCO2 = 0;
    //! HUM C degraded into ROM (kg/ha)
    double HUMcToROM = 0;
    //! C input in ROM  (including at start of simulation) (kg/ha)
    double ROMcInput = 0;
    //! C lost as CO2 from ROM (kg/ha)
    double ROMcCO2 = 0;
    //! Biochar C lost as CO2 (kg/ha)
    double BiocharcCO2 = 0;
    //! C input in biochar (including at start of simulation) (kg/ha)
    double BiocharcInput = 0;

    //! list of instances of SoilClayer in this sequence
    List<SoilClayer> theClayers = new List<SoilClayer>();
    //! get the list of soil C layers
    public List<SoilClayer> GettheClayers() { return theClayers; }

    //! Constructor
    /*!
      \param aparens, a string argument containing information about the farm and scenario number
    */
    public ctool2(string aparens)
    {
        parens = aparens;
    }
    //! Get the N in FOM.
    /*!
      \return the N in FOM (kg/ha) a double value for FOMn.
    */
    public double GetFOMn() { return FOMn; }
    //! Calculate the CN factor that controls the proportion of the initial total C allocated to ROM.
    /*!
     \param InitialCN the initial C:N ration of the soil as a double value.
      \return the CN factor as a double value.
    */
    double CN(double InitialCN)
    {
        return Math.Min(56.2 * Math.Pow(InitialCN, -1.69), 1);
    }
    //! Get the total amount of organic C in the soil.
    /*!
      \return the total amount of organic C in the soil (kg/ha) as a double.
    */
    public double GetOrgC(int layer)
    {
        double retVal = GettheClayers()[layer].GetCstored();
        return retVal;
    }
    //! Initialise the C-Tool simulation
    /*!
     \param soilTypeNo the soil number as an integer argument,
     \param ClayFraction the proportion of clay in the soil as a double argument, 
     \param offsetIn the time lag for the temperature variation in number of days. 
     \param amplitudeIn the amplitude of the annual temperature variation as a double argument (Celsius).
     \param maxSoilDepthIn the maximum soil depth (m) as a double argument
     \param dampingDepthIn the temperature damping depth (m) as a double argument
     \param initialC the initial amount of C in the soil (kg/ha) as a double.
     \param parameterFileName the parameter file name as a string
     \param errorFileName the error file name as a string 
     \param InitialCtoN the initial C:N ration of the soil as a double argument,
     \param pHUMupperLayer the HUM C in the upper soil layer as proportion of the total C in the top layer as a double 
     \param pHUMLowwerLayer the HUM C in the lower soil layer as proportion of the total C in the lower layer as a double 
     \param residualMineralN the residual mineral N in the soil at the end of spinning up the model (kg/ha)
     */
    public void Initialisation(int soilTypeNo, double ClayFraction, double offsetIn, double amplitudeIn, double maxSoilDepthIn, double dampingDepthIn,
        double initialC, string[] parameterFileName, string errorFileName, double InitialCtoN, double pHUMupperLayer, double pHUMLowerLayer,
        ref double residualMineralN)
    {
        amplitude = amplitudeIn;
        maxSoilDepth = maxSoilDepthIn;
        dampingDepth = dampingDepthIn;
        //!rho is the angular frequency of the harmonic oscillation in temperature (2*Pi/P in CTool paper) (1/(seconds per year)
        double rho = GlobalVars.Instance.Getrho();
        double Th_diff = GlobalVars.Instance.theZoneData.thesoilData[soilTypeNo].thermalDiff;
        residualMineralN = 0;
   
        FileInformation ctoolInfo = new FileInformation(parameterFileName);
        ctoolInfo.setPath("constants(0).C-Tool(-1).timeStep(-1)");
        timeStep = ctoolInfo.getItemDouble("Value"); //one day pr year
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "NumOfLayers";
        numberOfLayers = ctoolInfo.getItemInt("Value");
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "FOMdecompositionrate";
        FOMdecompositionrate = ctoolInfo.getItemDouble("Value");
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "HUMdecompositionrate";
        HUMdecompositionrate = ctoolInfo.getItemDouble("Value");
        Clayfraction = ClayFraction;
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "transportCoefficient";
        tF = ctoolInfo.getItemDouble("Value");
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "ROMdecompositionrate";
        ROMdecompositionrate = ctoolInfo.getItemDouble("Value");
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "fCO2";
        fCO2 = ctoolInfo.getItemDouble("Value");
        ctoolInfo.PathNames[ctoolInfo.PathNames.Count - 1] = "ROMificationfraction";
        ROMificationfraction = ctoolInfo.getItemDouble("Value");

        //Set up the soil layers
        SoilClayer aClayer = new SoilClayer(0, FOMdecompositionrate, HUMdecompositionrate, ROMdecompositionrate, tF, ROMificationfraction, fCO2, Clayfraction);
        theClayers.Add(aClayer);
        aClayer = new SoilClayer(aClayer);
        aClayer.setLayerNo(1);
        theClayers.Add(aClayer);

        double CNfactor = CN(InitialCtoN);
        double fractionCtopsoil = 0.47;  //should be moved to parameter file

        CtoNHUM = GlobalVars.Instance.getCNhum();
        CtoNROM = GlobalVars.Instance.getCNhum();

        int NonBaselinespinupYears=0;
        // Check to see if we need to read data from the baseline scenario that has been stored in a file
        if (GlobalVars.Instance.reuseCtoolData != -1)
        {
            FileInformation file = new FileInformation(GlobalVars.Instance.getConstantFilePath());
            file.setPath("constants(0).spinupYearsNonBaseLine(-1)");
            NonBaselinespinupYears = file.getItemInt("Value");
        }
        if ((GlobalVars.Instance.reuseCtoolData != -1)&&(NonBaselinespinupYears==0))
        {
            Console.WriteLine("handover Data from "+ GlobalVars.Instance.getReadHandOverData());
            string[] lines=null;
            try  //look for the file containing the status variables 
            {
                lines = System.IO.File.ReadAllLines(GlobalVars.Instance.getReadHandOverData());
            }
            catch
            {
                GlobalVars.Instance.Error("could not find CTool handover data " + GlobalVars.Instance.getReadHandOverData());
            }
            bool gotit=false;
            for (int j = 0; j < lines.Length; j++)
            {
                string[] data = lines[j].Split('\t');
                if (soilTypeNo == Convert.ToDouble(data[0]))
                {
                    theClayers[0].setFOM(Convert.ToDouble(data[1]));
                    theClayers[1].setFOM(Convert.ToDouble(data[2]));
                    theClayers[0].setHUM(Convert.ToDouble(data[3]));
                    theClayers[1].setHUM(Convert.ToDouble(data[4]));
                    theClayers[0].setROM(Convert.ToDouble(data[5]));
                    theClayers[1].setROM(Convert.ToDouble(data[6]));
                    theClayers[0].setBiochar(Convert.ToDouble(data[7]));
                    theClayers[1].setBiochar(Convert.ToDouble(data[8]));
                    FOMn = Convert.ToDouble(data[9]);
                    residualMineralN = Convert.ToDouble(data[10]);
                    gotit=true;
                }
            }
            if (!gotit)
                GlobalVars.Instance.Error("could not find soil carbon data for soil type " + soilTypeNo.ToString());
            // file.WriteLine(fomc[0].ToString() + '\t' + fomc[1].ToString() + '\t' + humc[0].ToString() + '\t' + humc[1].ToString() + '\t' + humc[0].ToString() + '\t' + humc[1].ToString() + '\t' + FOMn);
            //file.Close();
        }
        else
        {
            //This gives a rough distribution of C with depth
            //The initial amount of C in the different pools has a limited impact on simulations, provided the spinup time is more than about 100 years
            theClayers[0].setHUM(initialC * pHUMupperLayer * CNfactor * fractionCtopsoil);
            theClayers[0].setROM(initialC * fractionCtopsoil - theClayers[0].getHUM());
            theClayers[1].setHUM(initialC * pHUMLowerLayer * CNfactor * (1 - fractionCtopsoil));
            theClayers[1].setROM(initialC * (1 - fractionCtopsoil) - theClayers[1].getHUM());
            theClayers[0].setFOM(initialC * 0.05);
            theClayers[1].setFOM(0.0);
            FOMn = theClayers[0].getFOM()/10.0;  //guess at low C:N - it will all be decomposed before it becomes important
            theClayers[0].setBiochar(0.0);
            theClayers[1].setBiochar(0.0);
        }
        FOMcInput= theClayers[0].getFOM() + theClayers[1].getFOM();
        HUMcInput = theClayers[0].getHUM()+ theClayers[1].getHUM();
        ROMcInput = theClayers[0].getROM() + theClayers[1].getROM();
        BiocharcInput = theClayers[0].getBiochar() + theClayers[1].getBiochar();
        CInput = FOMcInput + HUMcInput + ROMcInput + BiocharcInput;
    }
    //! A copy constructor .
    /*!
      \param C_ToolToCopy instance of ctool2 class to be copied.
    */
    public ctool2(ctool2 C_ToolToCopy)
    {
        totalCO2Emission = C_ToolToCopy.totalCO2Emission;
        CInput = C_ToolToCopy.CInput;
        FOMNInput =C_ToolToCopy.FOMNInput;
        HUMNInput = C_ToolToCopy.HUMNInput;
        Nlost = C_ToolToCopy.Nlost;
        offset = C_ToolToCopy.offset;
        FOMcInput =  C_ToolToCopy.FOMcInput;
        FOMcCO2 = C_ToolToCopy.FOMcCO2;
        FOMcToHUM = C_ToolToCopy.FOMcToHUM;
        HUMcInput = C_ToolToCopy.HUMcInput;
        HUMcCO2 = C_ToolToCopy.HUMcCO2;
        HUMcToROM = C_ToolToCopy.HUMcToROM ;
        ROMcInput = C_ToolToCopy.ROMcInput ;
        ROMcCO2 = C_ToolToCopy.ROMcCO2;
        BiocharcCO2 = C_ToolToCopy.BiocharcCO2;
        BiocharcInput = C_ToolToCopy.BiocharcInput;

        maxSoilDepth = C_ToolToCopy.maxSoilDepth;
//        fCO2 = C_ToolToCopy.fCO2;
        FOMn = C_ToolToCopy.FOMn;
        CtoNHUM = C_ToolToCopy.CtoNHUM;
        CtoNROM = C_ToolToCopy.CtoNROM;
        amplitude = C_ToolToCopy.amplitude;
        dampingDepth = C_ToolToCopy.dampingDepth;
        numberOfLayers = C_ToolToCopy.numberOfLayers;
        //theClayers= new List<SoilClayer>();
        SoilClayer aSoilCLayer = new SoilClayer(C_ToolToCopy.GettheClayers()[0]);
        theClayers.Add(aSoilCLayer);
        aSoilCLayer = new SoilClayer(C_ToolToCopy.GettheClayers()[1]);
        theClayers.Add(aSoilCLayer);
    }
    //! Resets the amount of C in the layers to earlier values
    /*!
     * This is used in the CropClass model, when iterating to converge on the correct production and input of organic matter to the soil 
     \param original ctool2 instance.
    */
    public void reloadC_Tool(ctool2 original)
    {
        CInput = original.CInput;
        totalCO2Emission = original.totalCO2Emission;
        FOMn = original.FOMn;
        FOMNInput = original.FOMNInput;
        HUMNInput = original.HUMNInput;
        Nlost = original.Nlost;
        theClayers[0].CopySoilClayer(original.theClayers[0]);
        theClayers[1].CopySoilClayer(original.theClayers[1]);
        FOMcCO2 = original.FOMcCO2;
        FOMcToHUM = original.FOMcToHUM;
        HUMcCO2 = original.HUMcCO2;
        HUMcToROM= original.HUMcToROM;
        ROMcCO2 = original.ROMcCO2;
        BiocharcCO2 = original.BiocharcCO2;
        offset = original.offset;
        FOMcInput = original.FOMcInput;
        HUMcInput = original.HUMcInput;
        ROMcInput = original.ROMcInput;
        BiocharcInput = original.BiocharcInput;
        maxSoilDepth = original.maxSoilDepth;
        amplitude = original.amplitude;
        dampingDepth = original.dampingDepth;
        numberOfLayers = original.numberOfLayers;
    }
    //! Get mumber of soil layers. 
    /*!
     * The number of layers is currently 2
      \return an integer value for mumber of soil layers.
    */
    public int GetnumOfLayers() { return numberOfLayers; }

    //! Get the amount of C stored in the soil.
    /*!
      \return the amount of C stored in the soil (kg/ha) a as double value 
    */
    public double GetCStored()
    {
        double Cstored = 0;
        Cstored = GettheClayers()[0].GetCstored() + GettheClayers()[1].GetCstored();
        return Cstored;
    }
    //! Get the amount of FOM C stored in the soil.
    /*!
      \return the amount of FOM C stored in the soil (kg/ha) a as double value 
    */
    public double GetFOMCStored()
    {
        double FOMCstored = 0;
        FOMCstored = GettheClayers()[0].getFOM() + GettheClayers()[1].getFOM();
        return FOMCstored;
    }
    //! Get the amount of HUM C stored in the soil.
    /*!
      \return the amount of HUM C stored in the soil (kg/ha) a as double value 
    */
    public double GetHUMCStored()
    {
        double HUMCstored = 0;
        HUMCstored = GettheClayers()[0].getHUM() + GettheClayers()[1].getHUM();
        return HUMCstored;
    }
    //! Get the amount of ROM C stored in the soil.
    /*!
      \return the amount of ROM C stored in the soil (kg/ha) a as double value 
    */
    public double GetROMCStored()
    {
        double ROMCstored = 0;
        ROMCstored = GettheClayers()[0].getROM() + GettheClayers()[1].getROM();
        return ROMCstored;
    }
    //! Get the amount of biochar C stored in the soil.
    /*!
      \return the amount of biochar C stored in the soil (kg/ha) a as double value 
    */
    public double GetBiocharCStored()
    {
        double BiocharCstored = 0;
        BiocharCstored = GettheClayers()[0].getBiochar() + GettheClayers()[1].getBiochar();
        return BiocharCstored;
    }
    //! Get the amount of N stored in the soil.
    /*!
      \return the amount of N stored in the soil (kg/ha) a as double value 
    */
    public double GetNStored()
    {
        double Nstored = FOMn;
        Nstored += GettheClayers()[0].getHUM() / CtoNHUM + GettheClayers()[1].getHUM() / CtoNHUM + GettheClayers()[0].getROM() / CtoNROM + GettheClayers()[1].getROM() / CtoNROM;
        return Nstored;
    }
    //! Get the amount of HUM N stored in the soil.
    /*!
      \return the amount of HUM N stored in the soil (kg/ha) a as double value 
    */
    public double GetHUMn() { return GetHUMCStored() / CtoNHUM; }
    //! Get the amount of ROM N stored in the soil.
    /*!
      \return the amount of ROM N stored in the soil (kg/ha) a as double value 
    */
    public double GetROMn() { return GetROMCStored() / CtoNROM; }

    //! Check that the C balance is closed.
    /*!
      Only used when debugging. Will generate an error if the C balance is not closed
    */
    public void CheckCBalance()
    {
        double Cstored = GetCStored();
        double CBalance = CInput - (Cstored + totalCO2Emission);
        double diff = CBalance / CInput;
        if (Math.Abs(diff) > 0.05)
        {
            double errorPercent = 100 * diff;
            string messageString=("Error; C balance in C-Tool\n");
            messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
            GlobalVars.Instance.Error(messageString);
        }
    }
    //! Do the C and N dynamics for a given period
    /*!
     \param writeOutput if set to true, data will be sent to file
     \param julianDay Julian day on which to start the period an integer argument,
     \param startDay starting date as a long integer
     \param endDay end date as a long argument
     \param FOM_Cin a two-dimensional array of double [month, layer] of fresh organic matter C input (kg C/ha)
     \param HUM_Cin a two-dimensional array of double [month, layer] of humic organic matter C input (kgC/ha)
     \param Biochar_Cin a two-dimensional array of double [month, layer] of biochar C input (kg C/ha)
     \param FOMnIn a double array [month] of N input in fresh organic matter (kg/ha)
     \param cultivation a double array [month] of depth of cultivation (m) (not used yet)
     \param meanTemperature a double array [month] containing the mean monthly air temperature (Celcius)
     \param droughtIndex a double array [month] containing the mean monthly drought index (1=no inhibition of degradation, 0 = complete inhibition)
     \param Cchange, change in carbon in the soil over the period (kg/ha) 
     \param CO2Emission C emitted as CO2 (kg/ha)
     \param Clearched C lost by leaching (kg/ha) (not used)
     \param Nmin, mineralisation of soil N over the period (kg/ha) (negative if N is immobilised)
     \param Nleached N leached from the soil in organic matter (kg/ha)
     \param CropSeqID an integer containing the ID of the crop sequence (to inform the output file)
     */
    public XElement Dynamics(bool writeOutput, int julianDay, long startDay, long endDay, double[,] FOM_Cin, double[,] HUM_Cin, double[,] Biochar_Cin, double[] FOMnIn, 
        double[] cultivation, double[] meanTemperature, double[] droughtIndex, ref double Cchange, ref double CO2Emission,
        ref double Cleached, ref double Nmin, ref double Nleached,  int CropSeqID)
    {
        //Note that some variables are in units of C(e.g.FOMCO2) even though this is not indicated in the name
  
        Cchange = 0;
        Nmin = 0;
        FOMNInput = 0;
        double FOMC_InputThisCrop = 0;
        double HUMC_InputThisCrop = 0;
        double BiocharC_InputThisCrop = 0.0;
        double FOMnmineralised = 0;
        double CStart = GetCStored();  //total C in soil (kg/ha)
        double NStart = GetNStored(); //total N in soil (kg/ha)
        double startFOM = GetFOMCStored();
        double startHUM = GetHUMCStored();  //initial HUM C in soil (kg/ha)
        double startROM = GetROMCStored();// initial ROM C in soil (kg/ha)
        double FOMCO2 = 0; //emission of C as CO2 from FOM degradation (kg/ha)
        double HUMCO2 = 0; //emission of C as CO2 from HUM degradation (kg/ha)
        double ROMCO2 = 0; //emission of C as CO2 from ROM degradation (kg/ha)
        double BiocharCO2 = 0; //emission of C as CO2 from biochar degradation (kg/ha)
        CO2Emission = 0;
        Nleached = 0;
        Cleached = 0;
        long iterations = endDay - startDay+1; //duration of period to simulate (days)
        double balance = 0;  //used to see if the budget is closed
        
        XElement ctoolData = new XElement("ctool"); 
        if ((GlobalVars.Instance.Ctoolheader == false)&&(writeOutput))  //see if we need to write headers for file output
        {
            int times = 3;
            bool printUnits = false;
            bool printValues = false;
            for (int j = 0; j < times; j++)
            {
                if (j == 1)
                    printUnits = true;
                else
                    printUnits = false;
                if (j == 2)
                {
                    printValues = true;
                    GlobalVars.Instance.Ctoolheader = true;
                }
                else
                    printValues = false;
                if (writeOutput)
                {
                    GlobalVars.Instance.writeCtoolFile("CropSeqID", "CropSeqID", "CropSeqID", CropSeqID, parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("startDay", "startDay", "day", startDay.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("endDay", "endDay", "day", endDay.ToString(), parens, printValues, printUnits, 0);

                    GlobalVars.Instance.writeCtoolFile("FOMCStoredStart", "Initial C FOM", "MgC/ha", GetFOMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMCStoredStart", "Initial C HUM", "MgC/ha", GetHUMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("ROMCStoredStart", "Initial C ROM", "MgC/ha", GetROMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("BiocharCStoredStart", "Initial C ROM", "MgC/ha", GetBiocharCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("FOMnStoredStart", "Initial N FOM", "MgN/ha", GetFOMn().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMnStoredStart", "Initial N HUM", "MgN/ha", GetHUMn().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("ROMnStoredStart", "Initial N ROM", "MgN/ha", GetROMn().ToString(), parens, printValues, printUnits, 0);

                    GlobalVars.Instance.writeCtoolFile("FOMC_InputThisCrop", "FOM_C_input", "MgC/ha/period", FOMC_InputThisCrop.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMC_InputThisCrop", "HUM_C_input", "MgC/ha/period", HUMC_InputThisCrop.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("BiocharC_InputThisCrop", "Biochar_C_input", "MgC/ha/period", BiocharC_InputThisCrop.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("CO2Emission", "CO2_C_emission", "MgC/ha/period", CO2Emission.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("balance", "balance", "MgC/ha/period", balance.ToString(), parens, printValues, printUnits, 0);

                    GlobalVars.Instance.writeCtoolFile("FOMCStoredEnd", "Final_C_FOM", "MgC/ha", GetFOMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMCStoredEnd", "Final_C_HUM", "MgC/ha", GetHUMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("ROMCStoredEnd", "Final_C_ROM", "MgC/ha", GetROMCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("BiocharCStoredEnd", "Final_C_ROM", "MgC/ha", GetBiocharCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("SoilOrganicCarbon", "Final_C_Total", "MgC/ha", GetCStored().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("FOMnStoredEnd", "Final_N_FOM", "MgN/ha", GetFOMn().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMnStoredEnd", "Final_N_HUM", "MgN/ha", GetHUMn().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("ROMnStoredEnd", "Final_N_ROM", "MgN/ha", GetROMn().ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("Total_soil_N", "Total_soilN_Total", "MgN/ha", GetFOMn() + GetHUMn() + GetROMn(), parens, printValues, printUnits, 0);

                    GlobalVars.Instance.writeCtoolFile("FOMNInput", "FOMNin", "MgN/ha/period", FOMNInput.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("HUMNInput", "HUMNin", "MgN/ha/period", 0.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("Nmin", "Nmin", "MgN/ha/period", Nmin.ToString(), parens, printValues, printUnits, 0);

                    GlobalVars.Instance.writeCtoolFile("Org_N_leached", "Org_N_leached", "MgN/ha/period", Nleached.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("NStart", "NStart", "MgN/ha", NStart.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("Nend", "Nend", "MgN/ha", 0.ToString(), parens, printValues, printUnits, 0);
                    GlobalVars.Instance.writeCtoolFile("FOMnmineralised", "FOMnmineralised", "MgN/ha/period", FOMnmineralised, parens, printValues, printUnits, 1);
                }
            }
        }
        //Calculate the maximum and minimum air temperature
        double min = 999999;
        double max = 0;
        for (int j = 0; j < 12; j++)
        {
            if (meanTemperature[j] < min)
                min = meanTemperature[j];

            if (meanTemperature[j] > max)
                max = meanTemperature[j];
        }
        amplitude = (max - min) / 2;
        if (writeOutput)
        {
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("CropSeqID", "CropSeqID", "CropSeqID", CropSeqID, parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("startDay", "startDay", "day", startDay.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("endDay", "endDay", "day", endDay.ToString(), parens, true, false, 0));

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetFOMCStored", "Initial C FOM", "MgC/ha", GetFOMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetHUMCStored", "Initial C HUM", "MgC/ha", GetHUMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetROMCStored", "Initial C ROM", "MgC/ha", GetROMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetBiocharCStored", "Initial C ROM", "MgC/ha", GetBiocharCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetFOMn", "Initial N FOM", "MgN/ha", GetFOMn().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetHUMn", "Initial N HUM", "MgN/ha", GetHUMn().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetROMn", "Initial N ROM", "MgN/ha", GetROMn().ToString(), parens, true, false, 0));
        }

        double cumFOMnmineralised = 0;  //total amount of FOM mineralized (kg C/ha)
        double cumhumificationAmount = 0; //total amount of FOM converted to HUM (kg C/ha)
        double totFOMCO2 = 0;

        for (int i = 0; i < iterations; i++)
        {
			if (julianDay >= 365)
                julianDay = 1;
            double JulianAsDouble=(double)julianDay;
            int month = (int) Math.Floor(JulianAsDouble / 30.4166)+1;
            double timestep = 1 / 365.25;
            //Variables to contain the transport of material by leaching in and out of layers (kg C/ha)
            double FOMtransportIn=0;
            double FOMtransportOut=0;
            double HUMtransportIn=0;
            double HUMtransportOut = 0;
            double ROMtransportIn = 0;
            double ROMtransportOut = 0;
            double BiochartransportOut = 0;
            double BiochartransportIn = 0;
            //double startFOM = GetFOMCStored();  //Initial mass of FOM in the soil (kg C/ha)
            double newFOM = 0; //Total new FOM input to soil (kg/ha)
            double cumFOMCO2 = 0; //Total loss of C from FOM as CO2 (kg/ha)
            double newHUM = 0; //Total new HUM created by decomposition of FOM (kg/ha)
            double newROM = 0;//Total new ROM input to soil (kg/ha)
            double FOMmineralised = 0;//Total new created by decomposition of HUM (kg/ha)
            for (int j = 0; j < 2; j++)
            {
                FOMC_InputThisCrop += FOM_Cin[i, j];
                FOMcInput += FOM_Cin[i, j];
                HUMC_InputThisCrop += HUM_Cin[i, j];
                HUMcInput += HUM_Cin[i, j];
                BiocharC_InputThisCrop += Biochar_Cin[i, j];
                BiocharcInput += Biochar_Cin[i, j];
                double depthInLayer = (100.0) / numberOfLayers * j + (100.0) / numberOfLayers / 2;
                double temp = Temperature(meanTemperature[month - 1], julianDay, depthInLayer, amplitude, offset);
                double tempCofficent = temperatureCoefficent(temp);
                theClayers[j].layerDynamics(timestep, j, FOMdecompositionrate, HUMdecompositionrate, ROMdecompositionrate,
                    tF, fCO2, ROMificationfraction,tempCofficent, droughtIndex[month-1], FOMtransportIn, ref FOMtransportOut, 
                    ref FOMCO2, HUMtransportIn, ref HUMtransportOut, ref HUMCO2, ROMtransportIn, ref ROMtransportOut, ref ROMCO2,
                        BiochartransportIn, ref BiochartransportOut, ref BiocharCO2,ref newHUM, ref newROM);

                CO2Emission += FOMCO2 + HUMCO2 + ROMCO2 + BiocharCO2;
                FOMcCO2 += FOMCO2;
                HUMcCO2 += HUMCO2;
                ROMcCO2 += ROMCO2;
                BiocharcCO2 += BiocharCO2;
                FOMcToHUM += newHUM;
                HUMcToROM += newROM;
                cumFOMCO2 += FOMCO2;
                cumhumificationAmount += newHUM;
                FOMmineralised += FOMCO2 + newHUM;
                FOMtransportIn = FOMtransportOut;
                HUMtransportIn = HUMtransportOut;
                ROMtransportIn = ROMtransportOut;
                BiochartransportIn = BiochartransportOut;
                GettheClayers()[j].addFOM(FOM_Cin[i, j]);
                GettheClayers()[j].addHUM(HUM_Cin[i, j]);
                GettheClayers()[j].addBiochar(Biochar_Cin[i, j]);
                newFOM += FOM_Cin[i, j];
            }
            totFOMCO2 += cumFOMCO2;
            //last value of C transport out equates to C leaving the soil
            double FOMntransportOut = (FOMtransportOut * FOMn) / GetFOMCStored();
            if (startFOM > 0)
                FOMnmineralised = FOMmineralised * FOMn / startFOM;
            else
                FOMnmineralised = 0;
            cumFOMnmineralised += FOMnmineralised;
            FOMNInput += FOMnIn[i];
            FOMn += FOMnIn[i] - FOMnmineralised ;
            julianDay++;
        }
        CInput += FOMC_InputThisCrop + HUMC_InputThisCrop + BiocharC_InputThisCrop;
        double CEnd = GetCStored();  //total C stored in the soil at the end of the period (kg/ha)
        Cchange = CEnd - CStart;
        totalCO2Emission += CO2Emission;
        double Nend = GetNStored();  //total N stored in the soil at the end of the period (kg/ha)
        double HUMNInput = HUMC_InputThisCrop / CtoNHUM;  //HUM N can be calculated from HUM C because we use a fixed C:N for HUM
        Nmin = NStart + FOMNInput + HUMNInput - Nend - Nleached;
        if (Nmin < 0.0)
            Console.WriteLine();
        // Check that we can close C balances in pools
        balance = FOMcInput - (FOMcCO2  + GetFOMCStored() + FOMcToHUM);
        if (Math.Abs(balance) > 0.001)
            Console.WriteLine("Error in FOM balance in ctool2 Dynamics");
        balance = HUMcInput + FOMcToHUM - (HUMcCO2 + GetHUMCStored() + HUMcToROM);
        if (Math.Abs(balance) > 0.001)
            Console.WriteLine("Error in HUM balance in ctool2 Dynamics");
        balance = ROMcInput + HUMcToROM - ROMcCO2 - GetROMCStored();
        if (Math.Abs(balance) > 0.001)
            Console.WriteLine("Error in ROM balance in ctool2 Dynamics");
        balance = BiocharcInput - BiocharcCO2 - GetBiocharCStored();
        if (Math.Abs(balance) > 0.001)
            Console.WriteLine("Error in biochar balance in ctool2 Dynamics");
        balance = CStart + FOMC_InputThisCrop + HUMC_InputThisCrop + BiocharC_InputThisCrop - (CO2Emission + CEnd);
        if (Math.Abs(balance) > 0.001)
            Console.WriteLine("Error in Ctool balance in ctool2 Dynamics");

        CheckCBalance(); //enable if need to check if the C balance is closed
        if (writeOutput)
        {
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("FOMC_InputThisCrop", "FOM_C_input", "", FOMC_InputThisCrop.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("HUMC_InputThisCrop", "HUM_C_input", "", HUMC_InputThisCrop.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("BiocharC_InputThisCrop", "Biochar_C_input", "", BiocharcInput.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("CO2Emission", "CO2_C_emission", "", CO2Emission.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("balance", "balance", "", balance.ToString(), parens, true, false, 0));

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetFOMCStored", "Final_C_FOM", "", GetFOMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetHUMCStored", "Final_C_HUM", "", GetHUMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetROMCStored", "Final_C_ROM", "", GetROMCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetBiocharCStored", "Final_C_Biochar", "", GetBiocharCStored().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetCStored", "Final_C_Total", "", GetCStored().ToString(), parens, true, false, 0));

            double finalN = GetFOMn() + GetHUMn() + GetROMn();

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetFOMn", "Final_N_FOM", "", GetFOMn().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetHUMn", "Final_N_HUM", "", GetHUMn().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("GetROMn", "Final_N_ROM", "", GetROMn().ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("Total_soil_N","Total_soilN_Total","", finalN.ToString(), parens, true, false, 0));

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("FOMNInput", "FOMNin", "", FOMNInput.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("HUMNInput", "HUMNin", "", HUMNInput.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("Nmin", "Nmin", "", Nmin.ToString(), parens, true, false, 0));

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("Org_N_leached", "Org_N_leached", "", Nleached.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("NStart", "NStart", "", NStart.ToString(), parens, true, false, 0));
            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("Nend", "Nend", "", Nend.ToString(), parens, true, false, 0));

            ctoolData.Add(GlobalVars.Instance.writeCtoolFile("FOMnmineralised", "FOMnmineralised", "", cumFOMnmineralised, parens, true, false, 1));                
        }
        return ctoolData;
    }
    //!  Calculate Temperature. Taking five arguments and
    /*!
      \param avgTemperature the average temperature (Celsius) as a double argument
      \param day a double argument
      \param depth depth in the soil (m) as a double argument
      \param amplitude amplitude of the temperature variation (Celsius) as a double argument
      \param offset offset from a double argument
      \return a double value for temperature (Celsius).
    */
    public double Temperature(double avgTemperature, double day, double depth, double amplitude, double offset)
    {
        //! constant characterizing the decrease in amplitude with an increase in distance from the soil surface
        double retVal = avgTemperature + amplitude * Math.Exp(-depth / dampingDepth) * Math.Sin(GlobalVars.Instance.Getrho() * (day + offset) * 24.0 * 3600.0 - depth / dampingDepth);
        return retVal;
    }
    
    //! Calculate the temperature coefficent.
    /*!
      \param temperature temperature (Celsius) as a double argument
      \return a double value for the temperature coefficent.
    */
    private double temperatureCoefficent(double temperature)
    {
	    return 7.24*Math.Exp(-3.432+0.168*temperature*(1-0.5*temperature/36.9)); 
    }
    //! Write details. 
    public void Write()
    {

        GlobalVars.Instance.writeStartTab("Ctool2");
        GlobalVars.Instance.writeInformationToFiles("timeStep", "Timestep", "Day", timeStep, parens);
        GlobalVars.Instance.writeInformationToFiles("numberOfLayers", "Number of soil layers", "-", numberOfLayers, parens);
        GlobalVars.Instance.writeInformationToFiles("FOMdecompositionrate", "FOM decomposition rate", "per day", FOMdecompositionrate, parens);
        GlobalVars.Instance.writeInformationToFiles("HUMdecompositionrate", "HUM decomposition rate", "per day", HUMdecompositionrate, parens);
        GlobalVars.Instance.writeInformationToFiles("ROMdecompositionrate", "ROM decomposition rate", "per day", ROMdecompositionrate, parens);
        GlobalVars.Instance.writeInformationToFiles("ROMificationfraction", "ROMification rate", "per day", ROMificationfraction, parens);
        GlobalVars.Instance.writeInformationToFiles("fCO2", "fCO2", "-", fCO2, parens);
        GlobalVars.Instance.writeInformationToFiles("GetFOMCStored", "GetFOMCStored", "-", GetFOMCStored(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetHUMCStored", "GetHUMCStored", "-", GetHUMCStored(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetROMCStored", "GetROMCStored", "-", GetROMCStored(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetBiocharCStored", "GetBiocharCStored", "-", GetBiocharCStored(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetFOMn", "GetFOMn", "-", GetFOMn(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetHUMn", "GetHUMn", "-", GetHUMn(), parens);
        GlobalVars.Instance.writeInformationToFiles("GetROMn", "GetROMn", "-", GetROMn(), parens);
        GlobalVars.Instance.writeEndTab();
       
    }
}