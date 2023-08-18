using System;
using System.Xml;
/*! A class that named manure */
public class manure
{
    //! Path for the file(s) of input data
    string path;
    int id;
    //! Dry matter (kg)
    double DM;
    //! Non-degradable C (kg)
    double nonDegC;
    //! Degradable C (kg)
    double degC;
    //! humic C (kg)
    double humicC;
    //! labile organic N (kg)
    double labileOrganicN;
    //! Total ammoniacal nitrogen (kg)
    double TAN;
    //! humic N (kg)
    double humicN;
    //! Biochar C (kg)
    double biocharC;
    //! Potential methane production (m**3 CH4/kg VS)
    double Bo;
    //! Uniqe manure type ID
    int manureType;
    //! ID of species from which the manure originated
    int speciesGroup;
    //! Name of the manure
    string name;
    //! Is true if the manure is solid not liquid
    bool isSolid;
    string parens; /*!<! a string containing information about the farm and scenario number.*/ 
    //! Set amount of degradable C. 
    /*!
     \param aValue, mass of degradable C (kg) as double.
    */
    public void SetdegC(double aValue) { degC = aValue; }
    //! Set amount of non-degradable C. 
    /*!
     \param aValue, mass of non-degradable C (kg) as double.
    */
    public void SetnonDegC(double aValue) { nonDegC = aValue; }
    //! Set amount of humic C. 
    /*!
     \param aValue, mass of humic C (kg) as double.
    */
    public void SethumicC(double aValue) { humicC = aValue; }
    //! Set amount of TAN. 
    /*!
     \param aValue, mass of TAN (kg) as double.
    */
    public void SetTAN(double aValue) { TAN = aValue; }
    //! Set amount of labile organic N. 
    /*!
     \param aValue, mass of labile organic N (kg) as double.
    */
    public void SetlabileOrganicN(double aValue) { labileOrganicN = aValue; }
    //! Set species group. 
    /*!
     \param aValue, species group identifier as integer.
    */
    public void SetspeciesGroup(int aValue) { speciesGroup = aValue; }
    //! Set manureType.
    /*!
     \param aValue, manure type identifier as integer
    */
    public void SetmanureType(int aValue) { manureType = aValue; }
    //! Set isSolid. 
    /*!
     \param aValue, true if the manure is a solid.
    */
    public void SetisSolid(bool aValue) { isSolid = aValue; }
    //! Set thumicN. 
    /*!
     \param aVal, mass of humic N (kg) as a double.
    */
    public void SethumicN(double aVal) { humicN = aVal; }
    //! Set potential methane generation (Bo).
    /*!
     \param aVal, potential methane generation (Bo) (m**3 CH4/(kg VS)).
    */
    public void SetBo(double aVal) { Bo = aVal; }
    //! Set Name. 
    /*!
     \param aname, name of manure as string.
    */
    public void Setname(string aname) { name = aname; }
    //! Get the non-degradable C. 
    /*!
     \return mass of non-degradable C (kg) as a double.
    */
    public double GetnonDegC() { return nonDegC; }
    //! Get the humic C. 
    /*!
     \return mass of humic C (kg) as a double.
    */
    public double GethumicC() { return humicC; }
    //! Get the biochar C. 
    /*!
     \return mass of biochar C (kg) as a double.
    */
    public double GetBiocharC() { return biocharC; }
    //!  Get degC. returning one value.
    /*!
     \return a double value.
    */
    public double GetdegC() { return degC; }
    //! Get the mass of TAN. 
    /*!
     \return mass of TAN (kg) as a double.
    */
    public double GetTAN() { return TAN; }
    //! Get the mass of organic N. 
    /*!
     \return mass of organic N (kg) as a double.
    */
    public double GetorganicN() { return labileOrganicN + humicN; }
    //! Get the manure type.
    /*!
     \return the manure type as an integer value.
    */
    public int GetmanureType() { return manureType; }
    //! Get the species group.
    /*!
     \return the species group identifier an integer value.
    */
    public int GetspeciesGroup() { return speciesGroup; }
    //! Get isSolid. 
    /*!
     \return true if the manure is a solid.
    */
    public bool GetisSolid() { return isSolid; }
    //! Get Name. 
    /*!
     \return the name as a string value.
    */
    public string Getname() { return name; }
    //! Get the mass of humic N. 
    /*!
     \return mass of humic N (kg) as a double.
    */
    public double GethumicN() { return humicN; }
    //! Get the mass of labile organic N. 
    /*!
     \return mass of labile organic N (kg) as a double.
    */
    public double GetlabileOrganicN() { return labileOrganicN; }
    //! Get the total mass of N. 
    /*!
     \return total mass of N (kg) as a double.
    */
    public double GetTotalN() { return TAN + labileOrganicN + humicN; }
    //! Get potential methane production (Bo).
    /*!
     \return potential methane production (Bo) (m**3 CH4/(kg VS)) a double value.
    */
    public double GetBo() { return Bo; }
    //! Check to see if two manure instances are the same type.
    /*!
     \param aManure, pointer to an instance of manure class.
     \return true if the two manures are the same type.
    */
    public bool isSame(manure aManure)
    {
        if (manureType == aManure.manureType)
        {
            if ((speciesGroup == aManure.speciesGroup) || (aManure.speciesGroup == 0))
                return true;
            else
                return false;
        }
        else
        {
            if ((speciesGroup == aManure.speciesGroup) || (aManure.speciesGroup == 0))
            {
                if (manureType == 1 && aManure.manureType == 2)
                    return true;
                else if (manureType == 2 && aManure.manureType == 1)
                    return true;
                else if (manureType == 3 && aManure.manureType == 4)
                    return true;
                else if (manureType == 4 && aManure.manureType == 3)
                    return true;
                else if (manureType == 6 && aManure.manureType == 9)
                    return true;
                else if (manureType == 9 && aManure.manureType == 6)
                    return true;
                else if (manureType == 7 && aManure.manureType == 10)
                    return true;
                else if (manureType == 10 && aManure.manureType == 7)
                    return true;
                else if (manureType == 8 && aManure.manureType == 12)
                    return true;
                else if (manureType == 12 && aManure.manureType == 8)
                    return true;
                else if (manureType == 13 && aManure.manureType == 14)
                    return true;
                else if (manureType == 14 && aManure.manureType == 13)
                    return true;
                else
                    return false;
            }
            return false;
        }
           
    }
    //! A default constructor.
    public manure()
    {
        DM =0;
        nonDegC = 0;
        degC = 0;
        humicC = 0;
        biocharC = 0;
        labileOrganicN = 0;
        TAN = 0;
        humicN = 0;
        manureType = 0;
        speciesGroup = 0;
        Bo = 0;
        name = "";
    }
    //! A copy constructor.
    /*!
     \param manureToCopy, pointer to manure class instance to be copied.
    */
    public manure(manure manureToCopy)
    {
        DM = manureToCopy.DM;
        nonDegC = manureToCopy.nonDegC;
        degC = manureToCopy.degC;
        humicC = manureToCopy.humicC;
        biocharC = manureToCopy.biocharC;
        labileOrganicN = manureToCopy.labileOrganicN;
        TAN = manureToCopy.TAN;
        humicN = manureToCopy.humicN;
        Bo = manureToCopy.Bo;
        manureType = manureToCopy.manureType;
        speciesGroup = manureToCopy.speciesGroup;
        name = manureToCopy.name;
    }

    //! A constructor used to import manure, with amount determined by N required
    /*!
     \param aPath, path for file to manure characteristics as string argument.
     \param aID, one integer argument.
     \param amountN, amount of N required as a double argument.
     \param aparens, one string argument.
    */
    public manure(string aPath, int aID, double amountN, string aparens)
    {
        id=aID;
        path="AgroecologicalZone("+GlobalVars.Instance.GetZone().ToString()+")."+aPath+'('+id.ToString()+')';
        parens = aparens;
        FileInformation manureFile = new FileInformation(GlobalVars.Instance.getfertManFilePath());
        manureFile.setPath(path);
        name = manureFile.getItemString("Name");
        manureType = manureFile.getItemInt("ManureType");
        speciesGroup = manureFile.getItemInt("SpeciesGroup");
        path += ".TANconcentration(-1)";
        manureFile.setPath(path);
        double tempTAN = manureFile.getItemDouble("Value");
        manureFile.PathNames[2] = "organicNconcentration";
        double temporganicN = manureFile.getItemDouble("Value");
        double proportionTAN = tempTAN / (tempTAN + temporganicN);
        TAN = proportionTAN * amountN;
        labileOrganicN = (1 - proportionTAN) * amountN;
        double amount = amountN/(tempTAN+ temporganicN);
        manureFile.PathNames[2] = "degCconcentration";
        double degCconc=manureFile.getItemDouble("Value");
        degC = amount * degCconc;
        manureFile.PathNames[2] = "nonDegCconcentration";
        double nonDegCconc = manureFile.getItemDouble("Value");
        nonDegC = amount * nonDegCconc;
        manureFile.PathNames[2] = "humicCconcentration";
        double humicCconc = manureFile.getItemDouble("Value");
        manureFile.PathNames[2] = "biocharConcentration";
        double biocharCconc = manureFile.getItemDouble("Value",false);
        biocharC = 0.0;
        if (humicCconc>nonDegCconc)
        {
            string messageString = "Error; manure humic C is greater than total non-degradable C\n";
            messageString += "Manure name = " + name + "\n";
            GlobalVars.Instance.Error(messageString);
        }
        humicC = amount * humicCconc;
        if (humicC > 0)
        {
            humicN = humicC / GlobalVars.Instance.getCNhum();
            labileOrganicN -= humicN;
            if (labileOrganicN < 0)
            {
                string messageString = "Error; manure humic N is greater than total organic N\n";
                messageString += "Manure name = " + name + "\n";
                GlobalVars.Instance.Error(messageString);
            }
        }
        else
            humicN = 0;
        if (biocharCconc > 0.0)
        {
            biocharC = amount * biocharCconc;
        }
        manureFile.PathNames[2] = "Bo";
        Bo = manureFile.getItemDouble("Value",false);
    }
    //! Get the total C in the manure.
    /*!
     \return the total C  (kg) as a double value.
    */
    public double GetTotalC()
    {
        return degC + nonDegC + humicC;
    }
    //! Add one manure to another.
    /*!
     \param aManure, manure to be added as instance of manure class
    */
    public void AddManure(manure aManure)
    {
        //Bo is calculated as average of the two Bo values, weighted by total C
        double totalC = nonDegC + degC;
        double oldBo = Bo * totalC;
        double donorC = aManure.degC + aManure.nonDegC;
        double addedBo = aManure.Bo * donorC;
        Bo = (oldBo + addedBo) / (totalC + donorC);

        DM += aManure.DM;
        nonDegC += aManure.nonDegC;
        degC += aManure.degC;
        humicC += aManure.humicC;
        biocharC += aManure.biocharC;
        labileOrganicN += aManure.labileOrganicN;
        TAN += aManure.TAN;
        humicN += aManure.humicN;
    }
    //! Increase amount of manure by a factor.
    /*!
     \param factor, factor to use as one double argument.
    */
    public void IncreaseManure(double factor)
    {
        if (factor<0.0)
        {
            string messageString = name + " - negative factor not allowed in IncreaseManure function";
            GlobalVars.Instance.Error(messageString);
        }
        DM *= factor;
        nonDegC *= factor;
        degC *= factor;
        humicC *= factor;
        labileOrganicN *= factor;
        TAN *= factor;
        humicN *= factor;
    }
    //! Reduce the amount of manure by a factor.
    /*!
     \param proportion, one double argument.
    */
    public void DivideManure(double factor)
    {
        if (factor < 0.0)
        {
            string messageString = name + " - negative factor not allowed in DivideManure function";
            GlobalVars.Instance.Error(messageString);
        }
        DM *= factor;
        nonDegC *= factor;
        degC *= factor;
        humicC *= factor;
        labileOrganicN *= factor;
        TAN *= factor;
        humicN *= factor;
    }
    //! Remove an amount of manure. Amount is determined by the amount of N required.
    /*!
     \param amountN, amount of N required (kg) as a double argument.
     \param aManure, instance of manure class to which the manure should be added.
    */
    public void TakeManure(ref double amountN, ref manure aManure)
    {
        double totalN = GetTotalN();
        double proportion;
        if (amountN <= totalN)
            proportion = amountN / totalN;
        else
        {
            proportion=1.0;
            amountN = totalN;
        }
        aManure.DM=proportion*DM;
        DM -= aManure.DM;
            
        aManure.nonDegC = proportion * nonDegC;
        nonDegC-=aManure.nonDegC;
        aManure.degC = proportion * degC;
        degC-=aManure.degC;
        aManure.humicC = proportion * humicC;
        humicC-=aManure.humicC;
            
        aManure.labileOrganicN = proportion * labileOrganicN;
        labileOrganicN -= aManure.labileOrganicN;
        aManure.TAN = proportion * TAN;
        TAN -= aManure.TAN;
        aManure.humicN = proportion * humicN;
        humicN -= aManure.humicN;
        aManure.Setname(Getname());
    }
    //! Write details of manure to file.
    /*!
     \param addedInfo, one string argument.
    */
    public void Write(string addedInfo)
    {
        parens = "_" + addedInfo + "_" + name;
        GlobalVars.Instance.writeStartTab("manure");
        GlobalVars.Instance.writeInformationToFiles("name", "Name", "-", name, parens);
        GlobalVars.Instance.writeInformationToFiles("speciesGroup", "Species number", "-", speciesGroup, parens);
        GlobalVars.Instance.writeInformationToFiles("typeStored", "Storage type", "-", manureType, parens);
        GlobalVars.Instance.writeInformationToFiles("DM", "Dry matter", "kg", DM, parens);
        GlobalVars.Instance.writeInformationToFiles("nonDegC", "Non-degradable C", "kg", nonDegC, parens);
        GlobalVars.Instance.writeInformationToFiles("degC", "Degradable C", "kg", degC, parens);
        GlobalVars.Instance.writeInformationToFiles("humicC", "Humic C", "kg", humicC, parens);
        GlobalVars.Instance.writeInformationToFiles("TAN", "TAN", "kg", TAN, parens);
        GlobalVars.Instance.writeInformationToFiles("labileOrganicN", "Organic N", "kg", labileOrganicN, parens);
        GlobalVars.Instance.writeInformationToFiles("humicN", "humic N", "kg", humicN, parens);
        GlobalVars.Instance.writeEndTab();        
    }
}
