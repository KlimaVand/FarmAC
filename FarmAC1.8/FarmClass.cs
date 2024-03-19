using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class FarmClass
{
	List<CropSequenceClass> rotationList;
    List<livestock> listOfLivestock;
    List<manureStore> listOfManurestores;
    List<housing> listOfHousing;
    farmBalanceClass theBalances;
    double farmArea;
    int FarmType;
    int farmNo;
    int scenarioNo;
    public FarmClass()
	{
        listOfManurestores = new List<manureStore>();
        listOfHousing = new List<housing>();
        listOfLivestock = new List<livestock>();
        rotationList = new List<CropSequenceClass>();
    }
    public List<CropSequenceClass> GetRotationList() { return rotationList; }
    public void SetFarmType(int aType) { FarmType = aType; }
    public void SetFarmNo(int aNum) { farmNo = aNum; }
    public void SetScenarioNo(int aNum) { scenarioNo = aNum; }
    public double SetupRotation(FileInformation farmInformation, string newPath, int zoneNr, int farmNr, int ScenarioNr, int FarmTyp,
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

            for (int j = 0; j < lines.Length; j++)
            {
                string[] tmp = lines[j].Split('\t');

                int soilNr = Convert.ToInt32(tmp[0]);
                oldArea[soilNr] += Convert.ToDouble(tmp[11]);
            }
            for (int j = 0; j < 20; j++)
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
        return areaWeightedDuration;
    }
    public void SetupLivestockAndManure(FileInformation farmInformation, int zoneNr, int farmNr, string newPath, int ScenarioNr)
    {
        //start of livestock section
        string LivestockPath = newPath + "(" + ScenarioNr.ToString() + ").Livestock";
        farmInformation.setPath(LivestockPath);
        ///read the livestock details from file
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
        GlobalVars.Instance.theManureExchange = new GlobalVars.theManureExchangeClass();
        for (int i = 0; i < listOfManurestores.Count; i++)  //simulate each of the manure stores
        {
            manureStore amanurestore2 = listOfManurestores[i];
            amanurestore2.DoManurestore();
        }
        DoRotations();
    }
    public void WriteFarm(int farmNr,int ScenarioNr)
    {
        farmBalanceClass theBalances = new farmBalanceClass("farmnr" + farmNr.ToString() + "_ScenarioNr" + ScenarioNr.ToString() + "FarmBalance_1");
        theBalances.DoFarmBalances(rotationList, listOfLivestock, listOfHousing, listOfManurestores);
    }
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

      //  GlobalVars.Instance.CloseFieldFile();
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
    void writeCtoolData(List<CropSequenceClass> rotationList)
    {
        double[] rotarea = new double[20];
        double[] fomcLayer1 = new double[20];
        double[] fomcLayer2 = new double[20];
        double[] humcLayer1 = new double[20];
        double[] humcLayer2 = new double[20];
        double[] romcLayer1 = new double[20];
        double[] romcLayer2 = new double[20];
        double[] biocharcLayer1 = new double[20];
        double[] biocharcLayer2 = new double[20];
        double[] FOMn = new double[20];
        double[] rotresidualMineralN = new double[20];
        for (int soilNo = 0; soilNo < 20; soilNo++)
        {
            rotarea[soilNo] = 0;
            fomcLayer1[soilNo] = 0;
            fomcLayer2[soilNo] = 0;
            humcLayer1[soilNo] = 0;
            humcLayer2[soilNo] = 0;
            romcLayer1[soilNo] = 0;
            romcLayer2[soilNo] = 0;
            FOMn[soilNo] = 0;
            rotresidualMineralN[soilNo] = 0;
        }
        for (int i = 0; i < rotationList.Count; i++)
        {
            CropSequenceClass rotation = rotationList[i];
            int soiltypeNo = 0;
            if (GlobalVars.Instance.GetlockSoilTypes())
                soiltypeNo = rotation.GetsoilTypeCount();
            else
                soiltypeNo = rotation.GetsoiltypeNo();
            rotarea[soiltypeNo] += rotation.getArea();
            fomcLayer1[soiltypeNo] += rotation.aModel.GettheClayers()[0].getFOM() * rotation.getArea();
            fomcLayer2[soiltypeNo] += rotation.aModel.GettheClayers()[1].getFOM() * rotation.getArea();
            humcLayer1[soiltypeNo] += rotation.aModel.GettheClayers()[0].getHUM() * rotation.getArea();
            humcLayer2[soiltypeNo] += rotation.aModel.GettheClayers()[1].getHUM() * rotation.getArea();
            romcLayer1[soiltypeNo] += rotation.aModel.GettheClayers()[0].getROM() * rotation.getArea();
            romcLayer2[soiltypeNo] += rotation.aModel.GettheClayers()[1].getROM() * rotation.getArea();
            biocharcLayer1[soiltypeNo] += rotation.aModel.GettheClayers()[0].getBiochar() * rotation.getArea();
            biocharcLayer2[soiltypeNo] += rotation.aModel.GettheClayers()[1].getBiochar() * rotation.getArea();
            FOMn[soiltypeNo] += rotation.aModel.FOMn * rotation.getArea();
            rotresidualMineralN[soiltypeNo] += rotation.GetResidualSoilMineralN();
        }
        System.IO.StreamWriter extraCtoolData = new System.IO.StreamWriter(GlobalVars.Instance.getWriteHandOverData());
        for (int soilNo = 0; soilNo < 20; soilNo++)
        {
            if (rotarea[soilNo] > 0)
            {
                extraCtoolData.WriteLine(soilNo.ToString() + '\t' + (fomcLayer1[soilNo] / rotarea[soilNo]).ToString() + '\t' + (fomcLayer2[soilNo] / rotarea[soilNo]).ToString()
                    + '\t' + (humcLayer1[soilNo] / rotarea[soilNo]).ToString() + '\t' + (humcLayer2[soilNo] / rotarea[soilNo]).ToString()
                    + '\t' + (romcLayer1[soilNo] / rotarea[soilNo]).ToString() + '\t' + (romcLayer2[soilNo] / rotarea[soilNo]).ToString()
                    + '\t' + (biocharcLayer1[soilNo] / rotarea[soilNo]).ToString() + '\t' + (biocharcLayer2[soilNo] / rotarea[soilNo]).ToString()
                    + '\t' + FOMn[soilNo] / rotarea[soilNo] + '\t' + rotresidualMineralN[soilNo] / rotarea[soilNo] + '\t' + rotarea[soilNo]);
            }
        }
        extraCtoolData.Close();
    }
}

