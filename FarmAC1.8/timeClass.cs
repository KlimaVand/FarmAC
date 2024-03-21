using System;
using System.Xml;
/*! 
 * A class that contains functions related to time and date. 
 * This could usefully be replaced by the inbuilt C# DateTime class
 */
public class timeClass
{
    //!The day in month
    private int day;  
    //!The month
    private int month;
    //! The year
    private int year;
    string parens; /*!<! a string containing information about the farm and scenario number.*/
    //Array containing the number of days per month
    private int[] tabDaysPerMonth;
    //! A defaul constructor.
    public timeClass()
	{
        tabDaysPerMonth = new int[12];
        tabDaysPerMonth[0] = 31;
        tabDaysPerMonth[1] = 28;
        tabDaysPerMonth[2] = 31;
        tabDaysPerMonth[3] = 30;
        tabDaysPerMonth[4] = 31;
        tabDaysPerMonth[5] = 30;
        tabDaysPerMonth[6] = 31;
        tabDaysPerMonth[7] = 31;
        tabDaysPerMonth[8] = 30;
        tabDaysPerMonth[9] = 31;
        tabDaysPerMonth[10] = 30;
        tabDaysPerMonth[11] = 31;
	}
    //! A constructor that creates a new instance that is a copy of an existing instance.
    /*!
      \param existingClass, instance that points to a timeClass class.
     */
    public timeClass(timeClass existingClass)
    {
        day = existingClass.day;
        month = existingClass.month;
        year = existingClass.year;
        tabDaysPerMonth = new int [12];
        for (int i = 0; i < 12; i++)
            tabDaysPerMonth[i] = existingClass.tabDaysPerMonth[i];
    }
    //! Set the date using day, month and year. 
    /*!
      \param aday, the day as an integer argument.
      \param amonth, the month as an integer argument.
      \param ayear, the year as an integer argument.
      \return true if the date has been correctly set
    */
    public bool setDate(int aday, int amonth, int ayear, bool checkDay = true)
    {
        if (((aday <= 0) || (aday > 31))&& checkDay)
        {
            GlobalVars.Instance.Error("Attempt to set day to <1 or >31");
            return false;
        }
        else
            day = aday;
        if ((aday <= 0) || (aday> 31))
        {
            GlobalVars.Instance.Error("Attempt to set day to <1 or >31");
            return false;
        }
        if ((amonth <= 0) || (amonth > 12))
        {
            GlobalVars.Instance.Error("Attempt to set month to <1 or >12");
            return false;
        }
        else
            month = amonth;
        if ((ayear <= 0) || (ayear > 2100))
        {
            GlobalVars.Instance.Error("Attempt to set year to <1 or >2100");
            return false;
        }
        else
            year = ayear;
        return true;
    }
    //! Get date as a long integer of days since year 0.
    public long getLongTime()
    {
        long longTime = 365*(year-1);  // no leap years here!
        for (int i = 0; i < month-1; i++)
        {
            longTime += tabDaysPerMonth[i];
        }
        longTime += day;
        return longTime;
    }
    //! Get date as Julian day.
    /*!
      \return Julian day as an integer value.
    */
    public int getJulianDay()
    {
        int JulianDay = 0;
        for (int i = 0; i < month - 1; i++)
        {
            JulianDay += tabDaysPerMonth[i];
        }
        JulianDay += day;
        return JulianDay;
    }
    //! Get the year. 
    /*!
      \return year as an integer value.
    */
    public int GetYear() { return year; }
    //! Get the day. 
    /*!
      \return day as an integer value.
    */
    public int GetDay() { return day; }
    //! Get the month. 
    /*!
      \return month as an integer value.
    */
    public int GetMonth() { return month; }
    //! Set the day. 
    /*!
      \param aVal, day an integer argument.
    */
    public void SetDay(int aVal)
    {
        if ((aVal <= 0)||(aVal>31))
            GlobalVars.Instance.Error("Attempt to set day to <1 or >31");
        day = aVal;
    }
    //! Set the month. 
    /*!
      \param aVal, month an integer argument.
    */
    public void SetMonth(int aVal)
    {
        if ((aVal <= 0) || (aVal > 12))
            GlobalVars.Instance.Error("Attempt to set month to <1 or >12");
        month = aVal;
    }
    //! Set the Year. 
    /*!
      \param aVal, year an integer argument.
    */
    public void SetYear(int aVal)
    {
        year = aVal;
    }
    //! Increment date by one day. 
    public void incrementOneDay()
    {
        if (day + 1 > tabDaysPerMonth[month-1])
        {
            day = 1;
            month++;
        }
        else
            day++;
        if (month > 12)
        {
            year++;
            month = 1;
        }
    }
    //! Get the number of days in a month.
    /*!
      \param amonth as an integer argument.
      \return days in month as an integer value.
    */
    public int GetDaysInMonth(int amonth)
    {
        return tabDaysPerMonth[amonth-1];
    }
    //! Write day, month and year.
    public void Write()
    {
        GlobalVars.Instance.writeInformationToFiles("day", "Day", "-", day, parens);
        GlobalVars.Instance.writeInformationToFiles("month", "Month", "-", month, parens);
        GlobalVars.Instance.writeInformationToFiles("year", "Year", "-", year, parens);
    }
}
