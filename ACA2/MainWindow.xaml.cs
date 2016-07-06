using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using MessageBox = System.Windows.MessageBox;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ACA2
{
    enum Tab { Home, Upload, Select, Plan, Employer, Employee, Reports, Settings, Login }

    class SelectedCompany
    {
        public static string Id { get; set; }
        public static string TaxId { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ContentControl prevSelect = null;
        Color errorColor = (Color)ColorConverter.ConvertFromString("#FFBABA");
        Color goodColor = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        ObservableCollection<Employee> employees;
        public ICollectionView employeesView { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            loginUser();
            InitializeComboboxes();
        }

        private void InitializeComboboxes()
        {
            filingYear.DataContext = StaticContent.filingyear;
            state.DataContext = StaticContent.states;
            eeState.DataContext = StaticContent.states;
            country.DataContext = StaticContent.country;
            eeCountry.DataContext = StaticContent.country;
            formType.DataContext = StaticContent.formtype;
            originCode.DataContext = StaticContent.origincode;
            fundType.DataContext = StaticContent.fundtype;
            planType.DataContext = StaticContent.plantype;
            waitingPeriod.DataContext = StaticContent.waitingperiod;
            offeredSpouse.DataContext = StaticContent.yes_no;
            offeredDependent.DataContext = StaticContent.yes_no;
            termTermination.DataContext = StaticContent.yes_no;
            renewalMonth.DataContext = StaticContent.renewalmonth;
            minimumValue.DataContext = StaticContent.yes_no;
            bandingType.DataContext = StaticContent.bandingtype;
        }

        private void loginUser()
        {//
            mainControl.SelectedIndex = (int)Tab.Login;

            //disable all the button in the navigation bar
            uploadButton.IsEnabled = false;
            selectCompanyButton.IsEnabled = false;
            editPlanButton.IsEnabled = false;
            editEmployerButton.IsEnabled = false;
            editEmployeeButton.IsEnabled = false;
            printReportButton.IsEnabled = false;
            settingsButton.IsEnabled = false;
            HomeButton.IsEnabled = false;
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            //if login is successfull go to the select company tab
            //and enable the upload and select buttons
            mainControl.SelectedIndex = 2;
            selectCompanyTab();
            uploadButton.IsEnabled = true;
            selectCompanyButton.IsEnabled = true;
        }

        private void setSelected(ContentControl control)
        {
            //unselect previous button
            if(prevSelect != null)
            {
                var prevControl = (Border)prevSelect.Template.FindName("PressedBorder", prevSelect);
                prevControl.BorderBrush = Brushes.Transparent;
            }

            //select new button
            var myControl = (Border)control.Template.FindName("PressedBorder", control);
            myControl.BorderBrush = Brushes.Red;
            prevSelect = control;
        }

        private int convertStringtoInt(string num)
        {
            return Convert.ToInt32(Convert.ToDecimal(num));
        }

        private string convertMonthtoInt(string mon)
        {
            Double n;

            if (!Double.TryParse(mon, out n))
            {
                n = StaticContent.months.IndexOf(mon) + 1;
            }

            return n.ToString();
        }

        #region(Navigation Bar Button Events)
        private void HomeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Home;
        }

        private void uploadButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Upload;
        }

        private void selectCompanyButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            selectCompanyTab();
            mainControl.SelectedIndex = (int)Tab.Select;
        }

        private void editPlanButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Plan;
        }

        private void editEmployerButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Employer;
        }

        private void editEmployeeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Employee;
        }

        private void printReportButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Reports;
        }

        private void settingsButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            setSelected((ContentControl)sender);
            mainControl.SelectedIndex = (int)Tab.Settings;
        }

        //private void admin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    setSelected((ContentControl)sender);
        //    mainControl.SelectedIndex = 8;
        //}
        #endregion

        #region(Upload Window Options)

        #endregion

        #region(Select Company Tab)
        private ObservableCollection<CompanyData> Items = new ObservableCollection<CompanyData>();
        public ICollectionView ItemsView { get; set; }
        private string _filterText;

        private void selectCompanyTab()
        {

            if (Items.Count < 1)
            {
                //loop through the data and create an companyData object for each record
                foreach (DataRow row in DatabaseOPS.GetCompany().Rows)
                {
                    Items.Add(new CompanyData() { id = row["id"].ToString(), taxid = row["taxid"].ToString(), name = row["name"].ToString(), totalNumberEE = row["totalNumberEE"].ToString(), lastEdit = row["lastEdit"].ToString() });
                }
            }
            //display data
            ItemsView = CollectionViewSource.GetDefaultView(Items);
            DataContext = this;
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                ItemsView.Filter = DoFilter;
                OnPropertyChanged();
            }
        }

        private bool DoFilter(object obj)
        {
            var companyData = obj as CompanyData;
            if (companyData != null)
            {
                return companyData.Filter(FilterText);
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void companiesDataGrid_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow) sender;

            if (dgr != null)
            {
                // sets the id of the selected employer to the static Id from SelectedCompany class
                CompanyData cd = dgr.Item as CompanyData;

                SelectedCompany.Id = cd.id;
                SelectedCompany.TaxId = cd.taxid;

                // go to Home Tab
                // enable the remaining buttons if still disabled
                HomeButton.IsEnabled = true;
                editPlanButton.IsEnabled = true;
                editEmployerButton.IsEnabled = true;
                editEmployeeButton.IsEnabled = true;
                printReportButton.IsEnabled = true;
                settingsButton.IsEnabled = true;
                setSelected(HomeButton);

                LoadHome();
                LoadEmployer();
                LoadPlan();
                LoadEmployee();
                mainControl.SelectedIndex = (int) Tab.Home;
            }
            else
            {
                MessageBox.Show(@"Error selecting company.");
            }

            //to indicate that the event has been handleded. 
            e.Handled = true;
        }


        private void LoadHome()
        {
            //@empid is set inside the StoredProcedure in the database
            //SelectedCompany.Id is set in the Select tab when the user doubleclicks on a row
            SqlParameter paramater = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);

            //this should always return one row
            //passing the name of the StoredProcedure from the Database and the one paramater for it
            DataRow dr = DatabaseOPS.GetCompany(paramater).Rows[0];

            // setting the label content values equal to the values from the data row selected from the database
            employerNameLabel.Content = dr["name"].ToString();
            filingYearLabel.Content = dr["filingYear"].ToString();
            lastUploadDateLabel.Content = dr["lastEdit"].ToString();
            lastModifiedLabel.Content = dr["lastUploadDate"].ToString();
        }

        private void LoadEmployer()
        {
            //load all employers from the database with the the selected employer tax id
            SqlParameter sqlParam = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);
            DataTable dt = DatabaseOPS.GetEmployer(sqlParam);
            //check if any employers are found
            if (dt.Rows.Count > 0)
            {
                employerDataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void LoadPlan()
        {
            //load all employers from the database with the the selected employer tax id
            SqlParameter sqlParam = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);
            DataTable dt = DatabaseOPS.GetPlan(sqlParam);
            //check if any employers are found
            if (dt.Rows.Count > 0)
            {
                planDataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void LoadEmployee()
        {

            employees = null;
            employees = new ObservableCollection<Employee>();
            HireCollection hireCollection = new HireCollection();
            StatusCollection statusCollection = new StatusCollection();
            CoverageCollection coverageCollection = new CoverageCollection();

            DataTable employeesTable = DatabaseOPS.GetEmployee(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId));
            hireCollection.setData(DatabaseOPS.GetHireSpan(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId)));
            statusCollection.setData(DatabaseOPS.GetStatus(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId)));
            coverageCollection.setData(DatabaseOPS.GetEnrollment(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId)));

            foreach (DataRow row in employeesTable.Rows)
            {
                employees.Add(new Employee()
                {
                    id = row["id"].ToString(),
                    employeeCodeId = row["employeeCodeId"].ToString(),
                    employerName = null,
                    firstName = row["firstName"].ToString(),
                    middleName = row["middleName"].ToString(),
                    lastName = row["lastName"].ToString(),
                    ssn = row["ssn"].ToString(),
                    dateOfBirth = row["birthday"].ToString(),
                    emailAddress = row["email"].ToString(),
                    address = row["address"].ToString(),
                    address2 = row["address2"].ToString(),
                    city = row["city"].ToString(),
                    state = row["state"].ToString(),
                    zipCode = row["zip"].ToString(),
                    country = row["country"].ToString(),
                    sc = statusCollection,
                    cc = coverageCollection,
                    hc = hireCollection
                });
            }

            LoadProgress prog = new LoadProgress();

            prog.loadProgress.Maximum = employees.Count * 3;

            Thread hire = new Thread(() =>
            {
                foreach (Employee ee in employees)
                {
                    ee.setHireInformation();
                    prog.loadProgress.Dispatcher.Invoke(new Action(() => prog.loadProgress.Value += 1));
                }
            });

            hire.Start();

            Thread status = new Thread(() =>
            {
                foreach (Employee ee in employees)
                {
                    ee.setStatusInformation();
                    prog.loadProgress.Dispatcher.Invoke(new Action(() => prog.loadProgress.Value += 1));
                }
            });

            status.Start();

            Thread coverage = new Thread(() =>
            {
                foreach (Employee ee in employees)
                {
                    ee.setCoverageInformation();
                    prog.loadProgress.Dispatcher.Invoke(new Action(() => prog.loadProgress.Value += 1));
     
                }
            });

            coverage.Start();

            prog.ShowDialog();         
            
            employeesView = CollectionViewSource.GetDefaultView(employees);
            employeeDataGrid.ItemsSource = employeesView;

        }
        #endregion

        #region(Employer)
        int employerSaveMode = 0;

        private void clearEmployerForm()
        {
            //this method sets the value of every textbox, combobox, and checkbox to null
            //in the employer tab
            employerName.Text = null;
            ein.Text = null;
            filingYear.Text = null;
            address.Text = null;
            address2.Text = null;
            city.Text = null;
            state.Text = null;
            zipCode.Text = null;
            country.Text = null;
            contactName.Text = null;
            contactPhone.Text = null;
            title.Text = null;
            formType.Text = null;
            originCode.Text = null;
            shopIdentifier.Text = null;
            OfferMethod.IsChecked = false;
            OfferMethodRelief.IsChecked = false;
            Section4980H.IsChecked = false;
            OfferMethod98.IsChecked = false;
            DataTable part3Table = createDataTable();
            DataRow row;

            //create and add rows
            for (int i = 0; i < 13; i++)
            {
                row = part3Table.NewRow();
                if (i != 0)
                {
                    row["month"] = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);
                }
                else
                {
                    row["month"] = "All 12 Months";
                }

                row["minimum"] = "0";
                row["full"] = DBNull.Value;
                row["total"] = DBNull.Value;
                row["aggregate"] = "0";
                row["section"] = DBNull.Value;
                part3Table.Rows.Add(row);
            }
            ALEMemberInformation.ItemsSource = part3Table.DefaultView;
        }

        private bool employerInputDataValidation()
        {
            bool error = false;

            if (string.IsNullOrEmpty(employerName.Text))
            {
                employerName.Background = new SolidColorBrush(errorColor);
                error = true;
            }
            else
            {
                employerName.Background = new SolidColorBrush(goodColor);
            }

            if(string.IsNullOrEmpty(ein.Text))
            {
                ein.Background = new SolidColorBrush(errorColor);
                error = true;
            }
            else
            {
                ein.Background = new SolidColorBrush(goodColor);
            }


            return error;       
        }

        private void unhighlightEmployerFields()
        {
            Color goodColor = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            employerName.Background = new SolidColorBrush(goodColor);
            ein.Background = new SolidColorBrush(goodColor);
        }

        private DataTable createDataTable()
        {
            //create the data table for the datagrid. 
            DataTable part3Table = new DataTable();
            DataColumn column;

            //create columns
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "month";
            part3Table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.AllowDBNull = true;
            column.ColumnName = "minimum";
            part3Table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.AllowDBNull = true;
            column.ColumnName = "full";
            part3Table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.AllowDBNull = true;
            column.ColumnName = "total";
            part3Table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.AllowDBNull = true;
            column.ColumnName = "aggregate";
            part3Table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "section";
            part3Table.Columns.Add(column);

            return part3Table;
        }

        private void employerDataGrid_Row_Selected(object sender, EventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            DataRowView drv = employerDataGrid.Items.GetItemAt(dgr.GetIndex()) as DataRowView;

            if (drv != null)
            {
                //get the employer tax id
                string companyTaxId = drv["CompanyTaxID"].ToString();
                string employerTaxId = drv["EmployerTaxID"].ToString();

                //this should always return one row
                //passing the name of the StoredProcedure from the Database and two paramater
                DataRow dr = DatabaseOPS.GetEmployer(new SqlParameter("@CompanyTaxID", companyTaxId), new SqlParameter("@EmployerTaxID", employerTaxId)).Rows[0];

                // setting the label content values equal to the values from the data row selected from the database
                employerName.Text = dr["name"].ToString();
                ein.Text = dr["EmployerTaxID"].ToString().PadLeft(9, '0');
                filingYear.Text = dr["filingYear"].ToString();
                address.Text = dr["address"].ToString();
                address2.Text = dr["address2"].ToString();
                city.Text = dr["city"].ToString();
                state.Text = dr["state"].ToString();
                zipCode.Text = dr["zip"].ToString();
                country.Text = dr["country"].ToString();
                contactName.Text = dr["contactName"].ToString();
                contactPhone.Text = dr["phoneNumber"].ToString();
                title.Text = dr["signTitle"].ToString();
                formType.Text = dr["formType"].ToString();
                originCode.Text = dr["originCode"].ToString();
                shopIdentifier.Text = dr["SHOPIdentifier"].ToString();
                OfferMethod.IsChecked = Convert.ToBoolean(string.IsNullOrWhiteSpace(dr["eligibility_A"].ToString())?0: dr["eligibility_A"]);
                OfferMethodRelief.IsChecked = Convert.ToBoolean(string.IsNullOrWhiteSpace(dr["eligibility_B"].ToString()) ? 0 : dr["eligibility_B"]);
                Section4980H.IsChecked = Convert.ToBoolean(string.IsNullOrWhiteSpace(dr["eligibility_C"].ToString()) ? 0 : dr["eligibility_C"]);
                OfferMethod98.IsChecked = Convert.ToBoolean(string.IsNullOrWhiteSpace(dr["eligibility_D"].ToString()) ? 0 : dr["eligibility_D"]);

                //create the data table for the datagrid. 
                DataTable part3Table = createDataTable();
                DataRow row;

                //create and add rows
                for (int i = 0; i < 13; i++)
                {
                    row = part3Table.NewRow();
                    if (i != 0)
                    {
                        row["month"] = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);
                    }
                    else
                    {
                        row["month"] = "All 12 Months";
                    }

                    row["minimum"] = string.IsNullOrEmpty(dr[string.Join("_", "minimum", i.ToString())].ToString());
                    row["full"] = dr[string.Join("_", "fullTime", i.ToString())];
                    row["total"] = dr[string.Join("_", "total", i.ToString())];
                    row["aggregate"] = string.IsNullOrEmpty(dr[string.Join("_", "group", i.ToString())].ToString());
                    row["section"] = dr[string.Join("_", "S4980H", i.ToString())];
                    part3Table.Rows.Add(row);
                }

                //display table
                ALEMemberInformation.ItemsSource = new DataView(part3Table);

                //enable the tab control so the user can make changes
                employerTabControl.IsEnabled = true;

                //enable the save button so the user can save changes
                saveEmployer.IsEnabled = true;

                //rest textbox background color to white
                unhighlightEmployerFields();
                employerSaveMode = 0;

            }
            
        }

        private void saveEmployer_Click(object sender, RoutedEventArgs e)
        {
            //check if the required fields are complete (employer name and ein)
            if (!employerInputDataValidation())
            {
                //check if address is foreign(outside the US)
                int foreignAddress;
                if (!string.IsNullOrEmpty(country.Text))
                {
                    if (country.Text.Equals("United States of America"))
                    {
                        foreignAddress = 0;
                    }
                    else
                    {
                        foreignAddress = 1;
                    }
                }
                else
                {
                    foreignAddress = 1;
                }

                //if employerSaveMode 1 then it is a new Employer
                //if employerSaveMode 0 then it is a existant Employer
                if (employerSaveMode == 1)
                {
                    DatabaseOPS.AddEmployer(getEmployerParams(foreignAddress));
                    LoadEmployer();
                }
                else
                {
                    DatabaseOPS.EditEmployer(getEmployerParams(foreignAddress));
                }
            }
        }

        private SqlParameter[] getEmployerParams(int foreignAddress)
        {
            DataRowView drv = (DataRowView)employerDataGrid.SelectedItems[0];

            DataTable aleMonthly = ((DataView)ALEMemberInformation.ItemsSource).ToTable();
            SqlParameter[] param = new SqlParameter[97];
            #region(params)
            param[0] = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);
            param[1] = new SqlParameter("@EmployerTaxID", ein.Text);
            param[2] = new SqlParameter("@name", employerName.Text);
            param[3] = new SqlParameter("@isForeignAddress", foreignAddress);
            param[4] = new SqlParameter("@address", address.Text);
            param[5] = new SqlParameter("@address2", address2.Text);
            param[6] = new SqlParameter("@city", city.Text);
            param[7] = new SqlParameter("@state", state.Text);
            param[8] = new SqlParameter("@zip", zipCode.Text);
            param[9] = new SqlParameter("@country", country.Text);
            param[10] = new SqlParameter("@phoneNumber", contactPhone.Text);
            param[11] = new SqlParameter("@contactName", contactName.Text);
            param[12] = new SqlParameter("@signTitle", title.Text);
            param[13] = new SqlParameter("@signDate", null);
            param[14] = new SqlParameter("@originCode", originCode.Text);
            param[15] = new SqlParameter("@SHOPIdentifier", shopIdentifier.Text);
            param[16] = new SqlParameter("@totalNumberEE", 0);
            param[17] = new SqlParameter("@totalNumberForms", 0);
            param[18] = new SqlParameter("@eligibility_A", OfferMethod.IsChecked == true ? 1 : 0);
            param[19] = new SqlParameter("@eligibility_B", OfferMethodRelief.IsChecked == true ? 1 : 0);
            param[20] = new SqlParameter("@eligibility_C", Section4980H.IsChecked == true ? 1 : 0);
            param[21] = new SqlParameter("@eligibility_D", OfferMethod98.IsChecked == true ? 1 : 0);
            param[22] = new SqlParameter("@lockEmployer", 0);
            param[23] = new SqlParameter("@formType", formType.Text);
            param[24] = new SqlParameter("@isCompany", drv["isCompany"].ToString());
            param[25] = new SqlParameter("@status", null);
            param[26] = new SqlParameter("@teamId", null);
            param[27] = new SqlParameter("@lockCompany", 0);
            param[28] = new SqlParameter("@lastEdit", null);
            param[29] = new SqlParameter("@filingYear", filingYear.Text);
            param[30] = new SqlParameter("@lastUploadDate", null);
            param[31] = new SqlParameter("@minimum_0", aleMonthly.Rows[0][1]);
            param[32] = new SqlParameter("@minimum_1", aleMonthly.Rows[1][1]);
            param[33] = new SqlParameter("@minimum_2", aleMonthly.Rows[2][1]);
            param[34] = new SqlParameter("@minimum_3", aleMonthly.Rows[3][1]);
            param[35] = new SqlParameter("@minimum_4", aleMonthly.Rows[4][1]);
            param[36] = new SqlParameter("@minimum_5", aleMonthly.Rows[5][1]);
            param[37] = new SqlParameter("@minimum_6", aleMonthly.Rows[6][1]);
            param[38] = new SqlParameter("@minimum_7", aleMonthly.Rows[7][1]);
            param[39] = new SqlParameter("@minimum_8", aleMonthly.Rows[8][1]);
            param[40] = new SqlParameter("@minimum_9", aleMonthly.Rows[9][1]);
            param[41] = new SqlParameter("@minimum_10", aleMonthly.Rows[10][1]);
            param[42] = new SqlParameter("@minimum_11", aleMonthly.Rows[11][1]);
            param[43] = new SqlParameter("@minimum_12", aleMonthly.Rows[12][1]);
            param[44] = new SqlParameter("@fullTime_0", aleMonthly.Rows[0][2]);
            param[45] = new SqlParameter("@fullTime_1", aleMonthly.Rows[1][2]);
            param[46] = new SqlParameter("@fullTime_2", aleMonthly.Rows[2][2]);
            param[47] = new SqlParameter("@fullTime_3", aleMonthly.Rows[3][2]);
            param[48] = new SqlParameter("@fullTime_4", aleMonthly.Rows[4][2]);
            param[49] = new SqlParameter("@fullTime_5", aleMonthly.Rows[5][2]);
            param[50] = new SqlParameter("@fullTime_6", aleMonthly.Rows[6][2]);
            param[51] = new SqlParameter("@fullTime_7", aleMonthly.Rows[7][2]);
            param[52] = new SqlParameter("@fullTime_8", aleMonthly.Rows[8][2]);
            param[53] = new SqlParameter("@fullTime_9", aleMonthly.Rows[9][2]);
            param[54] = new SqlParameter("@fullTime_10", aleMonthly.Rows[10][2]);
            param[55] = new SqlParameter("@fullTime_11", aleMonthly.Rows[11][2]);
            param[56] = new SqlParameter("@fullTime_12", aleMonthly.Rows[12][2]);
            param[57] = new SqlParameter("@total_0", aleMonthly.Rows[0][3]);
            param[58] = new SqlParameter("@total_1", aleMonthly.Rows[1][3]);
            param[59] = new SqlParameter("@total_2", aleMonthly.Rows[2][3]);
            param[60] = new SqlParameter("@total_3", aleMonthly.Rows[3][3]);
            param[61] = new SqlParameter("@total_4", aleMonthly.Rows[4][3]);
            param[62] = new SqlParameter("@total_5", aleMonthly.Rows[5][3]);
            param[63] = new SqlParameter("@total_6", aleMonthly.Rows[6][3]);
            param[64] = new SqlParameter("@total_7", aleMonthly.Rows[7][3]);
            param[65] = new SqlParameter("@total_8", aleMonthly.Rows[8][3]);
            param[66] = new SqlParameter("@total_9", aleMonthly.Rows[9][3]);
            param[67] = new SqlParameter("@total_10", aleMonthly.Rows[10][3]);
            param[68] = new SqlParameter("@total_11", aleMonthly.Rows[11][3]);
            param[69] = new SqlParameter("@total_12", aleMonthly.Rows[12][3]);
            param[70] = new SqlParameter("@group_0", aleMonthly.Rows[0][4]);
            param[71] = new SqlParameter("@group_1", aleMonthly.Rows[1][4]);
            param[72] = new SqlParameter("@group_2", aleMonthly.Rows[2][4]);
            param[73] = new SqlParameter("@group_3", aleMonthly.Rows[3][4]);
            param[74] = new SqlParameter("@group_4", aleMonthly.Rows[4][4]);
            param[75] = new SqlParameter("@group_5", aleMonthly.Rows[5][4]);
            param[76] = new SqlParameter("@group_6", aleMonthly.Rows[6][4]);
            param[77] = new SqlParameter("@group_7", aleMonthly.Rows[7][4]);
            param[78] = new SqlParameter("@group_8", aleMonthly.Rows[8][4]);
            param[79] = new SqlParameter("@group_9", aleMonthly.Rows[9][4]);
            param[80] = new SqlParameter("@group_10", aleMonthly.Rows[10][4]);
            param[81] = new SqlParameter("@group_11", aleMonthly.Rows[11][4]);
            param[82] = new SqlParameter("@group_12", aleMonthly.Rows[12][4]);
            param[83] = new SqlParameter("@S4980H_0", aleMonthly.Rows[0][5]);
            param[84] = new SqlParameter("@S4980H_1", aleMonthly.Rows[1][5]);
            param[85] = new SqlParameter("@S4980H_2", aleMonthly.Rows[2][5]);
            param[86] = new SqlParameter("@S4980H_3", aleMonthly.Rows[3][5]);
            param[87] = new SqlParameter("@S4980H_4", aleMonthly.Rows[4][5]);
            param[88] = new SqlParameter("@S4980H_5", aleMonthly.Rows[5][5]);
            param[89] = new SqlParameter("@S4980H_6", aleMonthly.Rows[6][5]);
            param[90] = new SqlParameter("@S4980H_7", aleMonthly.Rows[7][5]);
            param[91] = new SqlParameter("@S4980H_8", aleMonthly.Rows[8][5]);
            param[92] = new SqlParameter("@S4980H_9", aleMonthly.Rows[9][5]);
            param[93] = new SqlParameter("@S4980H_10", aleMonthly.Rows[10][5]);
            param[94] = new SqlParameter("@S4980H_11", aleMonthly.Rows[11][5]);
            param[95] = new SqlParameter("@S4980H_12", aleMonthly.Rows[12][5]);
            param[96] = new SqlParameter("@disableChanges", Disable.IsChecked == true ? 1 : 0);
            #endregion

            return param;
        }

        private void cancelEmployer_Click(object sender, RoutedEventArgs e)
        {

            saveEmployer.IsEnabled = false;
            employerTabControl.IsEnabled = false;
            unhighlightEmployerFields();
            employerDataGrid.SelectedIndex = -1;
            employerSaveMode = 0;
        }

        private void addEmployer_Click(object sender, RoutedEventArgs e)
        {
            clearEmployerForm();
            employerSaveMode = 1;
            saveEmployer.IsEnabled = true;
            employerTabControl.IsEnabled = true;
            unhighlightEmployerFields();
        }

        private void deleteEmployer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rsult = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo,MessageBoxImage.Warning, MessageBoxResult.No);
            if (rsult == MessageBoxResult.Yes)
            {
                DataRowView drv = (DataRowView)employerDataGrid.SelectedItems[0];
                if (drv["isCompany"].ToString().Equals("1"))
                {
                    if (employerDataGrid.Items.Count == 1)
                        DatabaseOPS.DeleteCompany(new SqlParameter("@taxid", drv["CompanyTaxID"]));
                    else
                        MessageBox.Show("Cannot delete main employer before affiliated employers");
                }
                else
                {
                    DatabaseOPS.DeleteEmployer(new SqlParameter("@taxid", drv["EmployerTaxId"]));
                }
                LoadEmployer();
            }
        }

        private void ApplyUp_Click(object sender, RoutedEventArgs e)
        {
            int index = ALEMemberInformation.SelectedIndex;
            ALEMemberInformation.CommitEdit();

            DataTable aleMonthly = ((DataView)ALEMemberInformation.ItemsSource).ToTable();
            for (int i = index-1; i>=0; i--)
            {
                for(int j = 1; j<6; j++)
                {
                    aleMonthly.Rows[i][j] = aleMonthly.Rows[index][j];
                }
            }

            ALEMemberInformation.ItemsSource = new DataView(aleMonthly);
        }

        private void ApplyDown_Click(object sender, RoutedEventArgs e)
        {
            int index = ALEMemberInformation.SelectedIndex;
            ALEMemberInformation.CommitEdit();

            DataTable aleMonthly = ((DataView)ALEMemberInformation.ItemsSource).ToTable();
            for (int i = index + 1; i < 13; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    aleMonthly.Rows[i][j] = aleMonthly.Rows[index][j];
                }
            }

            ALEMemberInformation.ItemsSource = new DataView(aleMonthly);
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(System.Windows.Controls.DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }
        #endregion

        #region(Plan Tab)
        int planSaveMode = 0;

        private void clearPlanForm()
        {
            //this method sets the value of every textbox, combobox, and checkbox to null
            //in the Plan tab
            planName.Text = null;
            planType.Text = null;
            fundType.Text = null;
            waitingPeriod.Text = null;
            waitingDays.Text = null;
            offeredSpouse.Text = null;
            offeredDependent.Text = null;
            termTermination.Text = null;
            renewalMonth.Text = null;
            minimumValue.Text = null;
            bandingType.Text = null;

            //get datatable from the codes data grid
            DataTable codeSettings = ((DataView)codesDataGrid.ItemsSource).ToTable();

            //set the column values to false
            for (int i = 0; i < codeSettings.Rows.Count; i++)
            {
                codeSettings.Rows[i]["codeCheck1"] = false;
                codeSettings.Rows[i]["codeCheck2"] = false;
            }
            codesDataGrid.ItemsSource = codeSettings.DefaultView;

            //get datatable from the codes data grid
            DataTable premium = ((DataView)bandingValues.ItemsSource).ToTable();
            premium.Clear();
            bandingValues.ItemsSource = premium.DefaultView;
        }


        private DataTable fixPremiumName(DataTable dt)
        {
            string banType = bandingType.Text;

            //loop through the datatable to assign premium names
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //check if the banding name changed and if the banding name is not empty
                //to delete the old record from the database and re-insert a new record
                //with the correct name.
                if(!dt.Rows[i]["bandingName"].ToString().Equals(string.Concat(banType, i)) && !dt.Rows[i]["bandingName"].ToString().Equals(""))
                {
                    DatabaseOPS.DeletePremium(new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId),
                                              new SqlParameter("@name", planName.Text),
                                              new SqlParameter("@bandingName", dt.Rows[i]["bandingName"].ToString()));
                }
                //assign the new name
                dt.Rows[i]["bandingName"] = string.Concat(banType, i);
            }

            return dt;
        }

        private DataTable createCodesDataTable()
        {
            //create the data table for the code settings datagrid. 
            DataTable codeSettings = new DataTable();
            DataColumn column;

            //create columns
            column = new DataColumn();
            column.DataType = Type.GetType("System.Boolean");
            column.ColumnName = "codeCheck1";
            codeSettings.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "codeName1";
            codeSettings.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Boolean");
            column.ColumnName = "codeCheck2";
            codeSettings.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "codeName2";
            codeSettings.Columns.Add(column);

            return codeSettings;
        }

        private bool planInputDataValidation()
        {
            bool error = false;

            //check if plan name text filed is not empty
            if (string.IsNullOrEmpty(planName.Text))
            {
                planName.Background = new SolidColorBrush(errorColor);
                error = true;
            }
            else
            {
                planName.Background = new SolidColorBrush(goodColor);
            }

            //check if the premium valuse matching the banding type
            //months for monthly banding, and numbers for salary and age.
            DataTable premium = ((DataView)bandingValues.ItemsSource).ToTable();
            switch (bandingType.Text.ToString())
            {
                case "Month":
                    foreach (DataRow r in premium.Rows)
                    {
                        int rowIndex = premium.Rows.IndexOf(r) + 1;
                        if (!StaticContent.months.Contains(r["bandingValueStart"].ToString()))
                        {
                            invalidDataGridRow(rowIndex);
                            error = true;
                        }
                        else
                            validDataGridRow(rowIndex);

                        if (!StaticContent.months.Contains(r["bandingValueEnd"].ToString()))
                        {
                            invalidDataGridRow(rowIndex);
                            error = true;
                        }
                        else
                            validDataGridRow(rowIndex);
                    }
                    break;
                default:
                    foreach (DataRow r in premium.Rows)
                    {
                        int rowIndex = premium.Rows.IndexOf(r) + 1;
                        Double n;

                        if (!Double.TryParse(r["bandingValueStart"].ToString(),out n))
                        {
                            invalidDataGridRow(rowIndex);
                            error = true;
                        }
                        else
                            validDataGridRow(rowIndex);

                        if (!Double.TryParse(r["bandingValueEnd"].ToString(), out n))
                        {
                            invalidDataGridRow(rowIndex);
                            error = true;
                        }
                        else
                            validDataGridRow(rowIndex);

                    }
                    break;
            }

            return error;
        }

        private void invalidDataGridRow(int rowIndex)
        {
            //if the row has invalid data, highligh the row
            DataGridRow row = (DataGridRow)bandingValues.ItemContainerGenerator.ContainerFromIndex(rowIndex - 1);
            row.Background = new SolidColorBrush(errorColor);
        }

        private void validDataGridRow(int rowIndex)
        {
            //if the row has valid data, remove the highlight
            DataGridRow row = (DataGridRow)bandingValues.ItemContainerGenerator.ContainerFromIndex(rowIndex - 1);
            row.Background = new SolidColorBrush(goodColor);
        }

        private void unhighlightPlanFields()
        {
            //remove all highlights from the form
            Color goodColor = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            employerName.Background = new SolidColorBrush(goodColor);
            ein.Background = new SolidColorBrush(goodColor);
        }

        private void planDataGrid_Row_Selected(object sender, EventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            DataRowView drv = planDataGrid.Items.GetItemAt(dgr.GetIndex()) as DataRowView;

            if (drv != null)
            {
                //get the plan name
                string name = drv["name"].ToString();

                //this should always return one row
                //passing the name of the StoredProcedure from the Database and two paramater
                DataRow dr = DatabaseOPS.GetPlan(new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId), new SqlParameter("@name", name)).Rows[0];

                // setting the label content values equal to the values from the data row selected from the database
                planName.Text = dr["name"].ToString();
                planType.Text = dr["medicalPlan"].ToString().Equals("1") ? "Medical Plan" : "COBRA Plan";
                fundType.Text = dr["fundingType"].ToString();
                waitingPeriod.Text = dr["eligibile1stOfMonth"].ToString().Equals("1") ? "First of the Month Following" : "Next Day Following";
                waitingDays.Text = dr["waitingDays"].ToString();
                offeredSpouse.Text = dr["offeredSpouse"].ToString().Equals("0") ? "No" : "Yes";
                offeredDependent.Text = dr["offeredDependents"].ToString().Equals("0") ? "No" : "Yes";
                termTermination.Text = dr["planTermTermination"].ToString().Equals("0") ? "No" : "Yes";
                renewalMonth.Text = dr["planRenewal"].ToString();
                minimumValue.Text = dr["minimumValue"].ToString().Equals("0") ? "No" : "Yes";

                //create datatable for the codes data grid
                DataTable codeSettings = createCodesDataTable();
                DataRow row;
                string[] code = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };

                //create and add rows
                for (int i = 0; i < 9; i++)
                {
                    row = codeSettings.NewRow();
                    row["codeCheck1"] =  Convert.ToBoolean(string.IsNullOrWhiteSpace(dr[string.Concat("Code1", code[i])].ToString()) ? 0 : dr[string.Concat("Code1", code[i])]);
                    row["codeName1"] = string.Concat("Code 1", code[i]);
                    row["codeCheck2"] =  Convert.ToBoolean(string.IsNullOrWhiteSpace(dr[string.Concat("Code2", code[i])].ToString()) ? 0 : dr[string.Concat("Code2", code[i])]);
                    row["codeName2"] = string.Concat("Code 2", code[i]);
                    codeSettings.Rows.Add(row);
                }
                codesDataGrid.ItemsSource = codeSettings.DefaultView;
                bandingType.Text = dr["bandingType"].ToString();

                //load the premiums into the premium table
                loadPremiums();

                //enable the tab control so the user can make changes
                PlanInfoGrid.IsEnabled = true;

                //enable the save button so the user can save changes
                savePlan.IsEnabled = true;

                //rest textbox background color to white
                //unhighlightFields();
                planSaveMode = 0;

            }

        }

        private void loadPremiums()
        {
            //get premiums from database
            //clone data table to change column types
            DataTable premiumDT = DatabaseOPS.GetPremium(new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId), new SqlParameter("@name", planName.Text));
            DataTable dtCloned = premiumDT.Clone();
            dtCloned.Columns["bandingValueStart"].DataType = Type.GetType("System.String");
            dtCloned.Columns["bandingValueEnd"].DataType = Type.GetType("System.String");
            foreach (DataRow r in premiumDT.Rows)
            {
                dtCloned.ImportRow(r);
            }

            //switch between the banding types to format the number
            //Month banding, the numbers are switched to months
            //age banding, the number are displayed without decimals
            //salary, the numbers are displayed with two decimal places
            switch (bandingType.Text)
            {
                case "Month":
                    foreach (DataRow r in dtCloned.Rows)
                    {

                        r["bandingValueStart"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(convertStringtoInt(r["bandingValueStart"].ToString()));
                        r["bandingValueEnd"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(convertStringtoInt(r["bandingValueEnd"].ToString()));
                    }
                    break;
                case "Age":
                    foreach (DataRow r in dtCloned.Rows)
                    {
                        r["bandingValueStart"] = convertStringtoInt(r["bandingValueStart"].ToString());
                        r["bandingValueEnd"] = convertStringtoInt(r["bandingValueEnd"].ToString());
                    }
                    break;
                case "Salary":
                    foreach (DataRow r in dtCloned.Rows)
                    {
                        r["bandingValueStart"] = Convert.ToDouble(r["bandingValueStart"].ToString()).ToString("N2");
                        r["bandingValueEnd"] = Convert.ToDouble(r["bandingValueEnd"].ToString()).ToString("N2");
                    }
                    break;
                default:
                    break;
            }

            //display table
            bandingValues.ItemsSource = new DataView(dtCloned);
        }

        private void AddPremium_Click(object sender, RoutedEventArgs e)
        {
            bandingValues.SelectedIndex = -1;

            //get datatable from the codes data grid
            DataTable premium = ((DataView)bandingValues.ItemsSource).ToTable();
            premium.Rows.Add(premium.NewRow());
            bandingValues.ItemsSource = premium.DefaultView;
        }

        private void DeletePremium_Click(object sender, RoutedEventArgs e)
        {
            //get datatable from the codes data grid
            DataTable premium = ((DataView)bandingValues.ItemsSource).ToTable();
            if (premium.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //delete record from database
                    DatabaseOPS.DeletePremium(new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId), 
                                              new SqlParameter("@name", planName.Text), 
                                              new SqlParameter("@bandingName", premium.Rows[bandingValues.SelectedIndex]["bandingName"].ToString()));

                    //remove the record from the datatable and datagrid
                    premium.Rows[bandingValues.SelectedIndex].Delete();
                    bandingValues.ItemsSource = premium.DefaultView;
                }
            }
            else
            {
                MessageBox.Show("Table is empty!", "Warning", MessageBoxButton.OK ,MessageBoxImage.Warning);
            }
        }

        private void AddPlan_Click(object sender, RoutedEventArgs e)
        {
            clearPlanForm();
            planSaveMode = 1;
            savePlan.IsEnabled = true;
            PlanInfoGrid.IsEnabled = true;
            unhighlightPlanFields();
        }

        private void DeletePlan_Click(object sender, RoutedEventArgs e)
        {
            //get datatable from the codes data grid
            DataTable planGrid = ((DataView)planDataGrid.ItemsSource).ToTable();

            //check if there is a plan to delte
            if (planGrid.Rows.Count > 0)
            {
                //make sure the user wanted to delete the plan
                if (MessageBox.Show("Are you sure you would like to delete " + planGrid.Rows[planDataGrid.SelectedIndex]["name"].ToString() + "?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //delte the plan and remove it from the data grid.
                    DatabaseOPS.DeletePlan(new SqlParameter("@CompanyTaxId", SelectedCompany.TaxId), new SqlParameter("@name", planGrid.Rows[planDataGrid.SelectedIndex]["name"].ToString()));
                    LoadPlan();
                }
            }
            else
            {
                MessageBox.Show("There are no remaining Plans", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private SqlParameter[] getPlanParams()
        {
            //convert the data grid to a datatable
            DataTable codes = ((DataView)codesDataGrid.ItemsSource).ToTable();

            //declare the sqlparameter array and add the parameters
            SqlParameter[] param = new SqlParameter[30];
            #region(params)
            param[0] = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);
            param[1] = new SqlParameter("@name", planName.Text);
            param[2] = new SqlParameter("@medicalPlan", planType.Text.Equals("Medical Plan") ? 1 : 0);
            param[3] = new SqlParameter("@bandingType", bandingType.Text);
            param[4] = new SqlParameter("@offeredSpouse", offeredSpouse.Text.Equals("Yes")? 1 : 0);
            param[5] = new SqlParameter("@offeredDependents", offeredDependent.Text.Equals("Yes") ? 1 : 0);
            param[6] = new SqlParameter("@waitingDays", waitingDays.Text);
            param[7] = new SqlParameter("@eligibile1stOfMonth", waitingPeriod.Text.Equals("First of the Month Following") ? 1 : 0);
            param[8] = new SqlParameter("@fundingType", fundType.Text);
            param[9] = new SqlParameter("@planRenewal", renewalMonth.Text);
            param[10] = new SqlParameter("@planTermTermination", termTermination.Text.Equals("Yes") ? 1 : 0);
            param[11] = new SqlParameter("@minimumValue", minimumValue.Text.Equals("Yes") ? 1 : 0);
            param[12] = new SqlParameter("@code1A", Convert.ToBoolean(codes.Rows[0][0]) ? 1 : 0);
            param[13] = new SqlParameter("@code1B", Convert.ToBoolean(codes.Rows[1][0]) ? 1 : 0);
            param[14] = new SqlParameter("@code1C", Convert.ToBoolean(codes.Rows[2][0]) ? 1 : 0);
            param[15] = new SqlParameter("@code1D", Convert.ToBoolean(codes.Rows[3][0]) ? 1 : 0);
            param[16] = new SqlParameter("@code1E", Convert.ToBoolean(codes.Rows[4][0]) ? 1 : 0);
            param[17] = new SqlParameter("@code1F", Convert.ToBoolean(codes.Rows[5][0]) ? 1 : 0);
            param[18] = new SqlParameter("@code1G", Convert.ToBoolean(codes.Rows[6][0]) ? 1 : 0);
            param[19] = new SqlParameter("@code1H", Convert.ToBoolean(codes.Rows[7][0]) ? 1 : 0);
            param[20] = new SqlParameter("@code1I", Convert.ToBoolean(codes.Rows[8][0]) ? 1 : 0);
            param[21] = new SqlParameter("@code2A", Convert.ToBoolean(codes.Rows[0][2]) ? 1 : 0);
            param[22] = new SqlParameter("@code2B", Convert.ToBoolean(codes.Rows[1][2]) ? 1 : 0);
            param[23] = new SqlParameter("@code2C", Convert.ToBoolean(codes.Rows[2][2]) ? 1 : 0);
            param[24] = new SqlParameter("@code2D", Convert.ToBoolean(codes.Rows[3][2]) ? 1 : 0);
            param[25] = new SqlParameter("@code2E", Convert.ToBoolean(codes.Rows[4][2]) ? 1 : 0);
            param[26] = new SqlParameter("@code2F", Convert.ToBoolean(codes.Rows[5][2]) ? 1 : 0);
            param[27] = new SqlParameter("@code2G", Convert.ToBoolean(codes.Rows[6][2]) ? 1 : 0);
            param[28] = new SqlParameter("@code2H", Convert.ToBoolean(codes.Rows[7][2]) ? 1 : 0);
            param[29] = new SqlParameter("@code2I", Convert.ToBoolean(codes.Rows[8][2]) ? 1 : 0);
            #endregion

            return param;
        }

        private SqlParameter[][] getPremiumParams()
        {
            //convert the data grid to a datatable
            DataTable banding = fixPremiumName(((DataView)bandingValues.ItemsSource).ToTable());

            //declare the sqlparameter array and add the parameters
            SqlParameter[][] param = new SqlParameter[banding.Rows.Count][];
            #region(params)
            for (int i = 0; i < banding.Rows.Count; i++)
            {
                param[i] = new SqlParameter[6];
                param[i][0] = new SqlParameter("@CompanyTaxID", SelectedCompany.TaxId);
                param[i][1] = new SqlParameter("@name", planName.Text);
                param[i][2] = new SqlParameter("@bandingName", banding.Rows[i]["bandingName"].ToString());
                param[i][3] = new SqlParameter("@bandingValueStart", convertMonthtoInt(banding.Rows[i]["bandingValueStart"].ToString()));
                param[i][4] = new SqlParameter("@bandingValueEnd", convertMonthtoInt(banding.Rows[i]["bandingValueEnd"].ToString()));
                param[i][5] = new SqlParameter("@amount", banding.Rows[i]["amount"].ToString());
            }
            #endregion

            return param;
        }

        private void SavePlan_Click(object sender, RoutedEventArgs e)
        {
            if(!planInputDataValidation())
            {
                //if planSaveMode 1 then it is a new Employer
                //if planSaveMode 0 then it is a existant Employer
                if (planSaveMode == 1)
                {
                    //add plan
                    DatabaseOPS.AddPlan(getPlanParams());

                    //add premiums
                    SqlParameter[][] premParam= getPremiumParams();
                    for(int i = 0; i< premParam.Count(); i++)
                    {
                        DatabaseOPS.AddPremium(premParam[i]);
                    }
                    
                    //reload plan list
                    LoadPlan();
                }
                else
                {
                    //modify plan
                    DatabaseOPS.EditPlan(getPlanParams());

                    //modify premiums
                    SqlParameter[][] premParam = getPremiumParams();
                    for (int i = 0; i < premParam.Count(); i++)
                    {
                        DatabaseOPS.EditPremium(premParam[i]);
                    }

                    //reload premium list
                    loadPremiums();
                }
            }

            planSaveMode = 0;
        }

        private void CancelPlan_Click(object sender, RoutedEventArgs e)
        {
            getPremiumParams();
            PlanInfoGrid.IsEnabled = false;
            planSaveMode = 0;
        }


        #endregion

        #region(Employee)
        public ICollectionView hireDataView { get; set; }
        int employeeSaveMode = 0;
        string ssn = null;
        Thread hire = null;

        private void clearEmployeeForm()
        {
            //this method sets the value of every textbox, combobox, and checkbox to null
            //in the Employee tab
            eeEmployerName.Text = null;
            eeFirstName.Text = null;
            eeMiddleName.Text = null;
            eeLastName.Text = null;
            eeSocialSecurityNumber.Text = null;
            eeDateOfBirth.Text = null;
            eeEmailAddress.Text = null;
            eeAddress.Text = null;
            eeAddress2.Text = null;
            eeCity.Text = null;
            eeState.Text = null;
            eeZipCode.Text = null;
            eeCountry.Text = null;

            //get datatable from the codes data grid
            DataTable hireInformation = ((DataView)eeHireInformation.ItemsSource).ToTable();
            DataTable statusInformation = ((DataView)eeStatusInformation.ItemsSource).ToTable();
            DataTable enrollmentInformation = ((DataView)eeEnrollmentInformation.ItemsSource).ToTable();
            DataTable codes = ((DataView)eeCodes.ItemsSource).ToTable();
            DataTable coveredIndividuals = ((DataView)eeCoveredIndividuals.ItemsSource).ToTable();

            //clear the datatables
            hireInformation.Clear();
            statusInformation.Clear();
            enrollmentInformation.Clear();
            codes.Clear();
            coveredIndividuals.Clear();

            //assign the datatables to the datagrids
            eeHireInformation.ItemsSource = hireInformation.DefaultView;
            eeStatusInformation.ItemsSource = statusInformation.DefaultView;
            eeEnrollmentInformation.ItemsSource = enrollmentInformation.DefaultView;
            eeCodes.ItemsSource = codes.DefaultView;
            eeCoveredIndividuals.ItemsSource = coveredIndividuals.DefaultView;
        }

        private void employeeDataGrid_Row_Selected(object sender, EventArgs e)
        {

            DataGridRow dgr = (DataGridRow)sender;

            if (dgr != null)
            {
                // sets the id of the selected employer to the static Id from SelectedCompany class
                Employee ee = dgr.Item as Employee;

                eeEmployerName.Text = null;
                eeFirstName.Text = ee.firstName;
                eeMiddleName.Text = ee.middleName;
                eeLastName.Text = ee.lastName;
                eeSocialSecurityNumber.Text = ee.ssn;
                eeDateOfBirth.Text = ee.dateOfBirth;
                eeEmailAddress.Text = ee.emailAddress;
                eeAddress.Text = ee.address;
                eeAddress2.Text = ee.address2;
                eeCity.Text = ee.city;
                eeState.Text = ee.state;
                eeZipCode.Text = ee.zipCode;
                eeCountry.Text = ee.country;

                eeHireInformation.ItemsSource = CollectionViewSource.GetDefaultView(ee.hireInformation);
                eeStatusInformation.ItemsSource = CollectionViewSource.GetDefaultView(ee.statusInformation);
                eeEnrollmentInformation.ItemsSource = CollectionViewSource.GetDefaultView(ee.coverageInformation);


                //DataGridRow dgr = (DataGridRow)sender;
                //DataRowView drv = employeeDataGrid.Items.GetItemAt(dgr.GetIndex()) as DataRowView;

                //if (drv != null)
                //{
                //    //get the plan name
                //    ssn = drv["ssn"].ToString();

                //    //this should always return one row
                //    //passing the name of the StoredProcedure from the Database and two paramater

                //    //DataRow dr = DatabaseOPS.GetEmployee(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn)).Rows[0];

                //    //// setting the label content values equal to the values from the data row selected from the database
                //    //eeEmployerName.Text = null;
                //    //eeFirstName.Text = dr["firstName"].ToString();
                //    //eeMiddleName.Text = dr["middleName"].ToString();
                //    //eeLastName.Text = dr["lastName"].ToString();
                //    //eeSocialSecurityNumber.Text = dr["ssn"].ToString();
                //    //eeDateOfBirth.Text = dr["birthday"].ToString();
                //    //eeEmailAddress.Text = dr["email"].ToString();
                //    //eeAddress.Text = dr["address"].ToString();
                //    //eeAddress2.Text = dr["address2"].ToString();
                //    //eeCity.Text = dr["city"].ToString();
                //    //eeState.Text = dr["state"].ToString();
                //    //eeZipCode.Text = dr["zip"].ToString();
                //    //eeCountry.Text = dr["country"].ToString();

                //    if(hire != null)
                //    {
                //        if(hire.IsAlive)
                //        {
                //            hire.Abort();
                //        }
                //    }

                //    //hire = new Thread(() =>
                //    //{
                //    //    DataTable HireSpan = DatabaseOPS.GetHireSpan(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn));
                //    //    this.eeHireInformation.Dispatcher.Invoke(new Action(() => eeHireInformation.ItemsSource = HireSpan.DefaultView));
                //    //});

                //    hire.Name = "hire";
                //    hire.Start();


                //    new Thread(() =>
                //    {
                //        DataTable Status = DatabaseOPS.GetStatus(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn));
                //        this.eeStatusInformation.Dispatcher.Invoke(new Action(() => eeStatusInformation.ItemsSource = Status.DefaultView));
                //    }).Start();


                //    new Thread(() =>
                //    {
                //        DataTable Enrollment = DatabaseOPS.GetEnrollment(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn));
                //        this.eeEnrollmentInformation.Dispatcher.Invoke(new Action(() => eeEnrollmentInformation.ItemsSource = Enrollment.DefaultView));
                //    }).Start();

                //    new Thread(() =>
                //    {
                //        DataTable Codes = DatabaseOPS.GetCodes(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn));
                //        DataTable dispCodes = new DataTable();
                //        DataColumn column;
                //        DataRow row;

                //        column = new DataColumn();
                //        column.ColumnName = "name";
                //        dispCodes.Columns.Add(column);

                //        for (int i = 0; i < 12; i++)
                //        {
                //            column = new DataColumn();
                //            column.ColumnName = StaticContent.months[i];
                //            dispCodes.Columns.Add(column);
                //        }

                //        string[] name = { "Offer of Coverage", "Premium Amount", "Applicable Section 4980" };

                //        for (int i = 0; i < 3; i++)
                //        {
                //            row = dispCodes.NewRow();
                //            row["name"] = name[i];

                //            for(int j = 0; j < 12; j++)
                //            {
                //                row[StaticContent.months[j]] = Codes.Rows[0][StaticContent.EmployeeCodeColumns[(j + 1) + (13 * i)]];
                //            }
                //            dispCodes.Rows.Add(row);
                //        }

                //        this.eeCodes.Dispatcher.Invoke(new Action(() => eeCodes.ItemsSource = dispCodes.DefaultView));
                //    }).Start();

                //    new Thread(() =>
                //    {
                //        DataTable CoveredIndividuals = DatabaseOPS.GetCoveredIndividual(new SqlParameter("@EmployerTaxId", SelectedCompany.TaxId), new SqlParameter("@ssn", ssn));
                //        this.eeCoveredIndividuals.Dispatcher.Invoke(new Action(() => eeCoveredIndividuals.ItemsSource = CoveredIndividuals.DefaultView));
                //    }).Start();

                //    //assign the datatables to the datagrids
                //    //eeCodes.ItemsSource = codes.DefaultView;
                //    //eeCoveredIndividuals.ItemsSource = CoveredIndividuals.DefaultView;

                //    //enable the tab control so the user can make changes
                //    eeDataContainer.IsEnabled = true;

                //    //enable the save button so the user can save changes
                //    eeSaveChanges.IsEnabled = true;

                //    //reset textbox background color to white
                //    //unhighlightFields();
                //    employeeSaveMode = 0;

            }

            }

        private void employeeDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            Debug.Print("I AM HERE");
            switch (tc.SelectedIndex)
            {
                case 1:
                    
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //make sure the user wants to exit.
            if (MessageBox.Show("Are you sure you would like to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    //CompanyData class is used to store all the companies on the Select tab.
    //the main purpose of this class is to help with the searching feature. 
    public class CompanyData
    {
        public string id { get; set; }
        public string taxid { get; set; }
        public string name { get; set; }
        public string totalNumberEE { get; set; }
        public string lastEdit { get; set; }

        public bool Filter(string filterText)
        {
            return taxid.Contains(filterText) || name.ToLower().Contains(filterText.ToLower());
        }
    }
}


