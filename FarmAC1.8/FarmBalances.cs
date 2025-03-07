﻿using System;
using System.Collections.Generic;
using System.Xml;
/*! FarmBalanceClass calculates and displays farm C, N and GHG balances, plus ancillary information*/
class farmBalanceClass
{

    //! import of N in livestock feed (kg)
    double liveFeedImportN = 0;
    //!import of N in animal feed (kg)
    double livestockNintake = 0;

    ///N in grazed feed (kg)
    double liveGrazedN = 0;
    ///input of excretal N to housing (kg)
    double liveToHousingN = 0;
    ///deposition of N by grazing livestock (kg)
    double liveToFieldN = 0;

    ///excretal N deposited in housing (kg)
    double houseInFromAnimalsN = 0;
    ///Gaseous loss of N from housing (kg)
    double houseLossN = 0;
    ///N in excreta from housing to storage (minus gaseous N losses) (kg)
    double houseExcretaToStorageN = 0;
    //! N fed to livestock in animal housing (kg)
    double NinFeedFedInHouse = 0;
    ///N input to biogas plant as supplementary feedstock (kg)
    double biogasSupplN = 0;
    ///C input to biogas plant as supplementary feedstock (kg)
    double biogasSupplC = 0;

    ///N input to storage from excreta deposited in housing (minus NH3 emission) (kg)
    double storageFromHouseN = 0;
    ///N input to storage in bedding (kg)
    double storageFromBeddingN = 0;
    ///N input to storage in wasted feed (kg)
    double storageFromFeedWasteN = 0;
    ///N lost in gaseous emissions from storage (kg)
    double storageGaseousLossN = 0;
    ///N lost in runoff from storage (kg)
    double storageRunoffN = 0;

    ///N in imported manure (kg)
    double manureImportN = 0;
    ///N in exported manure (kg)
    double manureExportN = 0;
    //! manure N ex storage (kg)
    double manureNexStorage = 0;


    ///N in manure applied to fields (kg)
    double manureToFieldN = 0;
    ///N in gaseous emissions in the field
    double fieldGaseousLossN = 0;
    ///N in nitrate leaching
    double fieldNitrateLeachedN = 0;
    ///N removed by grazing animals
    double grazedN = 0;
    double Nharvested = 0;
    ///Change in soil N storage
    double changeSoilN = 0;
    /// <summary>
    /// Enteric greenhouse gas emissions in CO2 equivalents
    /// </summary>
    double entericCH4CO2Eq = 0.0;
    /// <summary>
    /// Greenhouse gas emissions from manure storage in CO2 equivalents
    /// </summary>
    double manureCH4CO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions as N2O from manure N, in CO2 equivalents
    /// </summary>
    double manureN2OCO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from mineralised soil N, in CO2 equivalents
    /// </summary>
    double fieldN2OCO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from field CH4 emissions, in CO2 equivalents
    /// </summary>
    double fieldCH4CO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from hydrolysis of urea, in CO2 equivalents
    /// </summary>
    double fieldCO2 = 0;
    /// <summary>
    /// Direct greenhouse gas emissions in CO2 equivalents
    /// </summary>
    double directGHGEmissionCO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from changes in soil C, in CO2 equivalents
    /// </summary>
    double soilCO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from NH3 emissions from livestock housing, in CO2 equivalents
    /// </summary>
    double housingNH3CO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from NH3 emissions from manure storage, in CO2 equivalents
    /// </summary>
    double manurestoreNH3CO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from NH3 emissions from field-applied manure, in CO2 equivalents
    /// </summary>
    double fieldmanureNH3CO2Eq = 0;
    /// <summary>
    /// Greenhouse gas emissions from NH3 emissions from fertilizer, in CO2 equivalents
    /// </summary>
    double fieldfertNH3CO2Eq =0;
    /// <summary>
    /// Greenhouse gas emissions from leached nitrate, in CO2 equivalents
    /// </summary>
    double leachedNCO2Eq = 0;
    /// <summary>
    /// Indirect greenhouse gas emissions in CO2 equivalents
    /// </summary>
    double indirectGHGCO2Eq = 0;
    //!carbon fixation by crops (kg)
    double carbonFromPlants = 0;
    //! carbon imported in livestock manure (kg)
    double Cmanimp = 0;
    //!carbon imported in animal feed (kg)
    double CPlantProductImported = 0;
    //!carbon in bedding (kg)
    double CinBedding = 0;
    //! carbon in imported bedding (kg)
    double CinImportedBedding = 0;
    //!Carbon in synthetic fertilisers  (kg)
    double CinImportedFertiliser = 0;
    //!Total amount of C input to farm (kg)
    double CInput = 0;
    //!Carbon exported in milk (kg)
    double Cmilk = 0;
    //!Carbon exported in meat (kg)
    double Cmeat = 0;
    //!Carbon exported in dead animals (kg)
    double Cmortalities = 0;
    //Carbon exported in manure  (kg)
    double Cmanexp = 0;
    //!Carbon ín sold crop products (kg)
    double CinCropProductsSold = 0;
    //!Total C exported from farm in products and manure (kg)
    double Cexport = 0;
    //!total carbon loss to environment (kg)
    double CLost = 0;
    //!Carbon lost as methane from livestock (kg)
    double livestockCH4C = 0;
    //!Carbon lost as carbon dioxide from livestock (kg)
    double livestockCO2C = 0;
    //!Total carbon loss from livestock (CO2 + CH4) (kg)
    double livstockCLoss = 0;
    //! Carbon in excreta deposited in livestock housing (kg)
    double CInhouseExcreta = 0;
    //!Carbon lost as carbon dioxide from urea hydrolysis in housing (kg)
    double housingCLoss = 0;
    //! Carbon in wasted feed (kg)
    double CinFeedWaste = 0;
    //! Carbon in feed fed in livestock housing (kg)
    double CinFeedFedInHouse = 0;
    //! Carbon in manure sent to manure storage  (kg)
    double CinManureSentToStorage = 0;
    //!Carbon lost as methane from manure storage (kg)
    double manurestoreCH4C = 0;
    //!Carbon lost as carbon dioxide from manure storage (kg)
    double manurestoreCO2C = 0;
    //!change in soil C (kg)
    double CDeltaSoil = 0;
    //!emissions of CO2 from soil (kg)
    double soilCO2_CEmission = 0;
    //!emissions of CH4 from excreta deposited during grazing (kg)
    double soilCH4_CEmission = 0;
    //!C lost from stored plant products (kg)
    double processStorageCloss = 0;
    //!C in organic matter leached from soil (kg)
    double soilCleached = 0;
    //!CO-C from burnt crop residues (kg)
    double burntResidueCOC = 0;
    //!Black C from burnt crop residues (kg)
    double burntResidueBlackC = 0;
    //!CO2-C from burnt crop residues (kg)
    double burntResidueCO2C = 0;
    //!C in CH4 from biogas reactor (kg)
    double biogasCH4C = 0;
    //!C in CO2 from biogas reactor (kg)
    double biogasCO2C = 0;
    //!C in manure organic matter lost in runoff from manure storage (kg)
    double manurestoreRunoffC = 0;
    //!C in crop residues remaining on the fields (kg)
    double residueCremaining = 0;
    //!C lost as CO2 from fertilisers (e.g. urea) (kg)
    double fertiliserCloss = 0;
    //!C in crop products harvested. Includes harvesting by grazing (kg)
    double harvestedC = 0;
    //! Carbon in crop residues (kg)
    double cropResidueC = 0;
    //! C in grazed herbage (kg)
    double grazedHerbageC = 0;
    //! C in pasture-fed supplementary feed (kg)
    double CinPastureFeed = 0;
    //!C in excreta deposited during grazing (kg)
    double excretalCtoPasture = 0;

    //!continuity check for C (kg)
    double Cbalance =0;

    //! N input via N fixation in crops (kg)
    double NFix = 0;
    //!N lost from stored plant products (kg)
    double processStorageNloss = 0;
    //! N input via atmospheric deposition (kg)
    double Natm = 0;
    //!N input in N fertilisers (kg)
    double NFert = 0;
    //!N input in bedding (kg)
    double Nbedding = 0;
    //! carbon imported in livestock manure (kg)
    double Nmanimp = 0;
    //! N imported in animal feed (kg)
    double NPlantProductImported = 0;
    //! N sold in crop products (kg)
    double Nsold = 0;
    //!N exported in milk (kg)
    double Nmilk = 0;
    //! N exported in animal growth (kg)
    double NGrowth = 0;
    //! N exported in animal mortalities (kg)
    double Nmortalities = 0;
    //!N exported in animal manure (kg)
    double Nmanexp = 0;
    //total N export (kg)
    double NExport = 0;
    // N losses and change in N stored in soil (kg)
    double NDeltaSoil = 0;
    //!total N lost (kg)
    double NLost = 0;
    //!N lost as NH3 from housing (kg)
    double housingNH3Loss = 0;
    //!N2O-N emission from stored manure (kg)
    double manureN2Emission = 0;
    //!N2-N emission from stored manure (kg)
    double manureN2OEmission = 0;
    //!NH3-N emission from stored manure (kg)
    double manureNH3Emission = 0;
    //! Total loss of N from manure storage (kg)
    double manurestoreNLoss = 0;
    //! Total loss of N from fields (kg)
    double fieldNLoss = 0;
    //N2-N emission from soil (kg)
    double fieldN2Emission = 0;
    //N2O-N emission from soil
    double fieldN2OEmission = 0;
    //NH3-N-N emission from fertiliser (kg)
    double fertNH3NEmission = 0;
    //NH3-N emission from field-applied manure (kg)
    double fieldmanureNH3Emission = 0;
    //NH3-N emission from urine deposited in the field (kg)
    double fieldUrineNH3Emission = 0;
    //NO3-N leaching from soil (kg)
    double Nleaching = 0;
    //N excreted in housing (kg)
    double NexcretedHousing = 0;
    //N excreted during grazing (kg)
    double NexcretedField = 0;
    //N fed in housing
    double NfedInHousing = 0;
    //N fed in at pasture
    double NfedAtPasture = 0;
    //N from grazed feed (kg)
    double NinGrazedFeed = 0;
    //DM from grazed (kg)
    double DMinGrazedFeed = 0;
    //!Change in mineral N in soil (kg)
    double changeInMinN = 0;
    //!nitrous oxide emission from fertiliser
    double fertiliserN2OEmission = 0;
    //!leaching of organic nitrogen (kg)
    double organicNLeached = 0;
    //!N2O-N in gases from burnt crop residues (kg)
    double burntResidueN2ON = 0;
    //!NH3N in gases from burnt crop residues (kg)
    double burntResidueNH3N = 0;
    //!NOX in gases from burnt crop residues (kg)
    double burntResidueNOxN = 0;
    //!N in other gases from burnt crop residues (kg)
    double burntResidueOtherN = 0;
    //!N in runoff from manure storage (kg)
    double runoffN = 0;
    //!residual soil mineral N at end of crop sequence (kg)
    double residualSoilMineralN = 0;
    //!total losses from pocess/storage of crop products, housing and manure storage (kg)
    double totalHouseStoreNloss = 0;
    //! total loss of N from fields (kg)
    double totalFieldNlosses = 0;
    //!change in total N storage (organic and inorganic) (kg)
    double changeAllSoilNstored = 0;
    //!N in crop residues remaining on the fields (kg)
    double residueNremaining = 0;
    //!farm N surplus (kg)
    double totalFarmNSurplus = 0;
    //!continuity check  (kg)
    double Nbalance = 0;
    //! N in crop residues input to the soil (kg)
    double NinCropResidues =0;
    //! N taken up by crop  (kg)
    double CropNuptake = 0;

    ///farm milk production (kg/yr)
    double farmMilkProduction = 0;
    ///farm meat production (kg/yr)
    double farmMeatProduction = 0;
    ///average milk production per head (kg/animal/yr)
    double avgProductionMilkPerHead = 0;
    //total DM used by livestock (Mg/animal/yr)
    double farmLivestockDM = 0;
    ///concentrate DM used (Mg/yr)
    double farmConcentrateDM = 0;
    ///concentrate energy used (MJ/yr)
    double farmConcentrateEnergy = 0;
    ///grazed DM used (Mg/yr)
    double farmGrazedDM = 0;
    ///farm utilised grazable DM (Mg/yr)
    double farmUnutilisedGrazableDM = 0;
    //!farm area (ha)
    double agriculturalArea = 0;
    //!DM production on farm (tonnes/yr)
    double totalDMproduction = 0;
    //!Utilised DM production on farm (tonnes/yr)
    double utilisedDMproduction = 0;
    //! Mean harvested DM (Mg/yr)
    double FarmHarvestDM=0;
    //!number of dairy ruminant livestock
    double numDairy = 0;
    //!number of non-dairy ruminant livestock
    double numOtherRuminants = 0;
    //!number of non-ruminant livestock
    double numNonRuminants = 0;

    string parens; /*!< a string containing information about the farm and scenario number.*/
    //! Annual precipitation (mm)
    double precip = 0;
    //! Annumal evaporation (mm)
    double evap = 0;
    //! Annual transpiration (mm)
    double transpire = 0;
    //! Annual irrigation
    double irrig = 0;
    //! Annual draindage (mm)
    double drainage = 0;
    //! Maximum plant-available water capacity (mm)
    double MaxPlantAvailWater = 0;
    //! Temporary variable used for diverse purposes
    double temp = 0;
    //! A constructor with one argument.
    /*!
      \param aparens, a string argument that points a string containing information about the farm and scenario number.
    */
    public farmBalanceClass(string aparens)
    {
        parens = aparens;
    }
    //!  Get AgriculturalArea returns the total area of all crop sequences on the farm
    /*!
     \param therotationList, list of CropSequenceClass.
      \return a double value for total agricultural area (ha).
    */
    public double GetAgriculturalArea(List<CropSequenceClass> therotationList)
    {
        double area = 0;
        for (int i = 0; i < therotationList.Count; i++)
        {
            area += therotationList[i].getArea();
        }
        return area;
    }
    //!  Do FarmBalance calculates all the nutrient balances for the farm
    /*!
     \param rotationList, one list value that points to CropSequenceClass.
     \param listOfLivestock, one list value that points to livestock.
     \param listOfHousing, one list value that points to housing.
     \param listOfManurestores, one list value that points to manureStore.
    */
    public void DoFarmBalances(List<CropSequenceClass> rotationList, List<livestock> listOfLivestock, List<housing> listOfHousing,
        List<manureStore> listOfManurestores)
    {
        //!  Calculate the import and export of products to/from the farm
        GlobalVars.Instance.CalculateTradeBalance();
        //! Do the farm carbon balance
        //! Start with the crop sequences
        int minRotation = 1;
        int maxRotation = rotationList.Count;
        for (int rotationID = minRotation; rotationID <= maxRotation; rotationID++)
        {
            carbonFromPlants += rotationList[rotationID - 1].getCFixed() / rotationList[rotationID - 1].GetlengthOfSequence(); //1.100
            NinCropResidues+= rotationList[rotationID - 1].GetresidueNinput() / rotationList[rotationID - 1].GetlengthOfSequence();
            CropNuptake+= rotationList[rotationID - 1].GetCropNuptake() / rotationList[rotationID - 1].GetlengthOfSequence();
            CinImportedFertiliser+= rotationList[rotationID - 1].GetFertiliserC() / rotationList[rotationID - 1].GetlengthOfSequence();
        }
        if (GlobalVars.Instance.theManureExchange != null)
        {
            for (int i = 0; i < GlobalVars.Instance.theManureExchange.GetmanuresImported().Count; i++)
            {
                Cmanimp += GlobalVars.Instance.theManureExchange.GetmanuresImported()[i].GetTotalC();
            }
        }
        else Cmanimp = 0.0;

        GlobalVars.product compositeProductImported = GlobalVars.Instance.GetPlantProductImports();
        CPlantProductImported = compositeProductImported.composition.Getamount() * compositeProductImported.composition.GetC_conc();
        CinImportedBedding = GlobalVars.Instance.GetthebeddingMaterial().Getamount() *
            GlobalVars.Instance.GetthebeddingMaterial().GetC_conc();
        CPlantProductImported += CinImportedBedding;

        //! Calculate the total C input to the farm (including C fixed by crops)
        CInput = carbonFromPlants + CPlantProductImported + Cmanimp + CinImportedFertiliser;

        ///C outputs
        /////! Start with crop product outputs 
        GlobalVars.product compositeProductExported = GlobalVars.Instance.GetPlantProductExports();
        GlobalVars.Instance.PrintPlantProducts();
        CinCropProductsSold = compositeProductExported.composition.Getamount() * compositeProductExported.composition.GetC_conc();

        //! Calculate C exported in animal products
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            Cmilk += listOfLivestock[i].GetMilkC() * listOfLivestock[i].GetAvgNumberOfAnimal(); //1.113
            Cmeat += listOfLivestock[i].GetGrowthC() * listOfLivestock[i].GetAvgNumberOfAnimal();//1.114
            Cmortalities += listOfLivestock[i].GetMortalitiesC() * listOfLivestock[i].GetAvgNumberOfAnimal();
        }
        //! Calculate C exported in manure
        for (int i = 0; i < GlobalVars.Instance.theManureExchange.GetmanuresExported().Count; i++)
        {
            Cmanexp += GlobalVars.Instance.theManureExchange.GetmanuresExported()[i].GetTotalC();
        }

        //! Calculate the total C export
        Cexport = CinCropProductsSold + Cmilk + Cmeat + Cmanexp + Cmortalities;//1.116
        //variables used for debugging
        double LivestockCconsumption = 0;
        double LivestockUrineC = 0;
        double LivestockFaecalC = 0;
        double LivestockGrowthC = 0;
        double LivestockMilkC = 0;
        double LivestockMortalityC = 0;
        //end of variables for debugging

        //! Calculate livestock C flows
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimalCategory = listOfLivestock[i];
            anAnimalCategory.CheckLivestockCBalance();
            livestockCH4C += anAnimalCategory.getCH4C() * listOfLivestock[i].GetAvgNumberOfAnimal();
            livestockCO2C += anAnimalCategory.getCO2C() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockCconsumption += anAnimalCategory.GetCintake() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockUrineC += anAnimalCategory.GeturineC() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockFaecalC += anAnimalCategory.GetfaecalC() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockGrowthC += anAnimalCategory.GetGrowthC() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockMilkC += anAnimalCategory.GetMilkC() * listOfLivestock[i].GetAvgNumberOfAnimal();
            LivestockMortalityC += anAnimalCategory.GetMortalitiesC() * listOfLivestock[i].GetAvgNumberOfAnimal();
            excretalCtoPasture+=anAnimalCategory.GetCexcretionToPasture()* listOfLivestock[i].GetAvgNumberOfAnimal();
       }
        livstockCLoss = livestockCH4C + livestockCO2C;
        //! More will be added to CLost later
        CLost += livstockCLoss;
        //! Calculate C losses from livestock housing
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            housing ahouse = listOfHousing[i];
            ahouse.CheckHousingCBalance();
            housingCLoss += ahouse.GetCO2C();
            CinManureSentToStorage += ahouse.GetCtoStorage();
            CinFeedFedInHouse += ahouse.GetCinFeedFedInHouse();
            NinFeedFedInHouse += ahouse.GetNinFeedFedInHouse();
        }
        CLost += housingCLoss;

        //! Calculate C flows for manure storage (including anaerobic digestion)
        biogasCH4C=0;
        biogasCO2C=0;
        manurestoreRunoffC = 0;
        double manurestoreCLoss = 0;
        
        for (int i = 0; i < listOfManurestores.Count; i++)
        {
            manureStore amanurestore2 = listOfManurestores[i];
            amanurestore2.CheckManureStoreCBalance();
            manurestoreCO2C += amanurestore2.GetCCO2ST();
            manurestoreCH4C += amanurestore2.GetCCH4ST();
            manurestoreRunoffC += amanurestore2.GetrunoffC();
            biogasCH4C += amanurestore2.GetbiogasCH4C();
            biogasCO2C += amanurestore2.GetbiogasCO2C();
            biogasSupplC += amanurestore2.GetsupplementaryC();
        }
        manurestoreCLoss = manurestoreCH4C + manurestoreCO2C + manurestoreRunoffC;
        Cexport += biogasCO2C + biogasCH4C;
        CLost += manurestoreCLoss;
        //! Calculate soil C flows
        soilCO2_CEmission = 0;
        processStorageCloss = 0;
        burntResidueCOC = 0;
        burntResidueCO2C = 0;
        burntResidueBlackC = 0;
        soilCH4_CEmission = 0;
    //variables used for debugging
        double CinputToSoil = 0;
        //end of debug variables
        for (int rotationID = minRotation; rotationID <= maxRotation; rotationID++)
        {
            rotationList[rotationID - 1].CheckRotationCBalance();
            CDeltaSoil += rotationList[rotationID - 1].GetCdeltaSoil() / rotationList[rotationID - 1].GetlengthOfSequence();
            soilCO2_CEmission += rotationList[rotationID - 1].GetsoilCO2_CEmission() / rotationList[rotationID - 1].GetlengthOfSequence();
            soilCleached += rotationList[rotationID - 1].GetCleached() / rotationList[rotationID - 1].GetlengthOfSequence();
            processStorageCloss += rotationList[rotationID - 1].getProcessStorageLossCarbon() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueCOC += rotationList[rotationID - 1].getBurntResidueCOC() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueBlackC += rotationList[rotationID - 1].getBurntResidueBlackC() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueCO2C += rotationList[rotationID - 1].getBurntResidueCO2C() / rotationList[rotationID - 1].GetlengthOfSequence();
            CinputToSoil+=rotationList[rotationID - 1].GetCinputToSoil()/ rotationList[rotationID - 1].GetlengthOfSequence();
            grazedHerbageC += rotationList[rotationID - 1].getGrazedC() / rotationList[rotationID - 1].GetlengthOfSequence();
            cropResidueC += rotationList[rotationID - 1].getCropResidueCarbon() / rotationList[rotationID - 1].GetlengthOfSequence();
            harvestedC+= rotationList[rotationID - 1].getCHarvested() / rotationList[rotationID - 1].GetlengthOfSequence();
            soilCH4_CEmission += rotationList[rotationID - 1].getGrazingMethaneC() / rotationList[rotationID - 1].GetlengthOfSequence();
            fertiliserCloss+= rotationList[rotationID - 1].GetFertiliserC() / rotationList[rotationID - 1].GetlengthOfSequence();
        }
        double burntResidueC = burntResidueBlackC + burntResidueCO2C + burntResidueCOC;

        CLost += processStorageCloss + soilCO2_CEmission + soilCleached + burntResidueC + soilCH4_CEmission + fertiliserCloss;
        //Calculate the C balance for the farm and report an error if the C budget cannot be closed (a margin of error is permitted, especially because of grazing)
        Cbalance = CInput - (Cexport + CLost + CDeltaSoil);//1.117
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        double diff = 0;
        if (CInput > 0)  //check absolute error, if no C input
            diff = Cbalance / CInput;
        else
            diff = Cbalance;
        if (Math.Abs(diff) > tolerance)
        {
            double errorPercent = 100 * diff;
            string tmp="Error; C balance at farm scale deviates by more than the permitted margin.\n";
            tmp += ("Percentage error = " + errorPercent.ToString("0.00") + "%"); ;
            GlobalVars.Instance.Error(tmp);
            if (GlobalVars.Instance.getPauseBeforeExit() && rotationList.Count != 0)  //Delay exit until the console can be read
                Console.Read();
            if ((rotationList.Count != 0)&&(GlobalVars.Instance.getstopOnError())) //!
                throw new System.ArgumentException("farmFail", "farmFail");
            else
            {
                Console.Write("there is no soil");
                Console.Read();
            }
        }

        ///Other C flows - these are to allow internal flows to be monitored in output files

        double CliveCO2 = 0;
        double CliveCH4 = 0;
        
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            CliveCO2 += listOfLivestock[i].getCO2C(); //1.120
            CliveCH4 += listOfLivestock[i].getCH4C();//1.121
            CinPastureFeed += listOfLivestock[i].GetpastureFedC();
        }

        for (int i = 0; i < listOfHousing.Count; i++)
        {
            CInhouseExcreta += listOfHousing[i].getPropTimeThisHouse() * (listOfHousing[i].getFaecesC() + listOfHousing[i].getUrineC()); //1.122
        }

        double CInhouseBedding = 0;
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            CInhouseBedding += listOfHousing[i].getBeddingC();//1.123
        }
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            CinFeedWaste += listOfHousing[i].getFeedWasteC();//1.124
        }

        ///Now do the farm N balance 
        //! N fixation
        for (int i = 0; i < rotationList.Count; i++)
        {
            NFix += rotationList[i].getNFix() / rotationList[i].GetlengthOfSequence();//1.132
        }
        /// N deposition from atmosphere
        for (int i = 0; i < rotationList.Count; i++)
        {
            Natm += rotationList[i].getNAtm() / rotationList[i].GetlengthOfSequence();//1.133
        }
        ///Fertiliser N
        for (int i = 0; i < rotationList.Count; i++)
        {
            NFert += rotationList[i].getFertiliserNapplied() / rotationList[i].GetlengthOfSequence();
        }

        ///bedding N
        ///N fed in housing
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            Nbedding += listOfHousing[i].getBeddingN(); //1.102
            NfedInHousing += listOfHousing[i].GetNfedInHousing();
        }
        ///Imported manure N
        for (int i = 0; i < GlobalVars.Instance.theManureExchange.GetmanuresImported().Count; i++)
        {
            Nmanimp += GlobalVars.Instance.theManureExchange.GetmanuresImported()[i].GetTotalN();// / GlobalVars.Instance.theZoneData.GetaverageYearsToSimulate();
        }
        manureImportN = Nmanimp;
        //! N in imported plant products
        NPlantProductImported = compositeProductImported.composition.Getamount() * compositeProductImported.composition.GetN_conc();
        double Ninput = Natm + NFert + NFix + Nmanimp + NPlantProductImported;
        
        liveFeedImportN = NPlantProductImported - Nbedding;

        ///N export
        ///N exported in crop products
        temp = compositeProductExported.composition.Getamount();/// GlobalVars.Instance.theZoneData.GetaverageYearsToSimulate();
        Nsold = compositeProductExported.composition.Getamount() * compositeProductExported.composition.GetN_conc();
        //! Livestock N flows
        livestockNintake = 0;
        double livestockNexcreted = 0;
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            Nmilk += listOfLivestock[i].GetMilkN() * listOfLivestock[i].GetAvgNumberOfAnimal(); //1.113
            NGrowth += listOfLivestock[i].GetGrowthN() * listOfLivestock[i].GetAvgNumberOfAnimal();//1.114
            Nmortalities += listOfLivestock[i].GetMortalitiesN() * listOfLivestock[i].GetAvgNumberOfAnimal();//1.114
            listOfLivestock[i].CheckLivestockNBalances();
            livestockNintake += listOfLivestock[i].GetNintake() * listOfLivestock[i].GetAvgNumberOfAnimal();
            NfedAtPasture+= listOfLivestock[i].GetpastureFedN() * listOfLivestock[i].GetAvgNumberOfAnimal();
            NinGrazedFeed += listOfLivestock[i].GetgrazedN() * listOfLivestock[i].GetAvgNumberOfAnimal();
            DMinGrazedFeed += listOfLivestock[i].GetgrazedDM() * listOfLivestock[i].GetAvgNumberOfAnimal();
            livestockNexcreted += listOfLivestock[i].GetExcretedN() * listOfLivestock[i].GetAvgNumberOfAnimal();
            liveToFieldN += listOfLivestock[i].GetNexcretionToPasture() * listOfLivestock[i].GetAvgNumberOfAnimal();
        }
        liveGrazedN = (NinGrazedFeed + NfedAtPasture);
        liveToHousingN = livestockNexcreted - liveToFieldN;

        ///N from grazed feed
        grazedN = NinGrazedFeed;
        NfedInHousing = livestockNintake - (NinGrazedFeed + NfedAtPasture);
        //! N exported in manure
        for (int i = 0; i < GlobalVars.Instance.theManureExchange.GetmanuresExported().Count; i++)
        {
            Nmanexp += GlobalVars.Instance.theManureExchange.GetmanuresExported()[i].GetTotalN();// / GlobalVars.Instance.theZoneData.GetaverageYearsToSimulate();
        }
        manureExportN = Nmanexp;
        //Calculate total N exported
        NExport = Nmanexp + Nsold + Nmilk + NGrowth + Nmortalities;
        //! Calculate housing N flows
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            housing ahouse = listOfHousing[i];
            ahouse.CheckHousingNBalance();
            housingNH3Loss += ahouse.GetNNH3housing();
            NexcretedHousing += ahouse.GetNinputInExcreta();
            storageFromFeedWasteN += ahouse.getFeedWasteN();
            storageFromBeddingN += ahouse.getBeddingN();
        }

        houseInFromAnimalsN = NexcretedHousing;
        houseLossN = housingNH3Loss;
        storageFromHouseN = NexcretedHousing + storageFromBeddingN + storageFromFeedWasteN - houseLossN;
        houseExcretaToStorageN = houseInFromAnimalsN - houseLossN;

        ///N excreted during grazing
        NexcretedField = livestockNexcreted - NexcretedHousing;
        manureNexStorage = 0;
        NLost += housingNH3Loss;
        //! Calculate manure storage N flows
        for (int i = 0; i < listOfManurestores.Count; i++)
        {
            manureStore amanurestore2 = listOfManurestores[i];
            amanurestore2.CheckManureStoreNBalance();
            manureN2Emission += amanurestore2.GettotalNstoreN2();
            manureN2OEmission += amanurestore2.GettotalNstoreN20();
            manureNH3Emission += amanurestore2.GettotalNstoreNH3();
            runoffN += amanurestore2.GetrunoffN();
            manureNexStorage += amanurestore2.GetManureN();
            biogasSupplN += amanurestore2.GetsupplementaryN();
        }
        manurestoreNLoss = manureN2Emission + manureN2OEmission + manureNH3Emission;
        storageGaseousLossN = manurestoreNLoss;
        storageRunoffN = runoffN;

        NLost += manurestoreNLoss + runoffN;

        manureToFieldN = manureNexStorage + manureImportN - manureExportN;
        //! Soil N flows (and some additional variables)
        double startsoilMineralN = 0;
        burntResidueN2ON = 0;
        burntResidueNH3N = 0;
        burntResidueNOxN = 0;
        fieldUrineNH3Emission = 0;
        Nharvested = 0;
        totalDMproduction = 0;
        utilisedDMproduction = 0;
        FarmHarvestDM = 0;
        for (int rotationID = minRotation; rotationID <= maxRotation; rotationID++)
        {
            rotationList[rotationID - 1].CheckRotationNBalance(true);
            NDeltaSoil += (rotationList[rotationID - 1].GetNStored() - rotationList[rotationID - 1].GetinitialSoilN()) / rotationList[rotationID - 1].GetlengthOfSequence();
            Nharvested += rotationList[rotationID - 1].getNharvested()/ rotationList[rotationID - 1].GetlengthOfSequence();
            fieldN2Emission += rotationList[rotationID - 1].GetN2NEmission() / rotationList[rotationID - 1].GetlengthOfSequence();
            fertiliserN2OEmission += rotationList[rotationID - 1].GetfertiliserN2ONEmissions() / rotationList[rotationID - 1].GetlengthOfSequence();
            fieldN2OEmission += rotationList[rotationID - 1].GetN2ONemission() / rotationList[rotationID - 1].GetlengthOfSequence();
            fertNH3NEmission += rotationList[rotationID - 1].GetFertNH3NEmission() / rotationList[rotationID - 1].GetlengthOfSequence();
            fieldmanureNH3Emission += rotationList[rotationID - 1].GetManureNH3NEmission() / rotationList[rotationID - 1].GetlengthOfSequence();
            fieldUrineNH3Emission += rotationList[rotationID - 1].GeturineNH3emissions() / rotationList[rotationID - 1].GetlengthOfSequence();
            Nleaching += rotationList[rotationID - 1].GettheNitrateLeaching() / rotationList[rotationID - 1].GetlengthOfSequence();
            organicNLeached += rotationList[rotationID - 1].GetOrganicNLeached() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueN2ON += rotationList[rotationID - 1].getBurntResidueN2ON ()/ rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueNH3N += rotationList[rotationID - 1].getBurntResidueNH3N() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueNOxN += rotationList[rotationID - 1].getBurntResidueNOxN() / rotationList[rotationID - 1].GetlengthOfSequence();
            burntResidueOtherN += rotationList[rotationID - 1].getBurntResidueOtherN() / rotationList[rotationID - 1].GetlengthOfSequence();
            residualSoilMineralN = rotationList[rotationID - 1].GetResidualSoilMineralN();
            startsoilMineralN = rotationList[rotationID - 1].GetstartsoilMineralN();
            residueNremaining = rotationList[rotationID - 1].GetresidueNremaining();
            changeInMinN += (residualSoilMineralN - startsoilMineralN) / rotationList[rotationID - 1].GetlengthOfSequence();
            //totalDMproduction is modelled yield (whether or not it is harvested)
            totalDMproduction += rotationList[rotationID - 1].GetDMYield() / (rotationList[rotationID - 1].GetlengthOfSequence() * 1000);
            //utilisedDMproduction is the modelled yield for harvested products from non-grazed crops and the grazed yield for grazed crops
            utilisedDMproduction += rotationList[rotationID - 1].GetUtilisedDMYield() / (rotationList[rotationID - 1].GetlengthOfSequence() * 1000);
            //FarmHarvestDM  is the harvested yield for non-grazed crops and the grazed yield for grazed crops
            FarmHarvestDM += rotationList[rotationID - 1].getDMHarvested() / (rotationList[rotationID - 1].GetlengthOfSequence());
            processStorageNloss += rotationList[rotationID - 1].getProcessStorageLossNitrogen() / rotationList[rotationID - 1].GetlengthOfSequence();
        }
        double burntResidueN = burntResidueN2ON + burntResidueNH3N + burntResidueNOxN + burntResidueOtherN;
        fieldGaseousLossN = fertNH3NEmission + fieldmanureNH3Emission + +fieldUrineNH3Emission + fieldN2Emission + fieldN2OEmission
            + burntResidueN;

        fieldNLoss = fertNH3NEmission + fieldmanureNH3Emission + fieldUrineNH3Emission + fieldN2Emission + fieldN2OEmission 
            + Nleaching + burntResidueN + organicNLeached;
        fieldNitrateLeachedN = organicNLeached + Nleaching;
        changeSoilN = NDeltaSoil;
        totalFieldNlosses = fieldNLoss + processStorageNloss;

        NLost += totalFieldNlosses;
        totalFarmNSurplus = Ninput - NExport; //1.137
        totalHouseStoreNloss = housingNH3Loss + manurestoreNLoss + processStorageNloss;
        changeAllSoilNstored = NDeltaSoil + changeInMinN;
        //! Calculate farm N balance and report an error if the N budget cannot be closed
        Nbalance = Ninput - (NExport + NLost + NDeltaSoil + changeInMinN + residueNremaining);//1.117
        if (Ninput > 0)
            diff = Nbalance / Ninput;
        else
            diff = Nbalance;
        if (Math.Abs(diff) > 2 *tolerance)
        {
            double errorPercent = 100 * diff;

            System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalVars.Instance.GeterrorFileName());
            string outstring1 = "Error; N balance at farm scale deviates by more than the permitted margin";
            string outstring2 = "Percentage error = " + errorPercent.ToString("0.00") + "%";
            string outstring3 = "Absolute error = " + Nbalance.ToString();
            file.WriteLine(outstring1);
            file.WriteLine(outstring2);
            file.Write(outstring3);
            file.Close();
            GlobalVars.Instance.log(outstring1, 5);
            GlobalVars.Instance.log(outstring2, 5);
            Console.Write(outstring3);
            if (GlobalVars.Instance.getPauseBeforeExit() && rotationList.Count != 0)
                Console.Read();
            if ((rotationList.Count != 0)&&(GlobalVars.Instance.getstopOnError()))
            {
                WriteFarmBalances(rotationList, listOfLivestock);
                GlobalVars.Instance.CloseOutputXML();
                GlobalVars.Instance.CloseOutputTabFile();
                GlobalVars.Instance.CloseCtoolFile();
                GlobalVars.Instance.Error("Error in farm N balance", "",false);
                throw new System.ArgumentException("farm Failed", "farm Failed");
            }
            else
            {
                Console.Write("there is no soil");
                if (GlobalVars.Instance.getPauseBeforeExit() && rotationList.Count != 0)
                    Console.Read();
            }

        }
        agriculturalArea= GetAgriculturalArea(rotationList);


        ///do GHG budget
        entericCH4CO2Eq = livestockCH4C * GlobalVars.Instance.GetCO2EqCH4();
        manureCH4CO2Eq = manurestoreCH4C * GlobalVars.Instance.GetCO2EqCH4();
        manureN2OCO2Eq = manureN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        fieldN2OCO2Eq = fieldN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        fieldCH4CO2Eq = soilCH4_CEmission * GlobalVars.Instance.GetCO2EqCH4();
        fieldCO2 = fertiliserCloss * GlobalVars.Instance.GetC_CO2();
        soilCO2Eq = -1 * CDeltaSoil * GlobalVars.Instance.GetCO2EqsoilC();
        directGHGEmissionCO2Eq = entericCH4CO2Eq + manureCH4CO2Eq + manureN2OCO2Eq + fieldN2OCO2Eq + soilCO2Eq + fieldCH4CO2Eq + fieldCO2;

        housingNH3CO2Eq = housingNH3Loss * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        manurestoreNH3CO2Eq = manureNH3Emission * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        fieldmanureNH3CO2Eq = fieldmanureNH3Emission * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        fieldfertNH3CO2Eq = fertNH3NEmission * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        leachedNCO2Eq = Nleaching * GlobalVars.Instance.GetIndirectNO3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        indirectGHGCO2Eq = housingNH3CO2Eq + manurestoreNH3CO2Eq + fieldmanureNH3CO2Eq + fieldfertNH3CO2Eq + leachedNCO2Eq;

        for (int rotationID = 0; rotationID <= (maxRotation-minRotation); rotationID++)
            farmUnutilisedGrazableDM += rotationList[rotationID].GetUnutilisedGrazableDM() / (rotationList[rotationID].GetlengthOfSequence() * 1000);

        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimalCategory = listOfLivestock[i];
            farmMilkProduction += anAnimalCategory.GetavgProductionMilk() * listOfLivestock[i].GetAvgNumberOfAnimal() * GlobalVars.avgNumberOfDays;
            farmMeatProduction += anAnimalCategory.GetavgProductionMeat() * listOfLivestock[i].GetAvgNumberOfAnimal() * GlobalVars.avgNumberOfDays / 1000.0;
            farmLivestockDM += anAnimalCategory.GetDMintake() * listOfLivestock[i].GetAvgNumberOfAnimal() / 1000.0;
            farmConcentrateEnergy += anAnimalCategory.GetConcentrateEnergy() * listOfLivestock[i].GetAvgNumberOfAnimal() * GlobalVars.avgNumberOfDays / 1000.0;
            farmConcentrateDM += anAnimalCategory.GetConcentrateDM() * listOfLivestock[i].GetAvgNumberOfAnimal() * GlobalVars.avgNumberOfDays / 1000.0;
            farmGrazedDM += anAnimalCategory.GetgrazedDM() * listOfLivestock[i].GetAvgNumberOfAnimal() / 1000.0;
            if (anAnimalCategory.GetisRuminant())
            {
                if (anAnimalCategory.GetisDairy())
                    numDairy += listOfLivestock[i].GetAvgNumberOfAnimal();
                else
                    numOtherRuminants += listOfLivestock[i].GetAvgNumberOfAnimal();
            }
            else numNonRuminants += listOfLivestock[i].GetAvgNumberOfAnimal();
            if (numDairy > 0)
                avgProductionMilkPerHead = farmMilkProduction / numDairy;
            else
                avgProductionMilkPerHead = 0;
        }
        //! Do water budget
        for (int rotationID = minRotation; rotationID <= maxRotation; rotationID++)
        {
            precip += rotationList[rotationID - 1].GetCumulativePrecip() / rotationList[rotationID - 1].GetlengthOfSequence();
            evap += rotationList[rotationID - 1].GetCumulativeEvaporation() / rotationList[rotationID - 1].GetlengthOfSequence();
            irrig += rotationList[rotationID - 1].GetCumulativeIrrigation() / rotationList[rotationID - 1].GetlengthOfSequence();
            transpire += rotationList[rotationID - 1].GetCumulativeTranspiration() / rotationList[rotationID - 1].GetlengthOfSequence();
            drainage += rotationList[rotationID - 1].GetCumulativeDrainage() / rotationList[rotationID - 1].GetlengthOfSequence();
            MaxPlantAvailWater += rotationList[rotationID - 1].GetMaxPlantAvailableWater() * rotationList[rotationID - 1].getArea();
        }
        MaxPlantAvailWater /= agriculturalArea;
        double cumPotEvapoTrans=0;
        for (int i=0; i<12; i++)
            cumPotEvapoTrans+=GlobalVars.Instance.theZoneData.PotentialEvapoTrans[i];
        cumPotEvapoTrans *= 30.43;
        precip /= agriculturalArea;
        evap /= agriculturalArea;
        irrig /= agriculturalArea;
        transpire /= agriculturalArea;
        drainage /= agriculturalArea;
        double ConcentrateDMexported = GlobalVars.Instance.GetConcentrateExports();
    }
    //!  Write farm budgets to file
    /*!
     \param CropSequence, list of CropSequenceClass.
     \param listOfLivestock, list of livestock.
    */
    public void WriteFarmBalances(List<CropSequenceClass> CropSequence, List<livestock> listOfLivestock)
     {
            double Nsurp = 0;
            if (agriculturalArea > 0)
                Nsurp = totalFarmNSurplus / agriculturalArea;//1,138

        ///Write outputs at the farm scale
        //! First writes to the xml and xls (csv) files
        GlobalVars.Instance.writeStartTab("FarmBalance");

        GlobalVars.Instance.writeStartTab("Farm");
        GlobalVars.Instance.writeInformationToFiles("liveFeedImportN", "Imported livestock feed", "kgN/yr", liveFeedImportN, parens);
        feedItem afeedItem=GlobalVars.Instance.GetBeddingImported();
        double NinImportedBedding = afeedItem.Getamount() * afeedItem.GetN_conc();
        GlobalVars.Instance.writeInformationToFiles("importedBeddingN", "Imported bedding", "kgN/yr", NinImportedBedding, parens);
        GlobalVars.Instance.writeInformationToFiles("NFix", "N fixation", "kg N/yr", NFix, parens);
        GlobalVars.Instance.writeInformationToFiles("Natm", "N deposited from atmosphere", "kg N/yr", Natm, parens);
        GlobalVars.Instance.writeInformationToFiles("NFert", "N in fertiliser", "kg N/yr", NFert, parens);
        GlobalVars.Instance.writeInformationToFiles("manureImportN", "Imported manure", "kgN/yr", manureImportN, parens);
        GlobalVars.Instance.writeInformationToFiles("Nsold", "N sold in crop products", "kg N/yr", Nsold, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmilk", "N sold in milk", "kg N/yr", Nmilk, parens);
        GlobalVars.Instance.writeInformationToFiles("NGrowth", "N exported in meat", "kg N/yr", NGrowth, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmortalities", "N in mortalities", "kg N/yr", Nmortalities, parens);
        GlobalVars.Instance.writeInformationToFiles("manureExportN", "Exported manure", "kgN/yr", manureExportN, parens);
        GlobalVars.Instance.writeInformationToFiles("houseLossN", "Gaseous loss housing", "kgN/yr", houseLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("processStorageNloss", "N lost from processing/stored crop products", "kg N/yr", processStorageNloss, parens);
        GlobalVars.Instance.writeInformationToFiles("storageGaseousLossN", "Gaseous loss storage", "kgN/yr", storageGaseousLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("storageRunoffN", "Runoff", "kgN/yr", storageRunoffN, parens);
        GlobalVars.Instance.writeInformationToFiles("totalFieldNlosses", "Gaseous loss field", "kgN/yr", fieldGaseousLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldNitrateLeachedN", "Nitrate leaching", "kgN/yr", fieldNitrateLeachedN, parens);
        GlobalVars.Instance.writeInformationToFiles("changeInMinN", "Change in mineral N in soil", "kgN/yr", changeInMinN, parens);
        GlobalVars.Instance.writeInformationToFiles("changeSoilN", "Change in organic N in soil", "kgN/yr", changeSoilN, parens);
        GlobalVars.Instance.writeEndTab();

        GlobalVars.Instance.writeStartTab("Herd");
        double NinFeedConsumedInHousing = livestockNintake - (NinGrazedFeed + NfedAtPasture); 
        GlobalVars.Instance.writeInformationToFiles("NinFeedConsumedInHousing", "Livestock feed consumed in housing", "kgN/yr", NinFeedConsumedInHousing, parens);
        GlobalVars.Instance.writeInformationToFiles("liveGrazedN", "Grazed", "kgN/yr", liveGrazedN, parens);
        GlobalVars.Instance.writeInformationToFiles("liveToHousingN", "Deposited in housing", "kgN/yr", liveToHousingN, parens);
        GlobalVars.Instance.writeInformationToFiles("liveToFieldN", "Deposited in field", "kgN/yr", liveToFieldN, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmilk", "N sold in milk", "kg N/yr", Nmilk, parens);
        GlobalVars.Instance.writeInformationToFiles("NGrowth", "N exported in meat", "kg N/yr", NGrowth, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmortalities", "N in mortalities", "kg N/yr", Nmortalities, parens);
        if (livestockNintake > 0)
        {
            double NeffLivestock = (Nmilk + NGrowth) / livestockNintake;
            GlobalVars.Instance.writeInformationToFiles("Nefficiency", "Efficiency of N use by livestock", "-", NeffLivestock, parens);
        }
        else
            GlobalVars.Instance.writeInformationToFiles("Nefficiency", "Efficiency of N use by livestock", "-", 0, parens);

        GlobalVars.Instance.writeEndTab();

        GlobalVars.Instance.writeStartTab("housing");
        GlobalVars.Instance.writeInformationToFiles("houseInFromAnimalsN", "Input from livestock", "kgN/yr", houseInFromAnimalsN, parens);
        GlobalVars.Instance.writeInformationToFiles("houseLossN", "Gaseous loss", "kgN/yr", houseLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("houseExcretaToStorageN", "Sent to storage", "kgN/yr", houseExcretaToStorageN, parens);
        GlobalVars.Instance.writeEndTab();

        GlobalVars.Instance.writeStartTab("ManureStorage");
        GlobalVars.Instance.writeInformationToFiles("houseExcretaToStorageN", "Input from housing manure", "kgN/yr", houseExcretaToStorageN, parens);
        GlobalVars.Instance.writeInformationToFiles("storageFromBeddingN", "Bedding", "kgN/yr", storageFromBeddingN, parens);
        GlobalVars.Instance.writeInformationToFiles("storageFromFeedWasteN", "Feed wastage", "kgN/yr", storageFromFeedWasteN, parens);
        //GlobalVars.Instance.writeInformationToFiles("biogasSupplementaryN", "Biogas supplementary feedstock", "kgN/yr", biogasSupplN, parens);
        GlobalVars.Instance.writeInformationToFiles("storageGaseousLossN", "Gaseous loss", "kgN/yr", storageGaseousLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("storageRunoffN", "Runoff from storage", "kgN/yr", storageRunoffN, parens);
        GlobalVars.Instance.writeInformationToFiles("manureNexStorage", "Manure ex storage", "kgN/yr", manureNexStorage, parens);
        GlobalVars.Instance.writeEndTab();


        GlobalVars.Instance.writeStartTab("Fields");
        double NharvestedMechanically = Nharvested - grazedN;
        GlobalVars.Instance.writeInformationToFiles("NFix", "N fixation", "kg N/yr", NFix, parens);
        GlobalVars.Instance.writeInformationToFiles("Natm", "N deposited from atmosphere", "kg N/yr", Natm, parens);
        GlobalVars.Instance.writeInformationToFiles("NFert", "N in fertiliser", "kg N/yr", NFert, parens);
        GlobalVars.Instance.writeInformationToFiles("manureToFieldN", "Manure applied", "kgN/yr", manureToFieldN, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldGaseousLossN", "Gaseous loss fields", "kgN/yr", fieldGaseousLossN, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldNitrateLeachedN", "Nitrate leaching", "kgN/yr", fieldNitrateLeachedN, parens);
        GlobalVars.Instance.writeInformationToFiles("NharvestedMechanically", "Harvested mechanically", "kgN/yr", NharvestedMechanically, parens);
        GlobalVars.Instance.writeInformationToFiles("grazedN", "Harvested by grazing", "kgN/yr", grazedN, parens);
        GlobalVars.Instance.writeInformationToFiles("changeSoilN", "Change in mineral N in soil", "kgN/yr", changeInMinN, parens);
        GlobalVars.Instance.writeInformationToFiles("changeSoilN", "Change in organic N in soil", "kgN/yr", changeSoilN, parens);
        GlobalVars.Instance.writeEndTab();

        GlobalVars.Instance.writeStartTab("FarmCBalance");
        GlobalVars.Instance.writeInformationToFiles("carbonFromPlants", "C fixed from atmosphere", "kg C/yr", carbonFromPlants, parens);
        GlobalVars.Instance.writeInformationToFiles("Cmanimp", "C in imported manure", "kg C/yr", Cmanimp, parens);
        GlobalVars.Instance.writeInformationToFiles("CPlantProductImported", "C in imported feed", "kg C/yr", CPlantProductImported, parens);
        GlobalVars.Instance.writeInformationToFiles("C_in_imported", "C in bedding", "kg C/yr", CinImportedBedding, parens);
        GlobalVars.Instance.writeInformationToFiles("ImportedFertiliserC", "C imported in fertiliser", "kg C/yr", CinImportedFertiliser, parens);
        GlobalVars.Instance.writeInformationToFiles("Cmilk", "C in exported milk", "kg C/yr", Cmilk, parens);
        GlobalVars.Instance.writeInformationToFiles("Cmeat", "C in exported meat", "kg C/yr", Cmeat, parens);
        GlobalVars.Instance.writeInformationToFiles("Cmortalities", "C in mortalities", "kg C/yr", Cmortalities, parens);
        GlobalVars.Instance.writeInformationToFiles("CinCropProductsSold", "C in crop products sold", "kg C/yr", CinCropProductsSold, parens);
        GlobalVars.Instance.writeInformationToFiles("Cmanexp", "C in exported manure", "kg C/yr", Cmanexp, parens);
        GlobalVars.Instance.writeInformationToFiles("livestockCH4C", "C in enteric methane emissions", "kg C/yr", livestockCH4C, parens);
        GlobalVars.Instance.writeInformationToFiles("livestockCO2C", "C in CO2 emitted by livestock", "kg C/yr", livestockCO2C, parens);
        GlobalVars.Instance.writeInformationToFiles("housingCLoss", "C in CO2 emitted from animal housing", "kg C/yr", housingCLoss, parens);
        GlobalVars.Instance.writeInformationToFiles("manurestoreCH4C", "C in methane emitted by manure", "kg C/yr", manurestoreCH4C, parens);
        GlobalVars.Instance.writeInformationToFiles("manurestoreCO2C", "C in CO2 emitted by manure", "kg C/yr", manurestoreCO2C, parens);
        GlobalVars.Instance.writeInformationToFiles("biogasCH4C", "C in biogas methane", "kg C/yr", biogasCH4C, parens);
        GlobalVars.Instance.writeInformationToFiles("biogasCO2C", "C in biogas CO2", "kg C/yr", biogasCO2C, parens);
        GlobalVars.Instance.writeInformationToFiles("processStorageCloss", "C in CO2 lost from stored crop products", "kg C/yr", processStorageCloss, parens);
        GlobalVars.Instance.writeInformationToFiles("soilCO2_CEmission", "C in CO2 emitted by the soil", "kg C/yr", soilCO2_CEmission, parens);
        GlobalVars.Instance.writeInformationToFiles("soilCH4_CEmission", "C in CH4 emitted by from excreta deposited on soil", "kg C/yr", soilCH4_CEmission, parens);
        GlobalVars.Instance.writeInformationToFiles("soilCleached", "C in organic matter leached from the soil", "kg C/yr", soilCleached, parens);
        GlobalVars.Instance.writeInformationToFiles("burntResidueCOC", "CO-C from burning crop residues", "kg C/yr", burntResidueCOC, parens);
        GlobalVars.Instance.writeInformationToFiles("burntResidueCO2C", "CO2-C in gases from burning crop residues", "kg C/yr", burntResidueCO2C, parens);
        GlobalVars.Instance.writeInformationToFiles("burntResidueBlackC", "Black carbon in gases from burning crop residues", "kg C/yr", burntResidueBlackC, parens);
        GlobalVars.Instance.writeInformationToFiles("CinFertiliserCO2", "C lost from fertiliser C", "kg C/yr", fertiliserCloss, parens);
        GlobalVars.Instance.writeInformationToFiles("CDeltaSoil", "Change in C stored in the soil", "kg C/yr", CDeltaSoil, parens);
        GlobalVars.Instance.writeInformationToFiles("CLost", "C lost to the environment", "kg C/yr", CLost, parens);
        GlobalVars.Instance.writeInformationToFiles("Cbalance", "Net C balance (should be about zero)", "kg C/yr", Cbalance, parens);
        //!Provided the farm has land, write the C flows on a per hectare basis
        if (agriculturalArea > 0)
        {
            GlobalVars.Instance.writeStartTab("PerUnitArea");
            GlobalVars.Instance.writeInformationToFiles("carbonFromPlants", "C fixed from atmosphere", "kg C/ha/yr", carbonFromPlants / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Cmanimp", "C in imported manure", "kg C/ha/yr", Cmanimp / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("CPlantProductImported", "C in imported feed", "kg C/ha/yr", CPlantProductImported / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("C_in_imported", "C in bedding", "kg C/ha/yr", CinImportedBedding / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("ImportedFertiliserC", "C imported in fertiliser", "kg C/yr", CinImportedFertiliser / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Cmilk", "C in exported milk", "kg C/ha/yr", Cmilk / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Cmeat", "C in exported meat", "kg C/ha/yr", Cmeat / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Cmortalities", "C in mortalities", "kg C/ha/yr", Cmortalities / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("CinCropProductsSold", "C in crop products sold", "kg C/ha/yr", CinCropProductsSold / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("soilCO2_CEmission", "C in CO2 emitted by the soil", "kg C/ha/yr", soilCO2_CEmission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("soilCH4_CEmission", "C in CH4 emitted by from excreta deposited on soil", "kg C/ha/yr", soilCH4_CEmission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("soilCleached", "C in organic matter leached from the soil", "kg C/ha/yr", soilCleached / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burntResidueCOC", "CO-C from burning crop residues", "kg C/ha/yr", burntResidueCOC / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burntResidueCO2C", "CO2-C in gases from burning crop residues", "kg C/ha/yr", burntResidueCO2C / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burntResidueBlackC", "Black carbon in gases from burning crop residues", "kg C/ha/yr", burntResidueBlackC / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("CinFertiliserCO2", "C lost from fertiliser C", "kg C/yr", fertiliserCloss / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("CDeltaSoil", "Change in C stored in the soil", "kg C/ha/yr", CDeltaSoil / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Cbalance", "Net C balance (should be about zero)", "kg C/ha/yr", Cbalance / agriculturalArea, parens);
            GlobalVars.Instance.writeEndTab();
        }
        GlobalVars.Instance.writeEndTab();
        GlobalVars.Instance.writeStartTab("FarmNBalance");
        GlobalVars.Instance.writeInformationToFiles("Nmanimp", "N in imported manure", "kg N/yr", Nmanimp, parens);
        GlobalVars.Instance.writeInformationToFiles("NFix", "N fixation", "kg N/yr", NFix, parens);
        GlobalVars.Instance.writeInformationToFiles("Natm", "N deposited from atmosphere", "kg N/yr", Natm, parens);
        GlobalVars.Instance.writeInformationToFiles("NFert", "N in fertiliser", "kg N/yr", NFert, parens);
        GlobalVars.Instance.writeInformationToFiles("Nbedding", "N in bedding", "kg N/yr", Nbedding, parens);
        GlobalVars.Instance.writeInformationToFiles("NPlantProductImported", "N in imported crop products", "kg N/yr", NPlantProductImported, parens);
        GlobalVars.Instance.writeInformationToFiles("processStorageNloss", "N lost from processing/stored crop products", "kg N/yr", processStorageNloss, parens);
        GlobalVars.Instance.writeInformationToFiles("Nsold", "N sold in crop products", "kg N/yr", Nsold, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmilk", "N sold in milk", "kg N/yr", Nmilk, parens);
        GlobalVars.Instance.writeInformationToFiles("NGrowth", "N exported in meat", "kg N/yr", NGrowth, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmortalities", "N in mortalities", "kg N/yr", Nmortalities, parens);
        GlobalVars.Instance.writeInformationToFiles("Nmanexp", "N in exported manure", "kg N/yr", Nmanexp, parens);
        GlobalVars.Instance.writeInformationToFiles("NExport", "Total amount of N exported", "kg N/yr", NExport, parens);
        GlobalVars.Instance.writeInformationToFiles("housingNH3Loss", "N lost in NH3 emission from housing", "kg N/yr", housingNH3Loss, parens);
        GlobalVars.Instance.writeInformationToFiles("manureN2Emission", "N lost in N2 emission from manure storage", "kg N/yr", manureN2Emission, parens);
        GlobalVars.Instance.writeInformationToFiles("manureN2OEmission", "N lost in N2O emission from manure storage", "kg N/yr", manureN2OEmission, parens);
        GlobalVars.Instance.writeInformationToFiles("manureNH3Emission", "N lost in NH3 emission from manure storage", "kg N/yr", manureNH3Emission, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldUrineNH3Emission", "N lost in NH3 emission from urine deposited in field", "kg N/yr", fieldUrineNH3Emission, parens);
        GlobalVars.Instance.writeInformationToFiles("runoffN", "N lost in runoff from manure storage", "kg N/yr", runoffN, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldN2Emission", "Emission of N2 from the field", "kg N/yr", fieldN2Emission, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldN2OEmission", "Emission of N2O from the field", "kg N/yr", fieldN2OEmission, parens);
        GlobalVars.Instance.writeInformationToFiles("fertNH3NEmission", "N lost via NH3 emission from fertilisers", "kg N/yr", fertNH3NEmission, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldmanureNH3Emission", "N lost as NH3 from field-applied manure", "kg N/yr", fieldmanureNH3Emission, parens);
        GlobalVars.Instance.writeInformationToFiles("Nleaching", "N lost via NO3 leaching from soil", "kg N/yr", Nleaching, parens);
        GlobalVars.Instance.writeInformationToFiles("organicNLeached", "N lost via leaching of organic N from soil", "kg N/yr", organicNLeached, parens);
        GlobalVars.Instance.writeInformationToFiles("burningN2ON", "N2O in gases from burning crop residues", "kg N/yr", burntResidueN2ON, parens);
        GlobalVars.Instance.writeInformationToFiles("burningNH3N", "NH3 in gases from burning crop residues", "kg N/yr", burntResidueNH3N, parens);
        GlobalVars.Instance.writeInformationToFiles("burningNOxN", "NOx in gases from burning crop residues", "kg N/yr", burntResidueNOxN, parens);
        GlobalVars.Instance.writeInformationToFiles("burningOtherN", "N in other gases from burning crop residues", "kg N/yr", burntResidueOtherN, parens);
        GlobalVars.Instance.writeInformationToFiles("NDeltaSoil", "Change in N stored in soil", "kg N/yr", NDeltaSoil, parens);
        GlobalVars.Instance.writeInformationToFiles("NDeltaMineral", "Change in N stored in mineral form in soil", "kg N/yr", changeInMinN, parens);
        GlobalVars.Instance.writeInformationToFiles("totalHouseStoreNloss", "Total N losses from product storage, housing and manure storage", "kg N/yr", totalHouseStoreNloss, parens);
        GlobalVars.Instance.writeInformationToFiles("totalFieldNlosses", "Total N losses from fields", "kg N/yr", totalFieldNlosses, parens);
        GlobalVars.Instance.writeInformationToFiles("changeAllSoilNstored", "Change in N stored in organic and mineral form in soil", "kg N/yr", changeAllSoilNstored, parens);

        GlobalVars.Instance.writeInformationToFiles("totalProcessStorageDMloss", "totalProcessStorageDMloss", "", processStorageCloss / 0.46, parens);

        GlobalVars.Instance.writeInformationToFiles("Nsurplus", "N surplus", "kg N/yr", totalFarmNSurplus, parens);
        GlobalVars.Instance.writeInformationToFiles("Nbalance", "N balance (should be about zero)", "kg N/yr", Nbalance, parens);
        //!Provided the farm has land, write the N flows on a per hectare basis
        if (agriculturalArea > 0)
        {
            GlobalVars.Instance.writeStartTab("PerUnitArea");
            GlobalVars.Instance.writeInformationToFiles("Nmanimp", "N in imported manure", "kg N/ha/yr", Nmanimp / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NFix", "N fixation", "kg N/ha/yr", NFix / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Natm", "N deposited from atmosphere", "kg N/ha/yr", Natm / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NFert", "N in fertiliser", "kg N/ha/yr", NFert / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nbedding", "N in bedding", "kg N/ha/yr", Nbedding / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NPlantProductImported", "N in imported crop products", "kg N/ha/yr", NPlantProductImported / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("processStorageNloss", "N lost from processing/stored crop products", "kg N/ha/yr", processStorageNloss / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nsold", "N sold in crop products", "kg N/ha/yr", Nsold / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nmilk", "N sold in milk", "kg N/ha/yr", Nmilk / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NGrowth", "N exported in meat", "kg N/ha/yr", NGrowth / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nmortalities", "N in mortalities", "kg N/ha/yr", Nmortalities / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nmanexp", "N in exported manure", "kg N/ha/yr", Nmanexp / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NExport", "Total amount of N exported", "kg N/ha/yr", NExport / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("housingNH3Loss", "N lost in NH3 emission from housing", "kg N/ha/yr", housingNH3Loss / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("manureN2Emission", "N lost in N2 emission from manure storage", "kg N/ha/yr", manureN2Emission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("manureN2OEmission", "N lost in N2O emission from manure storage", "kg N/ha/yr", manureN2OEmission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("manureNH3Emission", "N lost in NH3 emission from manure storage", "kg N/ha/yr", manureNH3Emission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("runoffN", "N lost in runoff from manure storage", "kg N/ha/yr", runoffN / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("fieldN2Emission", "Emission of N2 from the field", "kg N/ha/yr", fieldN2Emission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("fieldN2OEmission", "Emission of N2O from the field", "kg N/ha/yr", fieldN2OEmission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("fertNH3NEmission", "N lost via NH3 emission from fertilisers", "kg N/ha/yr", fertNH3NEmission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("fieldmanureNH3Emission", "N lost as NH3 from field-applied manure", "kg N/ha/yr", fieldmanureNH3Emission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("fieldUrineNH3Emission", "N lost in NH3 emission from urine deposited in field", "kg N/ha/yr", fieldUrineNH3Emission / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nleaching", "N lost via NO3 leaching from soil", "kg N/ha/yr", Nleaching / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("organicNLeached", "N lost via leaching of organic N from soil", "kg N/ha/yr", organicNLeached / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burningN2ON", "N2O in gases from burning crop residues", "kg N/ha/yr", burntResidueN2ON / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burningNH3", "NH3 in gases from burning crop residues", "kg N/ha/yr", burntResidueNH3N / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burningNOx", "NOx in gases from burning crop residues", "kg N/ha/yr", burntResidueNOxN / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("burningOtherN", "N in other gases from burning crop residues", "kg N/ha/yr", burntResidueOtherN / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NDeltaSoil", "Change in N stored in soil", "kg N/ha/yr", NDeltaSoil / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NDeltaMineral", "Change in N stored in mineral form in soil", "kg N/ha/yr", changeInMinN / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("Nsurplus", "N surplus", "kg N/ha/yr", totalFarmNSurplus / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NexcretedHousing", "N excreted in housing", "kg N/ha/yr", NexcretedHousing / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NexcretedField", "N excreted in field", "kg N/ha/yr", NexcretedField / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NinGrazedFeed", "N in grazed feed", "kg N/ha/yr", NinGrazedFeed / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("DMinGrazedFeed", "DM in grazed feed", "kg N/ha/yr", DMinGrazedFeed / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NfedInHousing", "N fed in housing", "kg N/ha/yr", NfedInHousing / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("totalHouseStoreNlossInFarmBalance", "Total N losses from product storage, housing and manure storage", "kg N/ha/yr", totalHouseStoreNloss / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("totalFieldNlossesInFarmBalance", "Total N losses from fields", "kg N/ha/yr", totalFieldNlosses / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("changeAllSoilNstoredInFarmBalance", "Change in N stored in organic and mineral form in soil", "kg N/ha/yr", changeAllSoilNstored / agriculturalArea, parens);
            GlobalVars.Instance.writeInformationToFiles("NbalanceInFarmBalance", "N balance (should be about zero)", "kg N/ha/yr", Nbalance / agriculturalArea, parens);
            GlobalVars.Instance.writeEndTab();
        }
        GlobalVars.Instance.writeEndTab();
        GlobalVars.Instance.writeStartTab("FarmDirectGHG");
        GlobalVars.Instance.writeInformationToFiles("entericCH4CO2Eq", "Enteric methane emissions", "kg CO2 equivalents/yr", entericCH4CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("manureCH4CO2Eq", "Manure methane emissions", "kg CO2 equivalents/yr", manureCH4CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("manureN2OCO2Eq", "Manure N2O emissions", "kg CO2 equivalents/yr", manureN2OCO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldN2OCO2Eq", "Field N2O emissions", "kg CO2 equivalents/yr", fieldN2OCO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldCH4CO2Eq", "Field excreta CH4 emissions", "kg CO2 equivalents/yr", fieldCH4CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldCO2Eq", "Fertiliser and liming CO2 emissions", "kg CO2/yr", fieldCO2, parens);        
        GlobalVars.Instance.writeInformationToFiles("soilCO2Eq", "Change in C stored in soil", "kg CO2 equivalents/yr", soilCO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("directGHGEmissionCO2Eq", "Total direct GHG emissions", "kg CO2 equivalents/yr", directGHGEmissionCO2Eq, parens);
        GlobalVars.Instance.writeEndTab();
        GlobalVars.Instance.writeStartTab("FarmIndirectGHG");
        GlobalVars.Instance.writeInformationToFiles("housingNH3CO2Eq", "Housing NH3 emissions", "kg CO2 equivalents/yr", housingNH3CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("manurestoreNH3CO2Eq", "Manure storage NH3 emissions", "kg CO2 equivalents/yr", manurestoreNH3CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldmanureNH3CO2Eq", "NH3 emissions from field-applied manure", "kg CO2 equivalents/yr", fieldmanureNH3CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("fieldfertNH3CO2Eq", "NH3 emissions from fertilisers", "kg CO2 equivalents/yr", fieldfertNH3CO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("leachedNCO2Eq", "N2O emissions resulting from leaching of N", "kg CO2 equivalents/yr", leachedNCO2Eq, parens);
        GlobalVars.Instance.writeInformationToFiles("indirectGHGCO2Eq", "Total indirect emissions", "kg CO2 equivalents/yr", indirectGHGCO2Eq, parens);
        GlobalVars.Instance.writeEndTab();
        GlobalVars.Instance.writeEndTab();
        //! Write diverse farm indicators
        double roughageDMimported = 0;
        double roughageDMExported = 0;
        double farmUnutilisedGrazableDMPercent = 0;
        if ((farmUnutilisedGrazableDM + farmGrazedDM)>0)
            farmUnutilisedGrazableDMPercent=100 * farmUnutilisedGrazableDM/(farmUnutilisedGrazableDM + farmGrazedDM);
        GlobalVars.Instance.GetRoughageExchange(ref roughageDMimported, ref roughageDMExported);
        roughageDMimported /= 1000;
        roughageDMExported /= 1000;
        GlobalVars.Instance.writeStartTab("Indicators");
        GlobalVars.Instance.writeInformationToFiles("FarmMilkProduction", "Total farm milk production", "kg/yr", farmMilkProduction, parens);
        GlobalVars.Instance.writeInformationToFiles("FarmMeatProduction", "Total farm meat production", "tonnes liveweight/yr", farmMeatProduction, parens);
        GlobalVars.Instance.writeInformationToFiles("FarmMilkProductionPerHead", "Farm milk production per head", "kg/yr", avgProductionMilkPerHead, parens);
        GlobalVars.Instance.writeInformationToFiles("MilkProductionPerUnitArea", "Milk production per unit area", "kg/ha/yr", farmMilkProduction / agriculturalArea, parens);
        GlobalVars.Instance.writeInformationToFiles("MeatProductionPerUnitArea", "Meat production per unit area", "kg/ha/yr", farmMeatProduction / agriculturalArea, parens);
        GlobalVars.Instance.writeInformationToFiles("LivestockDMintake", "LivestockDMintake", "kg DM/yr", farmLivestockDM, parens);
        GlobalVars.Instance.writeInformationToFiles("farmConcentrateDM", "farmConcentrateDM", "tonnes DM/yr", farmConcentrateDM, parens);
        GlobalVars.Instance.writeInformationToFiles("farmGrazedDM", "farmGrazedDM", "tonnes/yr", farmGrazedDM, parens);
        GlobalVars.Instance.writeInformationToFiles("farmUnutilisedGrazableDM", "farmUnutilisedGrazableDM", "tonnes DM/yr", farmUnutilisedGrazableDM, parens);
        GlobalVars.Instance.writeInformationToFiles("farmUnutilisedGrazableDMPercent", "farmUnutilisedGrazableDMPercent", "Percent", farmUnutilisedGrazableDMPercent, parens);
        GlobalVars.Instance.writeInformationToFiles("farmDMproduction", "farmDMproduction", "tonnes DM/yr", totalDMproduction, parens);
        GlobalVars.Instance.writeInformationToFiles("farmUtilisedDM", "farmUtilisedDM", "tonnes/yr", utilisedDMproduction, parens);
        GlobalVars.Instance.writeInformationToFiles("FarmHarvestedDM", "FarmHarvestedDM", "tonnes/yr", FarmHarvestDM, parens);
        GlobalVars.Instance.writeInformationToFiles("roughageDMimported", "roughageDMimported", "tonnes/yr", roughageDMimported, parens);
        GlobalVars.Instance.writeInformationToFiles("roughageDMExported", "roughageDMExported", "tonnes/yr", roughageDMExported, parens);
       // GlobalVars.Instance.writeInformationToFiles("farmConcentrateEnergy", "farmConcentrateEnergy", "MJ/yr", farmConcentrateEnergy, parens);
        
        
        GlobalVars.Instance.writeEndTab();

        //! Write the water budget indicators
        GlobalVars.Instance.writeStartTab("WaterBalance");
        GlobalVars.Instance.writeInformationToFiles("precip", "precipitation", "mm", precip, parens);
        GlobalVars.Instance.writeInformationToFiles("evap", "evaporation", "mm", evap, parens);
        GlobalVars.Instance.writeInformationToFiles("transpire", "transpiration", "mm", transpire, parens);
        GlobalVars.Instance.writeInformationToFiles("irrig", "irrigation", "mm", irrig, parens);
        GlobalVars.Instance.writeInformationToFiles("drainage", "drainage", "mm", drainage, parens);
        GlobalVars.Instance.writeInformationToFiles("MaxPlantAvailWater", "MaxPlantAvailWater", "mm", MaxPlantAvailWater, parens);
         GlobalVars.Instance.writeInformationToFiles("biogasSupplementaryN", "Biogas supplementary feedstock", "kgN/yr", biogasSupplN, parens);
       GlobalVars.Instance.writeEndTab();

        GlobalVars.Instance.writeInformationToFiles("totalFarmArea", "total farm area", "ha", agriculturalArea, parens);
        
 
        GlobalVars.Instance.writeStartTab("avgCarbon");
        for (int k = 0; k < GlobalVars.Instance.alltotCFom.Count; k++)
        {
            int rep=CropSequence[k].Getrepeats();
            int lenght = CropSequence[k].GetlengthOfSequence() / rep;

            GlobalVars.Instance.writeInformationToFiles("avgCarbon", "avgCarbon", "-", GlobalVars.Instance.alltotCFom[k].amounts /rep/lenght ,GlobalVars.Instance.alltotCFom[k].parants);
        }
        GlobalVars.Instance.writeEndTab();
        GlobalVars.Instance.writeStartTab("avgN");
        for (int k = 0; k < GlobalVars.Instance.alltotNFom.Count; k++)
        {
            int rep = CropSequence[k].Getrepeats();
            int lenght = CropSequence[k].GetlengthOfSequence() / rep;

            GlobalVars.Instance.writeInformationToFiles("avgN", "avgN", "-", GlobalVars.Instance.alltotNFom[k].amounts / rep / lenght , GlobalVars.Instance.alltotCFom[k].parants);
        }
        GlobalVars.Instance.writeInformationToFiles("biogasSupplementaryN", "Biogas supplementary feedstock", "kgN/yr", biogasSupplN, parens);
        GlobalVars.Instance.writeEndTab();

        //! Now write to the summary Excel (csv) file
        if (agriculturalArea > 0)
        {
            GlobalVars.Instance.writeSummaryExcel("Farm and scenario", "", GlobalVars.Instance.getFarmFilePath()[0]);
            GlobalVars.Instance.writeSummaryExcel("C fixed from atmosphere", "kg C/ha/yr", carbonFromPlants / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in imported feed", "kg C/ha/yr", CPlantProductImported / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in imported bedding", "kg C/ha/yr", CinImportedBedding / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in fertiliser", "kg C/ha/yr", CinImportedFertiliser / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Total_C_input", "kg C/ha/yr", (carbonFromPlants + CPlantProductImported + CinImportedBedding + CinImportedFertiliser) / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in exported milk", "kg C/ha/yr", Cmilk / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in exported meat", "kg C/ha/yr", Cmeat / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in mortalities", "kg C/ha/yr", Cmortalities / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in crop products sold", "kg C/ha/yr", CinCropProductsSold / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in exported manure", "kg C/ha/yr", Cmanexp / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Change in C stored in the soil", "kg C/ha/yr", CDeltaSoil / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Total_C_output", "kg C/ha/yr", (Cmanexp + Cmilk + Cmeat + CinCropProductsSold) / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in CO2 emitted by the soil", "kg C/ha/yr", soilCO2_CEmission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in process storage losses", "kg C/ha/yr", processStorageCloss / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in grazed herbage", "kg C/ha/yr", grazedHerbageC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in pasture-fed feed", "kg C/ha/yr", CinPastureFeed / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in crop products harvested", "kg C/ha/yr", harvestedC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in feed waste", "kg C/ha/yr", CinFeedWaste / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in excreta deposited in housing", "kg C/ha/yr", CInhouseExcreta / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in CO2 lost from housing", "kg C/ha/yr", housingCLoss / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in manure sent to storage", "kg C/ha/yr", CinManureSentToStorage / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in CO2 emission from storage", "kg C/ha/yr", manurestoreCO2C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in CH4 emission from storage", "kg C/ha/yr", manurestoreCH4C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in runoff from storage", "kg C/ha/yr", manurestoreRunoffC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in biogas CO2", "kg C/ha/yr", biogasCO2C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in biogas CH4", "kg C/ha/yr", biogasCH4C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue CO", "kg C/ha/yr", burntResidueCOC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue CO2", "kg C/ha/yr", burntResidueCO2C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue black C", "kg C/ha/yr", burntResidueBlackC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in livestock CH4", "kg C/ha/yr", livestockCH4C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in livestock CO2", "kg C/ha/yr", livestockCO2C / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in biogas supplementary feedstock", "kg C/ha/yr", biogasSupplC / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in feed fed in housing", "kg C/ha/yr", CinFeedFedInHouse / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in bedding used in housing", "kg C/ha/yr", CinBedding / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in soil CH4 emission", "kg C/ha/yr", soilCH4_CEmission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C lost from fertiliser C", "kg C/yr", fertiliserCloss / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("C in excreta deposited on fields", "kg C/ha/yr", excretalCtoPasture / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Change in soil C", "kg C/ha/yr", CDeltaSoil / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Total C emitted", "kg C/ha/yr", CLost / agriculturalArea);
            double FarmCbalance = CInput - (CLost + Cexport);
            double temp = 0;
            ///inputs of N
            GlobalVars.Instance.writeSummaryExcel("N in imported manure", "kg N/yr", Nmanimp / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N fixation", "kg N/ha/yr", NFix / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N deposited from atmosphere", "kg N/ha/yr", Natm / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in fertiliser", "kg N/ha/yr", NFert / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in imported crop products", "kg N/ha/yr", NPlantProductImported / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in biogas supplementary feedstock", "kg N/ha/yr", biogasSupplN / agriculturalArea);
            temp = (Nmanimp + Natm + NFix + NFert + NPlantProductImported + biogasSupplN) / agriculturalArea;
            GlobalVars.Instance.writeSummaryExcel("Total N input", "kg N/ha/yr", temp);
            ///outputs of N
            GlobalVars.Instance.writeSummaryExcel("N sold in crop products", "kg N/ha/yr", Nsold / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N sold in milk", "kg N/ha/yr", Nmilk / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N exported in meat", "kg N/ha/yr", NGrowth / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in mortalities", "kg N/ha/yr", Nmortalities / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in exported manure", "kg N/ha/yr", Nmanexp / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Total amount of N exported", "kg N/ha/yr", NExport / agriculturalArea);
            ///losses of N
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from housing", "kg N/ha/yr", housingNH3Loss / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in N2 emission from manure storage", "kg N/ha/yr", manureN2Emission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in N2O emission from manure storage", "kg N/ha/yr", manureN2OEmission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from manure storage", "kg N/ha/yr", manureNH3Emission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in runoff from manure storage", "kg N/ha/yr", runoffN / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as N2 from the field", "kg N/ha/yr", fieldN2Emission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as N2O from the field", "kg N/ha/yr", fieldN2OEmission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 emission from fertilisers", "kg N/ha/yr", fertNH3NEmission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 from field-applied manure", "kg N/ha/yr", fieldmanureNH3Emission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as NO3 leaching from soil", "kg N/ha/yr", Nleaching / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost from processing/stored crop products", "kg N/ha/yr", processStorageNloss / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from urine deposited in field", "kg N/ha/yr", fieldUrineNH3Emission / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as N2O in gases from burning crop residues", "kg N/ha/yr", burntResidueN2ON / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 in gases from burning crop residues", "kg N/ha/yr", burntResidueNH3N / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost as NOx in gases from burning crop residues", "kg N/ha/yr", burntResidueNOxN / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N lost in other gases from burning crop residues", "kg N/ha/yr", burntResidueOtherN / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("Change in N stored in soil", "kg N/ha/yr", NDeltaSoil / agriculturalArea);
            temp = (housingNH3Loss + manureN2Emission + manureN2OEmission + manureNH3Emission + runoffN + fieldN2Emission + fieldN2OEmission +
                fertNH3NEmission + fieldmanureNH3Emission + processStorageNloss + fieldUrineNH3Emission + Nleaching +
                burntResidueN2ON + burntResidueNH3N + burntResidueNOxN + burntResidueOtherN) / agriculturalArea;
            GlobalVars.Instance.writeSummaryExcel("Total_N_lost", "kg N/ha/yr", temp);
            ///internal flows
            GlobalVars.Instance.writeSummaryExcel("N in grazed feed", "kg N/ha/yr", NinGrazedFeed / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N fed in housing", "kg N/ha/yr", NfedInHousing / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N excreted in field", "kg N/ha/yr", NexcretedField / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N excreted in housing", "kg N/ha/yr", NexcretedHousing / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N input to storage in waste feed", "kg N/ha/yr", storageFromFeedWasteN / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N input to storage from housing", "kg N/ha/yr", storageFromHouseN / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N ex storage", "kg N/ha/yr", manureNexStorage / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N applied in manure to fields", "kg N/ha/yr", (manureNexStorage + Nmanimp - Nmanexp) / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N taken up by crops", "kg N/ha/yr", CropNuptake / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N input to soil in crop residues", "kg N/ha/yr", NinCropResidues / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N removed in crop products harvested", "kg N/ha/yr", Nharvested / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in bedding", "kg N/ha/yr", Nbedding / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in imported bedding", "kg N/ha/yr", NinImportedBedding / agriculturalArea);
            GlobalVars.Instance.writeSummaryExcel("N in pasture-fed feed", "kg N/ha/yr", NfedAtPasture / agriculturalArea);
        }
        else
        {
            GlobalVars.Instance.writeSummaryExcel("Farm and scenario", "", GlobalVars.Instance.getFarmFilePath()[0]);
            GlobalVars.Instance.writeSummaryExcel("C fixed from atmosphere", "kg C/yr", "0");
            GlobalVars.Instance.writeSummaryExcel("C in imported feed", "kg C/yr", CPlantProductImported);
            GlobalVars.Instance.writeSummaryExcel("C in imported bedding", "kg C/yr", CinImportedBedding );
            GlobalVars.Instance.writeSummaryExcel("C in fertiliser", "kg C/yr", "0");
            GlobalVars.Instance.writeSummaryExcel("Total_C_input", "kg C/yr", (carbonFromPlants + CPlantProductImported + CinImportedBedding + CinImportedFertiliser) );
            GlobalVars.Instance.writeSummaryExcel("C in exported milk", "kg C/yr", Cmilk );
            GlobalVars.Instance.writeSummaryExcel("C in exported meat", "kg C/yr", Cmeat );
            GlobalVars.Instance.writeSummaryExcel("C in mortalities", "kg C/yr", Cmortalities );
            GlobalVars.Instance.writeSummaryExcel("C in crop products sold", "kg C/yr", CinCropProductsSold );
            GlobalVars.Instance.writeSummaryExcel("C in exported manure", "kg C/yr", Cmanexp );
            GlobalVars.Instance.writeSummaryExcel("Change in C stored in the soil", "kg C/yr", CDeltaSoil );
            GlobalVars.Instance.writeSummaryExcel("Total_C_output", "kg C/yr", (Cmanexp + Cmilk + Cmeat + CinCropProductsSold) );
            GlobalVars.Instance.writeSummaryExcel("C in CO2 emitted by the soil", "kg C/yr", soilCO2_CEmission );
            GlobalVars.Instance.writeSummaryExcel("C in process storage losses", "kg C/yr", processStorageCloss );
            GlobalVars.Instance.writeSummaryExcel("C in grazed herbage", "kg C/yr", grazedHerbageC );
            GlobalVars.Instance.writeSummaryExcel("C in pasture-fed feed", "kg C/yr", CinPastureFeed );
            GlobalVars.Instance.writeSummaryExcel("C in crop products harvested", "kg C/yr", harvestedC );
            GlobalVars.Instance.writeSummaryExcel("C in feed waste", "kg C/yr", CinFeedWaste );
            GlobalVars.Instance.writeSummaryExcel("C in excreta deposited in housing", "kg C/yr", CInhouseExcreta );
            GlobalVars.Instance.writeSummaryExcel("C in CO2 lost from housing", "kg C/yr", housingCLoss );
            GlobalVars.Instance.writeSummaryExcel("C in manure sent to storage", "kg C/yr", CinManureSentToStorage );
            GlobalVars.Instance.writeSummaryExcel("C in CO2 emission from storage", "kg C/yr", manurestoreCO2C );
            GlobalVars.Instance.writeSummaryExcel("C in CH4 emission from storage", "kg C/yr", manurestoreCH4C );
            GlobalVars.Instance.writeSummaryExcel("C in runoff from storage", "kg C/yr", manurestoreRunoffC );
            GlobalVars.Instance.writeSummaryExcel("C in biogas CO2", "kg C/yr", biogasCO2C );
            GlobalVars.Instance.writeSummaryExcel("C in biogas CH4", "kg C/yr", biogasCH4C );
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue CO", "kg C/yr", burntResidueCOC );
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue CO2", "kg C/yr", burntResidueCO2C );
            GlobalVars.Instance.writeSummaryExcel("C in burnt residue black C", "kg C/yr", burntResidueBlackC );
            GlobalVars.Instance.writeSummaryExcel("C in livestock CH4", "kg C/yr", livestockCH4C );
            GlobalVars.Instance.writeSummaryExcel("C in livestock CO2", "kg C/yr", livestockCO2C );
            GlobalVars.Instance.writeSummaryExcel("C in biogas supplementary feedstock", "kg C/yr", biogasSupplC );
            GlobalVars.Instance.writeSummaryExcel("C in feed fed in housing", "kg C/yr", CinFeedFedInHouse );
            GlobalVars.Instance.writeSummaryExcel("C in bedding used in housing", "kg C/yr", CinBedding );
            GlobalVars.Instance.writeSummaryExcel("C in soil CH4 emission", "kg C/yr", soilCH4_CEmission );
            GlobalVars.Instance.writeSummaryExcel("C lost from fertiliser C", "kg C/yr", fertiliserCloss );
            GlobalVars.Instance.writeSummaryExcel("C in excreta deposited on fields", "kg C/yr", excretalCtoPasture );
            GlobalVars.Instance.writeSummaryExcel("Change in soil C", "kg C/yr", CDeltaSoil );
            GlobalVars.Instance.writeSummaryExcel("Total C emitted", "kg C/yr", CLost );
            double FarmCbalance = CInput - (CLost + Cexport);
            double temp = 0;
            ///inputs of N
            GlobalVars.Instance.writeSummaryExcel("N in imported manure", "kg N/yr", Nmanimp );
            GlobalVars.Instance.writeSummaryExcel("N fixation", "kg N/yr", NFix );
            GlobalVars.Instance.writeSummaryExcel("N deposited from atmosphere", "kg N/yr", Natm );
            GlobalVars.Instance.writeSummaryExcel("N in fertiliser", "kg N/yr", NFert );
            GlobalVars.Instance.writeSummaryExcel("N in imported crop products", "kg N/yr", NPlantProductImported );
            GlobalVars.Instance.writeSummaryExcel("N in biogas supplementary feedstock", "kg N/yr", biogasSupplN );
            temp = (Nmanimp + Natm + NFix + NFert + NPlantProductImported + biogasSupplN) ;
            GlobalVars.Instance.writeSummaryExcel("Total N input", "kg N/yr", temp);
            ///outputs of N
            GlobalVars.Instance.writeSummaryExcel("N sold in crop products", "kg N/yr", Nsold );
            GlobalVars.Instance.writeSummaryExcel("N sold in milk", "kg N/yr", Nmilk );
            GlobalVars.Instance.writeSummaryExcel("N exported in meat", "kg N/yr", NGrowth );
            GlobalVars.Instance.writeSummaryExcel("N in mortalities", "kg N/yr", Nmortalities );
            GlobalVars.Instance.writeSummaryExcel("N in exported manure", "kg N/yr", Nmanexp );
            GlobalVars.Instance.writeSummaryExcel("Total amount of N exported", "kg N/yr", NExport );
            ///losses of N
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from housing", "kg N/yr", housingNH3Loss );
            GlobalVars.Instance.writeSummaryExcel("N lost in N2 emission from manure storage", "kg N/yr", manureN2Emission );
            GlobalVars.Instance.writeSummaryExcel("N lost in N2O emission from manure storage", "kg N/yr", manureN2OEmission );
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from manure storage", "kg N/yr", manureNH3Emission );
            GlobalVars.Instance.writeSummaryExcel("N lost in runoff from manure storage", "kg N/yr", runoffN );
            GlobalVars.Instance.writeSummaryExcel("N lost as N2 from the field", "kg N/yr", fieldN2Emission );
            GlobalVars.Instance.writeSummaryExcel("N lost as N2O from the field", "kg N/yr", fieldN2OEmission );
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 emission from fertilisers", "kg N/yr", fertNH3NEmission );
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 from field-applied manure", "kg N/yr", fieldmanureNH3Emission );
            GlobalVars.Instance.writeSummaryExcel("N lost as NO3 leaching from soil", "kg N/yr", Nleaching );
            GlobalVars.Instance.writeSummaryExcel("N lost from processing/stored crop products", "kg N/yr", processStorageNloss );
            GlobalVars.Instance.writeSummaryExcel("N lost in NH3 emission from urine deposited in field", "kg N/yr", fieldUrineNH3Emission );
            GlobalVars.Instance.writeSummaryExcel("N lost as N2O in gases from burning crop residues", "kg N/yr", burntResidueN2ON );
            GlobalVars.Instance.writeSummaryExcel("N lost as NH3 in gases from burning crop residues", "kg N/yr", burntResidueNH3N );
            GlobalVars.Instance.writeSummaryExcel("N lost as NOx in gases from burning crop residues", "kg N/yr", burntResidueNOxN );
            GlobalVars.Instance.writeSummaryExcel("N lost in other gases from burning crop residues", "kg N/yr", burntResidueOtherN );
            GlobalVars.Instance.writeSummaryExcel("Change in N stored in soil", "kg N/yr", NDeltaSoil );
            temp = (housingNH3Loss + manureN2Emission + manureN2OEmission + manureNH3Emission + runoffN + fieldN2Emission + fieldN2OEmission +
                fertNH3NEmission + fieldmanureNH3Emission + processStorageNloss + fieldUrineNH3Emission + Nleaching +
                burntResidueN2ON + burntResidueNH3N + burntResidueNOxN + burntResidueOtherN) ;
            GlobalVars.Instance.writeSummaryExcel("Total_N_lost", "kg N/yr", temp);
            ///internal flows
            GlobalVars.Instance.writeSummaryExcel("N in grazed feed", "kg N/yr", NinGrazedFeed );
            GlobalVars.Instance.writeSummaryExcel("N fed in housing", "kg N/yr", NfedInHousing );
            GlobalVars.Instance.writeSummaryExcel("N excreted in field", "kg N/yr", NexcretedField );
            GlobalVars.Instance.writeSummaryExcel("N excreted in housing", "kg N/yr", NexcretedHousing );
            GlobalVars.Instance.writeSummaryExcel("N input to storage in waste feed", "kg N/yr", storageFromFeedWasteN );
            GlobalVars.Instance.writeSummaryExcel("N input to storage from housing", "kg N/yr", storageFromHouseN );
            GlobalVars.Instance.writeSummaryExcel("N ex storage", "kg N/yr", manureNexStorage );
            GlobalVars.Instance.writeSummaryExcel("N applied in manure to fields", "kg N/yr", (manureNexStorage + Nmanimp - Nmanexp) );
            GlobalVars.Instance.writeSummaryExcel("N taken up by crops", "kg N/yr", CropNuptake );
            GlobalVars.Instance.writeSummaryExcel("N input to soil in crop residues", "kg N/yr", NinCropResidues );
            GlobalVars.Instance.writeSummaryExcel("N removed in crop products harvested", "kg N/yr", Nharvested );
            GlobalVars.Instance.writeSummaryExcel("N in bedding", "kg N/yr", Nbedding );
            GlobalVars.Instance.writeSummaryExcel("N in imported bedding", "kg N/yr", NinImportedBedding );
            GlobalVars.Instance.writeSummaryExcel("N in pasture-fed feed", "kg N/yr", NfedAtPasture );
        }

        GlobalVars.Instance.writeSummaryExcel("", "", 0);
        GlobalVars.Instance.writeSummaryExcel("", "", 0);

        GlobalVars.Instance.writeSummaryExcel("Enteric methane emissions", "kg CO2 equivalents/yr", entericCH4CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Manure methane emissions", "kg CO2 equivalents/yr", manureCH4CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Manure N2O emissions", "kg CO2 equivalents/yr", manureN2OCO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Field N2O emissions", "kg CO2 equivalents/yr", fieldN2OCO2Eq);
        GlobalVars.Instance.writeSummaryExcel("fieldCH4CO2Eq", "kg CO2 equivalents/yr", fieldCH4CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("fieldCO2", "kg CO2 equivalents/yr", fieldCO2);
        GlobalVars.Instance.writeSummaryExcel("Change in C stored in soil", "kg CO2 equivalents/yr", soilCO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Total direct GHG emissions", "kg CO2 equivalents/yr", directGHGEmissionCO2Eq);
        temp = entericCH4CO2Eq + manureCH4CO2Eq + manureN2OCO2Eq + fieldCH4CO2Eq + fieldN2OCO2Eq + fieldCO2;
        GlobalVars.Instance.writeSummaryExcel("Total direct GHG emissions no soil seq", "kg CO2 equivalents/yr", temp);
        GlobalVars.Instance.writeSummaryExcel("", "", 0);

        GlobalVars.Instance.writeSummaryExcel("Housing NH3 emissions", "kg CO2 equivalents/yr", housingNH3CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Manure storage NH3 emissions", "kg CO2 equivalents/yr", manurestoreNH3CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("NH3 emissions from field-applied manure", "kg CO2 equivalents/yr", fieldmanureNH3CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("NH3 emissions from fertilisers", "kg CO2 equivalents/yr", fieldfertNH3CO2Eq);
        GlobalVars.Instance.writeSummaryExcel("N2O emissions resulting from leaching of N", "kg CO2 equivalents/yr", leachedNCO2Eq);
        GlobalVars.Instance.writeSummaryExcel("Total indirect emissions", "kg CO2 equivalents/yr", indirectGHGCO2Eq);

        GlobalVars.Instance.writeSummaryExcel("", "", 0);
        GlobalVars.Instance.writeSummaryExcel("", "", 0);

        GlobalVars.Instance.writeSummaryExcel("Agricultural area", " ha ", agriculturalArea);
        GlobalVars.Instance.writeSummaryExcel("Number of dairy ruminants", " ", numDairy);
        GlobalVars.Instance.writeSummaryExcel("Number of other ruminants", " ", numOtherRuminants);
        GlobalVars.Instance.writeSummaryExcel("Number of non-ruminants", " ", numNonRuminants);
        GlobalVars.Instance.writeSummaryExcel("Imported concentrate feed", "t dry matter/yr", farmConcentrateDM);
        GlobalVars.Instance.writeSummaryExcel("Total farm milk production", "kg/yr", farmMilkProduction);
        GlobalVars.Instance.writeSummaryExcel("Total farm meat production", "kg liveweight/yr", farmMeatProduction * 1000);
        GlobalVars.Instance.writeSummaryExcel("Farm milk production per head", "kg/yr", avgProductionMilkPerHead);
        GlobalVars.Instance.writeSummaryExcel("Milk production per unit area", "kg/ha/yr", farmMilkProduction / agriculturalArea);

        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimalCategory = listOfLivestock[i];
            string livestockName=anAnimalCategory.Getname();
            double DMintake = anAnimalCategory.GetDMintake()/ GlobalVars.avgNumberOfDays;
            GlobalVars.Instance.writeSummaryExcel(livestockName, "kg DM/day", DMintake);
            double numAnimals = anAnimalCategory.GetAvgNumberOfAnimal();
            GlobalVars.Instance.writeSummaryExcel(livestockName, "number", numAnimals);
        }
    }
}