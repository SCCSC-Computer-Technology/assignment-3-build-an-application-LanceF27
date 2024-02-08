using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace David_Hudgins_CPT_206_Lab2
{
    internal class SQLController
    {
        stateDataClassesDataContext db = new stateDataClassesDataContext();

        public void sortAsc(String fieldName, DataGridView gridName)
        {
            gridName.Sort(gridName.Columns[fieldName], ListSortDirection.Ascending);
        }

        public void sortDesc(String fieldName, DataGridView gridName)
        {

            if (fieldName == "postalCode") { fieldName = "POSTAL_CODE"; }

            gridName.Sort(gridName.Columns[fieldName], ListSortDirection.Descending);
        }

        public void queryStateInfo(String stateName, DataGridView gridName)
        {
            var results = from STATE in db.STATEs
                          where STATE.STATE_NAME == stateName
                          select STATE;

            gridName.DataSource = results;
        }

        public void queryByFieldText(String fieldName, String conditionName, String searchString, DataGridView gridName)
        {
            if (fieldName == "POSTAL_CODE" && conditionName == "contains")   //Postal Code + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.POSTAL_CODE.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "POSTAL_CODE" && conditionName == "startsWith") //Postal Code + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.POSTAL_CODE.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "STATE_NAME" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_NAME.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "STATE_NAME" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_NAME.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "CAPITAL" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.CAPITAL.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "CAPITAL" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.CAPITAL.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "STATE_FLOWER" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_FLOWER.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "STATE_FLOWER" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_FLOWER.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "STATE_BIRD" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_BIRD.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "STATE_BIRD" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_BIRD.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "STATE_COLORS" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_COLORS.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "STATE_COLORS" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.STATE_COLORS.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "FIRST_LARGEST_CITY" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.FIRST_LARGEST_CITY.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "FIRST_LARGEST_CITY" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.FIRST_LARGEST_CITY.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "SECOND_LARGEST_CITY" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.SECOND_LARGEST_CITY.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "SECOND_LARGEST_CITY" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.SECOND_LARGEST_CITY.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "THIRD_LARGEST_CITY" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.THIRD_LARGEST_CITY.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "THIRD_LARGEST_CITY" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.THIRD_LARGEST_CITY.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "FLAG_DESC" && conditionName == "contains")   //State Name + Contains
            {
                var results = from STATE in db.STATEs
                              where STATE.FLAG_DESC.Contains(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "FLAG_DESC" && conditionName == "startsWith") //State Name + Starts With
            {
                var results = from STATE in db.STATEs
                              where STATE.FLAG_DESC.StartsWith(searchString)
                              select STATE;

                gridName.DataSource = results;
            }
        }

        public void queryFieldNumeric(String fieldName, String conditionName, String searchString, String searchStringLow, String searchStringHigh, DataGridView gridName)
        {
            if (fieldName == "POPULATION" && conditionName == "greaterThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.POPULATION) > Convert.ToInt32(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "POPULATION" && conditionName == "lessThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.POPULATION) < Convert.ToInt32(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "POPULATION" && conditionName == "betweenValues" )
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.POPULATION) < Convert.ToInt32(searchStringHigh)
                              where Convert.ToInt32(STATE.POPULATION) > Convert.ToInt32(searchStringLow)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "MEDIAN_INCOME" && conditionName == "greaterThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.MEDIAN_INCOME) > Convert.ToInt32(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "MEDIAN_INCOME" && conditionName == "lessThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.MEDIAN_INCOME) < Convert.ToInt32(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "MEDIAN_INCOME" && conditionName == "betweenValues")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToInt32(STATE.MEDIAN_INCOME) < Convert.ToInt32(searchStringHigh)
                              where Convert.ToInt32(STATE.MEDIAN_INCOME) > Convert.ToInt32(searchStringLow)
                              select STATE;

                gridName.DataSource = results;
            }

            ///

            if (fieldName == "TECH_JOBS" && conditionName == "greaterThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToDecimal(STATE.TECH_JOBS) > Convert.ToDecimal(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "TECH_JOBS" && conditionName == "lessThan")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToDecimal(STATE.TECH_JOBS) < Convert.ToDecimal(searchString)
                              select STATE;

                gridName.DataSource = results;
            }

            if (fieldName == "TECH_JOBS" && conditionName == "betweenValues")
            {
                var results = from STATE in db.STATEs
                              where Convert.ToDecimal(STATE.TECH_JOBS) < Convert.ToDecimal(searchStringHigh)
                              where Convert.ToDecimal(STATE.TECH_JOBS) > Convert.ToDecimal(searchStringLow)
                              select STATE;

                gridName.DataSource = results;
            }
        }
    }
}
