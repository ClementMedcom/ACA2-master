using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACA2
{
    class StaticContent
    {
        public static List<string> filingyear = new List<string>
        {
            "2015", "2016", "2017", "2018", "2019"
        };

        public static List<string> states = new List<string>
        {
            "AL","AK","AZ","AR","CA","CO","CT","DE","FL","GA","HI","ID","IL","IN","IA","KS","KY",
            "LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND",
            "OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VT","VA","WA","WV","WI","WY"
        };

        public static List<string> country = new List<string>
        {
            "Afghanistan","Albania","Algeria","Andorra","Angola","Antigua and Barbuda","Argentina","Armenia",
            "Australia","Austria","Azerbaijan","Bahamas","Bahrain","Bangladesh","Barbados","Belarus",
            "Belgium","Belize","Benin","Bhutan","Bolivia","Bosnia and Herzegovina","Botswana","Brazil",
            "Brunei","Bulgaria","Burkina Faso","Burundi","Cabo Verde","Cambodia","Cameroon","Canada",
            "Central African Republic","Chad","Chile","China","Colombia","Comoros","Congo, Republic of the",
            "Congo, Democratic Republic of the","Costa Rica","Cote d'Ivoire","Croatia","Cuba","Cyprus",
            "Czech Republic","Denmark","Djibouti","Dominica","Dominican Republic","Ecuador","Egypt",
            "El Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Fiji","Finland","France",
            "Gabon","Gambia","Georgia","Germany","Ghana","Greece","Grenada","Guatemala","Guinea","Guinea-Bissau",
            "Guyana","Haiti","Honduras","Hungary","Iceland","India","Indonesia","Iran","Iraq","Ireland",
            "Israel","Italy","Jamaica","Japan","Jordan","Kazakhstan","Kenya","Kiribati","Kosovo","Kuwait",
            "Kyrgyzstan","Laos","Latvia","Lebanon","Lesotho","Liberia","Libya","Liechtenstein","Lithuania",
            "Luxembourg","Macedonia","Madagascar","Malawi","Malaysia","Maldives","Mali","Malta","Marshall Islands",
            "Mauritania","Mauritius","Mexico","Micronesia","Moldova","Monaco","Mongolia","Montenegro","Morocco",
            "Mozambique","Myanmar (Burma)","Namibia","Nauru","Nepal","Netherlands","New Zealand","Nicaragua",
            "Niger","Nigeria","North Korea","Norway","Oman","Pakistan","Palau","Palestine","Panama","Papua New Guinea",
            "Paraguay","Peru","Philippines","Poland","Portugal","Qatar","Romania","Russia","Rwanda","St. Kitts and Nevis",
            "St. Lucia","St. Vincent and The Grenadines","Samoa","San Marino","Sao Tome and Principe","Saudi Arabia",
            "Senegal","Serbia","Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Islands",
            "Somalia","South Africa","South Korea","South Sudan","Spain","Sri Lanka","Sudan","Suriname",
            "Swaziland","Sweden","Switzerland","Syria","Taiwan","Tajikistan","Tanzania","Thailand","Timor-Leste",
            "Togo","Tonga","Trinidad and Tobago","Tunisia","Turkey","Turkmenistan","Tuvalu","Uganda","Ukraine",
            "United Arab Emirates","United Kingdom (UK)","United States of America","Uruguay","Uzbekistan",
            "Vanuatu","Vatican City (Holy See)","Venezuela","Vietnam","Yemen","Zambia","Zimbabwe"
        };

        public static List<string> formtype = new List<string>
        {
            "1095-B", "1095-C"
        };

        public static List<string> origincode = new List<string>
        {
            "A", "B", "C", "D", "E", "F", "G", "H"
        };

        public static List<string> fundtype = new List<string>
        {
            "Self-Funded", "Fully-Insured"
        };

        public static List<string> plantype = new List<string>
        {
            "Medical Plan", "COBRA Plan"
        };

        public static List<string> waitingperiod = new List<string>
        {
            "First of the Month Following", "Next Day Following"
        };

        public static List<string> renewalmonth = new List<string>
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"
        };
        
        public static List<string> yes_no = new List<string>
        {
            "Yes", "No"
        };

        public static List<string> bandingtype = new List<string>
        {
            "Month", "Age", "Salary"
        };

        public static List<string> months = new List<string>
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        };

        public static List<string> EmployeeCodeColumns = new List<string>
        {
            "ALLM_COC","JAN_COC","FEB_COC","MAR_COC","APR_COC","MAY_COC","JUN_COC","JUL_COC","AUG_COC","SEP_COC","OCT_COC","NOV_COC","DEC_COC","ALLM_LCMP","JAN_LCMP","FEB_LCMP","MAR_LCMP","APR_LCMP","MAY_LCMP","JUN_LCMP","JUL_LCMP","AUG_LCMP","SEP_LCMP","OCT_LCMP","NOV_LCMP","DEC_LCMP","ALLM_SHC","JAN_SHC","FEB_SHC","MAR_SHC","APR_SHC","MAY_SHC","JUN_SHC","JUL_SHC","AUG_SHC","SEP_SHC","OCT_SHC","NOV_SHC","DEC_SHC"
        };
    }
}
