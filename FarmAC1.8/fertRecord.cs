using System;
using System.Collections.Generic;
using System.Xml;
/*! A class that named CropClass */
    //! An struct. Contains all the information on each application of manure or fertiliser
    /*! More detailed struct description. */
    public class fertRecord //Contains all the information on each application of manure or fertiliser
    {
        public string Name; /*!< Name of the manure or fertiliser. */
        public string Parens; /*!< Parens string. */
        public double Namount; /*!< Total amount of N applied. */
        public double Camount; /*!< total amount of C applied. */
        public int Month_applied; /*!<Month of the year when the application is made. */
        public int dayOfApplication; /*!<Day during the duration of the crop that the application is made. */
        public timeClass applicdate; /*!<calendar date when the application is made. */
        public string Applic_techniqueS; /*!<Name of the manure application technique. */
        public int Applic_techniqueI;  /*!<Identification number of the manure application technique. */
        public int ManureStorageID; /*!< Identification number of the manure storage from which the manure originates. */
        public int speciesGroup;  /*!< Species of livestock producing the manure. */
        public string Units;  /*!< Units used to. */

        public fertRecord()
        { }

        //! A normal member, Check to see if fertiliser or manure is applied outside the crop period.Taking three arguments and returning an bool value.
        /*!
          \param cropInformation, a class argument that points to FileInformation.
          \param startTime, a class argument that points to timeClass.
          \param endTime, a class argument that points to timeClass.
          \return a bool value    
        */
        public bool ReadFertManApplication(FileInformation cropInformation, timeClass startTime, timeClass endTime)
        {
            bool aVal = false;
            Name = cropInformation.getItemString("Name");
            Units = cropInformation.getItemString("Unit");
            Namount = cropInformation.getItemDouble("Value");
            int applicationYear = startTime.GetYear();
            int month_applied = cropInformation.getItemInt("Month_applied");
            //only have month of application, so need to set a sensible day in month
            if (month_applied < startTime.GetMonth())
                applicationYear++;
            if (month_applied == startTime.GetMonth())
                SetapplicDate(startTime.GetDay(), month_applied, applicationYear, false); //earliest possible day
            else if (month_applied == endTime.GetMonth())
            {
                int applicationDay = Math.Max(15, endTime.GetDay());
                SetapplicDate(applicationDay, month_applied, applicationYear, false); //last possible day
            }
            else
                SetapplicDate(15, month_applied, applicationYear,false);//some day in the middle of the month
            if ((GetDate().getLongTime() < startTime.getLongTime()) || (GetDate().getLongTime() > endTime.getLongTime()))
                aVal = false;
            else
                aVal = true;
            return aVal;
        }
        //! Write fertiliser and manure information to file
        /*!
          \param theParens a string argument containing information about the crop tow which this information applies       
        */
        public void Write(string theParens)
        {
            Parens = theParens;
            GlobalVars.Instance.writeStartTab("fertRecord");

            GlobalVars.Instance.writeInformationToFiles("Namount", "Total N applied", "kg/ha", Namount, Parens);
            GlobalVars.Instance.writeInformationToFiles("Applic_techniqueS", "Application technique", "-", Applic_techniqueS, Parens);
            GlobalVars.Instance.writeInformationToFiles("Applic_techniqueI", "Application technique ID", "-", Applic_techniqueI, Parens);
            GlobalVars.Instance.writeInformationToFiles("ManureType", "Manure category", "-", ManureStorageID, Parens);
            GlobalVars.Instance.writeInformationToFiles("speciesGroup", "Species category", "-", speciesGroup, Parens);
            GlobalVars.Instance.writeInformationToFiles("Month_applied", "Month applied", "month", Month_applied, Parens);
            GlobalVars.Instance.writeInformationToFiles("dayOfApplication", "Day applied", "Day in month", dayOfApplication, Parens);

            GlobalVars.Instance.writeEndTab();

        }
    //! Write fertiliser and manure information to Excel (csv) file
        public void WriteXls()
        {
            GlobalVars.Instance.WriteCropFile("Name", "-", Name, false, false);
            GlobalVars.Instance.WriteCropFile("Day applied", "Day", applicdate.GetDay(), true, false);
            GlobalVars.Instance.WriteCropFile("Month_applied", "Month", applicdate.GetMonth(), true, false);
            GlobalVars.Instance.WriteCropFile("N applied", "kgN/ha", Namount, true, false);
            GlobalVars.Instance.WriteCropFile("Applic_technique", "-", Applic_techniqueS, false, false);
        }

        //! A normal member, Copy constructor.Taking one argument and returning an bool value.
        /*!
          \param theCropToCopied, a constructor argument that points to fertRecord.          
        */
        public fertRecord(fertRecord theCropToBeCopied)
        {
            Name = theCropToBeCopied.Name;
            Parens = theCropToBeCopied.Parens;
            Namount = theCropToBeCopied.Namount;
            Camount = theCropToBeCopied.Camount;
            Month_applied = theCropToBeCopied.Month_applied;
            dayOfApplication = theCropToBeCopied.dayOfApplication;
            if (theCropToBeCopied.applicdate != null)
                applicdate = new timeClass(theCropToBeCopied.applicdate);
            else
                applicdate = null;
            Applic_techniqueS = theCropToBeCopied.Applic_techniqueS;
            Applic_techniqueI = theCropToBeCopied.Applic_techniqueI;
            ManureStorageID = theCropToBeCopied.ManureStorageID;
            speciesGroup = theCropToBeCopied.speciesGroup;
            Units = theCropToBeCopied.Units;
        }
        //! A normal member, Set Parens. Taking one argument.
        /*!
          \param aParen, a string argument that points to Parens.    
        */
        public void setParens(string aParen) { Parens = aParen; }
        //! A normal member, Get N amount.Returning a double value.
        /*!
          \return a double value forNamount.        
        */
        public double getNamount() { return Namount; }
        //! A normal member, Get C amount.Returning a double value.
        /*!
          \return a double value for Camount.     
        */
        public double getCamount() { return Camount; }
        //! A normal member, Get Name.Returning a string value.
        /*!
          \return a string value for Name.       
        */
        public string getName() { return Name; }
        //! A normal member, Get speiciesGroup.Returning an integer value.
        /*!
          \return an integer value for speciesGroup.       
        */
        public int getspeciesGroup() { return speciesGroup; }
        //! A normal member, Get ManureType.Returning an integer value.
        /*!
          \return an integer value for ManureStroageID.       
        */
        public int getManureType() { return ManureStorageID; }
        //! A normal member, Get Month_applied.Returning an integer value.
        /*!
          \return an integer value for Month_applied.       
        */
        public int GetMonth_applied() { return Month_applied; }
        //! A normal member, Set dayOfApplication. Taking one argument.
        /*!
          \param aDay, an integer argument that points to dayOfApplication.    
        */
        public void SetdayOfApplication(int aDay) { dayOfApplication = aDay; }
        //! A normal member, Get dayOfApplication.Returning an integer value.
        /*!
          \return an integer value for dayOfApplication.       
        */
        public int GetdayOfApplication() { return dayOfApplication; }
        //! A normal member, Set Namount. Taking one argument.
        /*!
          \param aVal, a double argument that points to Namount.    
        */
        public void SetNamount(double aVal) { Namount = aVal; }
        //! A normal member, Set applicDate. Taking three integer arguments.
        /*!
          \param day, an integer argument that points to day.
          \param month, an integer argument that points to month. 
          \param year, an integer argument that points to year. 
        */
        public void SetapplicDate(int day, int month, int year, bool checkday)
        {
            applicdate.setDate(day, month, year, checkday);
        }
        //! A normal member, Get Date.Returning a timeClass value.
        /*!
          \return a timeClass value for Date.       
        */
        public timeClass GetDate() { return applicdate; }
        //! A normal member, Get RelativeDay. Taking one argument and Returning a long value.
        /*!
         * \param startDay, a long argument that points to timeClass startDate.
          \return a long value for startDay.       
        */
        public long GetRelativeDay(long startDay)//timeClass startDate) 
        {
            long retVal = applicdate.getLongTime() - startDay;// startDate.getLongTime();

            return retVal;
        }
    }




