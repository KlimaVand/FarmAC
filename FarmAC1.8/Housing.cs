using System;
using System.Collections.Generic;
using System.Xml;
/*! A class that named housing. */
public class housing
{
    ///inputs
    //! ID of housing type
    int HousingType;
    //! Name of housing
    string Name;
    //! Proportion of feed that is wasted during feeding (is added to manure)
    double feedWasteFactor;
    //! Reference indoor temperature (Celsius) used in Tier 3 emission methodology
    double HousingRefTemp;
    //! Reference ammonia emission factor used in Tier 3 emission methodology
    double EFNH3ref;
    //! Reference ammonia emission factor used in Tier 2 emission methodology
    double EFNH3housingTier2;
    //! Rate of addition of bedding (kg bedding dry matter/(animal * day)
    double beddingFactor;
    //! Proportion of excreta deposited in housing, if livestock are housed overnight but not fed there.
    double nightTimeProp;
    //! Amount of bedding used (kg dry matter per day)
    double beddingDM;
    //! Amount of C input to the housing (kg)
    double Cinput;
    //! Loss of C as CO2 (kg)
    double CO2C;
    //! Amount of C sent to manure storage (kg)
    double CtoStorage;
    //! Amount of fibre C sent to storage (kg)
    double FibreCToStorage;
    //! Proportion of housed time that is spent in this house
    double propTimeThisHouse;
    //! Mean indoor temperature (Celsius) of Tier 3 methodology
    double meanTemp;
    //! Amount of C added in bedding
    double beddingC;
    string parens; /*!< a string containing information about the farm and scenario number.*/
    //! Pointer to the livestock housed
    livestock theLivestock;
    //! Amount of TAN input to the storage
    double NTanInstore;
    //! In the event that manure is sent to both liquid and solid storage, this is the proportion of degradable organic matter sent to the solid storage
    double proportionDegradable;
    //! In the event that manure is sent to both liquid and solid storage, this is the proportion of nondegradable organic matter sent to the solid storage
    double proportionNondegradable;
    //! In the event that manure is sent to both liquid and solid storage, this is the proportion of TAN sent to the solid storage
    double proportionTAN;
    //! Amount of C in wasted feed (kg)
    double feedWasteC=0;
    //! Amount of N in wasted feed (kg)
    double NWasteFeed = 0;
    //! Amount of N added in bedding (kg)
    double Nbedding = 0;
    //! Amount of TAN entering the housing (kg)
    double NtanInhouse = 0;
    //! Amount of TAN entering the housing (kg)
    double NorgInHouse = 0;
    //! Amount of N added in faeces (kg)
    double faecalN = 0;
    //! N in ammonia emission (kg)
    double NNH3housing = 0;
    //! N in feed fed in housing (kg)
    double NfedInHousing = 0;
    //! Total N input to housing (kg)
    double Ninput = 0;
    //! Total N output from housing (kg)
    double Nout = 0;
    //! Total loss of N from housing (kg)
    double NLost = 0;
    //! N balance check (kg)
    double Nbalance = 0;
    //! Feed item used to hold waste feed
    feedItem wasteFeed = null;
    //! Double array to hold TAN partitioned to up to two manure stores
    double[] TANtoThisStore = new double[2];
    //! Double array to hold labile N partitioned to up to two manure stores
    double[] labileOrganicNtoThisStore = new double[2];
    //! List of feed items that are fed in the housing
    List<feedItem> feedInHouse;
    //! List of manure storage associated with this housing
    List<GlobalVars.manurestoreRecord> manurestoreDetails = new List<GlobalVars.manurestoreRecord>();

    //! Return the proportion of the year when the livestock are in this house. 
    /*!
     \return proportion as a double value.
    */
    public double getPropTimeThisHouse() { return propTimeThisHouse; }
    //! Get the urine C (kg).
    /*!
     \return C in urine (kg/yr) as a double value.
    */
    public double getUrineC() { return theLivestock.GeturineC(); }
    //! Get the faecal C (kg).
    /*!
     \return C in faeces (kg/yr) as a double value.
    */
    public double getFaecesC() { return theLivestock.GetfaecalC(); }
    //! Get the urine N (kg).
    /*!
     \return N in urine (kg/yr) as a double value.
    */
    public double getUrineN() { return NtanInhouse; }
    //! Get the faecal N (kg).
    /*!
     \return N in faeces (kg/yr) as a double value.
    */
    public double getFaecesN() { return faecalN; }
    //! Get the C in bedding material.
    /*!
     \return C in bedding (kg/yr) as a double value.
    */
    public double getBeddingC() { return beddingC; }
    //! Get the N in bedding material.
    /*!
     \return N in bedding (kg/yr) as a double value.
    */
    public double getBeddingN() { return Nbedding; }
    //! Get C in feed waste. 
    /*!
     \return C in feed waste (kg/yr) as a double value.
    */
    public double getFeedWasteC() { return feedWasteC; }
    //! Get N in feed waste. 
    /*!
     \return N in feed waste (kg/yr) as a double value.
    */
    public double getFeedWasteN() { return NWasteFeed; }
    //! Get C lost as CO2. 
    /*!
     \return C lost as CO2 as a double value.
    */
    public double GetCO2C() { return CO2C; }
    //! Get NH3-N emitted from housing. 
    /*!
     \return NH3-N (kg/yr) as a double value.
    */
    public double GetNNH3housing() { return NNH3housing; }
    //! Get N in feed fed in the housing. 
    /*!
     \return N in feed fed in the housing (kg/yr) as a double value.
    */
    public double GetNfedInHousing() { return NfedInHousing; }
    //! Get N input to housing in excreta. 
    /*!
     \return N input in excreta (kg/yr) as a double value.
    */
    public double GetNinputInExcreta() {return (NtanInhouse + faecalN);}
    //! Get C input to housing in excreta. 
    /*!
     \return C input in excreta (kg/yr) as a double value.
    */
    public double GetCinputInExcreta() { return getUrineC() + getFaecesC(); }
    //! Get C sent for to the manure storage that takes manure from this house.
    /*!
     \return C in manure storage (kg/yr) as a double value.
    */
    public double getManureCarbon()
    {
        double returnVal = 0;
        for (int i = 0; i < manurestoreDetails.Count; i++)
        {
            returnVal += manurestoreDetails[i].manureToStorage.GetTotalC();
        }
        return returnVal;
    }
    //! Get N sent for to the manure storage that takes manure from this house.
    /*!
     \return N in manure storage (kg/yr) as a double value.
    */
    public double getManureNitrogen()
    {
        double returnVal = 0;
        for (int i = 0; i < manurestoreDetails.Count; i++)
        {
            returnVal += manurestoreDetails[i].manureToStorage.GetTotalN();
        }
        return returnVal;
    }
    //! Get the amount of C in feed fed in this house. 
    /*!
     \return C in feed fed (kg/yr) as a double value.
    */
    public double GetCinFeedFedInHouse()
    {
        double ret_val = 0;
        for (int i = 0; i < feedInHouse.Count; i++)
        {
            ret_val += feedInHouse[i].Getamount() * feedInHouse[i].GetC_conc();
        }
        return ret_val;
    }
    //! Get the amount of N in feed fed in this house. 
    /*!
     \return N in feed fed (kg/yr) as a double value.
    */
    public double GetNinFeedFedInHouse()
    {
        double ret_val = 0;
        for (int i = 0; i < feedInHouse.Count; i++)
        {
            ret_val += feedInHouse[i].Getamount() * feedInHouse[i].GetN_conc();
        }
        return ret_val;
    }
    //! Get the details of manure stores associated with this house.
    /*!
     \return a list value that points to GlobalVars.manurestoreRecord.
    */
    public List<GlobalVars.manurestoreRecord> GetmanurestoreDetails() { return manurestoreDetails; }
    //! Get a pointer to the livestock in this house.
    /*!
     \return a value that points to the livestock housed.
    */
    public livestock gettheLivestock() { return theLivestock; }
    //! A default constructor
    private housing()
    {
    }
    //! A constructor with five arguments.
    /*!
     \param aHousingType, a unique integer ID .
     \param aLivestock, an class livestock instance.
     \param houseIndex, the index for the housing (livestock can be in more than one housing type) - an integer .
     \param zoneNr, the agroecological zone as an integer argument.
     \param aparens, a string argument.
    */
    public housing(int aHousingType, livestock aLivestock, int houseIndex, int zoneNr, string aparens)
    {
        parens = aparens;
        theLivestock = aLivestock;
        //propDMgrazed = aLivestock.GetpropDMgrazed();
        HousingType = aHousingType;
        feedInHouse = new List<feedItem>();
        FileInformation paramFile = new FileInformation(GlobalVars.Instance.getParamFilePath());
        //Find the housing type in the correct AEZ
        if (HousingType != 0)
        {
            string basePath = "AgroecologicalZone(" + zoneNr.ToString() + ").Housing";
            paramFile.setPath(basePath);
            int minHouse = 99, maxHouse = 0;
            paramFile.getSectionNumber(ref minHouse, ref maxHouse);
            int tmpHousingType = -1;

            bool found = false;
            int num = 0;
            for (int i = minHouse; i <= maxHouse; i++)
            {
                if (paramFile.doesIDExist(i))
                {
                    paramFile.Identity.Add(i);
                    tmpHousingType = paramFile.getItemInt("HousingType");
                    if (tmpHousingType == HousingType)
                    {
                        found = true;
                        num = i;
                        basePath += "(" + num.ToString() + ")";
                        break;
                    }
                    paramFile.Identity.RemoveAt(paramFile.Identity.Count - 1);
                }
            }
            if (found == false)
            {

                string messageString = aLivestock.Getname() + " could not link housing and manure storage";
                GlobalVars.Instance.Error(messageString);
            }
            Name = paramFile.getItemString("Name");
            paramFile.PathNames.Add("feedWasteFactor");
            paramFile.Identity.Add(-1);
            feedWasteFactor = paramFile.getItemDouble("Value");
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "beddingFactor";
            beddingFactor = paramFile.getItemDouble("Value");
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "HousingRefTemp";
            HousingRefTemp = paramFile.getItemDouble("Value");
            nightTimeProp = -1;
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "nightTimeProp";
            nightTimeProp = paramFile.getItemDouble("Value",false);
            if (nightTimeProp > 0.0)
                theLivestock.SetExcretalDistributionHousing(nightTimeProp);
            switch (GlobalVars.Instance.getcurrentInventorySystem())
            {
                case 1: //IPCC 2006 does not estimate NH3 emissions from housing
                    EFNH3housingTier2 = 0.0;
                    break;
                case 2:
                case 3: //IPCC 2019 allows use of UNECE method
                    EFNH3housingTier2 = paramFile.getItemDouble("Value", basePath + ".EFNH3housingTier2(-1)");
                    break;
                case 4:
                    paramFile.PathNames[paramFile.PathNames.Count - 1] = "EFNH3housingRef";
                    EFNH3ref = paramFile.getItemDouble("Value");
                    paramFile.PathNames[paramFile.PathNames.Count - 1] = "meanTemp";
                    meanTemp = paramFile.getItemDouble("Value");
                    break;
            }
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "ProportionDegradable";
            proportionDegradable = paramFile.getItemDouble("Value");
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "ProportionNondegradable";
            proportionNondegradable = paramFile.getItemDouble("Value");
            paramFile.PathNames[paramFile.PathNames.Count - 1] = "ProportionTAN";
            proportionTAN = paramFile.getItemDouble("Value");
            GlobalVars.manurestoreRecord amanurestoreRecord = new GlobalVars.manurestoreRecord();
            int numManureStores = theLivestock.GethousingDetails()[houseIndex].GetManureRecipient().Count;
            // Link the housing to the manure storage that receives its manure
            for (int j = 0; j < numManureStores; j++)
            {
                manureStore aStore = new manureStore(theLivestock.GethousingDetails()[houseIndex].GetManureRecipient()[j].GetStorageType(),
                    theLivestock.GetspeciesGroup(), zoneNr, parens + "_manureStore" + (j + 1).ToString());
                amanurestoreRecord.SettheStore(aStore);
                manure amanureToStore = new manure();
                amanureToStore.Setname(aStore.Getname());
                amanurestoreRecord.SetmanureToStorage(amanureToStore);
                manurestoreDetails.Add(amanurestoreRecord);
            }
        }
        //Get the feed 
        for (int i = 0; i < aLivestock.GetfeedRation().Count; i++)
        {
            feedItem aFeedItem = aLivestock.GetfeedRation()[i];
            if (aFeedItem.GetfedAtPasture())
            {
                string messageString = ("Check that the exclusion of feed fed as pasture works\n");
                GlobalVars.Instance.Error(messageString);
            }
            if ((!aFeedItem.GetisGrazed()|| !aFeedItem.GetfedAtPasture()))
                feedInHouse.Add(aFeedItem);
        }
    }
    //! Returns the amount of C sent to storage. 
    /*!
     \return the amount of C sent to storage (kg/yr) as a double value.
    */
    public double GetCtoStorage() { return CtoStorage; }
    //! Do all the calculations for animal housing.
    public void DoHousing()
    {
        propTimeThisHouse = 0;  //proportion of year the livestock spend in this house
        for (int i = 0; i < theLivestock.GethousingDetails().Count; i++)
        {
            if (theLivestock.GethousingDetails()[i].GetHousingType() == HousingType)
                propTimeThisHouse = theLivestock.GethousingDetails()[i].GetpropTime();
        }
        //Calculate the amount of bedding DM input to house
        beddingDM = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * theLivestock.GetAvgNumberOfAnimal()
            * GlobalVars.avgNumberOfDays * beddingFactor;//1.28  
        //Create instance of feedItem to hold the bedding
        feedItem bedding = new feedItem(GlobalVars.Instance.GetthebeddingMaterial());
        bedding.Setamount(beddingDM);
        //Update the utilisation of the feed item used as bedding material
        GlobalVars.Instance.allFeedAndProductsUsed[bedding.GetFeedCode()].composition.AddFeedItem(bedding, false, true);
        //Fill the list of feed items fed in house
        for (int i = 0; i < theLivestock.GetfeedRation().Count; i++)
        {
            if (!theLivestock.GetfeedRation()[i].GetisGrazed()|| (!theLivestock.GetfeedRation()[i].GetfedAtPasture()))
            {
                feedInHouse[i].Setamount(propTimeThisHouse * theLivestock.GetfeedRation()[i].Getamount());
                feedInHouse[i].SetC_conc(theLivestock.GetfeedRation()[i].GetC_conc());
                feedInHouse[i].SetN_conc(theLivestock.GetfeedRation()[i].GetN_conc());
            }
        }
        //Calculate the mean outside temperature during the housing period
        int daysOnPasture = (int) Math.Round(theLivestock.GetpropDMgrazed() * 365);
        if (GlobalVars.Instance.getcurrentInventorySystem() == 3)
        {
            if (daysOnPasture > 0)
                    meanTemp = GetMeanTemperature(daysOnPasture);
            else
                    meanTemp = GetMeanTemperature(0);
        }
        //Simulate C dynamics
        DoCarbon();
        //Simulate N dynamics
        DoNitrogen();
        //
        for (int i = 0; i < manurestoreDetails.Count; i++)
        {
            GlobalVars.manurestoreRecord amanurestoreRecord = manurestoreDetails[i];
            amanurestoreRecord.GettheStore().Addmanure(amanurestoreRecord.GetmanureToStorage(), amanurestoreRecord.GetpropYearGrazing());
        }
    }

    //! Get the mean outside temperature. Taking one argument and returning one double value.
    /*!
     \param daysOnPasture, one integer argument.
     \return a double value.
    */
    private double GetMeanTemperature(int daysOnPasture)
    {
        int startDay=1;
        int endDay=365;
        double retVal = 0;
        if (daysOnPasture == 0)
            retVal = GlobalVars.Instance.GetDegreeDays(startDay, endDay, 0.0, GlobalVars.Instance.theZoneData.GetaverageAirTemperature(),
                GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude(), GlobalVars.Instance.theZoneData.GetairtemperatureOffset());
        else
        {
            int midpoint = GlobalVars.Instance.theZoneData.GetgrazingMidpoint();
            int halfPoint = midpoint - daysOnPasture / 2;
            if (halfPoint < 0)
            {
                startDay = midpoint + daysOnPasture / 2;
                endDay = 365 - startDay;
                retVal = GlobalVars.Instance.GetDegreeDays(startDay, endDay, 0.0, GlobalVars.Instance.theZoneData.GetaverageAirTemperature(),
                    GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude(), GlobalVars.Instance.theZoneData.GetairtemperatureOffset());
            }
            else
            {
                startDay = 1;
                endDay = midpoint - daysOnPasture / 2;
                retVal = GlobalVars.Instance.GetDegreeDays(startDay, endDay, 0.0, GlobalVars.Instance.theZoneData.GetaverageAirTemperature(),
                    GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude(), GlobalVars.Instance.theZoneData.GetairtemperatureOffset());
                startDay = midpoint + daysOnPasture / 2;
                endDay = 365;
                retVal += GlobalVars.Instance.GetDegreeDays(startDay, endDay, 0.0, GlobalVars.Instance.theZoneData.GetaverageAirTemperature(),
                    GlobalVars.Instance.theZoneData.GetairtemperatureAmplitude(), GlobalVars.Instance.theZoneData.GetairtemperatureOffset());
            }
        }
        retVal /= (365 - daysOnPasture);

        return retVal;
    }
    //! Register the amount of feed wasted. 
    void registerFeedWaste()
    {
        //Create new feed item to hold the feed wasted in housing
        wasteFeed = new feedItem();
        for (int i = 0; i < GlobalVars.Instance.getmaxNumberFeedItems(); i++)
        {
            for (int j = 0; j < feedInHouse.Count; j++)
                if(feedInHouse[j]!=null)
                if (feedInHouse[j].GetFeedCode() == i)
                {
                    feedItem afeedItem = new feedItem();//create empty dummy feedItem
                    afeedItem.setFeedCode(i);
                    afeedItem.AddFeedItem(feedInHouse[j], false);  //Copy house-fed feed item to the new dummy feed item
                    double amountConsumedPerYear=theLivestock.GetAvgNumberOfAnimal() * GlobalVars.Instance.GetavgNumberOfDays() 
                        * feedInHouse[j].Getamount(); //get total amount of feed item consumed in the year
                    double wastedAmountPerYear =feedWasteFactor * amountConsumedPerYear/(1-feedWasteFactor) ; //calculate amount wasted
                    afeedItem.Setamount(wastedAmountPerYear); //set amount in dummy feed item
                    GlobalVars.Instance.allFeedAndProductsUsed[i].composition.AddFeedItem(afeedItem, false); //record the use of this feed (even though it is wasted)
                    wasteFeed.AddFeedItem(afeedItem,true,false); //Add the dummmy feed item to the item holding the waste feed
                    break;
                }
        }
    }
    //! Do the carbon dynamics.
    void DoCarbon()
    {
        if (theLivestock.GetpropExcretaField() < 1.0)
        {
            //calculate the concentration of fibre in feed
            double amount = 0;
            double fibre_conc = 0;
            for (int i = 0; i < feedInHouse.Count; i++)
            {
                if (feedInHouse[i] != null)
                {
                    amount += feedInHouse[i].Getamount();
                    fibre_conc += feedInHouse[i].Getfibre_conc() * feedInHouse[i].Getamount();
                }
            }
            fibre_conc /= amount;
            beddingC = beddingDM * GlobalVars.Instance.GetthebeddingMaterial().GetC_conc();
            registerFeedWaste();
            feedWasteC = wasteFeed.Getamount() * wasteFeed.GetC_conc();
            double urineC = theLivestock.GeturineC() * theLivestock.GetAvgNumberOfAnimal();
            double faecalC = theLivestock.GetfaecalC() * theLivestock.GetAvgNumberOfAnimal();
            //Calculate total input of C to housing
            Cinput = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * (faecalC + urineC) + beddingC + feedWasteC;
            //All urine C is assumed to be lost as CO2
            CO2C = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * urineC;
            //Calculate input of C to storage
            CtoStorage = Cinput - CO2C;
            FibreCToStorage = ((1 - theLivestock.GetpropExcretaField()) * theLivestock.GetCintake() * theLivestock.GetAvgNumberOfAnimal() + feedWasteC) * fibre_conc
                + beddingC * GlobalVars.Instance.GetthebeddingMaterial().Getfibre_conc();
            double nonDegC; //Non-degradable C
            double DegC; //Degradable C
            //Calculate the potential degradation of organic C input to storage, using a weighted average of component items
            double beddingBo = GlobalVars.Instance.GetthebeddingMaterial().GetBo();
            double feedWasteBo = wasteFeed.GetBo();
            double excretaBo = theLivestock.GetBo();
            double manureBo = (beddingC * beddingBo + feedWasteC * feedWasteBo + excretaBo * faecalC) / (beddingC + feedWasteC + faecalC);
            GlobalVars.manurestoreRecord amanurestoreRecord;
            switch (manurestoreDetails.Count)
            {
                case 0:
                    string messageString=("Error - No manure storage destinations");
                    GlobalVars.Instance.Error(messageString);
                    break;
                case 1:
                    //proportionNondegradable is ignored - only one store
                    amanurestoreRecord = manurestoreDetails[0];
                    nonDegC = FibreCToStorage;
                    DegC = CtoStorage - FibreCToStorage;
                    amanurestoreRecord.GetmanureToStorage().SetdegC(DegC);
                    amanurestoreRecord.GetmanureToStorage().SetnonDegC(nonDegC);
                    amanurestoreRecord.GetmanureToStorage().SetBo(manureBo);
                    amanurestoreRecord.SetpropYearGrazing(theLivestock.GetpropDMgrazed());
                    break;
                case 2:
                    bool gotSolid = false;
                    bool gotLiquid = false;
                    for (int i = 0; i < manurestoreDetails.Count; i++)
                    {
                        amanurestoreRecord = manurestoreDetails[i];
                        if ((proportionNondegradable == 0) && (proportionDegradable == 0))
                        {
                            messageString = ("Error - proportionNondegradable & proportionDegradable are both zero, in housing " + Name);
                            GlobalVars.Instance.Error(messageString);
                            break;
                        }
                        if (amanurestoreRecord.GettheStore().GetStoresSolid())
                        {
                            nonDegC = proportionNondegradable * FibreCToStorage;
                            DegC = proportionDegradable * (CtoStorage - FibreCToStorage);
                            if (gotSolid)
                            {
                                messageString = ("Error - two manure storage destinations receive solid manure");
                                GlobalVars.Instance.Error(messageString);
                                break;
                            }
                            else
                                gotSolid = true;
                        }
                        else
                        {
                            nonDegC = (1 - proportionNondegradable) * FibreCToStorage;
                            DegC = (1 - proportionDegradable) * (CtoStorage - FibreCToStorage);
                            if (gotLiquid)
                            {
                                messageString = ("Error - two manure storage destinations receive liquid manure");
                                GlobalVars.Instance.Error(messageString);
                                break;
                            }
                            else
                                gotLiquid = true;
                        }
                        if (DegC < 0)
                        {
                            messageString = "Error - degradable carbon is less than zero for " + Name;
                            GlobalVars.Instance.Error(messageString);
                        }
                        amanurestoreRecord.GetmanureToStorage().SetdegC(DegC);
                        amanurestoreRecord.GetmanureToStorage().SetnonDegC(nonDegC);
                        amanurestoreRecord.GetmanureToStorage().SetBo(manureBo);
                        amanurestoreRecord.SetpropYearGrazing(theLivestock.GetpropDMgrazed());
                        //send C to manure store
                    }
                    break;
                default:
                    messageString=("Error - too manure storage destinations");
                    GlobalVars.Instance.Error(messageString);
                    break;
            }
        }
    }
    //! Do nitrogen dynamics.
    public void DoNitrogen()
    {
        if (theLivestock.GetpropExcretaField() < 1.0)
        {
            //calcualte TAN input
            NtanInhouse = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * theLivestock.GeturineN() * theLivestock.GetAvgNumberOfAnimal(); //1.37
            double amount = 0;
            double N_conc = 0;
            TANtoThisStore[0] = 0;
            labileOrganicNtoThisStore[0] = 0;
            TANtoThisStore[1] = 0;
            labileOrganicNtoThisStore[1] = 0;
            //Calculate amount of N input in feed that is fed inhouse
            for (int i = 0; i < feedInHouse.Count; i++)
            {
                if (feedInHouse[i] != null)
                {
                    amount += feedInHouse[i].Getamount();
                    N_conc += feedInHouse[i].GetN_conc() * feedInHouse[i].Getamount();
                }
            }
            N_conc /= amount;
            NfedInHousing = N_conc * amount * GlobalVars.avgNumberOfDays * theLivestock.GetAvgNumberOfAnimal();
            NWasteFeed = wasteFeed.Getamount() * wasteFeed.GetN_conc();
            Nbedding = beddingDM * GlobalVars.Instance.GetthebeddingMaterial().GetN_conc();
            faecalN = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * theLivestock.GetfaecalN() * theLivestock.GetAvgNumberOfAnimal();
            //calculate organic N input
            NorgInHouse = Nbedding + NWasteFeed + faecalN;
            Ninput = NorgInHouse + NtanInhouse;
            //Calculate NH3 emission
            switch (GlobalVars.Instance.getcurrentInventorySystem())
            {
                case 1://IPCC method, NH3 emissions from housing are included in manure storage emissions 
                    NNH3housing = 0.0;
                    NTanInstore = NtanInhouse;
                    break;
                case 2:
                    //UNECE method           
                    NNH3housing = EFNH3housingTier2 * NtanInhouse;
                    NTanInstore = NtanInhouse - NNH3housing; 
                    break;
                case 3:
                    double KHtheta = Math.Pow(10, -1.69 + 1447.7 / (meanTemp + GlobalVars.absoluteTemp));
                    double KHref = Math.Pow(10, -1.69 + 1447.7 / (HousingRefTemp + GlobalVars.absoluteTemp));
                    double EFNH3theta = (KHref / KHtheta) * EFNH3ref;
                    NNH3housing = propTimeThisHouse * (1 - theLivestock.GetpropExcretaField()) * EFNH3theta * NtanInhouse;
                    NTanInstore = NtanInhouse - NNH3housing;
                    break;
            }
            //Send manure to storage
            GlobalVars.manurestoreRecord amanurestoreRecord;
            switch (manurestoreDetails.Count)
            {
                case 1:
                    amanurestoreRecord = manurestoreDetails[0];
                    TANtoThisStore[0] = NTanInstore;
                    labileOrganicNtoThisStore[0] = NorgInHouse;
                    amanurestoreRecord.GetmanureToStorage().SetTAN(TANtoThisStore[0]);
                    amanurestoreRecord.GetmanureToStorage().SetlabileOrganicN(labileOrganicNtoThisStore[0]);
                    break;
                case 2:
                    for (int i = 0; i < manurestoreDetails.Count; i++)
                    {
                        amanurestoreRecord = manurestoreDetails[i];
                        if (amanurestoreRecord.GettheStore().GetStoresSolid())
                        {
                            TANtoThisStore[i] = proportionTAN * NTanInstore;
                            labileOrganicNtoThisStore[i] = proportionDegradable * NorgInHouse;
                        }
                        else
                        {
                            TANtoThisStore[i] = (1 - proportionTAN) * NTanInstore;
                            labileOrganicNtoThisStore[i] = (1 - proportionDegradable) * NorgInHouse;
                        }
                        amanurestoreRecord.GetmanureToStorage().SetTAN(TANtoThisStore[i]);
                        amanurestoreRecord.GetmanureToStorage().SetlabileOrganicN(labileOrganicNtoThisStore[i]);
                    }
                    break;
                default:
                    string messageString=("Error - too many manure storage destinations");
                    
                    GlobalVars.Instance.Error(messageString);
                    break;
            }
        }
    }
    //! Write details of housing to file.
    public void Write()
    {
        if (GlobalVars.Instance.getRunFullModel())
            theLivestock.Write();
        GlobalVars.Instance.writeStartTab("Housing");
        for (int i = 0; i < feedInHouse.Count; i++)
        {
            if (feedInHouse[i] != null)
            feedInHouse[i].Write(parens+"_");
        }
        GlobalVars.Instance.writeInformationToFiles("Name", "Name", "-", Name, parens);
        GlobalVars.Instance.writeInformationToFiles("HousingType", "Type of housing", "-", HousingType, parens);

        GlobalVars.Instance.writeInformationToFiles("Cinput", "Total C input to housing", "kg", Cinput, parens);
        GlobalVars.Instance.writeInformationToFiles("CO2C", "CO2-C emitted from housing", "kg", CO2C, parens);
        GlobalVars.Instance.writeInformationToFiles("CtoStorage", "C sent to manure storage", "kg", CtoStorage, parens);
        //GlobalVars.Instance.writeInformationToFiles("HousingRefTemp", "Reference temperature for housing NH3 emissions", "Celsius", HousingRefTemp);

        GlobalVars.Instance.writeInformationToFiles("Ninput", "Total N entering the housing", "kg", Ninput, parens);
        GlobalVars.Instance.writeInformationToFiles("NtanInhouse", "TAN entering the housing", "kg", NtanInhouse, parens);
        GlobalVars.Instance.writeInformationToFiles("NorgInHouse", "Organic N entering the housing", "kg", NorgInHouse, parens);
        GlobalVars.Instance.writeInformationToFiles("faecalNInHouseing", "Faecal N deposited", "kg", faecalN, parens);
        GlobalVars.Instance.writeInformationToFiles("Nbedding", "N added in bedding", "kg", Nbedding, parens);
        GlobalVars.Instance.writeInformationToFiles("NWasteFeed", "N added in waste feed", "kg", NWasteFeed, parens);
        GlobalVars.Instance.writeInformationToFiles("NNH3housing", "NH3-N emitted from housing", "kg", NNH3housing, parens);

        GlobalVars.Instance.writeInformationToFiles("NLost", "Total N lost", "kg", NLost, parens);
        GlobalVars.Instance.writeInformationToFiles("Nout", "N leaving housing", "kg", Nout, parens);
        GlobalVars.Instance.writeInformationToFiles("Nbalance", "N budget check", "kg", Nbalance, parens);
        if (!GlobalVars.Instance.getRunFullModel())
            GlobalVars.Instance.writeEndTab();
    }
    //! Check the housing C Balance.
    /*!
     * Checks whether the C balance is closed. If not, an error is generated and the function is terminated
    \return will always return false.
   */
    public bool CheckHousingCBalance()
    {
        bool retVal = false;
        double Cout = getManureCarbon();
        double CLost = CO2C;
        double Cbalance = Cinput - (Cout + CLost);
        double diff = Cbalance / Cinput;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
            double errorPercent = 100 * diff;
            string messageString=("Error; Housing C balance error is more than the permitted margin\n");
            messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
            GlobalVars.Instance.Error(messageString);
        }
        return retVal;
    }
    //!  Check Housing N Balance.
    /*!
    \return a boolean value.
   */
    //! Check the housing N Balance.
    /*!
     * Checks whether the N balance is closed. If not, an error is generated and the function is terminated
    \return will always return false.
   */
    public bool CheckHousingNBalance()
    {
        bool retVal = false;
        Nout = getManureNitrogen();
        NLost = NNH3housing;
        Nbalance = Ninput - (Nout + NLost);
        double diff = Nbalance / Ninput;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
                double errorPercent = 100 * diff;
                string messageString =("Error; Housing N balance error is more than the permitted margin\n");
                messageString+=("Percentage error = " + errorPercent.ToString("0.00") + "%");
                GlobalVars.Instance.Error(messageString);
        }
        return retVal;
    }
  
}
