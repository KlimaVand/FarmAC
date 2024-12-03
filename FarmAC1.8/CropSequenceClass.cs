using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using simplesoilModel;
/*! The CropSequenceClass contains a list of the crops in a single sequence
 * A crop sequence represents in principle the sequence of crops on a single field but is normally used to represent a typical crop rotation
 * The crop sequence must be closed i.e. the last date of a sequence must be on the day and month before the first date of the sequence
 * This is necessary, since the model will normally need to repeat the sequence serially
 * Bare soil is in this context, considered to be a crop
 * */
public class CropSequenceClass
{
    //inputs
    //! Name of the crop sequence
    string name;
    //! soil type identifier of this sequence
    string soilType;
    //! type of farm (used in initialising soil C pools)
    int FarmType;
    //! area of the sequence (ha)
    double area;
    //parameters 

    //!length of rotation in years
    int lengthOfSequence; 
    //! Initial amount of mineral N in the soil (kg/ha)
    double startsoilMineralN;
    //! instance of the soil model that represents the situation at the start of the daily estimation of crop growth
    public ctool2 startSoil;
    //! path for the file containing input data
    string path;
    int identity;
    //! number of times to repeat this sequence of crops
    int repeats;
    //! number of crops in this sequence
    int numCropsInSequence = 0;
    //! list of instances of CropClass in this sequence
    List<CropClass> theCrops = new List<CropClass>();
    //! For xml output
    public XElement node = new XElement("data");
    //! Initial amount of C in the soil (kg)
    double initialSoilC = 0;
    //! Initial amount of N in the soil (kg)
    double initialSoilN = 0;
    //! Final amount of C in the soil (kg)
    double finalSoilC = 0;
    //! Final amount of N in the soil (kg)
    double finalSoilN = 0;
    //! Emission of C as CO2 from the soil (kg)
    double soilCO2_CEmission = 0;
    //! C leached from the crop sequence (kg)
    double Cleached = 0;
    //! Cumulative C input to the crop sequence (kg)
    double CinputToSoil = 0;
    //! Daily C input to the crop sequence (kg)
    double[] dailyCinputToSoil;
    //! Change in C in the soil over the duration of the crop sequence(s) (kg)
    double CdeltaSoil = 0;
    //! C remaining on the soil as crop residues (kg)
    double residueCremaining = 0;
    //! N remaining on the soil as crop residues (kg)
    double residueNremaining = 0;
    //! N input to the soil in the crop sequence (kg)
    double NinputToSoil = 0;
    //! Mineralised soil N (kg)
    double mineralisedSoilN = 0;
    //! Change in N in the soil over the duration of the crop sequence(s) (kg)
    double NdeltaSoil = 0;
    //! the soil water model
    simpleSoil thesoilWaterModel;
    //! soil type (Danish classes)
    private int soiltypeNo = 0;
    //!
    private int soilTypeCount = 0;
    //! Instance of C-Tool v2 model 
    public ctool2 aModel;
    private string parens; /*!< a string containing information about the farm and scenario number.*/
    //! Get the name of the sequence
    /*!
      \return the name of the crop sequences as a string. 
    */

    public string Getname() { return name; }
    //! Get the list of crops in the sequence
    /*!
      \return the list of CropClass in the crop sequences. 
    */
    public List<CropClass> GettheCrops() { return theCrops; }
    //!  Get soil C stored in the sequence.
    /*!
     \return soil C stored (kg)
    */
    public double GetCStored() { return aModel.GetCStored() * area; }
    //!  Get soil N stored in the sequence.
    /*!
     \return soil N stored (kg)
    */
    public double GetNStored() { return aModel.GetNStored() * area; }
    //!  Get Initialial Soil N. 
    /*!
     \return Initialial Soil N (kg) as 
    */
    public double GetinitialSoilN() { return initialSoilN; }
    //!  Get Soil CO2_CEmission. 
    /*!
     \return Soil CO2 C Emission (kg)
    */
    public double GetsoilCO2_CEmission() { return soilCO2_CEmission; }
    //!  Get change in C in soil. 
    /*!
     \return change in C in soil (kg)
    */
    public double GetCdeltaSoil() { return CdeltaSoil; }
    //!  Get C leached from soil. 
    /*!
     \return C leached from soil (kg)
    */
    public double GetCleached() { return Cleached; }
    //!  Get C Input to Soil. 
    /*!
     \return C Input to Soil (kg)
    */
    public double GetCinputToSoil() { return CinputToSoil; }
    //!  Get Soil Type No. 
    /*!
     \return Soil Type No as an integer value.
    */
    public int GetsoiltypeNo() { return soiltypeNo; }
    //!  Get Soil Type Count. 
    /*!
     \return Soil Type Count as an integer value.
    */
    public int GetsoilTypeCount() { return soilTypeCount; }
    //!  Set Soil Type Count. 
    /*!
     \param aVal an integer argument.
    */
    public void SetsoilTypeCount(int aVal) { soilTypeCount = aVal; }
    //!  Get the Nitrate Leaching for the whole crop sequence. 
    /*!
     \return Nitrate Leaching (kg N)
    */
    public double GettheNitrateLeaching()
    {
        return GettheNitrateLeaching(theCrops.Count);
    }
    //!  Get the Nitrate Leaching for the crop sequence up until crop number maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return the Nitrate Leaching (kg N)
    */
    public double GettheNitrateLeaching(int maxCrops)
    {
        double Nleached = 0;
        for (int i = 0; i < maxCrops; i++)
            Nleached += theCrops[i].GetnitrateLeaching() * area;
        return Nleached;
    }
    //!  Get number of times to repeat this crop sequence.
    /*!
     \return number of times to repeat this crop sequence as an integer value.
    */
    public int Getrepeats() { return repeats; }
    //!  Get Start Soil Mineral N. 
    /*!
     \return Start Soil Mineral N (kg)
    */
    public double GetstartsoilMineralN() { return startsoilMineralN * area; }
    //!  Get Length of crop Sequence. 
    /*!
     \return Length of crop Sequence (years) as an integer value.
    */
    public int GetlengthOfSequence() { return lengthOfSequence; }
    //!  Get Residue N Remaining on the surface at the end of the sequence. 
    /*!
     \return Residue N Remaining on the surface (kg)
    */
    public double GetresidueNremaining() { return residueNremaining; }
      //!  Get Residue C Remaining. 
    /*!
     \return 
    */
    public double GetresidueCremaining() { return residueCremaining; }
    //!  Get N Input in crop residues. 
    /*!
     \return N Input in crop residues (kg)
    */
    public double GetresidueNinput()
    {
        double ret_val = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            ret_val += aCrop.GetresidueNinput();
        }
        ret_val += GetresidueNremaining();
        return ret_val * area;
    }
    //!  Get N Uptake by crops. 
    /*!
     \return N Uptake by crops (kg)
    */
    public double GetCropNuptake()
    {
        double ret_val = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            ret_val += aCrop.GetCropNuptake() * area;
        }
        return ret_val;
    }
    //!  Get emission of C from fertiliser C. 
    /*!
     \return emission of C from fertiliser C (kg)
    */
    public double GetFertiliserC()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetFertiliserC() * area;
        }
        return retVal;
    }
    //! A constructor with six arguments.
    /*!
     \param aPath path to the input file.
     \param aID an integer argument.
     \param currentFarmType the farm type as an integer argument.
     \param aparens, a string containing information about the farm and scenario number.
     \param asoilTypeCount, an integer argument.
    */
    public CropSequenceClass(string aPath, int aID, int zoneNr, int currentFarmType, string aparens, int asoilTypeCount)
    {
        parens = aparens;
        path = aPath;
        identity = aID;
        FarmType = currentFarmType;
        FileInformation rotation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        path += "(" + identity.ToString() + ")";
        rotation.setPath(path);
        name = rotation.getItemString("NameOfRotation");
        area = rotation.getItemDouble("Area");
        soilType = rotation.getItemString("SoilType");
        soilTypeCount = asoilTypeCount;
        //Get the crops in the sequence
        string crop = path + ".Crop";
        rotation.setPath(crop);
        int min = 99; int max = 0;
        rotation.getSectionNumber(ref min, ref max);
        for (int i = min; i <= max; i++)
        {
            if (rotation.doesIDExist(i))
            {
                GlobalVars.Instance.log("CropSequenceClass constructor, sequence number " +aID.ToString() + " entering CropClass constructor " + " crop " + name.ToString(), 6);
                CropClass aCrop = new CropClass(crop, i, zoneNr, name, area);
                aCrop.SetcropSequenceNo(identity);
                theCrops.Add(aCrop);
            }
        }
        //check for gaps in crop sequence (start of new crop must be one day after the end of the previous crop)
        long startLongTime=0;
        long endLongTime=0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            if (i == 0)
                endLongTime = aCrop.getEndLongTime();
            else
            {
                startLongTime = aCrop.getStartLongTime();
                long handover = startLongTime - endLongTime;
                if (handover!= 1)  //
                {
                    string messageString = ("Error - gap in cropping sequence number " + identity.ToString() + " between crop " + (i-1).ToString() + " and crop " + i.ToString());
                    GlobalVars.Instance.Error(messageString);
                }
                else
                    endLongTime = aCrop.getEndLongTime();
            }
        }

        //check to ensure that the end of the crop sequence is exactly one or more years after it start (this code might be simplified in the future)
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            if (i == theCrops.Count - 1) //true if this is the last (or only) crop
            {
                long adjustedStartTime;
                long adjustedEndTimeThisCrop;
                if (theCrops.Count == 1) //only one crop
                {
                    adjustedStartTime = aCrop.getStartLongTime();
                    adjustedEndTimeThisCrop = aCrop.getEndLongTime();
                }
                else
                {
                    adjustedStartTime = theCrops[0].getStartLongTime();
                    adjustedEndTimeThisCrop = theCrops[i].getEndLongTime();
                }
                long numDays = adjustedEndTimeThisCrop - adjustedStartTime + 1;
                if (numDays < 365)
                {
                    string messageString = ("Error - cropping sequence number " + identity.ToString() + " is less than one year");
                    GlobalVars.Instance.Error(messageString);
                }
                long mod = numDays % 365;
                if (Math.Abs(mod) > 1)
                {
                    string messageString = ("Error - gap at end of cropping sequence number " + identity.ToString());
                    GlobalVars.Instance.Error(messageString);
                }
            }
        }

        lengthOfSequence = calculatelengthOfSequence();  //calculate length of the input sequence in years
        if (GlobalVars.Instance.WriteCrop)  
        {
            GlobalVars.Instance.WriteCropFile("Sequence_name", "Name of crop sequence", name, true, false);
            GlobalVars.Instance.WriteCropFile("Area", "ha", area, true, false);
            for (int i = 0; i < theCrops.Count; i++)
            {
                theCrops[i].WriteXls();
            }
            GlobalVars.Instance.WriteCropFile("", "", "", false, true);
        }
        //If the duration of the simulation period is greater than the duration of the crop sequence, the crop sequence will need to be repeated x number of times
        //This requires that the crop sequence is copied and loaded into a linear list, starting with the first crop of the sequence and finishing with the last crop of the last repeat
        //e.g. sequence crop 1, crop2, crop 3 repeated three times needs to be listed as crop 1, crop2, crop 3, crop 1, crop2, crop 3, crop 1, crop2, crop 3.
        //If the duration of the simulation period is not an exact multiple of the duration of the sequence, the simulation period is extended until it is
        List<CropClass> CopyOfPlants = new List<CropClass>();
        for (int i = 0; i < theCrops.Count; i++)
        {
            double duration = theCrops[i].CalcDuration();
            if (duration == 0)
            {
                string messageString = ("Error - crop number " + i.ToString() + " in sequence " + name);
                messageString += (": duration of crop cannot be zero");
                GlobalVars.Instance.Error(messageString);
            }
            if ((duration > 366) && (duration % 365 != 0))
            {
                string messageString = ("Error - crop number " + i.ToString() + " in sequence " + name);
                messageString += (": crops lasting more than one year must last an exact number of years");
                GlobalVars.Instance.Error(messageString);
            }
            int durationInYears = (int)duration / 365;
            if (durationInYears > 1)     //need to clone for one or more years, if crop persists for more than one year
            {
                CropClass aCrop = theCrops[i];
                if ((aCrop.GetStartDay() == 1) && (aCrop.GetStartMonth() == 1))
                    aCrop.SetEndYear(aCrop.GetStartYear());
                else
                    aCrop.SetEndYear(aCrop.GetStartYear() + 1);
                aCrop.CalcDuration();

                for (int j = 1; j < durationInYears; j++)
                {
                    {
                        aCrop = new CropClass(theCrops[i]);
                        aCrop.SetStartYear(j + theCrops[i].GetStartYear());
                        if ((theCrops[i].GetStartDay() == 1) && (theCrops[i].GetStartMonth() == 1))
                            aCrop.SetEndYear(j + theCrops[i].GetStartYear());
                        else
                            aCrop.SetEndYear(j + theCrops[i].GetStartYear() + 1);
                        theCrops.Insert(j + i, aCrop);
                    }
                }

            }
        }
        numCropsInSequence = theCrops.Count;
        AdjustDates(theCrops[0].GetStartYear());    //this converts from calendar year to zero base e.g. 2010 to 0, 2011 to 1 etc
        int length = 0;
        if (GlobalVars.Instance.reuseCtoolData == -1)
            length = GlobalVars.Instance.GetadaptationTimePeriod();
        else
            length = GlobalVars.Instance.GetminimumTimePeriod();
        int startYr;  //year in which the crop starts (zero base)
        int endYr;  //year in which the crop ends (zero base)
        if ((theCrops[0].GetEndYear() > theCrops[0].GetStartYear()) == false && (theCrops[0].getEndLongTime() - theCrops[0].getStartLongTime()) == 364)  //only true if crop lasts 364 days
        {
            startYr = lengthOfSequence;
            endYr = lengthOfSequence;
        }
        else
        {
            startYr = lengthOfSequence + 1;
            endYr = lengthOfSequence + 1;
        }
        //the following code works out how many times the crop sequence will be repeated and then loads all the instances of CropClass into the CopyOfPlants list
        repeats = (int)Math.Ceiling(((double)length) / ((double)lengthOfSequence));//number of times to repeat this sequence of crops
        for (int j = 0; j < repeats - 1; j++)
        {
            //Go through the list of crops however many times as necessary, creating clones of the CropClass and loading them into CopyOfPlants list
            //Adjust the start and end date information, so the crops in CopyOfPlants appear sequentially
            for (int i = 0; i < theCrops.Count; i++)
            {
                CropClass newClass = new CropClass(theCrops[i]);
                int been = 0;
                int cropStartYr = theCrops[i].GetStartYear();
                int cropEndYr = theCrops[i].GetEndYear();
                if (cropEndYr > cropStartYr)
                {
                    endYr++;
                    been = 1;
                }
                if ((cropEndYr > cropStartYr) == false && (theCrops[0].getEndLongTime() - theCrops[0].getStartLongTime()) == 364)
                {
                    endYr++;
                    startYr++;
                    been = 2;
                }
                if (i > 0 && theCrops[i - 1].GetEndYear() == cropEndYr)
                {
                    if (been == 1)
                        endYr--;
                    if (been == 2)
                    {
                        endYr--;
                        startYr--;
                    }
                }
                if (been == 0 && theCrops[i].GetStartDay() == 1 && theCrops[i].GetStartMonth() == 1)
                {
                    endYr++;
                    startYr++;
                }
                newClass.SetStartYear(startYr);
                newClass.SetEndYear(endYr);
                //Need to adjust the dates of any fertilizer and manure applications
                for (int k = 0; k < newClass.manureApplied.Count; k++)
                {
                    if (newClass.manureApplied[k].applicdate.GetMonth() < newClass.GetStartMonth())
                        newClass.manureApplied[k].applicdate.SetYear(startYr + 1);
                    else
                        newClass.manureApplied[k].applicdate.SetYear(startYr);
                }
                for (int k = 0; k < newClass.fertiliserApplied.Count; k++)
                {
                    if (newClass.fertiliserApplied[k].applicdate.GetMonth() < newClass.GetStartMonth())
                        newClass.fertiliserApplied[k].applicdate.SetYear(startYr + 1);
                    else
                        newClass.fertiliserApplied[k].applicdate.SetYear(startYr);
                }
                if (theCrops[i].GetEndYear() > theCrops[i].GetStartYear())
                    startYr++;
                CopyOfPlants.Add(newClass);
            }
        }
        //Add the new sequence of crops onto the end of the theCrop list
        for (int i = 0; i < CopyOfPlants.Count; i++)
        {
            CropClass acrop = CopyOfPlants[i];
            theCrops.Add(acrop);
        }
        //End of loading the crops

        for (int i = 0; i < theCrops.Count; i++)
        {
            theCrops[i].Updateparens(parens + "_CropClass" + (i + 1).ToString(), i);
        }
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            aCrop.setArea(area);
        }
        lengthOfSequence = calculatelengthOfSequence();  //recalculate length of sequence in years

        //get the parameters for the sequence
        getparameters(zoneNr);
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            aCrop.SetlengthOfSequence(lengthOfSequence);
        }
        //calculate the climate variables for each crop
        for (int i = 0; i < theCrops.Count; i++)
            theCrops[i].CalculateClimate();
    }

    public void InitialiseSoilCN(bool doSpinup)
    {
        //Now sort out the soil modelling
        thesoilWaterModel = new simpleSoil();
        //This creates an instance of the modified version of the CTool model
        aModel = new ctool2(parens + "_1");
        //Make sure the soil type information is present in the zonal data
        soiltypeNo = -1;
        for (int i = 0; i < GlobalVars.Instance.theZoneData.thesoilData.Count; i++)
        {
            if (GlobalVars.Instance.theZoneData.thesoilData[i].name.CompareTo(soilType) == 0)
                soiltypeNo = i;
        }
        if (soiltypeNo == -1)
        {
            string messageString = ("Error - could not find soil type " + soilType + " in parameter file\n");
            messageString += ("Crop sequence name = " + name);
            GlobalVars.Instance.Error(messageString);
        }
        //Check to see if the crop rooting depth will be limited by soil depth (and issue a warning)
        double maxSoilDepth = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].GetSoilDepth();
        bool doneOnce = false;
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass aCrop = theCrops[i];
            if (aCrop.GetMaximumRootingDepth() > maxSoilDepth)
            {
                aCrop.SetMaximumRootingDepth(maxSoilDepth);
                if (!doneOnce)
                {
                    string messageString = ("Warning - crop rooting depth limited by soil depth " + aCrop.Getname() + "\n");
                    messageString += ("Crop sequence name = " + name);
                    GlobalVars.Instance.log(messageString, 6);
                    doneOnce = true;
                }
            }
        }

        //! initial total C in the soil (used for spinning up the soil C model)
        double initialC = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].initialC;
        //! initial FOM C input (used for spinning up the soil C model)
        double initialFOM_Cinput = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].InitialFOM;
        //! Initial C:N ratio of the fresh organic matter
        double InitialFOMCtoN = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].InitialFOMCtoN;
        //!Clay fraction of the soil
        double ClayFraction = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].ClayFraction;
        //!Damping depth (m) to be used in calculating the soil temperature at different depths in the soil
        double dampingDepth = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].GetdampingDepth();
        //! Proportion of the organic matter that is initially present as humic organic matter in the upper soil layer
        double pHUMupperLayer = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].pHUMupperLayer;
        //! Proportion of the organic matter that is initially present as humic organic matter in the lower soil layer
        double pHUMlowerLayer = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].pHUMlowerLayer;
        //! C:N ratio of the humic and resistent organic mattr
        double InitialCtoN = GlobalVars.Instance.theZoneData.thesoilData[soiltypeNo].theC_ToolData[FarmType - 1].InitialCtoN;
        //!Average air temperature (used for spinning up)
        double[] averageAirTemperature = GlobalVars.Instance.theZoneData.airTemp;
        //!The number of days that the 
        int offset = GlobalVars.Instance.theZoneData.GetairtemperatureOffset();
        double amplitude = GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude();
        double mineralNFromSpinup = 0;
        //! Initialize the soil organic matter model
        if (GlobalVars.Instance.GetlockSoilTypes())     //!if true, the C-TOOL pools for each crop sequence will be preserved but the areas must not change. If false, pools within a soil type will be merged and areas can change.            
            aModel.Initialisation(true, identity, soiltypeNo, ClayFraction, offset, amplitude, maxSoilDepth, dampingDepth, initialC,
                GlobalVars.Instance.getConstantFilePath(), GlobalVars.Instance.GeterrorFileName(), InitialCtoN,
                pHUMupperLayer, pHUMlowerLayer, ref mineralNFromSpinup);
        else
            aModel.Initialisation(false, identity, soiltypeNo, ClayFraction, offset, amplitude, maxSoilDepth, dampingDepth, initialC,
            GlobalVars.Instance.getConstantFilePath(), GlobalVars.Instance.GeterrorFileName(), InitialCtoN,
            pHUMupperLayer, pHUMlowerLayer, ref mineralNFromSpinup);
        if (doSpinup)
        {
            // spin up the soil model
            spinup(ref mineralNFromSpinup, initialFOM_Cinput, InitialFOMCtoN, averageAirTemperature, identity);
            startsoilMineralN = mineralNFromSpinup;
        }
        initialSoilC = GetCStored();//value in kg per crop sequence
        initialSoilN = GetNStored();//value in kgN per crop sequence

        //Initialise the soil water model
        double currentRootingDepth = 0;
        double currentLAI = 0;
        if (theCrops[0].Getpermanent())
        {
            currentRootingDepth = theCrops[0].GetMaximumRootingDepth();
            currentLAI = 3.0; //assume a reasonable crop LAI
        }
        else
        {
            currentRootingDepth = 0;
            currentLAI = 0;
        }
        double[] layerOM;
        layerOM = new double[2];
        layerOM[0] = aModel.GetOrgC(0);
        layerOM[1] = aModel.GetOrgC(1);
        thesoilWaterModel.Initialise2(soiltypeNo, theCrops[0].GetMaximumRootingDepth(), currentRootingDepth, currentLAI, layerOM);
        dailyCinputToSoil = new double[lengthOfSequence * 365];
    }

    //!  Get crop sequence parameters. 
    /*!
     \param zoneNR agroecological zone as an integer argument.
    */
    public void getparameters(int zoneNR)
    {
        //If this only finds one soil factor, it could be done elsewhere
        double soilN2Factor = 0;
        bool gotit = false;
        int max = GlobalVars.Instance.theZoneData.thesoilData.Count;
        for (int i = 0; i < max; i++)
        {
            string soilname = GlobalVars.Instance.theZoneData.thesoilData[i].name;
            if (soilname == soilType)
            {
                soilN2Factor = GlobalVars.Instance.theZoneData.thesoilData[i].N2Factor;
                for (int j = 0; j < theCrops.Count; j++)
                {
                    CropClass aCrop = theCrops[j];
                    aCrop.setsoilN2Factor(soilN2Factor);
                }
                gotit = true;
                break;
            }
        }
        if (gotit == false)
        {

            string messageString = ("Error - could not find soil type " + soilType + " in parameter file\n");
            messageString += ("Crop sequence name = " + name);
            GlobalVars.Instance.Error(messageString);
        }
    }
   
    //!  Adjust Dates. Adjust the crop dates so that the first year is year 1 rather than calendar yearTaking one argument.
    /*!
     \param firstYear an integer argument.
    */
    private void AdjustDates(int firstYear)
    {
        for (int i = 0; i < theCrops.Count; i++)
            theCrops[i].AdjustDates(firstYear);
    }
    //!  Get the area of the sequence. 
    /*!
     \return the area (ha)
    */
    public double getArea() { return area; }
    //!  Get C fixed by crops in the sequence. 
    /*!
     \return C fixed by crops (kg)
    */
    public double getCFixed()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].getCFixed() * area;
        }

        return result;
    }
    //!  Get C in harvested crop products. 
    /*!
     \return C in harvested crop products (kg)
    */
    public double getCHarvested()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetharvestedC() * area;
        }
        return result;
    }
    //!  Get dry matter in harvested crop products. 
    /*!
     \return dry matter in harvested crop products (kg)
    */
    public double getDMHarvested()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetharvestedDM() * area;
        }

        return result;
    }
    //!  Get C in grazed crop products. 
    /*!
     \return C in grazed crop products (kg)
    */
    public double getGrazedC()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetgrazedC() * area;
        }

        return result;
    }
    //!  Get C in crop residues. 
    /*!
     \return  C in crop residues (kg)
    */
    public double getCropResidueCarbon()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += (theCrops[i].GetsurfaceResidueC() + theCrops[i].GetsubsurfaceResidueC()) * area;
        }
        return result;
    }
    //!  Get C in CO2 from burnt crop residues. 
    /*!
     \return C in CO2 from burnt crop residues (kg)
    */
    public double getBurntResidueCO2C()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningCO2C() * area;
        }
        return result;
    }
    //!  Get C in CO from burnt crop residues. 
    /*!
     \return C in CO from burnt crop residues (kg)
    */
    public double getBurntResidueCOC()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningCOC() * area;
        }
        return result;
    }
    //!  Get C in black carbon from burnt crop residues. 
    /*!
     \return  C in black carbon from burnt crop residues (kg)
    */
    public double getBurntResidueBlackC()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningBlackC() * area;
        }
        return result;
    }
    //!  Get C in methane emitted during grazing. 
    /*!
     \return C in methane emitted during grazing (kg)
    */
    public double getGrazingMethaneC()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetgrazingCH4C() * area;
        }
        return result;
    }
    //!  Get N lost through burning of crop residues. 
    /*!
     \return N lost through burning of crop residues (kg)
    */
    public double getBurntResidueN()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburntResidueN() * area;
        }
        return result;
    }
    //!  Get N in N2O lost through burning of crop residues. 
    /*!
     \return N in N2O lost through burning of crop residues (kg)
    */
    public double getBurntResidueN2ON()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningN2ON() * area;
        }
        return result;
    }
    //!  Get N in NH3 lost through burning of crop residues. 
    /*!
     \return N in NH3 lost through burning of crop residues (kg)
    */
    public double getBurntResidueNH3N()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningNH3N() * area;
        }
        return result;
    }
    //!  Get N in other forms lost through burning of crop residues. 
    /*!
     \return N in other forms lost through burning of crop residues (kg)
    */
    public double getBurntResidueOtherN()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningOtherN() * area;
        }
        return result;
    }
    //!  Get N in NOx lost through burning of crop residues. 
    /*!
     \return N in NOx lost through burning of crop residues (kg)
    */
    public double getBurntResidueNOxN()
    {
        double result = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            result += theCrops[i].GetburningNOxN() * area;
        }
        return result;
    }
    //!  Get C lost through degradation in crop storage. 
    /*!
     \return C lost through degradation in crop storage (kg)
    */
    public double getProcessStorageLossCarbon()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetstorageProcessingCLoss() * area;
        }
        return retVal;
    }
    //!  Get N lost through degradation in crop storage. 
    /*!
     \return N lost through degradation in crop storage (kg)
    */
    public double getProcessStorageLossNitrogen()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetstorageProcessingNLoss() * area;
        }
        return retVal;
    }
    //!  Get total crop dry matter yield. 
    /*!
     \return total crop dry matter yield (kg)
    */
    public double GetDMYield()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetDMYield() * area;
        }
        return retVal;
    }
    //!  Get utilised dry matter yield. 
    /*!
     \return utilised dry matter yield (kg)
    */
    public double GetUtilisedDMYield()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetUtilisedDM() * area;
        }
        return retVal;
    }
    //!  Get Fertiliser N Applied for the crop sequence up until crop number maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return fertiliser N applied (kg/ha)
    */
    public double GetFertiliserNapplied(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
            retVal += theCrops[i].GetFertiliserNapplied() * area;
        return retVal;
    }
    //!  Get Manure N Applied for the whole crop sequence. 
    /*!
     \return manure N applied (kg)
    */
    public double GetManureNapplied()
    {
        return GetManureNapplied(theCrops.Count);
    }
    //!  Get Manure N Applied for the crop sequence up until crop number maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return Manure N Applied (kg)
    */
    public double GetManureNapplied(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
            retVal += theCrops[i].GetManureNapplied() * area;
        return retVal;
    }
    //!  Get Fertiliser N2O-N Emissions for the whole crop sequence. 
    /*
     \return Fertiliser N2O-N Emission (kg)
    */
    public double GetfertiliserN2ONEmissions()
    {
        return GetfertiliserN2ONEmissions(theCrops.Count);
    }
    //!  Get Fertiliser N2O N Emission  for the crop sequence up until crop number maxCrops. 
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return Fertiliser N2O N Emission (kg)
    */
    public double GetfertiliserN2ONEmissions(int maxCrops)
    {
        double fertiliserN2OEmission = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            fertiliserN2OEmission += theCrops[i].GetfertiliserN2ONEmission() * area;
        }
        return fertiliserN2OEmission;
    }
    //!  Get Manure N2O-N Emission for the whole crop sequence. 
    /*
     \return Manure N2O-N Emission (kg)
    */
    public double GetmanureN2ONEmissions()
    {
        return GetmanureN2ONEmissions(theCrops.Count);
    }
    //!  Get Manure N2O N Emissiions  for the crop sequence up until crop number maxCrops. 
    /*!
     \param maxCrops  maximum number of crops to include in the calculation
     \return Manure N2O N Emissiions (kg)
    */
    public double GetmanureN2ONEmissions(int maxCrops)
    {
        double manureN2OEmission = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            manureN2OEmission += theCrops[i].GetmanureN2ONEmission() * area;
        }
        return manureN2OEmission;
    }
    //!  Get Soil N2O-N Emission for the whole crop sequence
    /*
     \return soil N2O N emission (kg)
    */
    public double GetsoilN2ONEmissions()
    {
        return GetsoilN2ONEmissions(theCrops.Count);
    }
    //!  Get Soil N2O N Emissions for the crop sequence up to maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return Soil N2O N Emissions (kg)
    */
    public double GetsoilN2ONEmissions(int maxCrops)
    {
        double soilN2OEmission = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            soilN2OEmission += theCrops[i].GetsoilN2ONEmission() * area;
        }
        return soilN2OEmission;
    }
    //!  Get Soil N2N Emission for the whole crop sequence
    /*
     \return Soil N2-N Emission (kg)
    */
    public double GetsoilN2NEmissions()
    {
        return GetsoilN2NEmissions(theCrops.Count);
    }
    //!  Get Soil N2N Emissionos for crops up to maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return Soil N2-N Emission (kg)
    */
    public double GetsoilN2NEmissions(int maxCrops)
    {
        double soilN2Emission = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            soilN2Emission += theCrops[i].GetN2Nemission() * area;
        }
        return soilN2Emission;
    }
    //!  Get NH3N Manure Emission for the whole crop sequence.
    /*
     \return Manure NH3-N emission (kg)
    */

    public double GetNH3NmanureEmissions()
    {
        return GetNH3NmanureEmissions(theCrops.Count);
    }
    //!  Get NH3-N Manuere Emissions for the crop sequence up until crop number maxCrops. 
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return NH3-N Manuere Emissions (kg)
    */
    public double GetNH3NmanureEmissions(int maxCrops)
    {
        double manureNH3Emissions = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            manureNH3Emissions += theCrops[i].GetmanureNH3Nemission() * area;
        }
        return manureNH3Emissions;
    }
    //!  Get Fertiliser NH3-N emission for the whole crop sequence. 
    /*
     \return Fertiliser NH3-N emission (kg)
    */
    public double GetfertiliserNH3Nemissions()
    {
        return GetfertiliserNH3Nemissions(theCrops.Count);
    }
    //!  Get fertiliser NH3-N Emissions for the crop sequence up until crop number maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return fertiliser NH3-N Emissions (kg)
    */
    public double GetfertiliserNH3Nemissions(int maxCrops)
    {
        double fertiliserNH3emissions = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            fertiliserNH3emissions += theCrops[i].GetfertiliserNH3Nemission() * area;
        }
        return fertiliserNH3emissions;
    }
    //!  Get Urine NH3-N Emissions for the whole crop sequence. 
    /*
     \return Urine NH3-N Emission (kg)
    */
    public double GeturineNH3emissions()
    {
        return GeturineNH3emissions(theCrops.Count);
    }
    //!  Get Urine NH3 N Emissions for the crop sequence up until crop number maxCrops
    /*!
     \param maxCrops maximum number of crops to include in the calculation
     \return Urine NH3 N Emissions (kg)
    */
    public double GeturineNH3emissions(int maxCrops)
    {
        double urineNH3emissions = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            urineNH3emissions += theCrops[i].GeturineNH3emission() * area;
        }
        return urineNH3emissions;
    }
    //!  Get Unutilised Grazable DM. 
    /*
     \return the unutilized grazable dry matter (kg)
    */
    public double GetUnutilisedGrazableDM()
    {
        double unutilisedGrazableDM = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            unutilisedGrazableDM += theCrops[i].GetUnutilisedGrazableDM() * area;
        }
        return unutilisedGrazableDM;
    }
    //!  Get Cumulative Drainage. 
    /*
     \return Cumulative Drainage (mm)
    */
    public double GetCumulativeDrainage()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetcumulativeDrainage() * area;
        }
        return retVal;
    }
    //!  Get Cumulative Precipitation. 
    /*
     \return Cumulative Precipitation (mm)
    */
    public double GetCumulativePrecip()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetcumulativePrecipitation() * area;
        }
        return retVal;
    }
    //!  Get Cumulative Irrigation. 
    /*
     \return  Cumulative Irrigation (mm)
    */
    public double GetCumulativeIrrigation()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetcumulativeIrrigation() * area;
        }
        return retVal;
    }
    //!  Get Cumulative Evaporation. 
    /*
     \return Cumulative Evaporation (mm)
    */
    public double GetCumulativeEvaporation()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetcumulativeEvaporation() * area;
        }
        return retVal;
    }
    //!  Get Cumulative Transpiration. 
    /*
     \return Cumulative Transpiration (mm)
    */
    public double GetCumulativeTranspiration()
    {
        double retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal += theCrops[i].GetcumulativeTranspiration() * area;
        }
        return retVal;
    }
    //!  Get Maximum Plant Available Water. 
    /*
     \return Maximum Plant Available Water (mm)
    */
    public double GetMaxPlantAvailableWater()
    {
        double retVal = 0;
        retVal = thesoilWaterModel.GetMaxPlantAvailableWater();
        return retVal;
    }
    //!  Get Length of Cropping Period.
    /*
     \param maxCrop  maximum number of crops to include in the calculation
     \return length of the cropping period (years)
    */
    public double GetLengthCroppingPeriod(int maxCrop)
    {
        if (maxCrop > theCrops.Count)
        {
            string messageString = ("Error - CropSequenceClass:GetLengthCroppingPeriod - number of crops requested is greater than the number of crops in the sequence");
            GlobalVars.Instance.Error(messageString);
        }
        long firstDate = 999999999;
        long lastDate = -99999999;
        for (int i = 0; i < maxCrop; i++)
        {
            CropClass acrop = theCrops[i];
            long astart = acrop.getStartLongTime();
            if (astart < firstDate)
                firstDate = astart;
            long anend = acrop.getEndLongTime();
            if (anend > lastDate)
                lastDate = anend;
            // GlobalVars.Instance.log(i.ToString() +" Crop start " + acrop.GetStartYear() + " end " + acrop.GetEndYear());
        }
        long period = lastDate - firstDate;
        double retVal = ((double)period) / ((double)365);
        return retVal;
    }
    //!  Calculate Length of Sequence.
    /*
     \return Length of Sequence (years)
    */
    public int calculatelengthOfSequence()
    {
        long firstDate = 999999999;
        long lastDate = -99999999;
        for (int i = 0; i < theCrops.Count; i++)
        {
            CropClass acrop = theCrops[i];
            long astart = acrop.getStartLongTime();
            if (astart < firstDate)
                firstDate = astart;
            long anend = acrop.getEndLongTime();
            if (anend > lastDate)
                lastDate = anend;
            // GlobalVars.Instance.log(i.ToString() +" Crop start " + acrop.GetStartYear() + " end " + acrop.GetEndYear());
        }
        long period = lastDate - firstDate;
        double temp = ((double)period) / ((double)365);
        int retVal = (int)Math.Ceiling(temp);
        return retVal;
    }
    //!  Write crop sequence data to files. 
    public void Write()
    {
        GlobalVars.Instance.writeStartTab("CropSequenceClass");

        GlobalVars.Instance.writeInformationToFiles("nameCropSequenceClass", " Name", "-", name, parens);
        GlobalVars.Instance.writeInformationToFiles("identity", "Identity", "-", identity, parens);
        GlobalVars.Instance.writeInformationToFiles("soilType", "Soil type", "-", soilType, parens);
        GlobalVars.Instance.writeInformationToFiles("area", "area", "-", area, parens);

        for (int i = 0; i < theCrops.Count; i++)
        {
            theCrops[i].Write();

        }

        int year = calculatelengthOfSequence();
        //Ying - need to create functions for all these calculations

        double tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].getCFixed();
        }
        GlobalVars.Instance.writeInformationToFiles("CFixed", "C fixed", "kgC/ha/yr", (tmp / year), parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsurfaceResidueC();
        }
        GlobalVars.Instance.writeInformationToFiles("surfaceResidueC", "C in surface residues", "kgC/ha/yr", tmp / year, parens);
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsubsurfaceResidueC();
        }
        GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueCAndsurfaceResidueC", "C in surface residues and subsurface residues", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsubsurfaceResidueC();
        }
        GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueC", "C in subsurface residues", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetUrineC();
        }
        GlobalVars.Instance.writeInformationToFiles("urineCFeedItem", "C in urine", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetfaecalC();
        }
        GlobalVars.Instance.writeInformationToFiles("faecalCFeedItem", "C in faeces", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetstorageProcessingCLoss();
        }
        GlobalVars.Instance.writeInformationToFiles("storageProcessingCLoss", "C lost during processing or storage", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetFertiliserC();
        }
        GlobalVars.Instance.writeInformationToFiles("fertiliserC", "Emission of CO2 from fertiliser", "kgC/ha/yr", tmp / year, parens); tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetharvestedC();
        }
        GlobalVars.Instance.writeInformationToFiles("harvestedC", "Harvested C", "kgC/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetburntResidueC();
        }
        GlobalVars.Instance.writeInformationToFiles("burntResidueC", "C in burned crop residues", "kg/ha", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetUnutilisedGrazableC();
        }
        GlobalVars.Instance.writeInformationToFiles("unutilisedGrazableC", "C in unutilised grazable DM", "kg/ha", tmp / year, parens);
        //N budget
        double Ninput = 0;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].getNFix();
        }
        GlobalVars.Instance.writeInformationToFiles("Nfixed", "N fixed", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].getnAtm();
        }
        GlobalVars.Instance.writeInformationToFiles("nAtm", "N from atmospheric deposition", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetFertiliserNapplied();
        }
        GlobalVars.Instance.writeInformationToFiles("fertiliserNinput", "Input of N in fertiliser", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetUrineN();
        }
        GlobalVars.Instance.writeInformationToFiles("urineNfertRecord", "Urine N", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetfaecalN();
        }
        GlobalVars.Instance.writeInformationToFiles("faecalNCropSeqClass", "Faecal N", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetexcretaNInput();
        }
        GlobalVars.Instance.writeInformationToFiles("excretaNInput", "Input of N in excreta", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GettotalManureNApplied();
        }
        GlobalVars.Instance.writeInformationToFiles("totalManureNApplied", "Total N applied in manure", "kgN/ha/yr", tmp / year, parens);
        Ninput += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetharvestedN();
        }
        GlobalVars.Instance.writeInformationToFiles("harvestedN", "N harvested (N yield)", "kgN/ha/yr", tmp / year, parens);
        /*        
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
                {
                    tmp += theCrops[i].getSurfaceResidueDM();
                }
                GlobalVars.Instance.writeInformationToFiles("surfaceResidueDM", "Surface residue dry matter", "kg/ha", tmp / year, parens);*/
        double Nlost = 0;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetmanureNH3Nemission();
        }
        GlobalVars.Instance.writeInformationToFiles("manureNH3emission", "NH3-N from manure application", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetfertiliserNH3Nemission();
        }
        GlobalVars.Instance.writeInformationToFiles("fertiliserNH3emission", "NH3-N from fertiliser application", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GeturineNH3emission();
        }
        GlobalVars.Instance.writeInformationToFiles("urineNH3emission", "NH3-N from urine deposition", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetstorageProcessingNLoss();
        }
        GlobalVars.Instance.writeInformationToFiles("storageProcessingNLoss", "N2 emission during product processing/storage", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetN2ONemission();
        }
        GlobalVars.Instance.writeInformationToFiles("N2ONemission", "N2O emission", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetfertiliserN2ONEmission();
        }
        GlobalVars.Instance.writeInformationToFiles("fertiliserN2OEmission", "N2O emission from fertiliser", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        GlobalVars.Instance.writeInformationToFiles("Placeholder", "Placeholder", "", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsoilN2ONEmission();
        }
        GlobalVars.Instance.writeInformationToFiles("soilN2OEmission", "N2O emission from mineralised N", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetN2Nemission();
        }
        GlobalVars.Instance.writeInformationToFiles("N2Nemission", "N2 emission", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetburningN2ON();
        }
        GlobalVars.Instance.writeInformationToFiles("burningN2ON", "N2O emission from burned crop residues", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetburningNH3N();
        }
        GlobalVars.Instance.writeInformationToFiles("burningNH3N", "NH3 emission from burned crop residues", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetburningNOxN();
        }
        GlobalVars.Instance.writeInformationToFiles("burningNOxN", "NOx emission from burned crop residues", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetburningOtherN();
        }
        GlobalVars.Instance.writeInformationToFiles("burningOtherN", "N2 emission from burned crop residues", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetOrganicNLeached();
        }
        GlobalVars.Instance.writeInformationToFiles("OrganicNLeached", "Leached organic N", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;
        /*        tmp = 0;
                for (int i = 0; i < theCrops.Count; i++)
                {
                    tmp += theCrops[i].GetmineralNToNextCrop();
                }
                GlobalVars.Instance.writeInformationToFiles("mineralNToNextCrop", "Mineral N to next crop", "kgN/ha/yr", tmp / year, parens);
         */
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetfertiliserN2ONEmission();
        }
        GlobalVars.Instance.writeInformationToFiles("fertiliserN2OEmission", "N2O emission from fertiliser N", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetmanureN2ONEmission();
        }
        GlobalVars.Instance.writeInformationToFiles("manureN2OEmission", "N2O emission from manure N", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        GlobalVars.Instance.writeInformationToFiles("Placeholder", "Placeholder", "", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsoilN2ONEmission();
        }
        GlobalVars.Instance.writeInformationToFiles("soilN2OEmission", "N2O emission from mineralised N", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetnitrateLeaching();
        }
        GlobalVars.Instance.writeInformationToFiles("nitrateLeaching", "Nitrate N leaching", "kgN/ha/yr", tmp / year, parens);
        Nlost += tmp;

        double DeltaSoilN = (finalSoilN - initialSoilN) / area;

        GlobalVars.Instance.writeInformationToFiles("DeltaSoilN", "Change in soil N", "kgN/ha/yr", DeltaSoilN / year, parens);
        GlobalVars.Instance.writeInformationToFiles("Ninput", "N input", "kgN/ha/yr", Ninput / year, parens);
        GlobalVars.Instance.writeInformationToFiles("NLost", "N lost", "kgN/ha/yr", Nlost / year, parens);

        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetmineralNavailable();
        }
        GlobalVars.Instance.writeInformationToFiles("mineralNavailable", "Mineral N available", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetUnutilisedGrazableC();
        }
        GlobalVars.Instance.writeInformationToFiles("unutilisedGrazableN", "N in unutilised grazable DM", "kg/ha", tmp / year, parens);

        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].getMineralNFromLastCrop();
        }
        GlobalVars.Instance.writeInformationToFiles("mineralNFromLastCrop", "Mineral N from last crop", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetCropNuptake();
        }
        GlobalVars.Instance.writeInformationToFiles("cropNuptake", "Crop N uptake", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsurfaceResidueN();
        }
        GlobalVars.Instance.writeInformationToFiles("surfaceResidueN", "N in surface residues", "kgN/ha/yr", tmp / year, parens);
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsubsurfaceResidueN();
        }
        GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueNAndsurfaceResidueN", "N in surface residues and subsurface residues", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetsubsurfaceResidueN();
        }
        GlobalVars.Instance.writeInformationToFiles("subsurfaceResidueN", "N in subsurface residues", "kgN/ha/yr", tmp / year, parens);
        tmp = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            tmp += theCrops[i].GetSoilNMineralisation();
        }
        GlobalVars.Instance.writeInformationToFiles("soilNMineralisation", "Soil mineralised N", "kgN/ha/yr", tmp / year, parens);

        GlobalVars.Instance.writeEndTab();

    }
    //!  Get N Fixation for the whole crop sequence. 
    /*
     \return N fixation (kg)
    */
    public double getNFix()
    {
        return getNFix(theCrops.Count);
    }
    //!  Get N Fix for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops number of crops in the sequence to include in the calculation
     \return N fixation (kg N)
    */
    public double getNFix(int maxCrops)
    {
        double nFix = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            nFix += theCrops[i].getNFix() * area;
        }
        return nFix;
    }
    //!  Get N deposition from atmosphere for the whole crop sequence. 
    /*
     \return N deposition from atmosphere (kg)
    */
    public double getNAtm()
    {
        return getNAtm(theCrops.Count);
    }
    //!  Get N deposition from the atmosphere for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return atmospheric N deposition  (kg)
    */
    public double getNAtm(int maxCrops)
    {
        double nAtm = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            nAtm += theCrops[i].getnAtm() * area;
        }
        return nAtm;
    }
    //!  Get Manure N Applied for the whole crop sequence. 
    /*
     \return Manure N Applied (kg)
    */
    public double getManureNapplied()
    {
        return getManureNapplied(theCrops.Count);
    }
    //!  Get Manure N Applied for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return manure N applied (kg)
    */
    public double getManureNapplied(int maxCrops)
    {
        double manureN = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            for (int j = 0; j < theCrops[i].GetmanureApplied().Count; j++)
                manureN += theCrops[i].GetmanureApplied()[j].getNamount() * area;
        }
        return manureN;
    }//!  Get Fertiliser N Applied for the whole crop sequence 
    /*
     \return fertiliaer N applied (kg)
    */
    public double getFertiliserNapplied()
    {
        return getFertiliserNapplied(theCrops.Count);
    }
    //!  Get Fertiliser N Applied for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return  fertiliser N applied (kg)
    */
    public double getFertiliserNapplied(int maxCrops)
    {
        double fertiliserN = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            for (int j = 0; j < theCrops[i].GetfertiliserApplied().Count; j++)
            {
                if (theCrops[i].GetfertiliserApplied()[j].getName() != "Nitrification inhibitor")
                    fertiliserN += theCrops[i].GetfertiliserApplied()[j].getNamount() * area;
            }
        }
        return fertiliserN;
    }
    //!  Get N Harvested in crops for whole crop sequence. 
    /*
     \return N Harvested in crops (kg)
    */
    public double getNharvested()
    {
        return getNharvested(theCrops.Count);
    }
    //!  Get N Harvested for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return fertiliser N applied (kg).
    */
    public double getNharvested(int maxCrops)
    {
        double Nharvested = 0;
        for (int i = 0; i < maxCrops; i++)
            Nharvested += theCrops[i].GetharvestedN() * area;
        return Nharvested;
    }
    //!  Get Residual Soil Mineral N for whole crop sequence. 
    /*
     \return Residual Soil Mineral N (kg)
    */
    public double GetResidualSoilMineralN()
    {
        return GetResidualSoilMineralN(theCrops.Count);
    }
    //!  Get Residual Soil Mineral N  for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return residual soil mineral N (kg)
    */
    public double GetResidualSoilMineralN(int maxCrops)
    {
        double retVal = 0;
        retVal = theCrops[maxCrops - 1].GetmineralNToNextCrop() * area;
        return retVal;
    }
    //!  calculate grazed feedItems. 
    /*
     Calculates the total amount of each feed item that is grazed in this crop sequence
    */
    public void calcGrazedFeedItems()
    {
        for (int i = 0; i < theCrops.Count; i++)
            theCrops[i].calcGrazedFeedItems();
    }
    //!  Check to ensure that the modelled and expected yields are the same (or nearly so), and adjust for losses in storage
    /*
     \return zero (if there is an error in any of the crops, it will be trapped there).
    */
    public int CheckYields()
    {
        int retVal = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            retVal = theCrops[i].CheckYields(name);
            if (retVal > 0)
                break;
        }
        return retVal;
    }
    //!  Calculate the amount of manure N needed for all crops and take this from the manure bank.
    public void CalcManureForAllCrops()
    {
        for (int i = 0; i < theCrops.Count; i++)
            theCrops[i].GetManureForCrop();
    }
    //!  Calculate the modelled yield. 
    /*
     * Iterate until the modelled and expected yield converge to the same value (or nearly so).
     * Iteration is necessary because crop residues created at the start of the period contribute to N available later in the period
     * and the amounts of crop residues are dependent on the yield.
    */
    public void CalcModelledYield()
    {
        double surplusMineralN = 0;
        surplusMineralN = startsoilMineralN;
        double CropCinputToSoil = 0;
        double CropNinputToSoil = 0;
        double CropsoilCO2_CEmission = 0;
        double CropCleached = 0;
        double ManCinputToSoil = 0;
        double ManNinputToSoil = 0;
        double mineralisedN = 0;
        double cropStartSoilN = 0;
        for (int i = 0; i < theCrops.Count; i++)
            theCrops[i].Calcwaterlimited_yield(0);//sets expected yield to potential yield
        double avgCinput = 0;
        double avgNinput = 0;
        for (int i = 0; i < theCrops.Count; i++)
        {
            Console.Write(".");
            CalculateWater(i);  //this runs the soil water model for this crop
            theCrops[i].CalculateFertiliserInput();

            double meanTemperature = GlobalVars.Instance.theZoneData.GetMeanTemperature(theCrops[i]);
            double meandroughtFactorSoil = theCrops[i].CalculatedroughtFactorSoil();
            bool doneOnce = false;
            startSoil= new ctool2(aModel);
            cropStartSoilN = aModel.GetNStored();
            double oldsurplusMineralN = surplusMineralN;
            bool gotit = false;
            int count = 0;

            double[] Temperature = GlobalVars.Instance.theZoneData.airTemp;
            //CheckRotationCBalance(i + 1);  //only use for debugging. Causes problems if residues are carried over to the next crop

            //iterate for each crop, until the crop yield stabilises (note special treatment of grazed crops)
            while ((gotit == false) || (doneOnce == false))
            {
                count++;
                if (count > GlobalVars.Instance.GetmaximumIterations())
                {
                    string messageString = "Error; Crop production iterations exceeds maximum\n";
                    messageString += "Crop sequence name = " + name + "\n";
                    messageString += "Crop name = " + name + " crop number " + i.ToString();
                    Write();
                    GlobalVars.Instance.Error(messageString);
                }

                GlobalVars.Instance.log("seq " + identity.ToString() + " crop " + i.ToString() + " loop " + count.ToString(), 5);
                if (doneOnce)  //need to reset the soil model to its starting point before trying another iteration
                {
                    resetC_Tool();
                    surplusMineralN = oldsurplusMineralN;
                    theCrops[i].DoCropInputs(true);
                }
                else
                {
                    if (i > 0)
                    {
                        if (theCrops[i - 1].GetresidueToNext() != null)
                        {
                            GlobalVars.product residueFromPrevious = new GlobalVars.product(theCrops[i - 1].GetresidueToNext());
                            theCrops[i].HandleBareSoilResidues(residueFromPrevious);
                        }
                    }

                    theCrops[i].DoCropInputs(false);
                }
                //Run the soil C and N model
                RunCropCTool(GlobalVars.Instance.Writectoolxls, false, i, Temperature, theCrops[i].GetdroughtFactorSoil(), 0, ref CropCinputToSoil, ref CropNinputToSoil, ref ManCinputToSoil, ref ManNinputToSoil,
                    ref CropsoilCO2_CEmission, ref CropCleached, ref mineralisedN);
                // for debugging
                //if (mineralisedN < 0)
                //    Console.Write("");
                double relGrowth = 0;
                //calculate the amount of N available to the crop and the resulting crop growth
                theCrops[i].CalcAvailableNandGrowth(ref surplusMineralN, mineralisedN, ref relGrowth);
                gotit = theCrops[i].CalcModelledYield(surplusMineralN, relGrowth, true);
                doneOnce = true;
            }
            //Iterations are complete and the modelled yield is equal to the expected yield
            if (theCrops[i].GetresidueToNext() != null)
            {
                if (i == theCrops.Count - 1)
                {
                    string messageString = ("Error - crop number " + i.ToString() + " in sequence " + name);
                    messageString += (": last crop in sequence cannot leave residues for next crop");
                    GlobalVars.Instance.Error(messageString);
                }
                else if (theCrops[i + 1].Getname() != "Bare soil")
                {
                    string messageString = ("Error - crop number " + i.ToString() + " in sequence " + name);
                    messageString += (": crop leaves residues but is not followed by bare soil");
                    GlobalVars.Instance.Error(messageString);
                }
            }
            //Need to rerun the crop model, this time generating output
            if (i > 0)
                theCrops[i].SetnitrificationInhibitor(theCrops[i - 1].GetnitrificationInhibitor());
            theCrops[i].getCFixed();
            theCrops[i].DoCropInputs(true);
            resetC_Tool();

            double[] Temperatures = GlobalVars.Instance.theZoneData.airTemp;
            input Ctoolinput = RunCropCTool(false, GlobalVars.Instance.Writectoolxls, i, Temperatures, theCrops[i].GetdroughtFactorSoil(), 0, ref CropCinputToSoil, ref CropNinputToSoil, ref ManCinputToSoil, ref ManNinputToSoil, ref CropsoilCO2_CEmission, ref CropCleached, ref mineralisedN);
            avgCinput += Ctoolinput.totCarbon;
            avgNinput += Ctoolinput.totNitrogen;

            CinputToSoil += (CropCinputToSoil + ManCinputToSoil) * area;
            
            NinputToSoil += (CropNinputToSoil + ManNinputToSoil) * area;
            mineralisedSoilN += mineralisedN * area;
            soilCO2_CEmission += CropsoilCO2_CEmission * area;
            Cleached += CropCleached * area;
            CheckRotationCBalance(i + 1);
            CheckRotationNBalance(i + 1, false);
            double deltaSoilN = aModel.GetNStored() - cropStartSoilN; //value is in kg/ha
            theCrops[i].WriteFieldFile(deltaSoilN, CdeltaSoil, CropsoilCO2_CEmission);
            WriteWaterData(i);
            string productString = "Modelled yield for crop " + i + theCrops[i].Getname() + " ";
            for (int j = 0; j < theCrops[i].GettheProducts().Count; j++)
                productString += theCrops[i].GettheProducts()[j].composition.GetName() + " "
                    + theCrops[i].GettheProducts()[j].GetModelled_yield() + " ";
            GlobalVars.Instance.log(productString, 5);
        }
        GlobalVars.Instance.addtotCFom(avgCinput, parens);
        GlobalVars.Instance.addtotNFom(avgNinput, parens);
    }

    //!  Check C Balance for the whole crop sequence
    /*
     \return true if the C budget can be closed
    */
    public bool CheckRotationCBalance()
    {
        return CheckRotationCBalance(theCrops.Count);
    }
    //!  Check Rotaion C Balance for the crop sequence up until crop number maxCrops.
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return true if the C budget can be closed.
    */
    public bool CheckRotationCBalance(int maxCrops)
    {
        bool retVal = true;
        //!C in harvested plant material
        double harvestedC = 0;
        //!C fixed from atmosphere
        double fixedC = 0;
        //!C in manure applied
        double manureC = 0;
        //!C in dung from grazing livestock
        double faecalC = 0;
        //!C in urine  from grazing livestock
        double urineC = 0;
        //!C lost through burning of crop residues
        double burntC = 0;
        double croppingPeriod = GetLengthCroppingPeriod(maxCrops);
        for (int i = 0; i < maxCrops; i++)
        {
            retVal = theCrops[i].CheckCropCBalance(name, i + 1);
            if (retVal == false)
                break;
            harvestedC += theCrops[i].GetharvestedC() * area;
            fixedC += theCrops[i].getCFixed() * area;
            manureC += theCrops[i].GetManureC() * area;
            faecalC += theCrops[i].GetfaecalC() * area;
            urineC += theCrops[i].GetUrineC() * area;
            burntC += theCrops[i].GetburntResidueC() * area;
        }
        if (theCrops[maxCrops - 1].GetresidueToNext() != null)
            residueCremaining = theCrops[maxCrops - 1].GetResidueCtoNextCrop() * area;
        else
            residueCremaining = 0;
        finalSoilC = GetCStored();
        CdeltaSoil = finalSoilC - initialSoilC;
        double diffSoil = (CinputToSoil - (soilCO2_CEmission + Cleached + CdeltaSoil)) / (croppingPeriod * initialSoilC);
        double errorPercent = 100 * diffSoil;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diffSoil) > tolerance)
        {
            string messageString = "Error; soil C balance is greater than the permitted margin\n";
            messageString += "Crop sequence name = " + name + "\n";
            messageString += "Percentage error = " + errorPercent.ToString("0.00") + "%";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
        /*double Charvested = getCHarvested();
        double Cfixed = getCFixed();*/

        //! Cbalance (kg) should be zero or thereabouts
        double Cbalance = ((fixedC + manureC + faecalC + urineC - (soilCO2_CEmission + Cleached + CdeltaSoil + harvestedC + burntC + residueCremaining))) / croppingPeriod;
        double diffSeq = Cbalance / initialSoilC;
        errorPercent = 100 * diffSeq;
        if (Math.Abs(diffSeq) > tolerance)
        {
            string messageString = "Error; crop sequence C balance is greater than the permitted margin" + "\n"; ;
            messageString += "Crop sequence name = " + name + "\n"; ;
            messageString += "Percentage error = " + errorPercent.ToString("0.00") + "%";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
        return retVal;
    }
    //!  Check Rotaion N Balance for the whole crop sequence
    /*
     \param perHaperYr true if the N flows should be calculated per year not for the whole duration
    */
    public void CheckRotationNBalance(bool perHaperYr)
    {
        CheckRotationNBalance(theCrops.Count, perHaperYr);
    }
    //!  Check Rotaion N Balance for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \param perHaperYr if true, the calculation is expressed per year of the period under consideration
    */
    public void CheckRotationNBalance(int maxCrops, bool perHaperYr)
    {
        for (int i = 0; i < maxCrops; i++)
        {
            theCrops[i].CheckCropNBalance(name, i + 1);
        }
        if (theCrops[maxCrops - 1].GetresidueToNext() != null)
            residueNremaining = theCrops[maxCrops - 1].GetResidueNtoNextCrop() * area;
        else
            residueNremaining = 0;
        double residualMineralN = GetResidualSoilMineralN(maxCrops);
        double orgNleached = GetOrganicNLeached(maxCrops);//;
        finalSoilN = GetNStored();
        NdeltaSoil = finalSoilN - initialSoilN;
        double soilNbalance = NinputToSoil - (NdeltaSoil + mineralisedSoilN + orgNleached);
        double diff = 0;
        if (NinputToSoil > 0)
            diff = soilNbalance / NinputToSoil;
        double errorPercent = 100 * diff;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
            string messageString = "Error; soil N balance is greater than the permitted margin\n";
            messageString += "Crop sequence name = " + name + "\n";
            messageString += "Percentage error = " + errorPercent.ToString("0.00") + "%";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
        double NAtm = getNAtm(maxCrops);
        double ManureNapplied = GetManureNapplied(maxCrops);
        double excretaNInput = GetexcretaNInput(maxCrops);
        double FertiliserNapplied = GetFertiliserNapplied(maxCrops);
        double fixedN = getNFix();
        double Ninput;
        Ninput = NAtm + ManureNapplied + excretaNInput + FertiliserNapplied + fixedN + startsoilMineralN * area;
        double NH3NmanureEmissions = GetNH3NmanureEmissions(maxCrops);
        double fertiliserNH3Nemissions = GetfertiliserNH3Nemissions(maxCrops);
        double urineNH3emissions = GeturineNH3emissions(maxCrops);
        double N2ONEmission = GetN2ONemission(maxCrops);
        double N2NEmission = GetN2NEmission(maxCrops);
        double nitrateLeaching = GettheNitrateLeaching(maxCrops);
        double harvestedN = getNharvested(maxCrops);
        double burntN = getBurntResidueN();
        double NLost;
        NLost = NH3NmanureEmissions + fertiliserNH3Nemissions + urineNH3emissions + N2ONEmission + N2NEmission + burntN
                    + orgNleached + nitrateLeaching;
        //! Check if the N budget can be closed
        double Nbalance = 0;
        Nbalance = Ninput - (NLost + harvestedN + NdeltaSoil + residualMineralN + residueNremaining);
        diff = Nbalance / Ninput;
        if (perHaperYr)   //set this to true to get values in kg/ha/yr (useful for debugging)
        {
            double residueNremainingperHaperYr= residueNremaining /( lengthOfSequence * area);
            double residualMineralNperHaperYr = residualMineralN /( lengthOfSequence * area);
            double orgNleachedperHaperYr = orgNleached /( lengthOfSequence * area);
            double NAtmperHaperYr = NAtm /( lengthOfSequence * area);
            double ManureNappliedperHaperYr = ManureNapplied /( lengthOfSequence * area);
            double excretaNInputperHaperYr = excretaNInput /( lengthOfSequence * area);
            double FertiliserNappliedperHaperYr = FertiliserNapplied /( lengthOfSequence * area);
            double fixedNperHaperYr = fixedN /( lengthOfSequence * area);
            double NinputperHaperYr = NAtmperHaperYr + ManureNappliedperHaperYr + excretaNInputperHaperYr + FertiliserNappliedperHaperYr 
                + fixedNperHaperYr + startsoilMineralN;
            double NH3NmanureEmissionsperHaperYr = NH3NmanureEmissions /( lengthOfSequence * area);
            double fertiliserNH3NemissionsperHaperYr = fertiliserNH3Nemissions /( lengthOfSequence * area);
            double urineNH3emissionsperHaperYr = urineNH3emissions /( lengthOfSequence * area);
            double N2ONEmissionperHaperYr = N2ONEmission /( lengthOfSequence * area);
            double N2NEmissionperHaperYr = N2NEmission /( lengthOfSequence * area);
            double nitrateLeachingperHaperYr = nitrateLeaching /( lengthOfSequence * area);
            double harvestedNperHaperYr = harvestedN /( lengthOfSequence * area);
            double burntNperHaperYr = burntN /( lengthOfSequence * area);
            double NdeltaSoilperHaperYr = NdeltaSoil /( lengthOfSequence * area);
            double NLostperHaperYr = NH3NmanureEmissionsperHaperYr + fertiliserNH3NemissionsperHaperYr + urineNH3emissionsperHaperYr + N2ONEmissionperHaperYr + N2NEmissionperHaperYr + burntNperHaperYr
                    + orgNleachedperHaperYr + nitrateLeachingperHaperYr;
            double NbalanceperHaperYr = NinputperHaperYr - (NLostperHaperYr + harvestedNperHaperYr + NdeltaSoilperHaperYr + residualMineralNperHaperYr + residueNremainingperHaperYr);
            //diff = NbalanceperHaperYr / NinputperHaperYr;
        }
        errorPercent = 100 * diff;
        if ((Math.Abs(diff) > tolerance) && (Math.Abs(Nbalance / area) > 5.0))
        {
            string messageString = "Error; crop sequence N balance is greater than the permitted margin\n";
            messageString += "Crop sequence name = " + name + "\n";
            messageString += "Percentage error = " + errorPercent.ToString("0.00") + "%";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
    }

    //!  Get Total Manure N Applied for the whole crop sequence 
    /*
     \return total manure N applied (kg)
    */
    public double GettotalManureNApplied()
    {
        return GettotalManureNApplied(theCrops.Count);
    }
    //!  Get Total Manure N Applied for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return total manure N applied (kg)
    */
    public double GettotalManureNApplied(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GettotalManureNApplied() * area;
        }
        return retVal;
    }
    //!  Get Excreta N Input. 
    /*
     \return N input in excreta (kg)
    */
    public double GetexcretaNInput()
    {
        return GetexcretaNInput(theCrops.Count);
    }
    //!  Get Excreta N Input for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return excretal N input (kg)
    */
    public double GetexcretaNInput(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetexcretaNInput() * area;
        }
        return retVal;
    }
    //!  Get Excretal C Input for the whole crop sequence. 
    /*
     \return excretal C input (kg)
    */
    public double GetexcretaCInput()
    {
        return GetexcretaCInput(theCrops.Count);
    }
    //!  Get Excreta C Input for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return excretal C input (kg)
    */
    public double GetexcretaCInput(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetexcretaCInput() * area;
        }
        return retVal;
    }
    //!  Get Faecal N Input for the whole crop sequence. 
    /*
     \return faecal N input (kg)
    */
    public double GetFaecalNInput()
    {
        return GetFaecalNInput(theCrops.Count);
    }
    //!  Get Faecal N Input for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return faecalN input (kg)
    */
    public double GetFaecalNInput(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetfaecalN() * area;
        }
        return retVal;
    }
    //!  Get Manure NH3-N Emission for the whole crop sequence. 
    /*
     \return Manure NH3-N Emission (kg)
    */
    public double GetManureNH3NEmission()
    {
        return GetManureNH3NEmission(theCrops.Count);
    }
    //!  Get Manure NH3-N Emission for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return Manure NH3-N Emission (kg)
    */
    public double GetManureNH3NEmission(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetmanureNH3Nemission() * area;
        }
        return retVal;
    }
    //!  Get Fertiliser NH3-N Emission for the whole crop sequence. 
    /*
     \return fertiliser NH3-N emission (kg)
    */
    public double GetFertNH3NEmission()
    {
        return GetFertNH3NEmission(theCrops.Count);
    }
    //!  Get Fertiliser NH3-N Emission for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return fertiliser NH3-N emission (kg)
    */
    public double GetFertNH3NEmission(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetfertiliserNH3Nemission() * area;
        }
        return retVal;
    }
    //!  Get Organic N Leached for the whole crop sequence 
    /*
     \return organic N leached (kg)
    */
    public double GetOrganicNLeached()
    {
        return GetOrganicNLeached(theCrops.Count);
    }
    //!  Get Organic N Leached for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return organic N leached (kg)
    */
    public double GetOrganicNLeached(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetOrganicNLeached() * area;
        }
        return retVal;
    }
    //!  Get total N2-N Emission for the whole crop sequence. 
    /*
     \return total N2-N Emission (kg)
    */
    public double GetN2NEmission()
    {
        return GetN2NEmission(theCrops.Count);
    }
    //!  Get N2-N Emission for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return N2-N emission (kg).
    */
    public double GetN2NEmission(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetN2Nemission() * area;
        }
        return retVal;
    }
    //!  Get total crop N2O-N Emission for the whole crop sequence. 
    /*
     \return N2O-N Emission (kg)
    */
    public double GetN2ONemission()
    {
        return GetN2ONemission(theCrops.Count);
    }
    //!  Get N2ON Emission for the crop sequence up until crop number maxCrops
    /*
     \param maxCrops maximum number of crops to include in the calculation
     \return N2O-N emission (kg)
    */
    public double GetN2ONemission(int maxCrops)
    {
        double retVal = 0;
        for (int i = 0; i < maxCrops; i++)
        {
            retVal += theCrops[i].GetN2ONemission() * area;
        }
        return retVal;
    }
    //!  Spin up the soil C and N model.
    /*
     \param thestartsoilMineralN the initial amount of soil mineral N (kg/ha).
     \param initialFOM_Cinput the initial input of fresh organic matter (FOM) (kg/ha).
     \param InitialFOMCtoN the C:N ration of the FOM input
     \param meanTemperature the mean annual air temperature (Celsius)
     \param CropSeqID the crop sequence identification number.
    */
    public void spinup(ref double thestartsoilMineralN, double initialFOM_Cinput, double InitialFOMCtoN, double[] meanTemperature, int CropSeqID)
    {
        double CatStartSpinup = GetCStored();
        int spinupYears = 0;
        FileInformation file = new FileInformation(GlobalVars.Instance.getConstantFilePath());
        if (GlobalVars.Instance.reuseCtoolData == -1)
            file.setPath("constants(0).spinupYearsBaseLine(-1)");
        else
            file.setPath("constants(0).spinupYearsNonBaseLine(-1)");
        spinupYears = file.getItemInt("Value");
        bool writeToFile = GlobalVars.Instance.Writectoolxls;
        double soilNmineralised = 0;
        if (spinupYears > 0)
        {
            int startDay = (int)theCrops[0].getStartLongTime() - spinupYears * 365 - 365;
            int endDay = (int)theCrops[0].getStartLongTime() - spinupYears * 365;
            //! daily input of FOM
            double[,] FOM_Cin = new double[365, aModel.GetnumOfLayers()];
            //! daily input of HUM
            double[,] HUM_Cin = new double[365, aModel.GetnumOfLayers()];
            //! daily input of biochar
            double[,] Biochar_Cin = new double[365, aModel.GetnumOfLayers()];
            //! not used at present
            double[] cultivation = new double[365];
            //! daily input of N in FOM
            double[] fomnIn = new double[365];

            double Cchange = 0;
            double Nleached = 0;
            double CO2Emission = 0;
            double Cleached = 0;
            double FOM_Cinput = initialFOM_Cinput;
            double FOMnInput = initialFOM_Cinput / InitialFOMCtoN;
            double CinputSpin = 0;
            for (int i = 0; i < 365; i++)
            {
                for (int j = 0; j < aModel.GetnumOfLayers(); j++)
                {
                    FOM_Cin[i, j] = 0;
                    HUM_Cin[i, j] = 0;
                    Biochar_Cin[i, j] = 0;
                }
                fomnIn[i] = 0;
                FOM_Cin[i, 0] = FOM_Cinput / 365.0;
                fomnIn[i] = FOMnInput / 365.0;
                cultivation[i] = 0;
            }
            double initCStored = aModel.GetCStored();//value in kgC/ha

            double Nmin = 0;

            double[] spindroughtFactorSoil = GlobalVars.Instance.theZoneData.GetdroughtIndex();
            double[] dailyDroughtFactorSoil = new double[365];
            double[] dailyTemperature = new double[365];
            timeClass atimeClass = new timeClass();
            int dayNo = 0;
            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < atimeClass.GetDaysInMonth(j+1); i++)
                {
                    dailyDroughtFactorSoil[dayNo] = spindroughtFactorSoil[j];
                    dailyTemperature[dayNo] = meanTemperature[j];
                    dayNo += 1;
                }
            }

            for (int j = 0; j < spinupYears; j++)
            {
                double tempCchange = 0;
                double tempNleached = 0;
                double tempCO2Emission = 0;
                double tempCleached = 0;

                node.Add(aModel.Dynamics(writeToFile, 1, startDay + (j + 1) * 365, startDay - 1 + (j + 2) * 365, FOM_Cin, HUM_Cin, Biochar_Cin, fomnIn, cultivation, dailyTemperature, dailyDroughtFactorSoil,
                        ref tempCchange, ref tempCO2Emission, ref tempCleached, ref Nmin, ref tempNleached, CropSeqID));

                // GlobalVars.Instance.log(j.ToString() + " " + aModel.GetFOMCStored().ToString() + " " + aModel.GetHUMCStored().ToString() + " " + aModel.GetROMCStored().ToString() +
                // " " + aModel.GetCStored().ToString(), 6);

                Cchange += tempCchange;
                Nleached += tempNleached;
                CO2Emission += tempCO2Emission;
                Cleached += tempCleached;
            }

            double deltaC = aModel.GetCStored() - initCStored;
            CinputSpin = FOM_Cinput * spinupYears;
            double diff = CinputSpin - (CO2Emission + Cleached + deltaC);
            if ((Nmin < 0) && (Math.Abs(Nmin) < 0.0001))
                Nmin = 0;
            soilNmineralised = Nmin;
            thestartsoilMineralN = Nmin;
        }
        GlobalVars.Instance.log("tonnes C/ha at start of spinup " + (CatStartSpinup / (area * 1000)).ToString() + " tonnes C/ha at end of spinup " + (initialSoilC / (1000 * area)).ToString(), 6);
        GlobalVars.Instance.log("kg N/ha min at end of spinup " + (soilNmineralised / area).ToString(), 6);

        startSoil = new ctool2(aModel);
    }
    //!  Reset C_Tool if need to repeat the simulation during iterations. 
    public void resetC_Tool()
    {
        aModel.reloadC_Tool(startSoil);
    }
    //!  Run the soil C and N model for the specified crop
    /*
     \param diagnostics set true to print diagnostics
     \param writeOutput set true to output data
     \param cropNo number of the crop in the crop sequence
     \param meanTemperature array of mean monthly temperature (Celsius) during the crop duration
     \param droughtFactorSoil array of mean monthly drought factor during the crop duration
     \param cultivationDepth depth of cultivation (m) (not used at present)
     \param CropCinputToSoil on exit, contains the input of C in crop residues (kg/ha)
     \param CropNinputToSoil on exit, contains the input of N in crop residues (kg/ha)
     \param ManCinputToSoil on exit, contains the input of C in manure (kg/ha)
     \param ManNinputToSoil  on exit, contains the input of N in manure (kg/ha)
     \param CropsoilCO2_CEmission on exit, contains the C emitted as CO2 (kg/ha)
     \param CropCleanched on exit, contains the C leached (kg/ha)
     \param Nmin on exit, contains the mineral N in the soil (kg/ha)
    */
    public input RunCropCTool(bool diagnostics, bool writeOutput, int cropNo, double[] meanTemperature, double[] droughtFactorSoil, double cultivationDepth, ref double CropCinputToSoil, ref double CropNinputToSoil,
        ref double ManCinputToSoil, ref double ManNinputToSoil, ref double CropsoilCO2_CEmission, ref double CropCleached, ref double Nmin)
    {
        /********* These values are in kg/field ***********************/
        double Nin = GetNStored(); //N in soil at start of crop period
        double Cin = GetCStored(); //C in soil at start of crop period
        /***********End of values in kg/field ************************/

        /********* The values here are in kg/field ***********************/
        ManCinputToSoil = 0;
        ManNinputToSoil = 0;
        if (diagnostics)
        {
            GlobalVars.Instance.log(theCrops[cropNo].Getname().ToString(), 5);
            GlobalVars.Instance.log("N in " + Nin.ToString(), 5);
        }
        long startDay = theCrops[cropNo].getStartLongTime();
        long stopDay = theCrops[cropNo].getEndLongTime();

        long lastDay = theCrops[cropNo].getDuration();
        double[,] FOM_Cin = new double[lastDay, aModel.GetnumOfLayers()];
        double[,] HUM_Cin = new double[lastDay, aModel.GetnumOfLayers()];
        double[,] Biochar_Cin = new double[lastDay, aModel.GetnumOfLayers()];
        double[] cultivation = new double[lastDay];
        double[] fomnIn = new double[lastDay];
        double FOMCsurface = theCrops[cropNo].GetsurfaceResidueC(); //Fresh plant OM carbon input to surface of soil (e.g. leaf and stem litter, unharvested above-ground OM)
        double FOMCsubsurface = theCrops[cropNo].GetsubsurfaceResidueC(); //Fresh plant OM carbon input below the soil surface (e.g. roots)
        double urineCO2Emission = theCrops[cropNo].GetUrineC(); //assume urine C is all emitted
        double grazingCH4C = theCrops[cropNo].GetgrazingCH4C();
        double faecalC = theCrops[cropNo].GetfaecalC();
        ManCinputToSoil += urineCO2Emission + faecalC - grazingCH4C;
        CropCinputToSoil = FOMCsurface + FOMCsubsurface;
        double tempCleached = 0;
        double tempManHUMN = 0;
        double tempFOMN = 0;
        double faecalN = theCrops[cropNo].GetfaecalN();
        ManNinputToSoil += faecalN;
        double FOMNsurface = theCrops[cropNo].GetsurfaceResidueN(); //Fresh organic N added to soil surface
        double FOMNsubsurface = theCrops[cropNo].GetsubsurfaceResidueN();//Fresh organic N added below soil surface
        CropNinputToSoil = FOMNsurface + FOMNsubsurface;
        //distribute below ground OM using a triangular function
        int numOfLayers = aModel.GetnumOfLayers();
        double OMdepthDistribCoeff = 2 / ((double)numOfLayers);
        double OMtimeDistCoeff = 2 / (double)theCrops[cropNo].getDuration();
        double oldDayCum = 0;
        //run the soil C and N model on a daily basis for the whole duration of the crop 
        for (int j = 0; j < lastDay; j++)
        {
            // first set up all the inputs for each day of the crop's duration
            double manureFOMCsurface = theCrops[cropNo].GetmanureFOMCsurface(j);  //Fresh OM carbon input to surface of soil
            double manureHUMCsurface = theCrops[cropNo].GetmanureHUMCsurface(j); //Humic OM carbon input to surface of soil
            double manureBiocharCsurface = theCrops[cropNo].GetmanureBiocharCsurface(j); //Humic OM carbon input to surface of soil
            double manureFOMnsurface = theCrops[cropNo].GetmanureFOMNsurface(j); //Fresh OM nitrogen input to surface of soil
            double manureHUMnsurface = theCrops[cropNo].GetmanureHUMNsurface(j); //Humic nitrogen input to surface of soil
            double faecalCToday = (faecalC - grazingCH4C) / lastDay;   //distribute faecal C evenly over whole period
            if ((j == lastDay - 1) && (cultivationDepth > 0))
                cultivation[j] = cultivationDepth;
            else
                cultivation[j] = 0;
            ManCinputToSoil += manureFOMCsurface + manureHUMCsurface + manureBiocharCsurface;
            double faecalNToday = faecalN / lastDay;
            tempManHUMN += manureHUMnsurface;
            tempFOMN += manureFOMnsurface;
            ManNinputToSoil += manureFOMnsurface + manureHUMnsurface;

            //distribuute the organic matter inputs in crop residues and roots over time
            double propThisDay = 0;
            if (theCrops[cropNo].Getpermanent() == false)
            {
                double newDayCum = (((double)j + 1) / 2) * OMtimeDistCoeff * (((double)j + 1) / (double)theCrops[cropNo].getDuration());
                propThisDay = newDayCum - oldDayCum;
                oldDayCum = newDayCum;
            }
            else
                propThisDay = 1 / (double)theCrops[cropNo].getDuration();

            FOM_Cin[j, 0] = propThisDay * FOMCsurface;
            double oldDepthCum = 1.0;
            //distribute C and N inputs over time and soil depth
            for (int k = aModel.GetnumOfLayers() - 1; k >= 0; k--)
            {
                double newDepthCum = (((double)k) / 2) * OMdepthDistribCoeff * (((double)k) / (double)numOfLayers);
                double propThisLayer = oldDepthCum - newDepthCum;
                FOM_Cin[j, k] += FOMCsubsurface * propThisDay * propThisLayer;
                HUM_Cin[j, k] = 0;
                oldDepthCum = newDepthCum;
            }
            FOM_Cin[j, 0] += manureFOMCsurface + faecalCToday;
            HUM_Cin[j, 0] += manureHUMCsurface;
            Biochar_Cin[j, 0] = manureBiocharCsurface;
            //set fresh OM nitrogen inputs over time
            fomnIn[j] = (FOMNsurface + FOMNsubsurface) * propThisDay + manureFOMnsurface + faecalNToday;
        }
        double Cchange = 0;
        Nmin = 0;
        double orgNleached = 0;
        double CO2Emission = 0;
        long JDay = theCrops[cropNo].getStartLongTime();

        while (JDay > 365)
            JDay = JDay - 365;
        //Now run the soil C and N model for each day of the crop's duration
        XElement tmp = aModel.Dynamics(writeOutput, (int)JDay, theCrops[cropNo].getStartLongTime(), theCrops[cropNo].getEndLongTime(), FOM_Cin, HUM_Cin, Biochar_Cin, fomnIn, cultivation,
            meanTemperature, droughtFactorSoil, ref Cchange, ref CO2Emission, ref tempCleached, ref Nmin, ref orgNleached, identity);
        if (writeOutput == true)
            node.Add(tmp);
        CropsoilCO2_CEmission = CO2Emission + urineCO2Emission;
        CropCleached = tempCleached;
        theCrops[cropNo].SetOrganicNLeached(orgNleached);
        theCrops[cropNo].SetsoilNMineralisation(Nmin);

        double TotalmanureFOMNsurface = theCrops[cropNo].GetTotalmanureFOMNsurface();
        double TotalmanureHUMNsurface = theCrops[cropNo].GetTotalmanureHUMNsurface();

        /********* End of values in kg/ha ***********************/

        /********* These values are in kg/field ***********************/
        Nin += area * (FOMNsurface + TotalmanureHUMNsurface + TotalmanureFOMNsurface + FOMNsubsurface + faecalN);
        double Nout = GetNStored();
        double balance = Nin - (Nout + area * (orgNleached + Nmin));
        if (diagnostics)
        {
            GlobalVars.Instance.log("orgN leached " + (area * orgNleached).ToString() + " Nmin " + (area * Nmin).ToString(), 5);
            GlobalVars.Instance.log("N out " + Nout.ToString() + " bal " + balance.ToString(), 5);
        }
        //Check that we can close the N budget
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        double diff = balance / Nin;
        if (Math.Abs(diff) > tolerance)
        {
            string messageString = "Error; crop sequence soil C-Tool N balance is greater than the permitted margin\n";
            messageString += "Crop sequence name = " + name + "\n";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
        if ((Nmin < 0) && (Math.Abs(Nmin) < 0.0001))
            Nmin = 0;
        Cin += (CropCinputToSoil + ManCinputToSoil) * area;
        double Cout = GetCStored();
        balance = Cin - (Cout + CO2Emission * area);
        diff = balance / Cin;
        if (Math.Abs(diff) > tolerance)
        {
            string messageString = "Error; crop sequence soil C-Tool C balance is greater than the permitted margin\n";
            messageString += "Crop sequence name = " + name + "\n";
            Write();
            GlobalVars.Instance.Error(messageString);
        }
    /********* End of values in kg/field ***********************/

        double totCarbon = 0;
        for (int k = 0; k < lastDay; k++)
        {
            totCarbon += FOM_Cin[k, 0];
            totCarbon += FOM_Cin[k, 1];
        }
        double totNitrogen = 0;
        for (int k = 0; k < lastDay; k++)
            totNitrogen += fomnIn[k];
        input returnValue;
        returnValue.totCarbon = totCarbon;
        returnValue.totNitrogen = totNitrogen;
        return returnValue;
    }
    //! A structure for sending information back from the soil C and N modelling.
    public struct input
    {
        //!
        public double totCarbon;
        public double totNitrogen;
    }
    //!  Do Extra Output (sometimes useful for debugging)
    /*
     \param writer. one XMlWriter instance.
     \param tabFile, one StreamWriter.
    */
    public void extraoutput(XmlWriter writer, System.IO.StreamWriter tabFile)
    {
        for (int i = 0; i < theCrops.Count; i++)
        {
            writer.WriteStartElement("Crop");
            writer.WriteStartElement("Identity");
            writer.WriteValue(theCrops[i].Getidentity());
            for (int j = 0; j < theCrops[i].GettheProducts().Count; j++)
            {
                writer.WriteStartElement("Product");
                writer.WriteStartElement("Identity");
                writer.WriteValue(theCrops[i].GettheProducts()[j].GetExpectedYield());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

        }
    }
    //!  Process the expcted yield in preparation for output.
    /*
     \param parens string containing the tag to be attached to the output string
    */
    public void processExpectedYieldForOutput(string parens)
    {
        List<int> ID = new List<int>();
        List<double> ExpectedYeld0 = new List<double>();
        List<double> ExpectedYeld1 = new List<double>();
        List<int> NumberOfPlants = new List<int>();
        for (int i = 0; i < theCrops.Count; i++)
        {
            int newID = -1;
            for (int j = 0; j < ID.Count; j++)
            {
                if (ID[j] == theCrops[i].Getidentity())
                    newID = j;
            }
            if (newID != -1)
            {
                if (theCrops[newID].GettheProducts().Count >= 1)
                    ExpectedYeld0[newID] += theCrops[i].GettheProducts()[0].GetExpectedYield();
                if (theCrops[newID].GettheProducts().Count == 2)
                    ExpectedYeld1[newID] += theCrops[i].GettheProducts()[1].GetExpectedYield();
                NumberOfPlants[newID] += 1;
            }
            else
            {
                ID.Add(theCrops[i].Getidentity());
                if (theCrops[i].GettheProducts().Count >= 1)
                    ExpectedYeld0.Add(theCrops[i].GettheProducts()[0].GetExpectedYield());
                else
                    ExpectedYeld0.Add(-1);

                if (theCrops[i].GettheProducts().Count == 2)
                    ExpectedYeld1.Add(theCrops[i].GettheProducts()[1].GetExpectedYield());
                else
                    ExpectedYeld1.Add(-1);
                NumberOfPlants.Add(1);
            }
        }

        GlobalVars.Instance.writeStartTab("CropSequenceClass");
        for (int i = 0; i < ID.Count; i++)
        {
            GlobalVars.Instance.writeStartTab("Crop");
            GlobalVars.Instance.writeInformationToFiles("Identity", "ExpectedYield", "-", ID[i], parens + "_crop" + i.ToString());
            if (ExpectedYeld0[i] != -1)
            {
                GlobalVars.Instance.writeStartTab("product");
                GlobalVars.Instance.writeInformationToFiles("ExpectedYield", "ExpectedYield", "-", ExpectedYeld0[i] / NumberOfPlants[i], parens + "_crop" + i.ToString() + "_product(0)");
                GlobalVars.Instance.writeEndTab();
            }
            if (ExpectedYeld1[i] != -1)
            {
                GlobalVars.Instance.writeStartTab("product");
                GlobalVars.Instance.writeInformationToFiles("ExpectedYield", "ExpectedYield", "-", ExpectedYeld1[i] / NumberOfPlants[i], parens + "_crop" + i.ToString() + "_product(1)");
                GlobalVars.Instance.writeEndTab();
            }
            GlobalVars.Instance.writeEndTab();
        }
        aModel.Write();
        GlobalVars.Instance.writeEndTab();
    }
    //!  Calculate the water budget for a crop
    /*
     \param cropNo the number of the crop in the sequence
    */
    public void CalculateWater(int cropNo)
    {
        double soilC = aModel.GetCStored();
        thesoilWaterModel.CalcSoilWaterProps(soilC);
        double cumtranspire = 0;
        double irrigationThreshold = theCrops[cropNo].GetirrigationThreshold();
        double irrigationMinimum = theCrops[cropNo].GetirrigationMinimum();
        timeClass clockit = new timeClass(theCrops[cropNo].GettheStartDate());
        int k = 0;
        double cropDuration = theCrops[cropNo].getDuration();
        while (k < cropDuration)
        {
            //if ((k == 25) && (cropNo == 0))
              //  Console.Write("");
            double currentLAI = theCrops[cropNo].CalculateLAI(k);
            double rootingDepth = theCrops[cropNo].CalculateRootingDepth(k);
            double precip = theCrops[cropNo].Getprecipitation(k);
            double potevapotrans = theCrops[cropNo].GetpotentialEvapoTrans(k);
            double airTemp = theCrops[cropNo].Gettemperature(k);
            double SMD = thesoilWaterModel.getSMD(rootingDepth, rootingDepth);
            double maxAvailWaterToRootingDepth = thesoilWaterModel.GetMaxAvailWaterToRootingDepth(rootingDepth, rootingDepth);
            double propAvailWater = 1 - SMD / maxAvailWaterToRootingDepth;
            double droughtFactorPlant = 0;
            double dailydroughtFactorSoil = 0;
            double irrigation = 0;

            if ((theCrops[cropNo].GetisIrrigated()) && (propAvailWater <= irrigationThreshold))
            {
                double irrigationAmount = irrigationThreshold * SMD;
                if ((irrigationAmount - precip) > irrigationMinimum)
                    irrigation = irrigationAmount;
            }
            thesoilWaterModel.dailyRoutine(potevapotrans, precip, irrigation, airTemp, currentLAI, rootingDepth, ref droughtFactorPlant,
                ref dailydroughtFactorSoil);
            double evap = thesoilWaterModel.GetsnowEvap() + thesoilWaterModel.Getevap();
            double transpire = thesoilWaterModel.Gettranspire();
            double drainage = thesoilWaterModel.Getdrainage();
            double evapoTrans = evap + transpire;
            SMD = thesoilWaterModel.getSMD(rootingDepth, rootingDepth);
            double waterInSoil = thesoilWaterModel.getwaterInSystem();
            //if (waterInSoil < 6)
            // Console.Write("");
            theCrops[cropNo].SetsoilWater(k, waterInSoil);
            theCrops[cropNo].SetdroughtFactorPlant(k, droughtFactorPlant);
            theCrops[cropNo].SetdroughtFactorSoil(k, dailydroughtFactorSoil);
            cumtranspire += transpire;
            theCrops[cropNo].Setdrainage(k, drainage);
            theCrops[cropNo].Setevaporation(k, evap);
            theCrops[cropNo].Settranspire(k, transpire);
            theCrops[cropNo].Setirrigation(k, irrigation);
            theCrops[cropNo].SetplantavailableWater(k, thesoilWaterModel.GetRootingWaterVolume());
            theCrops[cropNo].SetcanopyStorage(k, thesoilWaterModel.getcanopyInterception());
            /*Console.WriteLine("Crop " + cropNo + " k " + k + " precip " + precip.ToString("F3") + " evap " + evap.ToString("F3")
                + " drought " + droughtFactorPlant.ToString("F3") + " drain " + drainage.ToString("F3")
                + " transpire " + transpire.ToString("F3") + " irr " + irrigation.ToString("F3"));*/
            k++;
            clockit.incrementOneDay();
        }
    }
    //!  Write the data from the crop water model to file
    /*
     \param cropNo the crop number in the crop sequence
    */
    public void WriteWaterData(int cropNo)
    {
        if (cropNo >= (theCrops.Count - numCropsInSequence))
        {
            int runningDay = 0;
            for (int i = 0; i < theCrops[cropNo].getDuration(); i++)
            {
                runningDay++;
                GlobalVars.Instance.WriteDebugFile("CropSeq", identity, '\t');
                GlobalVars.Instance.WriteDebugFile("crop_no", cropNo, '\t');
                GlobalVars.Instance.WriteDebugFile("day", runningDay, '\t');
                GlobalVars.Instance.WriteDebugFile("precip", theCrops[cropNo].Getprecipitation(i), '\t');
                GlobalVars.Instance.WriteDebugFile("irrigation", theCrops[cropNo].GetIrrigationWater(i), '\t');
                GlobalVars.Instance.WriteDebugFile("evap", theCrops[cropNo].Getevaporation(i), '\t');
                GlobalVars.Instance.WriteDebugFile("transpire", theCrops[cropNo].Gettranspire(i), '\t');
                GlobalVars.Instance.WriteDebugFile("drainage", theCrops[cropNo].Getdrainage(i), '\t');
                GlobalVars.Instance.WriteDebugFile("waterInSoil", theCrops[cropNo].GetsoilWater(i), '\t');
                GlobalVars.Instance.WriteDebugFile("plantwaterInSoil", theCrops[cropNo].GetplantavailableWater(i), '\t');
                GlobalVars.Instance.WriteDebugFile("droughtFactorPlant", theCrops[cropNo].GetdroughtFactorPlant(i), '\t');
                GlobalVars.Instance.WriteDebugFile("droughtFactorSoil", theCrops[cropNo].GetdroughtFactorSoil(i), '\t');
                GlobalVars.Instance.WriteDebugFile("LAI", theCrops[cropNo].GetLAI(i), '\t');
                //GlobalVars.Instance.WriteDebugFile("Month", clockit.GetMonth(), '\t');
                GlobalVars.Instance.WriteDebugFile("NO3-N", theCrops[cropNo].GetdailyNitrateLeaching(i), '\t');
                GlobalVars.Instance.WriteDebugFile("Canopy", theCrops[cropNo].getdailyCanopyStorage(i), '\n');
            }
        }

    }
    //!  Check crop N uptake. 
    /*
     * Sends a warning if the relative uptake of N is high
    */
    public void CheckNuptake()
    {
        for (int i = 0; i < theCrops.Count; i++)
        {
            double relNuptake = theCrops[i].GetRelativeNuptake();
            double critical_level = 0.8;
            if (relNuptake < critical_level)
            {
                string messageString = ("Warning - crop yield below threshold limit for " + theCrops[i].Getname() + " in crop sequence " + Getname());
                GlobalVars.Instance.Error(messageString,"",false);

            }
        }
    }
    //!  Write contents of soil pools to file at the end of a Baseline spinup (adaptation) simulation
    /*
     * Data will be used to initiate the soil pools when implementing projection scenarios (including Baseline)
    */
    public void writeCtoolData(System.IO.StreamWriter handoverCtoolData, bool writeHeader = true)
    {
        double rotarea = 0;
        double fomcLayer1 = 0;
        double fomcLayer2 = 0;
        double humcLayer1 = 0;
        double humcLayer2 = 0;
        double romcLayer1 = 0;
        double romcLayer2 = 0;
        double biocharcLayer1 = 0;
        double biocharcLayer2 = 0;
        double FOMn = 0;
        double rotresidualMineralN = 0;
        if (writeHeader)
            handoverCtoolData.WriteLine("sequenceNo" + '\t' + "soilTypeNo" +'\t' + "fomcLayer1_kg_ha" + '\t' + "fomcLayer2_kg_ha" + '\t' + "humcLayer1_kg_ha" + '\t' + "humcLayer2_kg_ha" + '\t'
                    + "romcLayer1_kg_ha" + '\t' + "romcLayer2_kg_ha" + '\t' + "biocharcLayer1_kg_ha" + '\t' + "biocharcLayer2_kg_ha" + '\t'
                    + "FOMn_kg_ha" + '\t' + "rotresidualMineralN_kg_ha" + '\t' + "rotarea_ha");
        rotarea += getArea();
        fomcLayer1 += aModel.GettheClayers()[0].getFOM() * getArea();
        fomcLayer2 += aModel.GettheClayers()[1].getFOM() * getArea();
        humcLayer1 += aModel.GettheClayers()[0].getHUM() * getArea();
        humcLayer2 += aModel.GettheClayers()[1].getHUM() * getArea();
        romcLayer1 += aModel.GettheClayers()[0].getROM() * getArea();
        romcLayer2 += aModel.GettheClayers()[1].getROM() * getArea();
        biocharcLayer1 += aModel.GettheClayers()[0].getBiochar() * getArea();
        biocharcLayer2 += aModel.GettheClayers()[1].getBiochar() * getArea();
        FOMn += aModel.FOMn * getArea();
        rotresidualMineralN += GetResidualSoilMineralN();
        rotarea = getArea();
        handoverCtoolData.WriteLine(identity.ToString() + '\t' + soiltypeNo + '\t'+ (fomcLayer1 / rotarea).ToString() + '\t' + (fomcLayer2 / rotarea).ToString()
            + '\t' + (humcLayer1 / rotarea).ToString() + '\t' + (humcLayer2 / rotarea).ToString()
            + '\t' + (romcLayer1 / rotarea).ToString() + '\t' + (romcLayer2 / rotarea).ToString()
            + '\t' + (biocharcLayer1 / rotarea).ToString() + '\t' + (biocharcLayer2 / rotarea).ToString()
            + '\t' + FOMn / rotarea + '\t' + rotresidualMineralN / rotarea + '\t' + rotarea);
    }
}
