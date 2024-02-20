using System.Collections.Generic;
using System.Xml;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
/*! A class that contains variables and functions that may need to be accessed from several classes in the model*/
public class GlobalVars
{
    private static GlobalVars instance;
    //! A structure totCFom that stores information about carbon.
    /*!
      with two variables .
    */
    public struct totCFom
    {
        public string parants;
        public double amounts;

    };
    //! an instance
    public List<totCFom> alltotCFom = new List<totCFom>();
    //! A normal member. Add variables to structure totCFom. Taking two arguments.
    /*!
      \param amount, a double value.
      \param parant, a string value.
    */
    public void addtotCFom(double amount, string parant)
    {
        totCFom tmp;
        tmp.parants = parant;
        tmp.amounts = amount;
        alltotCFom.Add(tmp);
    }
    //! A structure totNFom.
    /*!
      with two variables .
    */
    public struct totNFom
    {
        public string parants;
        public double amounts;

    };
    public List<totNFom> alltotNFom = new List<totNFom>();
    //! A normal member. Add variables to structure totNFom. Taking two arguments.
    /*!
      \param amount, a double value.
      \param parant, a string value.
    */
    public void addtotNFom(double amount, string parant)
    {
        totNFom tmp;
        tmp.parants = parant;
        tmp.amounts = amount;
        alltotNFom.Add(tmp);
    }
    //! is false until a header has been written to the Field output file 
    public bool headerField;
    //! is false until a header has been written to the livestock output file 
    public bool headerLivestock;
    //! is false until a header has been written to the C-Tool output file 
    public bool Ctoolheader;
    //! Used to keep track of execution times
    private Stopwatch sw;
    //! Summary data are output to this file
    System.IO.StreamWriter SummaryExcel;
    //! Default constructor.
    /*!
      set header variables to false .
    */
    private GlobalVars()
    {
        headerField = false;
        headerLivestock = false;
        Ctoolheader = false;
    }
    //! A GlobaleVars class instance.
    /*!
      return class instance as a static pointer that can be used anywhere.
    */
    public static GlobalVars Instance
    {
        get
        {
            if (instance == null)
            {
                //!Create new instance of GlobalVars
                instance = new GlobalVars();
                //! Create instance of Stopwatch for keeping track of timing
                instance.sw = new Stopwatch();
                instance.sw.Start();
            }
            return instance;
        }
    }
    ///Returns the maximum number of manure type that should be associated with the StorageID
    private int maxManureTypes = 25;
    //! Get the manure type associated with a storage type.
    /*!
      \param StorageID the ID of the manure storage type as an integer value.
      \return manure type ID as an integer value.
    */
    public int getManureStorageID(int StorageID)
    {
        int[] ManureType = new int[maxManureTypes];
        if (StorageID > maxManureTypes - 1)
        {
            string messageString = "Error - attempting to access a manure type that is not recognised";
            GlobalVars.instance.Error(messageString);
        }
        ManureType[0] = 0;
        ManureType[1] = 2;
        ManureType[2] = 3;
        ManureType[3] = 0;
        ManureType[4] = 13;
        ManureType[5] = 7;
        ManureType[6] = 6;
        ManureType[7] = 1;
        ManureType[8] = 3;
        ManureType[9] = 0;
        ManureType[10] = 1;
        ManureType[11] = 3;
        ManureType[12] = 5;
        ManureType[13] = 12;
        ManureType[14] = 1;
        ManureType[15] = 1;
        ManureType[16] = 13;
        ManureType[17] = 1;
        ManureType[18] = 1;
        ManureType[19] = 13;
        ManureType[20] = 13;
        ManureType[21] = 10;
        ManureType[22] = 9;
        ManureType[23] = 1;
        ManureType[24] = 3;
        return ManureType[StorageID];
    }
    //! A structure grazedItem.
    /*!
      Stores information used to identify the contribution of grazed DM 
    * and to enable the distribution of excreta and emissions to grazed fields.
    */
    public struct grazedItem
    {
        public double urineC;  //!urine C (kg)
        public double urineN;//!urine N (kg)
        public double faecesC; //!faecal C (kg)
        public double faecesN; //!faecal N (kg)
        public double ruminantDMgrazed; //! dry matter grazed by ruminants (kg)
        public double fieldDMgrazed; //! record of dry matter harvested by grazing (k)
        public double fieldCH4C; //! emission of methane associated with excreta deposited during grazing 
        public string name;  //! Name of the feed item
        public string parens; /*!< a string containing information about the farm and scenario number.*/
        //! Write data to xml file 
        public void Write()
        {
            GlobalVars.Instance.writeStartTab("GrazedItem");
            GlobalVars.Instance.writeInformationToFiles("name", "Name", "-", name, parens);
            GlobalVars.Instance.writeInformationToFiles("fieldDMgrazed", "FieldDMgrazed", "kg", fieldDMgrazed, parens);
            GlobalVars.Instance.writeInformationToFiles("ruminantDMgrazed", "ruminantDMgrazed", "kg", ruminantDMgrazed, parens);
            GlobalVars.Instance.writeEndTab();
        }
    }
    //! Array used to store information about the grazed feed items
    public grazedItem[] grazedArray = new grazedItem[maxNumberFeedItems];
    //! Array used to store information about the feed items used on the farm
    public product[] allFeedAndProductsUsed = new product[maxNumberFeedItems];
    //! Array used to store information about the feed items produced on the farm
    public product[] allFeedAndProductsProduced = new product[maxNumberFeedItems];
    //! Array used to store information about the potential yield of feed items on the farm
    public product[] allFeedAndProductsPotential = new product[maxNumberFeedItems];
    //! Array used to store information about the difference between production and use of feed items on the farm
    public product[] allFeedAndProductTradeBalance = new product[maxNumberFeedItems];
    //! Array used to store information about the modelled yield of feed items on the farm
    public product[] allFeedAndProductFieldProduction = new product[maxNumberFeedItems];
    //! Array used to store information about the grazed feed items that are produced on the farm but not used (returned to soil as crop residues)
    private product[] allUnutilisedGrazableFeed = new product[maxNumberFeedItems];

    //constants
    //! C-Tool parameter; the proportion of decomposed fresh organic matter that is deposited in humic organic matter
    private double humification_const;
    //! Concentration of C in organic matter (volatile solids) (kg/kg)
    private double alpha;
    //! Universal gas constant (J/(mol K) Not used at present
    private double rgas;
    //! C:N ratio of humic organic matter (HUM and ROM in C-Tool)
    private double CNhum;
    //! Proportion of biogas C that is methane (kg/kg)
    private double tor;
    //! Apparent activation energy (J/mol) Not used at present
    private double Eapp;
    //! Conversion from kg CO2-C to kg CO2
    private double C_CO2 = (12 + 2 * 16) / 12;
    //! 100 year GWP of methane C (kg CO2eq/kg CH4-C)
    private double CO2EqCH4;
    //! 100 year GWP of nitrous oxide N (kg CO2eq/kg N2O-N)
    private double CO2EqN2O;
    //! 100 year GWP of the change of the carbon stored in the soil
    private double CO2EqsoilC;
    //! kg N2O-N emitted/kg NH3-N emitted
    private double IndirectNH3N2OFactor;
    //! kg N2O-N emitted/kg NO3-N leached
    private double IndirectNO3N2OFactor;
    //! Concentration of C in default bedding (kg C/kg DM)
    private double defaultBeddingCconc;
    //! Concentration of N in default bedding (kg N/kg DM)
    private double defaultBeddingNconc;
    //! General maximum error margin (proportion)
    private double maxToleratedError;
    //! Maximum error in difference between grazed DM and production of grazed DM (proportion)
    private double maxToleratedErrorGrazing;
    //! Maximum number of iterations of the crop yield model before an error is reported
    private int maximumIterations;
    //! Leaching fraction - IPCC (2006)
    private double EFNO3_IPCC;
    //! Conversion factor for digestible energy to metabolizable energy (MJ/MJ)
    private double digestEnergyToME = 0.81;
    //! Minimum length of the period over which flows will be calculated when using projection mode (years)
    private int minimumTimePeriod;
    //! Minimum length of the period over which flows will be calculated when using adaptation mode (years)
    private int adaptationTimePeriod;
    //! List of ID of inventory systems (e.g. IPCC (2006), IPCC (2019)
    private List<int> theInventorySystems;
    //! ID of the inventory system currently used
    private int currentInventorySystem;
    //! Current factorial energy system for use in livestock
    private int currentEnergySystem;
    //! If true, the production and consumption of grazed DM must be (nearly) the same. If false, any deficit or surplus will be imported or exported
    bool strictGrazing;
    // If true, a log file will be produced
    public bool logFile;
    //! If true, the text sent to the log file will be echoed on the console
    public bool logScreen;
    //! Determines the level of detail of logging information
    public int verbosity;
    //! If true, any error message is returned to the server rather than written to an error file
    public bool returnErrorMessage = false;
    //! If true, full results will be written to an output xml file
    public bool Writeoutputxlm;
    //! If true, full results will be written to an output Excel (csv) file
    public bool Writeoutputxls;
    //! If true, the C-Tool results will be written to an output xml file
    public bool Writectoolxlm;
    //! If true, the C-Tool results will be written to an output Excel file
    public bool Writectoolxls;
    //! If true, the some results will be written to an output Excel file (used for debugging)
    public bool WriteDebug;
    //! If true, the livestock results will be written to an Excel file
    public bool Writelivestock;
    //! If true, the field results will be written to an Excel file
    public bool WriteField;
    //! If true, the crop results will be written to an Excel file
    public bool WriteCrop;
    //! If true, summary results will be written to an Excel file
    public bool WriteSummaryExcel;
    //! If true, the initial C-Tool variables will be read from a file
    public int reuseCtoolData;
    //!if true, the C-TOOL pools for each crop sequence will be preserved but the areas must not change. If false, pools within a soil type will be merged and areas can change.            
    bool lockSoilTypes = false; 
    //! File stream for the log file
    public System.IO.StreamWriter logFileStream;
    //! Number of the agro-ecological zone
    private int zoneNr;
    //! Get Humification_const. 
    /*!
      \return humification constant as a double value.
    */
        public double getHumification_const() { return humification_const; }
    //!  Get the concentration of C in organic matter (volatile solids) (kg/kg)
    /*!
      \return alpha as a double value.
    */
    public double getalpha() { return alpha; }
    //!  Get universal gas constant
    /*!
      \return gas constant as a double value.
    */
    public double getrgas() { return rgas; }
    //!  Get CNhum C:N of humic organic matter
    /*!
      \return CNhum as a double value.
    */
    public double getCNhum() { return CNhum; }
    //!  Get proportion of biogas C that is methane
    /*!
      \return tor as a double value.
    */
    public double gettor() { return tor; }
    //!  Get Eapp apparent activation energy
    /*!
      \return Eapp as a double value.
    */
    public double getEapp() { return Eapp; }
    //!  Get CO2eq of methane C.
    /*!
      \return CO2EqCH4 as a double value.
    */
    public double GetCO2EqCH4() { return CO2EqCH4; }
    //!  Get CO2eq of nitrous oxide N.
    /*!
      \return CO2EqN2O as a double value.
    */
    public double GetCO2EqN2O() { return CO2EqN2O; }
    //!  Get CO2EqsoilC.
    /*!
      \return CO2EqsoilC as a double value.
    */
    public double GetCO2EqsoilC() { return CO2EqsoilC; }
    //!  Get conversion factor for CO2-C to CO2.
    /*!
      \return conversion factor as a double value.
    */
    public double GetC_CO2() { return C_CO2; }
    //!  Get IndirectNH3N2OFactor, the kg N2O-N emitted/kg NH3-N emitted
    /*!
      \return IndirectNH3N2OFactor as a double value.
    */
    public double GetIndirectNH3N2OFactor() { return IndirectNH3N2OFactor; }
    //!  Get IndirectNO3N2OFactor, kg N2O-N emitted/kg NO3-N leached.
    /*!
      \return IndirectNO3N2OFactor as a double value.
    */
    public double GetIndirectNO3N2OFactor() { return IndirectNO3N2OFactor; }
    //!  Get minimumTimePeriod (years) for running the model in projection mode
    /*!
      \return minimumTimePeriod as an integer value.
    */
    public int GetminimumTimePeriod() { return minimumTimePeriod; }
    //!  Get adaptationTimePeriod (years) for spinning up the model.
    /*!
      \return adaptationTimePeriod as an integer value.
    */
    public int GetadaptationTimePeriod() { return adaptationTimePeriod; }
    //!  Get strictGrazing. 
    /*!
      \return true if the production and consumption of grazed dry matter must balance.
    */
    public bool GetstrictGrazing() { return strictGrazing; }
    //!  Get maximumIterations, the maximum number of iterations of the crop yield modelling before giving up and generating an error.
    /*!
      \return maximumIterations as an integer value.
    */
    public int GetmaximumIterations() { return maximumIterations; }
    //!  Get digestEnergyToME, the conversion of digestible energy to metabolizable energy.
    /*!
      \return conversion factor as a double value.
    */
    public double GetdigestEnergyToME() { return digestEnergyToME; }
    //!  Get the number of the agroecological zone.
    /*!
      \return agroecological zone number as an integer value.
    */
    public int GetZone() { return zoneNr; }
    //!  Set the number of the agroecological zone
    /*!
      \param zone number as an integer argument.
    */
    public void SetZone(int zone) { zoneNr = zone; }
    //!  Get defaultBeddingCconc, the concentration of C in default bedding DM (kg C/kg DM).
    /*!
      \return defaultBeddingCconc as a double value.
    */
    public double getdefaultBeddingCconc() { return defaultBeddingCconc; }
    //!  Get defaultBeddingNconc, the concentration of N in default bedding DM (kg N/kg DM).
    /*!
      \return defaultBeddingNconc as a double value.
    */
    public double getdefaultBeddingNconc() { return defaultBeddingNconc; }
    //!  Get ID number of currentInventorySystem.
    /*!
      \return ID number as an integer value.
    */
    public int getcurrentInventorySystem() { return currentInventorySystem; }
    //!  Get ID number of the currentEnergySystem.
    /*!
      \return ID number as an integer value.
    */
    public int getcurrentEnergySystem() { return currentEnergySystem; }
    //!  Get EFNO3_IPCC.
    /*!
      \return a double value.
    */
    public double getEFNO3_IPCC() { return EFNO3_IPCC; }
    //!  Get maxToleratedError.
    /*!
      \return a double value.
    */
    public double getmaxToleratedError() { return maxToleratedError; }
    //!  Get maxToleratedErrorGrazing, the maximum imbalance between produced and consumed grazed DM (proportion).
    /*!
      \return maxToleratedErrorGrazing as a double value.
    */
    public double getmaxToleratedErrorGrazing() { return maxToleratedErrorGrazing; }
    //!  Get lock SoilTypes, if true, the C-TOOL pools for each crop sequence will be preserved but the areas must not change. If false, pools within a soil type will be merged and areas can change.            
    /*!
      \return lockSoilTypes as a boolean value.
    */
    public bool GetlockSoilTypes() { return lockSoilTypes; }
    //!  Set ID of currentInventorySystem
    /*!
      \param aVal, an integer argument.
    */
    public void setcurrentInventorySystem(int aVal) { currentInventorySystem = aVal; }
    //!  Set ID of currentEnergySystem. 
    /*!
      \param aVal, an integer argument.
    */
    public void setcurrentEnergySystem(int aVal) { currentEnergySystem = aVal; }
    //!  Set strictGrazing. Set true if the production and consumption of grazed dry matter must balance.
    /*!
      \param aVal, a boolean argument.
    */
    public void SetstrictGrazing(bool aVal) { strictGrazing = aVal; }
    //!  Get rho, the parameter in the soil temperature damping.
    /*!
      \return rho as a double value.
    */
    public double Getrho() { return 3.1415926 * 2.0 / (365.0 * 24.0 * 3600.0); }
    //! If true, the simulation will stop when an error is encountered
    private bool stopOnError;
    //! If true, the return button must be pressed when in the Console window, before the model exits
    private bool pauseBeforeExit;
    //!  Set pauseBeforeExit. Set true, if the return button must be pressed when in the Console window, before the model exits 
    /*!
      \param stop, a boolean argument.
    */
    public void setPauseBeforeExit(bool stop) { pauseBeforeExit = stop; }
    //!  Get pauseBeforeExit.
    /*!
      \return pauseBeforeExit as a boolean value.
    */
    public bool getPauseBeforeExit() { return pauseBeforeExit; }
    //!  Set stopOnError. Set true if simulation should stop on an error.
    /*!
      \param stop, a boolean argument.
    */
    public void setstopOnError(bool stop) { stopOnError = stop; }
    //!  Get stopOnError. 
    /*!
      \return stopOnError as a boolean value.
    */
    public bool getstopOnError() { return stopOnError; }

    static string parens; /*!< a string containing information about the farm and scenario number.*/
    //! Sets ErrorMessageReturn to null. 
    /*!
      ErrorMessageReturn holds any error message so it can be returned to the server
    */
    public void ResetErrorMessageReturn()
    { AnimalChange.model.errorMessageReturn = ""; }
    //!  ReSet. Taking one argument.
    /*!
      \param aparens, a string argument.
    */
    public void reset(string aparens)
    {
        instance = null;
        parens = aparens;
        FileInformation information = new FileInformation();
        information.reset();
    }

    //! If true, it forces exit with error if energy requirements not met
    private bool RunFullModel;
    //!  Set Run FullModel. Taking one argument.
    /*!
      \param aVal, a boolean argument.
    */
    public void setRunFullModel(bool aVal) { RunFullModel = aVal; }
    //!  Get Run FullModel. Returning a boolean value.
    /*!
      \return a boolean value.
    */
    public bool getRunFullModel() { return RunFullModel; }
    //!  Get ECM. Taking three arguments and returing a double value.
    /*!
      \param litres, a double argument.
      \param percentFat, a double argument.
      \param percentProtein, a double argument.
      \return a double value
    */
    public double GetECM(double litres, double percentFat, double percentProtein)
    {
        double retVal = litres * (0.383 * percentFat + 0.242 * percentProtein + 0.7832) / 3.1138;
        return retVal;
    }
    //! A structure zoneSpecificData.
    /*!
      more details.
    */
    public struct zoneSpecificData
    {

        private string debugFileName;
        private System.IO.StreamWriter debugfile;

        //! An internal member in structure, Set debugFileName. Taking one argument.
        /*!
          \param aName, a string argument.
        */
        public void SetdebugFileName(string aName) { debugFileName = aName; }


        public double[] airTemp;
        private double[] droughtIndex;
        public double[] Precipitation;
        public double[] PotentialEvapoTrans;
        public int[] rainDays;
        private int numberRainyDaysPerYear;
        private double Ndeposition;

        //! An internal structure fertiliserData.
        /*!
         data read from parameters.xml, fertilisers or manure tag.
        */
        public struct fertiliserData
        {
            public int manureType;
            public int speciesGroup; //livestock type for this manure (not used for fertilisers)
            public double fertManNH3EmissionFactor; //NH3 emission factor for field-applied manure or fertiliser (read from EFNH3)
            public double EFNH3FieldTier2; //Tier 2 NH3 emission for fertiliser (read from EFNH3FieldTier2)
            public double fertManNH3EmissionFactorHousingRefTemperature;
            public string name;
        }
        //! An internal structure C_ToolData.
        /*!
         data read from C_Tool.
        */
        public struct C_ToolData
        {
            public double initialC;
            public double InitialFOMCtoN;
            public double InitialFOM;
            public double InitialCtoN;
            public double pHUMupperLayer;
            public double pHUMlowerLayer;

        }
        //! An internal structure soilLayerData.
        /*!
         data read from soilLayer.
        */
        public struct soilLayerData
        {
            public double z_lower;
            public double fieldCapacity;
        }
        //! An internal structure soilWaterData.
        /*!
         data read from soilWater.
        */
        public struct soilWaterData
        {
            public double drainageConstant;
            public List<soilLayerData> thesoilLayerData;
        }
        //! An internal structure soilData.
        /*!
         data read from soil.
        */
        public struct soilData
        {
            public double N2Factor;
            public string name;
            public double ClayFraction;
            public double SandFraction;
            public double maxSoilDepth;
            public double dampingDepth;
            public double thermalDiff;
            public double GetdampingDepth() { return dampingDepth; }
            public List<C_ToolData> theC_ToolData;
            public soilWaterData thesoilWaterData;

            //! Return the damping depth. 
            /*!
              \param thermalDiff the thermal diffusivity of the soil
              \param rho the angular frequency of the harmonic oscillation in temperature (2*Pi/P in CTool paper) (1/(seconds per year)

              \return a double value for damping depth (m).
            */

            public double CalcDampingDepth(double thermalDiff, double rho)
            {
                return Math.Sqrt(3600 * 24 * 2.0 * thermalDiff / rho);
            }
            public string Getname() { return name; }
            public double GetSoilDepth() { return maxSoilDepth; }
        }
        //! An internal structure manureAppData.
        /*!
         data read from manureApp.
        */
        public struct manureAppData
        {
            public double NH3EmissionReductionFactor;
            public string name;
        }

        public List<fertiliserData> theFertManData;
        public List<soilData> thesoilData;
        public List<manureAppData> themanureAppData;
        double urineNH3EmissionFactor;
        double manureN20EmissionFactor;
        double fertiliserN20EmissionFactor;
        double residueN2OEmissionFactor;
        double burntResidueN2OEmissionFactor;
        double burntResidueNH3EmissionFactor;
        double burntResidueNOxEmissionFactor;
        double burntResidueCOEmissionFactor;
        double burntResidueBlackCEmissionFactor;
        double soilN2OEmissionFactor;
        double manureN2Factor;
        double averageAirTemperature;
        int airtemperatureOffset;
        double airtemperatureAmplitude;
        int grazingMidpoint;
        double averageYearsToSimulate;
        public double geturineNH3EmissionFactor() { return urineNH3EmissionFactor; }
        public double getmanureN20EmissionFactor() { return manureN20EmissionFactor; }
        public double getfertiliserN20EmissionFactor() { return fertiliserN20EmissionFactor; }
        //        public double GetfertManNH3EmissionFactorHousingRefTemperature() { return fertManNH3EmissionFactorHousingRefTemperature; }
        public double getresidueN2OEmissionFactor() { return residueN2OEmissionFactor; }
        public double getsoilN2OEmissionFactor() { return soilN2OEmissionFactor; }
        public double GetburntResidueN2OEmissionFactor() { return burntResidueN2OEmissionFactor; }
        public double GetburntResidueNH3EmissionFactor() { return burntResidueNH3EmissionFactor; }
        public double GetburntResidueNOxEmissionFactor() { return burntResidueNOxEmissionFactor; }
        public double GetburntResidueCOEmissionFactor() { return burntResidueCOEmissionFactor; }
        public double GetburntResidueBlackCEmissionFactor() { return burntResidueBlackCEmissionFactor; }
        public double GetaverageAirTemperature() { return averageAirTemperature; }
        public int GetairtemperatureOffset() { return airtemperatureOffset; }
        public double GetairtemperatureAmplitude() { return airtemperatureAmplitude; }
        public int GetgrazingMidpoint() { return grazingMidpoint; }
        public void SetaverageYearsToSimulate(double aVal) { averageYearsToSimulate = aVal; }
        public double GetaverageYearsToSimulate() { return averageYearsToSimulate; }
        public void SetNdeposition(double aVal) { Ndeposition = aVal; }
        public double GetNdeposition() { return Ndeposition; }

        public void OpenDebugFile(string afilename)
        {
            SetdebugFileName(afilename);
            debugfile = new System.IO.StreamWriter(debugFileName);
        }
        public void CloseDebugFile()
        {
            if (debugfile!=null)
            {
                debugfile.Close();
            }
        }

        public void WriteToDebug(string aString)
        {
            debugfile.Write(aString);
        }

        public void WriteLineToDebug(string aString)
        {
            debugfile.WriteLine(aString);
        }

        ///this is only used if the soil C model needs to spin up. The drought index is read from parameters.xml
        public double [] GetdroughtIndex()
        {
            return droughtIndex;
        }
        public double GetMeanTemperature(CropClass cropData)
        {
            double MeanTemperature = 0;
            int startDay = cropData.GetStartDay();
            int startMonth = cropData.GetStartMonth();
            int startYear = cropData.GetStartYear();
            int endDay = cropData.GetEndDay();
            int endMonth = cropData.GetEndMonth();
            int endYear = cropData.GetEndYear();
            MeanTemperature = GetMeanTemperature(startDay, startMonth, startYear, endDay, endMonth, endYear);
            return MeanTemperature;
        }

        public double GetMeanTemperature(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
            timeClass time = new timeClass();
            double MeanTemperature = 0;
            int endCount = endMonth;
            if (endYear > startYear)
                endCount += 12;
            for (int i = (startMonth) + 1; i <= endCount - 1; i++)
            {
                int monthCount = i;
                if (i > 12)
                    monthCount -= 12;
                MeanTemperature += airTemp[monthCount - 1] * time.GetDaysInMonth(monthCount);
            }
            double startMonthAmount = ((time.GetDaysInMonth(startMonth) - startDay) / time.GetDaysInMonth(startMonth)) * airTemp[startMonth - 1];
            MeanTemperature += startMonthAmount;
            double endMonthAmount = ((time.GetDaysInMonth(endMonth) - endDay) / time.GetDaysInMonth(endMonth)) * airTemp[endMonth - 1];
            MeanTemperature += endMonthAmount;
            int numbersOfDays = 0;
            for (int i = (startMonth) + 1; i <= endCount - 1; i++)
            {
                int monthCount = i;
                if (i > 12)
                    monthCount -= 12;
                numbersOfDays += time.GetDaysInMonth(monthCount);
            }
            numbersOfDays += (time.GetDaysInMonth(endMonth) - endDay);
            numbersOfDays += (time.GetDaysInMonth(startMonth) - startDay);

            return MeanTemperature / numbersOfDays;
        }

        public double GetTemperatureSum(double temperature, double baseTemp)
        {
            double Tsum = 0;
            if (temperature > baseTemp)
                Tsum = temperature - baseTemp;
            return Tsum;
        }

        public double GetPeriodTemperatureSum(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear, double baseTemp)
        {
            timeClass time = new timeClass();
            double Tsum = 0;
            int endCount = endMonth;
            if (endYear > startYear)
                endCount += 12;
            for (int i = (startMonth) + 1; i <= endCount - 1; i++)
            {
                int monthCount = i;
                if (i > 12)
                    monthCount -= 12;
                Tsum += GetTemperatureSum(airTemp[monthCount - 1], baseTemp) * time.GetDaysInMonth(monthCount);
            }
            Tsum += GetTemperatureSum(airTemp[startMonth - 1], baseTemp) * (time.GetDaysInMonth(startMonth) - startDay + 1);
            Tsum += GetTemperatureSum(airTemp[endMonth - 1], baseTemp) * endDay;
            return Tsum;
        }

        //!Generate temperature model parameters from monthly temperature data
        public void CalcTemperatureParameters()
        {
            double minTemp = 300;
            double maxTemp = -300;
            int maxMonth = 0;
            averageAirTemperature = 0;
            for (int i = 1; i <= 12; i++)
            {
                averageAirTemperature += airTemp[i - 1];
                if (airTemp[i - 1] > maxTemp)
                {
                    maxTemp = airTemp[i - 1];
                    maxMonth = i;
                }
                if (airTemp[i - 1] < minTemp)
                    minTemp = airTemp[i - 1];
            }
            averageAirTemperature /= 12.0;
            airtemperatureAmplitude = (maxTemp - minTemp) / 2;
            if ((maxMonth > 3) && (maxMonth <= 9))
                airtemperatureOffset = 245;//Northern hemisphere
            else
                airtemperatureOffset = 65;//Southern hemisphere
        }

        public void readZoneSpecificData(int zone_nr, int currentFarmType)
        {
            FileInformation AEZParamFile = new FileInformation(GlobalVars.Instance.getParamFilePath());
            //get zone-specific constants
            string basePath = "AgroecologicalZone(" + zone_nr.ToString() + ")";
            AEZParamFile.setPath(basePath);
            airTemp = new double[12];
            droughtIndex = new double[12];
            Precipitation = new double[12];
            Precipitation = new double[12];
            PotentialEvapoTrans = new double[12];
            rainDays = new int[12];
            double cumulativePrecip = 0;
            //check if the agroecological zone data are present. Note that the numbering of agroecological zones starts at 1 not zero
            string ZoneName=AEZParamFile.getItemString("Name", basePath);

            if (ZoneName.CompareTo("nothing")==0)
            {
                string messageString = "Error - agroecological zone " + zone_nr.ToString() + " not present in parameters.xml";
                GlobalVars.instance.Error(messageString);
            }


            //getItemInt("Value", basePath + ".Identity(" + zone_nr + ")");
            numberRainyDaysPerYear = AEZParamFile.getItemInt("Value", basePath + ".NumberRaindays(-1)");
            AEZParamFile.setPath(basePath);
            bool monthlyData = AEZParamFile.getItemBool("MonthlyAirTemp");
            AEZParamFile.PathNames.Add("Month");
            int max = 0; int min = 99;
            AEZParamFile.getSectionNumber(ref min, ref max);
            if ((max - min + 1) != 12)
            {
                string messageString = "Error - number of months in parameters.xml is not 12";
                GlobalVars.instance.Error(messageString);
            }
            AEZParamFile.Identity.Add(-1);
            AEZParamFile.Identity.Add(-1);
            AEZParamFile.PathNames.Add("DroughtIndex");
            for (int i = min; i <= max; i++)
            {
                AEZParamFile.Identity[1] = i;
                droughtIndex[i - 1] = AEZParamFile.getItemDouble("Value", false);
            }
            if (monthlyData == true)
            {
                AEZParamFile.PathNames[2] = "AirTemperature";
                for (int i = min; i <= max; i++)
                {
                    AEZParamFile.Identity[1] = i;
                    airTemp[i - 1] = AEZParamFile.getItemDouble("Value");
                    averageAirTemperature += airTemp[i - 1];
                }
                averageAirTemperature /= 12.0;
                /*AEZParamFile.PathNames[2] = "DroughtIndex"; 
                for (int i = min; i <= max; i++)
                {
                    AEZParamFile.Identity[1] = i;
                    droughtIndex[i - 1] = AEZParamFile.getItemDouble("Value");
                }*/
                AEZParamFile.PathNames[2] = "Precipitation";
                for (int i = min; i <= max; i++)
                {
                    AEZParamFile.Identity[1] = i;
                    Precipitation[i - 1] = AEZParamFile.getItemDouble("Value");
                    cumulativePrecip += Precipitation[i - 1];
                }
                AEZParamFile.PathNames[2] = "PotentialEvapoTrans";
                for (int i = min; i <= max; i++)
                {
                    AEZParamFile.Identity[1] = i;
                    PotentialEvapoTrans[i - 1] = AEZParamFile.getItemDouble("Value");
                }
                int checkdays = 0;
                for (int i = 0; i < 12; i++)
                {
                    rainDays[i] = (int)Math.Round(numberRainyDaysPerYear * (Precipitation[i] / cumulativePrecip));
                    checkdays += rainDays[i];
                }
                CalcTemperatureParameters();
            }
            else
            {
                AEZParamFile.setPath(basePath + ".AverageAirTemperature(-1)");
                averageAirTemperature = AEZParamFile.getItemDouble("Value");
                AEZParamFile.PathNames[1] = "AirTemperatureMaxDay";

                int temp = AEZParamFile.getItemInt("Value");
                airtemperatureOffset = temp + 94;
                AEZParamFile.PathNames[1] = "AirTemperaturAmplitude";

                airtemperatureAmplitude = AEZParamFile.getItemDouble("Value");
            }
            AEZParamFile.setPath(basePath + ".GrazingMidpoint(-1)");
            grazingMidpoint = AEZParamFile.getItemInt("Value");
            AEZParamFile.setPath(basePath + ".UrineNH3EF(-1)");
            urineNH3EmissionFactor = AEZParamFile.getItemDouble("Value");
            AEZParamFile.setPath(basePath + ".Manure(-1).EFN2O(-1)");
            manureN20EmissionFactor = AEZParamFile.getItemDouble("Value");
            AEZParamFile.setPath(basePath + ".Manure(-1).N2Factor(-1)");
            manureN2Factor = AEZParamFile.getItemDouble("Value");
            string tempPath = basePath + ".CropResidues(-1).EFN2O(-1)";
            residueN2OEmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            tempPath = basePath + ".CropResidues(-1).EFN2O_burning(-1)";
            burntResidueN2OEmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            tempPath = basePath + ".CropResidues(-1).EFNOx_burning(-1)";
            burntResidueNOxEmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            tempPath = basePath + ".CropResidues(-1).EFNH3_burning(-1)";
            burntResidueNH3EmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            tempPath = basePath + ".CropResidues(-1).EFBlackC_burning(-1)";
            burntResidueBlackCEmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            tempPath = basePath + ".CropResidues(-1).EFCO_burning(-1)";
            burntResidueCOEmissionFactor = AEZParamFile.getItemDouble("Value", tempPath);
            AEZParamFile.setPath(basePath + ".MineralisedSoilN(-1).EFN2O(-1)");
            soilN2OEmissionFactor = AEZParamFile.getItemDouble("Value");
            AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").ManureApplicationTechnique");
            int maxApp = 0, minApp = 99;
            AEZParamFile.getSectionNumber(ref minApp, ref maxApp);
            themanureAppData = new List<manureAppData>();
            AEZParamFile.Identity.Add(-1);
            for (int j = minApp; j <= maxApp; j++)
            {
                AEZParamFile.Identity[1] = j;
                manureAppData newappData = new manureAppData();
                string RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").ManureApplicationTechnique" + '(' + j.ToString() + ").Name";
                newappData.name = AEZParamFile.getItemString("Name", RecipientPath);
                RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").ManureApplicationTechnique" + '(' + j.ToString() + ").NH3ReductionFactor(-1)";
                newappData.NH3EmissionReductionFactor = AEZParamFile.getItemDouble("Value", RecipientPath);
                themanureAppData.Add(newappData);
            }
            AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").SoilType");
            int maxSoil = 0, minSoil = 99;
            AEZParamFile.getSectionNumber(ref minSoil, ref maxSoil);
            thesoilData = new List<soilData>();
            AEZParamFile.Identity.Add(-1);
            for (int j = minSoil; j <= maxSoil; j++)
            {
                AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").SoilType");
                if (AEZParamFile.doesIDExist(j))
                {

                    soilData newsoilData = new soilData();
                    string RecipientStub = "AgroecologicalZone(" + zone_nr.ToString() + ").SoilType" + '(' + j.ToString() + ").";
                    string RecipientPath = RecipientStub;
                    newsoilData.name = AEZParamFile.getItemString("Name", RecipientPath);
                    RecipientPath = RecipientStub + "N2Factor(-1)";
                    newsoilData.N2Factor = AEZParamFile.getItemDouble("Value", RecipientPath);
                    RecipientPath = RecipientStub + "SandFraction(-1)";
                    newsoilData.SandFraction = AEZParamFile.getItemDouble("Value", RecipientPath);
                    RecipientPath = RecipientStub + "ClayFraction(-1)";
                    newsoilData.ClayFraction = AEZParamFile.getItemDouble("Value", RecipientPath);


                    RecipientPath = RecipientStub + "ThermalDiffusivity(-1)";
                    newsoilData.thermalDiff = AEZParamFile.getItemDouble("Value", RecipientPath);
                    newsoilData.dampingDepth = newsoilData.CalcDampingDepth(newsoilData.thermalDiff, GlobalVars.Instance.Getrho());
                    RecipientStub = "AgroecologicalZone(" + zone_nr.ToString() + ").SoilType(" + j.ToString() + ").C-Tool";
                    AEZParamFile.setPath(RecipientStub);
                    int maxHistory = 0, minHistory = 99;
                    AEZParamFile.getSectionNumber(ref minHistory, ref maxHistory);
                    newsoilData.theC_ToolData = new List<C_ToolData>();
                    AEZParamFile.Identity.Add(-1);
                    for (int k = minHistory; k <= maxHistory; k++)
                    {
                        AEZParamFile.Identity[1] = k;
                        C_ToolData newC_ToolData = new C_ToolData();
                        RecipientStub = "AgroecologicalZone(" + zone_nr.ToString() + ").SoilType(" + j.ToString() + ").C-Tool" + '(' + k.ToString() + ").";
                        RecipientPath = RecipientStub + "InitialC(-1)";
                        newC_ToolData.initialC = AEZParamFile.getItemDouble("Value", RecipientPath);
                        RecipientPath = RecipientStub + "InitialFOMinput(-1)";
                        newC_ToolData.InitialFOM = AEZParamFile.getItemDouble("Value", RecipientPath);
                        RecipientPath = RecipientStub + "InitialFOMCtoN(-1)";
                        newC_ToolData.InitialFOMCtoN = AEZParamFile.getItemDouble("Value", RecipientPath);
                        RecipientPath = RecipientStub + "InitialCtoN(-1)";
                        newC_ToolData.InitialCtoN = AEZParamFile.getItemDouble("Value", RecipientPath);
                        RecipientPath = RecipientStub + "pHUMupperLayer(-1)";
                        newC_ToolData.pHUMupperLayer = AEZParamFile.getItemDouble("Value", RecipientPath);
                        RecipientPath = RecipientStub + "pHUMlowerLayer(-1)";
                        newC_ToolData.pHUMlowerLayer = AEZParamFile.getItemDouble("Value", RecipientPath);
                        newsoilData.theC_ToolData.Add(newC_ToolData);
                    }
                    RecipientStub = "AgroecologicalZone(" + zone_nr.ToString() + ").SoilType(" + j.ToString() + ").SoilWater(-1)";

                    AEZParamFile.setPath(RecipientStub);
                    newsoilData.thesoilWaterData = new soilWaterData();
                    AEZParamFile.Identity.Add(-1);
                    AEZParamFile.setPath(RecipientStub + ".drainageConst(-1)");
                    newsoilData.thesoilWaterData = new soilWaterData();
                    newsoilData.thesoilWaterData.thesoilLayerData = new List<soilLayerData>();

                    newsoilData.thesoilWaterData.drainageConstant = AEZParamFile.getItemDouble("Value");

                    AEZParamFile.setPath(RecipientStub + ".layerClass");
                    min = 99; max = 0;
                    AEZParamFile.getSectionNumber(ref min, ref max);
                    for (int index = min; index <= max; index++)
                    {
                        soilLayerData anewsoilLayer = new soilLayerData();
                        string temp = RecipientStub + ".layerClass(" + index.ToString() + ").z_lower(-1)";
                        anewsoilLayer.z_lower = AEZParamFile.getItemDouble("Value", temp);
                        newsoilData.thesoilWaterData.thesoilLayerData.Add(anewsoilLayer);
                    }
                    newsoilData.maxSoilDepth = newsoilData.thesoilWaterData.thesoilLayerData[newsoilData.thesoilWaterData.thesoilLayerData.Count - 1].z_lower;
                    thesoilData.Add(newsoilData);
                }
            }
            AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").Fertiliser(-1).EFN2O(-1)");
            fertiliserN20EmissionFactor = AEZParamFile.getItemDouble("Value");
            AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").Fertiliser(-1).FertiliserType");
            int maxFert = 0, minFert = 99;
            AEZParamFile.getSectionNumber(ref minFert, ref maxFert);
            theFertManData = new List<fertiliserData>();

            for (int j = minFert; j <= maxFert; j++)
            {
                AEZParamFile.setPath("AgroecologicalZone(" + zone_nr.ToString() + ").Fertiliser(-1).FertiliserType");
                if (AEZParamFile.doesIDExist(j))
                {
                    AEZParamFile.Identity.Add(j);
                    fertiliserData newfertData = new fertiliserData();
                    string RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Fertiliser(-1).FertiliserType" + '(' + j.ToString() + ")";
                    newfertData.name = AEZParamFile.getItemString("Name", RecipientPath);
                    RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Fertiliser(-1).FertiliserType" + '(' + j.ToString() + ").EFNH3(-1)";
                    newfertData.fertManNH3EmissionFactor = AEZParamFile.getItemDouble("Value", RecipientPath);
                    newfertData.fertManNH3EmissionFactorHousingRefTemperature = 0;
                    theFertManData.Add(newfertData);

                }
            }
            string tmpPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType";
            AEZParamFile.setPath(tmpPath);
            int maxMan = 0, minMan = 99;
            AEZParamFile.getSectionNumber(ref minMan, ref maxMan);
            AEZParamFile.Identity.Add(-1);
            for (int j = minMan; j <= maxMan; j++)
            {
                tmpPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType";
                AEZParamFile.setPath(tmpPath);
                if (AEZParamFile.doesIDExist(j))
                {
                    tmpPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType(-1)";
                    AEZParamFile.setPath(tmpPath);
                    AEZParamFile.Identity[2] = j;
                    fertiliserData newfertData = new fertiliserData();
                    newfertData.manureType = AEZParamFile.getItemInt("StorageType");
                    newfertData.speciesGroup = AEZParamFile.getItemInt("SpeciesGroup");
                    newfertData.name = AEZParamFile.getItemString("Name");
                    string RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType" + '(' + j.ToString() + ").EFNH3FieldRef(-1)";
                    newfertData.fertManNH3EmissionFactor = AEZParamFile.getItemDouble("Value", RecipientPath);
                    RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType" + '(' + j.ToString() + ").EFNH3FieldRefTemperature(-1)";
                    newfertData.fertManNH3EmissionFactorHousingRefTemperature = AEZParamFile.getItemDouble("Value", RecipientPath);
                    RecipientPath = "AgroecologicalZone(" + zone_nr.ToString() + ").Manure(-1).ManureType" + '(' + j.ToString() + ").EFNH3FieldTier2(-1)";
                    newfertData.EFNH3FieldTier2 = AEZParamFile.getItemDouble("Value", RecipientPath);
                    theFertManData.Add(newfertData);
                }
            }
        }
    }
    public zoneSpecificData theZoneData;
    //!  Read global constants. 
    /*!
      more details.
    */
    public void readGlobalConstants()
    {
        FileInformation constants = new FileInformation(GlobalVars.Instance.getConstantFilePath());
        constants.setPath("constants(0)");
        constants.Identity.Add(-1);
        constants.PathNames.Add("humification_const");
        humification_const = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "alpha";
        alpha = constants.getItemDouble("Value");

        constants.PathNames[constants.PathNames.Count - 1] = "rgas";
        rgas = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "CNhum";
        CNhum = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "tor";
        tor = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "Eapp";
        Eapp = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "CO2EqCH4";
        CO2EqCH4 = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "CO2EqN2O";
        CO2EqN2O = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "CO2EqsoilC";
        CO2EqsoilC = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "IndirectNH3N2OFactor";
        IndirectNH3N2OFactor = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "IndirectNO3N2OFactor";
        IndirectNO3N2OFactor = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "defaultBeddingCconc";
        defaultBeddingCconc = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "defaultBeddingNconc";
        defaultBeddingNconc = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "ErrorToleranceYield";
        maxToleratedError = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "ErrorToleranceGrazing";
        maxToleratedErrorGrazing = constants.getItemDouble("Value");

        constants.PathNames[constants.PathNames.Count - 1] = "maximumIterations";
        maximumIterations = constants.getItemInt("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "EFNO3_IPCC";
        EFNO3_IPCC = constants.getItemDouble("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "minimumTimePeriod";
        minimumTimePeriod = constants.getItemInt("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "adaptationTimePeriod";
        adaptationTimePeriod = constants.getItemInt("Value");
        constants.PathNames[constants.PathNames.Count - 1] = "lockSoilTypes";
        lockSoilTypes = constants.getItemBool("Value");
        /*constants.PathNames[constants.PathNames.Count - 1] = "CurrentInventorySystem";
        currentInventorySystem = constants.getItemInt("Value");*/

        List<int> theInventorySystems = new List<int>();
        constants.setPath("constants(0).InventorySystem");
        int maxInvSysts = 0, minInvSysts = 99;
        constants.getSectionNumber(ref minInvSysts, ref maxInvSysts);
        constants.Identity.Add(-1);
        for (int i = minInvSysts; i <= maxInvSysts; i++)
        {
            constants.Identity[constants.Identity.Count - 1] = i;
            theInventorySystems.Add(constants.getItemInt("Value"));
        }
    }

    private string[] constantFilePath;
    //!  Set constant FilePath. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setConstantFilePath(string[] path)
    {
        constantFilePath = path;
    }
    //!  Get constant FilePath. Returing one string value.
    /*!
      \return a string array.
    */
    public string[] getConstantFilePath()
    {
        return constantFilePath;
    }

    private string[] ParamFilePath;
    //!  Set param FilePath. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setParamFilePath(string[] path)
    {
        ParamFilePath = path;
    }
    //!  Get param FilePath. Returing one string value.
    /*!
      \return a string array.
    */
    public string[] getParamFilePath()
    {
        return ParamFilePath;
    }
    private string[] farmFilePath;
    //!  Set farm FilePath. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setFarmtFilePath(string[] path)
    {
        farmFilePath = path;
    }
    //!  Get farm FilePath. Returing one string value.
    /*!
      \return a string array.
    */
    public string[] getFarmFilePath()
    {
        return farmFilePath;
    }
    private string[] feeditemPath;
    //!  Set FeedItem FilePath. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setFeeditemFilePath(string[] path)
    {
        feeditemPath = path;
    }
    //!  Get FeedItem FilePath. Returing one string value.
    /*!
      \return a string array.
    */
    public string[] getfeeditemFilePath()
    {
        return feeditemPath;
    }
    private string[] fertManPath;
    //!  Set fertMan FilePath. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setfertManFilePath(string[] path)
    {
        fertManPath = path;
    }
    //!  Get fertMan FilePath. Returing one string value.
    /*!
      \return a string array.
    */
    public string[] getfertManFilePath()
    {
        return fertManPath;
    }
    private string writeHandOverData = "simplesoilModel.xml";
    //!  Set Write HandOver Data. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setWriteHandOverData(string path)
    {
        writeHandOverData = path;
    }
    //!  Get Write HandOver Data. Returing one string value.
    /*!
      \return  a string value.
    */
    public string getWriteHandOverData() { return writeHandOverData; }
    private string ReadHandOverData = "simplesoilModel.xml";
    //!  Set Read HandOver Data. Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void setReadHandOverData(string path)
    {
        ReadHandOverData = path;
    }
    //!  Get Read HandOver Data. Returing one string value.
    /*!
      \return  a string value.
    */
    public string getReadHandOverData() { return ReadHandOverData; }

    private string errorFileName = "error.xml";
    //!  Set error FileName Taking one argument.
    /*!
      \param path a string value for path.
    */
    public void seterrorFileName(string path)
    {
        errorFileName = path;
    }
    public string GeterrorFileName() { return errorFileName; }
    public const int totalNumberLivestockCategories = 1;
    public const int totalNumberHousingCategories = 1;
    public const int totalNumberSpeciesGroups = 1;
    public const int totalNumberStorageTypes = 1;
    public const double avgNumberOfDays = 365;
    public const double NtoCrudeProtein = 6.25;
    public const double absoluteTemp = 273.15;
    public const int maxNumberFeedItems = 2000;
    public int getmaxNumberFeedItems() { return maxNumberFeedItems; }
    public double GetavgNumberOfDays() { return avgNumberOfDays; }
    public List<housing> listOfHousing = new List<housing>();

    public List<manureStore> listOfManurestores = new List<manureStore>();
    //! A class named product.
    /*!
      more details.
    */
    public class product
    {
        public double Modelled_yield;
        public double Expected_yield;
        public double Potential_yield;
        public double waterLimited_yield;
        public double Grazed_yield;
        public string Harvested;
        public feedItem composition;
        public string Units;
        public bool burn;
        public double ResidueGrazingAmount;
        //! An constructor.
        /*!
          without argument.
        */
        public product()
        {
            Modelled_yield = 0;
            Expected_yield = 0;
            waterLimited_yield = 0;
            Grazed_yield = 0;
            Potential_yield = 0;
            Harvested = "";
            Units = "";
            burn = false;
            ResidueGrazingAmount = 0;
            composition = new feedItem();
        }
        //! An constructor with one argument.
        /*!
          \param aProduct, a product instance.
        */
        public product(product aProduct)
        {
            Modelled_yield = aProduct.Modelled_yield;
            Expected_yield = aProduct.Expected_yield;
            waterLimited_yield = aProduct.waterLimited_yield;
            Grazed_yield = aProduct.Grazed_yield;
            Potential_yield = aProduct.Potential_yield;
            Harvested = aProduct.Harvested;
            Units = aProduct.Units;
            burn = aProduct.burn;
            ResidueGrazingAmount = aProduct.ResidueGrazingAmount;
            composition = new feedItem(aProduct.composition);
        }
        //! A normal member inside product class, Set ExpectedYield. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void SetExpectedYield(double aVal) { Expected_yield = aVal; }
        //! A normal member inside product class, Get ExpectedYield. Returing a double value.
        /*!
          \return a double value.
        */
        public double GetExpectedYield() { return Expected_yield; }
        //! A normal member inside product class, Set Modelled_yield. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void SetModelled_yield(double aVal)
        {
            Modelled_yield = aVal;
        }
        //! A normal member inside product class, Set waterLimited_yield. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void SetwaterLimited_yield(double aVal) { waterLimited_yield = aVal; }
        //! A normal member inside product class, Set Grazed_yield. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void SetGrazed_yield(double aVal) { Grazed_yield = aVal; }
        //! A normal member inside product class, Get Modelled_yield. Returing one double value.
        /*!
          \return a double value.
        */
        public double GetModelled_yield() { return Modelled_yield; }
        //! A normal member inside product class, Get waterLimited_yield. Returing one double value.
        /*!
          \return a double value.
        */
        public double GetwaterLimited_yield() { return waterLimited_yield; }
        //! A normal member inside product class, Get Potential_yield. Returing one double value.
        /*!
          \return a double value.
        */
        public double GetPotential_yield() { return Potential_yield; }
        //! A normal member inside product class, Get Grazed_yield. Returing one double value.
        /*!
          \return a double value.
        */
        public double GetGrazed_yield() { return Grazed_yield; }
        //! A normal member inside product class, Get isBedding. Returing one boolean value.
        /*!
          \return a boolean value.
        */

        public bool GetisBedding() { return composition.GetbeddingMaterial(); }
        //! A normal member inside product class, Add Expected Yield. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void AddExpectedYield(double aVal) { Expected_yield += aVal; }
        //! A normal member inside product class, Add ActualAmount. Taking one argument.
        /*!
          \param aVal a double argument.
        */
        public void AddActualAmount(double aVal) { composition.Setamount(composition.Getamount() + aVal); }
        //! A normal member inside product class, Write. Taking one argument.
        /*!
          \param theparens a string argument.
        */
        public void Write(string theparens)
        {
            GlobalVars.Instance.writeStartTab("product");
            parens = theparens + "_FeedCode" + composition.GetFeedCode().ToString();
            GlobalVars.Instance.writeInformationToFiles("Name", "Name", "-", composition.GetName(), parens);
            GlobalVars.Instance.writeInformationToFiles("Potential_yield", "Potential yield", "kgDM/ha", Potential_yield, parens);
            GlobalVars.Instance.writeInformationToFiles("waterLimited_yield", "Expected yield", "kgDM/ha", waterLimited_yield, parens);
            GlobalVars.Instance.writeInformationToFiles("Modelled_yield", "Modelled yield", "kgDM/ha", Modelled_yield, parens);
            GlobalVars.Instance.writeInformationToFiles("Expected_yield", "Expected yield", "kgDM/ha", Expected_yield, parens);
            GlobalVars.Instance.writeInformationToFiles("Grazed_yield", "Grazed yield", "kgDM/ha", Grazed_yield, parens);
            GlobalVars.Instance.writeInformationToFiles("Harvested", "Is harvested", "-", Harvested, parens);
            GlobalVars.Instance.writeInformationToFiles("usableForBedding", "Usable for bedding", "-", GetisBedding(), parens);

            if (composition != null)
                composition.Write(parens);

            GlobalVars.Instance.writeEndTab();

        }

        //! A normal member inside product class, Write FieldFile. Taking three arguments.
        /*!
          \param theparens, a string argument.
          \param i, an integer argument
          \param count, an integer argument
        */

        public void WriteFieldFile(string theparens, int i, int count)
        {

            parens = theparens + "_FeedCode" + composition.GetFeedCode().ToString();
            if (GlobalVars.Instance.headerField == false)
            {
                GlobalVars.Instance.writeFieldFile("Name", "Name", "kg/ha", composition.GetName(), parens, 0);
                GlobalVars.Instance.writeFieldFile("Potential_yield", "Potential yield", "kgDM/ha", Potential_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("waterLimited_yield", "Expected yield", "kgDM/ha", waterLimited_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Modelled_yield", "Modelled yield", "kgDM/ha", Modelled_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Expected_yield", "Expected yield", "kgDM/ha", Expected_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Grazed_yield", "Grazed yield", "kgDM/ha", Grazed_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Harvested", "Is harvested", "-", Harvested, parens, 0);

                GlobalVars.Instance.writeFieldFile("usableForBedding", "Usable for bedding", "-", GetisBedding(), parens, 0);

            }
            else if (GlobalVars.Instance.headerField == true)
            {
                GlobalVars.Instance.writeFieldFile("Name", "Name", "kg/ha", composition.GetName(), parens, 0);
                GlobalVars.Instance.writeFieldFile("Potential_yield", "Potential yield", "kgDM/ha", Potential_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("waterLimited_yield", "Expected yield", "kgDM/ha", waterLimited_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Modelled_yield", "Modelled yield", "kgDM/ha", Modelled_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Expected_yield", "Expected yield", "kgDM/ha", Expected_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Grazed_yield", "Grazed yield", "kgDM/ha", Grazed_yield, parens, 0);
                GlobalVars.Instance.writeFieldFile("Harvested", "Is harvested", "-", Harvested, parens, 0);

                GlobalVars.Instance.writeFieldFile("usableForBedding", "Usable for bedding", "-", GetisBedding(), parens, 0);
            }
        }
    }

    public feedItem thebeddingMaterial = new feedItem();
    //!  Get the bedding Material Returing one string value.
    /*!
      \return  a feedItem.
    */
    public feedItem GetthebeddingMaterial() { return thebeddingMaterial; }

    //need to calculate these values
    //!  Calculate bedding Material. Taking one argument.
    /*!
      \param rotationList a list argument that points to CropSequenceClass.
    */
    public void CalcbeddingMaterial(List<CropSequenceClass> rotationList)
    {
        thebeddingMaterial.Setfibre_conc(0.1); //guess
        double tmp1 = 0;
        double tmp2 = 0;
        double tmp3 = 0;
        if (rotationList != null)
        {
            for (int i = 0; i < rotationList.Count; i++)
            {

                CropSequenceClass arotation = rotationList[i];
                for (int j = 0; j < arotation.GettheCrops().Count; j++)
                {
                    CropClass acrop = arotation.GettheCrops()[j];
                    for (int k = 0; k < acrop.GettheProducts().Count; k++)
                    {
                        product aproduct = acrop.GettheProducts()[k];
                        if (aproduct.composition.GetbeddingMaterial())
                        {
                            tmp1 += acrop.getArea() * aproduct.GetPotential_yield() * (1 - aproduct.composition.GetC_conc()) * aproduct.composition.Getash_conc();
                            tmp3 += acrop.getArea() * aproduct.GetPotential_yield() * (1 - aproduct.composition.GetN_conc()) * aproduct.composition.Getash_conc();
                            tmp2 += acrop.getArea() * aproduct.GetPotential_yield();
                        }
                    }
                }
            }
        }
        if (tmp2 > 0.0 && tmp3 > 0.0)
        {
            thebeddingMaterial.SetC_conc(tmp1 / tmp2);
            thebeddingMaterial.SetN_conc(tmp3 / tmp2);
            thebeddingMaterial.Setname("mixed beddding");
        }
        if (thebeddingMaterial.GetC_conc() == 0) //no bedding found on farm or field model not called
        {
            thebeddingMaterial.SetC_conc(GlobalVars.Instance.getdefaultBeddingCconc());
            thebeddingMaterial.SetN_conc(GlobalVars.Instance.getdefaultBeddingNconc());
            thebeddingMaterial.Setname("default beddding");
        }
        thebeddingMaterial.setFeedCode(999);
    }
    //! A structure manurestoreRecord.
    /*!
      more details.
    */
    public struct manurestoreRecord
    {
        manureStore theStore;
        double propYearGrazing;
        public void SetpropYearGrazing(double aVal) { propYearGrazing = aVal; }
        public manure manureToStorage;
        public void SetmanureToStorage(manure amanureToStorage) { manureToStorage = amanureToStorage; }
        public double GetpropYearGrazing() { return propYearGrazing; }
        public manure GetmanureToStorage() { return manureToStorage; }
        public manureStore GettheStore() { return theStore; }
        public void SettheStore(manureStore aStore) { theStore = aStore; }
        public void Write()
        {
            GlobalVars.Instance.writeStartTab("manurestoreRecord");
            theStore.Write();
            manureToStorage.Write(theStore.Getname().ToString());
            GlobalVars.Instance.writeEndTab();
        }
    }

     //! A class named theManureExchangeClass.
    /*!
      \the theManureExchangeClass is used to keep track of the manure generated on the farm and the manure that must be imported.
    */
    public class theManureExchangeClass
    {
        private List<manure> manuresStored;
        private List<manure> manuresProduced;
        private List<manure> manuresImported;
        private List<manure> manuresUsed;
        public List<manure> GetmanuresImported() { return manuresImported; }
        public List<manure> GetmanuresExported() { return manuresStored; }
        //! A constructor for theManureExchangeClass.
        /*!
          more details.
        */
        public theManureExchangeClass()
        {
            manuresStored = new List<manure>();
            manuresProduced = new List<manure>();
            manuresImported = new List<manure>();
            manuresUsed = new List<manure>();
        }
        //!  Write. 
        /*!
          a method for class theManureExchangeClass.
        */
        public void Write()
        {
            for (int i = 0; i < manuresProduced.Count; i++)
                manuresUsed.Add(new manure(manuresProduced[i]));
            for (int i = 0; i < manuresImported.Count; i++)
            {
                bool gotit = false;
                for (int k = 0; k < manuresUsed.Count; k++)
                {
                    if ((manuresUsed[k].GetmanureType() == manuresImported[i].GetmanureType()) &&
                        (manuresUsed[k].GetspeciesGroup() == manuresImported[i].GetspeciesGroup()))
                    {
                        manuresUsed[k].AddManure(manuresImported[i]);
                        gotit = true;
                    }
                }
                if (gotit == false)
                    manuresUsed.Add(manuresImported[i]);
            }
            for (int i = 0; i < manuresStored.Count; i++)
            {
                for (int k = 0; k < manuresUsed.Count; k++)
                {
                    if ((manuresUsed[k].GetmanureType() == manuresStored[i].GetmanureType()) &&
                        (manuresUsed[k].GetspeciesGroup() == manuresStored[i].GetspeciesGroup()))
                    {
                        manure amanure = new manure(manuresStored[i]);
                        double amount = manuresStored[i].GetTotalN();
                        manuresUsed[k].TakeManure(ref amount, ref amanure);
                    }
                }
            }
            GlobalVars.Instance.writeStartTab("theManureExchangeClass");
            GlobalVars.Instance.writeStartTab("producedManure");

            for (int i = 0; i < manuresProduced.Count; i++)
            {
                manuresProduced[i].Write("Produced");
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("exportedManure");
            for (int i = 0; i < manuresStored.Count; i++)
            {
                if (manuresStored[i].GetTotalN() > 0)
                    manuresStored[i].Write("Exported");
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("importedManure");
            for (int i = 0; i < manuresImported.Count; i++)
            {
                manuresImported[i].Write("Imported");
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("usedManure");
            for (int i = 0; i < manuresUsed.Count; i++)
            {
                manuresUsed[i].Write("Used");
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeEndTab();
        }
        
        //!  Add to Manure Exchange. Taking one argument. Adds manure to the list of manures available
        /*!
          a method for class theManureExchangeClass.
        \param aManure, a manure instance argument
        */
        public void AddToManureExchange(manure aManure)
        {
            bool gotit = false;
            for (int i = 0; i < manuresStored.Count; i++)
            {
                if (manuresStored[i].isSame(aManure)) //add this manure to an existing manure
                {
                    gotit = true;
                    manuresStored[i].AddManure(aManure);
                    manure newManure = new manure(aManure);
                    manuresProduced[i].AddManure(newManure);
                    continue;
                }
            }
            if (gotit == false)
            {
                manuresStored.Add(aManure);
                manure newManure = new manure(aManure);
                manuresProduced.Add(newManure);
            }
        }
        //!  Take Manure. Taking four arguments and returing a manure class instance value.
        /*!
          \param amountN, a double argument.
          \param lengthOfSequence, a double argument.
          \param manureType, an integer argument.
          \param speciesGroup, an integer argument.
          \return a manure instance value. 
        */
        public manure TakeManure(double amountN, double lengthOfSequence, int manureType, int speciesGroup)
        {
            bool gotit = false;
            double amountNFound = amountN / lengthOfSequence;
            double amountNNeeded = amountN / lengthOfSequence;
            amountN /= lengthOfSequence;
            manure aManure = new manure();
            aManure.SetmanureType(manureType);
            aManure.SetspeciesGroup(speciesGroup);
            int i = 0;
            while ((i < manuresStored.Count) && (gotit == false))
            {
                if (manuresStored[i].isSame(aManure))
                {
                    gotit = true;
                    manuresStored[i].TakeManure(ref amountNFound, ref aManure);
                    amountNNeeded = amountN - amountNFound;
                }
                else
                    i++;
            }

            ///if cannot find this manure or there is none left
            if ((gotit == false) || (amountNNeeded > 0))
            {
                i = 0;
                gotit = false;
                while ((i < manuresImported.Count) && (gotit == false))
                {
                    if (manuresImported[i].isSame(aManure))  //there is already some of this manure that will be imported
                    {
                        double proportion = 0;
                        if (manuresImported[i].GetTotalN() != 0)
                            proportion = amountNNeeded / manuresImported[i].GetTotalN();
                        aManure.SetTAN(manuresImported[i].GetTAN() * proportion);
                        aManure.SetlabileOrganicN(manuresImported[i].GetlabileOrganicN() * proportion);
                        aManure.SethumicN(manuresImported[i].GethumicN() * proportion);
                        aManure.SethumicC(manuresImported[i].GethumicC() * proportion);
                        aManure.SetnonDegC(manuresImported[i].GetnonDegC() * proportion);
                        aManure.SetdegC(manuresImported[i].GetdegC() * proportion);
                        manuresImported[i].AddManure(aManure);
                        //and for all the other components of manure...
                        gotit = true;
                    }
                    else
                        i++;
                }
                if (gotit != true)
                {
                    ///find a standard manure of this type

                    FileInformation file = new FileInformation(GlobalVars.Instance.getfertManFilePath());
                    file.setPath("AgroecologicalZone(" + GlobalVars.Instance.GetZone().ToString() + ").manure");
                    int min = 99; int max = 0;
                    file.getSectionNumber(ref min, ref max);
                    file.Identity.Add(-1);
                    int itemNr = 0;
                    i = min;
                    while ((i <= max) && (gotit == false))
                    {
                        file.Identity[1] = i;
                        int ManureType = file.getItemInt("ManureType");
                        int SpeciesGroupFile = file.getItemInt("SpeciesGroup");
                        if (ManureType == manureType)
                        {
                            if ((SpeciesGroupFile == speciesGroup) || (speciesGroup == 0))
                            {
                                itemNr = i;
                                gotit = true;
                            }
                        }
                        i++;
                    }
                    if (gotit == false)
                    {

                        string messageString = "problem finding manure to import: species group = " + speciesGroup.ToString()
                            + " and storage type = " + manureType.ToString();
                        GlobalVars.Instance.Error(messageString);
                    }

                    manure anotherManure = new manure("manure", itemNr, amountNNeeded, parens + "_" + i.ToString());
                    manuresImported.Add(anotherManure);
                    aManure.AddManure(anotherManure);
                    aManure.Setname(anotherManure.Getname());
                }
            }
            aManure.IncreaseManure(lengthOfSequence);
            return aManure;
        }

    }///end of manure exchange
    private XmlWriter writer;
    //   public XElement writerCtool;
    //public XmlWriter writerCtool;
    private System.IO.StreamWriter tabFile;
    private System.IO.StreamWriter DebugFile;
    private System.IO.StreamWriter FieldFile;
    private System.IO.StreamWriter livestockFile;
    private System.IO.StreamWriter CtoolFile;
    private System.IO.StreamWriter CropFile;
    private string FieldfileName;
    private string CtoolfileName;
    private string DebugfileName;
    private string livestockfileName;
    private string cropfileName;
    string FieldHeaderName;
    string FieldHeaderUnits;
    string CtoolHeaderName;
    string CtoolHeaderUnits;
    string livestockHeaderName;
    string livestockHeaderUnits;
    string cropHeaderName;
    string cropHeaderUnits;
    //!  open Output XML. Taking one argument and return a XMLWriter.
    /*!
      \param outputName, a string argument.
      \return a XMLWriter instance.
    */
    public XmlWriter OpenOutputXML(string outputName)
    {
        if (Writeoutputxlm)
        {
            writer = XmlWriter.Create(outputName);
            writer.WriteStartDocument();
        }
        return writer;
    }
    //!  Close Output XML. 
    /*!
      a method for closing the output XML file.
    */
    public void CloseOutputXML()
    {
        try //closing the output XML file
        {
            if (Writeoutputxlm)
            {
                writer.WriteEndDocument();
                writer.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); //write message to screen, if this fails
            log(e.Message, 6);
            log("Cannot close output xml file", 6);
        }


    }
    //!  Write StartTab. Taking one argument. 
    /*!
      \param name, a string argument.
    */
    public void writeStartTab(string name)
    {
        if (Writeoutputxls)
            tabFile.Write(name + "\n");
        if (Writeoutputxlm)
            writer.WriteStartElement(name);

    }
    //!  Write EndTab. Taking one argument. 
    /*!
      a method for writing the end tab of file.
    */
    public void writeEndTab()
    {
        if (Writeoutputxlm)
            writer.WriteEndElement();
    }
    //!  Write Information to Files (for boolean values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a boolean argument.
      \param parens, a string argument.
    */
    public void writeInformationToFiles(string name, string Description, string Units, bool value, string parens)
    {
        writeInformationToFiles(name, Description, Units, Convert.ToString(value), parens);
    }
    //!  Write Information to Files (for double values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a double argument.
      \param parens, a string argument.
    */
    public void writeInformationToFiles(string name, string Description, string Units, double value, string parens)
    {
        writeInformationToFiles(name, Description, Units, Convert.ToString(Math.Round(value, 4)), parens);
    }
    //!  Write Information to Files (for integer values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, an integer argument.
      \param parens, a string argument.
    */
    public void writeInformationToFiles(string name, string Description, string Units, int value, string parens)
    {
        writeInformationToFiles(name, Description, Units, Convert.ToString(value), parens);
    }
    //!  Write Information to Files (for string values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param parens, a string argument.
    */
    public void writeInformationToFiles(string name, string Description, string Units, string value, string parens)
    {
        if (Writeoutputxlm)
        {
            writer.WriteStartElement(name);
            writer.WriteStartElement("Description");
            writer.WriteValue(Description);
            writer.WriteEndElement();
            writer.WriteStartElement("Units");
            writer.WriteValue(Units);
            writer.WriteEndElement();
            writer.WriteStartElement("Name");
            writer.WriteValue(name);
            writer.WriteEndElement();
            writer.WriteStartElement("Value");
            writer.WriteValue(value);
            writer.WriteEndElement();
            writer.WriteStartElement("StringUI");
            writer.WriteValue(name + parens);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        if (Writeoutputxls)
        {
            tabFile.Write("Description" + "\t");
            tabFile.Write(Description + "\t");
            tabFile.Write("Units" + "\t");
            tabFile.Write(Units + "\t");
            tabFile.Write("Name" + "\t");
            tabFile.Write(name + parens + "\t");
            tabFile.Write("Value" + "\t");
            tabFile.Write(value + "\n");
        }

    }
    //!  Write Field Files (for boolean values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a boolean argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeFieldFile(string name, string Description, string Units, bool value, string parens, int newlineField)
    {
        writeFieldFile(name, Description, Units, Convert.ToString(value), parens, newlineField);
    }
    //!  Write Field Files (for double values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a double argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeFieldFile(string name, string Description, string Units, double value, string parens, int newlineField)
    {
        writeFieldFile(name, Description, Units, Convert.ToString(Math.Round(value, 4)), parens, newlineField);
    }
    //!  Write Field Files (for integer values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, an integer argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeFieldFile(string name, string Description, string Units, int value, string parens, int newlineField)
    {
        writeFieldFile(name, Description, Units, Convert.ToString(value), parens, newlineField);
    }
    //!  Write Field Files (for string values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeFieldFile(string name, string Description, string Units, string value, string parens, int newlineField)
    {
        if (WriteField)
        {
            if (headerField == false)
            {
                if (newlineField == 0)
                {

                    FieldHeaderName += name + "\t";
                    FieldHeaderUnits += Units + "\t"; ;
                }
                if (newlineField == 1)
                {
                    FieldHeaderName += name;
                    FieldHeaderUnits += Units;
                    if (FieldFile != null)
                    {
                        FieldFile.Write(FieldHeaderName + "\n");
                        FieldFile.Write(FieldHeaderUnits + "\n");
                    }
                }
            }
            else
            {
                if (newlineField == 0)
                {
                    if (FieldFile != null)
                        FieldFile.Write(value + "\t");
                }
                if (newlineField == 1)
                {
                    if (FieldFile != null)
                        FieldFile.Write(value + "\n");
                }
            }
        }
    }
    //!  Write Livestock Files (for boolean values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a boolean argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeLivestockFile(string name, string Description, string Units, bool value, string parens, int newlineField)
    {
        writeLivestockFile(name, Description, Units, Convert.ToString(value), parens, newlineField);
    }
    //!  Write Livestock Files (for double values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a double argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeLivestockFile(string name, string Description, string Units, double value, string parens, int newlineField)
    {
        writeLivestockFile(name, Description, Units, Convert.ToString(Math.Round(value, 4)), parens, newlineField);
    }
    //!  Write Livestock Files (for integer values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, an integer argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeLivestockFile(string name, string Description, string Units, int value, string parens, int newlineField)
    {
        writeLivestockFile(name, Description, Units, Convert.ToString(value), parens, newlineField);
    }
    //!  Write Livestock Files (for string values). Taking six arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param parens, a string argument.
      \param newlineField, an integer argument. 
    */
    public void writeLivestockFile(string name, string Description, string Units, string value, string parens, int newlineField)
    {
        if (Writelivestock)
        {
            if (headerLivestock == false)
            {
                if (newlineField == 0)
                {

                    livestockHeaderName += name + "\t";
                    livestockHeaderUnits += Units + "\t"; ;
                }
                if (newlineField == 1)
                {
                    livestockFile.Write(livestockHeaderName + name + "\n");
                    livestockFile.Write(livestockHeaderUnits + Units + "\n");
                }
            }
            else
            {
                if (newlineField == 0)
                {

                    livestockFile.Write(value + "\t");
                }
                if (newlineField == 1)
                {
                    livestockFile.Write(value + "\n");
                }
            }
        }
    }

    //!  Write CTool Files (for boolean values). Taking eight arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a boolean argument.
      \param parens, a string argument.
      \param printValue, a boolean argument.
      \param printUnits, a boolean argument. 
      \param newline, an integer argument. 
    */
    public XElement writeCtoolFile(string name, string Description, string Units, bool value, string parens, bool printValues, bool printUnits, int newline)
    {
        return writeCtoolFile(name, Description, Units, Convert.ToString(value), parens, printValues, printUnits, newline);
    }
    //!  Write CTool Files (for double values). Taking eight arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a double argument.
      \param parens, a string argument.
      \param printValue, a boolean argument.
      \param printUnits, a boolean argument. 
      \param newline, an integer argument. 
    */
    public XElement writeCtoolFile(string name, string Description, string Units, double value, string parens, bool printValues, bool printUnits, int newline)
    {
        return writeCtoolFile(name, Description, Units, Convert.ToString(Math.Round(value, 4)), parens, printValues, printUnits, newline);
    }
    //!  Write CTool Files (for integer values). Taking eight arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, an integer argument.
      \param parens, a string argument.
      \param printValue, a boolean argument.
      \param printUnits, a boolean argument. 
      \param newline, an integer argument. 
    */
    public XElement writeCtoolFile(string name, string Description, string Units, int value, string parens, bool printValues, bool printUnits, int newline)
    {
        return writeCtoolFile(name, Description, Units, Convert.ToString(value), parens, printValues, printUnits, newline);
    }

    //if newlineField = 0, the data is written with a trailing tab character, if newlineField = 1, data is written with a trailing carriage retturn (new line) character
    //!  Write CTool Files (for string values). Taking eight arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param parens, a string argument.
      \param printValue, a boolean argument.
      \param printUnits, a boolean argument. 
      \param newline, an integer argument. 
    */
    public XElement writeCtoolFile(string name, string Description, string Units, string value, string parens, bool printValues, bool printUnits, int newline)
    {

        if (Ctoolheader == false)
        {
            if (!printUnits)
            {
                if (newline == 1)
                    CtoolFile.Write(name + "\n");
                else
                    CtoolFile.Write(name + "\t");
            }
            else
            {
                if (newline == 1)
                    CtoolFile.Write(Units + "\n");
                else
                    CtoolFile.Write(Units + "\t");
            }
            return null;
        }
        if (printValues)
        {
            if (newline == 0)
            {
                if (Writectoolxls)
                    CtoolFile.Write(value + "\t");
            }
            if (newline == 1)
            {
                if (Writectoolxls)
                    CtoolFile.Write(value + "\n");
            }
        }
        return writeInformationToCtoolFiles(name, Description, Units, value, parens);
    }
    //!  Write Information to CTool Files (for string values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Description, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param parens, a string argument. 
      \return a XElement. 
    */
    public XElement writeInformationToCtoolFiles(string name, string Description, string Units, string value, string parens)
    {
        XElement node = new XElement(name);

        XElement DescriptionNode = new XElement("Description");
        DescriptionNode.Value = Description;
        node.Add(DescriptionNode);

        DescriptionNode = new XElement("Units");
        DescriptionNode.Value = Units;
        node.Add(DescriptionNode);

        DescriptionNode = new XElement("name");
        DescriptionNode.Value = name;
        node.Add(DescriptionNode);
        DescriptionNode = new XElement("value");
        DescriptionNode.Value = value;
        node.Add(DescriptionNode);

        DescriptionNode = new XElement("StringUI");
        DescriptionNode.Value = name + parens;
        node.Add(DescriptionNode);
        return node;

    }
    //!  Open output tab files. Taking two arguments. 
    /*!
      \param outputName, a string argument.
      \param output, a string argument.
    */
    public void OpenOutputTabFile(string outputName, string output)
    {
        string tabfileName = outputName + ".xls";
        if (Writeoutputxls)
            tabFile = new System.IO.StreamWriter(tabfileName);
        FieldfileName = outputName + "Fieldfile.xls";
        if (File.Exists(FieldfileName))
            File.Delete(FieldfileName);
        livestockfileName = outputName + "livetockfile.xls";
        CtoolfileName = outputName + "CtoolFile.xls";
        DebugfileName = outputName + "Debug.xls";
        cropfileName = outputName + "Crop.xls";
    }
    static bool usedField = false;
    //!  Open Field File. 
    /*!
      more details.
    */
    public void OpenFieldFile()
    {
        if (WriteField)
        {
            if (usedField == false)
                FieldFile = new System.IO.StreamWriter(FieldfileName);
            else
                FieldFile = File.AppendText(FieldfileName);
            usedField = true;
        }
    }
    //!  Close Field File. 
    /*!
      more details.
    */
    public void CloseFieldFile()
    {
        try  //closing the crop output file
        {
            if (WriteField)
                FieldFile.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close output Field file", 6);
        }
    }

    static bool usedCrop = false;
    private bool CropHeader = false;
    private string headerCrop;
    private string dataCrop;
    //!  Open Crop File. 
    /*!
      more details.
    */
    public void OpenCropFile()
    {
        if (WriteCrop)
        {
            if (usedCrop == false)
                CropFile = new System.IO.StreamWriter(cropfileName);
            else
                CropFile = File.AppendText(cropfileName);
            usedField = true;
        }
    }
    //!  Close Crop File. 
    /*!
      more details.
    */
    public void CloseCropFile()
    {
        try
        {
            if (WriteCrop)
                CropFile.Close();
        }
        catch
        {
        }
    }
    //!  Write Crop Files (for integer values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Units, a string argument.
      \param value, an integer argument.
      \param printUnits, a boolean argument. 
      \param newline, a boolean argument. 
    */
    public void WriteCropFile(string name, string Units, int value, bool printUnits, bool newline)
    {
        WriteCropFile(name, Units, value.ToString(), printUnits, newline);
    }
    //!  Write Crop Files (for double values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Units, a string argument.
      \param value, a double argument.
      \param printUnits, a boolean argument. 
      \param newline, a boolean argument. 
    */
    public void WriteCropFile(string name, string Units, double value, bool printUnits, bool newline)
    {
        WriteCropFile(name, Units, value.ToString(), printUnits, newline);
    }
    //!  Write Crop Files (for string values). Taking five arguments. 
    /*!
      \param name, a string argument.
      \param Units, a string argument.
      \param value, a string argument.
      \param printUnits, a boolean argument. 
      \param newline, a boolean argument. 
    */
    public void WriteCropFile(string name, string Units, string value, bool printUnits, bool newline)
    {
        if (WriteCrop)
        {
            if (CropHeader)
            {
                string headerCrop = name + " (" + Units + ")";
                if (newline)
                    CropFile.Write(headerCrop + "\n");
                else
                    CropFile.Write(headerCrop + "\t");
                CropHeader = false;
            }
            else
            {
                if (printUnits)
                    CropFile.Write(Units + "\t");
                if (newline)
                    CropFile.Write(value + "\n");
                else
                    CropFile.Write(value + "\t");
            }
        }
    }

    //!  Open Debug Files. 
    public void OpenDebugFile()
    {
        if (WriteDebug)
            DebugFile = new System.IO.StreamWriter(DebugfileName);

    }
    //!  Close Debug Files. 
    public void CloseDebugFile()
    {

        try  //closing the debug file
        {
            if (WriteDebug)
            {
                DebugFile.Write(headerDebug);
                DebugFile.Write(dataDebug);
                DebugFile.Close();
                if (headerDebug == null)
                    File.Delete(DebugfileName);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close output debug file", 6);
        }
    }
    private bool DebugHeader = true;
    private string headerDebug;
    private string dataDebug;
    //!  Write Debug Files (for integer values). Taking three arguments. 
    /*!
      \param name, a string argument.
      \param value, an integer argument.
      \param seperater, a char argument. 
    */
    public void WriteDebugFile(string name, int value, char seperater)
    {
        WriteDebugFile(name, value.ToString(), seperater);
    }
    //!  Write Debug Files (for double values). Taking three arguments. 
    /*!
      \param name, a string argument.
      \param value, a double argument.
      \param seperater, a char argument. 
    */
    public void WriteDebugFile(string name, double value, char seperater)
    {
        WriteDebugFile(name, value.ToString(), seperater);
    }
    //!  Write Debug Files (for string values). Taking three arguments. 
    /*!
      \param name, a string argument.
      \param value, a string argument.
      \param seperater, a char argument. 
    */
    public void WriteDebugFile(string name, string value, char seperater)
    {
        if (DebugHeader == true)
        {
            headerDebug += name + seperater;
        }
        if (seperater == '\n')
            DebugHeader = false;
        dataDebug += value + seperater;
    }
    //!  Open CTool File. 
    
    public void OpenCtoolFile()
    {
        if (Writectoolxls)
            CtoolFile = new System.IO.StreamWriter(CtoolfileName);
        /*        if (usedCtoolFile == false)
                    CtoolFile = new System.IO.StreamWriter(CtoolfileName);
                else
                    CtoolFile = File.AppendText(CtoolfileName);
                usedCtoolFile = true;
              */

    }
    //!  Close CTool File. 
    public void CloseCtoolFile()
    {
        try  //closing the CTOOL output file
        {
            if (Writectoolxls)
                CtoolFile.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close output Ctool file", 6);
        }
    }
    //!  Open Livestock File. 
    public void OpenLivestockFile()
    {
        headerLivestock = false;
        if (Writelivestock)
            livestockFile = new System.IO.StreamWriter(livestockfileName);
    }
    //!  Close Livestock File. 
    public void CloseLivestockFile()
    {
        try  //closing the livestock output file
        {
            if (Writelivestock)
                livestockFile.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close output livestock file", 6);
        }
    }
    //!  Close Output Tab File. 
    public void CloseOutputTabFile()
    {
        try //try to close output Excel file
        {
            if (Writeoutputxls)
                tabFile.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close output tab file", 6);
        }
    }
    //!  Get Tab File Writer. Returning a tabFile value.
    /*!
     \return a tabFile. 
   */
    public System.IO.StreamWriter GetTabFileWriter() { return tabFile; }
    //! A normol member. erroMsg contains the description of the error, stackTrace contains the details of the calls to the stack, if stopOnException is true, the program will exit
    //! use stopOnException = false when running more than one farm or scenario and you wish to continue to the next farm or scenario, when an error occurs
    /*!
     \param erroMsg, a string argument.
     \param stackTrace, a string argument.
     \param stopOnException, a boolean argument. 
   */
    public void Error(string erroMsg, string stackTrace = "", bool stopOnException = true)
    {
        if (stopOnException == true)
        {
            if (logFileStream != null)
                log(erroMsg + " " + stackTrace, -1);
            CloseOutputXML();
            CloseFieldFile();
            CloseLivestockFile();
            CloseDebugFile();
            closeSummaryExcel();
            if (!erroMsg.Contains("farm Fail"))
            {
                if (returnErrorMessage)
                {
                    AnimalChange.model.errorMessageReturn = " " + erroMsg + " " + stackTrace;
                    Console.WriteLine(erroMsg + " " + stackTrace);
                    sw.Stop();
                    Console.WriteLine("RunTime (hrs:mins:secs) " + sw.Elapsed);
                    if (GlobalVars.Instance.getPauseBeforeExit())
                        Console.Read();
                }
                else
                {
                    Console.WriteLine(GlobalVars.Instance.GeterrorFileName());
                    System.IO.StreamWriter files = new System.IO.StreamWriter(GlobalVars.Instance.GeterrorFileName());
                    files.WriteLine(erroMsg + " " + stackTrace);
                    files.Close();
                    Console.WriteLine(erroMsg + " " + stackTrace);
                    sw.Stop();
                    Console.WriteLine("RunTime (hrs:mins:secs) " + sw.Elapsed);
                    if (GlobalVars.Instance.getPauseBeforeExit())
                        Console.Read();
                }
            }
        }
        else
        {
            try
            {
                CloseOutputXML();
                CloseOutputTabFile();
                CloseFieldFile();
                CloseLivestockFile();
                CloseCtoolFile();
                CloseDebugFile();
                closeSummaryExcel();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exit without trapping error ");
                Console.WriteLine(erroMsg + " " + stackTrace);
                Console.Read();
            }
        }
        if (logFileStream != null)
            logFileStream.Close();
        if (stopOnException == true)
            throw new System.ArgumentException("farm Fail", "farm Fail");
    }
    public theManureExchangeClass theManureExchange;
    //!  Initialise Excreta Exchange.
    public void initialiseExcretaExchange()
    {
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            grazedArray[i].ruminantDMgrazed = 0;  //kg
            grazedArray[i].fieldDMgrazed = 0;  //kg
            grazedArray[i].urineC = 0;  //kg
            grazedArray[i].urineN = 0;  //kg
            grazedArray[i].faecesC = 0;  //kg
            grazedArray[i].faecesN = 0;  //kg
            grazedArray[i].fieldCH4C = 0; //kg
        }
    }
    //!  Write Grazed Items.
    /*!
     Write the consumed and produced DM for all grazed products/feeditems. 
   */
    public void writeGrazedItems()/// writes the consumed and produced DM for all grazed products/feeditems
    {
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if ((grazedArray[i].fieldDMgrazed > 0.0) || (grazedArray[i].ruminantDMgrazed > 0.0))
                grazedArray[i].Write();
        }
    }
    //!  Initialise Feed and Product lists.
   
    public void initialiseFeedAndProductLists()
    {
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            feedItem aproduct = new feedItem();
            allFeedAndProductsUsed[i] = new product();
            allFeedAndProductsUsed[i].composition = aproduct;
            allFeedAndProductsUsed[i].composition.setFeedCode(i);
            allFeedAndProductsUsed[i].composition.Setamount(0);
            aproduct = new feedItem();
            allFeedAndProductsProduced[i] = new product();
            allFeedAndProductsProduced[i].composition = aproduct;
            allFeedAndProductsProduced[i].composition.setFeedCode(i);
            allFeedAndProductsProduced[i].composition.Setamount(0);
            aproduct = new feedItem();
            allFeedAndProductsPotential[i] = new product();
            allFeedAndProductsPotential[i].composition = aproduct;
            allFeedAndProductsPotential[i].composition.setFeedCode(i);
            allFeedAndProductsPotential[i].composition.Setamount(0);
            aproduct = new feedItem();
            allFeedAndProductFieldProduction[i] = new product();
            allFeedAndProductFieldProduction[i].composition = aproduct;
            allFeedAndProductFieldProduction[i].composition.setFeedCode(i);
            allFeedAndProductFieldProduction[i].composition.Setamount(0);
            aproduct = new feedItem();
            allFeedAndProductTradeBalance[i] = new product();
            allFeedAndProductTradeBalance[i].composition = aproduct;
            allFeedAndProductTradeBalance[i].composition.setFeedCode(i);
            allFeedAndProductTradeBalance[i].composition.Setamount(0);
            aproduct = new feedItem();
            allUnutilisedGrazableFeed[i] = new product();
            allUnutilisedGrazableFeed[i].composition = aproduct;
            allUnutilisedGrazableFeed[i].composition.setFeedCode(i);
            allUnutilisedGrazableFeed[i].composition.Setamount(0);
        }
    }
    //!  Add Product Produced. Taking one argument.
    /*!
     \param anItem, a feedItem class instance. 
   */
    public void AddProductProduced(feedItem anItem)
    {
        allFeedAndProductsProduced[anItem.GetFeedCode()].composition.Setname(anItem.GetName());
        allFeedAndProductsProduced[anItem.GetFeedCode()].composition.AddFeedItem(anItem, false);
    }
    //!  Add Grazable Product Unutilised. Taking one argument.
    /*!
     \param anItem, a feedItem class instance. 
   */
    public void AddGrazableProductUnutilised(feedItem anItem)
    {
        allUnutilisedGrazableFeed[anItem.GetFeedCode()].composition.Setname(anItem.GetName());
        allUnutilisedGrazableFeed[anItem.GetFeedCode()].composition.AddFeedItem(anItem, false);
    }
    //!  Test all Feed and Products used. 
    /*!
      Print to screen the amount of N in products used. Only for debugging.
   */
   
    public void testallFeedAndProductsUsed()
    {
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if (allFeedAndProductsUsed[i].composition.Getamount() > 0)
                Console.WriteLine(allFeedAndProductsUsed[i].composition.GetFeedCode() + " " + allFeedAndProductsUsed[i].composition.Getamount() * allFeedAndProductsUsed[i].composition.GetN_conc());
        }
    }
    //!  Calculate Trade Balance.
    public void CalculateTradeBalance()
    {
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            product feedItemUsed = allFeedAndProductsUsed[i];
            product feedItemProduced = allFeedAndProductsProduced[i];
            if ((feedItemUsed.composition.Getamount() > 0) || (feedItemProduced.composition.Getamount() > 0))
            {
                if (feedItemUsed.composition.Getamount() > 0)
                {
                    if (feedItemProduced.composition.Getamount() == 0)
                    {
                        if (feedItemUsed.composition.GetFeedCode() == 999)
                        {
                            allFeedAndProductTradeBalance[i].composition.AddFeedItem(allFeedAndProductsUsed[999].composition, true, true);
                            allFeedAndProductTradeBalance[i].composition.Setamount(0);
                        }
                        else
                            allFeedAndProductTradeBalance[i].composition.GetStandardFeedItem(i);
                    }
                }
                if (feedItemProduced.composition.Getamount() > 0)
                    allFeedAndProductTradeBalance[i].composition.AddFeedItem(feedItemProduced.composition, true);
                if (feedItemUsed.composition.Getamount() > 0)
                    allFeedAndProductTradeBalance[i].composition.SubtractFeedItem(feedItemUsed.composition, true);
            }
        }
    }
    //!  Get Plant Product Imports. Returing a product instance.
    /*!
     \return a class product instance.
  */
    public product GetPlantProductImports()
    {
        product aProduct = new product();
        aProduct.composition = new feedItem();
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            product productToAdd = new product();
            productToAdd.composition = new feedItem();
            if (allFeedAndProductTradeBalance[i].composition.Getamount() < 0)
            {
                productToAdd.composition.AddFeedItem(allFeedAndProductTradeBalance[i].composition, true);
                productToAdd.composition.Setamount(allFeedAndProductTradeBalance[i].composition.Getamount() * -1.0);
                aProduct.composition.AddFeedItem(productToAdd.composition, true);
                double N = productToAdd.composition.Getamount() * productToAdd.composition.GetN_conc();

                GlobalVars.Instance.log(productToAdd.composition.GetFeedCode().ToString() + " " + productToAdd.composition.Getamount().ToString("0.0") + " " + N.ToString("0.0"), 5);
            }
        }
        return aProduct;
    }
    //!  Get Bedding Imported. Returing a feedItem instance.
    /*!
     \return a class feedItem instance.
  */
    public feedItem GetBeddingImported()
    {
        feedItem beddingItem = new feedItem();
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if ((allFeedAndProductTradeBalance[i].composition.GetbeddingMaterial()) && (allFeedAndProductTradeBalance[i].composition.Getamount() < 0))
                beddingItem.AddFeedItem(allFeedAndProductTradeBalance[i].composition, true, true);
        }
        beddingItem.Setamount(beddingItem.Getamount() * -1);
        return beddingItem;
    }
    //!  Get Plant Product Exports. Returing a product instance.
    /*!
     \return a class product instance.
  */

    public product GetPlantProductExports()
    {
        product aProduct = new product();
        aProduct.composition = new feedItem();
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            product productToAdd = new product();
            productToAdd.composition = new feedItem();
            if (allFeedAndProductTradeBalance[i].composition.Getamount() > 0)
            {
                productToAdd.composition.AddFeedItem(allFeedAndProductTradeBalance[i].composition, true);
                aProduct.composition.AddFeedItem(productToAdd.composition, true);
            }
        }
        return aProduct;
    }
    //!  Calculate all Feed and Products Potential. Taking one list argument.
    /*!
     \param list, a list argument that points to CropSequenceClass.
  */

    public void CalcAllFeedAndProductsPotential(List<CropSequenceClass> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            List<CropClass> crops = list[i].GettheCrops();
            int yearsInSequence = list[i].GetlengthOfSequence();
            for (int j = 0; j < crops.Count; j++)
            {
                List<product> products = crops[j].GettheProducts();
                for (int k = 0; k < products.Count; k++)
                {
                    int feedCode = products[k].composition.GetFeedCode();
                    if (products[k].Potential_yield > 0)
                    {
                        allFeedAndProductsPotential[feedCode].composition.Addamount(crops[j].getArea() * products[k].Potential_yield / yearsInSequence);
                        allFeedAndProductsPotential[feedCode].composition.Setname(products[k].composition.GetName());
                    }
                    if (products[k].Modelled_yield > 0)
                    {
                        allFeedAndProductFieldProduction[feedCode].composition.Addamount(crops[j].getArea() * products[k].Modelled_yield / yearsInSequence);
                        allFeedAndProductFieldProduction[feedCode].composition.Setname(products[k].composition.GetName());
                    }
                }

            }
        }
    }
    //!  Calculate all Feed and Products Potential. Taking two arguments.
    /*!
     \param importedRoughageDM, a double argument.
     \param exportedRoughageDM, a double argument.
  */
    public void GetRoughageExchange(ref double importedRoughageDM, ref double exportedRoughageDM)
    {
        importedRoughageDM = 0;
        exportedRoughageDM = 0;
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            int feedCode = allFeedAndProductTradeBalance[i].composition.GetFeedCode();
            double amount = allFeedAndProductTradeBalance[i].composition.Getamount();
            if ((feedCode >= 400) && (feedCode < 700))
            {
                if (amount > 0)
                    exportedRoughageDM += amount;
                if (amount < 0)
                    importedRoughageDM -= amount;
            }
        }
    }
    //!  Print Plant Products.
    
    public void PrintPlantProducts()
    {
        double totDM = 0;
        double totN = 0;
        double totC = 0;
        GlobalVars.Instance.log("Fdcode FdAndProdProduced FdAndProdUsed  FdAndProdTradeBalance", 5);
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if ((allFeedAndProductsProduced[i].composition.Getamount() != 0) || (allFeedAndProductsUsed[i].composition.Getamount() != 0))
            {
                totDM += allFeedAndProductsProduced[i].composition.Getamount();
                totN += allFeedAndProductsProduced[i].composition.Getamount() * allFeedAndProductsProduced[i].composition.GetN_conc();
                totC += allFeedAndProductsProduced[i].composition.Getamount() * allFeedAndProductsProduced[i].composition.GetC_conc();

                GlobalVars.Instance.log(allFeedAndProductTradeBalance[i].composition.GetFeedCode().ToString() + " " +
                    allFeedAndProductsProduced[i].composition.Getamount().ToString("0.0") + " " +
                    allFeedAndProductsUsed[i].composition.Getamount().ToString("0.0") + " " +
                     allFeedAndProductTradeBalance[i].composition.Getamount().ToString("0.0"), 5);
            }
        }
        GlobalVars.Instance.log("Tot DM produced " + totDM.ToString("0.0"), 5);
        GlobalVars.Instance.log("Tot C produced " + totC.ToString("0.0"), 5);
        GlobalVars.Instance.log("Tot N produced " + totN.ToString("0.0"), 5);
    }
    //!  Check Grazing Data.
    /*!
     \return an integer value.
  */
    public int CheckGrazingData()
    {
        double diff = 0;
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if ((grazedArray[i].fieldDMgrazed > 0) || (grazedArray[i].ruminantDMgrazed > 0.0))
            {
                double expectedDM = grazedArray[i].fieldDMgrazed;
                double actualDM = grazedArray[i].ruminantDMgrazed;
                if (expectedDM == 0.0)
                {
                    string messageString = "Error; grazed feed item not produced on farm.\n feed code = " + i.ToString();
                    GlobalVars.instance.Error(messageString);

                }
                else
                {
                    diff = (expectedDM - actualDM) / expectedDM;
                    double tolerance = GlobalVars.Instance.getmaxToleratedErrorGrazing();
                    if ((tolerance <= 1) && (Math.Abs(diff) > tolerance))
                    {
                        double errorPercent = 100 * diff;
                        string productName = grazedArray[i].name;
                        string messageString = "";
                        if (expectedDM>actualDM)
                            messageString= "Error; modelled production of grazed DM exceeds livestock requirement for grazed DM for " +
                            productName + " by more than the permitted margin.\n Percentage error = " + errorPercent.ToString("0.00") + "%";
                        else
                            messageString = "Error; Livestock requirement for grazed DM exceeds modelled production of grazed DM for " +
                            productName + " by more than the permitted margin.\n Percentage error = " + errorPercent.ToString("0.00") + "%";
                        GlobalVars.instance.Error(messageString);
                    }
                }
            }
        }
        return 0;
    }
    //!  Return Temperature. Taking six arguments and
    /*!
     \param avgTemperature, a double argument.
     \param dampingDepth, a double argument.
     \param day, an integer argument.
     \param depth, a double argument.
     \param amplitude, a double argument.
     \param offset, an integer argument.
     \return a double value.
  */
    public double Temperature(double avgTemperature, double dampingDepth, int day, double depth, double amplitude, int offset)
    {
        double retVal = 0;
        double rho = 3.1415926 * 2.0 / 365.0;
        if (dampingDepth == 0)
            retVal = avgTemperature + amplitude * Math.Sin(rho * (day + offset));
        else
            retVal = avgTemperature + amplitude * Math.Exp(-depth / dampingDepth) * Math.Sin(rho * (day + offset) - depth / dampingDepth);
        return retVal;
    }

    //!  Get Degree Days. Taking six arguments and
    /*!
     \param startDay, an integer argument.
     \param endDay, an integer argument.
     \param basetemperature, a double argument.
     \param averageTemperature, a double argument.
     \param amplitude, a double argument.
     \param offset, an integer argument.
     \return a double value.
  */
    public double GetDegreeDays(int startDay, int endDay, double basetemperature, double averageTemperature, double amplitude, int offset)
    {
        double retVal = 0;
        for (int i = startDay; i <= endDay; i++)
            retVal += Temperature(averageTemperature, 0.0
                , i, 0.0, amplitude, offset) - basetemperature;
        return retVal;
    }
    //!  Get Concentrate Exports.
    /*!
     \return a double value.
  */
    public double GetConcentrateExports()
    {
        double ret_val = 0;
        product aProduct = new product();
        for (int i = 0; i < maxNumberFeedItems; i++)
        {
            if (allFeedAndProductTradeBalance[i].composition.Getamount() > 0)
            {
                if (allFeedAndProductTradeBalance[i].composition.isConcentrate()) ;
                ret_val += allFeedAndProductTradeBalance[i].composition.Getamount();
            }
        }
        return ret_val;
    }
    //!  Write. Taking one argument.
    /*!
     \param fullModelRun, a boolean argument.
  */
    public void Write(bool fullModelRun)
    {
        theManureExchange.Write();
        if (!fullModelRun)
        {
            GlobalVars.Instance.writeStartTab("PotentialBalance");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if ((allFeedAndProductsUsed[i].composition.Getamount() > 0) || (allFeedAndProductsPotential[i].composition.Getamount() > 0))
                {
                    GlobalVars.Instance.writeStartTab("FeedItem");
                    GlobalVars.Instance.writeInformationToFiles("name", "Name", "-", allFeedAndProductsUsed[i].composition.GetName(), parens);
                    GlobalVars.Instance.writeInformationToFiles("Used", "Used", "kg", allFeedAndProductsUsed[i].composition.Getamount(), parens);
                    GlobalVars.Instance.writeInformationToFiles("Potential", "Potential", "kg", allFeedAndProductsPotential[i].composition.Getamount(), parens);
                    GlobalVars.Instance.writeInformationToFiles("Expected", "Expected", "kg", allFeedAndProductsProduced[i].composition.Getamount(), parens);
                    GlobalVars.Instance.writeEndTab();
                }
            }
            GlobalVars.Instance.writeEndTab();
        }

        if (fullModelRun)
        {
            GlobalVars.Instance.writeStartTab("FeedAndProductsUsed");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if (allFeedAndProductsUsed[i].composition.Getamount() > 0)
                {
                    allFeedAndProductsUsed[i].composition.Write("Used");
                }
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("FeedAndProductsProduced");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if (allFeedAndProductsProduced[i].composition.Getamount() > 0)
                {
                    allFeedAndProductsProduced[i].composition.Write("Produced");
                }
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("FeedAndProductTradeBalance");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if ((allFeedAndProductsUsed[i].composition.Getamount() > 0) || (allFeedAndProductsProduced[i].composition.Getamount() > 0))
                {
                    allFeedAndProductTradeBalance[i].composition.Write("Balance");
                }
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("FeedAndProductsPotential");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if (allFeedAndProductsPotential[i].composition.Getamount() > 0)
                    allFeedAndProductsPotential[i].composition.Write("Potential");
            }
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("FeedAndProductsField");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if (allFeedAndProductFieldProduction[i].composition.Getamount() > 0)
                    allFeedAndProductFieldProduction[i].composition.Write("Field");
            }
            GlobalVars.Instance.writeEndTab();

            GlobalVars.Instance.writeStartTab("allUnutilisedGrazableFeed");
            for (int i = 0; i < maxNumberFeedItems; i++)
            {
                if (allUnutilisedGrazableFeed[i].composition.Getamount() > 0)
                    allUnutilisedGrazableFeed[i].composition.Write("UnutilisedGrazableFeed");
            }
            GlobalVars.Instance.writeEndTab();
        }
    }
    //!  Log. Taking two arguments.
    /*!
     \param informatio, a string argument.
     \param level, an integer argument.
  */
    public void log(string informatio, int level)
    {
#if !server
        if (level <= verbosity)
        {
            if (logScreen)
                Console.WriteLine(informatio);
            if (logFile)
            {
                try
                {
                    logFileStream.WriteLine(informatio);

                }
                catch
                {

                }

            }
        }
#endif
    }

    /*!
 \param information, a string argument.
 \param units, a string argument.
 \param amount, a double argument.
*/
public void CloseLogFile()
    {
#if !server
        if (logFileStream != null)
            logFileStream.Close();
#endif
    }

    //!  Write Summary Excel. Taking three arguments.
    /*!
     \param information, a string argument.
     \param units, a string argument.
     \param amount, a double argument.
  */
    public void writeSummaryExcel(string information, string units, double amount)
    {

        writeSummaryExcel(information, units, Convert.ToString(amount));

    }
    //!  Write Summary Excel. Taking three arguments.
    /*!
     \param information, a string argument.
     \param units, a string argument.
     \param amount, a string argument.
  */
    public void writeSummaryExcel(string information, string units, string amount)
    {
        if (WriteSummaryExcel)
            SummaryExcel.WriteLine(information + '\t' + units + '\t' + amount);

    }
    //!  Open Summary Excel. Taking three arguments.
    /*!
     \param outputDir, a string argument.
     \param scenarioNr, a string argument.
     \param farmNr, a string argument.
  */
    public void openSummaryExcel(string outputDir, string scenarioNr, string farmNr)
    {
        if (WriteSummaryExcel)
            SummaryExcel = new System.IO.StreamWriter(outputDir + "SummaryExcel" + farmNr + "_" + scenarioNr + ".xls");
    }
    //!  Close Summary Excel. 
    public void closeSummaryExcel()
    {
        try
        {
            if (WriteSummaryExcel)
                SummaryExcel.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            log(e.Message, 6);
            log("Cannot close summary Excel file", 6);
        }
    }
    //!  Calculate the time-integrated mass load following a series of CO2 emissions (units = kg CO2 per year per kg CO2).
    /*!
     \param timeHorizon, the duration over which the integration should occur, as an integer argument.
     \param emissionYear, the year when the pulse was emitted, as an integer argument.
      \return reVal mass load, as a double value.
 */
    public double CalcCseqEffect(double timeHorizon, double [] CO2emission)
    {
        double retVal = 0.0;
        for (int i = 0; i < timeHorizon; i++)
        {
            retVal += CO2emission[i] * CalcCseqEffectPulse(timeHorizon, i);
        }
        return retVal;
    }

    //!  Calculate the time-integrated mass load following a pulse emission of a unit mass of CO2 (units = kg CO2 per year per kg CO2).
    /*!
     \param timeHorizon, the duration over which the integration should occur, as an double argument.
     \param emissionYear, the year when the pulse was emitted, as a double argument.
      \return reVal cumulative mass load, as a double value.
 */
    public double CalcCseqEffectPulse(double timeHorizon, double emissionYear)
    {
        //see doi:10.5194/acp-13-2793-2013
        const double alpha_0 = 0.2173;
        const double alpha_1 = 0.2240;
        const double alpha_2 = 0.2824;
        const double alpha_3 = 0.2763;
        const double tor_1 = 394.4;
        const double tor_2 = 36.54;
        const double tor_3 = 4.304;
        double retVal = 0.0;
        retVal = alpha_0 * (timeHorizon - emissionYear) + alpha_1 * tor_1 * (1 - Math.Exp(-(timeHorizon - emissionYear) / tor_1))
                + alpha_2 * tor_2* (1 - Math.Exp(-(timeHorizon - emissionYear) / tor_2))
                + alpha_3 * tor_3* (1 - Math.Exp(-(timeHorizon - emissionYear) / tor_3));
        return retVal;
    }
}