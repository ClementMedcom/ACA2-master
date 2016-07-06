using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ACA2
{
    class Employee
    {
        public string id { get; set; }
        public string employeeCodeId { get; set; }
        public string employerName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string ssn { get; set; }
        public string dateOfBirth { get; set; }
        public string emailAddress { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string country { get; set; }
        public ObservableCollection<HireData> hireInformation { get; set; }
        public ObservableCollection<StatusData> statusInformation { get; set; }
        public ObservableCollection<CoverageData> coverageInformation { get; set; }
        public StatusCollection sc { get; set; }
        public CoverageCollection cc { get; set; }
        public HireCollection hc { get; set; }
        public string test { get; set; }

        public void setHireInformation()
        {
            hc.FilterText = employeeCodeId;
            var l = hc.hireView.Cast<HireData>().ToList();
            hireInformation = new ObservableCollection<HireData>(l);
        }

        public void setStatusInformation()
        {
            sc.FilterText = employeeCodeId;
            var l = sc.statusView.Cast<StatusData>().ToList();
            statusInformation = new ObservableCollection<StatusData>(l);
        }

        public void setCoverageInformation()
        {
            cc.FilterText = employeeCodeId;
            var l = cc.coverageView.Cast<CoverageData>().ToList();
            coverageInformation = new ObservableCollection<CoverageData>(l);
        }

        ~Employee()
        {
            hireInformation = null;
            statusInformation = null;
            coverageInformation = null;
            sc = null;
            cc = null;
            hc = null;
        }
    }

    class HireCollection
    {
        public ObservableCollection<HireData> hireData = new ObservableCollection<HireData>();
        public ICollectionView hireView { get; set; }
        private string _filterText;

        public void setData(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                hireData.Add(new HireData()
                {
                    id = row["id"].ToString(),
                    hireName = row["hireName"].ToString(),
                    employeeCodeId = row["employeeCodeId"].ToString(),
                    startDate = row["startDate"].ToString(),
                    endDate = row["endDate"].ToString()
                });
            }

            hireView = CollectionViewSource.GetDefaultView(hireData);
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                hireView.Filter = DoFilter;
            }
        }

        private bool DoFilter(object obj)
        {
            var hireData = obj as HireData;
            if (hireData != null)
            {
                return hireData.Filter(FilterText);
            }
            return false;
        }
    }

    class HireData
    {
        public string id { get; set; }
        public string hireName { get; set; }
        public string employeeCodeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        public bool Filter(string filterText)
        {
            return employeeCodeId.Contains(filterText);
        }
    }

    class StatusCollection
    {
        public ObservableCollection<StatusData> statusData = new ObservableCollection<StatusData>();
        public ICollectionView statusView { get; set; }
        private string _filterText;

        public void setData(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                statusData.Add(new StatusData()
                {
                    id = row["id"].ToString(),
                    statusName = row["statusName"].ToString(),
                    employeeCodeId = row["employeeCodeId"].ToString(),
                    status = row["status"].ToString(),
                    startDate = row["startDate"].ToString(),
                    endDate = row["endDate"].ToString()
                });
            }

            statusView = CollectionViewSource.GetDefaultView(statusData);
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                statusView.Filter = DoFilter;
            }
        }

        private bool DoFilter(object obj)
        {
            var statusData = obj as StatusData;
            if (statusData != null)
            {
                return statusData.Filter(FilterText);
            }
            return false;
        }
    }

    class StatusData
    {
        public string id { get; set; }
        public string statusName { get; set; }
        public string employeeCodeId { get; set; }
        public string status { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        public bool Filter(string filterText)
        {
            return employeeCodeId.Contains(filterText);
        }
    }

    class CoverageCollection
    {
        public ObservableCollection<CoverageData> coverageData = new ObservableCollection<CoverageData>();
        public ICollectionView coverageView { get; set; }
        private string _filterText;

        public void setData(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                coverageData.Add(new CoverageData()
                {
                    id = row["id"].ToString(),
                    enrollmentName = row["enrollmentName"].ToString(),
                    employeeCodeId = row["employeeCodeId"].ToString(),
                    planName = row["name"].ToString(),
                    unionMember = Convert.ToBoolean(row["unionMember"]),
                    contributionStartDate = row["contributionStartDate"].ToString(),
                    contributionEndDate = row["contributionEndDate"].ToString(),
                    coverageOfferDate = row["coverageOfferDate"].ToString(),
                    isEnrolled = Convert.ToBoolean(row["isEnrolled"]),
                    coverageStartDate = row["coverageStartDate"].ToString(),
                    coverageEndDate = row["coverageEndDate"].ToString(),
                    cobraEnrolled = Convert.ToBoolean(row["cobraEnrolled"]),
                    cobraStartDate = row["cobraStartDate"].ToString(),
                    cobraEndDate = row["cobraEndDate"].ToString()
                });
            }

            coverageView = CollectionViewSource.GetDefaultView(coverageData);
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                coverageView.Filter = DoFilter;
            }
        }

        private bool DoFilter(object obj)
        {
            var coverageData = obj as CoverageData;
            if (coverageData != null)
            {
                return coverageData.Filter(FilterText);
            }
            return false;
        }
    }

    class CoverageData
    {
        public string id { get; set; }
        public string enrollmentName { get; set; }
        public string employeeCodeId { get; set; }
        public string planName { get; set; }
        public bool unionMember { get; set; }
        public string contributionStartDate { get; set; }
        public string contributionEndDate { get; set; }
        public string coverageOfferDate { get; set; }
        public bool isEnrolled { get; set; }
        public string coverageStartDate { get; set; }
        public string coverageEndDate { get; set; }
        public bool cobraEnrolled { get; set; }
        public string cobraStartDate { get; set; }
        public string cobraEndDate { get; set; }

        public bool Filter(string filterText)
        {
            return employeeCodeId.Contains(filterText);
        }
    }
}