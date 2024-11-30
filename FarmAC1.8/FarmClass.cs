using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

/*! A class that owns and controls all farm components */
public class FarmClass
{
    //! List of crop sequences on the farm
	List<CropSequenceClass> rotationList;
    //! List of livestock on the farm
    List<livestock> listOfLivestock;
    //! List of manure stores on the farm
    List<manureStore> listOfManurestores;
    //! List of livestock housing on the farm
    List<housing> listOfHousing;
    //! The farm nutrient balances
    farmBalanceClass theBalances;
    //!Farm area (ha)
    double farmArea;
    int FarmType;
    //! Farm number allocated from xml file name
    int FarmNo;
    //! Scenario number allocated from xml file name
    int ScenarioNo;
    //!  FarmClass constructor.
    public FarmClass()
	{
        listOfManurestores = new List<manureStore>();
        listOfHousing = new List<housing>();
        listOfLivestock = new List<livestock>();
        rotationList = new List<CropSequenceClass>();
    }
    //!  Get list of all crop sequences on the farm.
    /*!
      \return a list of CropSequenceClass
    */

    public List<CropSequenceClass> GetRotationList() { return rotationList; }
    //! Set the farm type
    public void SetFarmType(int aType) { FarmType = aType; }
    public void SetFarmNo(int aNum) { FarmNo = aNum; }
    public void SetScenarioNo(int aNum) { ScenarioNo = aNum; }
    //!  Create the list of crop sequences on the farm.
    /*!
    \param farmInformation points to the FileInformation class used for reading data
    \param newPath string containing the path for the input files
    \param zoneNr the climate zone number as an integer
    \param farmNr farm number as an integer
    \param ScenarioNr scenario number as an integer
    \param FarmTyp farm type as an integer
    \param soilTypeCount number of soil types available as an integer
    */
    public void SetupRotation(FileInformation farmInformation, string newPath, int zoneNr, int farmNr, int ScenarioNr, int FarmTyp,
        int soilTypeCount)
    {
        double areaWeightedDuration = 0.0;
        int minRotation = 99, maxRotation = 0;
        string RotationPath = newPath + "(" + ScenarioNr.ToString() + ").Rotation";
        farmInformation.setPath(RotationPath);
        farmInformation.getSectionNumber(ref minRotation, ref maxRotation);
        //read data for each crop sequence
        for (int rotationID = minRotation; rotationID <= maxRotation; rotationID++)
        {
            if (farmInformation.doesIDExist(rotationID))
            {
                CropSequenceClass anExample = new CropSequenceClass(RotationPath, rotationID, zoneNr, FarmTyp,
                    "farmnr" + farmNr.ToString() + "_ScenarioNr" + ScenarioNr.ToString() + "_CropSequenceClass" +
                    rotationID.ToString(), soilTypeCount);
                areaWeightedDuration += anExample.getArea() * anExample.GetlengthOfSequence();
                anExample.calcGrazedFeedItems();
                farmArea += anExample.getArea();
                if (GlobalVars.Instance.GetlockSoilTypes())
                    soilTypeCount++;
                else
                    anExample.SetsoilTypeCount(anExample.GetsoiltypeNo());
                rotationList.Add(anExample);
            }
        }
        //execute this if the soil carbon pool data needs to be read from file (i.e. not the baseline scenario)
        //this code checks to make sure that the field areas have not been changed between the Baseline and mitigation scenarios
        if (GlobalVars.Instance.reuseCtoolData != -1)
        {
            double[] oldArea = new double[20];
            double[] newArea = new double[20];
            for (int i = 0; i < 20; i++)
            {
                oldArea[i] = 0;
                newArea[i] = 0;
            }
            for (int i = 0; i < rotationList.Count; i++)
            {
                int soilNr = 0;
                if (GlobalVars.Instance.GetlockSoilTypes())
                    soilNr = rotationList[i].GetsoilTypeCount();
                else
                    soilNr = rotationList[i].GetsoiltypeNo();
                newArea[soilNr] += rotationList[i].getArea();
            }
            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(GlobalVars.Instance.getReadHandOverData());
            }
            catch
            {
                GlobalVars.Instance.Error("could not find CTool handover data " + GlobalVars.Instance.getReadHandOverData());
            }

            for (int j = 1; j < lines.Length; j++)
            {
                string[] tmp = lines[j].Split('\t');

                int soilNr = Convert.ToInt32(tmp[0]);
                oldArea[soilNr] += Convert.ToDouble(tmp[11]);
            }
            for (int j = 1; j < 20; j++)
            {
                if (oldArea[j] != newArea[j])
                {
                    GlobalVars.Instance.Error("area for soil type " + j.ToString() + " in scenario does not match area in Baseline scenario");
                }
            }

        }
        if (farmArea > 0)
            areaWeightedDuration /= farmArea;
        else
            areaWeightedDuration = 1;
        GlobalVars.Instance.theZoneData.SetaverageYearsToSimulate(areaWeightedDuration);
        ///calculate composition of bedding material
        GlobalVars.Instance.CalcbeddingMaterial(rotationList);
    }
    //!  Create the list of livestock and manure management facilities on the farm.
    /*!
    \param farmInformation points to the FileInformation class used for reading data
    \param zoneNr the climate zone number as an integer
    \param farmNr farm number as an integer
    \param newPath string containing the path for the input files
    \param ScenarioNr scenario number as an integer
    */
    public void SetupLivestockAndManure(FileInformation farmInformation, int zoneNr, int farmNr, string newPath, int ScenarioNr)
    {
        //start of livestock section
        string LivestockPath = newPath + "(" + ScenarioNr.ToString() + ").Livestock";
        farmInformation.setPath(LivestockPath);
        ///read the livestock details from file

        LoadLivestock(farmInformation, LivestockPath, zoneNr, farmNr, ScenarioNr);
        ///read details of any manure stores that do not receive manure from livestock on the farm
        string ManureStoragePath = newPath + "(" + ScenarioNr.ToString() + ").ManureStorage";
        farmInformation.setPath(ManureStoragePath);
        //
        int minManureStorage = 99, maxManureStorage = 0;

        farmInformation.getSectionNumber(ref minManureStorage, ref maxManureStorage);
        for (int ManureStorageID = minManureStorage; ManureStorageID <= maxManureStorage; ManureStorageID++)
        {
            if (farmInformation.doesIDExist(ManureStorageID))
            {
                manureStore amanurestore = new manureStore(ManureStoragePath, ManureStorageID, zoneNr, "farmnr" + farmNr.ToString() + "_ScenarioNr" + ScenarioNr.ToString() + "_manureStore" + ManureStorageID.ToString());
                listOfManurestores.Add(amanurestore);
            }
        }

        ///get details of animal housing (for each livestock category)
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimalCategory = listOfLivestock[i];
            for (int j = 0; j < anAnimalCategory.GethousingDetails().Count; j++)
            {
                int housingType = anAnimalCategory.GethousingDetails()[j].GetHousingType();
                double proportionOfTime = anAnimalCategory.GethousingDetails()[j].GetpropTime();
                housing aHouse = new housing(housingType, anAnimalCategory, j, zoneNr, "farmnr" + farmNr.ToString() + "_ScenarioNr" + ScenarioNr.ToString() + "_housingi" + i.ToString() + "_housingj" + j.ToString());
                listOfHousing.Add(aHouse);
                //storage for manure produced in housing is initiated in the housing module
                for (int k = 0; k < aHouse.GetmanurestoreDetails().Count; k++)
                {
                    manureStore aManureStore = aHouse.GetmanurestoreDetails()[k].GettheStore();
                    aManureStore.SettheHousing(aHouse);
                    listOfManurestores.Add(aManureStore);
                }
            }
        }
    }
    //!  Run the simulation for the farm.
    public void RunFarm()
    {
        //Run the livestock models
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimal = listOfLivestock[i];
            //Run if the livestock are ruminants
            if (anAnimal.GetisRuminant())
            {
                anAnimal.DoRuminant();
                if ((GlobalVars.Instance.getRunFullModel()) &&
                        ((anAnimal.GetpropDMgrazed() > 0) && (rotationList.Count == 0)))
                {
                    string messageString = ("Error - livestock are indicated as grazing but there are no fields to graze");
                    GlobalVars.Instance.Error(messageString);
                }
            }
            else if (anAnimal.GetspeciesGroup() == 2)  //run the pig model
            {
                anAnimal.DoPig();
            }
        }
        for (int i = 0; i < listOfHousing.Count; i++)  //simulate each of the animal houses
        {
            housing ahouse = listOfHousing[i];
            ahouse.DoHousing();
        }
//        GlobalVars.Instance.theManureExchange = new GlobalVars.theManureExchangeClass();
        for (int i = 0; i < listOfManurestores.Count; i++)  //simulate each of the manure stores
        {
            manureStore amanurestore2 = listOfManurestores[i];
            amanurestore2.DoManurestore();
        }
        DoRotations();
    }
    //! Create and calculate the nutrient balances for the farm.
    /*!
    \param farmInformation points to the FileInformation class used for reading data
    \param zoneNr the climate zone number as an integer
    \param farmNr farm number as an integer
    \param newPath string containing the path for the input files
    \param ScenarioNr scenario number as an integer
    */
    public void CreateFarmBalances()
    {
        theBalances = new farmBalanceClass("farmnr" + FarmNo.ToString() + "_ScenarioNr" + ScenarioNo.ToString() + "FarmBalance_1");
        theBalances.DoFarmBalances(rotationList, listOfLivestock, listOfHousing, listOfManurestores);
    }
    //!  Run the simulations for all crop sequences on the farm.
    public void DoRotations()
    {
        for (int rotationID = 0; rotationID < rotationList.Count; rotationID++)
        {
            rotationList[rotationID].CalcModelledYield();
            rotationList[rotationID].CheckYields();
            GlobalVars.Instance.log("Seq " + rotationID.ToString() + " DM " + rotationList[rotationID].GetDMYield().ToString(), 5);
            GlobalVars.Instance.log("Seq " + rotationID.ToString() + " C " + rotationList[rotationID].getCHarvested().ToString(), 5);
        }
        GlobalVars.Instance.CheckGrazingData();

    }

    public void LoadLivestock(FileInformation farmInformation, string LivestockPath, int zoneNr, int farmNr, int ScenarioNr)
    {
        int minLivestock = 99, maxLivestock = 0;
        farmInformation.getSectionNumber(ref minLivestock, ref maxLivestock);

        for (int LiveStockID = minLivestock; LiveStockID <= maxLivestock; LiveStockID++)
        {

            if (farmInformation.doesIDExist(LiveStockID))
            {
                livestock anAnimal = new livestock(LivestockPath, LiveStockID, zoneNr, "farmnr" + farmNr.ToString() + "_ScenarioNr" + ScenarioNr.ToString() + "_livestock" + LiveStockID.ToString());
                anAnimal.GetAllFeedItemsUsed();
                listOfLivestock.Add(anAnimal);
            }
        }

    }
    public void CalcLivestockProduction()
    {

    }
    //! Write the nutrient balances for the livestock and manure management.
    public void WriteLivestockAndManure()
    {
        for (int i = 0; i < listOfHousing.Count; i++)
        {
            listOfHousing[i].Write();
        }
        for (int i = 0; i < listOfManurestores.Count; i++)
        {
            manureStore amanurestore2 = listOfManurestores[i];
            amanurestore2.Write();
        }
        //GlobalVars.Instance.OpenLivestockFile();
        for (int i = 0; i < listOfLivestock.Count; i++)
        {
            livestock anAnimal = listOfLivestock[i];
            anAnimal.Write();
            anAnimal.WriteLivestockFile();
        }
        for (int rotationID = 0; rotationID < rotationList.Count; rotationID++)
            rotationList[rotationID].CalcManureForAllCrops();
        //GlobalVars.Instance.CloseLivestockFile();
        GlobalVars.Instance.Write(false);
    }
    //! Write the nutrient balances for the crop sequences.
    public void WriteRotation(string outputName)
    {
        if (GlobalVars.Instance.WriteField)
        {
            for (int i = 0; i < rotationList.Count; i++)
            {
                CropSequenceClass rotation = rotationList[i];
                rotation.Write();
            }
        }

        GlobalVars.Instance.CalcAllFeedAndProductsPotential(rotationList);
        GlobalVars.Instance.Write(true);
        GlobalVars.Instance.writeStartTab("ExpectedYield");
        for (int i = 0; i < rotationList.Count; i++)
            rotationList[i].processExpectedYieldForOutput("ExpectedYield0_CropSequenceClass" + i.ToString());

        if (GlobalVars.Instance.Writectoolxlm)
        {
            XmlWriter writerCtool;
            writerCtool = XmlWriter.Create(outputName + "CtoolFile.xml");
            XElement fileCtool = new XElement("file");
            for (int i = 0; i < rotationList.Count; i++)
            {
                XElement rotation = new XElement("rotation");
                rotation.Add(rotationList[i].node);
                fileCtool.Add(rotation);

            }
            fileCtool.Save(writerCtool);
            writerCtool.Close();
        }
        if (GlobalVars.Instance.reuseCtoolData == -1)
        {
            writeCtoolData(rotationList);
        }
    }

    //! Write the nutrient balances for the farm.
    public void WriteFarmBalances()
    {
        theBalances.WriteFarmBalances(rotationList, listOfLivestock);
    }

    //! Write the soil carbon data for each crop sequence on the farm to file.
    void writeCtoolData(List<CropSequenceClass> rotationList)
    {
        bool writeHeader = true;
        System.IO.StreamWriter extraCtoolData = new System.IO.StreamWriter(GlobalVars.Instance.getWriteHandOverData());
        for (int i = 0; i < rotationList.Count; i++)
        {
            CropSequenceClass rotation = rotationList[i];
            rotation.writeCtoolData(extraCtoolData, writeHeader);
            writeHeader = false;
        }
        extraCtoolData.Close();
    }
}

