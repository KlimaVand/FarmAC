//#define LocalServer
#define test_runs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
#if server
//using FarmAC.Controls;
#endif
namespace AnimalChange
{
    //! Contains the main functions for controlling the model
    class model 
    {
        List<FarmClass> theFarms;
        public static string errorMessageReturn = "";
        //! If errors are to be read by the server, this function is used to access the string.
        /*!
         \return the error message as a string value.
        */
        public string GetErrorMessageReturn
        {
            get { return errorMessageReturn; }
        }
        //args[0] and args[1] are farm number and scenario number respectively
        //if args[2] is 1, the energy demand of livestock must be met (0 is used when reporting current energy balance of livestock to users)
        //if args[3] is 1, the crop model requires the modelled production of grazed DM to match the expected production. If =0, grazed DM will be imported if there is a deficit
        //if args[4] is -1, the model will spinup for the years in the spinupYearsBaseLine parameter then run a baseline scenario and generate a Ctool data transfer file
        //if args[4] is a positive integer and the spinupYearsNonBaseLine parameter is zero, the model will read Ctool data from the Ctool data transfer file. If spinupYearsNonBaseLine is not zero, the model will spin up for spinupYearsNonBaseLine years then run the scenario
        // So to just calculate the animal production, set the farm file in system.xml to "farm.xml" and the args to <farm number> <scenario number> "0" "0" "-1"
        //if the number of arguments are 6 instead of 5, the errormessages will be returned as a string instead of being written to the error-file

        //! This function controls the model run.
        /*!
         \param args arguments sent by the call to initiate the model, as a string argument.
        */
        public void run(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-GB");
                string dir = Directory.GetCurrentDirectory();
#if LocalServer
                Directory.SetCurrentDirectory(GlobalVars.Instance.GetlocalServerDirectory());
#endif
                //start timing of the excution
                Stopwatch timer = new Stopwatch();
                timer.Start();
                //start reading the files controlling the execution
                string[] system = new string[2];

#if test_runs   //Run all test farms
                system[0] = "system_test.xml";
                system[1] = "system_testAlternative.xml";
#else
                system[0] = "system.xml";
                system[1] = "systemAlternative.xml";
#endif
                FileInformation settings = new FileInformation(system);
                settings.setPath("CommonSettings(-1)");
                string outputDir = String.Copy(settings.getItemString("alternativePath")); ;
                //determine whether logging will occur and how
                bool logFile = settings.getItemBool("logFile", false);
                string logfilename = "";
                if (logFile)  //open log file at alternative location
                {
                    logfilename = outputDir + "\\log.txt";
                    GlobalVars.Instance.OpenLogfile(logfilename);
                }
                bool logScreen = settings.getItemBool("logScreen", false);
                //sets level of reporting from model run
                int verbosity = settings.getItemInt("verbosityLevel", false);
                //read whether output files should be created
                //output results as xml file
                bool outputxlm = settings.getItemBool("outputxlm", false);
               //output results as Excel file
                bool outputxls = settings.getItemBool("outputxls", false);
                //output results from soil carbon model as xml file
                bool ctooltxlm = settings.getItemBool("ctooltxlm", false);
                //output results from soil carbon model as Excel file
                bool ctoolxls = settings.getItemBool("ctoolxls", false);
                //Check if need to create file for outputs during debugging. Contents can vary
                bool Debug = settings.getItemBool("Debug", false);
                // Check if need to create output file for livestock data
                bool livestock = settings.getItemBool("livestock", false);
                //Check if need to create output file for plant data
                bool Plant = settings.getItemBool("Plant", false);
                // Check if need to create file for summary output data
                bool SummaryExcel = settings.getItemBool("SummaryExcel", false);

                if (verbosity == -1)
                    verbosity = 5;
                int minSetting = 99, maxSetting = 0;
                //see how many farms need to be simulated. This is only relevant when running the model locally (not called from user interface)
                settings.setPath("settings");
                settings.getSectionNumber(ref minSetting, ref maxSetting);
                if (minSetting == 99 && maxSetting == 0)
                {
                    GlobalVars.Instance.log("No farms to simulate found in system.xml", -1);
                    if (logScreen)
                    {
                        Console.Write("No farms to simulate found in system.xml");
                        Console.ReadKey();
                    }
                    GlobalVars.Instance.Error("No farms to simulate found in system.xml");
                }
                else
                {
                    theFarms = new List<FarmClass>();
                    GlobalVars.Instance.CloseLogFile();  //close log file at alternative location. Log files may be opened for each farm
                }
                for (int settingsID = minSetting; settingsID <= maxSetting; settingsID++)
                {
                    FarmClass aFarm = new FarmClass();
                    theFarms.Add(aFarm);
                    GlobalVars.Instance.reset(settingsID.ToString());
                    GlobalVars.Instance.logFile = logFile;
                    GlobalVars.Instance.logScreen = logScreen;
                    GlobalVars.Instance.verbosity = verbosity;

                    GlobalVars.Instance.Writeoutputxlm = outputxlm;
                    GlobalVars.Instance.Writeoutputxls = outputxls;
                    GlobalVars.Instance.Writectoolxlm = ctooltxlm;
                    GlobalVars.Instance.Writectoolxls = ctoolxls;
                    GlobalVars.Instance.WriteDebug = Debug;
                    GlobalVars.Instance.Writelivestock = livestock;
                    GlobalVars.Instance.WriteField = Plant;
                    GlobalVars.Instance.WriteCrop = Plant;
                    GlobalVars.Instance.WriteSummaryExcel = SummaryExcel;
                    string farmAndScenario= "";
                    if (args.Length != 5 && args.Length != 0 && args.Length != 6)
                    {
                        GlobalVars.Instance.log("Missing arguments in arg list", 5);
                        GlobalVars.Instance.Error("Missing arguments in arg list");
                    }
                    //This code is called when the model is initiated by a call from the web interface on the server
                    if (args.Length == 5 || args.Length == 6)                //the errormessages will be returned as a string instead of being written to the error-file
                    {
                        farmAndScenario = args[0] + "_" + args[1];
                        if (args[2].CompareTo("1") == 0) //if args[2] = 1, the full model is run
                            GlobalVars.Instance.setRunFullModel(true);
                        else if (args[2].CompareTo("0") == 0)
                            GlobalVars.Instance.setRunFullModel(false);   //used when reporting current energy balance of livestock to users
                        else
                        {
                            GlobalVars.Instance.log("Invalid args input", 5);
                            GlobalVars.Instance.Error("Invalid args input");
                        }
                        // Setting args[3] to false means the production and consumption of grazed feed does not need to balance (within error margin).
                        // Setting this to false should only be done when wishing to adjust other aspects of the farm, before finally adjusting the grazing feed balance
                        if (args[3].CompareTo("1") == 0)
                            GlobalVars.Instance.SetstrictGrazing(true);
                        else if (args[3].CompareTo("0") == 0)
                            GlobalVars.Instance.SetstrictGrazing(false);
                        else
                        {
                            GlobalVars.Instance.log("invalid input", 5);
                            GlobalVars.Instance.Error("Missing arguments in arg list");
                        }
                        GlobalVars.Instance.reuseCtoolData = Convert.ToInt32(args[4]);
                        if (args.Length == 6)
                        {
                            GlobalVars.Instance.ResetErrorMessageReturn();
                            GlobalVars.Instance.returnErrorMessage = true;
                        }
                    }
                    else  //model is called directly from OS or IDE
                    {
                        GlobalVars.Instance.SetstrictGrazing(true);
                        GlobalVars.Instance.setRunFullModel(true);
                        if (settings.Identity.Count == 1)
                            settings.Identity.RemoveAt(0);
                        if (settings.doesIDExist(settingsID))
                        {
                            GlobalVars.Instance.reuseCtoolData = -1;
                            settings.Identity.Add(settingsID);
                            GlobalVars.Instance.reuseCtoolData = settings.getItemInt("Adaptation", false);
                        }
                        farmAndScenario = settings.getItemString("farm");
                    }

                    if (settings.Identity.Count == 1)
                        settings.Identity.RemoveAt(0);
                    if (settings.doesIDExist(settingsID))
                    {
                        settings.setPath("settings(" + settingsID.ToString() + ")");
                        outputDir = String.Copy(settings.getItemString("outputDir"));
                        if (!Directory.Exists(outputDir))
                            Directory.CreateDirectory(outputDir);
                        if (args.Length != 0 && args[0].CompareTo("-1") != 0)
                            GlobalVars.Instance.seterrorFileName(outputDir + "error" + "_" + args[0] + "_" + args[1] + ".txt");
                        else
                            GlobalVars.Instance.seterrorFileName(outputDir + "error.txt");
                        if (args.Count() == 0)  //called from OS or IDE
                        {
                            try
                            {
                                logfilename = outputDir + "\\log.txt";
                                if (File.Exists(logfilename))
                                    File.Delete(logfilename);
                                GlobalVars.Instance.logFileStream = new System.IO.StreamWriter(logfilename);
                            }
                            catch (Exception e)
                            {
                                GlobalVars.Instance.Error("Failed to open log file",e.StackTrace);
                            }
                        }
                        else
                        {
                            try
                            {
                                logfilename = outputDir + "log" + args[0] + "_" + args[1] + ".txt";
                                GlobalVars.Instance.logFileStream = new System.IO.StreamWriter(logfilename) ;
                            }
                            catch (Exception e)
                            {
                                GlobalVars.Instance.Error("Failed to open log file", e.StackTrace);
                            }
                        }
                        string[] file = fileName(args, settings, "constants");
                        GlobalVars.Instance.setConstantFilePath(file);
                        string farmName = settings.getItemString("farm");

                        //this code section expects the 'farm' tag in system.xml to consist of a full file name (path + filename.xxx). 
                        //The model will look for input in path + filename + "_" + args[0] + "_" + args[1].xxx
                        //note that the farm file in system.xml must be called "farm.xml" for this to work
                        if (args.Length != 0 && args[0].CompareTo("-1") != 0)
                        {
                            string[] names = farmName.Split('.');
                            farmName = getPath(farmName) + "_" + args[0] + "_" + args[1] + "." + names[names.Count() - 1];
                        }
                        //Set up paths for file names
                        string[] farmNames = new string[2];
                        farmNames[0] = farmName;
                        farmNames[1] = getPath(farmName) + "Alternative.xml";
                        GlobalVars.Instance.setFarmtFilePath(farmNames);
                        GlobalVars.Instance.log("Begin simulation of:", 5);
                        GlobalVars.Instance.log(farmName, 5);
                        file = fileName(args, settings, "feedItems");
                        GlobalVars.Instance.setFeeditemFilePath(file);
                        file = fileName(args, settings, "parameters");
                        GlobalVars.Instance.setParamFilePath(file);
                        file = fileName(args, settings, "fertAndManure");
                        GlobalVars.Instance.setfertManFilePath(file);
                        GlobalVars.Instance.setPauseBeforeExit(Convert.ToBoolean(settings.getItemString("pauseBeforeExit")));
                        GlobalVars.Instance.setstopOnError(Convert.ToBoolean(settings.getItemString("stopOnError")));
                        try
                        {
                            GlobalVars.Instance.readGlobalConstants();
                        }
                        catch (Exception e)
                        {
                            GlobalVars.Instance.Error("constant file not found " + e.Message, "program" + e.Message, false);
                        }
                        string tmps = Directory.GetCurrentDirectory();
                        //Find out how many farms need to be simulated
                        FileInformation farmInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
                        farmInformation.setPath("Farm");
                        int min = 99999999, max = 0;

                        farmInformation.getSectionNumber(ref min, ref max);
                        if ((GlobalVars.Instance.reuseCtoolData == -1) && (max != min))
                            GlobalVars.Instance.Error("Model called in baseline mode, when more than one scenario is present in the system.dat file");
                        //Run through each farm in turn
                        for (int farmNr = min; farmNr <= max; farmNr++)
                        {
                            //See if the farm file with this number exists
                            try
                            {
                                if (farmInformation.doesIDExist(farmNr))
                                {
                                    aFarm.SetFarmNo(farmNr);
                                    string newPath = "Farm(" + farmNr.ToString() + ")";
                                    farmInformation.setPath(newPath);
                                    int zoneNr = farmInformation.getItemInt("AgroEcologicalZone");
                                    GlobalVars.Instance.SetZone(zoneNr);
                                    int FarmType = farmInformation.getItemInt("FarmType");
                                    if (FarmType > 3)
                                        GlobalVars.Instance.Error("Farmtype not supported");
                                    else
                                        aFarm.SetFarmType(FarmType);
                                    GlobalVars.Instance.theZoneData.readZoneSpecificData(zoneNr, FarmType);
                                    double Ndep = farmInformation.getItemDouble("Value", newPath + ".NDepositionRate(-1)");
                                    GlobalVars.Instance.theZoneData.SetNdeposition(Ndep);
                                    //start dealing with the different scenarios (must at least be a baseline scenario)
                                    newPath = newPath + ".SelectedScenario";
                                    farmInformation.setPath(newPath);
                                    int minScenario = 99, maxScenario = 0;
                                    farmInformation.getSectionNumber(ref minScenario, ref maxScenario);
                                    for (int settingsnr = minScenario; settingsnr <= maxScenario; settingsnr++)
                                    {
                                        int InventorySystem = 0;
                                        int energySystem = 0;
                                        double areaWeightedDuration = 0;
                                        double farmArea = 0;

                                        int ScenarioNr = 0;
                                        if (args.Length == 0)
                                        {
                                            string[] tmp = GlobalVars.Instance.getFarmFilePath()[0].Split('_');
                                            ScenarioNr = Convert.ToInt32(tmp[tmp.Length - 1].Split('.')[0]);
                                        }
                                        else
                                            ScenarioNr = Convert.ToInt32(args[1]);
                                        aFarm.SetScenarioNo(ScenarioNr);

                                        GlobalVars.Instance.log("Starting farm " + farmNr.ToString() + " scenario nr " + ScenarioNr, 0);
                                        if (args.Length == 0)  //Set location of soil carbon pool data
                                        {
                                            GlobalVars.Instance.setReadHandOverData(outputDir + "dataCtool" + "_" + farmNr + "_" + GlobalVars.Instance.reuseCtoolData + ".txt");
                                            GlobalVars.Instance.setWriteHandOverData(outputDir + "dataCtool" + "_" + farmNr + "_" + ScenarioNr.ToString() + ".txt");
                                        }
                                        else
                                        {
                                            GlobalVars.Instance.setReadHandOverData(outputDir + "dataCtool" + "_" + args[0] + "_" + args[4] + ".txt");
                                            GlobalVars.Instance.setWriteHandOverData(outputDir + "dataCtool" + "_" + args[0] + "_" + args[1] + ".txt");
                                        }
                                        int soilTypeCount = 0;

                                        if (farmInformation.doesIDExist(settingsnr))
                                        {
                                            string outputName;
                                            if ((GlobalVars.Instance.reuseCtoolData == -1) && (args.Length == 0))
                                                outputName = "outputFarm" + farmNr.ToString() + "BaselineScenarioNr" + ScenarioNr.ToString();
                                            else
                                                outputName = "outputFarm" + farmNr.ToString() + "ScenarioNr" + ScenarioNr.ToString();

                                            GlobalVars.Instance.OpenOutputFiles(outputName, outputDir);
                                            GlobalVars.Instance.writeStartTab("Farm");

                                            GlobalVars.Instance.initialiseExcretaExchange();
                                            GlobalVars.Instance.initialiseFeedAndProductLists();
                                            string ScenarioPath = newPath + "(" + ScenarioNr.ToString() + ")";
                                            farmInformation.setPath(ScenarioPath);
                                            farmInformation.Identity.Add(-1);
                                            if (GlobalVars.Instance.getcurrentInventorySystem() == 0)
                                            {
                                                farmInformation.PathNames.Add("InventorySystem");
                                                InventorySystem = farmInformation.getItemInt("Value");
                                                GlobalVars.Instance.setcurrentInventorySystem(InventorySystem);
                                            }
                                            farmInformation.PathNames.Add("EnergySystem");
                                            energySystem = farmInformation.getItemInt("Value");
                                            GlobalVars.Instance.setcurrentEnergySystem(energySystem);
                                            if (GlobalVars.Instance.GetstrictGrazing() == true)
                                            {
                                                aFarm.SetupRotation(farmInformation, newPath, zoneNr, farmNr, ScenarioNr, FarmType, soilTypeCount);
                                                aFarm.SetupLivestockAndManure(farmInformation, zoneNr, farmNr, newPath, ScenarioNr);
                                                GlobalVars.Instance.theManureExchange = new GlobalVars.theManureExchangeClass();
                                                //GlobalVars.Instance.setRunFullModel(false);
                                                if (!GlobalVars.Instance.getRunFullModel()) //only called when only the livestock excretion is needed
                                                {
                                                    GlobalVars.Instance.CalcAllFeedAndProductsPotential(aFarm.GetRotationList());
                                                    //write output to xml file
                                                    GlobalVars.Instance.writeGrazedItems();
                                                    aFarm.WriteLivestockAndManure();
                                                }
                                                else
                                                {
                                                    aFarm.RunFarm();
                                                    aFarm.CreateFarmBalances();
                                                    aFarm.WriteLivestockAndManure();
                                                    aFarm.WriteRotation(outputDir + outputName);
                                                    aFarm.WriteFarmBalances();
                                                    GlobalVars.Instance.writeEndTab(); }
                                            }
                                        }//end of scenario exists
                                        long ticks = DateTime.UtcNow.Ticks;
                                    }//end of scenario
                                }//end of farm exists
                            }
                            catch (Exception e)
                            {
                                if (!e.Message.Contains("farm Fail"))
                                {
                                    GlobalVars.Instance.log("Program terminated with untrapped error " + e.StackTrace, 0);
                                }
                                GlobalVars.Instance.Error(e.Message, e.StackTrace, true);
                            }
                        }
                        //GlobalVars.Instance.theZoneData.CloseDebugFile();
                        //GlobalVars.Instance.CloseLogFile();
                        GlobalVars.Instance.CloseAllFiles();
                        GlobalVars.Instance.CloseLogFile();  //close log file 
                    }
                    Console.WriteLine("Finished after running " + (settingsID + 1).ToString() + " scenarios");
                    timer.Stop();
                    // Get the elapsed time as a TimeSpan value.
                    TimeSpan ts = timer.Elapsed;

                    // Format and display the TimeSpan value. 
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                    Console.WriteLine("RunTime (hrs:mins:secs) " + elapsedTime);
                    //
                }
            }
            catch (Exception e)
            {
                string errorString = e.Message;
            }
            Console.Write("Press any key");
            Console.Read();
        }


        //! A normal member. get Path. Taking one argument and returning string value.
        /*!
         \param oldSPath, one string argument.
         \return one string value.
        */
        static string getPath(string oldSPath)
        {
            string[] oldSPathSub = oldSPath.Split('.');
            string returnValue = "";
            for (int i = 0; i < oldSPathSub.Count() - 1; i++)
            {
                returnValue += oldSPathSub[i];
                if (i < (oldSPathSub.Count() - 2))
                {
                    returnValue += ".";
                }
            }
            return returnValue;
        }
        //! A normal member. get fileName. Taking three arguments and returning string value.
        /*!
         \param args, one string argument.
         \param settings, one class instance argument that points to FileInformation.
         \param file, one string argument.
         \return one string value.
        */
        static string[] fileName(string[] args, FileInformation settings, string file)
        {
            string[] names = new string[2];
            if (args.Length != 0)
            {
                    List<string> tmpPath = new List<string>(settings.PathNames);
                List<int> tmpID = new List<int>(settings.Identity);
                settings.Identity.Clear();
                settings.PathNames.Clear();
                settings.setPath("CommonSettings(-1)");
                string alternativePath = settings.getItemString("alternativePath");
                settings.PathNames = tmpPath;
                settings.Identity = tmpID;
                string constants = settings.getItemString(file);
                string[] constantsList = constants.Split('\\');
                string fileName = constantsList[constantsList.Length - 1];
                if (args[0] == "-1")
                    alternativePath += "\\" + fileName;
                else
                    alternativePath += "\\" + args[0] + "\\" + fileName;
                if (File.Exists(alternativePath))
                {
                    names[0] = alternativePath;
                }
                else
                {
                    names[0] = constants;
                }
                alternativePath = getPath(alternativePath) + "Alternative.xml";
                if (File.Exists(alternativePath))
                {
                    names[1] = alternativePath;
                }
                else
                {
                    names[1] = getPath(constants) + "Alternative.xml";
                }
            }
            else
            {
                names[0] = settings.getItemString(file);
                names[1] = getPath(settings.getItemString(file)) + "Alternative.xml";
            }
            return names;


        }
    }

}
