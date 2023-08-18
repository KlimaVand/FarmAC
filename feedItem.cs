using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
/*! A class that named feedItem */
public class feedItem
{
    //!Amount in kg dry matter
    double amount;
    //! Name of the feed
    string name;
    //!Unique code
    int feedCode;
    string path;

    int ID;
    //! Concentration of metabolisable energy (MJ/kg DM)
    double energy_conc;
    //! Concentration of ash (kg/kg DM)
    double ash_conc;
    //! Concentration of carbon (kg/kg DM)
    double C_conc;
    //! Concentration of nitrogen (kg/kg DM)
    double N_conc;
    //! Concentration of neutral detergent fibre (kg/kg DM)
    double NDF_conc;
    //! Concentration of fibre (lignin) (kg/kg DM)
    double fibre_conc;
    //! Concentration of fat (kg/kg DM)
    double fat_conc;
    //! Concentration of nitrate (kg/kg DM)
    double nitrate_conc;
    //! Dr matter digestibility (kg/kg)
    double DMdigestibility;
    //! True if grazed
    bool isGrazed;
    //! True if fed at pasture
    bool fedAtPasture;
    //! True if suitable as bedding material
    bool beddingMaterial;
    //! Proportion of feed item lost by degradation in storage
    double StoreProcessFactor;
    //! Set the feedCode. .
    /*!
      \param aFeedCode an integer value for feedCode.
    */
    public void setFeedCode(int aFeedCode) { feedCode = aFeedCode; }
    //! Get feedCode. 
    /*!
      \return the integer value for feedCode.
    */
    public int GetFeedCode(){return feedCode;}
    //! Set the amount. 
    /*!
      \param anamount the amount (kg dry matter) as a double.
    */
    public void Setamount(double anamount) { amount = anamount; }
    //! Set C concentration. 
    /*!
      \param aVal a double value for C concentration (kg/kg dry matter).
    */
    public void SetC_conc(double aVal) { C_conc = aVal; }
    //! Set N concentration. 
    /*!
      \param aVal a double value for N concentration (kg/kg dry matter).
    */
    public void SetN_conc(double aVal) { N_conc = aVal; }
    //! Set concentration of fibre. 
    /*!
      \param aVal a double value for fibre concentration (kg/kg dry matter).
    */
    public void Setfibre_conc(double aVal) { fibre_conc = aVal; }
    //! Set name of feed. 
    /*!
      \param aVal a string containing the name of the feed.
    */
    public void Setname(string aString) { name = aString; }
    //! Set isGrazed.
    /*!
      \param aVal a bool value set true if the feed is grazed.
    */
    public void SetisGrazed(bool aVal) { isGrazed = aVal; }
    //! Set fedAtPasture. 
    /*!
      \param aVal a bool value set true if the feed is fed at pasture.
    */
    public void SetfedAtPasture(bool aVal) { fedAtPasture = aVal; }
    //! Get the amount. 
    /*!
      \return the amount (kg) as a double
    */
    public double Getamount() { return amount; }
    //! Get the name. 
    /*!
      \return a string value for the name.
    */
    public string GetName() { return name; }
    //! Get energy concentration.
    /*!
      \return a double value for energy concentration (MJ/kg dry matter).
    */
    public double Getenergy_conc() { return energy_conc; }
    //! Get the ash concentration.
    /*!
      \return a double value for the concentration of ash (kg/kg dry matter).
    */
    public double Getash_conc() { return ash_conc; }
    //! Get the C concentration.
    /*!
      \return a double value for the concentration of C (kg/kg dry matter).
    */
    public double GetC_conc() { return C_conc; }
    //! Get the N concentration.
    /*!
      \return a double value for the concentration of N (kg/kg dry matter).
    */
    public double GetN_conc() { return N_conc; }
    //! Get the nitrate concentration.
    /*!
      \return a double value for the concentration of nitrate (kg/kg dry matter).
    */
    public double GetNitrate_conc() { return nitrate_conc; }
    //! Get the NDF concentration.
    /*!
      \return a double value for the concentration of NDF (kg/kg dry matter).
    */
    public double GetNDF_conc() { return NDF_conc; }
    //!  Get fibre_conc.
    /*!
      \return a double value for fibre_conc.
    */
    public double Getfibre_conc() { return fibre_conc; }
    //! Get the fat concentration.
    /*!
      \return a double value for the concentration of fat (kg/kg dry matter).
    */
    public double Getfat_conc() { return fat_conc; }
    //! Get the DM digestibility. 
    /*!
      \return a double value for dry matter digestibility (kg/kg).
    */
    public double GetDMdigestibility() { return DMdigestibility; }
    //! Get the proportion of feed lost during storage. 
    /*!
      \return a double value for the proportion of feed lost during storage.
    */
    public double GetStoreProcessFactor() { return StoreProcessFactor; }
    //! Get whether the feed is grazed.
    /*!
      \return true if it is grazed.
    */
    public bool GetisGrazed() { return isGrazed; }
    //! Get whether the feed is fed at pasture.
    /*!
      \return true if the feed is fed at pasture.
    */
    public bool GetfedAtPasture() { return fedAtPasture; }
    //! Get whether the item can be used as bedding material.
    /*!
      \return true if the item can be used for bedding material.
    */
    public bool GetbeddingMaterial() { return beddingMaterial; }
    //! Add an amount. 
    /*!
      \param aVal a double value for the amount of dry matter to be added (kg).
    */
    public void Addamount(double aVal){amount += aVal;}
    //! Units used for amount
    private string Unit;
    string parens; /*!< a string containing information about the farm and scenario number.*/
    //! A default constructor without argument.
    public feedItem()
    {
        name = "None";
        path = "None";
        amount = 0;
        feedCode=0;
        energy_conc=0;
        ash_conc=0;
        C_conc=0;
        N_conc=0;
        NDF_conc=0;
        fibre_conc=0;
        fat_conc=0;
        nitrate_conc = 0;
        DMdigestibility=0;
        isGrazed=false;
        beddingMaterial=false;
        StoreProcessFactor=0;
        Unit = "kg/day";
    }
    //! A constructor with four arguments.
    /*!
      \param feeditemPath, a string argument that points to a path.
      \param id, an integer argument
      \param getamount, a bool argument that is set true if the amount of dry matter must be read from file
      \param aparens, a string argument
    */
    public feedItem(string feeditemPath, int id, bool getamount, string aparens)
    {
        parens = aparens;
        ID = id;
        path = feeditemPath + '(' + id + ')';
        FileInformation feedFile = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        feedFile.setPath(path); 
        if (getamount == true)
        {
            amount = feedFile.getItemDouble("Amount");
            Unit = feedFile.getItemString("Unit");
        }
        name = feedFile.getItemString("Name");
        feedCode = feedFile.getItemInt("FeedCode");
        if ((feedCode > 1000) && (feedCode < 2000)) //some feed codes have 1000 or 2000 added
        {
            feedCode -= 1000;
        }
        string aString = feedFile.getItemString("Grazed");
        if (aString == "true")
            isGrazed = true;
        else
            isGrazed = false;

        //need a more elegant method of detecting incorporation
        bool checkIncorp = name.Contains("incorporated");
        if (name.Contains("incorporated"))
            feedCode-=2000;
        GetStandardFeedItem(feedCode);
        if (checkIncorp)//get standard feeditem has renamed the product, so add incorporated again, so it will be recognised later
            name += " incorporated";
    }

    //! A copy constructor with one argument.
    /*!
      \param afeedItem, a class instance that points to class feedItem.
    */
    public feedItem(feedItem afeedItem)
        {
            energy_conc = afeedItem.energy_conc ;
            ash_conc = afeedItem.ash_conc;
            C_conc = afeedItem.C_conc;
            N_conc = afeedItem.N_conc;
            NDF_conc = afeedItem.NDF_conc;
            fibre_conc = afeedItem.fibre_conc;
            fat_conc = afeedItem.fat_conc;
            DMdigestibility = afeedItem.DMdigestibility;
            amount = afeedItem.amount;
            name = afeedItem.name;
            isGrazed = afeedItem.isGrazed;
            feedCode = afeedItem.feedCode;
            StoreProcessFactor = afeedItem.StoreProcessFactor;
            fedAtPasture = afeedItem.fedAtPasture;
        }
    //! Get a feedItem with a default composition.
    /*!
      \param targetFeedCode, the feed code of the target feed, as an integer value.
    */
    public void GetStandardFeedItem(int targetFeedCode)
    {
        FileInformation file=new FileInformation(GlobalVars.Instance.getfeeditemFilePath());
        file.setPath("feedItem");
        int min = 99; int max = 0;
        file.getSectionNumber(ref min, ref max);
        bool found = false;
        for(int i=min;i<=max;i++)
        {
            file.setPath("feedItem");
            if (file.doesIDExist(i))
            {
                string coreString = "feedItem(" + i.ToString() + ")";
                file.setPath(coreString);
                int StandardFeedCode = file.getItemInt("FeedCode");
                if (StandardFeedCode == targetFeedCode)
                {
                    found = true;
                    feedCode = targetFeedCode;
                    name = file.getItemString("Name");
                    file.setPath(coreString+".Fibre_concentration(-1)");
                    fibre_conc = file.getItemDouble("Value");
                    file.setPath(coreString+".NDF_concentration(-1)");
                    NDF_conc = file.getItemDouble("Value");
                    if (NDF_conc==-1)
                    {
                        string messageString = ("could not find NDF for feeditem ");
                        messageString += feedCode.ToString() + " name = " + name + "\n";
                        GlobalVars.Instance.Error(messageString);
                    }
                    file.setPath(coreString+".CrudeProtein_concentration(-1)");
                    double CrudeProtein_concentration = file.getItemDouble("Value");
                    SetN_conc(CrudeProtein_concentration / GlobalVars.NtoCrudeProtein);
                    file.setPath(coreString+".Fat_concentration(-1)");
                    fat_conc = file.getItemDouble("Value");
       
                    file.setPath(coreString+".Energy_concentration(-1)");
                    energy_conc = file.getItemDouble("Value"); 
                    energy_conc *= GlobalVars.Instance.GetdigestEnergyToME();

                    file.setPath(coreString+".Ash_concentration(-1)");
                    ash_conc = file.getItemDouble("Value");
                    SetC_conc((1.0 - Getash_conc()) * 0.46);
                    if (energy_conc == 0)
                        SetC_conc(0);
                    file.setPath(coreString+".Nitrate_concentration(-1)");
                    nitrate_conc = file.getItemDouble("Value");
     
                    file.setPath(coreString+".DMDigestibility(-1)");
                    DMdigestibility = file.getItemDouble("Value");
     
                    file.setPath(coreString+".Bedding_material(-1)");
                    beddingMaterial =file.getItemBool("Value");

                    file.setPath(coreString+".processStorageLoss(-1)");
                    StoreProcessFactor = file.getItemDouble("Value");
                    break;
                }
            }
        }
        if (found == false)
        {
            string messageString=("could not find feeditem ");
            messageString+=feedCode.ToString() + " name = " + name +  "\n";
            GlobalVars.Instance.Error(messageString);
        }
 
}
    //! Add two FeedItems. .
    /*!
      \param afeedItem, an instance that points to a donor feedItem.
      \param pooling, a bool value that is true if items should be pooled
      \param isBedding, true if the item can be used as bedding (default value false)
      \return an integer value
    */
    public int AddFeedItem(feedItem afeedItem, bool pooling, bool isBedding=false)
    {
        if ((afeedItem.GetFeedCode() != feedCode) && (pooling != true))
        {
              string messageString=("Error; attempt to combine two different feed items");
               GlobalVars.Instance.Error(messageString);
        }
        double donorAmount = afeedItem.Getamount();
        name = afeedItem.GetName();
        feedCode = afeedItem.GetFeedCode();
        if (donorAmount != 0)
        {
            energy_conc = (energy_conc * amount + donorAmount * afeedItem.Getenergy_conc()) / (amount + donorAmount);
            ash_conc = (ash_conc * amount + donorAmount * afeedItem.Getash_conc()) / (amount + donorAmount);
            C_conc = (C_conc * amount + donorAmount * afeedItem.GetC_conc()) / (amount + donorAmount);
            N_conc = (N_conc * amount + donorAmount * afeedItem.GetN_conc()) / (amount + donorAmount);
            NDF_conc = (NDF_conc * amount + donorAmount * afeedItem.GetNDF_conc()) / (amount + donorAmount);
            fibre_conc = (fibre_conc * amount + donorAmount * afeedItem.Getfibre_conc()) / (amount + donorAmount);
            fat_conc = (fat_conc * amount + donorAmount * afeedItem.Getfat_conc()) / (amount + donorAmount);
            DMdigestibility = (DMdigestibility * amount + donorAmount * afeedItem.GetDMdigestibility()) / (amount + donorAmount);
            amount += donorAmount;
            if (isBedding)
                beddingMaterial = true;
        }
        return 0;
    }
    //! Substract an amount from a feedItem. 
    /*!
      \param afeedItem, an instance that points to a donor feedItem.
      \param pooling, a bool value
      \return an integer value
    */
    public int SubtractFeedItem(feedItem afeedItem, bool pooling)
    {
        if ((afeedItem.GetFeedCode() != feedCode) && (pooling != true))
        {
            string messageString=("Error; attempt to subtract two different feed items");
            GlobalVars.Instance.Error(messageString);
        }
        double donorAmount = afeedItem.Getamount();
        amount -= donorAmount;
        return 0;
    }
    //! Write details to file.
    /*!
      \param theparens, a string value.
    */
    public void Write(string theparens)
    {
        parens = theparens + "feedCode" + feedCode.ToString();
        GlobalVars.Instance.writeStartTab("FeedItem");
        GlobalVars.Instance.writeInformationToFiles("name", "Name", "-", name,parens);
        GlobalVars.Instance.writeInformationToFiles("amount", "Amount", "kg DM", amount, parens);
        GlobalVars.Instance.writeInformationToFiles("feedCode", "Feed code", "-", feedCode, parens);
        GlobalVars.Instance.writeInformationToFiles("ash_conc", "Ash concentration", "kg/kg DM", ash_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("C_conc", "C concentration", "kg/kg DM", C_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("N_conc", "N concentration", "kg/kg DM", N_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("NDF_conc", "NDF concentration", "kg/kg DM", NDF_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("fibre_conc", "Fibre concentration", "kg/kg DM", fibre_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("fat_conc", "Fat concentration", "kg/kg DM", fat_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("energy_conc", "Energy concentration", "ME/kg DM", energy_conc, parens);
        GlobalVars.Instance.writeInformationToFiles("DMdigestibility", "DM digestibility", "kg/kg DM", DMdigestibility, parens);
        GlobalVars.Instance.writeInformationToFiles("isGrazed", "Fed at pasture", "-", isGrazed, parens);
        GlobalVars.Instance.writeInformationToFiles("beddingMaterial", "Can be used for bedding", "-", beddingMaterial, parens);
        GlobalVars.Instance.writeEndTab();

        
    }
    //! Multiply the amount of a feed item by a factor.
    /*!
      \param factor, a double that is multiplied with the amount.
    */
    public void AdjustAmount(double factor)
    {
        amount *= factor;
    }
    //! Calculate the potential methane production (Bo). 
    /*!
      \return the potential methane production (Bo) as a double value.
    */
    public double GetBo()
    {
        double NDF = (144.5 - 1.54 * DMdigestibility * 100)/10; //NDF as percentage of DM
        double CelAndHemi = NDF - fibre_conc * 100;
        if (CelAndHemi < 0)
            CelAndHemi = 0;
        double Bo = 19.05 * N_conc * 6.25 * 100 + 27.73 * fat_conc * 100 + 1.75 * CelAndHemi;
        Bo /= 1000;
        return Bo;
    }
    //! Get whether the feed is a concentrate.
    /*!
      \return a true if the feed is a concentrate.
    */
    public bool isConcentrate()
    {
        bool ret_val = false;
        if ((GetFeedCode() < 348) || ((GetFeedCode() >= 908) && GetFeedCode() <= 944))
            ret_val = true;
        return ret_val;
    }
}

