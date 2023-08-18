using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
/*! A class named ctool2 */
/*!
 Based on CTool model, expanded to include N
*/
public class SoilClayer
{
    //! Proportion of decomposed FOM C that is partitioned to HUM
    double humification = 0.0;
    double Rfraction =0.0;
    //! Clay fraction in the soil
    double Clayfraction = 0;
    //! fomc = fresh organic matter C, kg/ha, in layer 0 (upper) and 1 (lower)
    double fomc;
    //! humc = humic organic matter C, kg/ha, in layer 0 (upper) and 1 (lower)
    double humc;
    //! romc = resistant organic matter C, kg/ha, in layer 0 (upper) and 1 (lower)
    double romc;
    //! biocharc = biochar organic matter C, kg/ha, in layer 0 (upper) and 1 (lower)
    double biocharc;

    int layerNo = 0;

    public void setLayerNo(int aVal) { layerNo = aVal; }
    //! A private member to calculate R (effect of clay on humification)
    /*!
      \param clay fraction of the soil as a double argument
      \return a double value for R.
    */
    private double R(double Clayfraction)
    {
        return 1.67 * (1.85 + 1.6 * Math.Exp(-7.86 * Clayfraction));
    }

    public void setFOM(double aVal)
    {
        fomc = aVal;
    }
    public void setHUM(double aVal)
    {
        humc = aVal;
    }
    public void setROM(double aVal)
    {
        romc = aVal;
    }
    public void setBiochar(double aVal)
    {
        biocharc = aVal;
    }
    public void addFOM(double aVal)
    {
        fomc += aVal;
    }
    public void addHUM(double aVal)
    {
        humc += aVal;
    }
    public void addBiochar(double aVal)
    {
        biocharc += aVal;
    }
    public double getFOM()
    {
        return fomc;
    }
    public double getHUM()
    {
        return humc;
    }
    public double getROM()
    {
        return romc;
    }
    public double getBiochar()
    {
        return biocharc;
    }

    public SoilClayer(int theLayerNo, double theFOMdecompositionrate, double theHUMdecompositionrate, double theROMdecompositionrate, double thetF, double theROMificationfraction,
        double thefCO2, double theClayfraction)
    {
        layerNo = theLayerNo;
        Clayfraction = theClayfraction;
        Rfraction = R(Clayfraction);
        humification = 1 / (Rfraction + 1);
        fomc = 0.0;
        humc = 0.0;
        romc = 0.0;
        biocharc = 0.0;
    }
    public SoilClayer(SoilClayer theLayerToCopy)
    {
        Clayfraction = theLayerToCopy.Clayfraction;
        fomc = theLayerToCopy.fomc;
        humc = theLayerToCopy.humc;
        romc = theLayerToCopy.romc;
        biocharc = theLayerToCopy.biocharc;
        layerNo = theLayerToCopy.layerNo;
        Rfraction = R(Clayfraction);
        humification = 1 / (Rfraction + 1);
    }

    public void CopySoilClayer(SoilClayer theLayerToCopy)
    {
        Clayfraction = theLayerToCopy.Clayfraction;
        fomc = theLayerToCopy.fomc;
        humc = theLayerToCopy.humc;
        romc = theLayerToCopy.romc;
        biocharc = theLayerToCopy.biocharc;
        layerNo = theLayerToCopy.layerNo;
        Rfraction = R(Clayfraction);
        humification = 1 / (Rfraction + 1);
    }

    public double GetCstored()    { return fomc + humc + romc + biocharc; }

    public void layerDynamics(double timeStep, int layerNo, double FOMdecompositionrate, double HUMdecompositionrate, double ROMdecompositionrate,
        double tF, double fCO2, double ROMificationfraction,
        double tempCofficent, double droughtCoefficient, double FOMtransportIn,
        ref double FOMtransportOut, ref double FOMCO2, double HUMtransportIn, ref double HUMtransportOut, ref double HUMCO2,
        double ROMtransportIn, ref double ROMtransportOut, ref double ROMCO2,
        double BiochartransportIn, ref double BiochartransportOut, ref double BiocharCO2,
        ref double newHUM, ref double newROM)
    {
        double CO2 = 0;
        double fomcStart = fomc;
        double humcStart = humc;
        double romcStart = romc;
        double biocharcStart = biocharc;
        bool zeroRatesForDebugging = false; //use true to help when debugging
        //tF = 0;
        double temporaryCoefficient = tempCofficent * (1 - droughtCoefficient);
        if (zeroRatesForDebugging)
        {
            FOMdecompositionrate = 0;
            tF = 0.0;
        }
        //do FOM decomposition
        double initialFOM = fomc;
        double FomAfterDecom = rk4decay(fomc, timeStep, FOMdecompositionrate, temporaryCoefficient);
        double FOMdecomposition = fomc - FomAfterDecom;

        newHUM = FOMdecomposition * humification;
        FOMCO2 = FOMdecomposition * (1 - humification);
        CO2 += FOMCO2;
        if (layerNo == 0)
            FOMtransportOut = FomAfterDecom * tF;
        else
            FOMtransportOut = 0.0;
        fomc = FomAfterDecom + FOMtransportIn - FOMtransportOut;
        double test = (initialFOM + FOMtransportIn) - (fomc + FOMtransportOut + newHUM + FOMCO2);

        //do HUM
        if (zeroRatesForDebugging)
            HUMdecompositionrate = 0;
        double InitialHUM = humc;
        double HumAfterDecom = rk4decay(humc, timeStep, HUMdecompositionrate, temporaryCoefficient);

        double HUMdecomposition = humc - HumAfterDecom;
        newROM = ROMificationfraction * HUMdecomposition;
        HUMCO2 = fCO2 * HUMdecomposition;
        HUMtransportOut = HUMdecomposition * (1 - fCO2 - ROMificationfraction);
        CO2 += HUMCO2;
        double test2 = 0.0;
        if (layerNo == 0)
        {
            humc = HumAfterDecom + newHUM + HUMtransportIn;  //Note; HUMtransportOut is calculated from the decomposed FOM, so is accounted for in the value of HumAfterDecom
            test2 = (InitialHUM + HUMtransportIn + newHUM) - (humc + newROM + HUMCO2 + HUMtransportOut);
        }
        else
        {
            humc = HumAfterDecom + newHUM + HUMtransportIn + HUMtransportOut; //HUMtransportOut is recycled in the lower layer
            HUMtransportOut = 0;
            test2 = (InitialHUM + HUMtransportIn + newHUM) - (humc + newROM + HUMCO2);
        }

        //do ROM
        if (zeroRatesForDebugging)
            ROMdecompositionrate = 0;
        double InitialROM = romc;
        double RomAfterDecom = rk4decay(romc, timeStep, ROMdecompositionrate, temporaryCoefficient);
        double ROMdecomposition = romc - RomAfterDecom;
        ROMCO2 = fCO2 * ROMdecomposition;
        ROMtransportOut = ROMdecomposition * (1 - fCO2);
        double test3 = 0.0;
        if (layerNo == 0)
        {
            romc = RomAfterDecom + newROM + ROMtransportIn; //Note; ROMtransportOut is calculated from the decomposed HUM, so is accounted for in the value of RomAfterDecom
            test3 = (InitialROM + ROMtransportIn + newROM) - (romc + ROMCO2 + ROMtransportOut);
        }
        else
        {
            romc = RomAfterDecom + newROM + ROMtransportIn + ROMtransportOut;
            test3 = (InitialROM + ROMtransportIn + newROM) - (romc + ROMCO2);
            ROMtransportOut = 0;
        }

        //use ROM decomposition rate for biochar
        double InitialBiochar = biocharc;
        double BiocharAfterDecom = rk4decay(biocharc, timeStep, ROMdecompositionrate, temporaryCoefficient);

        double Biochardecomposition = biocharc - BiocharAfterDecom;
        BiocharCO2 = fCO2 * Biochardecomposition;
        BiochartransportOut = Biochardecomposition * (1 - fCO2);
        double test4 = 0.0;
        if (layerNo == 0)
        {
            biocharc = BiocharAfterDecom + BiochartransportIn; //Note; BiochartransportOut is calculated from the decomposed Biochar, so is accounted for in the value of BiocharAfterDecom
            test4 = (InitialBiochar + BiochartransportIn) - (biocharc + BiocharCO2 + BiochartransportOut);
        }
        else
        {
            biocharc = BiocharAfterDecom + BiochartransportIn; //Note; BiochartransportOut is calculated from the decomposed Biochar, so is accounted for in the value of BiocharAfterDecom
            test4 = (InitialBiochar + BiochartransportIn) - (biocharc + BiocharCO2);
            BiochartransportOut = 0;
        }

        double balance1 = fomcStart + FOMtransportIn - (fomc + FOMCO2 + FOMtransportOut + newHUM);
        double balance2 = humcStart + HUMtransportIn + newHUM - (humc + HUMCO2 + HUMtransportOut + newROM);
        double balance3 = romcStart + ROMtransportIn + newROM - (romc + ROMCO2 + ROMtransportOut);
        double balance4 = biocharcStart + BiochartransportIn - (biocharc + BiocharCO2 + BiochartransportOut);
    }
    private double func(double amount, double coeff)
    {
        return amount * -coeff;
    }
    //! Integrate decomposition using the rk4decay digital integration function.
    /*!
      \param u0 amount of C in the pool at the start of the time step as a double argument
      \param dt time step (days) as a double argument
      \param k decomposition rate (per day) a double argument
      \param temporaryCoefficient adjustment to decomposition rate as a double argument
      \return the amount of C pool at the end of the time step a double.
    */
    private double rk4decay(double u0, double dt, double k, double temporaryCoefficient)
    {
        double coeff = k * temporaryCoefficient;
        double f1 = func(u0, coeff);
        double f2 = func(u0 + dt * f1 / 2, coeff);
        double f3 = func(u0 + dt * f2 / 2, coeff);
        double f4 = func(u0 + dt * f3, coeff);
        double retVal = u0 + dt * (f1 + 2.0 * f2 + 2.0 * f3 + f4) / 6.0;
        return retVal;
    }

}
