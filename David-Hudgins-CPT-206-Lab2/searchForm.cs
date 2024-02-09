//David Hudgins
//CPT-206-A01S
//Lab2-State-Info

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace David_Hudgins_CPT_206_Lab2
{
    public partial class searchForm : Form
    {
        String activePanel = "state";
        String fieldName = "";
        bool fieldIsChecked = false;
        bool conditionIsChecked = false;

        SQLController mySQLController = new SQLController();

        public searchForm()
        {
            InitializeComponent();

            buttonSortAsc.Click += delegate(object sender, EventArgs e) { buttonSortAsc_Click(sender, e, fieldName); };
            buttonSortDesc.Click += delegate (object sender, EventArgs e) { buttonSortDesc_Click(sender, e, fieldName); };

            radioButtonPostalCode.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);  //Events to check if radio button status has changed
            radioButtonStateName.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonCapital.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonStateFlower.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonStateBird.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonStateColors.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonLargestCity1.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonLargestCity2.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonLargestCity3.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonFlagDescription.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);

            radioButtonPopulation.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonMedianIncome.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonTechJobs.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);

            radioButtonContains.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonStartsWith.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonGreaterThan.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonLessThan.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            radioButtonBetweenValues.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
        }

        private void searchForm_Load(object sender, EventArgs e)
        {
            this.sTATETableAdapter.Fill(this.stateInformationDataSet.STATE);

            fillComboBox();

            panelSearchByState.BringToFront();
            activePanel = "state";

            panelTextConditions.Visible = false;
            panelNumberConditions.Visible = false;
        }

        private void clearAllFields() //Make all text fields null and read-only
        {
            comboBoxSelectState.SelectedItem = null;

            textBoxContains.Text = null;

            textBoxStartsWith.Text = null;

            textBoxGreaterThan.Text = null;

            textBoxLessThan.Text = null;

            textBoxValueLow.Text = null;

            textBoxValueHigh.Text = null;

            foreach (RadioButton radbutt in panelFieldSelect.Controls)
            {
                radbutt.Checked = false;
            }

            radioButtonContains.Checked = false;
            radioButtonStartsWith.Checked = false;
            radioButtonGreaterThan.Checked = false;
            radioButtonLessThan.Checked = false;
            radioButtonBetweenValues.Checked = false;

            fieldIsChecked = false;
            conditionIsChecked = false;

        }

        public void fillComboBox()//Populate ComboBox with state names from db
        {
            String[] stateNames = new String[50];

            stateDataClassesDataContext db = new stateDataClassesDataContext();

            foreach (STATE c in db.STATEs)
            {
                comboBoxSelectState.Items.Add(c.STATE_NAME.ToString());
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSearchByField_Click(object sender, EventArgs e)
        {
            panelSearchByField.BringToFront(); //This puts the panel in focus and reactivates it
            activePanel = "field";

            comboBoxSelectState.SelectedItem = null;
        }

        private void buttonSearchByState_Click(object sender, EventArgs e)
        {
            panelSearchByState.BringToFront();
            activePanel = "state";
            fieldIsChecked = false;
            conditionIsChecked = false;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clearAllFields();
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e) //This is for making the condition panels appear/disappear
        {
            RadioButton radioButton = sender as RadioButton; //Get ID of control

            if (radioButtonPostalCode.Checked || radioButtonStateName.Checked || radioButtonCapital.Checked || radioButtonStateFlower.Checked || radioButtonStateBird.Checked
                || radioButtonStateColors.Checked || radioButtonLargestCity1.Checked || radioButtonLargestCity2.Checked || radioButtonLargestCity3.Checked || radioButtonFlagDescription.Checked )
            {
                panelTextConditions.Visible = true;
                panelTextConditions.BringToFront();

                panelNumberConditions.Visible = false;

                fieldIsChecked = true;
            }

            if (radioButtonPopulation.Checked || radioButtonMedianIncome.Checked || radioButtonTechJobs.Checked) //These are the numeric fields
            {
                panelNumberConditions.Visible = true;
                panelNumberConditions.BringToFront();

                panelTextConditions.Visible = false;

                fieldIsChecked = true;
            }

            if (radioButtonContains.Checked || radioButtonStartsWith.Checked || radioButtonGreaterThan.Checked || radioButtonLessThan.Checked || radioButtonBetweenValues.Checked)
            {
                conditionIsChecked = true;
            }
        }

        //Search Button
        //Clicking search checks all that follows: Both a field and condition radio button are checked; All text fields are not null
        //Have to parse for numeric fields. Have to validate input. Load grid display panel containing datagrid with correct query

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var conditionName = "";
            var searchString = "";
            var searchStringLow = "";
            var searchStringHigh = "";

            if (activePanel == "state") //If on state screen
            {
                if (comboBoxSelectState.Text != "")
                {
                    var stateName = comboBoxSelectState.Text;
                    mySQLController.queryStateInfo(stateName, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }
                else
                {
                    MessageBox.Show("No State Selected");
                    return;
                }
            }

            if (activePanel == "field") //If on field screen
            {
                if (fieldIsChecked == false )
                {
                    MessageBox.Show("Must Select a Field");
                    return;
                }
                if (conditionIsChecked == false)
                {
                    MessageBox.Show("Must Select a Condition");
                    return;
                }

                /////////////////////////////////////////////////////////////////////////////////////   Input Validation for empty fields

                if (radioButtonContains.Checked)
                {
                    if (textBoxContains.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for contains");
                        return;
                    }
                    conditionName = "contains";
                    searchString = textBoxContains.Text;
                }
                if (radioButtonStartsWith.Checked)
                {
                    if (textBoxStartsWith.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for starts with");
                        return;
                    }
                    conditionName = "startsWith";
                    searchString = textBoxStartsWith.Text;
                }

                /////////////////////////////////////////////////////////////////////////////////////   Input Validation for empty fields and parse

                if (radioButtonPopulation.Checked && radioButtonGreaterThan.Checked)
                {
                    if (textBoxGreaterThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for greater than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxGreaterThan.Text.Replace(",",""));
                        conditionName = "greaterThan";
                        searchString  = textBoxGreaterThan.Text.Replace(",", "");


                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonPopulation.Checked && radioButtonLessThan.Checked)
                {
                    if (textBoxLessThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for less than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxLessThan.Text.Replace(",", ""));
                        conditionName = "lessThan";
                        searchString = textBoxLessThan.Text.Replace(",", "");


                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonPopulation.Checked && radioButtonBetweenValues.Checked)
                {
                    if (textBoxValueLow.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for Low End of Range");
                        return;
                    }
                    if (textBoxValueHigh.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for High End of Range");
                        return;
                    }

                    try
                    {
                        var parseLow = textBoxValueLow.Text.Replace(",", "");
                        parseLow = parseLow.Replace("%","");

                        var parseHigh = textBoxValueHigh.Text.Replace(",", "");
                        parseHigh = parseHigh.Replace("%", "");

                        int.Parse(parseLow);
                        int.Parse(parseHigh);

                        conditionName = "betweenValues";

                        searchStringLow = parseLow;
                        searchStringHigh = parseHigh;
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter Numbers");
                        return;
                    }
                }

                ///

                if (radioButtonMedianIncome.Checked && radioButtonGreaterThan.Checked)
                {
                    if (textBoxGreaterThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for greater than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxGreaterThan.Text.Replace(",", ""));
                        conditionName = "greaterThan";
                        searchString = textBoxGreaterThan.Text.Replace(",", "");
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonMedianIncome.Checked && radioButtonLessThan.Checked)
                {
                    if (textBoxLessThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for less than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxLessThan.Text.Replace(",", ""));
                        conditionName = "lessThan";
                        searchString = textBoxLessThan.Text.Replace(",", "");
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonMedianIncome.Checked && radioButtonBetweenValues.Checked)
                {
                    if (textBoxValueLow.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for Low End of Range");
                        return;
                    }
                    if (textBoxValueHigh.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for High End of Range");
                        return;
                    }

                    try
                    {
                        var parseLow = textBoxValueLow.Text.Replace(",", "");
                        parseLow = parseLow.Replace("%", "");

                        var parseHigh = textBoxValueHigh.Text.Replace(",", "");
                        parseHigh = parseHigh.Replace("%", "");

                        int.Parse(parseLow);
                        int.Parse(parseHigh);

                        conditionName = "betweenValues";

                        searchStringLow = parseLow;
                        searchStringHigh = parseHigh;
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter Numbers");
                        return;
                    }
                }

                ///

                if (radioButtonTechJobs.Checked && radioButtonGreaterThan.Checked)
                {
                    if (textBoxGreaterThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for greater than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxGreaterThan.Text.Replace(",", ""));
                        conditionName = "greaterThan";
                        searchString = textBoxGreaterThan.Text.Replace(",", "");
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonTechJobs.Checked && radioButtonLessThan.Checked)
                {
                    if (textBoxLessThan.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for less than");
                        return;
                    }
                    try
                    {
                        int.Parse(textBoxLessThan.Text.Replace(",", ""));
                        conditionName = "lessThan";
                        searchString = textBoxLessThan.Text.Replace(",", "");
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter A Number");
                        return;
                    }
                }

                if (radioButtonTechJobs.Checked && radioButtonBetweenValues.Checked)
                {
                    if (textBoxValueLow.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for Low End of Range");
                        return;
                    }
                    if (textBoxValueHigh.Text == "")
                    {
                        MessageBox.Show("Must Enter A Value for High End of Range");
                        return;
                    }

                    try
                    {
                        var parseLow = textBoxValueLow.Text.Replace(",", "");
                        parseLow = parseLow.Replace("%", "");

                        var parseHigh = textBoxValueHigh.Text.Replace(",", "");
                        parseHigh = parseHigh.Replace("%", "");

                        int.Parse(parseLow);
                        int.Parse(parseHigh);

                        conditionName = "betweenValues";

                        searchStringLow = parseLow;
                        searchStringHigh = parseHigh;
                    }
                    catch
                    {
                        MessageBox.Show("Must Enter Numbers");
                        return;
                    }
                }

                /////////////////////////////////////////////////////////////////////////////////////   Call the SQL Methods

                if (radioButtonPostalCode.Checked == true)
                {
                    fieldName = "POSTAL_CODE";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonStateName.Checked == true)
                {
                    fieldName = "STATE_NAME";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonCapital.Checked == true)
                {
                    fieldName = "CAPITAL";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonStateFlower.Checked == true)
                {
                    fieldName = "STATE_FLOWER";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonStateBird.Checked == true)
                {
                    fieldName = "STATE_BIRD";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonStateColors.Checked == true)
                {
                    fieldName = "STATE_COLORS";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonLargestCity1.Checked == true)
                {
                    fieldName = "FIRST_LARGEST_CITY";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonLargestCity2.Checked == true)
                {
                    fieldName = "SECOND_LARGEST_CITY";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonLargestCity3.Checked == true)
                {
                    fieldName = "THIRD_LARGEST_CITY";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                ///

                if (radioButtonFlagDescription.Checked == true)
                {
                    fieldName = "FLAG_DESC";

                    mySQLController.queryByFieldText(fieldName, conditionName, searchString, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (radioButtonPopulation.Checked == true)
                {
                    fieldName = "POPULATION";

                    mySQLController.queryFieldNumeric(fieldName, conditionName, searchString, searchStringLow, searchStringHigh, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                if (radioButtonMedianIncome.Checked == true)
                {
                    fieldName = "MEDIAN_INCOME";

                    mySQLController.queryFieldNumeric(fieldName, conditionName, searchString, searchStringLow, searchStringHigh, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

                if (radioButtonTechJobs.Checked == true)
                {
                    fieldName = "TECH_JOBS";

                    mySQLController.queryFieldNumeric(fieldName, conditionName, searchString, searchStringLow, searchStringHigh, dataGridViewDisplay);

                    panelDatabaseDisplay.BringToFront();
                    activePanel = "database";
                }

            }
        }
        private void buttonSortAsc_Click(object sender, EventArgs e, String fieldName)
        {
            mySQLController.sortAsc(fieldName, dataGridViewDisplay);
        }

        private void buttonSortDesc_Click(object sender, EventArgs e, String fieldName)
        {

            mySQLController.sortDesc(fieldName, dataGridViewDisplay);
        }
    }
}
