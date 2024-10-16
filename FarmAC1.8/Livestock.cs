using System;
using System.Collections.Generic;
using System.Xml;
//! A class named livestock.
public class livestock
{
    //! ManureRecipient.
    public struct ManureRecipient
    {
        //! Integer ID of storage type receiving manure from this animal
        int ManureStorageID;
        //! Name of the manure storage
        string ManureStorageName;
        //!  a string containing information about the farm and scenario number.
        string parens;  /*!< a string containing information about the farm and scenario number.*/
        public void setparens(string aparens){parens=aparens;}
        public void setManureStorageID(int aType)
        {ManureStorageID = aType;}
        public int GetStorageType() { return ManureStorageID; }
        public void setManureStorageName(string aName) { ManureStorageName = aName; }
        //! Write details of the manure recipient to an xml file
        public void WriteXML()
        {
            GlobalVars.Instance.writeStartTab("ManureRecipient");
            GlobalVars.Instance.writeInformationToFiles("StorageType", "Type of manure store", "-", ManureStorageID, parens);
            GlobalVars.Instance.writeEndTab();
        }
        //! Write details of the manure recipient to an Excel (csv) file
        public void WriteXls()
        {
            GlobalVars.Instance.writeLivestockFile("StorageType", "Type of manure store", "-", ManureStorageID, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("ManureStorageName", "ManureStorageName", "-", ManureStorageName, "livestock", 0);
        }
    }
    //! Contains details of the livestock housing for this animal.
    public struct housingRecord
    {
        //! Integer ID of the type of housing
        int HousingType;
        //! Proportion of the year that the animal is in this housing
        double propTime;
        //! Name of the housing
        string NameOfHousing;
        string parens; /*!<! a string containing information about the farm and scenario number.*/ 
        //! List of the manure storages that receive manure from the housing
        public List<ManureRecipient> Recipient;
        public void SetNameOfHousing(string aName) { NameOfHousing = aName; }
        public void SetHousingType(int aVal) { HousingType = aVal; }
        public void SetpropTime(double aVal) { propTime = aVal; }
        public int GetHousingType() { return HousingType; }
        public string GetHousingName() { return NameOfHousing; }
        public double GetpropTime() { return propTime; }
        public List<ManureRecipient> GetManureRecipient() { return Recipient; }
        public void setparens(string aparens) { parens = aparens; }
        //! Write details of the housing to an xml file
        public void WriteXML()
        {
            GlobalVars.Instance.writeStartTab("housingRecord");
            GlobalVars.Instance.writeInformationToFiles("HousingType", "Type of housing", "-", HousingType, parens);
            GlobalVars.Instance.writeInformationToFiles("propTime", "Proportion of time spent in house", "-", propTime, parens);
            for (int i = 0; i < Recipient.Count; i++)
                Recipient[i].WriteXML();
            GlobalVars.Instance.writeEndTab();
        }
        //! Write details of the housing to an Excel file
        public void WriteXls()
        {
            GlobalVars.Instance.writeLivestockFile("NameOfHousing", "NameOfHousing", "-", NameOfHousing, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("propTime", "Proportion of time spent in house", "-", propTime, "livestock", 0);
            for (int i = 0; i < Recipient.Count; i++)
                Recipient[i].WriteXls();
        }
    }
    string path;

    ///characteristics of livestock

    //! true if the animal is a ruminant
    bool isRuminant;
    //! true if this is a milk-producing animal
    bool isDairy;
    //true if the production (meat or milk) is an input
    bool inputProduction;
    //! Annual average number of animals
    double avgNumberOfAnimal;
    //! Average milk production (kg/day)
    double avgProductionMilk;
    //! Average growth rate (kg/day)
    double avgProductionMeat;
    //! Average milk production (kg Energy-corrected milk/day)
    double avgProductionECM;
    //! Efficiency of use of use of digested protein for milk production
    double efficiencyProteinMilk;
    //! Holds details of where the livestock are housed (if housed)
    List<housingRecord> housingDetails;  
    ///parameters
    private string parens; /*!<! a string containing information about the farm and scenario number.*/
    //! Unique ID for animal species (= 1 for cattle, = 2 for pigs)
    int speciesGroup;
    //Subcategory of a species (e.g. dairy, beef)
    int LivestockType;
    //!Average live weight (kg)
    double liveweight; 
    //! Initial weight (kg)
    double startWeight;
    //! Final weight (kg)
    double endWeight;
    //! Number of weaner pigs (should be moved to Pigs)
    double numberWeaners;
    //! End weight of weaner pigs (should be moved to Pigs)
    double endWeightWeaners;
    //! Duration of production cycle in days (only for livestock where production is an input)
    double duration;
    //! Proportion of C input that is partitioned to urine
    double urineProp;
    //! Name of the livestock
    string name;
    //! Concentration of N in weight gain (kg/kg)
    double growthNconc;
    //! Concentration of C in weight gain (kg/kg)
    double growthCconc;
    //! Concentration of N in milk (kg/kg)
    double milkNconc;
    //! Concentration of C in milk (kg/kg)
    double milkCconc;
    //! Concentration of fat in milk (kg/kg)
    double milkFat;
    //! Age of the animal (yrs)
    double age; 
    //! Coefficient of maintenance energy demand equation
    double maintenanceEnergyCoeff;
    //! Coefficient of growth energy demand equation
    double growthEnergyDemandCoeff;
    //! Multiplier to increase (>1) or decrease (<1) the energy requirement per unit of milk produced
    double milkAdjustmentCoeff;
    //! Proportion of livestock that dies and does not contribute to production
    double mortalityCoefficient;
    //! Ym value for enteric methane production (IPCC method)
    double entericYm;
    //! Potential m3 CH4 kg-1 of VS excreted
    double Bo;
    //! Efficiency with which nitrate reduces enteric methane emission (1 = one molecule methane per molecule nitrate)
    double nitrateEfficiency;
    //! Proportion of excreta that is deposited in the fields
    double propExcretaField =-1.0; 
    //! Energy intake (MJ/year)
    double energyIntake; 
    //! Total demand for energy (MJ/year)
    double energyDemand;
    //! Maintenance energy (MJ/yr)
    double energyUseForMaintenance;
    //! Growth energy (MJ/yr)
    double energyUseForGrowth;
    //! Milk energy (MJ/yr)
    double energyUseForMilk;
    //! Energy used for grazing (MJ/yr)
    double energyUseForGrazing;
    //! Energy remobilized from body reserves (MJ/yr)
    double energyFromRemobilisation;
    //! Deficit of energy required for maintenance (MJ/yr)
    double maintenanceEnergyDeficit;
    //! Deficit of energy required for growth (MJ/yr)
    double growthEnergyDeficit;
    //! Energy supplied in concentrate feed (MJ/yr)
    double concentrateEnergy;
    //! Limit on N use efficiency when production is an input (exceedance will generate an error)
    double maxNuseEfficiency; 
    //! Dry matter intake (kg/yr)
    double DMintake;
    //! Dry matter intake via grazing (kg/yr)
    double DMgrazed;
    //! Scandinavian feed units 
    double FE;
    //! Dry matter supplied in concentrate feed
    double concentrateDM;
    //! Total N intake (kg/yr)
    double Nintake;
    //! Total intake of ash (kg/yr)
    double diet_ash;
    //! Total intake of fibre (kg/yr)
    double diet_fibre;
    //! Total intake of fat (kg/yr)
    double diet_fat;
    //! Total intake of neutral detergent fibre (kg/yr)
    double diet_NDF;  
    //! Dry matter digestibility (kg/kg)
    double digestibilityDiet;
    //! Concentration of nitrate in diet (kg/kg)
    double diet_nitrate;//kg/kg
    //! Total intake of C (kg/yr)
    double Cintake;  
    //! Energy intake as a proportion of maintenance energy demand
    double energyLevel;
    //! N in milk (kg/yyr)
    double milkN;
    // C in milk (kg/yr)
    double milkC;
    //! N in weight gain (kg/yr)
    double growthN;
    //! C in weight gain (kg/yr)
    double growthC;
    //! N in mortalities (kg/yr)
    double mortalitiesN;
    //! C in mortalities (kg/yr)
    double mortalitiesC;
    //! C in urine (kg/yr)
    double urineC;
    //! N in urine (kg/yr)
    double urineN;
    //! C in faeces (kg/yr)
    double faecalC;
    //! C in methane from excreta deposited during grazing (kg/yr)
    double CCH4GR;
    //! N in faeces (kg/yr)
    double faecalN;
    //! N in excreted to pasture (kg/yr)
    double NexcretionToPasture;
    //! C in excreted to pasture (kg/yr)
    double CexcretionToPasture;
    //! C in enteric methane (kg/yr)
    double CH4C;
    //! C in CO2 expired by animal (kg/yr)
    double CO2C;
    //! Intake of N in grazed feed
    double grazedN = 0;
    //! Intake of C in grazed feed
    double grazedC = 0;
    //! Intake of DM in grazed feed
    double grazedDM = 0;
    //!Intake of N in feed fed at pasture
    double pastureFedN = 0;
    //!Intake of C in feed fed at pasture
    double pastureFedC = 0;
    //! Is true if production is limited by availability of protein not energy
    bool proteinLimited;
    //! Proportion of the dry matter intake that is obtained by grazing
    public double propDMgrazed;
    //! List of feed items in the feed ration
    List<feedItem> feedRation;

    //! Returns true if animal is a lactating.
    public bool GetisDairy() { return isDairy; }
    //! Returns enteric methane C emission as double .
    public double getCH4C() { return CH4C; }
    //! Returns CO2 C emission as a double (kg).
    public double getCO2C() { return CO2C; }
    //!  Returns Milk C as a double (kg).
    public double GetMilkC() { return milkC; }
    //! Returns Growth C as a double (kg).
    public double GetGrowthC() { return growthC; }
    //!  Returns milk N as a double (kg).
    public double GetMilkN() { return milkN; }
    //! Returns N in growth as a double (kg).
    public double GetGrowthN() { return growthN; }
    //!  Returns N lost in mortalities as a double (kg).
    public double GetMortalitiesN() { return mortalitiesN; }
    //!  Returns C lost in mortalities as a double (kg).
    public double GetMortalitiesC() { return mortalitiesC; }
    //!  Returns average milk production as a double (kg).
    public double GetavgProductionMilk() { return avgProductionMilk; }
    //!  Returns average growth as a double (kg).
    public double GetavgProductionMeat() { return avgProductionMeat; }
    //!  Returns N intake via grazing as a double (kg).
    public double GetgrazedN() { return grazedN; }
    //!  Returns N intake via feed fed at pasture as a double (kg).
    public double GetpastureFedN() { return pastureFedN; }
    //!  Returns C intake via feed fed at pasture as a double (kg).
    public double GetpastureFedC() { return pastureFedC; }
    //!  Returns proportion of excretal C or N deposited in the fields as a double .
    public double GetpropExcretaField() { return propExcretaField; }
    //!  Returns the proportion of DM obtained by grazing as a double .
    public double GetgrazedDM() { return grazedDM; }
    //! Returns the amount of N deposited on pasture as a double (kg).
    public double GetNexcretionToPasture() { return NexcretionToPasture; }
    //! Returns the amount of C deposited on pasture as a double (kg).
    public double GetCexcretionToPasture() { return CexcretionToPasture; }
    //!  Returns true if the animal is a ruminant.
    public bool GetisRuminant() { return isRuminant; }
    //!  Returns DM intake in kg as a double .
    public double GetDMintake() { return DMintake; }
    //!  Returns C in intake as a double (kg).
    public double GetCintake() { return Cintake; }
    //!  Returns N in intake as a double (kg).
    public double GetNintake() { return Nintake; }
    //!  Returns C in urine as a double (kg).
    public double GeturineC() { return urineC; }
    //!  Returns C in faeces as a double (kg).
    public double GetfaecalC() { return faecalC; }
    //!  Returns N in urine as a double (kg).
    public double GeturineN() { return urineN; }
    //!  Returns N in faeces as a double (kg).
    public double GetfaecalN() { return faecalN; }
    //!  Get Excreted N. Returns kg N as a double .
    public double GetExcretedN() { return faecalN + urineN; }
    //!  Get Excreted C. Returns kg C as a double .
    public double GetExcretedC() { return faecalC + urineC; }
    //!  Returns annual average number of animals as a double .
    public double GetAvgNumberOfAnimal(){return avgNumberOfAnimal;}
    //!  Returns the proportion of DM intake consumed by grazing as a double .
    public double GetpropDMgrazed() { return propDMgrazed; }
    //!  Returns the list of feed items in the ration.
    public List<feedItem> GetfeedRation() { return feedRation; }
    //!  Returns the potential methane production of manure from this animal in m3 CH4 kg-1 of VS excreted as a double .
    public double GetBo() { return Bo; }
    //!  Returns name as a string value.
    public string Getname() { return name; }
    //! Returns speicies group ID as an integer value.
    public int GetspeciesGroup() { return speciesGroup; }
    //!  Retruns housing details as a list of housingRecord.
    public List<housingRecord> GethousingDetails() { return housingDetails; }
    //! Default constructor.
    public livestock()
    {
    }
    //! A constructor with five arguments.
    /*!
     \param aPath a string argument containing the path to the file containing initialisation details.
     \param id a unique integer argument.
     \param zoneNr an integer argument containing the number of the agroecological zone.
     \param aparens a string argument.
    */
        public livestock(string aPath, int id, int zoneNr, string aparens, bool include_manure = true)
    {
        parens = aparens;
        FileInformation livestockFile =new FileInformation(GlobalVars.Instance.getFarmFilePath());
        path = aPath+"("+id.ToString()+")";
        livestockFile.setPath(path);
        feedRation = new List<feedItem>();
        urineProp = 0;
        DMintake =0;
        DMgrazed = 0;
        energyDemand = 0;
        energyIntake = 0;
        diet_ash = 0;
        diet_nitrate = 0;
        digestibilityDiet = 0;
        propDMgrazed = 0;
        proteinLimited = false;
        name = livestockFile.getItemString("NameOfAnimals");
        avgNumberOfAnimal = livestockFile.getItemDouble("NumberOfAnimals");
        housingDetails = new List<housingRecord>();
        if (avgNumberOfAnimal > 0)
        {
            LivestockType = livestockFile.getItemInt("LivestockType");
            speciesGroup = livestockFile.getItemInt("Species_group");
            if ((speciesGroup == 2)&&(LivestockType==1))  // pigs only and when production is specified. Should be moved to Pigs
            {
                numberWeaners = livestockFile.getItemDouble("ProductionLevel");
                endWeightWeaners = livestockFile.getItemDouble("ProductionLevel2");
            }
            FileInformation paramFile = new FileInformation(GlobalVars.Instance.getParamFilePath());

            //read livestock parameters
            string basePath = "AgroecologicalZone(" + zoneNr.ToString() + ").Livestock";
            int min = 99, max = 0;
            paramFile.setPath(basePath);
            paramFile.getSectionNumber(ref min, ref max);
            bool gotit = false;
            int livestockID = 0;
            for (int i = min; i <= max; i++) //look through all the livestock to find the one we want 
            {
                if (paramFile.doesIDExist(i))
                {
                    string testPath = basePath + "(" + i.ToString() + ").LivestockType(0)";
                    int testLivestockType = paramFile.getItemInt("Value", testPath);
                    testPath = basePath + "(" + i.ToString() + ").SpeciesGroup(0)";
                    int testspeciesGroup = paramFile.getItemInt("Value", testPath);
                    if ((testLivestockType == LivestockType) && (testspeciesGroup == speciesGroup))
                    {
                        livestockID = i;
                        gotit = true;
                        break;
                    }
                    paramFile.setPath(basePath);
                }
            }
            if (gotit == false)
            {
                string messageString = ("Livestock " + name + " Species " + speciesGroup.ToString() + ", Livestocktype  " + LivestockType.ToString() + " not found in parameters.xml");
                GlobalVars.Instance.Error(messageString);
            }
            basePath = "AgroecologicalZone(" + zoneNr.ToString() + ").Livestock(" + Convert.ToInt32(livestockID) + ")";
            //paramFile.setPath(basePath + ".SpeciesGroup(0)");
            //speciesGroup = paramFile.getItemInt("Value");
            paramFile.setPath(basePath + ".efficiencyProteinMilk(0)");
            efficiencyProteinMilk = paramFile.getItemDouble("Value");
            
            paramFile.setPath(basePath + ".isRuminant(0)");
            isRuminant = paramFile.getItemBool("Value");
            paramFile.setPath(basePath + ".isDairy(0)");
            isDairy = paramFile.getItemBool("Value");
            paramFile.setPath(basePath + ".growthNconc(0)");
            growthNconc = paramFile.getItemDouble("Value"); 
            paramFile.setPath(basePath + ".growthCconc(0)");
            growthCconc = paramFile.getItemDouble("Value");
            paramFile.setPath(basePath + ".urineProp(0)");
            urineProp = paramFile.getItemDouble("Value");
            paramFile.setPath(basePath + ".maintenanceEnergyCoeff(0)");
            maintenanceEnergyCoeff = paramFile.getItemDouble("Value");
            paramFile.setPath(basePath + ".growthEnergyDemandCoeff(0)");
            growthEnergyDemandCoeff = paramFile.getItemDouble("Value");
            if (isDairy)
            {
                paramFile.setPath(basePath + ".milkAdjustmentCoeff(0)");
                milkAdjustmentCoeff = paramFile.getItemDouble("Value");
                paramFile.setPath(basePath + ".milkFat(0)");
                milkFat = paramFile.getItemDouble("Value");
            }
            paramFile.setPath(basePath + ".Liveweight(0)");
            liveweight = paramFile.getItemDouble("Value");
            paramFile.setPath(basePath + ".Age(0)");
            age = paramFile.getItemDouble("Value");
            paramFile.setPath(basePath + ".Mortality(0)");
            mortalityCoefficient = paramFile.getItemDouble("Value");
            entericYm = paramFile.getItemDouble("Value", basePath + ".entericYm(-1)");
            Bo = paramFile.getItemDouble("Value", basePath + ".Bo(-1)");
            if (isRuminant)
            {
                paramFile.setPath(basePath + ".milkNconc(0)");
                milkNconc = paramFile.getItemDouble("Value");
                paramFile.setPath(basePath + ".milkCconc(0)");
                milkCconc = paramFile.getItemDouble("Value");
                paramFile.setPath(basePath + ".nitrateEfficiency(0)");
                nitrateEfficiency = paramFile.getItemDouble("Value");
                //nitrateEfficiency
            }
            //back to reading user input
            if (isDairy)
            {
                //avgProductionMilk = livestockFile.getItemDouble("avgProductionMilk");
                paramFile.setPath(basePath + ".weightGainDairy(0)");
                avgProductionMeat = paramFile.getItemDouble("Value");
                avgProductionMeat /= GlobalVars.avgNumberOfDays;
            }
            else
            {
                if (speciesGroup == 1)
                    avgProductionMeat = livestockFile.getItemDouble("avgProductionMeat");
                else
                if (speciesGroup == 2)
                {
                    paramFile.setPath(basePath + ".ProductionCycle(0).Startweight(0)");
                    startWeight = paramFile.getItemDouble("Value");
                    paramFile.setPath(basePath + ".ProductionCycle(0).Endweight(0)");
                    endWeight = paramFile.getItemDouble("Value");
                    paramFile.setPath(basePath + ".ProductionCycle(0).Duration(0)");
                    duration = paramFile.getItemDouble("Value");
                    paramFile.setPath(basePath + ".ProductionCycle(0).MaxLiveweightGain(0)");
                    double MaxLiveweightGain = paramFile.getItemDouble("Value");

                    avgProductionMeat = (endWeight - startWeight) / duration;
                    if (avgProductionMeat > MaxLiveweightGain)
                    {
                        string messageString = ("Growth rate of " + name + " is greater than the maximum permitted");
                        GlobalVars.Instance.Error(messageString);
                    }
                    if (LivestockType == 1)
                        avgProductionMeat += endWeightWeaners * numberWeaners / duration;
                    paramFile.setPath(basePath + ".ProductionCycle(0).MaxNuseEfficiency(0)");
                    maxNuseEfficiency = paramFile.getItemDouble("Value");
                }
            }
            if (include_manure == true)
            {
                // read in the type of housing used by the livestock
                string housingPath = path + ".Housing";
                min = 99;
                max = 0;
                livestockFile.setPath(housingPath);
                livestockFile.getSectionNumber(ref min, ref max);
                if (max > 0)
                {
                    double testPropTime = 0; //used to check that the proportions of time in all housing adds to 1.0
                    for (int i = min; i <= max; i++)
                    {
                        if (livestockFile.doesIDExist(i))
                        {
                            housingRecord newHouse = new housingRecord();
                            newHouse.setparens(parens + "_housingRecord" + i.ToString());
                            livestockFile.Identity.Add(i);
                            newHouse.SetHousingType(livestockFile.getItemInt("HousingType"));
                            newHouse.SetNameOfHousing(livestockFile.getItemString("NameOfHousing"));
                            if (newHouse.GetHousingName() != "None") //then read in the manure storage that will take manure from the housing containing these animals 
                            {
                                newHouse.SetpropTime(livestockFile.getItemDouble("PropTime"));
                                testPropTime += newHouse.GetpropTime();
                                int maxManureRecipient = 0, minManureRecipient = 99;
                                newHouse.Recipient = new List<ManureRecipient>();
                                string RecipientPath = housingPath + '(' + i.ToString() + ").ManureRecipient";
                                livestockFile.setPath(RecipientPath);
                                livestockFile.getSectionNumber(ref minManureRecipient, ref maxManureRecipient);
                                for (int j = minManureRecipient; j <= maxManureRecipient; j++)
                                {
                                    if (livestockFile.doesIDExist(j))
                                    {
                                        ManureRecipient newRecipient = new ManureRecipient();
                                        newRecipient.setparens(parens + "_ManureRecipientI" + i.ToString() + "_ManureRecipientJ" + j.ToString());
                                        livestockFile.Identity.Add(j);
                                        int type = livestockFile.getItemInt("StorageType");
                                        newRecipient.setManureStorageID(type);
                                        string manurestoreName = livestockFile.getItemString("StorageName");
                                        newRecipient.setManureStorageName(manurestoreName);
                                        newHouse.Recipient.Add(newRecipient);
                                        livestockFile.Identity.RemoveAt(livestockFile.Identity.Count - 1);
                                    }
                                }
                                housingDetails.Add(newHouse);
                                livestockFile.setPath(housingPath);
                            }
                            else
                            {
                                testPropTime = 1.0;
                                livestockFile.Identity.RemoveAt(livestockFile.Identity.Count - 1);
                            }
                        }
                    }
                    if (testPropTime != 1.0)
                    {
                        string messageString = ("Sum of proportions of time in different housing does not equal 1.0 ");
                        GlobalVars.Instance.Error(messageString);
                    }
                }
            }
            ///read livestock feed ration
            string feeditemPath = path + ".itemFed";
            min = 99;
            max = 0;
            livestockFile.setPath(feeditemPath);
            livestockFile.getSectionNumber(ref min, ref max);
            for (int i = min; i <= max; i++)
            {
                if (livestockFile.doesIDExist(i))
                {
                    ///find the feed code for the first feed item
                    feedItem newFeedItem = new feedItem(feeditemPath, i, true,parens+"_"+i.ToString());
                    //if there is no housing or corralling, all feed is fed at pasture
                    if (include_manure && (housingDetails.Count == 0) && (newFeedItem.GetisGrazed() == false))
                        newFeedItem.SetfedAtPasture(true);
                    feedRation.Add(newFeedItem); //add this feed item to ration list
                }
            }
        } ///end if average number of animals >0
    }

    //! Calculate daily maintenance energy of ruminants.
    /*!
     \return the maintenance energy (MJ per day) as a double value.
    */
    double dailymaintenanceEnergy() //MJ per animal
    {
        double maintenanceEnergy = 0;
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            case 2: //Use simplified CSIRO method
                if (speciesGroup == 1)  //cattle
                {
                    double efficiencyMaintenance = 0.02 * energyIntake / DMintake + 0.5;
                    double dailyEnergyIntake = energyIntake / GlobalVars.avgNumberOfDays;
                    //CSIRO (1990) eq 1.20, minus EGRAZE and ECOLD
                    if (age < 6.0)
                        maintenanceEnergy = maintenanceEnergyCoeff * (0.26 * Math.Pow(liveweight, 0.75) * Math.Exp(-0.03 * age)) / efficiencyMaintenance
                            + 0.09 * (energyIntake / GlobalVars.avgNumberOfDays);
                    else
                        maintenanceEnergy = maintenanceEnergyCoeff * (0.26 * Math.Pow(liveweight, 0.75) * 0.84) / efficiencyMaintenance
                            + 0.09 * (energyIntake / GlobalVars.avgNumberOfDays);
                }
                if (speciesGroup == 2)
                {
                    maintenanceEnergy = 0.44 * Math.Pow(liveweight, 0.75);
                }
                break;
      
            default: 
                    string messageString=("Energy system for " + name + " not found");
                    GlobalVars.Instance.Error(messageString);
                    break;

        }
        return maintenanceEnergy;
    }
    //! Returns the concentration of energy in growth of ruminants (MJ/kg weight gain)
    double dailyGrowthEnergyPerkg() //MJ per kg
    {
        double growthEnergyPerkg = 0;
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            case 2://use CSIRO method
               if (speciesGroup == 1)
                {
                    double efficiencyGrowth = 0.042 * energyIntake / DMintake + 0.006;//CSIRO 2007 1.36,
                    growthEnergyPerkg = growthEnergyDemandCoeff / efficiencyGrowth;
                    //growthEnergyPerkg =(6.7 - 1.0*energyLevel)-(20.3-1.0*energyLevel))/(1+Math.Exp(-6.0*))
                }
                if (speciesGroup == 2)
                    growthEnergyPerkg = 24.0; // 47.0;
                break;
            //case 3: IPCC 2019

            default:
                string messageString=("Energy system for livestock not found");
                GlobalVars.Instance.Error(messageString);
                break;

        }
        return growthEnergyPerkg;
    }
    //! Return energy concentration in milk (MJ/kg)
    /*!
     \returns energy concentration in milk (MJ/kg) as a double value.
    */
    double dailyMilkEnergyPerkg()//MJ per kg
    {
        double milkEnergyPerkg = 0;
        double milkEnergyContentPerkg=0;
        switch (speciesGroup)  //Use CSIRO method
        {
            case 1: milkEnergyContentPerkg = GlobalVars.Instance.GetECM(1, milkFat/10, milkNconc * 6.38 * 100) * 3.054;//Australian standards state 3.054 MJ/kg ECM
                break;
            case 2: break;
            case 3: milkEnergyContentPerkg = 0.0328 * milkFat + 0.0025 * 42 /*assume 6 weeks for day of lactation*/ + 2.203;
                break;
        }
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            //Use CSIRO method
            case 2: double efficiencyMilk = (0.02 * energyIntake / DMintake + 0.4);//SCA 1990 1.48
                milkEnergyPerkg = milkAdjustmentCoeff * milkEnergyContentPerkg / efficiencyMilk; // milkAdjustmentCoeff is  Multiplier to increase (>1) or decrease (<1) the energy requirement per unit of milk produced
                break;
            default: 
                string messageString=("Energy system for livestock not found");
                GlobalVars.Instance.Error(messageString);
                break;
        }
        return milkEnergyPerkg;
    }
    //! Returns energy remobilised for a given weight loss (MJ/kg)
    /*!
     \param weightloss weight lost (kg), a double
    */
    double dailyEnergyRemobilisation(double weightLoss)//MJ ME/day
    {
        double energyRemobilisation = weightLoss * growthEnergyDemandCoeff * 0.84;
        return energyRemobilisation;
    }
    //! Calculate weight loss needed to supply a given amount of remobilized energy.
    /*!
     \return weight lost as a double value (kg).
    */
    double dailyWeightLoss(double remobilisedEnergy)//MJ ME/day
    {
        avgProductionMeat = remobilisedEnergy / (growthEnergyDemandCoeff * 0.84);
        return avgProductionMeat;
    }
    //! Placeholder to allow for calculation of daily energy for grazing.
    double dailyEnergyForGrazing() //MJ ME/day
    {
        double retVal = 0;
        return retVal;
    }
    //! Calculate the intake energy level of a ruminant (multiples of maintenance).
    void calcEnergyLevel()
    {
        energyLevel=energyIntake/(dailymaintenanceEnergy() * GlobalVars.avgNumberOfDays);
    }
    //! Calculate endogenous Faecal Protein.
    double dailyEndogenousFaecalProtein()//g per animal/day
    {
        double endogenousFaecalProtein = 0;
        endogenousFaecalProtein = 15.2 * DMintake / GlobalVars.avgNumberOfDays;
        return endogenousFaecalProtein;
    }
    //!  calculate daily Faecal Protein.
    double dailyFaecalProtein()//g per animal per day - RedNex equation
    {
        double dailyDMI=DMintake/GlobalVars.avgNumberOfDays;
        double dailyNintake=1000*Nintake/GlobalVars.avgNumberOfDays;
        double faecalProtein = 0;
        if (dailyDMI < 5)
        {
            double faecalProtAtFiveKgDMI = 6.25 * (6.3 * 5 + 0.17 * 5.0 * (dailyNintake/dailyDMI) - 31.0);
            faecalProtein = (dailyDMI / 5.0) * faecalProtAtFiveKgDMI;
        }
        else
            faecalProtein = 6.25 * (6.3 * dailyDMI + 0.17 * dailyNintake - 31.0);

        faecalProtein = 6.25 * (0.04 * dailyNintake + (dailyDMI * dailyDMI * 1.8 / 6.25) + dailyDMI * 20.0 / 6.25);
        return faecalProtein;
    }
    //!  calculate daily Endogenous Urinary Protein.
    double dailyEndogenousUrinaryProtein()//g per animal/day
    {
        double endogenousUrinaryProtein = 0;
        switch (speciesGroup)
        {
            case 1:
                endogenousUrinaryProtein = 16.1 * Math.Log(liveweight) - 42.2;
                break;
            case 3:
                endogenousUrinaryProtein = 0.147 * liveweight + 3.375;
                break;
            default:
                string messageString1 = ("Protein system for livestock not found");
                GlobalVars.Instance.Error(messageString1);
                break;
        }
        return endogenousUrinaryProtein;
    }
    //! Calculates daily maintenance protein demand in g/day.
    /*!
     \return daily maintenance protein demand in g/day as a double value.
    */
    double dailymaintenanceProtein() //g per animal/day
    {
        double maintenanceProtein = 0;
        double endogenousUrinaryProtein = 0;
        double endogenousFaecalProtein = 0;
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            case 2:
                double efficiencyMaintenance = 0.7; //from Australian feeding standards
                endogenousUrinaryProtein= dailyEndogenousUrinaryProtein();
                endogenousFaecalProtein = dailyEndogenousFaecalProtein();
                maintenanceProtein = (endogenousUrinaryProtein + endogenousFaecalProtein) / efficiencyMaintenance;
                break;

            default:
                string messageString2 = ("Protein system for livestock not found");
                GlobalVars.Instance.Error(messageString2);
                break;

        }
        return maintenanceProtein;
    }
    //!  calculate daily Milk Protein Perkg.
    /*!
     \return a double value.
    */
    double dailyMilkProteinPerkg(double dailyProteinAvailableForProduction)//g per kg
    {
        double milkProteinPerkg = 0;
        double milkProteinContentPerkg = 0;
        milkProteinContentPerkg = 1000.0 * milkNconc * 6.38; 
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            case 2: double dailyME = energyIntake / GlobalVars.avgNumberOfDays;
                    double Nmet = dailyProteinAvailableForProduction / 6.25;
                    
                    milkProteinPerkg = milkProteinContentPerkg / efficiencyProteinMilk;
                break;
            default:
                string messageString = ("Protein system for livestock not found");
                GlobalVars.Instance.Error(messageString);
                break;
        }
        return milkProteinPerkg;
    }
    //!  calculate daily Grouth Protein.
    /*!
     \return a double value.
    */
    double dailyGrowthProteinPerkg() //g per kg
    {
        double growthProteinPerkg = 0;
        switch (GlobalVars.Instance.getcurrentEnergySystem())
        {
            case 1:
            case 2:
                if (speciesGroup == 1)
                {
                    double efficiencyGrowth = 0.7;
                    growthProteinPerkg = 1000.0 * growthNconc * 6.25 / efficiencyGrowth;
                }
                if (speciesGroup == 2) //need to make efficiency dependent on amino acids in diet
                {
                    double efficiencyGrowth = 0.7;
                    growthProteinPerkg = 1000.0 * growthNconc * 6.25 / efficiencyGrowth;
                }
                break;

            default:
                string messageString = ("Protein system for livestock not found");
                GlobalVars.Instance.Error(messageString);
                break;

        }
        return growthProteinPerkg;
    }
    //! Calculate protein made available via remobilisation of body reserves.
    /*!
     * \param weightLoss loss of weight in kg 
     *\return protein remobilized (g)
    */
    double dailyProteinRemobilisation(double weightLoss)
    {
        double proteinRemobilisation = weightLoss * growthNconc * 6.25;
        return proteinRemobilisation;
    }
    //! Returns maintenance energy per year (MJ).
    /*!
     \return maintenance energy (MJ) per year as a double value.
    */
    public double GetmaintenanceEnergy()//MJ ME per year
    {
        double maintenanceEnergy = dailymaintenanceEnergy() * GlobalVars.avgNumberOfDays;
        return maintenanceEnergy;
    }
    //! Returns grouwth energy per year (MJ).
    /*!
     \return grouwth energy per year (MJ) as a double value.
    */
    public double GetGrowthEnergy()//MJ ME per year
    {
        double growthEnergy = avgProductionMeat * dailyGrowthEnergyPerkg() * GlobalVars.avgNumberOfDays;
        return growthEnergy;
    }
    //! Returns milk energy per year (MJ).
    /*!
     \return milk energy per year (MJ) as a double value.
    */
    public double GetMilkEnergy()//MJ ME per year
    {
        double milkEnergy = 0;
        if (isDairy)
            milkEnergy= avgProductionMilk * dailyMilkEnergyPerkg() * GlobalVars.avgNumberOfDays;
        return milkEnergy;
    }
    //! Placeholder for maintenance protein. Not used at present.
    public double GetmaintenanceProtein()//kg protein per year
    {
        double maintenanceProtein = dailymaintenanceProtein() * GlobalVars.avgNumberOfDays/1000.0;
        return maintenanceProtein;
    }
    //! Returns protein partitioned to growth (kg/year).
    /*!
     \return protein in growth per year (kg) as a double value.
    */
    public double GetGrowthProtein()//kg protein per year
    {
        double growthProtein = avgProductionMeat * dailyGrowthProteinPerkg() * GlobalVars.avgNumberOfDays / 1000.0;
        return growthProtein;
    }
    //! Calculates the animal's annual energy demand.
    public void CalcEnergyDemand()//MJ per year
    {
        calcEnergyLevel();
        energyDemand = GetmaintenanceEnergy() + GetGrowthEnergy() + GetMilkEnergy();
    }
    //! Calculates daily production permitted by energy available.
    /*!
     * Production of milk or growth is first calculated from energy available.
     * Then there is a check to see if this production can be supported by the available N and
     * if not, the maximum production is reduced.
     \return bool which is false if there is an error.
    */
    public bool CalcMaximumProduction()
    {
        bool retVal = true;
        energyUseForMaintenance=0;
        energyUseForGrowth=0;
        energyUseForMilk=0;
        energyFromRemobilisation=0;
        maintenanceEnergyDeficit = 0;
        growthEnergyDeficit = 0;
        double proteinFromRemobilisation = 0;
        calcEnergyLevel();
        double MEAvail = 0.81* energyIntake;///energyIntake is in MJ per animal per year,
        double proteinSupply = Nintake * 6.25;
        double faecalProtein = dailyFaecalProtein() * GlobalVars.avgNumberOfDays/1000.0; //kg per year
        faecalN = faecalProtein / 6.25;
        energyUseForMaintenance = dailymaintenanceEnergy() * GlobalVars.avgNumberOfDays;
        MEAvail -= energyUseForMaintenance;
        energyFromRemobilisation = 0;
        if (avgProductionMeat < 0.0)  ///if parameter file says that weight loss is expected (e.g. in lactating dairy cows in early lactation)
        {
            energyFromRemobilisation = dailyEnergyRemobilisation(avgProductionMeat) * GlobalVars.avgNumberOfDays;
            proteinFromRemobilisation = dailyProteinRemobilisation(avgProductionMeat) * GlobalVars.avgNumberOfDays;
            liveweight-=avgProductionMeat;
        }
        MEAvail += energyFromRemobilisation;
        proteinSupply += proteinFromRemobilisation;
        double proteinAvailableForProduction = proteinSupply - faecalProtein;
        //First deal with a situation where feeding is below maintenance
        if ((MEAvail < 0)||(proteinAvailableForProduction<0))//feeding below maintenance for either energy or protein
        {
            double weightlLoss=0;
            if (avgProductionMeat>0.0)//wanted growth but not enough energy or protein available
                avgProductionMeat=0;
            if (MEAvail < 0)//remoblise energy
            {
                double remobilisationForMaintenance = 0;
                remobilisationForMaintenance=Math.Abs(MEAvail);
                energyFromRemobilisation+=remobilisationForMaintenance;
                weightlLoss=dailyWeightLoss(remobilisationForMaintenance /GlobalVars.avgNumberOfDays);
                double associatedProteinRemob=dailyProteinRemobilisation(weightlLoss) * GlobalVars.avgNumberOfDays;
                proteinAvailableForProduction+=associatedProteinRemob;
                proteinFromRemobilisation += associatedProteinRemob;
                avgProductionMeat = -weightlLoss;
                liveweight -= weightlLoss;
                MEAvail = 0.0;
                if (liveweight < 0)
                {
                    if (GlobalVars.Instance.getRunFullModel())
                    {
                        string messageString = name + " - liveweight has fallen below zero!";
                        GlobalVars.Instance.Error(messageString);
                        retVal = false;
                    }
                    retVal = false;
                }
            }
            if (proteinAvailableForProduction<0.0) //need to remobilise protein
            {
                weightlLoss=Math.Abs(proteinAvailableForProduction)/(dailyGrowthProteinPerkg() * GlobalVars.avgNumberOfDays);
                avgProductionMeat = -weightlLoss;
                liveweight += avgProductionMeat;
                proteinLimited = true;
                if (liveweight < 0)
                {
                    if (GlobalVars.Instance.getRunFullModel())
                    {
                        string messageString = name + " - liveweight has fallen below zero!";
                        GlobalVars.Instance.Error(messageString);
                        retVal = false;
                    }
                    retVal = false;
                }
                else
                    proteinAvailableForProduction=0;
            }
        }  //end of feeding below maintenance
        if (isDairy)
        {
            energyUseForGrowth = 0;
            if (avgProductionMeat > 0)//these animals are growing
            {
                energyUseForGrowth = avgProductionMeat * dailyGrowthEnergyPerkg() * GlobalVars.avgNumberOfDays;
                double proteinRequiredForGrowth=GetGrowthProtein();
                if ((MEAvail < energyUseForGrowth)||(proteinAvailableForProduction < proteinRequiredForGrowth))  //need to reduce growth
                {
                    if (MEAvail < energyUseForGrowth)//reduce growth to match energy available
                    {
                        growthEnergyDeficit = -1 * (energyUseForGrowth - MEAvail);
                        avgProductionMeat = MEAvail / (dailyGrowthEnergyPerkg() * GlobalVars.avgNumberOfDays);
                        energyUseForMilk = 0;
                        avgProductionMilk = 0;
                        MEAvail = 0;
                        proteinRequiredForGrowth=avgProductionMeat * dailyGrowthProteinPerkg() * GlobalVars.avgNumberOfDays;
                    }
                    if (proteinAvailableForProduction < proteinRequiredForGrowth)//reduce growth to match protein available
                    {
                        avgProductionMeat=proteinAvailableForProduction/(dailyGrowthProteinPerkg() * GlobalVars.avgNumberOfDays);//reduce growth to match available protein
                        proteinAvailableForProduction=0;
                        avgProductionMilk = 0;
                        proteinLimited = true;
                    }                    
                }
                else //There is enough energy and protein for milk production
                {
                    energyUseForMilk = MEAvail - energyUseForGrowth;
                    proteinAvailableForProduction -= proteinRequiredForGrowth; //enough protein to satisfy growth demand of dairy animals
                }
            }
            else //growth is zero or less
            {
                energyUseForMilk = MEAvail;
            }
            double thedailyMilkEnergyPerkg = dailyMilkEnergyPerkg();
            double energyLimitedMilk= energyUseForMilk / (thedailyMilkEnergyPerkg * GlobalVars.avgNumberOfDays);
            double dailyproteinAvailableForProduction = 1000 * proteinAvailableForProduction / GlobalVars.avgNumberOfDays;
            double proteinLimitedMilk = dailyproteinAvailableForProduction / dailyMilkProteinPerkg(dailyproteinAvailableForProduction) ;
            //Find out whether production is limited by energy or protein availability
            if (energyLimitedMilk < proteinLimitedMilk)
            {
                avgProductionMilk = energyLimitedMilk;
            }
            else
            {
                avgProductionMilk = proteinLimitedMilk;
                proteinLimited = true;
            }
            if (avgProductionMilk > 0.0)
            {
                double percentMilkProtein = (avgProductionMilk * milkNconc * 6.23 * 100.0) / avgProductionMilk;
                avgProductionECM = GlobalVars.Instance.GetECM(avgProductionMilk, (milkFat / 10.0), percentMilkProtein);
            }
            else
                avgProductionECM = 0;
            retVal = true;
        }
        else //these are meat only animals
        {
            energyUseForMilk = 0;         
            energyUseForGrowth = MEAvail - maintenanceEnergyDeficit;
            double energyLimitedGrowth= MEAvail / (dailyGrowthEnergyPerkg() * GlobalVars.avgNumberOfDays);
            double proteinLimitedGrowth=1000 * proteinAvailableForProduction / (dailyGrowthProteinPerkg() * GlobalVars.avgNumberOfDays);
            if (avgProductionMeat >= 0)
            {
                if (energyLimitedGrowth < proteinLimitedGrowth)
                {
                    avgProductionMeat = energyLimitedGrowth;
                }
                else
                {
                    avgProductionMeat = proteinLimitedGrowth;
                    proteinLimited = true;
                }
            }
            retVal = true;
        }
        return retVal;
    }
    //! Reads the feed ration for a single category of livestock
    /*!
     Details of feedstuffs are contained in feedstuffs.xml
    */
    public void intake()
    {
        concentrateEnergy = 0;
        for (int k = 0; k < feedRation.Count; k++)
        {
            feedItem anItem = feedRation[k];
            double amount = anItem.Getamount();
            DMintake += GlobalVars.avgNumberOfDays * amount;
            energyIntake += GlobalVars.avgNumberOfDays * amount * anItem.Getenergy_conc();
            diet_ash += GlobalVars.avgNumberOfDays * amount * anItem.Getash_conc();
            Nintake += GlobalVars.avgNumberOfDays * amount * anItem.GetN_conc();
            Cintake += GlobalVars.avgNumberOfDays * amount * anItem.GetC_conc();
            diet_fat += GlobalVars.avgNumberOfDays * amount * anItem.Getfat_conc();
            diet_fibre += GlobalVars.avgNumberOfDays * amount * anItem.Getfibre_conc();
            diet_NDF += GlobalVars.avgNumberOfDays * amount * anItem.GetNDF_conc();
            diet_nitrate += GlobalVars.avgNumberOfDays * amount * anItem.GetNitrate_conc();
            digestibilityDiet += amount * anItem.GetDMdigestibility();
        }
        if (feedRation.Count==0)
        {
            string message1 = "Error; no feed provided for " + name;
            GlobalVars.Instance.Error(message1);
        }
        digestibilityDiet /= (DMintake/ GlobalVars.avgNumberOfDays);
        for (int j = 0; j < feedRation.Count; j++)
        {
            if ((feedRation[j].GetisGrazed())||(feedRation[j].GetfedAtPasture()))
                DMgrazed += feedRation[j].Getamount() * GlobalVars.avgNumberOfDays;
        }
        propDMgrazed = DMgrazed / GetDMintake();
        //if the proportion of excreta deposited to the fields has not been set using the nightTimeProp variable of housing,
        // assume it is the same as the DM intake
        if (propExcretaField < 0.0)
            propExcretaField = propDMgrazed;
        concentrateDM = GetConcentrateDM() * GlobalVars.avgNumberOfDays;
        concentrateEnergy = GetConcentrateEnergy() * GlobalVars.avgNumberOfDays;
        FE = (0.75 * 1000 * (energyIntake / DMintake) - 1883) / 7720;
    }
    //! Calculates the enteric methane emission.
    /*!
     \return the enteric methane (kg/year) as a double value.
    */
    public double entericMethane()
    {
        double numDays = GlobalVars.avgNumberOfDays;
        double methane = 0; //initially in grams
        double grossEnergyIntake = 18.4 * DMintake;
        switch (GlobalVars.Instance.getcurrentInventorySystem())
        {
            case 1: //IPCC 2006 methodology
                methane = grossEnergyIntake * entericYm / 55.65;//1.13
            break;
            case 2:
            case 3://IPCC 2019 methodology. 
                double diet_NDF_prop = diet_NDF / DMintake;
                if (speciesGroup == 1) //Cattle
                {
                    if (isDairy)
                    {
                        if ((digestibilityDiet >= 0.7) && (diet_NDF_prop <= 0.35))
                            entericYm = 0.057;
                        if ((digestibilityDiet >= 0.7) && (diet_NDF_prop > 0.35))
                            entericYm = 0.06;
                        if ((digestibilityDiet < 0.7) && (digestibilityDiet >= 0.63) && (diet_NDF_prop > 0.35))
                            entericYm = 0.063;
                        if ((digestibilityDiet < 0.63) && (diet_NDF_prop > 0.35))
                            entericYm = 0.065;
                    }
                    else  //Other cattle
                    {
                        if (LivestockType == 2)
                        {
                            if (digestibilityDiet <= 0.62)
                                entericYm = 0.07;
                            if ((digestibilityDiet < 0.71) && (digestibilityDiet >= 0.62))
                                entericYm = 0.063;
                            if (digestibilityDiet >= 0.72)
                                entericYm = 0.057;
                            if (digestibilityDiet >= 0.7)
                                entericYm = 0.04;
                            if (digestibilityDiet > 0.75)
                                entericYm = 0.03;
                        }
                    }
                }
                methane = grossEnergyIntake * entericYm / 55.65;//1.13
                break;
        }
        double methane_reduction = 0;
        if (diet_nitrate > 0)  //allow feeding with nitrate to reduce methane emissions
        {
            double mol_nitrate = diet_nitrate / 62; //mols of nitrate
            double mol_methane = methane / 16; //mols of methane
            methane_reduction = nitrateEfficiency * mol_nitrate / mol_methane;
            if (methane_reduction > 1)
                methane_reduction = 1.0;
        }
        methane *= (1-methane_reduction);
        return methane;
    }
    //! Calculate the carbon dynamics of the livestock.
    public void DoCarbon()
    {
        milkC = GlobalVars.avgNumberOfDays * avgProductionMilk * milkCconc;
        double totalGrowthC = GlobalVars.avgNumberOfDays * avgProductionMeat * growthCconc;
        mortalitiesC = mortalityCoefficient / 2 * totalGrowthC;
        growthC = totalGrowthC - mortalitiesC;
        double ashConc = diet_ash / DMintake;
        faecalC = Cintake * (1 - digestibilityDiet) / (1 - ashConc);
        urineC = urineProp * Cintake;
        CH4C = entericMethane() * 12 / 16;  //convert from kg CH4 to kg CH4-C
        CO2C = Cintake - (milkC + growthC + mortalitiesC + faecalC + urineC + CH4C);  //CO2 is difference
        //Calculate the methane emission from dung deposited on pasture
        CexcretionToPasture = propExcretaField * (faecalC + urineC);
        CCH4GR = 0.0; //methane emission during grazing
        if (CexcretionToPasture > 0)
        {
            double MCF = 0;
            double grazingVS = CexcretionToPasture / GlobalVars.Instance.getalpha(); ; //calculate volatile solids excreted
            switch (GlobalVars.Instance.getcurrentInventorySystem())
            {
                case 1: //IPCC 2006 methodology
                    double aveTemperature = GlobalVars.Instance.theZoneData.GetaverageAirTemperature();  //get regional average temperature
                    if (aveTemperature < 14.5)
                        MCF = 0.01;
                    if ((aveTemperature >= 14.5) && (aveTemperature < 25.5))
                        MCF = 0.015;
                    if (aveTemperature >= 25.5)
                        MCF = 0.02;
                    break;
                case 2://IPCC 2019 methodology. 
                    MCF = 0.0047;
                    break;
            }
            CCH4GR = MCF * grazingVS * Bo * 0.67 * 12 / 16;   //calculate methane emission and convert to CH4-C
        }
    }
    //! Calculate the nitrogen dynamics of the livestock.
    public void DoNitrogen()
    {
        milkN = GlobalVars.avgNumberOfDays * avgProductionMilk * milkNconc;
        double totalGrowthN = 0;  //N deposited in weight gain
        if (avgProductionMeat>=0)
            totalGrowthN= GlobalVars.avgNumberOfDays * avgProductionMeat * growthNconc;
        else
            totalGrowthN = avgProductionMeat * GlobalVars.avgNumberOfDays * dailyGrowthProteinPerkg()/6.25;  //this will be negative
        mortalitiesN = mortalityCoefficient / 2 * totalGrowthN;  //death is assumed to occur at half growth
        growthN = totalGrowthN - mortalitiesN;
        urineN = Nintake - (milkN + growthN + mortalitiesN + faecalN);  //urine N is obtained by difference
        if (urineN < -1E-10)  //there is a negative N balance - this is not permitted
        {
            Write();
            string message1 = "Error; urine N for " + name + " (population "+ avgNumberOfAnimal + ") has gone negative";
            GlobalVars.Instance.Error(message1);
        }        
        NexcretionToPasture = propExcretaField * (faecalN + urineN);
    }
    //! Do all the calculations for a ruminant animal.
    /*!
     Includes intake, production, carbon and nitrogen dynamics, and excretion
    */
    public void DoRuminant()
    {
        intake();
        CalcMaximumProduction();
        CalcEnergyDemand();
        DoCarbon();
        DoNitrogen();
        GetExcretaDeposition();
    }
    //! Calculate the excreta deposition on pasture.
    public void GetExcretaDeposition()
    {
        if ((propDMgrazed == 0.0)&&(propExcretaField!=0.0))
        {
            string messageString = name + " - attempt to enforce manure partitioning when there is no grazing";
            GlobalVars.Instance.Error(messageString);
        }
        double[] DM = new double[GlobalVars.Instance.getmaxNumberFeedItems()];  //array to collect amounts of dry matter grazed or fed to livestock in the field
        for (int i = 0; i < GlobalVars.Instance.getmaxNumberFeedItems(); i++)
            DM[i] = 0;

        double sum = 0;
        for (int j = 0; j < feedRation.Count; j++)
        {
            if (feedRation[j].GetisGrazed())
            {
                int feedCode = feedRation[j].GetFeedCode();
                double temp = avgNumberOfAnimal * GlobalVars.Instance.GetavgNumberOfDays() * feedRation[j].Getamount();
                grazedN += feedRation[j].Getamount() * feedRation[j].GetN_conc() * GlobalVars.Instance.GetavgNumberOfDays();
                grazedC += feedRation[j].Getamount() * feedRation[j].GetC_conc() * GlobalVars.Instance.GetavgNumberOfDays();
                grazedDM += feedRation[j].Getamount() * GlobalVars.Instance.GetavgNumberOfDays();
                DM[feedCode] += temp;
                GlobalVars.Instance.grazedArray[feedCode].ruminantDMgrazed += temp;
                sum += temp;
            }
            if (feedRation[j].GetfedAtPasture())
            {
                double temp = avgNumberOfAnimal * GlobalVars.Instance.GetavgNumberOfDays() * feedRation[j].Getamount();
                pastureFedN += feedRation[j].Getamount() * feedRation[j].GetN_conc() * GlobalVars.Instance.GetavgNumberOfDays();
                pastureFedC += feedRation[j].Getamount() * feedRation[j].GetC_conc() * GlobalVars.Instance.GetavgNumberOfDays();
            }
        }
        double excretaN = 0;
        for (int i = 0; i < GlobalVars.Instance.getmaxNumberFeedItems(); i++)
        {
            if (DM[i]>0)
            {
                double theUrineN = propExcretaField * avgNumberOfAnimal * urineN * DM[i] / sum;
                double theUrineC = propExcretaField * avgNumberOfAnimal * urineC * DM[i] / sum;
                double theFaecalN = propExcretaField * avgNumberOfAnimal * faecalN * DM[i] / sum;
                double theFaecalC = propExcretaField * avgNumberOfAnimal * faecalC * DM[i] / sum;
                double theCH4C = avgNumberOfAnimal * CCH4GR * DM[i] / sum;
                GlobalVars.Instance.grazedArray[i].urineC += theUrineC; 
                GlobalVars.Instance.grazedArray[i].urineN += theUrineN;
                GlobalVars.Instance.grazedArray[i].faecesC += theFaecalC;
                GlobalVars.Instance.grazedArray[i].faecesN += theFaecalN;
                GlobalVars.Instance.grazedArray[i].fieldCH4C += theCH4C;
                excretaN += theFaecalN + theUrineN;
            }
        }
        if ((excretaN==0) && (housingDetails.Count == 0))
        {
            string message1 = "Error; animals are fed at pasture only but no pasture is consumed. Livestock name = " + name;
            GlobalVars.Instance.Error(message1);
        }
    }
    //!  Get all FeedItem Used.
    /*!
     more details.
    */
    public void GetAllFeedItemsUsed()
    {
        for (int i = 0; i < GlobalVars.Instance.getmaxNumberFeedItems(); i++)
        {
            for (int j = 0; j < feedRation.Count; j++)
                 if (feedRation[j].GetFeedCode() == i) 
                 {
                    feedItem afeedItem = new feedItem(feedRation[j]);
                    afeedItem.setFeedCode(i);
                    afeedItem.AddFeedItem(feedRation[j], false);
                    afeedItem.Setamount(avgNumberOfAnimal * GlobalVars.Instance.GetavgNumberOfDays() * feedRation[j].Getamount());
/*                    if ((afeedItem.GetisGrazed())&&(GlobalVars.Instance.GetstrictGrazing()))
                   {
                        afeedItem.Setname(afeedItem.GetName() + ", grazed");
                        afeedItem.setFeedCode(afeedItem.GetFeedCode() + 1000);
                        GlobalVars.Instance.allFeedAndProductsUsed[i+1000].composition.AddFeedItem(afeedItem, false);
                    }
                    else*/
                        GlobalVars.Instance.allFeedAndProductsUsed[i].composition.AddFeedItem(afeedItem, false);
                    //break;
                }
        }
    }
    //!  Check Livestock C Balance. Returing a boolean value.
    /*!
     \return a boolean value.
    */
    public bool CheckLivestockCBalance()
    {
        bool retVal = false;
        double Cout = urineC + growthC + faecalC + milkC + mortalitiesC;
        double CLost = CH4C + CO2C;
        double Cbalance = Cintake - (Cout + CLost);
        double diff = Cbalance / Cintake;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
           
                double errorPercent = 100 * diff;
               
                string messageString=("Error; Livestock C balance error is more than the permitted margin of "
                    + tolerance.ToString() +"\n");
                messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
                GlobalVars.Instance.Error(messageString);
        }
        return retVal;
    }
    //!  Check Livestock N Balance. Returing a boolean value.
    /*!
     \return a boolean value.
    */
    public bool CheckLivestockNBalances()
    {
        bool retVal = false;
        double Nout = urineN + growthN + faecalN + milkN + mortalitiesN;
        double Nbalance = Nintake - Nout;
        double diff = Nbalance / Nintake;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
                double errorPercent = 100 * diff;
                string messageString = ("Error; Livestock N balance error is more than the permitted margin of "
                    + tolerance.ToString() + "\n");
                messageString += ("Percentage error = " + errorPercent.ToString("0.00") + "%");
                GlobalVars.Instance.Error(messageString);  
        }
        return retVal;
    }
    //! Output details of livestock to xml and Excel files.
    public void Write()
    {
        double numofDaysInYear = GlobalVars.avgNumberOfDays;
        GlobalVars.Instance.writeStartTab("LiveStock");
        GlobalVars.Instance.writeInformationToFiles("nameLiveStock", "Name", "-", name, parens);
        GlobalVars.Instance.writeInformationToFiles("speciesGroup", "Species identifier", "-", speciesGroup, parens);
        GlobalVars.Instance.writeInformationToFiles("LivestockType", "Livestock type", "", LivestockType, parens);
        GlobalVars.Instance.writeInformationToFiles("liveweight", "Liveweight", "kg", liveweight, parens);
        GlobalVars.Instance.writeInformationToFiles("isRuminant", "Is a ruminant", "-", isRuminant, parens);
        GlobalVars.Instance.writeInformationToFiles("avgNumberOfAnimal", "Annual average number of animals", "-", avgNumberOfAnimal, parens);

        GlobalVars.Instance.writeInformationToFiles("DMintake", "Intake of DM", "kg/day", DMintake / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("energyIntake", "Intake of energy", "MJ/day", energyIntake / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("energyUseForGrowth", "Energy used for growth", "MJ/day", energyUseForGrowth / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("energyUseForMilk", "Energy used for milk production", "MJ/day", energyUseForMilk / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("energyFromRemobilisation", "Energy supplied by remobilisation", "MJ/day", energyFromRemobilisation / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("energyUseForMaintenance", "Energy used for maintenance", "MJ/day", energyUseForMaintenance / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("maintenanceEnergyDeficit", "Maintenance energy deficit", "MJ/day", maintenanceEnergyDeficit / numofDaysInYear, parens);
        GlobalVars.Instance.writeInformationToFiles("growthEnergyDeficit", "Growth energy deficit", "MJ/day", growthEnergyDeficit / numofDaysInYear, parens);
        //GlobalVars.Instance.writeInformationToFiles("milkEnergyDeficit", "Deficit in energy required for milk production", "MJ", milkEnergyDeficit);

        GlobalVars.Instance.writeInformationToFiles("diet_ash", "Ash in diet", "kg", diet_ash, parens);
        GlobalVars.Instance.writeInformationToFiles("diet_fibre", "Fibre in diet", "kg", diet_fibre, parens);
        GlobalVars.Instance.writeInformationToFiles("diet_fat", "Fat in diet", "kg", diet_fat, parens);
        GlobalVars.Instance.writeInformationToFiles("diet_NDF", "NDF  in diet", "kg", diet_NDF, parens);
        GlobalVars.Instance.writeInformationToFiles("digestibilityDiet", "Diet DM digestibility", "kg/kg", digestibilityDiet, parens);

        GlobalVars.Instance.writeInformationToFiles("Cintake", "Intake of C", "kg", Cintake, parens);
        GlobalVars.Instance.writeInformationToFiles("milkC", "C in milk", "kg", milkC, parens);
        GlobalVars.Instance.writeInformationToFiles("growthC", "C in growth", "kg", growthC, parens);
        GlobalVars.Instance.writeInformationToFiles("urineCLiveStock", "C in urine", "kg", urineC, parens);
        GlobalVars.Instance.writeInformationToFiles("faecalCLiveStock", "C in faeces", "kg", faecalC, parens);
        GlobalVars.Instance.writeInformationToFiles("CH4C", "CH4-C emitted", "kg", CH4C, parens);
        GlobalVars.Instance.writeInformationToFiles("CO2C", "CO2-C emitted", "kg", CO2C, parens);
        //GlobalVars.Instance.writeInformationToFiles("energyLevel", "??", "??", energyLevel);
        GlobalVars.Instance.writeInformationToFiles("Nintake", "Intake of N", "kg", Nintake, parens);
        GlobalVars.Instance.writeInformationToFiles("milkN", "N in milk", "kg", milkN, parens);
        GlobalVars.Instance.writeInformationToFiles("growthN", "N in growth", "kg", growthN, parens);
        GlobalVars.Instance.writeInformationToFiles("mortalitiesN", "N in mortalities", "kg", mortalitiesN, parens);
        GlobalVars.Instance.writeInformationToFiles("urineN", "N in urine", "kg", urineN, parens);
        GlobalVars.Instance.writeInformationToFiles("faecalN", "N in faeces", "kg", faecalN, parens);

        GlobalVars.Instance.writeInformationToFiles("avgDailyProductionMilk", "Average daily milk production", "kg/day", avgProductionMilk, parens);
        double temp = avgProductionMilk * GlobalVars.avgNumberOfDays;
        GlobalVars.Instance.writeInformationToFiles("avgProductionMilk", "Average yearly milk production", "kg", temp, parens);
        if (avgProductionMilk > 0.0)
        {
            double percentMilkProtein = (milkN * 6.23 * 100.0) / (avgProductionMilk * GlobalVars.avgNumberOfDays);
            avgProductionECM = GlobalVars.Instance.GetECM(avgProductionMilk, (milkFat / 10.0), percentMilkProtein);
        }
        else
            avgProductionECM = 0;
        GlobalVars.Instance.writeInformationToFiles("avgProductionECM", "Average energy-corrected milk production", "kg/day", avgProductionECM * 365.0, parens);
        GlobalVars.Instance.writeInformationToFiles("avgDailyProductionECM", "Average daily energy-corrected milk production", "kg/day", avgProductionECM, parens);
        GlobalVars.Instance.writeInformationToFiles("avgProductionMeat", "Average weight change", "g/day", avgProductionMeat * 1000.0, parens);
        //GlobalVars.Instance.writeInformationToFiles("housedDuringGrazing", "??", "??", housedDuringGrazing);
        for (int i = 0; i < housingDetails.Count; i++)
            housingDetails[i].WriteXML();
        if (!GlobalVars.Instance.getRunFullModel())
            GlobalVars.Instance.writeEndTab();
    }

    //! Calculate C and N flows using defined animal production. 
    /*!
     Calculate N and C flows, using defined start and end weights, plus duration of production cycle. Suitable for all animal types.
    */
    public void DoDefinedProduction()
    {
        DoCarbon();
        faecalN = Nintake * (1 - digestibilityDiet);  //in kg/yr, assumes protein digestib = DM digestib
        DoNitrogen();
        double NUE = growthN / Nintake;
        if (NUE > maxNuseEfficiency)
        {
            string messageString = ("N use efficiency of " + name + " is " + NUE + " which is greater than the maximum permitted");
            GlobalVars.Instance.Error(messageString);
        }
    }

    //! Calculate pig maintenance energy demand. Uses NRC equations. Not currently used.
    /*!
     \return  pig maintenance energy requirement (MJ/day) as a double value.
    */
    public double pigMaintenanceEnergy()
    {
        double retVal = 0.44 * Math.Pow(liveweight, 0.75);
        return retVal;
    }

    //! Calculate pig weight gain. Uses NRC equations. Not currently used.  
    /*!
     \param energyAvailable in MJ 
     \return change in weight in kg as a double value.
    */
    public double GetPigGrowth(double energyAvailable)
    {
        double retVal = energyAvailable / 44.35;
        return retVal;
    }
    //! Calculate pig lactation energy. Uses NRC equations. Not currently used.
    /*!
     \param numPiglets number of piglets lactating as a double argument.
     \param birthWt birthweight of piglets (kg) as a double argument.
     \param weanedWt weight at weaning (kg) as a double argument.
     \param duration duration of lactation (days) as a double argument.
     \return energy use for lactation (MJ) as a double value.
    */
    public double pigLactationEnergy(double numPiglets, double birthWt, double weanedWt, double duration)
    {
        double retVal = 0;
        double growthRate = (weanedWt - birthWt) / duration;
        retVal = 4.184 * numPiglets * (6.83 * ((weanedWt - birthWt) / duration) - 125);
        return retVal;
    }
    //! Calculate pig
    /*!
     end of US pig model.
    */
    public void DoPig()
    {
        intake();
        DoGrowingPigs();
    }
    //! a normal member. Do Growing pigs
    /*!
     more details.
    */
    public void DoGrowingPigs()
    {
        inputProduction = true;
        double FE = (0.75 * 1000 * (energyIntake / DMintake) - 1883) / 7720;
        double FEIntake = DMintake * FE;
        if (inputProduction)
        {
            DoDefinedProduction();
        }
        else
        {
            avgProductionMeat = FEIntake / (2.82 * GlobalVars.avgNumberOfDays);
            CalcMaximumPigProduction();
            if ((startWeight > 0) && (duration > 0))
                endWeight = startWeight + duration * avgProductionMeat;
        }
        double FEperKgPigProduced = (duration * FEIntake / GlobalVars.avgNumberOfDays) / (endWeight - startWeight);
        double ProteinperFE = 6.25 * Nintake / FEIntake;
        double NperPigProduced = duration * (faecalN + urineN) / GlobalVars.avgNumberOfDays;
    }
    //! Calculate maximum pig production. Placeholder for when pig production can be calduated from feed ration
    /*!
     \return a boolean value.
    */

    public bool CalcMaximumPigProduction()///calculate daily production permitted by energy available
    {
        bool retVal = true;
      
        return retVal;
    }
    //! Calculate the dry matter fed as concentrate feed.
    /*!
     \return dry matter of concentrate feed fed (kg) as a double value.
    */
    public double GetConcentrateDM()
    {
        double retVal = 0;
        for (int i = 0; i < feedRation.Count; i++)
        {
            feedItem afeedItem = feedRation[i];
            if (afeedItem.isConcentrate())
                retVal += afeedItem.Getamount();
        }
        return retVal;
    }
    //! Calculate the energy supplied by concentrate feed (MJ).
    /*!
     \return energy supplied by concentrate feed (MJ) as a double value.
    */
    public double GetConcentrateEnergy()
    {
        double retVal = 0;
        for (int i = 0; i < feedRation.Count; i++)
        {
            feedItem afeedItem = feedRation[i];
            if (afeedItem.isConcentrate())                
                retVal += afeedItem.Getamount() * afeedItem.Getenergy_conc();
        }
        return retVal;
    }
    //! Set proportion of excreta deposited to housing. 
    /*!
     * Used to force the proportion of excreta deposited to housing (and thus also to fields)
     * Is an alternative to ëstimating the proportion using the grazed dry matter as a proportion of total dry matter intake 
     \param PropExcretalDepositionHousing proportion of excreta deposited to housing as a double argument.
    */
    public void  SetExcretalDistributionHousing(double PropExcretalDepositionHousing)
    {
        if (housingDetails.Count != 1)
        {
            string messageString = name + " - attempt to enforce manure partitioning to more than one animal house";
            GlobalVars.Instance.Error(messageString);
        }
        propExcretaField = (1 - PropExcretalDepositionHousing);
    }
    //! Output to file details of the livestock.
    /*!
     Writes to both XML and Excel (csv) files
    */
    public void WriteLivestockFile()
    {
        int times = 1;
        if (GlobalVars.Instance.headerLivestock == false)
            times = 2;
        for (int j = 0; j < times; j++)
        {
            GlobalVars.Instance.writeLivestockFile("name", "name", "-", name, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("avgNumberOfAnimal", "avgNumberOfAnimal", "-", avgNumberOfAnimal, "livestock", 0);

            GlobalVars.Instance.writeLivestockFile("diet_ash", "diet_ash", "kg/day", diet_ash / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("diet_fibre", "diet_fibre", "kg/day", diet_fibre / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("diet_fat", "diet_fat", "kg/day", diet_fat / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("diet_NDF", "diet_NDF", "kg/day", diet_NDF / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("diet_nitrate", "diet_nitrate", "kg/day", diet_nitrate / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("digestibilityDiet", "digestibilityDiet", "-", digestibilityDiet, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyIntake", "energyIntake", "MJ ME/day", energyIntake / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyUseForMaintenance", "energyUseForMaintenance", "MJ ME/day", energyUseForMaintenance / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyUseForGrowth", "energyUseForGrowth", "MJ ME/day", energyUseForGrowth / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyUseForMilk", "energyUseForMilk", "MJ ME/day", energyUseForMilk / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyUseForGrazing", "energyUseForGrazing", "MJ ME/day", energyUseForGrazing / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyFromRemobilisation", "energyFromRemobilisation", "MJ ME/day", energyFromRemobilisation / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("maintenanceEnergyDeficit", "maintenanceEnergyDeficit", "MJ ME/day", maintenanceEnergyDeficit / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("growthEnergyDeficit", "growthEnergyDeficit", "MJ ME/day", growthEnergyDeficit / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("energyLevel", "energyLevel", "-", energyLevel, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("DMintake", "DMintake", "kg/day", DMintake / GlobalVars.avgNumberOfDays, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("concentrateDM", "concentrateDM", "kg/yr", concentrateDM, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("grazedDM", "grazedDM", "kg/day", grazedDM / GlobalVars.avgNumberOfDays, "livestock", 0);
            if (proteinLimited)
                GlobalVars.Instance.writeLivestockFile("proteinLimited", "proteinLimited", "-", 1, "livestock", 0);
            else
                GlobalVars.Instance.writeLivestockFile("proteinLimited", "proteinLimited", "-", 0, "livestock", 0);

            GlobalVars.Instance.writeLivestockFile("Cintake", "Cintake", "kg/yr", Cintake, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("milkC", "milkC", "kg/yr", milkC, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("growthC", "growthC", "kg/yr", growthC, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("mortalitiesC", "mortalitiesC", "kg/yr", mortalitiesC, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("urineC", "urineC", "kg/yr", urineC, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("faecalC", "faecalC", "kg/yr", faecalC, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("CexcretionToPasture", "CexcretionToPasture", "kg/yr", CexcretionToPasture, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("LiveCH4C", "LiveCH4C", "kg/yr", CH4C, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("LiveCO2C", "LiveCO2C", "kg/yr", CO2C, "livestock", 0);

            GlobalVars.Instance.writeLivestockFile("Nintake", "Nintake", "kg/yr", Nintake, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("grazedN", "grazedN", "kg/yr", grazedN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("milkN", "milkN", "kg/yr", milkN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("growthN", "growthN", "kg/yr", growthN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("mortalitiesN", "mortalitiesN", "kg/yr", mortalitiesN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("urineN", "urineN", "kg/yr", urineN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("faecalN", "faecalN", "kg/yr", faecalN, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("NexcretionToPasture", "NexcretionToPasture", "kg/yr", NexcretionToPasture, "livestock", 0);

            GlobalVars.Instance.writeLivestockFile("avgProductionMeat", "avgProductionMeat", "g/day", avgProductionMeat*1000, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("avgProductionMilk", "avgProductionMilk", "kg/day", avgProductionMilk, "livestock", 0);
            GlobalVars.Instance.writeLivestockFile("avgProductionECM", "avgProductionECM", "kg/day", avgProductionECM, "livestock", 0);
            for (int i = 0; i < housingDetails.Count; i++)
                housingDetails[i].WriteXls();
            GlobalVars.Instance.writeLivestockFile("NULL", "NULL", "-", "-", "livestock", 1);
            GlobalVars.Instance.headerLivestock = true;
        }
    }
}
