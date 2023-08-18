using System;
using System.Collections.Generic;
using System.Xml;
//! A class that named manureStore. 
public class manureStore
{
    string path;
    //! Name of the manure storage
    string name; 
    //! ID of the storage type
    int ManureStorageID;
    //! Species of livestock sending manure to this storage
    int speciesGroup;
    double StorageRefTemp;
    //! Mean temperature of the storage (Celsius)
    double meanTemp;
    //! Methane convertion factor
    double MCF; //needs to be AEZ specific
    //! Emission factor for NH3
    double EFStoreNH3;
    //! Emission factor for N2O
    double EFStoreN20;
    //! Multiplier to calculate N2 emission from N2O emission
    double Lambda;
    //! Proportion of biogas that is captured (e.g. for utilisation)
    double propGasCapture;

    string parens; /*!<! a string containing information about the farm and scenario number.*/ 
    //! Unique identity number
    int identity;
    //! Pointer to instance of the manure stored
    manure theManure;
    //! Pointer to instance of the housing from which manure is sent
    housing theHousing;

    double tstore=0;
    //! Rate of degradation of organic C in storage (Not used at present)
    double CdegradationRate;
    //! Total input of C (kg/yr)
    double Cinput=0;
    //! Methane C emission from storage (kg/yr)
    double CCH4ST=0;
    //! CO2 C emission from storage (kg/yr)
    double CCO2ST = 0;
    //! Proportion of organic C degraded in storage
    double Cdegradation = 0;
    //! Amount of TAN input to the storage (kg/yr)
    double NTanInstore;
    //! Amount of labile N input to the storage (kg/yr)
    double NlabileOrgInstore = 0;
    //! Amount of humic N input to the storage (kg/yr)
    double NhumicInstore = 0;
    //! Amount of degradable  output from the storage (kg/yr) Not used
    double NDegOrgOut;
    //! Proportion of manure organic matter that is lost as runoff
    double ohmOrg;
    //! Proportion of manure TAN that is lost as runoff
    double ohmTAN;
    //! Amount of N lost from the storage as labile N in runoff (kg/yr)
    double NRunOffLabileOrg;
    //! Amount of C lost from the storage in runoff (kg/yr)
    double CRunOffOrg;
    //! New humic N created during degradation of organic N in storage (kg/yr)
    double newNHUM;
    //! N in humic N lost from storage in runoff (kg/yr)
    double NRunoffHum;
    //! N in labile organic matter output from storage (kg/yr)
    double NlabileOrgOutStore;
    //! N in humic organic matter output from storage (kg/yr)
    double NhumicOutstore = 0;
    //! TAN output from storage (kg/yr)
    double NTanOutstore;
    //! TAN lost from storage (kg/yr)
    double NTANLost;
    //! TAN lost from storage in runoff (kg/yr)
    double NrunoffTan;
    //! Total N lost from storage as NH3 (kg/yr)
    double totalNstoreNH3;
    //! Total N lost from storage as N2 (kg/yr)
    double totalNstoreN2;
    //! Total N lost from storage as N2O (kg/yr)
    double totalNstoreN20;
    //! Total N input to storage (kg/yr)
    double Ninput = 0;
    //! Total N ouput from storage (kg/yr)
    double Nout = 0;
    //! Total N lost from storage (kg/yr)
    double NLost = 0;
    //! Check for closure of N budget (kg/yr). Error generated if not very close to zero.
    double Nbalance = 0;
    //! Methane C (kg/yr) captured in biogas.
    double biogasCH4C = 0;
    //! CO2 C (kg/yr) captured in biogas.
    double biogasCO2C = 0;
    //! Input of N in supplementary feedstock for biogas production (kg/yr)
    double supplementaryN = 0;
    //! Input of C in supplementary feedstock for biogas production (kg/yr)
    double supplementaryC = 0;
    //! List of organic matter feedstocks added to biogas reactor
    public List<feedItem> supplementaryFeedstock;
    //! Get CH4-C emission from storage (kg/yr)
    /*!
     \return CH4-C emission from storage (kg/yr) as a double value.
    */
    public double GetCCH4ST() { return CCH4ST; }
    //! Get CO2-C emission from storage (kg/yr)
    /*!
     \return CO2-C emission from storage (kg/yr) as a double value.
    */
    public double GetCCO2ST() { return CCO2ST; }
    //! Get total N in storage NH3 emission.
    /*!
     \return otal N in storage NH3 emission (kg/yr) as a double value.
    */
    public double GettotalNstoreNH3() { return totalNstoreNH3; }
    //! Get total N in storage N2 emission.
    /*!
     \return otal N in storage N2 emission (kg/yr) as a double value.
    */
    public double GettotalNstoreN2() { return totalNstoreN2; }
    //! Get total N in storage N2O emission.
    /*!
     \return otal N in storage N2O emission (kg/yr) as a double value.
    */
    public double GettotalNstoreN20() { return totalNstoreN20; }
    //! Get run off N. 
    /*!
     \return N in runoff (kg/yr) as a double value.
    */
    public double GetrunoffN() { return NRunoffHum + NRunOffLabileOrg + NrunoffTan; }
    //! Get run off C. 
    /*!
     \return C in runoff (kg/yr) as a double value.
    */
    public double GetrunoffC() { return CRunOffOrg; }
    //! Get biogas CH4-C.
    /*!
     \return biogas CH4-C (kg/yr) as a double value.
    */
    public double GetbiogasCH4C() { return biogasCH4C; }
    //! Get biogas CO2-C.
    /*!
     \return biogas CO2-C (kg/yr) as a double value.
    */
    public double GetbiogasCO2C() { return biogasCO2C; }
    //! Get C in supplementary feedstock. 
    /*!
     \return C in supplementary feedstock (kg/yr) as a double value.
    */
    public double GetsupplementaryC() { return supplementaryC; }
    //! Get N in supplementary feedstock. 
    /*!
     \return N in supplementary feedstock (kg/yr) as a double value.
    */
    public double GetsupplementaryN() { return supplementaryN; }
    //! Get C in manure.
    /*!
     \return C in manure (kg/yr) as a double value.
    */
    public double GetManureC()
    {
        double retVal = 0;
        retVal = theManure.GetdegC() + theManure.GethumicC() + theManure.GetnonDegC();
        return retVal;
    }
    //! Get N in manure.
    /*!
     \return N in manure (kg/yr) as a double value.
    */
    public double GetManureN()
    {
        double retVal = 0;
        retVal = theManure.GetTAN()+ theManure.GetlabileOrganicN() + theManure.GethumicN();
        return retVal;
    }
    //! A default constructor. 
    private manureStore()
    {    }
    //! A constructor with four arguments.
    /*!
     \param aPath, path to input file as string argument.
     \param id, unique ID as integer argument.
     \param zoneNr, agroecological zone as integer argument.
     \param aparens, one string argument.
    */
    public manureStore(string aPath, int id, int zoneNr, string aparens)
    {
        supplementaryFeedstock = new List<feedItem>();
        string parens = aparens;
        FileInformation manureStoreFile = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        identity = id;
        path=aPath+'('+id.ToString()+')';

        manureStoreFile.setPath(path);
        name=manureStoreFile.getItemString("NameOfStorage");
        ManureStorageID = manureStoreFile.getItemInt("StorageType");
        speciesGroup = manureStoreFile.getItemInt("SpeciesGroup");
        getParameters(zoneNr);
    }
    //! A constructor with four arguments.
    /*!
     \param manureStorageType, mannure type ID as integer argument.
     \param livestockSpeciesGroup, livestock type as integer argument.
     \param zoneNr, agroecological zone as integer argument.
     \param aparens, one string argument.
    */
    public manureStore(int manureStorageType, int livestockSpeciesGroup, int zoneNr, string aparens)
    {
        supplementaryFeedstock = new List<feedItem>();
        ManureStorageID = manureStorageType;
        speciesGroup = livestockSpeciesGroup;
        getParameters(zoneNr);
        parens = aparens;
    }
    //! Get parameters for manure store.
    /*!
     \param zoneNr, agroecological zone as integer argument.
    */
    public void getParameters(int zoneNr)
    {
        FileInformation manureParamFile = new FileInformation(GlobalVars.Instance.getParamFilePath());
        manureParamFile.setPath("AgroecologicalZone("+zoneNr.ToString()+").ManureStorage");
        int maxManure = 0, minManure = 99;
        manureParamFile.getSectionNumber(ref minManure, ref maxManure);

        bool found = false;
        int num=0;
        //GlobalVars.Instance.log("ind " + " Req " + " test" + " sg ");  //for testing
        // find the correct manure storage, using the storage type and species of livestock
        for (int i = minManure; i <= maxManure; i++)
        {
            if (manureParamFile.doesIDExist(i))
            {
                manureParamFile.Identity.Add(i);
                int tmpStorageType = manureParamFile.getItemInt("StorageType");
                int tmpSpeciesGroup = manureParamFile.getItemInt("SpeciesGroup");
                name = manureParamFile.getItemString("Name");
                if (ManureStorageID == tmpStorageType & speciesGroup == tmpSpeciesGroup)
                {
                    found = true;
                    num = i;
                    break;
                }
                manureParamFile.Identity.RemoveAt(manureParamFile.Identity.Count - 1);
            }
        }
        if (found == false)
        {
          string messageString = ("could not match StorageType and SpeciesGroup at ManureStore. Was trying to find StorageType " + ManureStorageID.ToString() + " and speciesGroup " + speciesGroup.ToString());
          GlobalVars.Instance.Error(messageString);
        }
        string RecipientPath = "AgroecologicalZone("+zoneNr.ToString()+").ManureStorage" + '(' + num.ToString() + ").StoresSolid(-1)";
        bool StoresSolid;
        string tempString = manureParamFile.getItemString("Value",RecipientPath);
        if (tempString == "true")
            StoresSolid = true;
        else
            StoresSolid = false;
        //these parameters are for the Tier 3 method that is not currently used
        manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "ohmOrg";
        ohmOrg = manureParamFile.getItemDouble("Value");
        manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "ohmTAN";
        ohmTAN = manureParamFile.getItemDouble("Value");
        switch (GlobalVars.Instance.getcurrentInventorySystem())
        {
            case 1://IPCC 2006
                //manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "MCF";
                //MCF = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFNH3storageIPCC";
                EFStoreNH3 = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFN2OstorageIPCC";
                EFStoreN20 = manureParamFile.getItemDouble("Value");
                break;
            case 2: 
            case 3: //IPCC 2019 allows UNECE method
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "MCF";
                MCF = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFNH3storageRef";
                EFStoreNH3 = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFN2OstorageIPCC";
                EFStoreN20 = manureParamFile.getItemDouble("Value");
                break;
            case 4:
                // Componenet of Tier 3 method (not used)
                double b1;
                double lnArr;
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "b1";
                b1 = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "lnArr";
                lnArr = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "meanTemp";
                meanTemp = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFNH3storageRef";
                EFStoreNH3 = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "EFN2OstorageRef";
                EFStoreN20 = manureParamFile.getItemDouble("Value");
                manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "StorageRefTemp";
                StorageRefTemp = manureParamFile.getItemDouble("Value");
                break;
        }
        manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "PropGasCapture";
        propGasCapture = manureParamFile.getItemDouble("Value");
        manureParamFile.PathNames[manureParamFile.PathNames.Count - 1] = "lambda_m";
        Lambda = manureParamFile.getItemDouble("Value");
        //See if this is a biogas reactor that receives supplementary feedstocks
        string aPath = "AgroecologicalZone(" + zoneNr.ToString() + ").ManureStorage(" + num.ToString() + ").SupplementaryFeedstocks(-1).Feedstock";
        manureParamFile.setPath(aPath);
        int minsuppFeed = 99, maxsuppFeed = 0;
        manureParamFile.getSectionNumber(ref minsuppFeed, ref maxsuppFeed);
        for (int k = minsuppFeed; k <= maxsuppFeed; k++)
        {
            manureParamFile.Identity.Add(k);
            feedItem aFeedstock = new feedItem();
            aFeedstock.Setamount(manureParamFile.getItemDouble("Amount"));
            aFeedstock.GetStandardFeedItem(manureParamFile.getItemInt("FeedCode"));
            supplementaryFeedstock.Add(aFeedstock);
            manureParamFile.Identity.RemoveAt(manureParamFile.Identity.Count - 1);
        }
        // find the correct manure that is stored, using the manure type and species of livestock
        theManure = new manure();
        theManure.SetisSolid(StoresSolid);
        //indicate the type of manure
        theManure.SetspeciesGroup(speciesGroup);
        FileInformation file = new FileInformation(GlobalVars.Instance.getfertManFilePath());
        file.setPath("AgroecologicalZone("+GlobalVars.Instance.GetZone().ToString()+").manure");
        int min = 99; int max = 0;
        file.getSectionNumber(ref min, ref max);
  
        bool gotit = false;
        int j = min;
        while ((j <= max) && (gotit == false))
        {
            if(file.doesIDExist(j))
            {
        
                file.Identity.Add(j);
                int StoredTypeFile = file.getItemInt("ManureType");
                int SpeciesGroupFile = file.getItemInt("SpeciesGroup");
                string manureName = file.getItemString("Name");
                if (StoredTypeFile == ManureStorageID && SpeciesGroupFile == speciesGroup)
                {
                    //itemNr = j;
                    theManure.SetmanureType(ManureStorageID);
                    theManure.Setname(manureName);
                    gotit = true;
                }
                j++;
                file.Identity.RemoveAt(file.Identity.Count-1);
            }
        }
        if (gotit == false)
        {
            string messageString = "Error - manure type not found for manure storage " + name + " ManureStorageID = " 
                + ManureStorageID.ToString() + " Species group = " + speciesGroup.ToString();
            GlobalVars.Instance.Error(messageString);
        }
    }
    //! Set the housing from which the manure comes.
    /*!
     \param ahouse, a pointer argument to the housing.
    */
    public void SettheHousing(housing ahouse){theHousing=ahouse;}
    //! Get the name of the housing.
    /*!
     \return name as a string value.
    */
    public string Getname() { return name; }
    //! Determine if the storage stores solid manure.
    /*!
     \return true if the storage stores solid manure.
    */
    public bool GetStoresSolid() { return theManure.GetisSolid(); }
    //! Adds manure to storage.
    /*!
     \param amanure, pointer to instance of manure class.
     \param proportionOfYearGrazing, proportion of the year grazing.
    */
    public void Addmanure(manure amanure, double proportionOfYearGrazing)
    {
        theManure.AddManure(amanure);
        Cinput += amanure.GetdegC();
        Cinput += amanure.GetnonDegC();
        tstore += ((1 - proportionOfYearGrazing) * (theManure.GetdegC() + theManure.GetnonDegC()) / (2 * (theManure.GetdegC() + theManure.GetnonDegC())));
    }
    //! Update the database of manure transactions.
    public void UpdateManureExchange()
    {
        manure manureToManureExchange = new manure(theManure);
        //manureToManureExchange.IncreaseManure(GlobalVars.Instance.theZoneData.GetaverageYearsToSimulate());
        GlobalVars.Instance.theManureExchange.AddToManureExchange(manureToManureExchange);
    }
    //! Do the manure store dynamics.
    public void DoManurestore()
    {
        supplementaryC = 0;
        supplementaryN = 0;
        double temp = theManure.GetnonDegC() + theManure.GetdegC() +theManure.GetTAN() + theManure.GetorganicN();
        if (temp > 0.0)
        {
            double Bo = theManure.GetBo();
            //Check to see if we need to account for supplementary feedstock (i.e. this is a biogas reactor)
            if (supplementaryFeedstock.Count > 0)
            {
                double degSupplC = 0;
                double nondegSupplC = 0;
                double supplN = 0;
                double manureDM = Cinput / 0.42; //hack!
                double cumBo = 0;
                double cumSupplDM=0;
                for (int i = 0; i < supplementaryFeedstock.Count; i++)
                {
                    feedItem aFeedstock = supplementaryFeedstock[i];
                    double amountThisFeedstock = manureDM * aFeedstock.Getamount(); //kg DM
                    if (amountThisFeedstock > 0)
                    {
                        cumSupplDM += amountThisFeedstock;
                        cumBo += amountThisFeedstock * aFeedstock.GetBo();
                        nondegSupplC += amountThisFeedstock * aFeedstock.GetC_conc() * aFeedstock.Getfibre_conc();
                        degSupplC += amountThisFeedstock * aFeedstock.GetC_conc() * (1 - aFeedstock.Getfibre_conc());
                        supplementaryC += nondegSupplC + degSupplC;
                        supplN = amountThisFeedstock * aFeedstock.GetN_conc();
                        supplementaryN += supplN;
                        aFeedstock.Setamount(amountThisFeedstock); //convert from amount per unit manure DM mass to total amount
                        GlobalVars.Instance.allFeedAndProductsUsed[aFeedstock.GetFeedCode()].composition.AddFeedItem(aFeedstock, false);
                    }
                }
                if (cumSupplDM > 0)
                {
                    double aveSupplBo = cumBo / cumSupplDM;
                    Bo = (manureDM * Bo + cumSupplDM * aveSupplBo) / (manureDM + cumSupplDM);
                }
                theManure.SetBo(Bo);
                theManure.SetdegC(theManure.GetdegC() + degSupplC);
                theManure.SetnonDegC(theManure.GetnonDegC() + nondegSupplC);
                theManure.SetlabileOrganicN(theManure.GetlabileOrganicN() + supplementaryN);
            }
            DoCarbon();
            DoNitrogen();
            CheckManureStoreNBalance();
            UpdateManureExchange();
        }
    }
    //! Do the carbon dynamics.
    public void DoCarbon()
    {
        Cinput = GetManureC();
        double tor = GlobalVars.Instance.gettor();
        double aveTemperature = GlobalVars.Instance.theZoneData.GetaverageAirTemperature();
        double Bo = theManure.GetBo();
        double VS = 0.0;
        double biogasC =0.0;
        CRunOffOrg = Cinput * ohmOrg; //assume runoff occurs immediately, before degradation
        theManure.SetdegC(theManure.GetdegC() * (1 - ohmOrg));
        theManure.SetnonDegC(theManure.GetnonDegC() * (1 - ohmOrg));
        switch (GlobalVars.Instance.getcurrentInventorySystem())
        {
            case 1://IPCC 2006
                VS = theHousing.gettheLivestock().GetAvgNumberOfAnimal() * ((theHousing.gettheLivestock().GetExcretedC() - theHousing.gettheLivestock().GetCexcretionToPasture())/
                    GlobalVars.Instance.getalpha());
                //hardcoded numbering - should be read from parameter file
                if ((theManure.GetmanureType() != 5) && (theManure.GetmanureType() < 11))  
                {
                    bool isCovered = false;
                    switch (theManure.GetmanureType())
                    {
                        case 2: isCovered = true;
                            break;
                        case 4: isCovered = true;
                            break;
                        case 9: isCovered = true;
                            break;
                        case 10: isCovered = true;
                            break;
                    }
                    if (GlobalVars.Instance.getcurrentInventorySystem() == 1)
                    {
                        if (theManure.GetisSolid())
                        {
                            if (aveTemperature < 14.5)
                                MCF = 0.02;
                            if ((aveTemperature >= 14.5) && (aveTemperature < 25.5))
                                MCF = 0.04;
                            if (aveTemperature >= 25.5)
                                MCF = 0.05;
                        }
                        else
                        {
                            if (isCovered)
                                MCF = Math.Exp(0.0896159864767708 * aveTemperature - 3.1458426322101);
                            else
                                MCF = Math.Exp(0.088371620269402 * aveTemperature - 2.64281541545576);
                        }
                    }
                }
                CCH4ST = MCF * VS * Bo * 0.67 * 12 / 16;
                CCO2ST = (CCH4ST * (1 - tor)) / tor;//1.47
                biogasC = CCH4ST + CCO2ST;
                Cdegradation = biogasC / (1 - GlobalVars.Instance.getHumification_const());
                break;
            case 2:
                VS = (theManure.GetdegC() + theManure.GetnonDegC() + theManure.GethumicC()) / GlobalVars.Instance.getalpha();
                CCH4ST = MCF * VS * Bo * 0.67 * 12 / 16;
                CCO2ST = (CCH4ST * (1 - tor)) / tor;//1.47
                biogasC = CCH4ST + CCO2ST;
                Cdegradation = biogasC / (1 - GlobalVars.Instance.getHumification_const());
                break;
            case 3: //IPCC 2019
                VS = theHousing.gettheLivestock().GetAvgNumberOfAnimal() * ((theHousing.gettheLivestock().GetExcretedC() - theHousing.gettheLivestock().GetCexcretionToPasture()) /
                    GlobalVars.Instance.getalpha());
                CCH4ST = MCF * VS * Bo * 0.67 * 12 / 16;
                CCO2ST = (CCH4ST * (1 - tor)) / tor;//1.47
                biogasC = CCH4ST + CCO2ST;
                Cdegradation = biogasC / (1 - GlobalVars.Instance.getHumification_const());
                break;
            case 4:
                string message1 = "Un-upgraded code in manure storage " + name;
                double rgas = GlobalVars.Instance.getrgas();
                GlobalVars.Instance.Error(message1);
                /*            CdegradationRate = b1 * Math.Pow(Math.E, lnArr - GlobalVars.Instance.getEapp() * (1 / (rgas * (meanTemp + GlobalVars.absoluteTemp)))); //1.48
                            CdegST = theManure.GetdegC() * (1 - Math.Pow(Math.E, -CdegradationRate * tstore * GlobalVars.avgNumberOfDays));//1.49
                            CCH4ST = CdegST * tor;//1.51
                            CCO2ST = (CdegST * (1 - tor));//1.52
                            theManure.SetdegC(theManure.GetdegC() - CdegST * (1 + GlobalVars.Instance.getHumification_const()));*/
                break;//disable until can get workng properly
                      //theManure.SethumicC(GlobalVars.Instance.getHumification_const() * CdegST);
        }
        if (Cdegradation > theManure.GetdegC())
        {
            if (Cdegradation > (theManure.GetdegC() + theManure.GetnonDegC()))
            {
                string message2 = "C degradation greater than sum of degradable and non-degradable C in store " + name;
                GlobalVars.Instance.Error(message2);
            }
            else
            {
                double nonDegCdegraded = Cdegradation - theManure.GetdegC();
                theManure.SetdegC(0.0);
                theManure.SetnonDegC(theManure.GetnonDegC() - nonDegCdegraded);
            }
        }
        else
            theManure.SetdegC(theManure.GetdegC() - Cdegradation);
        theManure.SethumicC(theManure.GethumicC() + Cdegradation * GlobalVars.Instance.getHumification_const());
        biogasCH4C = propGasCapture * CCH4ST;
        CCH4ST -= biogasCH4C;
        biogasCO2C = propGasCapture * CCO2ST;
        CCO2ST -= biogasCO2C;
        CheckManureStoreCBalance();
    }
    //! Do the nitrogen dynamics.
    public void DoNitrogen()
    {
        NTanInstore = theManure.GetTAN();
        NlabileOrgInstore = theManure.GetlabileOrganicN();
        NhumicInstore = theManure.GethumicN();
        double totalOrgNdegradation = 0; //only used if GlobalVars.Instance.getcurrentInventorySystem() = 1
        double newTAN = 0;
        switch (GlobalVars.Instance.getcurrentInventorySystem())
        {
            case 1: 
            case 2:
                NRunOffLabileOrg = NlabileOrgInstore * ohmOrg; //assume runoff occurs immediately, before degradation
                theManure.SetlabileOrganicN(theManure.GetlabileOrganicN() - NRunOffLabileOrg);
                NRunoffHum = theManure.GethumicN() * ohmOrg;
                theManure.SethumicN(theManure.GethumicN() - NRunoffHum);  //adjust humic N for loss in runoff
                totalOrgNdegradation = (Cdegradation / (Cinput-CRunOffOrg)) * NlabileOrgInstore;
                newNHUM = (theManure.GethumicC() / GlobalVars.Instance.getCNhum()) - theManure.GethumicN();
                if (totalOrgNdegradation < newNHUM)  //there will be immobilisation of TAN in humic N
                {
                    if ((theManure.GetTAN() + totalOrgNdegradation) < newNHUM)  //there is insufficient mineral N to create the humic N
                    {
                        string message2 = "Insufficient mineral N to create new humic N in " + name;
                        GlobalVars.Instance.Error(message2);
                    }
                    else
                    {
                        double immobilisedTAN = newNHUM - totalOrgNdegradation;  //immobilise some TAN
                        theManure.SetTAN(theManure.GetTAN() - immobilisedTAN);
                    }
                }
                NlabileOrgOutStore = NlabileOrgInstore - (NRunOffLabileOrg + totalOrgNdegradation);
                NhumicOutstore = NhumicInstore - NRunoffHum + newNHUM;
                newTAN = totalOrgNdegradation - newNHUM;
                break;
            case 3:
                string message1 = "Out of date code in Manurestore.cs for " + name;
            GlobalVars.Instance.Error(message1);
                //if (CdegradationRate > 0)
                //{
                //    NDegOrgOut = NlabileOrgInstore * Math.Exp(-(CdegradationRate + ohmOrg) * tstore * GlobalVars.avgNumberOfDays);
                //    NRunOffOrg = (ohmOrg / (ohmOrg + CdegradationRate)) * NlabileOrgInstore * (1 - Math.Pow(Math.E, -(ohmOrg + CdegradationRate * tstore * GlobalVars.avgNumberOfDays))); //1.59
                //    //disable until can get workng properly
                //    /*    NHUM = (GlobalVars.Instance.getHumification_const() / GlobalVars.Instance.getCNhum()) * CdegradationRate
                //            * (Math.Pow(Math.E, -(ohmOrg + CdegradationRate) * tstore) - Math.Pow(Math.E, -ohmOrg * tstore * GlobalVars.avgNumberOfDays));
                //        NRunoffHum = (CdegradationRate / (ohmOrg + CdegradationRate));*/
                //}
                //else
                //{
                //    NDegOrgOut = NlabileOrgInstore * Math.Exp(-ohmOrg * tstore * GlobalVars.avgNumberOfDays);
                //    NRunOffOrg = NlabileOrgInstore - NDegOrgOut;
                //    NRunoffHum = 0;
                //}
                //NorgOutStore = NDegOrgOut + NHUM;
                break;
        }

        switch (GlobalVars.Instance.getcurrentInventorySystem())
        {
            case 1: 
                double Nexcetion = theHousing.gettheLivestock().GetExcretedN();
                totalNstoreN20 = EFStoreN20 * (NTanInstore + NlabileOrgInstore); //1.64 - not quite..
                totalNstoreN2 = Lambda * totalNstoreN20;//1.66
                totalNstoreNH3 = EFStoreNH3 * (NTanInstore + newTAN);//1.65 - not quite..
                NrunoffTan = NTanInstore * ohmTAN;
                NTanOutstore = NTanInstore + totalOrgNdegradation - (totalNstoreN20 + totalNstoreN2 + totalNstoreNH3 + NrunoffTan + newNHUM);
                break;
            case 2:
                totalNstoreN20 = EFStoreN20 * theManure.GetTotalN(); //1.64 - not quite..
                totalNstoreN2 = Lambda * totalNstoreN20;//1.66
                totalNstoreNH3 = EFStoreNH3 * theManure.GetTotalN();//1.65 - not quite..
                NrunoffTan = NTanInstore * ohmTAN;
                NTanOutstore = NTanInstore + totalOrgNdegradation - (totalNstoreN20 + totalNstoreN2 + totalNstoreNH3 + NrunoffTan + newNHUM);
                break;
            case 3:
                double EFStoreN2 = EFStoreN20 * Lambda;
                double CN = theManure.GetdegC() / NlabileOrgInstore;
                double StorageRefTemp = 0;
                double EFNH3ref = 0;
                double KHø = 1 - 1.69 + 1447.7 / (meanTemp + GlobalVars.absoluteTemp);
                double KHref = 1 - 1.69 + 1447.7 / (StorageRefTemp + GlobalVars.absoluteTemp);
                EFStoreNH3 = KHref / KHø * EFNH3ref; //1.67
                double EFsum = EFStoreNH3 + EFStoreN20 + EFStoreN2;

                NTanOutstore = ((CdegradationRate * (1 / CN - GlobalVars.Instance.getHumification_const() / GlobalVars.Instance.getCNhum())
                    * theManure.GetdegC()) / ((EFsum + ohmTAN) - (ohmOrg + CdegradationRate / CN)))
                    * Math.Pow(Math.E, -(ohmOrg + CdegradationRate / CN) * tstore);//1.63
                //NTanOutstore += NTanInstore - (theManure.GetdegC() * (1 / CN - tau / GlobalVars.Instance.getCNhum()) * theManure.GetOrgDegC()) / ((EFsum + ohmTAN) - (ohmOrg + theManure.GetdegC() / CN)) * Math.Pow(Math.E, -(EFsum + ohmOrg) * tstore);//1.63
                NTANLost = NlabileOrgInstore + NTanInstore - (NTanInstore + NrunoffTan + NTanOutstore);//1.68
                NrunoffTan = ohmTAN / (ohmTAN + EFStoreNH3 + EFStoreN20 + EFStoreN2) * NTANLost;
                totalNstoreNH3 = NTANLost * EFStoreNH3 / (ohmTAN + EFStoreN2 + EFStoreN20 + EFStoreNH3);
                totalNstoreN2 = NTANLost * EFStoreN2 / (ohmTAN + EFStoreN2 + EFStoreN20 + EFStoreNH3);
                totalNstoreN20 = NTANLost * EFStoreN20 / (ohmTAN + EFStoreN2 + EFStoreN20 + EFStoreNH3);
                break;
        }
        theManure.SethumicN(NhumicOutstore);
        theManure.SetlabileOrganicN(NlabileOrgOutStore);
        theManure.SetTAN(NTanOutstore);
    }
    //! Write details to file.
    public void Write()
    {
        if (GlobalVars.Instance.getRunFullModel())
            theHousing.Write();
        GlobalVars.Instance.writeStartTab("ManureStore");
        GlobalVars.Instance.writeInformationToFiles("name", "Name", "-", name, parens);
        GlobalVars.Instance.writeInformationToFiles("identity", "ID", "-", identity, parens);
        GlobalVars.Instance.writeInformationToFiles("Cinput", "C input", "kg", Cinput, parens);
        GlobalVars.Instance.writeInformationToFiles("CCH4ST", "CH4-C emitted", "kg", CCH4ST, parens);
        GlobalVars.Instance.writeInformationToFiles("CCO2ST", "CO2-C emitted", "kg", CCO2ST, parens);

        GlobalVars.Instance.writeInformationToFiles("Ninput", "N input", "kg", Ninput, parens);
        GlobalVars.Instance.writeInformationToFiles("NTanInstore", "TAN input to storage", "kg", NTanInstore, parens);
        GlobalVars.Instance.writeInformationToFiles("totalNstoreNH3", "NH3-N emitted", "kg", totalNstoreNH3, parens);
        GlobalVars.Instance.writeInformationToFiles("totalNstoreN2", "N2-N emitted", "kg", totalNstoreN2, parens);
        GlobalVars.Instance.writeInformationToFiles("totalNstoreN20", "N2O-N emitted", "kg", totalNstoreN20, parens);
        GlobalVars.Instance.writeInformationToFiles("NTANLost", "Total TAN lost", "kg", NTANLost, parens);
        GlobalVars.Instance.writeInformationToFiles("NDegOrgOut", "Degradable N ex storage", "kg", NDegOrgOut, parens);
        GlobalVars.Instance.writeInformationToFiles("newNHUM", "new Humic N created in manure storage", "kg", newNHUM, parens);
        GlobalVars.Instance.writeInformationToFiles("NlabileOrgOutStore", "Labile organic N ex storage", "kg", NlabileOrgOutStore, parens);
        GlobalVars.Instance.writeInformationToFiles("NhumicOutstore", "Humic organic N ex storage", "kg", NhumicOutstore, parens);
        GlobalVars.Instance.writeInformationToFiles("NRunoffHum", "Humic N in runoff", "kg", NRunoffHum, parens);
        GlobalVars.Instance.writeInformationToFiles("NrunoffTan", "TAN in runoff", "kg", NrunoffTan, parens);
        GlobalVars.Instance.writeInformationToFiles("NRunOffLabileOrg", "Labile organic N in runoff", "kg", NRunOffLabileOrg, parens);
        GlobalVars.Instance.writeInformationToFiles("NLost", "Total N lost", "kg", NLost, parens);
        //GlobalVars.Instance.writeInformationToFiles("Nout", "Total N ", "kg", Nout);
        //GlobalVars.Instance.writeInformationToFiles("Nbalance", "??", "??", Nbalance);
        
        theManure.Write("");
        GlobalVars.Instance.writeEndTab();
    }
    //! Check if the manure store C budget is closed.
    /*!
     \return always returns false (throws an error if budget does not close).
    */
    public bool CheckManureStoreCBalance()
    {
        bool retVal = false;
        double Cout = GetManureC() + biogasCO2C + biogasCH4C;
        double CLost = CCH4ST + CCO2ST + CRunOffOrg;
        double Cbalance = Cinput - (Cout + CLost);
        double diff = Cbalance / Cinput;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
                double errorPercent = 100 * diff;
                string message1 = "Error; Manure storage C balance error for " + name + " is more than the permitted margin\n";
                string message2 =message1+ "Percentage error = " + errorPercent.ToString("0.00") + "%";
                GlobalVars.Instance.Error(message2);
             
        }
        return retVal;
    }
    //! Check if the manure store N budget is closed.
    /*!
     \return always returns false (throws an error if budget does not close).
    */
    public bool CheckManureStoreNBalance()
    {
        bool retVal = false;
        Ninput = NTanInstore + NlabileOrgInstore + NhumicInstore;
        Nout = GetManureN();
        NLost = NRunOffLabileOrg + NRunoffHum + NrunoffTan+ totalNstoreN2+totalNstoreN20+totalNstoreNH3;
        Nbalance = Ninput - (Nout + NLost);
        double diff = Nbalance / Ninput;
        double tolerance = GlobalVars.Instance.getmaxToleratedError();
        if (Math.Abs(diff) > tolerance)
        {
            Write();
            GlobalVars.Instance.CloseOutputXML();

                double errorPercent = 100 * diff;
       
                string messageString= ("Error; Manure storage N balance error for " + name + " is more than the permitted margin\n");
                messageString = messageString+("Percentage error = " + errorPercent.ToString("0.00") + "%");
        
                GlobalVars.Instance.Error(messageString);
          
        }
        return retVal;
    }
}
