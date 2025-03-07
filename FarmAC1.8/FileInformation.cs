﻿//uncomment the following line to get more detailed diagnostics
//#define printDiagnostics
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using AnimalChange;
#if (server)
using System.Web;
#endif
/*! A class to manage xml and Excel file input and output */
public class FileInformation
{
    //!storing the currently used files
    private string fileNameOrg = "test.xml";
    private string fileNameAlt = "test.xml";

    //!storing the path down the xml tree
    public List<int> Identity = new List<int>();
    //! stores the components of path names as strings
    public List<string> PathNames = new List<string>();
    //!storing currently used primary file
    private Node treeOrg;
    //!storing currently used alternative file 
    private Node treeAlt;
    //! If true, some data will be read from the alternative (non-default) parameter xml files
    private bool UsingAlternative;
    //! If true, the execution will pause before exiting when an error is detected
    private bool pauseBeforeExit=false;
    //!  initialize.
    /*!
      reset FileName and AllNodes.
    */
    public void reset()
    {
        FileName = new List<string>();
        AllNodes = new List<Node>();
    }
    //! A default constructor.
    public FileInformation()
    {
        UsingAlternative = false;
    }
    // contains a list of files that is stored in memery
    static List<string> FileName = new List<string>();
    //a list of xml tress that has been read into memery
    static List<Node> AllNodes = new List<Node>();

   private bool IsFilePresent(string fileString)
    {
        bool isPresent = false;
        string directoryName = "";
        string filename = "";
        string[] partFileNames = fileString.Split('\\');
        string[] theFiles = { "" };
        if (partFileNames.Count() > 1)
        {
            for (int i = 0; i < partFileNames.Count() - 1; i++)
            {
                directoryName += partFileNames[i] + '\\';
            }
            theFiles = Directory.GetFiles(directoryName);
        }
        filename = partFileNames[partFileNames.Count()-1];
        for (int i=0; i<theFiles.Count(); i++)
        {
            if (String.Equals(filename, theFiles[i]))
                isPresent = true;
        }
        return isPresent;
    }
    //!  Read parameter file. 
    /*!
     * Data from files that need to be read frequently are loaded into memory, to save execution time
     * Request for reading a file first checks to see if the file is already in memory
      \param nameOfFile name of the file to read, as a string
    */
    private void ReadingFile(string nameOfFile)
    {
        //bool ret_val = false;
        string file = "";
        //getting the file name from path
        string[] partFileName = nameOfFile.Split('\\');
        bool inUse = false;
        //check if we already have the file in memory
        for (int i = 0; i < FileName.Count; i++)
        {
            //checks if we have the generic file
            if (FileName.ElementAt(i).CompareTo(partFileName[partFileName.Count() - 1]) == 0 && !nameOfFile.Contains("Alternative"))
            {
                //using the generic file
                inUse = true;
                treeOrg = AllNodes.ElementAt(i);
            }
            if (FileName.ElementAt(i).CompareTo(partFileName[partFileName.Count() - 1]) == 0 && nameOfFile.Contains("Alternative"))
            {
                //using the alternative file
                inUse = true;
                treeAlt = AllNodes.ElementAt(i);
            }           
        }
        //the file is not found. so it is going to be read
        if (inUse == false)
        {
            Node tree = new Node();
            string itemName = "ino";
            try
            {
                XmlReader data = null;
                try
                {
                //IsFilePresent(nameOfFile);
                    //check if the file does exist
                    while (!File.Exists(nameOfFile))
                    {
    #if printDiagnostics
                        Console.WriteLine("Looking for " + nameOfFile);
    #endif
                    //file does not exist. Checking if the file is in the directorym above.
                    string[] path = nameOfFile.Split('\\');
                    //at the top directoy so file is not found
                    if (path.Count() == 1 || (path.Count() == 2 && path[0][1] == ':'))
                    {
                        if (!nameOfFile.Contains("Alternative"))
                        {
                            GlobalVars.Instance.log("Could not find " + nameOfFile, 4);
                            GlobalVars.Instance.Error(nameOfFile + " not found", "in FileInformation(string nameOfFile)", true);
                        }
                        break;
                    }
                        nameOfFile = "";
                        //creating new path for checking the file. The dir is the parent dir
                        for (int i = 0; i < path.Count() - 2; i++)
                        {
                            nameOfFile += path[i] + '\\';
                        }
                        nameOfFile += path[path.Count() - 1];
                    }

                    file = nameOfFile;
                    //reading file - if the file does not exist, the program jumps to the catch statement
                    data = XmlReader.Create(nameOfFile);
    #if printDiagnostics
                    Console.WriteLine("Found " + nameOfFile + " in " + Directory.GetCurrentDirectory().ToString());
    #endif
                    if (nameOfFile.Contains("Alternative"))
                    {
                        UsingAlternative = true;
                        GlobalVars.Instance.log("Using " + nameOfFile,4);
                    }

                    string[] partFileNames = nameOfFile.Split('\\');
                    //saving file name so we dont read it again.
                    FileName.Add(partFileNames[partFileNames.Count() - 1]);
                }
                catch
                {
                    //if the generic file (eg non-alternative) is not found then we throw an error
                    if (!nameOfFile.Contains("Alternative.xml"))
                    {
                        GlobalVars.Instance.Error(nameOfFile + " not found", "in FileInformation(string nameOfFile)", false);
                        throw new System.Exception("farm Fail");
                    }
    #if printDiagnostics
                    else
                    Console.WriteLine("Not found " + nameOfFile);
    #endif
                }
                if (data != null)
                {
                    //start reading through the xml-tree
                    while (data.Read())
                    {
                        if (data.NodeType == XmlNodeType.Element)
                        {
                            XElement el = XNode.ReadFrom(data) as XElement;
                            IEnumerable<XElement> node = el.Elements();
                            //running through each sub-trees from the top node
                            for (int i = 0; i < node.Count(); i++)
                            {
                                IEnumerable<XElement> ting = node.ElementAt(i).Elements();
                                //creating a new node with value and name
                                Node newNode = new Node();
                                newNode.setNodeValue(node.ElementAt(i).Value);
                                newNode.setNodeName(node.ElementAt(i).Name.ToString());
                                //reading the nodes children
                                recursionRead(ting, ref newNode);
                                //adding the new node to the tree
                                tree.addChild(newNode);
                            }
                        }
                    }
                    data.Close();
                    //file name saved
                    tree.FileName = file;
                    //saving the xml-tree
                    AllNodes.Add(tree);
                }
            }
            catch (Exception e)
            {
                if (nameOfFile != null)
                    if (!nameOfFile.Contains("Alternative.xml"))
                    {
                        GlobalVars.Instance.log(e.ToString(), 5);
                        if (e.Message.CompareTo("farm Fail") != 0)
                        {
                            string messageString = ("problem with reading: " + nameOfFile + " because of " + e.ToString()) + "\r\n";
                            messageString += ("model terminated") + "\r\n";
                            messageString += ("the path is: ");
                            for (int i = 0; i < PathNames.Count; i++)
                            {
                                messageString += (PathNames[i] + "(" + Identity[i].ToString() + ")");
                            }
                            GlobalVars.Instance.Error(messageString, e.StackTrace, true);
                        }
                        else
                            throw new System.Exception("farm Fail");
                    }
            }
        }
    }
    //! A constructor with one argument.
    /*!
      \param nameOfFile name of the file to be read
    */
    public FileInformation(string[] nameOfFile)
    {
        // Storing the filenames
        fileNameOrg = nameOfFile[0]; //default file name
        fileNameAlt = nameOfFile[1]; //user-defined alternative file name
        //reading if nessesary
        ReadingFile(fileNameAlt);
        ReadingFile(fileNameOrg);
        //choosing the the file from memory
        ReadingFile(fileNameAlt);
        ReadingFile(fileNameOrg); 
    }
    //!  Read the children (subnode) of a node
    /*!
     * this function can call itself recursively
      \param node parent node
      \param subNode points to child (subNode)
    */
    private void recursionRead(IEnumerable<XElement> node,  ref Node subNode)
    {
      
        for (int i = 0; i < node.Count(); i++)
        {
           //creating the new node
            IEnumerable<XElement> ting = node.ElementAt(i).Elements();
            Node child = new Node();
            //setting values and string for the new node
            child.setNodeName(node.ElementAt(i).Name.ToString());
            child.setNodeValue(node.ElementAt(i).Value);
            //reading the nodes children
            recursionRead(ting, ref child);
            //adding the node to the tree
                subNode.SubNode.Add(child);                    
        }
    }
    
    //!  judge ID Exist. checks if a tag with this Identity exists. This function allows a list of tags to be listed in a non-continuous series
    /*!
      \param id node identifier as an integer argument.
      \return true if the node has been found.
    */
    public bool doesIDExist(int ID)
    {
        bool ret_val = false;
        if (treeAlt != null)
            ret_val = recursionDoesIDExist(treeAlt.SubNode, ID, 0);
        if (ret_val == false)
            ret_val = recursionDoesIDExist(treeOrg.SubNode, ID, 0);
        return ret_val;
    }

    //!  seaching down the xml tree recursively to section found. Taking three arguments and returning a boolean value.
    /*!
     * this function can call itself recursively
      \param node, a list argument that points to Node.
      \param id, the identifier number that is being looked for, as an integer value.
      \param iteration the an integer value.
      \return true if the node with the correct identfier is found
    */
    private bool recursionDoesIDExist(List<Node> node, int id, int iteration)
    {
        if (Identity.Count() == iteration)
        {
            for (int i = 0; i < node.Count(); i++)
            {                  
                //section is found
                if (PathNames[iteration].CompareTo(node.ElementAt(i).getNodeName().ToString()) == 0)
                {
                    List<Node> ting = node.ElementAt(i).SubNode;
                    //testing if current section has the correct ID
                    if (Convert.ToInt32(ting.ElementAt(0).getNodeValue()) == id)
                        return true;
                }
            }
        }
        else
        {
            //section not found.Finding the right sub-tree to be analyzed 
            for (int j = 0; j < node.Count(); j++)
            {
                if (node.ElementAt(j).getNodeName().ToString().CompareTo(PathNames[iteration]) == 0)
                {
                    //sub tree has been selected
                    List<Node> ting = node.ElementAt(j).SubNode;
                    if (ting.ElementAt(0).getNodeValue() == Identity[iteration].ToString() || Identity[iteration] == -1)
                    {
                        //returning the result from that sub-treee
                        return recursionDoesIDExist(ting, id, iteration + 1);
                    }
                }
            }
        }
        //id not found. 
        return false;
    }

    //getting the path in xml tree
    //!  Get the path in xml tree. 
    /*!
      \return the path in xml tree as a string value.
    */
    private string pathToString()
    {
        string returnValue="";
        for(int i=0;i<Identity.Count();i++)
        {
            returnValue+=PathNames[i]+"("+Identity[i]+")";
        }
        if (PathNames.Count > Identity.Count)
        {
            returnValue += PathNames[Identity.Count()];
        }
        return returnValue;

    }
   
    //!  Set path. Taking one argument.it must be on item-name(item-id).item-name(item-id)..... if there is no ID then -1 should be used.
    /*!
      \param name the full path as a string argument.
    */
    public void setPath(string name)
    {
        //clearing old data
        PathNames.Clear();
        Identity.Clear();
        string[] items=name.Split('.');
        for (int i = 0; i < items.Count(); i++)
        {
            //getting the ID-number
            int first = items[i].IndexOf('(');
            int last = items[i].IndexOf(')');
            string tmp = items[i];
            if (last != -1 && first != -1)
            {
                //store the id number if found
                try
                {
                    tmp = items[i].Remove(last, 1);
                    tmp = tmp.Remove(0, first + 1);
                    Identity.Add(Convert.ToInt32(tmp));
                    tmp = items[i].Remove(first);
                }
                catch
                {
                    //the string is not in correct format. throwing an error
                  string messageString=(name + " is wrong")+"\n";
                  messageString+=("model terminated") + "\n";
                  messageString += ("the file name is " + fileNameOrg) + "\n";
                  GlobalVars.Instance.Error(messageString);
                }
            }
            //store the item name
            PathNames.Add(tmp);
        }
    }

    //!  Get what is stored at the path as an Integer. Taking two arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as an integer.
    */
    public int getItemInt(string itemName, bool stopOnError = true, string detail ="")
    {
        string output=getItemString(itemName, stopOnError, detail);
        if (output.CompareTo("nothing") != 0)
            return Convert.ToInt32(output);
        else return -1;
    }

    //!  Get what is stored at the path as a double.If the items contains a comma then it will throw an error. Taking two arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as a dpuble.
    */
    public double getItemDouble(string itemName, bool stopOnError = true, string detail = "")
    {
        string stuff = getItemString(itemName, stopOnError, detail);
        if (stuff.Contains(","))
        {

            GlobalVars.Instance.Error("the data value for " + itemName + " contains a comma for file " + fileNameOrg + " or " + fileNameAlt);
        }
        if (stuff.CompareTo("nothing") != 0)
            return Convert.ToDouble(stuff);
        else return 0.0;
    }

    //!  Get what is stored at the path as a bool. Taking two arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as a boolean.
    */
    public bool getItemBool(string itemName, bool stopOnError = true, bool defaultValue = false, string detail = "")
    {
        bool ret_val = defaultValue;
        string stuff = getItemString(itemName, stopOnError, detail);
        if (stuff.CompareTo("nothing") != 0)
            ret_val=Convert.ToBoolean(stuff);
        return ret_val;
    }

    //!  Finds the itemName in xml in the xml file at path and returning it as an int. Taking three arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param path the directory path to be used, a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as an integer value.
    */
    public int getItemInt(string itemName, string path, bool stopOnError = true, string detail = "")
    {
        setPath(path);
        return Convert.ToInt32(getItemString(itemName, stopOnError, detail));
    }

    //!  Finds the itemName in xml in the xml file at path and returning it as a double. Taking three arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param path the directory path to be used, a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as a double value.
    */
    public double getItemDouble(string itemName, string path, bool stopOnError = true, string detail = "")
    {
        setPath(path);
        return Convert.ToDouble(getItemString(itemName, stopOnError,detail));
    }
    //!  Finds the itemName in xml in the xml file at path and returning it as a boolean. Taking three arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param path the directory path to be used, a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as a boolean.
    */
    public bool getItemBool(string itemName, string path, bool stopOnError=true, string detail = "")
    {
        setPath(path);
        string stuff = getItemString(itemName,stopOnError,detail);
        return Convert.ToBoolean(stuff);
    }
    //Finds the itemName in xml in the xml file at path and returning it as a string.
    //!  Finds the itemName in xml in the xml file at path and returning it as a string. Taking three arguments.
    /*!
      \param itemName the name of the variable that is being looked for, as a string argument.
      \param path the directory path to be used, a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the parameter as string
    */
    public string getItemString(string itemName, string path, bool stopOnError=true, string detail = "")
    {
        setPath(path);
        return getItemString(itemName,stopOnError,detail);
    }
   
    //!  Get the minimum and maximum number of tags in a section.  Note that the section numbers do not need to be continuous
    /*!
     * Used when using 'for' loops
      \param min minimum number as an integer argument.
      \param max maximum number as an integer argument
    */
    public void getSectionNumber(ref int min, ref int max)
    {
        /* this code is intended to check whether there are more tags in the alternative file than in the default value and return an error if this is the case
         * If there are fewer entries in the default file then not all the data in the alternative file will be searched. For some reason, this does not work*/
       int altmin = 99, altmax = 0;
        if (treeAlt!=null)
            recursionForSectionNumber(treeAlt.SubNode, ref altmin, ref altmax,0);
        recursionForSectionNumber(treeOrg.SubNode, ref min, ref max, 0);
        int altNo = altmax - altmin;
        int origNo = max - min;
        if (altNo>origNo)
        {
            string messageString = "More data in alternative file " + fileNameAlt + " than in default file " + fileNameOrg;
            GlobalVars.Instance.Error(messageString); 
        }
    }

    //!  searching for the path down the xml-tree and report back how many section there are.
    /*!
      \param node a list argument that points to Node.
      \param min minimum number as an integer argument.
      \param max maximum number as an integer argument
      \param iteration number of times the function has called itself, as an integer argument.
      \return true if the section has been found, otherwise false.
    */
    private bool recursionForSectionNumber(List<Node> node, ref int min, ref int max, int iteration)
    {
        if (Identity.Count() == iteration)
        {
            for (int i = 0; i < node.Count(); i++)
            {
                //section found
                string testNodeName = node.ElementAt(i).getNodeName().ToString();
                if (PathNames[iteration].CompareTo(testNodeName) == 0)
                {
                    //finding the minimum and maximum section
                    List<Node> ting = node.ElementAt(i).SubNode;
                    if (Convert.ToInt32(ting.ElementAt(0).getNodeValue()) > max)
                        max = Convert.ToInt32(ting.ElementAt(0).getNodeValue());
                    if (Convert.ToInt32(ting.ElementAt(0).getNodeValue()) < min)
                        min = Convert.ToInt32(ting.ElementAt(0).getNodeValue());
                }
            }
            return true;
        }
        else
        {
            //section not found. Seaching further down the xml tree
            for (int j = 0; j < node.Count(); j++)
            {
                //match according to the path
                if (node.ElementAt(j).getNodeName().ToString().CompareTo(PathNames[iteration]) == 0)
                {
                    List<Node> ting = node.ElementAt(j).SubNode;
                     //seaching in the right child.
                    if (ting.ElementAt(0).getNodeValue() == Identity[iteration].ToString() || Identity[iteration] == -1)
                             if (recursionForSectionNumber(ting, ref min, ref max, iteration + 1) == true)
                                 break;
                }
            }
        }
        return false;
    }

    //!  seaching for a string variable
    /*!
      \param itemName the name of the variable as a string argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return the variable as a string value.
    */
    public string getItemString(string itemName, bool stopOnError=true, string detail = "")
    {
        string info = "nothing";
        //seaching in the alternative file if it is present
        if (treeAlt != null)
        {
            //running through all top level nodes to see which children that fits the criteria from the path
            for (int i = 0; i < treeAlt.SubNode.Count(); i++)
                if (PathNames[0].CompareTo(treeAlt.SubNode[i].getNodeName()) == 0)
                {
                    //child found. Seaching through its kids 
                    if (treeAlt.SubNode[i].SubNode.ElementAt(0).getNodeValue() == Identity[0].ToString() || Identity[0] == -1)
                        info = recursionForItem(treeAlt.SubNode[i].SubNode, itemName, 0, false, detail);
                }
        }
        bool found=false;
        //item found. Writing it to the log 
        if (info.CompareTo("nothing") != 0)
        {
            GlobalVars.Instance.log(pathToString() + itemName + " as " + info + "found in " + treeAlt.FileName, 6);
            found = true;
        }
        if(info.CompareTo("nothing")==0)
        {
            //item not found. Seaching in normal file
        for (int i = 0; i < treeOrg.SubNode.Count(); i++)
            if (PathNames[0].CompareTo(treeOrg.SubNode[i].getNodeName()) == 0)
            {

                if (treeOrg.SubNode[i].SubNode.ElementAt(0).getNodeValue() == Identity[0].ToString() || Identity[0] == -1)
                    info = recursionForItem(treeOrg.SubNode[i].SubNode, itemName, 0, stopOnError, detail);
            }
        }
        if (found==false)
            GlobalVars.Instance.log(pathToString() + itemName + " as " + info + " found in " + treeOrg.FileName, 6);

        //if the item is not found then we throw an error
        if (stopOnError == true && info.CompareTo("nothing")==0)
        {
            string messageString = ("could not find " + itemName) + "\n";
            if (detail.Length > 0)
                messageString += detail + "\n";
            messageString += ("model terminated") + "\n";
            messageString += ("the path is: ");
            for (int i = 0; i < PathNames.Count; i++)
            {
                messageString += (PathNames[i] + "(" + Identity[i].ToString() + ")");
            }

            messageString += ("the file name is " + fileNameOrg);
            GlobalVars.Instance.Error(messageString);
        }
        return info;
      
        
    }

    //!  seaching for itemName recursively. Taking four arguments. 
    /*!
      \param node a list argument that points to Node.
      \param itemName the name of the variable as a string argument.
      \param iteration number of times the function has called itself, as an integer argument.
      \param stopOnError if true, the program will stop if there is an error. Set false for optional variables.
      \return variable as a string value.
    */
    public string recursionForItem(List<Node> node, string itemName, int iteration, bool stopOnError, string detail = "")
    {
        //running through all notes 
        for (int i = 0; i < node.Count(); i++)
        {
            //checks if we found the right child
            string tmp = node.ElementAt(i).getNodeName();
            if (node.ElementAt(0).getNodeName().ToString().CompareTo("Identity") == 0 || Identity[iteration] == -1)
                {
                    if (node.ElementAt(0).getNodeValue() == Identity[iteration].ToString() || Identity[iteration] == -1)
                    {
                        if (Identity.Count() == (iteration + 1))
                        {
                            for (int j = 0; j < node.Count(); j++)
                            {
                                if (node.ElementAt(j).getNodeName().ToString().CompareTo(itemName) == 0)
                                {
                                    if (node.ElementAt(j).getNodeValue().CompareTo("") == 0)
                                        break;
                                    else
                                    {
                                        //right child has been found. returning its value
                                        return node.ElementAt(j).getNodeValue();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //the right child has not been found. 
                            //seaching for the next sub tree to be analysed
                            for (int j = 0; j < node.Count(); j++)
                            {

                                if (node.ElementAt(j).getNodeName().ToString().CompareTo(PathNames[iteration + 1]) == 0)
                                {
                                    //found the right sub tree. 
                                    List<Node> ting=node.ElementAt(j).SubNode;
                                    if (ting.ElementAt(0).getNodeValue() == Identity[iteration + 1].ToString() || Identity[iteration + 1] == -1)
                                    {
                                        //returning the result from that subtree 
                                        return recursionForItem(ting, itemName, iteration + 1, stopOnError, detail);
                                    }
                                }

                            }
                        }

                    }
                }
            }
        //if item is not found and if it is critical that we find it then we are throwing an error 
        if (stopOnError)
        {
            string messageString = ("could not find " + itemName);
            if (detail.Length>0)
                messageString += " for parameter " + detail + "\n";
            messageString += (" model terminated") + "\n";
            messageString += (" the path is: ");
            for (int i = 0; i < PathNames.Count; i++)
            {
                messageString += (PathNames[i] + "(" + Identity[i].ToString() + ")");
            }

            messageString += (" the file name is " + fileNameOrg);
            GlobalVars.Instance.Error(messageString);
        }
        return "nothing";
   }

}
