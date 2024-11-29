using System;
using System.Collections.Generic;
using System.Xml;
/*! A class that named CropClass */
public class CropClass
{    
    //! List of manures applied to this crop
    public List<manure> theManureApplied;
    //!  a string containing information about the farm and scenario number.
    public string parens; 
    //!  Copy CropClass constructor.
    /*!
      \param theCropToBeCopied, a class argument that points to CropClass.
      \return a copy of the CropClass instance
    */
    public CropClass(CropClass theCropToBeCopied)
    {
        name = theCropToBeCopied.name;
        identity = theCropToBeCopied.identity;
        cropSequenceNo = theCropToBeCopied.cropSequenceNo;
        area = theCropToBeCopied.area;
        theStartDate = new timeClass(theCropToBeCopied.theStartDate);       
        theEndDate = new timeClass(theCropToBeCopied.theEndDate);
        if (theCropToBeCopied.residueToNext!=null)
            residueToNext = new GlobalVars.product(theCropToBeCopied.residueToNext);
        isIrrigated = theCropToBeCopied.isIrrigated;
        fertiliserApplied=new List<fertRecord>();
        for (int i = 0; i < theCropToBeCopied.fertiliserApplied.Count; i++)
            fertiliserApplied.Add(new fertRecord( theCropToBeCopied.fertiliserApplied[i]));
        manureApplied=new List<fertRecord>();
        for (int i = 0; i < theCropToBeCopied.manureApplied.Count; i++)
            manureApplied.Add(new fertRecord(theCropToBeCopied.manureApplied[i]));
        propAboveGroundResidues[0] = theCropToBeCopied.propAboveGroundResidues[0];
        propAboveGroundResidues[1] = theCropToBeCopied.propAboveGroundResidues[1];
        propBelowGroundResidues = theCropToBeCopied.propBelowGroundResidues;
        CconcBelowGroundResidues = theCropToBeCopied.CconcBelowGroundResidues;
        CtoNBelowGroundResidues = theCropToBeCopied.CtoNBelowGroundResidues;
        NDepositionRate = theCropToBeCopied.NDepositionRate;
        urineNH3EmissionFactor = theCropToBeCopied.urineNH3EmissionFactor;
        manureN2OEmissionFactor = theCropToBeCopied.manureN2OEmissionFactor;
        fertiliserN2OEmissionFactor = theCropToBeCopied.fertiliserN2OEmissionFactor;
        soilN2OEmissionFactor = theCropToBeCopied.soilN2OEmissionFactor;
        soilN2Factor = theCropToBeCopied.soilN2Factor;
        harvestMethod = theCropToBeCopied.harvestMethod;
        MaximumRootingDepth = theCropToBeCopied.MaximumRootingDepth;
        NfixationFactor = theCropToBeCopied.NfixationFactor;
        duration = theCropToBeCopied.duration;
        permanent = theCropToBeCopied.permanent;

        theProducts = new List<GlobalVars.product>();
        for (int i = 0; i < theCropToBeCopied.theProducts.Count; i++)
        {
            GlobalVars.product aProduct=new  GlobalVars.product(theCropToBeCopied.theProducts[i]);
            theProducts.Add(aProduct);
        }
        CFixed = theCropToBeCopied.CFixed;
        Nfixed = theCropToBeCopied.Nfixed;
        nAtm = theCropToBeCopied.nAtm;
        manureNH3emission = theCropToBeCopied.manureNH3emission;
        fertiliserNH3emission = theCropToBeCopied.fertiliserNH3emission;
        urineNH3emission = theCropToBeCopied.urineNH3emission;
        surfaceResidueC = theCropToBeCopied.surfaceResidueC;
        subsurfaceResidueC = theCropToBeCopied.subsurfaceResidueC;
        surfaceResidueN = theCropToBeCopied.surfaceResidueN;
        subsurfaceResidueN = theCropToBeCopied.subsurfaceResidueN;
        manureFOMCsurface = new double[theCropToBeCopied.manureFOMCsurface.Length];
        for (int i = 0; i < theCropToBeCopied.manureFOMCsurface.Length; i++)
        {
            manureFOMCsurface[i] = theCropToBeCopied.manureFOMCsurface[i];
        }
        manureHUMCsurface = new double[theCropToBeCopied.manureHUMCsurface.Length];
        for (int i = 0; i < theCropToBeCopied.manureHUMCsurface.Length; i++)
        {
            manureHUMCsurface[i] = theCropToBeCopied.manureHUMCsurface[i];
        }
        manureBiocharCsurface = new double[theCropToBeCopied.manureBiocharCsurface.Length];
        for (int i = 0; i < theCropToBeCopied.manureBiocharCsurface.Length; i++)
        {
            manureBiocharCsurface[i] = theCropToBeCopied.manureBiocharCsurface[i];
        }
        manureFOMNsurface = new double[theCropToBeCopied.manureFOMNsurface.Length];
        for (int i = 0; i < theCropToBeCopied.manureFOMNsurface.Length; i++)
        {
            manureFOMNsurface[i] = theCropToBeCopied.manureFOMNsurface[i];
        }
        manureHUMNsurface = new double[theCropToBeCopied.manureHUMNsurface.Length];
        for (int i = 0; i < theCropToBeCopied.manureHUMNsurface.Length; i++)
        {
            manureHUMNsurface[i] = theCropToBeCopied.manureHUMNsurface[i];
        }
        manureTAN = new double[theCropToBeCopied.manureTAN.Length];
        for (int i = 0; i < theCropToBeCopied.manureTAN.Length; i++)
        {
            manureTAN[i] = theCropToBeCopied.manureTAN[i];
        }
        fertiliserN = new double[theCropToBeCopied.fertiliserN.Length];
        for (int i = 0; i < theCropToBeCopied.fertiliserN.Length; i++)
            fertiliserN[i] = theCropToBeCopied.fertiliserN[i];
        nitrificationInhibitor = new double[theCropToBeCopied.nitrificationInhibitor.Length];
        for (int i = 0; i < theCropToBeCopied.nitrificationInhibitor.Length; i++)
            nitrificationInhibitor[i] = theCropToBeCopied.nitrificationInhibitor[i];
        droughtFactorPlant = new double[theCropToBeCopied.droughtFactorPlant.Length];
        for (int i = 0; i < theCropToBeCopied.droughtFactorPlant.Length; i++)
            droughtFactorPlant[i] = theCropToBeCopied.droughtFactorPlant[i];
        droughtFactorSoil = new double[theCropToBeCopied.droughtFactorSoil.Length];
        for (int i = 0; i < theCropToBeCopied.droughtFactorSoil.Length; i++)
            droughtFactorSoil[i] = theCropToBeCopied.droughtFactorSoil[i];
        dailyNitrateLeaching = new double[theCropToBeCopied.dailyNitrateLeaching.Length];
        for (int i = 0; i < theCropToBeCopied.dailyNitrateLeaching.Length; i++)
            dailyNitrateLeaching[i] = theCropToBeCopied.dailyNitrateLeaching[i];
        LAI = new double[theCropToBeCopied.LAI.Length];
        for (int i = 0; i < theCropToBeCopied.LAI.Length; i++)
            LAI[i] = theCropToBeCopied.LAI[i];
        drainage = new double[theCropToBeCopied.drainage.Length];
        for (int i = 0; i < theCropToBeCopied.drainage.Length; i++)
            drainage[i] = theCropToBeCopied.drainage[i];
        transpire = new double[theCropToBeCopied.transpire.Length];
        for (int i = 0; i < theCropToBeCopied.transpire.Length; i++)
            transpire[i] = theCropToBeCopied.transpire[i];
        dailyCanopyStorage = new double[theCropToBeCopied.dailyCanopyStorage.Length];
        for (int i = 0; i < theCropToBeCopied.dailyCanopyStorage.Length; i++)
            dailyCanopyStorage[i] = theCropToBeCopied.dailyCanopyStorage[i];
        evaporation = new double[theCropToBeCopied.evaporation.Length];
        for (int i = 0; i < theCropToBeCopied.evaporation.Length; i++)
            evaporation[i] = theCropToBeCopied.evaporation[i];
        irrigationWater = new double[theCropToBeCopied.irrigationWater.Length];
        for (int i = 0; i < theCropToBeCopied.irrigationWater.Length; i++)
            irrigationWater[i] = theCropToBeCopied.irrigationWater[i];
        soilWater = new double[theCropToBeCopied.soilWater.Length];
        for (int i = 0; i < theCropToBeCopied.soilWater.Length; i++)
            soilWater[i] = theCropToBeCopied.soilWater[i];
        plantavailableWater = new double[theCropToBeCopied.plantavailableWater.Length];
        for (int i = 0; i < theCropToBeCopied.plantavailableWater.Length; i++)
            plantavailableWater[i] = theCropToBeCopied.plantavailableWater[i];

        fertiliserC = theCropToBeCopied.fertiliserC;
        urineC = theCropToBeCopied.urineC;
        faecalC = theCropToBeCopied.faecalC;
        harvestedC = theCropToBeCopied.harvestedC;
        harvestedDM = theCropToBeCopied.harvestedDM;
        storageProcessingCLoss = theCropToBeCopied.storageProcessingCLoss;
        storageProcessingNLoss = theCropToBeCopied.storageProcessingNLoss;
        mineralNavailable = theCropToBeCopied.mineralNavailable;
        excretaNInput = theCropToBeCopied.excretaNInput;
        excretaCInput = theCropToBeCopied.excretaCInput;
        fertiliserNinput = theCropToBeCopied.fertiliserNinput;
        harvestedN = theCropToBeCopied.harvestedN;
        totalManureNApplied = theCropToBeCopied.totalManureNApplied;
        N2ONemission = theCropToBeCopied.N2ONemission;
        N2Nemission = theCropToBeCopied.N2Nemission;
        cropNuptake = theCropToBeCopied.cropNuptake;
        urineNasFertilizer = theCropToBeCopied.urineNasFertilizer;
        faecalN = theCropToBeCopied.faecalN;
        OrganicNLeached = theCropToBeCopied.OrganicNLeached;
        soilNMineralisation = theCropToBeCopied.soilNMineralisation;
        mineralNFromLastCrop = theCropToBeCopied.mineralNFromLastCrop;
        mineralNToNextCrop = theCropToBeCopied.mineralNToNextCrop;
        fertiliserN2OEmission = theCropToBeCopied.fertiliserN2OEmission;
        manureN2OEmission = theCropToBeCopied.manureN2OEmission;
        soilN2OEmission = theCropToBeCopied.soilN2OEmission;
        nitrateLeaching = theCropToBeCopied.nitrateLeaching;
        cropSequenceName = theCropToBeCopied.cropSequenceName;
        droughtSusceptability = theCropToBeCopied.droughtSusceptability;
        totalTsum = theCropToBeCopied.totalTsum;
        maxLAI= theCropToBeCopied.maxLAI;
        irrigationThreshold = theCropToBeCopied.irrigationThreshold;
        irrigationMinimum = theCropToBeCopied.irrigationMinimum;
    }
    //! Name of the crop
    string name;
    int identity;
    //! area of crop cultivated (ha)
    double area;
    //! date at which the crop starts
    public timeClass theStartDate;
    //! date at which the crop ends
    timeClass theEndDate;
    //! true if the crop can be irrigated
    bool isIrrigated;
    //! name of the crop sequence to which it belongs
    string cropSequenceName;
    //! number of the crop sequence to which it belongs
    int cropSequenceNo;
    //! list of the details of fertiliser applications made to this crop
    public List<fertRecord> fertiliserApplied;
    //! list of the details of manure applications made to this crop
    public List<fertRecord> manureApplied;

    //! The above ground crop residues, as a proportion of the crop yield
    /*!
     * Array of two doubles. Element 0 contains the proportion when the crop is mechanically harvested
     * Element 0 contains the proportion when the crop is harvested by grazing
     */
    double[] propAboveGroundResidues = new double[2];
    //! The below ground crop residues, as a proportion of the crop yield
    double propBelowGroundResidues;
    //! Concentration of C in below ground residues (kg C/kg DM)
    double CconcBelowGroundResidues;
    //! C:N ratio of below ground crop residues
    double CtoNBelowGroundResidues;
    //! Rate of N deposition from the atmosphere (kg/ha/yr)
    double NDepositionRate;
    //! Emission factor for NH3 for urine (kg NH3-N/kg urine-N)
    double urineNH3EmissionFactor;
    //! Emission factor for N2O for manure (kg N2O-N/kg N)
    double manureN2OEmissionFactor;
    //! Emission factor for N2O for fertiliser (kg N2O-N/kg N)
    double fertiliserN2OEmissionFactor;
    //! Emission factor for N2O for mineralised soil N (kg N2O-N/kg N)
    double soilN2OEmissionFactor;
    //! Emission factor for N2 for soil (kg N2-N/kg N2O-N emitted)
    double soilN2Factor;
    //! name of harvesting method
    string harvestMethod;
    //! Maximum rooting depth of the crop (m)
    double MaximumRootingDepth;
    //! Proportion of the difference between optimum and actual availability of N to the crop that is supplied by N fixation (max = 1.0)
    double NfixationFactor;
    //! Base temperature for growth (Celsius)
    double baseTemperature;
    //! Duration of the crop (days)
    long duration = 0;
    //! If true, the below-ground crop material is not converted to residues at the end of the crop (LAI = max LAI and rooting depth = max rooting depth)
    bool permanent = false;
    //! If = 1.0, the drought index has full effect, if = 0, the drought index has no effect
    double droughtSusceptability = 1.0;
    //! If true, all gas emissions are set to zero (used for debugging only)
    bool zeroGasEmissionsDebugging = false;
    //! If true, N leaching is set to zero (used for debugging only)
    bool zeroLeachingDebugging = false;
    //! Threshold for irrigation (proportion of soil moisture deficit)
    double irrigationThreshold = 0.5;
    //! Minimum irrigation amount (mm)
    double irrigationMinimum = 5.0;
    // Daily potential evapotranspiration of crop (mm)
    double[] potentialEvapoTrans = new double[366];
    //! Daily precipitation (mm)
    double[] precipitation = new double[366];
    //! Daily air temperature (Celsius)
    double[] temperature = new double[366];
    //! Daily temperature sum (degrees Celsius days)
    double[] Tsum = new double[366];
    //! Daily evaporation of crop (mm)
    double[] evaporation;
    //! Daily drainage of crop (mm)
    double[] drainage;
    //! Daily transpiration of crop (mm)
    double[] transpire;
    //! Daily amount of water in the soil (mm)
    double[] soilWater;
    //! Daily amount of plant-available water in the soil (mm)
    double[] plantavailableWater;
    //! Daily leaf area index of the crop
    double[] LAI;
    //! Daily amount of irrigation water of the crop (mm)
    double[] irrigationWater;
    //! Daily drought factor of the crop (affects growth)
    double[] droughtFactorPlant;
    //! Daily drought factor of the soil (affects organic matter decomposition)
    double[] droughtFactorSoil;
    //! Daily amount of nitrate leaching (kg N/ha)
    double[] dailyNitrateLeaching;
    //! Daily amount of water stored in the crop canopy (mm)
    double[] dailyCanopyStorage;
    //! List of the products produced by the crop
    List<GlobalVars.product> theProducts = new List<GlobalVars.product>();

    //other variables
    //!carbon fixed (kg/ha)
    double CFixed = 0;
    //! C in surface crop residues (kg/ha)
    double surfaceResidueC;
    //! C in subsurface crop residues (kg/ha)
    double subsurfaceResidueC;
    //! C in urine input to the crop (kg/ha)
    double urineC = 0;
    //! C in faeces input to the crop (kg/ha)
    double faecalC = 0;
    //! C in urine input to the crop (kg/ha)
    double grazedC = 0;
    //! C emitted in methane during grazing (kg/ha)
    double grazingCH4C = 0;
    //! C in harvest material (kg/ha)
    double harvestedC = 0;
    //! dry matter in harvest material (kg/ha)
    double harvestedDM = 0;
    //! C lost during storage of crop products (kg/ha)
    double storageProcessingCLoss = 0;
    //! N lost during storage of crop products (kg/ha)
    double storageProcessingNLoss = 0;
    //! DM lost during storage of crop products (kg/ha)
    double storageProcessingDMLoss = 0;
    //! Maximum N yield (kg/ha)
    double NyieldMax = 0;
    //! Maximum crop N uptake (kg/ha)
    double maxCropNuptake = 0;
    //! Maximum N uptake capacity of the crop (kg/ha)
    double modelledCropNuptake = 0;
    //! N fixation (kg/ha)
    double Nfixed = 0;
    //! N deposition from the atmosphere (kg/ha)
    double nAtm = 0;
    //! manure NH3 emission (kg N/ha)
    double manureNH3emission = 0;
    //! fertiliser NH3 emission (kg N/ha)
    double fertiliserNH3emission = 0;
    //! urine NH3 emission (kg N/ha)
    double urineNH3emission = 0;
    //! N in surface crop residues (kg/ha)
    double surfaceResidueN;
    //! N in subsurface crop residues (kg/ha)
    double subsurfaceResidueN;
    //! DM in surface crop residues (kg/ha)
    double surfaceResidueDM = 0;
    //! C emitted from fertilisers (kg/ha)
    double fertiliserC = 0;
    //! Mineral N available in the soil (kg/ha)
    double mineralNavailable = 0;
    //! N input in excreta (kg/ha)
    double excretaNInput = 0;
    //! C input in excreta (kg/ha)
    double excretaCInput = 0;
    //! N input in fertiliser (kg/ha)
    double fertiliserNinput = 0;
    //! N harvested (kg/ha)
    double harvestedN = 0;
    //! N grazed (kg/ha)
    double grazedN = 0;
    //! total N input in manure (kg/ha)
    double totalManureNApplied = 0;
    //! N2O emission (kg/ha)
    double N2ONemission = 0;
    //! N emission (kg/ha)
    double N2Nemission = 0;
    //! N taken up by the crop (kg/ha)
    double cropNuptake = 0;
    //! N input in urine (kg/ha)
    double urineNasFertilizer = 0;
    //! N input in faeces (kg/ha)
    double faecalN = 0;
    //! C in crop residues burnt (kg/ha)
    double burntResidueC = 0;
    //! N in crop residues burnt (kg/ha)
    double burntResidueN = 0;
    //! Emission of N2O-N due to crop residue burning (kg/ha)
    double burningN2ON = 0;
    //! Emission of NH3-N due to crop residue burning (kg/ha)
    double burningNH3N = 0;
    //! Emission of NOx-N due to crop residue burning (kg/ha)
    double burningNOxN = 0;
    //! Emission of C in CO due to crop residue burning (kg/ha)
    double burningCOC = 0;
    //! Emission of C in CO2 due to crop residue burning (kg/ha)
    double burningCO2C = 0;
    //! Emission of black carbon due to crop residue burning (kg/ha)
    double burningBlackC = 0;
    //! Emission of N in other gases and particulates, due to crop residue burning (kg/ha)
    double burningOtherN = 0;
    //! Leaching of N in organic forms (kg/ha)
    double OrganicNLeached = 0;
    //! Mineralisation of soil N (kg/ha)
    double soilNMineralisation = 0;
    //! Soil mineral N remaining from previous (kg/ha)
    double mineralNFromLastCrop = 0;
    //! Soil mineral N remaining in the soil for the following crop (kg/ha)
    double mineralNToNextCrop = 0;
    //! C in crop residues remaining on the soil surface from the previous crop (kg/ha)
    double residueCfromLastCrop = 0;
    //! N in crop residues remaining on the soil surface from the previous crop (kg/ha)
    double residueNfromLastCrop = 0;
    //! C in crop residues remaining on the soil surface for the following crop (kg/ha)
    double residueCtoNextCrop = 0;
    //! N in crop residues remaining on the soil surface for the following crop (kg/ha)
    double residueNtoNextCrop = 0;
    //! N2O-N emission from fertilizer applied (kg/ha)
    double fertiliserN2OEmission = 0;
    //! N2O-N emission from manure applied (kg/ha)
    double manureN2OEmission;
    //! N2O-N emission from mineralised soil organic matter (kg/ha)
    double soilN2OEmission = 0;
    //! Nitrate leaching (kg N/ha)
    double nitrateLeaching = 0;
    //! Length of the crop sequence to which this crop belongs (days)
    double lengthOfSequence = 0;
    //! Grazable dry matter that is produced but not consumed (kg/ha)
    double unutilisedGrazableDM = 0;
    //! Grazable C that is produced but not consumed (kg/ha)
    double unutilisedGrazableC = 0;
    //! Grazable N that is produced but not consumed (kg/ha)
    double unutilisedGrazableN = 0;
    //! Cumulative temperature sum for the duration of the crop (degree days)
    double totalTsum = 0;
    //! Maximum leaf area index of the crop
    double maxLAI = 5.0;
    //! Stores the amount of manure FOM C applied to the soil surface on a given day (kg/ha)
    public double [] manureFOMCsurface;
    //! Stores the amount of manure HUM C applied to the soil surface on a given day (kg/ha)
    double[] manureHUMCsurface;
    //! Stores the amount of biochar C applied to the soil surface on a given day (kg/ha)
    double[] manureBiocharCsurface;
    //! Stores the amount of manure FOM C applied below the soil surface on a given day (kg/ha)
    //! Stores the amount of manure FOM N applied to the soil surface on a given day (kg/ha)
    double[] manureFOMNsurface;
    //! Stores the amount of manure HUM N applied to the soil surface on a given day (kg/ha)
    double[] manureHUMNsurface;
    //! Stores the amount of manure TAN applied to the soil surface on a given day (kg/ha)
    double[] manureTAN;
    //! Stores the amount of fertiliser N applied to the soil surface on a given day (kg/ha)
    double[] fertiliserN;
    //! Stores the amount of nitrification inhibitor - 1 at time of application
    double[] nitrificationInhibitor;
    //! Stores the amount and composition of crop residues remaining on the surface from the previous crop
    GlobalVars.product residueFromPrevious;
    //! Stores the amount and composition of crop residues remaining on the surface for the following crop
    GlobalVars.product residueToNext;
    //!  Get evaporation on a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for evaporation (mm).
    */
    public double Getevaporation(int index) { return evaporation[index]; }
    //!  Get transpiration for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for transpiration (mm).
    */
    public double Gettranspire(int index) { return transpire[index]; }
    //!  Get irrigation water for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for irrigation water (mm).
    */
    public double GetIrrigationWater(int index) { return irrigationWater[index]; }
    //!  Get irrigationThreshold.
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for irrigationThreshold (mm).
    */
    public double GetirrigationThreshold() { return irrigationThreshold; }
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for irrigationMinimum (mm).
    */
    public double GetirrigationMinimum() { return irrigationMinimum; }
    //!  Set soilWater. Taking two arguments.
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \param val, a double value for val, soilWater.
    */
    public void SetsoilWater(int index, double val) { soilWater[index] = val; }
    //!  Get soilWater. Taking one integer argument and
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for the amount of water in the soil (mm).
    */
    public double GetsoilWater(int index) { return soilWater[index]; }
    //!  Get plantavailableWater for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return a double value for plant-available water (mm).
    */
    public double GetplantavailableWater(int index) { return plantavailableWater[index]; }
    //!  Set plantavailableWater for a given day
    /*!
      \param index, the day for which the value is to be set, as an integer argument.
      \param val, a double value for plantavailableWater (mm).
    */
    public void SetplantavailableWater(int index, double val) { plantavailableWater[index]=val; }
    //!  Set droughtFactorPlant for a given day.
    /*!
      \param index, the day for which the value should be set, as an integer argument.
      \param val, a double value for droughtFactorPlant.
    */
    public void SetdroughtFactorPlant(int index, double val) { droughtFactorPlant[index] = val; }
    //!  Get droughtFactorPlant for a given day.
    /*!
      \param index, an integer argument that points to index.
      \return, a double value for droughtFactorPlant.
    */
    public double GetdroughtFactorPlant(int index) { return droughtFactorPlant[index]; }
    //! Get the drought factors for the soil for the duration of the crop.
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return, a double value for droughtFactorSoil.
    */
    public double[] GetdroughtFactorSoil() { return droughtFactorSoil; }
    //!  Get droughtFactorSoil for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return, a double value for droughtFactorSoil.
    */
    public double GetdroughtFactorSoil(int index) { return droughtFactorSoil[index]; }
    //!  Set droughtFactorSoil for a given day.
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \param val, a double value for droughtFactorSoil.
    */
    public void SetdroughtFactorSoil(int index, double val) { droughtFactorSoil[index] = val; }
    //!  Get dailyNitrateLeaching for a given day.
    /*!
      \param index, an integer argument that points to index.
      \return, a double value for daily nitrate leaching (kg N/ha).
    */
    public double GetdailyNitrateLeaching(int index) { return dailyNitrateLeaching[index]; }
    //!  Get dailyCanopyStorage for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return, a double value for canopy storage (mm).
    */
    public double getdailyCanopyStorage(int index) { return dailyCanopyStorage[index]; }
    //!  Get drainage for a given day
    /*!
      \param index, the day for which the value is required, as an integer argument.
      \return, a double value for drainage (mm).
    */
    public double Getdrainage(int index) { return drainage[index]; }
    //!  Set the crop area 
    /*!
      \param aVal, area (ha) as a double argument.
    */
    public void setArea(double aVal) { area = aVal; }
    //!  Set soilN2Factor
    /*!
      \param aVal, a double argument that sets soilN2Factor.
    */
    public void setsoilN2Factor(double aVal) { soilN2Factor = aVal; }
    //!  Set cropSequenceNo
    /*!
      \param aVal, the number of the cropping sequence as an integer argument.
    */
    public void SetcropSequenceNo(int aVal) { cropSequenceNo = aVal; }
    //!  Set nitrification Inhibitor
    /*!
      \param aVal, a double argument with value 1.0 or less.
    */
    public void SetnitrificationInhibitor(double aVal) { nitrificationInhibitor[0] = aVal; }
    //!  Get MineralN from lastCrop
    /*!
      \return the mineral N remaining from the last crop (kg/ha) as a double value.
    */
    public double getMineralNFromLastCrop(){return mineralNFromLastCrop;}
    //!  Get N fixation. 
    /*!
      \return N fixation (kg/ha) as a double value.
    */
    public double getNFix() { return Nfixed; }
    //!  Get atmospheric N deposition 
    /*!
      \return N deposition (kg/ha) as a double value.
    */
    public double getnAtm() { return nAtm; }
    //!  Get the name of the crop. 
    /*!
      \return crop name as a string value.
    */
    public string Getname() { return name; }
    //!  Get identity. 
    /*!
      \return identity as an integer value.
    */
    public int Getidentity() { return identity; }
    //!  Get Area. 
    /*!
      \return crop area (ha) as a double value.
    */
    public double getArea() { return area; }
    //!  Set the length of cropping sequence.
    /*!
      \param aVal, sequence length as an double argument.
    */
    public void SetlengthOfSequence(double aVal) { lengthOfSequence = aVal; }
    //!  Get proportion of below ground residues. 
    /*!
      \return  proportion of below ground residues as a double value.
    */
    public double GetpropBelowGroundResidues() { return propBelowGroundResidues; }
    //!  Get concentration of C in below ground residues. 
    /*!
      \return concentration of C in below ground residues (kg C/kg DM) as a double value.
    */
    public double GetCconcBelowGroundResidues() { return CconcBelowGroundResidues; }
    //!  Get storage processing C loss. 
    /*!
      \return storage processing C loss (kg/ha) a double value.
    */
    public double GetstorageProcessingCLoss() { return storageProcessingCLoss; }
    //!  Get storage processing N loss. 
    /*!
      \return storage processing N loss (kg/ha) a double value.
    */
    public double GetstorageProcessingNLoss() { return storageProcessingNLoss; }
    //!  Get havested C. 
    /*!
      \return harvested C (kg/ha) a double value.
    */
    public double GetharvestedC() { return harvestedC; }
    //!  Get havested dry matter. 
    /*!
      \return havested dry matter (kg/ha)a double value.
    */
    public double GetharvestedDM() { return harvestedDM; }
    //!  Get grazed C. 
    /*!
      \return grazed C (kg/ha) as a double value.
    */
    public double GetgrazedC() { return grazedC; }
    //!  Get Surface Residue C. 
    /*!
      \return Surface Residue C (kg/ha) as a double value.
    */
    public double GetsurfaceResidueC() { return surfaceResidueC; }
    //!  Get subsurface Residue C. 
    /*!
      \return subsurface Residue C (kg/ha) as a double value.
    */
    public double GetsubsurfaceResidueC() { return subsurfaceResidueC; }
    //!  Get Surface Residue N. 
    /*!
      \return Surface Residue N (kg/ha) as a double value.
    */
    public double GetsurfaceResidueN() { return surfaceResidueN; }
    //!  Get subsurface Residue N. 
    /*!
      \return subsurface Residue N (kg/ha) as a double value.
    */
    public double GetsubsurfaceResidueN() { return subsurfaceResidueN; }
    //!  Get Urine NH3 Emission. 
    /*!
      \return Urine NH3 Emission (kg/ha) as a double value.
    */
    public double GeturineNH3emission() { return urineNH3emission; }
    //!  Get Fertiliser C emission. 
    /*!
      \return Fertiliser C emission (kg/ha) as a double value.
    */
    public double GetFertiliserC() { return fertiliserC; }
    //!  Get nitrification inhibitor. 
    /*!
      \return  nitrification inhibitor as a double value.
    */
    public double GetnitrificationInhibitor()  { return nitrificationInhibitor[duration-1]; }
    //!  Get residue C remaining for to next crop. 
    /*!
      \return residue C remaining for to next crop (kg/ha) as a double value.
    */
    public double GetResidueCtoNextCrop() { return residueCtoNextCrop; }
    //!  Get residue N remaining for to next crop. 
    /*!
      \return residue N remaining for to next crop (kg/ha) as a double value.
    */
    public double GetResidueNtoNextCrop() { return residueNtoNextCrop; }
    //!  Get unutilised grazable C. 
    /*!
      \return unutilised grazable C (kg/ha) as a double value.
    */
    public double GetUnutilisedGrazableC() { return unutilisedGrazableC; }
    //!  Get unutilised grazable dry matter. 
    /*!
      \return unutilised grazable DM (kg/ha) as a double value.
    */
    public double GetUnutilisedGrazableDM() { return unutilisedGrazableDM; }
    //!  Get utilised dry matter. 
    /*!
      \return utilised dry matter (kg/ha) as a double value.
    */
    public double GetUtilisedDM()
    {
        double retVal = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed())
                retVal+=theProducts[i].GetGrazed_yield();
            else if (theProducts[i].Harvested=="Harvested")
                retVal += theProducts[i].GetModelled_yield();
        }
        return retVal;
    }
    //!  Get manure C. 
    /*!
      \return manure C (kg/ha) as a double value.
    */
    public double GetManureC() 
    {
        double retVal = 0;
        int numDays = manureHUMCsurface.Length;
        for (int i = 0; i < numDays; i++)
            retVal += manureHUMCsurface[i] + manureFOMCsurface[i];
        return retVal;
    }
    //!  Get Urine N. 
    /*!
      \return Urine N (kg/ha) a double value.
    */
    public double GetUrineN() { return urineNasFertilizer; }
    //!  Get Urine C. 
    /*!
      \return Urine C (kg/ha) a double value.
    */
    public double GetUrineC() { return urineC; }
    //!  Get Grazing CH4C. 
    /*!
      \return Grazing CH4-C (kg/ha) as a double value.
    */
    public double GetgrazingCH4C() { return grazingCH4C; }
    //!  Get faecal C. 
    /*!
      \return faecal C a double value.
    */
    public double GetfaecalC() { return faecalC; }
    //!  Get Faecal N. 
    /*!
      \return (kg/ha) as a double value.
    */
    public double GetfaecalN() { return faecalN; }
    //!  Get Total Manure N Applied. 
    /*!
      \return Total Manure N Applied (kg/ha) as a double value.
    */
    public double GettotalManureNApplied() { return totalManureNApplied; }
    //!  Get Excreta N Input. 
    /*!
      \return Excreta N Input (kg/ha) as a double value.
    */
    public double GetexcretaNInput() { return excretaNInput; }
    //!  Get Excreta C Input. 
    /*!
      \return Excreta C Input (kg/ha) as a double value.
    */
    public double GetexcretaCInput() { return excretaCInput; }
    //!  Get Manure NH3-N Emission. 
    /*!
      \return Manure NH3-N Emission (kg/ha) as a double value.
    */
    public double GetmanureNH3Nemission() { return manureNH3emission; }
    //!  Get N2O-N Emission. 
    /*!
      \return N2O-N Emission (kg/ha) as a double value.
    */
    public double GetN2ONemission() { return N2ONemission; }
    //!  Get N2-N Emission. 
    /*!
      \return N2-N Emission (kg/ha) as a double value.
    */
    public double GetN2Nemission() { return N2Nemission; }
    //!  Get Fertiliser NH3-N Emission. 
    /*!
      \return Fertiliser NH3-N Emission (kg/ha) as a double value.
    */
    public double GetfertiliserNH3Nemission() { return fertiliserNH3emission; }
    //!  Get Mineral N available. 
    /*!
      \return Mineral N available (kg/ha) as a double value.
    */
    public double GetmineralNavailable() { return mineralNavailable; }
    //!  Get Fertiliser N2O-N Emission. 
    /*!
      \return Fertiliser N2O-N Emission (kg/ha) as a double value.
    */
    public double GetfertiliserN2ONEmission() { return fertiliserN2OEmission; }
    //!  Get Manure N2O-N Emission. 
    /*!
      \return Manure N2O-N Emission (kg/ha) as a double value.
    */
    public double GetmanureN2ONEmission() { return manureN2OEmission; }
    //!  Get Soil N2O-N Emission. 
    /*!
      \return Soil N2O-N Emission (kg/ha) as a double value.
    */
    public double GetsoilN2ONEmission() { return soilN2OEmission; }
    //!  Get burning N2O-N. 
    /*!
      \return burning N2O-N (kg/ha) as a double value.
    */
    public double GetburningN2ON() { return burningN2ON; }
    //!  Get burning NH3-N. 
    /*!
      \return burning NH3-N (kg/ha) as a double value.
    */
    public double GetburningNH3N() { return burningNH3N; }
    //!  Get burning NOxN. 
    /*!
      \return burning NOx-N (kg/ha) as  a double value.
    */
    public double GetburningNOxN() { return burningNOxN; }
    //!  Get burning Other N. 
    /*!
      \return burning Other N (kg/ha) as  a double value.
    */
    public double GetburningOtherN() { return burningOtherN; }
    //!  Get burning CO-C. 
    /*!
      \return burning CO-C (kg/ha) as  a double value.
    */
    public double GetburningCOC() { return burningCOC; }
    //!  Get burning CO2-C. 
    /*!
      \return burning CO2-C (kg/ha) as  a double value.
    */
    public double GetburningCO2C() { return burningCO2C; }
    //!  Get burning black C. 
    /*!
      \return black C (kg/ha) as  a double value.
    */
    public double GetburningBlackC() { return burningBlackC; }
    //!  Get burnt residue C. 
    /*!
      \return burnt residue C (kg/ha) as  a double value.
    */
    public double GetburntResidueC() { return burntResidueC; }
    //!  Get burnt Residue N. 
    /*!
      \return burnt residue N (kg/ha) as  a double value.
    */
    public double GetburntResidueN() { return burntResidueN; }
    //!  Get Harvested N. 
    /*!
      \return Harvested N (kg/ha) as  a double value.
    */
    public double GetharvestedN() { return harvestedN; }
    //!  Get Mineral N to NextCrop. 
    /*!
      \return Mineral N to Next Crop (kg/ha) as  a double value.
    */
    public double GetmineralNToNextCrop() { return mineralNToNextCrop; }
    //!  Get Crop N uptake. 
    /*!
      \return Crop N uptake (kg/ha) as  a double value.
    */
    public double GetCropNuptake() { return cropNuptake; }
    //!  Get the Products. 
    /*!
      \return a list of the crop products.
    */
    public List<GlobalVars.product> GettheProducts() { return theProducts; }
    //!  Get Fertiliser Applied. 
    /*!
      \return a list of the fertilisation events.
    */
    public List<fertRecord> GetfertiliserApplied() { return fertiliserApplied; }
    //!  Get Manure Applied. 
    /*!
      \return a list of the manure applications.
    */
    public List<fertRecord> GetmanureApplied() { return manureApplied; }
    //!  Get Start Day. 
    /*!
      \return the starting day of the crop as an integer value.
    */
    public int GetStartDay() { return theStartDate.GetDay(); }
    //!  Get End Day. 
    /*!
      \return the ending day of the crop as an integer value.
    */
    public int GetEndDay() { return theEndDate.GetDay(); }
    //!  Get Start Month. 
    /*!
      \return the starting month of the crop as an integer value.
    */
    public int GetStartMonth() { return theStartDate.GetMonth(); }
    //!  Get End Month. 
    /*!
      \return athe ending month of the crop as an integer value.
    */
    public int GetEndMonth() { return theEndDate.GetMonth(); }
    //!  Get End Year. 
    /*!
      \return the starting year of the crop as an integer value.
    */
    public int GetEndYear() { return theEndDate.GetYear(); }
    //!  Get Start Year. 
    /*!
      \return the ending year of the crop as an integer value.
    */
    public int GetStartYear() { return theStartDate.GetYear(); }
    //!  Set Start Year
    /*!
      \param aVal, the start year as an integer argument.
    */
    public void SetStartYear(int aVal) { theStartDate.SetYear(aVal); }
    //!  Set End Year.
    /*!
      \param aVal, the end year an integer argument.
    */
    public void SetEndYear(int aVal) { theEndDate.SetYear(aVal); }
    //!  Get Permanent. 
    /*!
      \return true if the crop is permanent (not to be sown).
    */
    public bool Getpermanent() {return permanent;}
    //!  Get Maximum Rooting Depth. 
    /*!
      \return maximum rooting depth (m) as  a double value.
    */
    public double GetMaximumRootingDepth() { return MaximumRootingDepth; }
    //!  Set Maximum Rooting Depth. 
    /*!
      \param aVal, the maximum rooting depth (m) as a double argument.
    */
    public void SetMaximumRootingDepth(double aVal) { MaximumRootingDepth = aVal; }
    //!  Get Manure FOM C surface.  
    /*!
     \param day, day in crop life for which the value is required, as an integer argument.
      \return manure FOM C applied on the surface (kg/ha) as  a double value.
    */
    public double GetmanureFOMCsurface(int day) { return manureFOMCsurface[day]; }
    //!  Get Manure FOM N surface.  
    /*!
     \param day, day in crop life for which the value is required, as an integer argument.
      \return manure FOM N applied on the surface (kg/ha) as  a double value.
    */
    public double GetmanureFOMNsurface(int day) { return manureFOMNsurface[day]; }
    //!  Get Manure HUM C surface.  
    /*!
     \param day, day in crop life for which the value is required, as an integer argument.
      \return Manure HUM C on surface (kg/ha) as  a double value.
    */
    public double GetmanureHUMCsurface(int day) { return manureHUMCsurface[day]; }
    //!  Get Manure Biochar C surface.  
    /*!
     \param day, day in crop life for which the value is required, as an integer argument.
      \return Manure HUM N surface (kg/ha) as  a double value.
    */
    public double GetmanureBiocharCsurface(int day) { return manureBiocharCsurface[day]; }
    //!  Get Duration of the crop. 
    /*!
      \return duration in days as a long value.
    */
    public long getDuration() { return duration; }
    //!  Get Organic N Leached. 
    /*!
      \return Organic N Leached (kg/ha) as  a double value.
    */
    public double GetOrganicNLeached() {return OrganicNLeached; }
    //!  Get Soil N Mineralisation. 
    /*!
      \return Soil N Mineralisation (kg/ha) as  a double value.
    */
    public double GetSoilNMineralisation() { return soilNMineralisation; }
    //!  Get Nitrate Leaching. 
    /*!
      \return  Nitrate Leaching (kg N/ha) as  a double value.
    */
    public double GetnitrateLeaching() { return nitrateLeaching; }
    //!  Get Total Manure HUM N surface. 
    /*!
      \return Total Manure HUM N on surface (kg/ha) as a double value.
    */
    public double GetTotalmanureHUMNsurface()
    {
        double retVal = 0;
        for (int i = 0; i < manureHUMNsurface.Length; i++)
            retVal += manureHUMNsurface[i];
        return retVal;
    }
    //!  Get Manure HUM N on surface.
    /*!
      \param j, day in crop life for which the value is required as an integer argument.
      \return Manure HUM N (kg/ha) pon day number j as a double value.
    */
    public double GetmanureHUMNsurface(int j)
    {
        double retVal = manureHUMNsurface[j];
        return retVal;
    }
    //!  Get Residue that will be passed to the next crop. 
    /*!
      \return the residue as a GlobalVar.product.
    */
    public GlobalVars.product GetresidueToNext() { return residueToNext; }
    //!  Get Total Manure FOM N on the surface. 
    /*!
      \return Total Manure FOM N (kg/ha) as  a double value.
    */
    public double GetTotalmanureFOMNsurface()
    {
        double retVal = 0;
        for (int i = 0; i < manureFOMNsurface.Length; i++)
            retVal += manureFOMNsurface[i];
        return retVal;
    }
    //!  Get Fertiliser N Applied. 
    /*!
      \return Fertiliser N Applied (kg/ha) as  a double value.
    */
    public double GetFertiliserNapplied()
    {
        double retVal = 0;
        for (int i = 0; i < fertiliserApplied.Count; i++)
            retVal += fertiliserApplied[i].getNamount();
        return retVal;
    }
    //!  Get Manure N Applied. 
    /*!
      \return Manure N Applied (kg/ha) as a double value.
    */
    public double GetManureNapplied()
    {
        double retVal = 0;
        for (int i = 0; i < manureApplied.Count; i++)
            retVal += manureApplied[i].getNamount();
        return retVal;
    }
    //!  Get Residue N Input. 
    /*!
      \return Residue N Input (kg/ha) as  a double value.
    */
    public double GetresidueNinput()
    {
        double NinputInResidues = surfaceResidueN + subsurfaceResidueN;
        return NinputInResidues;
    }
    //!  Sets evaporation on a particular day 
    /*!
     \param index, the target day an integer argument.
      \param val, the evaporation (mm) as a double argument.
    */
    public void Setevaporation(int index, double val)
    {
        evaporation[index] = val;
    }
    //!  Set Drainage on a particular day. 
    /*!
     \param index, the target day an integer argument.
      \param val, the drainage (mm) as a double argument.
    */
    public void Setdrainage(int index, double val)
    {
        drainage[index] = val;
    }
    //!  Set Canopy Storage on a particular day. 
    /*!
     \param index, the target day an integer argument.
      \param val, the Canopy Storage (mm) as a double argument.
    */
    public void SetcanopyStorage(int index, double val)
    {
        dailyCanopyStorage[index] = val;
    }
    //!  Set Transpiration on a particular day. 
    /*!
     \param index, the target day an integer argument.
      \param val, the Transpiration (mm) as a double argument.
    */
    public void Settranspire(int index, double val)
    {
        transpire[index] = val;
    }
    //!  Set Organic N Leached.
    /*!
      \param aVal, Organic N Leached (kg/ha) as a double argument.
    */
    public void SetOrganicNLeached(double aVal) { OrganicNLeached = aVal; }
    //!  Set Soil N Mineralisation.
    /*!
      \param aVal, Soil N Mineralisation (kg/ha) as a double argument.
    */
    public void SetsoilNMineralisation(double aVal) { soilNMineralisation = aVal; }
    //! A default constructor.
    private CropClass(){}
    //!  Get Start Long Time. 
    /*!
      \return the starting date of the crop as a long value.
    */
    public long getStartLongTime() { return theStartDate.getLongTime(); }
    //!  Get End Long Time. 
    /*!
      \return the end date of the crop asa long value.
    */
    public long getEndLongTime(){return theEndDate.getLongTime();}
    //!  Get C Fixed by the crop. 
    /*!
      \return C Fixed (kg/ha) as a double value.
    */
    public double getCFixed(){return CFixed;}
    //! A constructor with five arguments.
    /*!
      \param path, path for the input file as a string arguemnt.
      \param index, number of the crop in the crop sequence as an integer argument.
      \param zoneNr, agroecological zone number an integer argument.
      \param theCropSeqName, name of the crop sequence as a string arugment.
      \param theArea, the area of the crop (ha) a double argument.
    */
    public CropClass(string path, int index, int zoneNr, string theCropSeqName, double theArea)
    {
        fertiliserApplied = new List<fertRecord>();
        manureApplied = new List<fertRecord>();
        theStartDate = new timeClass();
        theEndDate = new timeClass();
        area = theArea;
        cropSequenceName = theCropSeqName;
        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        cropInformation.setPath(path+"("+index+")");
        name = cropInformation.getItemString("NameOfCrop");
        int Start_day = cropInformation.getItemInt("Start_day");
        int Start_month = cropInformation.getItemInt("Start_month");
        int Start_year = cropInformation.getItemInt("Start_year");
        if (!theStartDate.setDate(Start_day, Start_month, Start_year))
        {
            GlobalVars.Instance.Error("Incorrect date for start date for " + name);
        }
        int End_day = cropInformation.getItemInt("End_day");
        int End_month = cropInformation.getItemInt("End_month");
        int End_year = cropInformation.getItemInt("End_year");
        if (!theEndDate.setDate(End_day, End_month, End_year))
        {
            GlobalVars.Instance.Error("Incorrect date for end date for " + name);
        }
        duration = theEndDate.getLongTime() - theStartDate.getLongTime();
        if (duration <= 0)
        {
            string outputString = "negative duration in crop sequence name " + theCropSeqName + " crop name " + name;
            GlobalVars.Instance.Error(outputString);
        }
        drainage = new double[duration];
        evaporation = new double[duration];
        transpire = new double[duration];
        LAI = new double[duration];
        soilWater = new double[duration];
        plantavailableWater = new double[duration];
        droughtFactorPlant = new double[duration];
        droughtFactorSoil = new double[duration];
        dailyNitrateLeaching = new double[duration];
        dailyCanopyStorage = new double[duration];
        irrigationWater = new double[duration];
        fertiliserN = new double[duration];
        nitrificationInhibitor = new double[duration];
        manureFOMCsurface = new double[duration];
        manureHUMCsurface = new double[duration];
        manureBiocharCsurface = new double[duration];
        manureFOMNsurface = new double[duration];
        manureHUMNsurface = new double[duration];
        manureTAN = new double[duration];
        string tempString = cropInformation.getItemString("Irrigation");
        if (tempString == "false")
            isIrrigated = false;
        else
            isIrrigated = true;
        if (name != "Bare soil")
            GetCropInformation(path, index, zoneNr);
        int length = name.IndexOf("Permanent");
        if (length != -1)
        {
            permanent = true;
            droughtSusceptability = 0.5;
        }
        // get the synthetic fertiliser N applied
        int minFertiliserApplied = 99, maxFertiliserApplied = 0;
        cropInformation.PathNames.Add("Fertilizer_applied");
        cropInformation.getSectionNumber(ref minFertiliserApplied, ref maxFertiliserApplied);
        for (int i = minFertiliserApplied; i <= maxFertiliserApplied; i++)
        {
            if (cropInformation.doesIDExist(i))
            {
                fertRecord newFertRecord = new fertRecord();
                newFertRecord.applicdate = new timeClass();
                cropInformation.Identity.Add(i);
                newFertRecord.Name = cropInformation.getItemString("Name");
                newFertRecord.Units = cropInformation.getItemString("Unit");
                if (newFertRecord.Units.CompareTo("kg N/ha") == 0)
                    newFertRecord.Namount = cropInformation.getItemDouble("Value");
                if (newFertRecord.Units.CompareTo("t/ha") == 0)
                    newFertRecord.Camount = cropInformation.getItemDouble("Value");
                newFertRecord.Month_applied = cropInformation.getItemInt("Month_applied");
                if (newFertRecord.ReadFertManApplication(cropInformation, theStartDate, theEndDate) == false)
                {
                    string messageString = ("Error - fertiliser applied outside the period of this crop\n");
                    messageString = messageString + ("Crop sequence name = " + cropSequenceName + "\n");
                    messageString = messageString + ("Crop sequence number = " + cropSequenceNo + "\n");
                    messageString = messageString + ("Crop name = " + name);
                    GlobalVars.Instance.Error(messageString);
                }
                cropInformation.PathNames.Add("Applic_technique_Fertilizer");
                cropInformation.Identity.Add(-1);
                newFertRecord.Applic_techniqueS = cropInformation.getItemString("NameS");
                newFertRecord.Applic_techniqueI = cropInformation.getItemInt("NameI");
                fertiliserApplied.Add(newFertRecord);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }
        // get the manure applied
        int minManure_applied = 99, maxManure_applied = 0;
        cropInformation.PathNames[cropInformation.PathNames.Count - 1] = "Manure_applied";
        cropInformation.getSectionNumber(ref minManure_applied, ref maxManure_applied);
        //Check to see if manure is applied outside crop period. Why does this code not use ReadFertManApplication? NJH
        for (int i = minManure_applied; i <= maxManure_applied; i++)
        {
            if (cropInformation.doesIDExist(i))
            {
                fertRecord newFertRecord = new fertRecord();
                newFertRecord.applicdate = new timeClass();
                cropInformation.Identity.Add(i);
                newFertRecord.Name = cropInformation.getItemString("Name");
                newFertRecord.Units = cropInformation.getItemString("Unit");
                newFertRecord.Namount = cropInformation.getItemDouble("Value");
                newFertRecord.Month_applied = cropInformation.getItemInt("Month_applied");
                //only have month of application, so need to set a sensible day in month
                if (newFertRecord.ReadFertManApplication(cropInformation, theStartDate, theEndDate) == false)
                {
                    string messageString = ("Error - manure applied outside the period of this crop\n");
                    messageString = messageString + ("Crop sequence name = " + cropSequenceName + "\n");
                    messageString = messageString + ("Crop sequence number = " + cropSequenceNo + "\n");
                    messageString = messageString + ("Crop name = " + name);
                    GlobalVars.Instance.Error(messageString);
                }
                newFertRecord.ManureStorageID = GlobalVars.Instance.getManureStorageID(cropInformation.getItemInt("StorageType"));
                newFertRecord.speciesGroup = cropInformation.getItemInt("SpeciesGroup");
                cropInformation.PathNames.Add("Applic_technique_Manure");
                cropInformation.Identity.Add(-1);
                newFertRecord.Applic_techniqueS = cropInformation.getItemString("NameS");
                newFertRecord.Applic_techniqueI = cropInformation.getItemInt("NameI");
                manureApplied.Add(newFertRecord);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }
        totalTsum = GlobalVars.Instance.theZoneData.GetPeriodTemperatureSum(Start_day, Start_month, Start_year, End_day, End_month, End_year, baseTemperature);
        if (totalTsum <= 0)
        {
            string messageString = ("Error - total Tsum is zero or less\n");
            messageString = messageString + ("Crop sequence name = " + cropSequenceName + "\n");
            messageString = messageString + ("Crop sequence number = " + cropSequenceNo + "\n");
            messageString = messageString + ("Crop name = " + name);
            GlobalVars.Instance.Error(messageString);
        }
        // get the crop parameters
        getParameters(index, zoneNr, path);
    }
    //! Set the string that contains information about the farm and scenario number.
    public void Updateparens(string aParent, int ID)
    {
        identity = ID;
        parens = aParent;
        for (int i = 0; i < fertiliserApplied.Count; i++)
        {
            fertiliserApplied[i].setParens( parens + "_" + i.ToString());
        }
        for (int i = 0; i < manureApplied.Count; i++)
        {
            manureApplied[i].setParens(parens + "_" + i.ToString());
        }
    }

    //public void GetBareSoilResidues(string path, int index, int zoneNr)
    //{
    //    FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
    //    cropInformation.setPath(path + "(" + index + ").Product(-1)");
    //    cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
    //    int minProduct = 99, maxProduct = 0;
    //    cropInformation.getSectionNumber(ref minProduct, ref maxProduct);
    //    double isGrazedProduction = 0;
    //    for (int i = minProduct; i <= maxProduct; i++)
    //    {
    //        if (cropInformation.doesIDExist(i) == true)
    //        {
    //            cropInformation.Identity.Add(i);
    //            string cropPath = path + "(" + index + ")" + ".Product";
    //            GlobalVars.product anExample = new GlobalVars.product();
    //            feedItem aComposition = new feedItem();
    //            anExample.composition = aComposition;
    //            anExample.Harvested = cropInformation.getItemString("Harvested");
    //            string temp = path + "(" + index + ")" + ".Product" + "(" + i.ToString() + ").Expected_yield(-1)";
    //            anExample.Expected_yield = cropInformation.getItemDouble("Value", temp);
    //            if (anExample.composition.GetisGrazed() == true)
    //            {
    //                anExample.Grazed_yield = anExample.Expected_yield;
    //                isGrazedProduction += anExample.Expected_yield;
    //            }
    //            if (anExample.Expected_yield > 0)
    //                theProducts.Add(anExample);
    //            else
    //            {
    //                string messageString = ("Error - grazed yield of grazed residue crop must be greater than zero \n");
    //                messageString = messageString + ("Crop sequence name = " + cropSequenceName + "\n");
    //                messageString = messageString + ("Crop sequence number = " + cropSequenceNo + "\n");
    //                messageString = messageString + ("Crop name = " + name);
    //                GlobalVars.Instance.Error(messageString);
    //            }
    //            cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
    //            cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
    //            cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
    //        }
    //    }
    //}
    //public void AddProductsWithResidue(List<GlobalVars.product> list)
    //{
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        theProducts.Add(list[i]);
    //    }
    //}
    //! Get the crop management parameters
    /*!
      \param path, path for the input file as a string argument.
      \param index, number of the crop in the crop sequence as an integer argument.
      \param zoneNr, agroecological zone number an integer argument.
    */
    public void GetCropInformation(string path, int index, int zoneNr)
    {
        GlobalVars.Instance.log("Entering GetCropInformation " + " crop " + name.ToString(), 6);

        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        cropInformation.setPath(path + "(" + index + ")");
        cropInformation.Identity.Add(-1);
        cropInformation.PathNames.Add("HarvestMethod");
        harvestMethod = cropInformation.getItemString("Value");
        cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
        cropInformation.PathNames[cropInformation.PathNames.Count - 1] = "Product";
        int minProduct = 99, maxProduct = 0;
        cropInformation.getSectionNumber(ref minProduct, ref maxProduct);
        double totProduction = 0;
        double isGrazedProduction = 0;
        for (int i = minProduct; i <= maxProduct; i++)
        {
            if(cropInformation.doesIDExist(i)==true)
            {
                cropInformation.Identity.Add(i);
                string cropPath = path + "(" + index + ")" + ".Product";
                GlobalVars.product anExample = new GlobalVars.product();
                feedItem aComposition = new feedItem(cropPath, i, false,parens+"_"+i.ToString());
                anExample.composition = aComposition;
                anExample.Harvested = cropInformation.getItemString("Harvested");
                string temp = path + "(" + index + ")" + ".Product" + "(" + i.ToString() + ").Potential_yield(-1)";
                anExample.Potential_yield = cropInformation.getItemDouble("Value", temp);
                if (anExample.Potential_yield<=0)
                {
                    string messageString = ("Error - potential production of a crop product cannot be zero or less than zero\n");
                    messageString += ("Crop source = " + path + "\n");
                    messageString += ("Crop name = " + name);
                    GlobalVars.Instance.Error(messageString);
                }
                temp = path + "(" + index + ")" + ".Product" + "(" + i.ToString() + ").Expected_yield(-1)";
                anExample.Expected_yield = cropInformation.getItemDouble("Value", temp);
                totProduction += anExample.Potential_yield;
                if (anExample.composition.GetisGrazed() == true || anExample.Harvested.Contains("Residue"))
                {
                    anExample.Grazed_yield = anExample.Expected_yield;
                    isGrazedProduction += anExample.Expected_yield;
                }
                else
                    if (!anExample.Harvested.Contains("Incorporated"))
                        anExample.composition.AdjustAmount(1+anExample.composition.GetStoreProcessFactor());
                if (anExample.Potential_yield >0)
                    theProducts.Add(anExample);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }

        if ((totProduction== 0)&&(name!="Bare soil"))
        {
            string messageString=("Error - total potential production of a crop cannot be zero\n");
            messageString+=("Crop source = " + path+"\n");
            messageString += ("Crop name = " + name);
            GlobalVars.Instance.Error(messageString);
        }
    }
    //!  Get crop growth parameters
    /*!
     \param cropIdentityNo, an integer argument.
      \param zoneNR, agroecological zone an integer argument.
      \param cropPath, file pather for the parameter file, as a string argument.
    */
    public void getParameters(int cropIdentityNo, int zoneNR, string cropPath)
    {
        identity = cropIdentityNo;
        FileInformation cropParamFile = new FileInformation(GlobalVars.Instance.getParamFilePath());  //prob here
        string basePath = "AgroecologicalZone(" + zoneNR + ")";
        string tmpPath;
        NDepositionRate = GlobalVars.Instance.theZoneData.GetNdeposition();
        tmpPath = basePath + ".UrineNH3EF(-1)";
        cropParamFile.setPath(tmpPath);
        if (zeroGasEmissionsDebugging)
            urineNH3EmissionFactor = 0;
        else
            urineNH3EmissionFactor = cropParamFile.getItemDouble("Value");
        cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
        if (name != "Bare soil")
        {
            cropParamFile.PathNames[cropParamFile.PathNames.Count - 1] = "Crop";
            int min = 99, max = 0;
            cropParamFile.getSectionNumber(ref min, ref max);
            bool gotit = false;
            string aname ="None";
            for (int j = min; j <= max; j++)
            {
                if (cropParamFile.doesIDExist(j))
                {
                    tmpPath = basePath + ".Crop" + "(" + j.ToString() + ")";
                    cropParamFile.setPath(tmpPath);
                    aname = cropParamFile.getItemString("Name");         
                    if (aname == name)  //Find crop by its name
                    {
                        gotit = true;
                        break;
                    }
                    cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
                }
            }
            if (gotit == false)
            {
                string message1=("Error - could not find crop in parameter file\n");
                message1 += message1 + ("Crop source = " + cropPath + "(" + cropIdentityNo.ToString() + ")\n");
                message1+=("Crop name = " + name);
                GlobalVars.Instance.Error(message1);
            }
            
            cropParamFile.setPath(tmpPath + ".MaxLAI(-1)");
            double value = cropParamFile.getItemDouble("Value", false);
            if (value!=0)
                maxLAI = value;
            cropParamFile.setPath(tmpPath + ".NavailMax(-1)");
            cropParamFile.setPath(tmpPath + ".NfixationFactor(-1)");
            NfixationFactor = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".PropBelowGroundResidues(-1)");
            propBelowGroundResidues = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".BelowGroundCconc(-1)");
            CconcBelowGroundResidues = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".BelowGroundCtoN(-1)");
            CtoNBelowGroundResidues = cropParamFile.getItemDouble("Value");

            cropParamFile.setPath(tmpPath + ".MaximumRootingDepth(-1)");
            MaximumRootingDepth = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".Irrigation(-1).irrigationThreshold(-1)");
            irrigationThreshold= cropParamFile.getItemDouble("Value", false);
            cropParamFile.setPath(tmpPath + ".Irrigation(-1).irrigationMinimum(-1)");
            irrigationMinimum = cropParamFile.getItemDouble("Value", false);
            cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
            cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
            cropParamFile.PathNames.RemoveAt(cropParamFile.PathNames.Count - 1);
            cropParamFile.PathNames[cropParamFile.PathNames.Count - 1] = "HarvestMethod";
            //find the parameters for the harvesting method
            max = 0;
            min = 999;
            cropParamFile.getSectionNumber(ref min, ref max);
            cropParamFile.Identity.Add(-1);
            for (int i = min; i <= max; i++)
            {
                cropParamFile.Identity[cropParamFile.PathNames.Count - 1] = i;
                string harvestMethodName = cropParamFile.getItemString("Name");
                if (harvestMethodName == harvestMethod)
                {
                    cropParamFile.PathNames.Add("PropAboveGroundResidues");
                    cropParamFile.Identity.Add(-1);
                    if (harvestMethodName == "Grazing")
                        propAboveGroundResidues[1] = cropParamFile.getItemDouble("Value");
                    else
                        propAboveGroundResidues[0] = cropParamFile.getItemDouble("Value");
                    break;
                }
            }
        }
    }
    //!  Calculate Water limited yield.
    /*!
     \param droughtIndex, a double argument.
    */
    public void Calcwaterlimited_yield(double droughtIndex)
    {
        if (Getname() != "Bare soil")
        {
            for (int k = 0; k < theProducts.Count; k++)
            {
                if (isIrrigated)
                {
                    NyieldMax += theProducts[k].composition.GetN_conc() * theProducts[k].Potential_yield;
                    theProducts[k].SetwaterLimited_yield(theProducts[k].Potential_yield);
                }
                else
                {
                    NyieldMax += theProducts[k].composition.GetN_conc() * theProducts[k].Potential_yield * (1 - droughtSusceptability * droughtIndex);
                    theProducts[k].SetwaterLimited_yield(theProducts[k].Potential_yield * (1 - droughtSusceptability * droughtIndex));
                }
                theProducts[k].SetExpectedYield(theProducts[k].GetwaterLimited_yield());
            }
            maxCropNuptake = CalculateCropNUptake();
        }
    }

    //!  Calculate Crop N Uptake. 
    /*!
     \return Crop N Uptake (kg/ha) as  a double value.
    */
    public double CalculateCropNUptake()
    {
        double uptakeN = 0;
        double NinProduct = 0;
        double NinSurfaceResidues = 0;
        double NinSubsurfaceResidues = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed()) //grazed crop
            {
                NinProduct += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[1];
            }
            else
            {
                //ungrazed part of crop
                NinProduct += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[0];
            }
            double adjustment = 1.0;
            NinSubsurfaceResidues += theProducts[i].GetExpectedYield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues() * adjustment) / CtoNBelowGroundResidues;
        }
        uptakeN += NinProduct + NinSurfaceResidues + NinSubsurfaceResidues;
        return uptakeN;
    }
    //!  Calculate Max Crop N Uptake. 
    /*!
     \return Max Crop N Uptake (kg/ha) as  a double value.
    */
    public double CalculateMaxCropNUptake()
    {
        double uptakeN = 0;
        double NinProduct = 0;
        double NinSurfaceResidues = 0;
        double NinSubsurfaceResidues = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed()) //grazed crop
            {
                NinProduct += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[1];
                NinSubsurfaceResidues += theProducts[i].GetPotential_yield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues()) / CtoNBelowGroundResidues;
            }
            else
            {
                //ungrazed part of crop
                NinProduct += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[0];
                NinSubsurfaceResidues += theProducts[i].GetPotential_yield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues()) / CtoNBelowGroundResidues;
            }
        }
        uptakeN += NinProduct + NinSurfaceResidues + NinSubsurfaceResidues;
        return uptakeN;
    }
    //!  Calculate the Harvested Yields.
    public void CalculateHarvestedYields()
    {
        if (residueFromPrevious == null)
        {
            harvestedC = 0;
            harvestedN = 0;
            harvestedDM = 0;
            grazedC = 0;
            grazedN = 0;
        }
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed()) //grazed crop
            {
                if (GlobalVars.Instance.GetstrictGrazing())
                {
                    double grazedProductC = theProducts[i].Grazed_yield * theProducts[i].composition.GetC_conc();
                    harvestedDM = theProducts[i].Grazed_yield;
                    harvestedC += grazedProductC;
                    grazedC += grazedProductC;
                    harvestedN += theProducts[i].Grazed_yield * theProducts[i].composition.GetN_conc();
                    grazedN += theProducts[i].Grazed_yield * theProducts[i].composition.GetN_conc();
                }
                else
                {
                    if (theProducts[i].GetExpectedYield() > theProducts[i].GetGrazed_yield() && theProducts[i].GetGrazed_yield()!=0)
                    {
                        harvestedDM += theProducts[i].GetGrazed_yield();
                        harvestedC += theProducts[i].GetGrazed_yield() * theProducts[i].composition.GetC_conc();
                        harvestedN += theProducts[i].GetGrazed_yield() * theProducts[i].composition.GetN_conc();
                        grazedN += theProducts[i].GetGrazed_yield() * theProducts[i].composition.GetN_conc();
                    }
                    else
                    {
                        harvestedDM += theProducts[i].GetExpectedYield();
                        harvestedC += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc();
                        harvestedN += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                    }
                }
            }
            else
            {
                if ((theProducts[i].Harvested == "Harvested") || (theProducts[i].Harvested == "Burnt stubble"))
                {
                    harvestedDM += theProducts[i].GetExpectedYield();
                    harvestedC += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc();
                    harvestedN += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                }
            }
        }
    }
    //!  Calculate C Fixed.
    public void CalculateCFixed()
    {
        CFixed = 0;
        if (Getname() != "Bare soil")
        {
            for (int i = 0; i < theProducts.Count; i++)
            {
                double CFixedThisCrop = 0;
                double CaboveGroundResidues = 0;
                double CbelowGroundResidues = 0;
                double CHarvestable = 0;
                if (theProducts[i].composition.GetisGrazed())  //
                    CaboveGroundResidues = theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc() * propAboveGroundResidues[1];
                else
                    CaboveGroundResidues = theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc() * propAboveGroundResidues[0];
                CFixedThisCrop += CaboveGroundResidues;
                double adjustment = 1.0;// (1 - 0.5) / (theProducts[i].GetExpectedYield() / theProducts[i].GetPotential_yield());
                CbelowGroundResidues = theProducts[i].GetExpectedYield() * propBelowGroundResidues * GetCconcBelowGroundResidues() * adjustment;
                CFixedThisCrop += CbelowGroundResidues;
                CHarvestable = theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc();
                CFixedThisCrop += CHarvestable;
                CFixed += CFixedThisCrop;
            }
        }
    }
    //!  Calculate the Crop Residues.
    public void CalculateCropResidues()
    {
        surfaceResidueC = 0;
        subsurfaceResidueC = 0;
        surfaceResidueN = 0;
        subsurfaceResidueN = 0;
        surfaceResidueDM = 0;
        unutilisedGrazableC = 0;
        unutilisedGrazableN = 0;
        unutilisedGrazableDM = 0;
        if (Getname() != "Bare soil")
        {
            for (int i = 0; i < theProducts.Count; i++)
            {
                if (theProducts[i].composition.GetisGrazed()) //grazed crop
                {
                    surfaceResidueC += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc() * propAboveGroundResidues[1];
                    surfaceResidueN += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[1];
                    surfaceResidueDM += theProducts[i].GetExpectedYield() * propAboveGroundResidues[1];
                    if (theProducts[i].Expected_yield >= theProducts[i].Grazed_yield)//yield in excess of grazed is added to surface residues
                    {
                        unutilisedGrazableC += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield) * theProducts[i].composition.GetC_conc();
                        surfaceResidueC += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield) * theProducts[i].composition.GetC_conc();
                        unutilisedGrazableN += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield) * theProducts[i].composition.GetN_conc();
                        surfaceResidueN += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield) * theProducts[i].composition.GetN_conc();
                        unutilisedGrazableDM += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield);
                        surfaceResidueDM += (theProducts[i].Expected_yield - theProducts[i].Grazed_yield);
                    }
                }
                else //ungrazed crop
                {
                    if ((theProducts[i].Harvested == "Harvested") || (theProducts[i].Harvested == "Burnt stubble"))
                    {
                        surfaceResidueC += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc() * propAboveGroundResidues[0];
                        surfaceResidueN += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[0];
                        surfaceResidueDM += theProducts[i].GetExpectedYield() * propAboveGroundResidues[0];
                    }
                    else if (theProducts[i].Harvested.Contains("Residue"))
                    {
                        residueToNext = new GlobalVars.product(theProducts[i]);
                        //add surface residues to amount to carry over to bare soil
                        residueToNext.SetModelled_yield(theProducts[i].GetExpectedYield() * (1 + propAboveGroundResidues[0]));
                        residueToNext.SetExpectedYield(theProducts[i].GetExpectedYield() * (1 + propAboveGroundResidues[0]));
                    }
                    else
                    {
                        surfaceResidueC += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetC_conc() * (propAboveGroundResidues[0] + 1);
                        surfaceResidueN += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * (propAboveGroundResidues[0] + 1);
                        surfaceResidueDM += theProducts[i].GetExpectedYield() * (propAboveGroundResidues[0] + 1);
                    }
                }
                double adjustment = 1.0;// (1 - 0.5) / (theProducts[i].GetExpectedYield() / theProducts[i].GetPotential_yield());
                subsurfaceResidueC += theProducts[i].GetExpectedYield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues() * adjustment);
                subsurfaceResidueN += theProducts[i].GetExpectedYield() * GetCconcBelowGroundResidues() * GetpropBelowGroundResidues()*adjustment / CtoNBelowGroundResidues;
            }
        }
    }

    //!  Calculate Crop Residue Burning.
    public void CalculateCropResidueBurning()
    {
        double DMburnt = 0;  //DM that is burnt
        for (int i = 0; i < theProducts.Count; i++)
        {
            if ((theProducts[i].Harvested == "Burnt") || (theProducts[i].Harvested == "Burnt stubble"))
            {
                DMburnt = surfaceResidueDM;
                burntResidueC = surfaceResidueC;
                burntResidueN = surfaceResidueN;
                surfaceResidueN = 0.0;
                surfaceResidueC = 0.0;
            }
        }
        if (!zeroGasEmissionsDebugging)
        {
            burningBlackC = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueBlackCEmissionFactor();
            burningCOC = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueCOEmissionFactor();
            burningCO2C = burntResidueC - (burningBlackC + burningCOC);
            burningN2ON = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueN2OEmissionFactor();
            burningNH3N = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueNH3EmissionFactor();
            burningNOxN = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueNOxEmissionFactor();
        }
        burningOtherN = burntResidueN - (burningN2ON + burningNH3N + burningNOxN);
    }
    //!  Calculate Excreta N Input.
    /*!
     Assumes that the distribution of excreta between grazed crops is the same as the contribution the crop makes to the total grazed DM
    */
    public void CalculateExcretaNInput()
    {
        excretaNInput = 0;
        urineNasFertilizer = 0;
        faecalN = 0;
        int feedCode;
        urineNH3emission = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            feedCode = theProducts[i].composition.GetFeedCode();
            if (theProducts[i].composition.GetisGrazed())
            {
                for (int j = 0; j < GlobalVars.Instance.getmaxNumberFeedItems(); j++)
                {
                    if (theProducts[i].composition.GetFeedCode() == j)
                    {
                        double grazedDM=GlobalVars.Instance.grazedArray[j].fieldDMgrazed;
                        if (grazedDM > 0)
                        {
                            double proportion = theProducts[i].Grazed_yield / grazedDM;
                            urineNasFertilizer += proportion * GlobalVars.Instance.grazedArray[j].urineN;
                            urineNH3emission += urineNH3EmissionFactor * urineNasFertilizer;
                            faecalN += proportion * GlobalVars.Instance.grazedArray[j].faecesN;
                        }
                    }
                }
            }
        }
        if (residueFromPrevious!=null)
        {
            feedCode = residueFromPrevious.composition.GetFeedCode();
            for (int j = 0; j < GlobalVars.Instance.getmaxNumberFeedItems(); j++)
            {
                if (residueFromPrevious.composition.GetFeedCode() == j)
                {
                    double grazedDM = GlobalVars.Instance.grazedArray[j].fieldDMgrazed;
                    if (grazedDM > 0)
                    {
                        double proportion = residueFromPrevious.Grazed_yield / grazedDM;
                        urineNasFertilizer += proportion * GlobalVars.Instance.grazedArray[j].urineN;
                        urineNH3emission += urineNH3EmissionFactor * urineNasFertilizer;
                        faecalN += proportion * GlobalVars.Instance.grazedArray[j].faecesN;
                    }
                }
            }
        }
        excretaNInput = urineNasFertilizer + faecalN;
    }
    //!  Calculate Excreta C Input.
    /*!
     Assumes that the distribution of excreta between grazed crops is the same as the contribution the crop makes to the total grazed DM
    */
    public void CalculateExcretaCInput()
    {
        excretaCInput = 0;
        urineC = 0;
        faecalC = 0;
        grazingCH4C = 0;
        int feedCode;
        for (int i = 0; i < theProducts.Count; i++)
        {
            feedCode = theProducts[i].composition.GetFeedCode();
            if (theProducts[i].composition.GetisGrazed())
            {
                for (int j = 0; j < GlobalVars.Instance.getmaxNumberFeedItems(); j++)
                {
                    if (theProducts[i].composition.GetFeedCode() == j)
                    {
                        if (GlobalVars.Instance.grazedArray[j].fieldDMgrazed > 0)
                        {
                            double proportion = theProducts[i].Grazed_yield / GlobalVars.Instance.grazedArray[j].fieldDMgrazed;
                            urineC += proportion * GlobalVars.Instance.grazedArray[j].urineC;
                            faecalC += proportion * GlobalVars.Instance.grazedArray[j].faecesC;
                            grazingCH4C += proportion * GlobalVars.Instance.grazedArray[j].fieldCH4C;
                        }
                    }
                }
            }
        }
        if (residueFromPrevious != null)
        {
            feedCode = residueFromPrevious.composition.GetFeedCode();
            for (int j = 0; j < GlobalVars.Instance.getmaxNumberFeedItems(); j++)
            {
                if (residueFromPrevious.composition.GetFeedCode() == j)
                {
                    double grazedDM = GlobalVars.Instance.grazedArray[j].fieldDMgrazed;
                    if (grazedDM > 0)
                    {
                        double proportion = residueFromPrevious.Grazed_yield / GlobalVars.Instance.grazedArray[j].fieldDMgrazed;
                        urineC += proportion * GlobalVars.Instance.grazedArray[j].urineC;
                        faecalC += proportion * GlobalVars.Instance.grazedArray[j].faecesC;
                        grazingCH4C += proportion * GlobalVars.Instance.grazedArray[j].fieldCH4C;
                    }
                }
            }
        }
        excretaCInput = urineC + faecalC;
    }
    //!  Calculate the amount of manure N needed for this crop and take this from the manure bank.
    public void GetManureForCrop()
    {
        for (int i = 0; i < manureApplied.Count; i++)
        {
            double amountTotalN = manureApplied[i].getNamount();
            int ManureType = manureApplied[i].getManureType();
            int speciesGroup = manureApplied[i].getspeciesGroup();            
             GlobalVars.Instance.theManureExchange.TakeManure(amountTotalN, lengthOfSequence, ManureType, speciesGroup);
        }
    }
    //!  Calculate the fluxes associated with manure applications. 
    /*!
     * Calculates the NH3 emission and the amount of manure C and C entering the soil
     \param lockit, one boolean argument.
    */
    public void CalculateManureInput(bool lockit)
    {
        manureNH3emission = 0;
        //need to modify this to allow manure OM to be placed at different depths in the soil
        for (int i = 0; i < duration; i++)
        {
            manureFOMCsurface[i] = 0;
            manureHUMCsurface[i] = 0;
            manureBiocharCsurface[i] = 0;
            manureFOMNsurface[i] = 0;
            manureHUMNsurface[i] = 0;
            manureTAN[i] = 0;
        }
        double NH3EmissionFactor=0;
        double totManureCapplied = 0;
        manure aManure;
        for (int i = 0; i < manureApplied.Count; i++)
        {
            double amountTotalN = manureApplied[i].getNamount() * area;
            totalManureNApplied += manureApplied[i].getNamount();
            int ManureType = manureApplied[i].getManureType();
            int speciesGroup = manureApplied[i].getspeciesGroup();
            string applicType = manureApplied[i].Applic_techniqueS;
            int applicationMonth = manureApplied[i].GetMonth_applied();
            if (lockit == false)
            {
                aManure = GlobalVars.Instance.theManureExchange.TakeManure(amountTotalN, lengthOfSequence, ManureType, speciesGroup);
                aManure.DivideManure(1 / area);
                manure anextraManure = new manure(aManure);
                if (theManureApplied == null)
                    theManureApplied = new List<manure>();
                theManureApplied.Add(anextraManure);
            }
            else
                aManure = theManureApplied[i];
            //parameters for ALFAM. Only used for liquid manures
            double airTemperature = GlobalVars.Instance.theZoneData.airTemp[applicationMonth];
            double manureDM = 5.0;
            double EFNH3TotalN = 0;//emission factor as proportion of total N i.e. kg NH3-N/kg total N
            double EFNH3TAN = 0;//emission factor as proportion of TAN i.e. kg NH3-N/kg total ammoniacal N
            int j=GlobalVars.Instance.theZoneData.GetTypeOfManureApplied(ManureType, speciesGroup);
            if (j!=0)
            {
                switch (GlobalVars.Instance.getcurrentInventorySystem())
                {
                    case 1://UNECE with modifications
                        ALFAM emissionEvent = new ALFAM();
                        switch (ManureType)
                        {
                            case 1:
                                emissionEvent.initialise(0, airTemperature, 2.0, speciesGroup, manureDM, 2.2, 25.0, 1, 148.0);
                                EFNH3TAN = emissionEvent.ALFAM_volatilisation();
                                break;
                            case 2:
                                emissionEvent.initialise(0, airTemperature, 2.0, speciesGroup, manureDM, 2.2, 25.0, 1, 148.0);
                                EFNH3TAN = emissionEvent.ALFAM_volatilisation();
                                break;
                            case 3:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 4:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 5:
                                emissionEvent.initialise(0, airTemperature, 2.0, speciesGroup, manureDM, 2.2, 25.0, 1, 148.0);
                                EFNH3TAN = emissionEvent.ALFAM_volatilisation();
                                break;
                            case 6:
                                emissionEvent.initialise(0, airTemperature, 2.0, speciesGroup, manureDM, 2.2, 25.0, 1, 148.0);
                                EFNH3TAN = emissionEvent.ALFAM_volatilisation();
                                break;
                            case 7:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 8:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 9:
                                emissionEvent.initialise(0, airTemperature, 2.0, speciesGroup, manureDM, 2.2, 25.0, 1, 148.0);
                                EFNH3TAN = emissionEvent.ALFAM_volatilisation();
                                break;
                            case 10:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 12:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 13:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            case 14:
                                EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                                break;
                            default:
                                break;

                        }
                        if (EFNH3TotalN > 0) //then need to convert EF based on total N to an EF based on TAN
                        {
                            //Revised NH3 emission
                            double temp = aManure.GetTotalN() / aManure.GetTAN();
                            NH3EmissionFactor = EFNH3TotalN * temp;
                            //end revised NH3 emission
                        }
                        else
                            NH3EmissionFactor = EFNH3TAN;
                        //                                gotit = true;
                        break;
                    case 2: //IPCC
                        EFNH3TotalN = GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N            NH3EmissionFactor = EFNH3TotalN;
                        NH3EmissionFactor = EFNH3TotalN;
                        //gotit = true;
                        break;
                    case 3:
                        double refEFNH3 = GlobalVars.Instance.theZoneData.theFertManData[j].fertManNH3EmissionFactor;
                        double HousingRefTemp = GlobalVars.Instance.theZoneData.theFertManData[j].fertManNH3EmissionFactorHousingRefTemperature;
                        double actualTemp = GlobalVars.Instance.Temperature(GlobalVars.Instance.theZoneData.GetaverageAirTemperature(),
                            0.0, manureApplied[i].GetdayOfApplication(), 0.0, GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude(), GlobalVars.Instance.theZoneData.GetairtemperatureOffset());
                        double KHtheta = Math.Pow(10, -1.69 + 1447.7 / (actualTemp + GlobalVars.absoluteTemp));
                        double KHref = Math.Pow(10, -1.69 + 1447.7 / (HousingRefTemp + GlobalVars.absoluteTemp));
                        NH3EmissionFactor = (KHref / KHtheta) * refEFNH3;
                        //gotit = true;
                        break;
                }//end switch of inventory system
            }
            else
            {
                string messageString = ("Error - unable to find ammonia emission factor for a field-applied manure\n");
                messageString += " Manure name " + aManure.Getname() + " ManureType = " + ManureType + " SpeciesGroup = " + speciesGroup + " \n";
                messageString += " Crop sequence name " + cropSequenceName + " \n";
                messageString += " Crop start year " + GetStartYear().ToString();
                GlobalVars.Instance.Error(messageString);
            }
            if (zeroGasEmissionsDebugging)
                NH3EmissionFactor = 0;

            double NH3ReductionFactor = 0;
            int maxApps = GlobalVars.Instance.theZoneData.themanureAppData.Count;
            bool gotit = false;
            for (int k = 0; k < maxApps; k++)
            {
                string tmpName = GlobalVars.Instance.theZoneData.themanureAppData[k].name;
                if (tmpName == applicType)
                {
                    NH3ReductionFactor = GlobalVars.Instance.theZoneData.themanureAppData[k].NH3EmissionReductionFactor;
                    gotit = true;
                    break;
                }
            }
            if (!gotit)
            {
                string messageString = ("Error - unable to find ammonia emission reduction factor for a manure or fertiliser application method\n");
                messageString += " Application method name " + applicType + " \n";
                messageString += " Crop sequence name " + cropSequenceName + " \n";
                messageString += " Crop start year " + GetStartYear().ToString();
                GlobalVars.Instance.Error(messageString);
            }
            double tmpNH3emission = 0.0;
            switch (GlobalVars.Instance.getcurrentInventorySystem())
            {
                case 1:
                    tmpNH3emission = NH3EmissionFactor * (1 - NH3ReductionFactor) * aManure.GetTAN();
                    break;
                case 2://IPCC EF, based on total N
                    tmpNH3emission = NH3EmissionFactor * (1 - NH3ReductionFactor) * aManure.GetTotalN();
                    break;
                case 3:
                    tmpNH3emission = NH3EmissionFactor * (1 - NH3ReductionFactor) * aManure.GetTAN();
                    break;
            }
            manureNH3emission += tmpNH3emission;
            int dayNo= (int) manureApplied[i].GetRelativeDay(getStartLongTime());
            if (tmpNH3emission <= aManure.GetTAN())
                aManure.SetTAN(aManure.GetTAN() - tmpNH3emission);
            else //this should rarely happen and only if using Tier 2 for solid manures
            {
                tmpNH3emission -= aManure.GetTAN();
                aManure.SetTAN(0.0);
                aManure.SetlabileOrganicN(aManure.GetlabileOrganicN() - tmpNH3emission);
            }
            if (dayNo >= duration)
            {
                Console.Write("");
                dayNo = (int)manureApplied[i].GetRelativeDay(getStartLongTime());
            }
            manureTAN[dayNo] += aManure.GetTAN();
            manureFOMCsurface[dayNo] += aManure.GetdegC() + aManure.GetnonDegC();
            manureHUMCsurface[dayNo] += aManure.GethumicC();
            manureBiocharCsurface[dayNo] += aManure.GetBiocharC();
            manureFOMNsurface[dayNo] += aManure.GetlabileOrganicN();
            manureHUMNsurface[dayNo] += aManure.GethumicN();
            totManureCapplied += aManure.GetdegC() + aManure.GetnonDegC() + aManure.GethumicC();
            GlobalVars.Instance.log(manureFOMNsurface[dayNo].ToString(), 5);
        }//end of each manure application
    }
    //!  Calculate the fluxes associated with fertiliser applications. 
    /*!
     * Calculates the NH3 emission and the amount of fertiliser N entering the soil
    */



    //!  Calculate the fluxes associated with fertiliser applications. 
    /*!
     * Calculates the NH3 emission and the amount of fertiliser N entering the soil
    */
    public void CalculateFertiliserInput()
    {
        double fertiliserNin = 0;
        fertiliserNH3emission = 0;
        fertiliserNinput = 0;
        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getfertManFilePath());
        for (int i = 0; i < fertiliserApplied.Count; i++)
        {
            double Napplied = 0;
            if (fertiliserApplied[i].getName() != "Nitrification inhibitor")
                Napplied=fertiliserApplied[i].getNamount();
            fertiliserNinput += Napplied;
            string fertilizerName = fertiliserApplied[i].getName();
            cropInformation.setPath("AgroecologicalZone("+GlobalVars.Instance.GetZone().ToString()+").fertiliser");
            int max = 0;
            int min = 99;
            cropInformation.getSectionNumber(ref min, ref max);
            cropInformation.Identity.Add(min);
            bool found = false;
            for (int j = min; j <= max; j++)
            {
                cropInformation.Identity[1] = j;
                string fertManName = cropInformation.getItemString("Name");
                if (fertManName.CompareTo(fertilizerName) == 0)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
                GlobalVars.Instance.Error("Fertiliser not found in FertMan file for " + fertilizerName);
            cropInformation.PathNames.Add("Cconcentration");
            cropInformation.Identity.Add(-1);
            double Cconc = cropInformation.getItemDouble("Value");
            cropInformation.PathNames[2] = "Nconcentration";
            double Nconc = cropInformation.getItemDouble("Value");
            double amount = 0;
            if (Nconc > 0)
            {
                amount = Napplied / Nconc;
                fertiliserNin += Napplied;
                fertiliserC += amount * Cconc;
            }
            else
            {
                amount = fertiliserApplied[i].getCamount();
                fertiliserC += amount * 1000.0 * Cconc;
            }
            double NH3EmissionFactor = 0;
            int maxFert = 0;
            maxFert = GlobalVars.Instance.theZoneData.theFertManData.Count;
            found=false;
            for (int j = 0; j < maxFert; j++)
            {
                string tmpName = GlobalVars.Instance.theZoneData.theFertManData[j].name;             
                if (tmpName == fertilizerName)
                {
                    NH3EmissionFactor = GlobalVars.Instance.theZoneData.theFertManData[j].fertManNH3EmissionFactor;
                    found = true;
                    break;
                }
            }
            if (zeroGasEmissionsDebugging)
                NH3EmissionFactor = 0;
            if (found == false)
                GlobalVars.Instance.Error("NH3 not found in parameter file for " + fertilizerName);
            double tmpNH3emission = NH3EmissionFactor * Napplied;
            fertiliserNH3emission += tmpNH3emission;
            int applicDay = fertiliserApplied[i].GetdayOfApplication();
            if ((applicDay < 0) || (applicDay > duration))
            {
                string messageString = ("Error - Fertiliser applied outside crop period\n");
                messageString += " Crop sequence name " + cropSequenceName + " \n";
                messageString += " Crop number " + identity + " \n";
                messageString += " Crop start year " + GetStartYear().ToString();
                GlobalVars.Instance.Error(messageString);
            }
            //Napplied now refers to N entering soil
            fertiliserN[applicDay]+= Napplied - tmpNH3emission; //this is what enters the soil
            if (fertilizerName == "Nitrification inhibitor")
            {
                if (Napplied > 1.0)
                {
                    string messageString = ("Error - nitrification inhibitor efficiency must be <=1.0\n");
                    messageString += " Crop sequence name " + cropSequenceName + " \n";
                    messageString += " Crop number " + identity + " \n";
                    messageString += " Crop start year " + GetStartYear().ToString();
                    GlobalVars.Instance.Error(messageString);
                }
                else
                    nitrificationInhibitor[applicDay] = fertiliserApplied[i].getNamount();
            }
        }        
    }
    //!  Calculate the effective amount of Nitrification Inhibitor.
    /*!
     * includes the degradation of inhibitor with temperature
     \return the effective inhibitor as a double value.
    */
    public double CalculateNitrificationInhibitor()
    {
        double retVal = 0;
        for (int i = 0; i < duration; i++)
        {
            if (i > 0)
            {
                if (nitrificationInhibitor[i] < nitrificationInhibitor[i - 1])  //no new inhibitor applied
                {
                    double temp = temperature[i];
                    double degRate = Math.Log(2) / (168 * Math.Exp(-0.084 * temp));//from F.M. Kelliher a,b,*, T.J. Clough b, H. Clark c, G. Rys d, J.R. Sedcole b
                    nitrificationInhibitor[i] = nitrificationInhibitor[i - 1] * Math.Exp(-degRate);
                    if (nitrificationInhibitor[i] < 0.000001)
                        nitrificationInhibitor[i] = 0;
                }
            }
            retVal += nitrificationInhibitor[i];
        }
        retVal /= duration;
        return retVal;
    }
    //!  Calculate all the Crop Inputs of C and N . 
    /*!
     \param lockit, a boolean argument.
    */
    public void DoCropInputs(bool lockit)
    {
        if (!lockit)
        {
            CalculateExcretaCInput();
            CalculateExcretaNInput();
            CalculateManureInput(lockit);
        }
        if (Getname() != "Bare soil")
        {
            CalculateCFixed();
            CalculateCropResidues();
            CalculateCropResidueBurning();
        }
        CalculateHarvestedYields();
    }
    //!  Handle the fate of the crop residues remaining on bare soil from the previous crop. 
    /*!
     \param someResidueFromPrevious, an instance that points to a GlobalVar.product.
    */
    public void HandleBareSoilResidues(GlobalVars.product someResidueFromPrevious)
    {
        if ((Getname() == "Bare soil") && (someResidueFromPrevious!=null))
        {
            double DMburnt = 0;
            double grazedDM = 0;
            residueFromPrevious = new GlobalVars.product(someResidueFromPrevious);
            //This is to keep 
            //residueFromPrevious.SetExpectedYield(residueFromPrevious.GetModelled_yield();
            //retrieve the DM in the residue from the previous crop
            double remainingResidueDM = residueFromPrevious.GetModelled_yield();
            if (remainingResidueDM > 0)
            {
                if (residueFromPrevious.GetGrazed_yield() > 0)
                {
                    residueFromPrevious.composition.SetisGrazed(true);
                    grazedDM = residueFromPrevious.GetGrazed_yield();
                    harvestedC = residueFromPrevious.GetGrazed_yield() * residueFromPrevious.composition.GetC_conc();
                    grazedC = harvestedC;
                    harvestedN += residueFromPrevious.GetGrazed_yield() * residueFromPrevious.composition.GetN_conc();
                    grazedN += harvestedN;
                    harvestedDM += grazedDM;
                    GlobalVars.product aProduct = new GlobalVars.product(residueFromPrevious);
                    //convert to yield from yield per ha
                    aProduct.composition.Setamount((aProduct.GetModelled_yield() - aProduct.GetGrazed_yield())*area);
                    GlobalVars.Instance.AddGrazableProductUnutilised(aProduct.composition);
                }
                remainingResidueDM -= grazedDM;
                unutilisedGrazableDM += remainingResidueDM;
                unutilisedGrazableC += remainingResidueDM * someResidueFromPrevious.composition.GetC_conc();
                unutilisedGrazableN += remainingResidueDM * someResidueFromPrevious.composition.GetN_conc();

                if (residueFromPrevious.Harvested.Contains("burnt")) //burn the DM remaining after grazing of residues
                {
                    DMburnt = remainingResidueDM;
                    if (DMburnt >= 0)
                    {
                        burntResidueC = DMburnt * residueFromPrevious.composition.GetC_conc();
                        burntResidueN = DMburnt * residueFromPrevious.composition.GetN_conc();
                        surfaceResidueN = 0.0;
                        surfaceResidueC = 0.0;
                    }
                }
                else if (residueFromPrevious.Harvested.Contains("harvested")) //
                {
                    harvestedC += remainingResidueDM * residueFromPrevious.composition.GetC_conc();
                    harvestedN += remainingResidueDM * residueFromPrevious.composition.GetN_conc();
                   // theProducts[1].SetExpectedYield(remainingResidueDM);//otherwise the residues will be harvested with zero mass
                    string messageString = ("Error - attempt to harvest crop residue on bare soil - function not implemented\n");
                    messageString += " Crop sequence name " + cropSequenceName + " \n";
                    messageString += " Crop number " + identity + " \n";
                    messageString += " Crop start year " + GetStartYear().ToString();
                    GlobalVars.Instance.Error(messageString);
                }
                else
                //return the residue to the soil surface
                {
                    surfaceResidueC = remainingResidueDM * residueFromPrevious.composition.GetC_conc();
                    surfaceResidueN = remainingResidueDM * residueFromPrevious.composition.GetN_conc();
                }
            }
            residueFromPrevious.composition.Setamount(grazedDM);//so this is logged as produced
            burningBlackC = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueBlackCEmissionFactor();
            burningCOC = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueCOEmissionFactor();
            burningCO2C = burntResidueC - (burningBlackC + burningCOC);

            burningN2ON = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueN2OEmissionFactor();
            burningNH3N = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueNH3EmissionFactor();
            burningNOxN = DMburnt * GlobalVars.Instance.theZoneData.GetburntResidueNOxEmissionFactor();
            burningOtherN = burntResidueN - (burningN2ON + burningNH3N + burningNOxN);
        }
    }
    //!  Get the amount of manure organic N on the soil surface.
    /*!
     \return the amount of N (kg/ha) as a double value.
    */
    public double GetmanureOrgN()
    {
        double retVal = 0;
        for (int i = 0; i < manureFOMNsurface.Length; i++)
            retVal += manureFOMNsurface[i];
        for (int i = 0; i < manureHUMNsurface.Length; i++)
            retVal += manureHUMNsurface[i];
        return retVal;
    }
    //!  Get wthe manure TAN available to the crop.
    /*!
     \return amount of TAN (kg/ha) as  a double value.
    */
    public double GetmanureTAN()
    {
        double retVal = 0;
        for (int i = 0; i < manureTAN.Length; i++)
            retVal += manureTAN[i];
        return retVal;
    }
    //!  Calculate N fixation
    /*!
     \param deficit, the difference between the maximum crop N uptake and the mineral N available from the soil as a double argument.
     \return N fixation (kg/ha) as  a double value.
    */
    public double GetNfixation(double deficit)
    {
        double retVal = 0;
        if ((deficit>0)&&(NfixationFactor>=0))
            retVal = deficit * NfixationFactor;
        return retVal;
    }

    //!  Calculate the N available for growth and then the relative growth rate.
    /*!
     \param surplusMineralN mineral N remaining in the soil from the previous crop (kg/ha).
     \param thesoilNMineralisation the mineral N resulting from decomposition of soil organic matter  (kg/ha).
     \param relGrowth the growth rate relative to the maximum potential rate.
    */
    public void CalcAvailableNandGrowth(ref double surplusMineralN, double thesoilNMineralisation, ref double relGrowth)
    {
        mineralNFromLastCrop = surplusMineralN;
        soilNMineralisation = thesoilNMineralisation;
        if (soilNMineralisation < 0)
            Console.WriteLine();
        double manureOrgN = GetmanureOrgN();
        double totmanureTAN = GetmanureTAN();
        double soilNSupply = mineralNFromLastCrop + soilNMineralisation;
        //calculate N deposition from atmosphere (kg/ha)
        nAtm = (NDepositionRate/365) * (getEndLongTime() - getStartLongTime());
        //note tha N2O emissions are currently calculated after removal of NH3 emissions and without humic N
        double evenNSupply = urineNasFertilizer + soilNMineralisation + nAtm - urineNH3emission;
        relGrowth = 0;
        CalculateLeachingAndUptake(mineralNFromLastCrop, evenNSupply, ref relGrowth, ref surplusMineralN);//calculate leaching and modelled crop uptake

        GlobalVars.Instance.log("Crop " + name + " rel growth " + relGrowth.ToString("0.00") + " Nfix " + Nfixed.ToString("0.00") + " soil min " + soilNMineralisation.ToString("0.00")
            + " fromlast " + mineralNFromLastCrop.ToString("0.00") + " surplus " +surplusMineralN.ToString("0.00") + " leaching " + nitrateLeaching.ToString("0.00")
            + " avail " + mineralNavailable.ToString("0.00"), 5);
        //GlobalVars.Instance.log("Manure NH3N " + manureNH3emission.ToString());
    }
    //!  Calculate the amount of each feedItem that is grazed.
    public void calcGrazedFeedItems()
    {
        for (int i = 0; i < theProducts.Count; i++)
        {
            if ((theProducts[i].composition.GetisGrazed())||(theProducts[i].Harvested.Contains("Residue")))
            {
                int feedCode = theProducts[i].composition.GetFeedCode();
                GlobalVars.Instance.grazedArray[feedCode].fieldDMgrazed += theProducts[i].Grazed_yield * area/lengthOfSequence;
                GlobalVars.Instance.grazedArray[feedCode].name = theProducts[i].composition.GetName();
            }
        }
    }
    //!  Get the total dry matter Yield.
    /*!
     \return total dry matter Yield (kg/ha) as  a double value.
    */
    public double GetDMYield()
    {
        double DMYield = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            DMYield += theProducts[i].GetModelled_yield();
        }
        return DMYield;
    }
    //!  Check if modelled and expected yield are sufficiently close that the iterations can stop.
    /*!
     \return true if the convergence is complete.
    */
    public bool expect()
    {
        int numberOfMatching = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].GetModelled_yield() > 0)
            {
                double diff = theProducts[i].GetModelled_yield() - theProducts[i].GetExpectedYield();
                double relative_diff = System.Math.Abs(diff / theProducts[i].GetModelled_yield());
                double threshold = 0.1 * GlobalVars.Instance.getmaxToleratedError();
                if (relative_diff < threshold)
                    numberOfMatching++;
            }
            else
            {
                theProducts[i].SetExpectedYield(0);
                theProducts[i].SetModelled_yield(0);
                numberOfMatching++;
            }
        }
        if (numberOfMatching == theProducts.Count)
        {
            if (mineralNavailable < 0.0)
            {
                string messageString = ("Error - insufficient mineral N available to satisfy immobilisation in soil\n");
                messageString += " Crop sequence name " + cropSequenceName + " \n";
                messageString += " Crop number " + identity + " \n";
                messageString += " Crop start year " + GetStartYear().ToString();
                GlobalVars.Instance.Error(messageString);
                return false;
            }
            else
                return true;
        }
        else
            return false;
    }
    //!  Calculate modelled yield
    /*!
     \param surplusMineralN, one double argument.
     \param relGrowth, oen double argument.
     \param final, one boolean argument.
     \return a boolean value.
    */
    public bool CalcModelledYield(double surplusMineralN, double relGrowth, bool final)
    {
        bool retVal = false;
        if (theProducts.Count > 4)
        {
           string messageString=("Error - too many products in crop");
           GlobalVars.Instance.Error(messageString);
        }
        if (Getname() != "Bare soil")
        {
            GlobalVars.Instance.log("relGrowth " + relGrowth.ToString(), 5);
            for (int i = 0; i < theProducts.Count; i++)
            {
                if (theProducts[i].GetGrazed_yield() > theProducts[i].GetPotential_yield())
                {
                    string messageString = ("Error - grazed yield " + theProducts[i].GetGrazed_yield().ToString() +
                            " required is greater than the potential yield " + theProducts[i].GetwaterLimited_yield().ToString());
                    GlobalVars.Instance.Error(messageString);
                }
                theProducts[i].SetModelled_yield(theProducts[i].GetPotential_yield() * relGrowth);
                GlobalVars.Instance.log("expected yield " + theProducts[i].Modelled_yield.ToString(), 5);
            }
            if (expect())  //then the expected and modelled yields have converged 
            {
                for (int i = 0; i < theProducts.Count; i++)
                {
                    bool residueGrazed = false;
                    if (theProducts[i].Harvested.Contains("Residue"))
                        residueGrazed = true;
                    if (((theProducts[i].composition.GetisGrazed())|| (residueGrazed)) && (GlobalVars.Instance.GetstrictGrazing()))
                    {
                        double diff_grazed = theProducts[i].GetModelled_yield() - theProducts[i].Grazed_yield;
                        FileInformation constantFile = new FileInformation(GlobalVars.Instance.getConstantFilePath());
                        constantFile.setPath("constants(0).absoluteGrazedDMtolerance(-1)");
                        double absoluteGrazedDMtolerance = constantFile.getItemDouble("Value");
                        if (Math.Abs(diff_grazed) > absoluteGrazedDMtolerance)
                        {
                            double rel_diff_grazed = 0;
                            if (theProducts[i].Grazed_yield > 0)
                                rel_diff_grazed = diff_grazed / theProducts[i].Grazed_yield;
                            double tolerance = GlobalVars.Instance.getmaxToleratedErrorGrazing();
                            if ((rel_diff_grazed < 0.0) && (Math.Abs(rel_diff_grazed) > tolerance))
                            {
                                WriteFieldFile(0, 0, 0);
                                string messageString = ("Error - modelled production lower than required production for grazed feed item \n");
                                messageString += " Modelled yield " + theProducts[i].GetModelled_yield().ToString() + " Required yield " + theProducts[i].Grazed_yield.ToString();
                                messageString += " Crop sequence name " + cropSequenceName + " \n";
                                messageString += "Crop number " + identity + " \n";
                                messageString += "Crop product = " + theProducts[i].composition.GetName() + " \n";
                                messageString += "Crop start year " + GetStartYear().ToString();
                                GlobalVars.Instance.Error(messageString);
                            }
                        }
                    }
                    theProducts[i].SetExpectedYield(theProducts[i].GetModelled_yield());
                }
                cropNuptake = CalculateCropNUptake();
                mineralNToNextCrop = surplusMineralN;
                retVal = true;  //finish simulation and exit
            }
            else
            {   //calculate new value for expected yield from the difference between expected and modelled yield
                for (int i = 0; i < theProducts.Count; i++)
                {
                    double newExpectedYield = 0;
                    double diff = theProducts[i].GetModelled_yield() - theProducts[i].GetExpectedYield();
                    if (diff < 0)
                        newExpectedYield = theProducts[i].GetExpectedYield() + diff / 2;
                    else
                        newExpectedYield = theProducts[i].GetExpectedYield() - diff / 2;
                    if (newExpectedYield <= 0.0)
                    {
                        newExpectedYield = 0.0001;
                        retVal = true;
                    }
                    theProducts[i].SetExpectedYield(newExpectedYield);
                }
            }
        }
        else  //bare soil
        {
            mineralNToNextCrop = surplusMineralN;
            retVal = true;
        }
    return retVal;
}
    //!  Check to ensure that the modelled and expected yields are the same (or nearly so), and adjust for losses in storage
    /*!
     * Check to ensure that the modelled and expected yields are the same (or nearly so), throwing an error if not.
     * Adjust for losses of DM, C and N in storage
     * Store the information in the feed item baskets
     \param RotationName name of the crop sequence as a string argument.
     \return an integer value of zero (if there is an error, it is trapped internally).
    */
    public int CheckYields(string RotationName)
    {
        if (residueFromPrevious != null)
        {
            residueFromPrevious.composition.Setamount(residueFromPrevious.composition.Getamount() * area / lengthOfSequence);
            GlobalVars.Instance.AddProductProduced(residueFromPrevious.composition);
            //reset value to per ha basis (for debugging)
            residueFromPrevious.composition.Setamount(residueFromPrevious.composition.Getamount() / area);
        }
        else
        {
            for (int i = 0; i < theProducts.Count; i++)
            {
                if (theProducts[i].GetModelled_yield() == 0)
                {
                    string messageString = ("Error - modelled yield is zero\n");
                    messageString += ("Rotation name = " + RotationName + "\n");
                    messageString += ("Crop product = " + theProducts[i].composition.GetName());
                    GlobalVars.Instance.Error(messageString);
                }
                else
                {
                    double expected = theProducts[i].Expected_yield;
                    double modelled = theProducts[i].GetModelled_yield();
                    if (Double.IsNaN(modelled)) //this should never happen..
                    {
                        string messageString = "Error; modelled yield has not been calculated\n";
                        messageString += "Rotation name = " + RotationName + "\n";
                        messageString += "Crop product = " + theProducts[i].composition.GetName() + "\n";
                        messageString += "Crop start year " + GetStartYear().ToString();
                        GlobalVars.Instance.Error(messageString);
                    }

                    double diff = (modelled - expected) / modelled;
                    double tolerance = GlobalVars.Instance.getmaxToleratedError();
                    if (Math.Abs(diff) > tolerance)
                    {
                        double errorPercent = 100 * diff;
                        string messageString;
                        if (diff > 0)
                            messageString = "Error; modelled yield exceeds expected yield by more than the permitted margin of "
                            + tolerance.ToString() + "\n";
                        else
                            messageString = "Error; expected yield exceeds modelled yield by more than the permitted margin"
                            + tolerance.ToString() +"\n";
                        if (errorPercent < 0)
                            errorPercent *= -1.0;
                        messageString += "Rotation name = " + RotationName + "\n";
                        messageString += "Crop product = " + theProducts[i].composition.GetName() + "\n";
                        messageString += "Crop start year " + GetStartYear().ToString() + "\n";
                        messageString += "Percentage error = " + errorPercent.ToString("0.00") + "%" + "\n";
                        messageString += "Expected yield= " + expected.ToString() + " Modelled yield= " + modelled.ToString() + "\n";
                        Write();
                        GlobalVars.Instance.Error(messageString);
                    }
                    else
                    {
                        //accept the modelled yield as valid and add to allFeedAndProductsProduced
                        double productProcessingLossFactor = theProducts[i].composition.GetStoreProcessFactor();
                        if ((theProducts[i].Harvested == "Harvested") || ((theProducts[i].composition.GetisGrazed()))
                            || (theProducts[i].Harvested == "Burnt stubble"))
                        {
                            if (!theProducts[i].composition.GetisGrazed())
                            {
                                theProducts[i].composition.Setamount(theProducts[i].GetExpectedYield());
                                double originalC = theProducts[i].composition.Getamount() * theProducts[i].composition.GetC_conc();
                                double tempCLoss = productProcessingLossFactor * originalC;
                                storageProcessingCLoss += tempCLoss;
                                double originalN = theProducts[i].composition.Getamount() * theProducts[i].composition.GetN_conc();
                                double tempNLoss = productProcessingLossFactor * originalN;
                                storageProcessingNLoss += tempNLoss;
                                theProducts[i].composition.AdjustAmount(1 - productProcessingLossFactor);
                                theProducts[i].composition.SetC_conc((originalC - tempCLoss) / theProducts[i].composition.Getamount());
                                double temp2 = theProducts[i].composition.GetC_conc() * theProducts[i].composition.Getamount();
                            }
                            else
                                theProducts[i].composition.Setamount(theProducts[i].GetGrazed_yield());
                            //multiply by crop area to obtain yield from yield per ha
                            theProducts[i].composition.Setamount(theProducts[i].composition.Getamount() * area / lengthOfSequence);
                            double temp = theProducts[i].composition.Getamount() * theProducts[i].composition.GetN_conc();
                            GlobalVars.Instance.AddProductProduced(theProducts[i].composition);
                            if (theProducts[i].GetGrazed_yield() > 0)
                            {
                                GlobalVars.product aProduct = new GlobalVars.product(theProducts[i]);
                                aProduct.composition.Setamount(aProduct.GetModelled_yield() - aProduct.GetGrazed_yield());
                                GlobalVars.Instance.AddGrazableProductUnutilised(aProduct.composition);
                            }
                            //reset yield value back to per ha basis (for debugging)
                            theProducts[i].composition.Setamount(theProducts[i].composition.Getamount() / area);
                        }
                    }
                }
            }
        }
        return 0;
    }
    //!  Adjust the dates of the crop so fertiliser and manure can be applied according to the day number (counting from the start of the crop) rather than calendar date. 
    /*!
     \param firstYear the first calendar year of the crop as an integer.
    */
    public void AdjustDates(int firstYear)
    {
        bool ret_val = true;
        for (int i = 0; i < GetfertiliserApplied().Count; i++)
        {
            fertRecord aRecord = fertiliserApplied[i];
            ret_val = aRecord.AdjustDates(firstYear,GettheStartDate(), GettheEndDate());
            if (!ret_val)  //this should not happen
            {
                string errorMsg = "Fertiliser applied after end of crop for " + Getname() + " in " + GetfertiliserApplied()[i].GetDate().ToString();
                GlobalVars.Instance.Error(errorMsg);
            }
        }
        for (int i = 0; i < manureApplied.Count; i++)
        {
            fertRecord aRecord = manureApplied[i];
            ret_val = aRecord.AdjustDates(firstYear,GettheStartDate(), GettheEndDate());
            if (!ret_val)  //this should not happen
            {
                string errorMsg = "Manure applied after end of crop for " + Getname() + " in " + GetmanureApplied()[i].GetDate().ToString();
                GlobalVars.Instance.Error(errorMsg);
            }
        }
        //Console.WriteLine(" init start yr " + GetStartYear().ToString() + " init end " + GetEndYear().ToString());
        SetStartYear(GetStartYear() - firstYear);
        SetEndYear(GetEndYear() - firstYear);
        //Console.WriteLine(" fin start yr " + GetStartYear().ToString() + " fin end " + GetEndYear().ToString());
    }
    //!  Check Crop C Balance. 
    /*!
     \param rotationName name of crop sequence as a string argument.
     \param cropNo, number of the crop in the crop sequence as an integer argument.
     \return true if the C budget can be closed within the error tolerance limits
    */
    public bool CheckCropCBalance(string rotationName, int cropNo)
    {
        bool retVal = false;
        double manureCinput = GetManureC();
        double Cinp = CFixed;
        if (residueFromPrevious != null)
        {
            residueCfromLastCrop = residueFromPrevious.GetModelled_yield() * residueFromPrevious.composition.GetC_conc();
            Cinp += residueCfromLastCrop;
        }
        if (residueToNext != null)
            residueCtoNextCrop = residueToNext.GetModelled_yield() * residueToNext.composition.GetC_conc();
        double Cout = surfaceResidueC + subsurfaceResidueC + harvestedC + burntResidueC + residueCtoNextCrop;
        double Cbalance = Cinp - Cout;
        if (Cinp != 0) //cropped or bare soil with crop residues
        {
            double diff = Cbalance / Cinp;
            double tolerance = GlobalVars.Instance.getmaxToleratedError();
            if (Math.Abs(diff) > tolerance)
            {
                double errorPercent = 100 * diff;
                string messageString=("Error; Crop C balance error is more than the permitted margin of "
                        + tolerance.ToString() +"\n");
                messageString+=("Crop name " + name+"\n");
                messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
                GlobalVars.Instance.Error(messageString);
            }
            else
                return true;
        }
        else
            retVal = true;
        return retVal;
    }
    //!  Check Crop N Balance. 
    /*!
     \param rotationName name of crop sequence as a string argument.
     \param cropNo, number of the crop in the crop sequence as an integer argument.
     \return true if the N budget can be closed within the error tolerance limits
    */
    public void CheckCropNBalance(string rotationName, int cropNo)
    {
        double Ninp = 0;
        if (Getname() != "Bare soil")
        Ninp = CalculateCropNUptake();
        else
        {
            if (residueFromPrevious != null)
            {
                residueNfromLastCrop = residueFromPrevious.GetModelled_yield() * residueFromPrevious.composition.GetN_conc();
                Ninp += residueNfromLastCrop;
            }
        }
        if (residueToNext!=null)
            residueNtoNextCrop = residueToNext.GetModelled_yield() * residueToNext.composition.GetN_conc();
        double Nout = surfaceResidueN + subsurfaceResidueN + harvestedN + burntResidueN + residueNtoNextCrop;
        double Nbalance = Ninp - Nout;
       // GlobalVars.Instance.log("crop " + name + " Ninp " + Ninp.ToString() + " Nsurface " + surfaceResidueN.ToString() + " Nsubsurf " + subsurfaceResidueN.ToString() +
         //   " harvestedN" + harvestedN.ToString());
        if (Ninp != 0) //not bare soil
        {
            double diff = Nbalance / Ninp;
            double tolerance = GlobalVars.Instance.getmaxToleratedError();
            if (Math.Abs(diff) > tolerance)
            {               
                    double errorPercent = 100 * diff;                 
                    string messageString=("Error; Crop N balance error is more than the permitted margin of "
                        + tolerance.ToString() +"\n");
                    messageString+=("Crop name " + name+"\n");
                    messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
                    GlobalVars.Instance.Error(messageString);
           }
        }
    }
    //!  Calculate the duration of the crop.
    /*!
     \return the duration (days) as a double value.
    */
    public double  CalcDuration()
    {
        return duration = theEndDate.getLongTime() - theStartDate.getLongTime();
    }
    //!  Write results data to file
    public void Write()
    {
        GlobalVars.Instance.writeStartTab("CropClass");
        
            GlobalVars.Instance.writeInformationToFiles("Identity", "Identity", "-", identity, parens);
            GlobalVars.Instance.writeInformationToFiles("name", "Crop name", "-", name, parens);
            GlobalVars.Instance.writeInformationToFiles("area", "Area", "ha", area, parens);
            GlobalVars.Instance.writeStartTab("theStartDate");
            theStartDate.Write();
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeStartTab("theEndDate");
            theEndDate.Write();
            GlobalVars.Instance.writeEndTab();
            GlobalVars.Instance.writeInformationToFiles("isIrrigated", "Is irrigated", "-", isIrrigated, parens);
            GlobalVars.Instance.writeInformationToFiles("fertiliserN2OEmission", "N2O emission from fertiliser", "kgN/ha", fertiliserN2OEmission, parens);
            GlobalVars.Instance.writeInformationToFiles("Placeholder", "Placeholder", "", 0, parens);
            GlobalVars.Instance.writeInformationToFiles("soilN2OEmission", "N2O emission from mineralised N", "kgN/ha", soilN2OEmission, parens);
            GlobalVars.Instance.writeInformationToFiles("unutilisedGrazableDM", "Unutilised grazable DM", "kg DM/ha", unutilisedGrazableDM, parens);

            //        GlobalVars.Instance.writeInformationToFiles("NyieldMax", "??", "??", NyieldMax);
            //potential and water limited yield

            GlobalVars.Instance.writeInformationToFiles("CFixed", "C fixed", "kgC/ha", CFixed, parens);
            GlobalVars.Instance.writeInformationToFiles("residueCfromLastCrop", "C in residues from previous crop", "kgC/ha", residueCfromLastCrop, parens);
            GlobalVars.Instance.writeInformationToFiles("surfaceResidueC", "C in surface residues", "kgC/ha", surfaceResidueC, parens);
            GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueC", "C in subsurface residues", "kgC/ha", subsurfaceResidueC, parens);

            GlobalVars.Instance.writeInformationToFiles("urineCCropClass", "C in urine", "kgC/ha", urineC, parens);
            double amount=0;
            for(int i=0;i<manureFOMCsurface.Length;i++)
            {
                amount+=manureFOMCsurface[i];
            }
            GlobalVars.Instance.writeInformationToFiles("manureFOMCsurface", "manureFOMCsurface", "kgC/ha", amount, parens);
            GlobalVars.Instance.writeInformationToFiles("faecalCCropClass", "C in faeces", "kgC/ha", faecalC, parens);
            GlobalVars.Instance.writeInformationToFiles("storageProcessingCLoss", "C lost during processing or storage", "kgC/ha", storageProcessingCLoss, parens);
            GlobalVars.Instance.writeInformationToFiles("fertiliserC", "Emission of CO2 from fertiliser", "kgC/ha", fertiliserC, parens);
            GlobalVars.Instance.writeInformationToFiles("harvestedC", "Harvested C", "kgC/ha", harvestedC, parens);
            GlobalVars.Instance.writeInformationToFiles("harvestedDM", "Harvested DM", "kg DM/ha", harvestedDM, parens);
            GlobalVars.Instance.writeInformationToFiles("burntResidueC", "C in burned crop residues", "kg/ha", burntResidueC, parens);
            GlobalVars.Instance.writeInformationToFiles("unutilisedGrazableC", "C in unutilised grazable DM", "kg/ha", unutilisedGrazableC, parens);
            GlobalVars.Instance.writeInformationToFiles("residueCtoNextCrop", "C in residues passed to next crop", "kgC/ha", residueCtoNextCrop, parens);

            GlobalVars.Instance.writeInformationToFiles("NyieldMax", "Maximum N yield", "kgN/ha", NyieldMax, parens);
            GlobalVars.Instance.writeInformationToFiles("maxCropNuptake", "Maximum crop N uptake", "kgN/ha", maxCropNuptake, parens);
            GlobalVars.Instance.writeInformationToFiles("cropNuptake", "Crop N uptake", "kgN/ha", cropNuptake, parens);
            GlobalVars.Instance.writeInformationToFiles("Nfixed", "N fixed", "kgN/ha", Nfixed, parens);
            GlobalVars.Instance.writeInformationToFiles("nAtm", "N from atmospheric deposition", "kgN/ha", nAtm, parens);
            GlobalVars.Instance.writeInformationToFiles("fertiliserNinput", "Input of N in fertiliser", "kgN/ha", fertiliserNinput, parens);
            GlobalVars.Instance.writeInformationToFiles("excretaNInput", "Input of N in excreta", "kgN/ha", excretaNInput, parens);
            GlobalVars.Instance.writeInformationToFiles("totalManureNApplied", "Total N applied in manure", "kgN/ha", totalManureNApplied, parens);
            GlobalVars.Instance.writeInformationToFiles("mineralNFromLastCrop", "N2O emission from mineralised N", "kgN/ha", mineralNFromLastCrop, parens);
            GlobalVars.Instance.writeInformationToFiles("residueNfromLastCrop", "N in residues from previous crop", "kgN/ha", residueNfromLastCrop, parens);
            GlobalVars.Instance.writeInformationToFiles("manureNH3emission", "NH3-N from manure application", "kgN/ha", manureNH3emission, parens);
            GlobalVars.Instance.writeInformationToFiles("surfaceResidueN", "N in surface residues", "kg/ha", surfaceResidueN, parens);
            GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueN", "N in subsurface residues", "kgN/ha", subsurfaceResidueN, parens);

            GlobalVars.Instance.writeInformationToFiles("surfaceResidueDM", "Surface residue dry matter", "kg/ha", surfaceResidueDM, parens);
            GlobalVars.Instance.writeInformationToFiles("fertiliserNH3emission", "NH3-N from fertiliser application", "kgN/ha", fertiliserNH3emission, parens);
            GlobalVars.Instance.writeInformationToFiles("urineNH3emission", "NH3-N from urine deposition", "kgN/ha", urineNH3emission, parens);
            GlobalVars.Instance.writeInformationToFiles("harvestedN", "N harvested (N yield)", "kgN/ha", harvestedN, parens);
            GlobalVars.Instance.writeInformationToFiles("grazedN", "N grazed", "kgN/ha", grazedN, parens);
            GlobalVars.Instance.writeInformationToFiles("storageProcessingNLoss", "N2 emission during product processing/storage", "kgN/ha", storageProcessingNLoss, parens);
            GlobalVars.Instance.writeInformationToFiles("residueNtoNextCrop", "N in residues passed to next crop", "kgN/ha", residueNtoNextCrop, parens);
            GlobalVars.Instance.writeInformationToFiles("N2Nemission", "N2 emission", "kgN/ha", N2Nemission, parens);
            GlobalVars.Instance.writeInformationToFiles("urineNCropClass", "Urine N", "kgN/ha", urineNasFertilizer, parens);
            GlobalVars.Instance.writeInformationToFiles("faecalNCropClass", "Faecal N", "kgN/ha", faecalN, parens);
            GlobalVars.Instance.writeInformationToFiles("burningN2ON", "N2O emission from burned crop residues", "kgN/ha", burningN2ON, parens);
            GlobalVars.Instance.writeInformationToFiles("burningNH3N", "NH3 emission from burned crop residues", "kgN/ha", burningNH3N, parens);
            GlobalVars.Instance.writeInformationToFiles("burningNOxN", "NOx emission from burned crop residues", "kgN/ha", burningNOxN, parens);
            GlobalVars.Instance.writeInformationToFiles("burningOtherN", "N2 emission from burned crop residues", "kgN/ha", burningOtherN, parens);
            GlobalVars.Instance.writeInformationToFiles("OrganicNLeached", "Leached organic N", "kgN/ha", OrganicNLeached, parens);
            GlobalVars.Instance.writeInformationToFiles("mineralNToNextCrop", "Mineral N to next crop", "kgN/ha", mineralNToNextCrop, parens);
            GlobalVars.Instance.writeInformationToFiles("fertiliserN2OEmission", "N2O emission from fertiliser N", "kgN/ha", fertiliserN2OEmission, parens);
            GlobalVars.Instance.writeInformationToFiles("manureN2OEmission", "N2O emission from manure N", "kgN/ha", manureN2OEmission, parens);
            GlobalVars.Instance.writeInformationToFiles("Placeholder", "Placeholder", "", 0, parens);
            GlobalVars.Instance.writeInformationToFiles("soilN2OEmission", "N2O emission from mineralised N", "kgN/ha", soilN2OEmission, parens);
            GlobalVars.Instance.writeInformationToFiles("N2ONemission", "N2O emission", "kgN/ha", N2ONemission, parens);
            GlobalVars.Instance.writeInformationToFiles("soilNMineralisation", "Soil mineralised N", "kgN/ha", soilNMineralisation, parens);
            GlobalVars.Instance.writeInformationToFiles("mineralNavailable", "Mineral N available", "kgN/ha", mineralNavailable, parens);
            GlobalVars.Instance.writeInformationToFiles("nitrateLeaching", "Nitrate N leaching", "kgN/ha", nitrateLeaching, parens);
            GlobalVars.Instance.writeInformationToFiles("unutilisedGrazableN", "N in unutilised grazable DM", "kgN/ha", unutilisedGrazableN, parens);

            GlobalVars.Instance.writeInformationToFiles("cumulativepotEvapoTrans", "cumulativepotEvapoTrans", "mm", GetcumulativepotEvapoTrans(), parens);
            GlobalVars.Instance.writeInformationToFiles("cumulativePrecipitation", "cumulativePrecipitation", "mm", GetcumulativePrecipitation(), parens);
            GlobalVars.Instance.writeInformationToFiles("cumulativeIrrigation", "cumulativeIrrigation", "mm", GetcumulativeIrrigation(), parens);
            GlobalVars.Instance.writeInformationToFiles("cumulativeEvaporation", "cumulativeEvaporation", "mm", GetcumulativeEvaporation(), parens);
            GlobalVars.Instance.writeInformationToFiles("cumulativeTranspiration", "cumulativeTranspiration", "mm", GetcumulativeTranspiration(), parens);
            GlobalVars.Instance.writeInformationToFiles("cumulativeDrainage", "cumulativeDrainage", "kg/ha", GetcumulativeDrainage(), parens);
            GlobalVars.Instance.writeInformationToFiles("AverageDroughtIndex", "AverageDroughtIndex", "kg/ha", GetAverageDroughtIndexPlant(), parens);

        
        for (int i = 0; i < theProducts.Count; i++)
        {
            theProducts[i].Write(parens + "_theProducts" + i.ToString());
        }
        for (int i = 0; i < fertiliserApplied.Count; i++)
        {
            fertiliserApplied[i].Write(parens + "_fertiliserApplied" + i.ToString());
        }
        for (int i = 0; i < manureApplied.Count; i++)
        {
            manureApplied[i].Write(parens + "_manureApplied" + i.ToString());
        }

        GlobalVars.Instance.writeEndTab();

    }
    //!  Write Excel results file.
    public void WriteXls()
    {
        GlobalVars.Instance.WriteCropFile("name", "New crop", name, true, false);
        GlobalVars.Instance.WriteCropFile("day", "Start_day", theStartDate.GetDay(), true, false);
        GlobalVars.Instance.WriteCropFile("month", "Start_month", theStartDate.GetMonth(), true, false);
        GlobalVars.Instance.WriteCropFile("year", "Start_year", theStartDate.GetYear(), true, false);
        GlobalVars.Instance.WriteCropFile("day", "End_day", theEndDate.GetDay(), true, false);
        GlobalVars.Instance.WriteCropFile("month", "End_month", theEndDate.GetMonth(), true, false);
        GlobalVars.Instance.WriteCropFile("year", "End_year", theEndDate.GetYear(), true, false);
        for (int i = 0; i < manureApplied.Count; i++)
            manureApplied[i].WriteXls();
        for (int i = 0; i < fertiliserApplied.Count; i++)
            fertiliserApplied[i].WriteXls();
        for (int i = 0; i < theProducts.Count; i++)
        {
            GlobalVars.Instance.WriteCropFile("product", "product name", theProducts[i].composition.GetName(), true, false);
            GlobalVars.Instance.WriteCropFile("fate", "fate of product", theProducts[i].Harvested, true, false);
        }
    }
    //!  Write Field File. 
    /*!
     \param deltaSoilN change in soil N storage (kg/ha) as double argument.
     \param deltaSoilC change in soil C storage (kg/ha) as double argument.
     \param soilCO2_CEmission, one double argument.
    */
    public void WriteFieldFile(double deltaSoilN, double deltaSoilC, double soilCO2_CEmission)
    {
        int times = 1;
        if (GlobalVars.Instance.headerField == false)
            times = 2;
        for (int j = 0; j < times; j++)
        {
            GlobalVars.Instance.writeFieldFile("Identity", "Identity", "-", identity, parens, 0);
            GlobalVars.Instance.writeFieldFile("name", "Crop name", "-", name, parens, 0);
            GlobalVars.Instance.writeFieldFile("area", "Area", "ha", area, parens, 0);
            GlobalVars.Instance.writeFieldFile("isIrrigated", "Is irrigated", "-", isIrrigated, parens, 0);
            GlobalVars.Instance.writeFieldFile("unutilisedGrazableDM", "Unutilised grazable DM", "kg/ha", unutilisedGrazableDM, parens, 0);

            //        GlobalVars.Instance.writeInformationToFiles("NyieldMax", "??", "??", NyieldMax);
            //potential and water limited yield

            GlobalVars.Instance.writeFieldFile("CFixed", "C fixed", "kgC/ha", CFixed, parens, 0);
            GlobalVars.Instance.writeFieldFile("surfaceResidueC", "C in surface residues", "kgC/ha", surfaceResidueC, parens, 0);
            GlobalVars.Instance.writeFieldFile("subsurfaceResidueC", "C in subsurface residues", "kgC/ha", subsurfaceResidueC, parens, 0);
           
            GlobalVars.Instance.writeFieldFile("urineCCropClass", "C in urine", "kgC/ha", urineC, parens, 0);
            double amount = 0;
            for (int i = 0; i < manureFOMCsurface.Length; i++)
            {
                amount += manureFOMCsurface[i];
            }
            GlobalVars.Instance.writeFieldFile("manureFOMCsurface", "manureFOMCsurface", "kgC/ha", amount, parens,0);
            GlobalVars.Instance.writeFieldFile("faecalCCropClass", "C in faeces", "kgC/ha", faecalC, parens, 0);
            GlobalVars.Instance.writeFieldFile("storageProcessingCLoss", "C lost during processing or storage", "kgC/ha", storageProcessingCLoss, parens, 0);
            GlobalVars.Instance.writeFieldFile("fertiliserC", "Emission of CO2 from fertiliser", "kgC/ha", fertiliserC, parens, 0);
            GlobalVars.Instance.writeFieldFile("harvestedC", "Harvested C", "kgC/ha", harvestedC, parens, 0);
            GlobalVars.Instance.writeFieldFile("harvestedDM", "Harvested DM", "kgDM/ha", harvestedDM, parens, 0);
            GlobalVars.Instance.writeFieldFile("burntResidueC", "C in burned crop residues", "kgC/ha", burntResidueC, parens, 0);
            GlobalVars.Instance.writeFieldFile("residueCtoNextCrop", "C in residues passed to next crop", "kgC/ha", residueCtoNextCrop, parens, 0);
            
            GlobalVars.Instance.writeFieldFile("unutilisedGrazableC", "C in unutilised grazable DM", "kg/ha", unutilisedGrazableC, parens, 0);

            GlobalVars.Instance.writeFieldFile("NyieldMax", "Maximum N yield", "kgN/ha", NyieldMax, parens, 0);
            GlobalVars.Instance.writeFieldFile("maxCropNuptake", "Maximum crop N uptake", "kgN/ha", maxCropNuptake, parens, 0);
            GlobalVars.Instance.writeFieldFile("cropNuptake", "Crop N uptake", "kgN/ha", cropNuptake, parens, 0);
            GlobalVars.Instance.writeFieldFile("mineralNavailable", "Mineral N available", "kgN/ha", mineralNavailable, parens, 0);
            GlobalVars.Instance.writeFieldFile("soilNMineralisation", "Soil mineralised N", "kgN/ha", soilNMineralisation, parens, 0);
            GlobalVars.Instance.writeFieldFile("Nfixed", "N fixed", "kgN/ha", Nfixed, parens, 0);
            GlobalVars.Instance.writeFieldFile("nAtm", "N from atmospheric deposition", "kgN/ha", nAtm, parens, 0);
            GlobalVars.Instance.writeFieldFile("fertiliserNinput", "Input of N in fertiliser", "kgN/ha", fertiliserNinput, parens, 0);
            GlobalVars.Instance.writeFieldFile("totalManureNApplied", "Total N applied in manure", "kgN/ha", totalManureNApplied, parens, 0);
            GlobalVars.Instance.writeFieldFile("urineNCropClass", "Urine N", "kgN/ha", urineNasFertilizer, parens, 0);
            GlobalVars.Instance.writeFieldFile("faecalNCropClass", "Faecal N", "kgN/ha", faecalN, parens, 0);
            GlobalVars.Instance.writeFieldFile("mineralNFromLastCrop", "N2O emission from mineralised N", "kgN/ha", mineralNFromLastCrop, parens, 0);
            GlobalVars.Instance.writeFieldFile("surfaceResidueN", "N in surface residues", "kgN/ha", surfaceResidueN, parens, 0);
            GlobalVars.Instance.writeFieldFile("subsurfaceResidueN", "N in subsurface residues", "kgN/ha", subsurfaceResidueN, parens, 0);
            GlobalVars.Instance.writeFieldFile("excretaNInput", "Input of N in excreta", "kgN/ha", excretaNInput, parens, 0);
            GlobalVars.Instance.writeFieldFile("manureNH3emission", "NH3-N from manure application", "kgN/ha", manureNH3emission, parens, 0);
            GlobalVars.Instance.writeFieldFile("fertiliserNH3emission", "NH3-N from fertiliser application", "kgN/ha", fertiliserNH3emission, parens, 0);
            GlobalVars.Instance.writeFieldFile("urineNH3emission", "NH3-N from urine deposition", "kgN/ha", urineNH3emission, parens, 0);
            GlobalVars.Instance.writeFieldFile("harvestedN", "N harvested (N yield)", "kgN/ha", harvestedN, parens, 0);
            GlobalVars.Instance.writeFieldFile("storageProcessingNLoss", "N2 emission during product processing/storage", "kgN/ha", storageProcessingNLoss, parens, 0);
            GlobalVars.Instance.writeFieldFile("manureN2OEmission", "N2O emission from manure N", "kgN/ha", manureN2OEmission, parens, 0);
            GlobalVars.Instance.writeFieldFile("soilN2OEmission", "N2O emission from mineralised N", "kgN/ha", soilN2OEmission, parens, 0);
            GlobalVars.Instance.writeFieldFile("fertiliserN2OEmission", "N2O emission from fertiliser", "kgN/ha", fertiliserN2OEmission, parens, 0);
            GlobalVars.Instance.writeFieldFile("Placeholder", "Placeholder", "", 0, parens, 0);
            GlobalVars.Instance.writeFieldFile("burningN2ON", "N2O emission from burned crop residues", "kgN/ha", burningN2ON, parens, 0);
            GlobalVars.Instance.writeFieldFile("N2Nemission", "N2 emission", "kgN/ha", N2Nemission, parens, 0);
            GlobalVars.Instance.writeFieldFile("burningNH3N", "NH3 emission from burned crop residues", "kgN/ha", burningNH3N, parens, 0);
            GlobalVars.Instance.writeFieldFile("burningNOxN", "NOx emission from burned crop residues", "kgN/ha", burningNOxN, parens, 0);
            GlobalVars.Instance.writeFieldFile("burningOtherN", "N2 emission from burned crop residues", "kgN/ha", burningOtherN, parens, 0);
            GlobalVars.Instance.writeFieldFile("N2ONemission", "N2O emission", "kgN/ha", N2ONemission, parens, 0);
            GlobalVars.Instance.writeFieldFile("nitrateLeaching", "Nitrate N leaching", "kgN/ha", nitrateLeaching, parens, 0);
            GlobalVars.Instance.writeFieldFile("mineralNToNextCrop", "Mineral N to next crop", "kgN/ha", mineralNToNextCrop, parens, 0);
            GlobalVars.Instance.writeFieldFile("Precipitation", "Cum precipitation", "mm", GetcumulativePrecipitation(), parens, 0);
            GlobalVars.Instance.writeFieldFile("Irrigation", "Irrigation", "mm", GetcumulativeIrrigation(), parens, 0);
            GlobalVars.Instance.writeFieldFile("Drainage", "Drainage", "mm", GetcumulativeDrainage(), parens, 0);
            GlobalVars.Instance.writeFieldFile("surfaceResidueDM", "Surface residue dry matter", "kg/ha", surfaceResidueDM, parens, 0);
            GlobalVars.Instance.writeFieldFile("startday ", "startday", "day", theStartDate.GetDay(), parens, 0);
            GlobalVars.Instance.writeFieldFile("startmonth ", "startmonth", "month", theStartDate.GetMonth(), parens, 0);
            GlobalVars.Instance.writeFieldFile("startyear ", "startyear", "year", theStartDate.GetYear(), parens, 0);

            GlobalVars.Instance.writeFieldFile("endday ", "endday", "day", theEndDate.GetDay(), parens, 0);
            GlobalVars.Instance.writeFieldFile("endmonth ", "endmonth", "month", theEndDate.GetMonth(), parens, 0);
            GlobalVars.Instance.writeFieldFile("endyear ", "endyear", "year", theEndDate.GetYear(), parens, 0);
            //This section of code writes the storage and processing losses to the Field file. However, this is only for information as the losses are accounted for elsewhere
            double temp = 0;
            for (int I = 0; I < theProducts.Count; I++)
            {
                temp += theProducts[I].Modelled_yield * theProducts[I].composition.GetStoreProcessFactor();
            }
            GlobalVars.Instance.writeFieldFile("storageProcessingDMLoss", "DM lost during processing/storage", "kg DM/ha", temp, parens, 0);
            temp = 0;
            for (int I = 0; I < theProducts.Count; I++)
            {
                temp += theProducts[I].Modelled_yield * theProducts[I].composition.GetStoreProcessFactor() * theProducts[I].composition.GetC_conc();
            }
            temp = 0;
            GlobalVars.Instance.writeFieldFile("storageProcessingCLoss", "C lost during processing/storage", "kg C/ha", temp, parens, 0);
            for (int I = 0; I < theProducts.Count; I++)
            {
                temp += theProducts[I].Modelled_yield * theProducts[I].composition.GetStoreProcessFactor() * theProducts[I].composition.GetN_conc();
            }
            GlobalVars.Instance.writeFieldFile("storageProcessingNLoss", "N lost during processing/storage", "kg N/ha", temp, parens, 0);
            GlobalVars.Instance.writeFieldFile("unutilisedGrazableN", "N in unutilised grazable DM", "kgN/ha", unutilisedGrazableN, parens, 0);

            if(GlobalVars.Instance.headerField==false)
                for (int i = 0; i < 2; i++)
                {
                    GlobalVars.product tmp = new GlobalVars.product();
                    tmp.WriteFieldFile(parens + "_theProducts" + i.ToString(), i, 2);
                }
            if (GlobalVars.Instance.headerField == true)
            {
               
                for (int i = 0; i < theProducts.Count; i++)
                {
                    theProducts[i].WriteFieldFile(parens + "_theProducts" + i.ToString(), i, theProducts.Count);
                }
                if(theProducts.Count==0)
                {
                    for (int i = 0; i <( 16); i++)
                        GlobalVars.Instance.writeFieldFile("0", "0", "0", "0", parens, 0);
                }
                if (theProducts.Count == 1)
                {
                    for (int i = 0; i < (8); i++)
                        GlobalVars.Instance.writeFieldFile("0", "0", "0", "0", parens, 0);
                }
            }
            GlobalVars.Instance.writeFieldFile("CropSeq", "CropSeq", "-", cropSequenceNo, parens, 0);
            GlobalVars.Instance.writeFieldFile("duration", "duration", "days", duration, parens, 0);
            GlobalVars.Instance.writeFieldFile("residueCfromLastCrop", "C in residues from previous crop", "kgC/ha", residueCfromLastCrop, parens, 0);
            GlobalVars.Instance.writeFieldFile("soilCO2_CEmission", "Soil CO2-C emission", "kgC/ha", soilCO2_CEmission, parens, 0);
            GlobalVars.Instance.writeFieldFile("CdeltaSoil", "Change in soil C", "kgC/ha", deltaSoilC, parens, 0);
            GlobalVars.Instance.writeFieldFile("deltaSoilN", "Change in soil N", "kgN/ha", deltaSoilN, parens, 0);
            GlobalVars.Instance.writeFieldFile("residueNfromLastCrop", "N in residues from previous crop", "kgN/ha", residueNfromLastCrop, parens, 0);
            GlobalVars.Instance.writeFieldFile("grazedN", "N grazed", "kgN/ha", grazedN, parens, 0);
            GlobalVars.Instance.writeFieldFile("residueNtoNextCrop", "N in residues passed to next crop", "kgN/ha", residueNtoNextCrop, parens, 0);
            GlobalVars.Instance.writeFieldFile("StartDay", "JulianDay", "JulianDay", getStartLongTime(), parens, 1);
            GlobalVars.Instance.headerField = true;            
        }
    }

    //!  Get LAI on a particular day.
    /*!
     \param index day number as an integer argument.
     \return LAI as a double value.
    */
    public double GetLAI(int index) { return LAI[index]; }
    //!  Get potential evapotranspiration on a particular day
    /*!
     \param day day number as integer.
     \return potential evapotranspiration (mm) a double value.
    */
    public double GetpotentialEvapoTrans(int day) { return potentialEvapoTrans[day]; }
    //!  Get Precipitation on a particular day. 
    /*!
     \param day day number as an integer argument.
     \return Precipitation (mm) a double value.
    */
    public double Getprecipitation(int day) { return precipitation[day]; }
    //!  Get Temperature on a particular day. 
    /*!
     \param day day number as integer
     \return temperature (Celsius) as a double value.
    */
    public double Gettemperature(int day) { return temperature[day]; }
    //!  Get if the crop is irrigated.
    /*!
     \return true if irrigated.
    */
    public bool GetisIrrigated() { return isIrrigated; }
    //!  Get the Start Date of the crop.
    /*!
     \return starting date as a timeClass value.
    */
    public timeClass GettheStartDate() { return theStartDate; }
    //!  Get the end Date of the crop.
    /*!
     \return ending date as a timeClass value.
    */
    public timeClass GettheEndDate() { return theEndDate; }
    //!  Set Irrigation water applied
    /*!
     \param index day number as integer argument.
     \param val  irrigation water (mm) as a double argument.
    */
    public void Setirrigation(int index, double val)
    {
        irrigationWater[index] = val;
    }
    //!  Calculate LAI of the crop on a given day. 
    /*!
     \param dayNo day number as an integer argument.
     \return LAI as a double value.
    */
    public double CalculateLAI(int dayNo)
    {
        if (permanent)
            LAI[dayNo] = maxLAI;
        else
        {
            double maxDM = 0;
            double currentDM = 0;
            double timeIntoCrop = (double)dayNo / (double)duration;
            for (int i = 0; i < theProducts.Count; i++)
            {
                maxDM += theProducts[i].GetPotential_yield();
                //a more sensible way to simulate LAI
                //currentDM = theProducts[i].GetExpectedYield() * (Tsum[i] / totalTsum);
                currentDM += theProducts[i].GetExpectedYield() * timeIntoCrop;
            }
            if (maxDM == 0)
                LAI[dayNo] = 0;
            else
                LAI[dayNo] = maxLAI * currentDM / maxDM;
        }
        return LAI[dayNo];
    }
    //!  Calculate the Rooting Depth. 
    /*!
     \param dayNo as an integer
     \return rooting depth (m) as a double value.
    */
    public double CalculateRootingDepth(int dayNo)
    {
        double rootingDepth = 0;
        if (permanent)
            rootingDepth = MaximumRootingDepth;
        else
        {
            double maxDM = 0;
            double currentDM = 0;
            double timeIntoCrop = (double)dayNo / (double)duration;
            for (int i = 0; i < theProducts.Count; i++)
            {
                maxDM += theProducts[i].GetPotential_yield();
                currentDM += theProducts[i].GetExpectedYield() * timeIntoCrop;
            }
            if (maxDM == 0)
                rootingDepth = 0;
            else
                rootingDepth = MaximumRootingDepth * currentDM / maxDM;
        }
        return rootingDepth;
    }
    //!  Calculate drought factor for the soil for the duration of the crop.
    /*!
     \return drought factor as a double value.
    */
    public double CalculatedroughtFactorSoil()
    {
        double cumDroughtIndex = 0;
        for (int i = 0; i < duration; i++)
        {
            cumDroughtIndex += droughtFactorSoil[i];
        }
        double cropdroughtIndex = cumDroughtIndex / (double)duration;
        return cropdroughtIndex;
    }
    //!  Get Cumulative Precipitation.
    /*!
     \return cumulative precipitation (mm) as a double value.
    */
    public double GetcumulativePrecipitation()
    {
        double cumPrecip = 0;
        for (int i = 0; i < (int)duration; i++)
            cumPrecip += precipitation[i];
        return cumPrecip;
    }
    //!  Get Cumulative Drainage. 
    /*!
     \return Cumulative Drainage (mm) as a double value.
    */
    public double GetcumulativeDrainage()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += drainage[i];
        return cum;
    }
    //!  Get Cumulative Irrigation.
    /*!
     \return Cumulative Irrigation (mm) as a double value.
    */
    public double GetcumulativeIrrigation()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += irrigationWater[i];
        return cum;
    }
    //!  Get Cumulative potential evapotranspiration.
    /*!
     \return Cumulative potential evapotranspiration (mm) as a double value.
    */
    public double GetcumulativepotEvapoTrans()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += potentialEvapoTrans[i];
        return cum;
    }
    //!  Get Cumulative Evaporation.
    /*!
     \return Cumulative evaporation (mm) as a double value.
    */
    public double GetcumulativeEvaporation()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += evaporation[i];
        return cum;
    }
    //!  Get Cumulative Transpiration.
    /*!
     \return Cumulative transpiration (mm) as a double value.
    */
    public double GetcumulativeTranspiration()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += transpire[i];
        return cum;
    }
    //!  Get Average Drought Index Plant
    /*!
     \return Average Drought Index Plant as a double value.
    */
    public double GetAverageDroughtIndexPlant()
    {
        double averageDroughtIndex = 0;
        for (int i = 0; i < duration; i++)
            averageDroughtIndex += droughtFactorPlant[i];
        averageDroughtIndex /= duration;
        return averageDroughtIndex;
    }
    //!  Calculate climate variables. 
    public void CalculateClimate()
    {
        double[] temppotentialEvapoTrans = new double[366];
        double[] tempprecipitation = new double[366];
        double[] temptemperature = new double[366];
        double[] tempTsum = new double[366];
        for (int i = 0; i < 366; i++)
        {
            temppotentialEvapoTrans[i] = 0;
            tempprecipitation[i] = 0;
            temptemperature[i] = 0;
            tempTsum[i] = 0;
        }
        int rainfreeDays = 1;
        double cumPrecip = 0;
        double dailyPrecip = 0;
        timeClass realTime = new timeClass(theStartDate);
        realTime.setDate(1, 1, realTime.GetYear());
        int daycount = 0;
        int maxRainfreeDays = 31;
        int currentMonth = 0;
        //calculate climate, using calendar year as starting point
        for (int i = 0; i < 366; i++)
        {
            if (realTime.GetMonth() > currentMonth)
            {
                double temp = 0;
                if (GlobalVars.Instance.theZoneData.rainDays[realTime.GetMonth() - 1] > 0)
                {
                    temp = realTime.GetDaysInMonth(realTime.GetMonth()) / GlobalVars.Instance.theZoneData.rainDays[realTime.GetMonth() - 1];
                    maxRainfreeDays = (int)Math.Round(temp);
                }
                else
                    maxRainfreeDays = 0;
                double precip = GlobalVars.Instance.theZoneData.Precipitation[realTime.GetMonth() - 1];
                double daysInMonth = (double)realTime.GetDaysInMonth(realTime.GetMonth());
                dailyPrecip = precip / daysInMonth;
                if (realTime.GetMonth() == 1)
                    rainfreeDays = (int)Math.Round(temp / 2);
                currentMonth = realTime.GetMonth();
            }
            temppotentialEvapoTrans[daycount] = GlobalVars.Instance.theZoneData.PotentialEvapoTrans[realTime.GetMonth() - 1];
            if (potentialEvapoTrans[daycount] > 20)
            {
                string messageString = ("Error; potential evapotranspiration for month \n");
                messageString += (daycount.ToString() + " is unrealistically high at " + potentialEvapoTrans[daycount].ToString());
                GlobalVars.Instance.Error(messageString);
            }
            cumPrecip += dailyPrecip;
            if (rainfreeDays >= maxRainfreeDays)
            {
                tempprecipitation[daycount] = cumPrecip;
                cumPrecip = 0;
                rainfreeDays = 1;
            }
            else
                rainfreeDays++;
            //            Console.WriteLine(" k " + k + " precip " + precipitation[k].ToString("F3"));
            temptemperature[daycount] = GlobalVars.Instance.theZoneData.airTemp[realTime.GetMonth() - 1];
            tempTsum[daycount] = GlobalVars.Instance.theZoneData.GetTemperatureSum(temptemperature[daycount], baseTemperature);
            //Console.WriteLine(name + " month " + realTime.GetMonth().ToString() + " day " + daycount.ToString() + " ppt " + tempprecipitation[daycount].ToString("0.00")
//                + " potevap " + temppotentialEvapoTrans[daycount].ToString("0.00"));
              //  + " dailyPrecip " + dailyPrecip.ToString("0.00"));
            daycount++;
            realTime.incrementOneDay();
        }

        //map climate based on calendar year, onto climate based on crop period
        timeClass clockit = new timeClass(theStartDate);
        int k = 0;
        k = clockit.getJulianDay()-1;

        for (int i = 0; i < 366; i++)
        {
            if (i < k)
                daycount = i + 366 - k;
            else
                daycount = i - k;
            potentialEvapoTrans[daycount] = temppotentialEvapoTrans[i];
            precipitation[daycount] = tempprecipitation[i];
            temperature[daycount] = temptemperature[i];
            Tsum[daycount]=tempTsum[i];
        }
        double temp2 = GetcumulativepotEvapoTrans();
        double temp1 = GetcumulativePrecipitation();
    }
    //!  Calculate Leaching and Uptake.
    /*!
     \param mineralNFromLastCrop mineral N available from the previous crop (kg/ha) as double argument.
     \param evenNsupply mineral N from the soil (kg/ha) spread evenly over the crop duration.
     \param relGrowth relative growth rate as a double argument.
    */
    public void CalculateLeachingAndUptake(double mineralNFromLastCrop, double evenNsupply, ref double relGrowth, 
        ref double mineralNsurplus)
    {
        double averagNitrificationInhibition=CalculateNitrificationInhibitor();
        double maxCropNuptake = CalculateMaxCropNUptake();
        if (zeroGasEmissionsDebugging)
        {
            soilN2OEmissionFactor = 0;
            manureN2OEmissionFactor = 0;
            fertiliserN2OEmissionFactor = 0;
        }
        else
        {
            soilN2OEmissionFactor = (1-averagNitrificationInhibition) * GlobalVars.Instance.theZoneData.getsoilN2OEmissionFactor(); ;
            manureN2OEmissionFactor = (1 - averagNitrificationInhibition) * GlobalVars.Instance.theZoneData.getmanureN20EmissionFactor();
            fertiliserN2OEmissionFactor = (1 - averagNitrificationInhibition) * GlobalVars.Instance.theZoneData.getfertiliserN20EmissionFactor();
        }

        modelledCropNuptake = 0;
        nitrateLeaching = 0;
        manureN2OEmission = 0;
        fertiliserN2OEmission = 0;
        Nfixed = 0;

        if (evenNsupply>=0.0)
        {
            soilN2OEmission = soilN2OEmissionFactor * evenNsupply;
            N2Nemission = soilN2OEmission * soilN2Factor;
        }
        else
        {
            soilN2OEmission = 0.0;
            N2Nemission = 0.0;
        }
        double Ninp = mineralNFromLastCrop + evenNsupply;
        evenNsupply -= soilN2OEmission + N2Nemission;
        double Nout = 0;
        double cumdrainage = 0;
        evenNsupply /= duration; //added evenly over duration

        mineralNavailable= mineralNFromLastCrop;
        double N2ON = 0;
        double N2N = 0;
        for (int i = 0; i < duration; i++)
        {
            double NleachingToday = 0;
            double fixationToday = 0;
            if ((!zeroLeachingDebugging)&&(mineralNavailable>0))
                NleachingToday=(1 - nitrificationInhibitor[i]) * mineralNavailable * drainage[i] / soilWater[i];
            cumdrainage += drainage[i];
            double NuptakeToday = 0;
            if (mineralNavailable>0)
                NuptakeToday=mineralNavailable * (1 - droughtFactorPlant[i]);
            if (NuptakeToday < 0)
                Console.Write("");
            double maxDailyCropNuptake = 0;
            if ((Tsum[i] > 0)&&(maxCropNuptake>0))
                maxDailyCropNuptake = (Tsum[i] / totalTsum) * maxCropNuptake;
            if (NuptakeToday > maxDailyCropNuptake)
                NuptakeToday = maxDailyCropNuptake;
          /*  Console.WriteLine("i " + i.ToString() + " drain " + drainage[i].ToString("0.00") + " leach " + 
                NleachingToday.ToString("0.00") + " min " + mineralNavailable.ToString("0.00") + " Nup " + NuptakeToday.ToString("0.00"));*/
            if (mineralNavailable > 0)
            {
                if ((NleachingToday + NuptakeToday) > mineralNavailable)
                {
                    NleachingToday *= mineralNavailable / (NleachingToday + NuptakeToday);
                    NuptakeToday *= mineralNavailable / (NleachingToday + NuptakeToday);
                }
            }
            dailyNitrateLeaching[i] = NleachingToday;
            nitrateLeaching += NleachingToday;
            modelledCropNuptake += NuptakeToday;
            if (NuptakeToday < 0)
                Console.WriteLine();
            if (((maxDailyCropNuptake - NuptakeToday) > 0) && (NfixationFactor > 0))
            {
                fixationToday = GetNfixation(maxDailyCropNuptake - NuptakeToday) * (1 - droughtFactorPlant[i]);
                Ninp += fixationToday;
            }
            modelledCropNuptake += fixationToday;
            Nfixed += fixationToday;
            Nout += NuptakeToday + NleachingToday + fixationToday;
            mineralNavailable += evenNsupply - (NleachingToday + NuptakeToday);
            if (fertiliserN[i] > 0)
            {
                Ninp += fertiliserN[i];
                N2ON = fertiliserN2OEmissionFactor * fertiliserN[i];
                fertiliserN2OEmission += N2ON;
                N2N = N2ON * soilN2Factor;
                N2Nemission += N2N;
                mineralNavailable += fertiliserN[i] - (N2ON + N2N);
            }
            if (manureTAN[i] > 0)
            {
                Ninp += manureTAN[i];
                N2ON = manureN2OEmissionFactor * manureTAN[i];
                manureN2OEmission += N2ON;
                N2N = N2ON * soilN2Factor;
                N2Nemission += N2N;
                mineralNavailable += manureTAN[i] - (N2ON + N2N);
            }
            if (N2ON < 0.0)
                Console.WriteLine();
        }
        if (modelledCropNuptake > maxCropNuptake)
        {
            mineralNavailable += modelledCropNuptake - maxCropNuptake;
            modelledCropNuptake = maxCropNuptake;
        }
        if (maxCropNuptake > 0) //not bare soil
            relGrowth = modelledCropNuptake / maxCropNuptake;
        else
            relGrowth = 0;
//        if (nitrateLeaching > 0)
  //          Console.Write("drain " + cumdrainage.ToString());
        GlobalVars.Instance.log(" Cum potevap " + GetcumulativepotEvapoTrans().ToString("F6") + " Cum ppt " + GetcumulativePrecipitation().ToString("F6")
            + " evap " + GetcumulativeEvaporation().ToString("F6") + " ave drought " + GetAverageDroughtIndexPlant() + " cum drain " + GetcumulativeDrainage().ToString("F6"),5);
        mineralNsurplus = mineralNavailable;
        N2ONemission = fertiliserN2OEmission + manureN2OEmission + soilN2OEmission;
        Nout += N2ONemission + N2Nemission;
        double balance = Ninp - (Nout + mineralNavailable);
    }
    //!  Get Relative N Uptake.
    /*!
     \return the maximum N uptake, relative to the modelled N uptake as a double value.
    */
    public double GetRelativeNuptake()
    {
        double ret_val = 0;
        double maxNuptake = CalculateMaxCropNUptake();
        if (modelledCropNuptake > 0)
            ret_val = maxNuptake / modelledCropNuptake;
        else
            ret_val = 0;
        return ret_val;
    }
    //!  Add a one-time input of organic matter.
    /*!
     \params feedCode feed code of the feed item to be added
    \params Anamount the amount expressed as kg dry matter
    */
    public feedItem AddOne_timeOrganicMatter(int feedCode, double Anamount)
    {
        feedItem anOMsource = new feedItem();
        anOMsource.GetStandardFeedItem(feedCode);
        anOMsource.Addamount(Anamount);
        return anOMsource;
    }
}
