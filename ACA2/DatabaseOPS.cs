using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACA2
{
    class DatabaseOPS
    {
        private static readonly string ConnectionString = Properties.Settings.Default.ConnectionString;
        
        public static DataTable ExcuteProcedure(string procedureName, int queryType, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection))
                {

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddRange(parameters);

                    sqlConnection.Open();
                    if (queryType == 0)
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataAdapter.Fill(dt);
                    }
                    else
                    {
                        sqlCommand.ExecuteNonQuery();
                        dt = null;
                    }
                }
            }

            return dt;
        }

        public static DataTable AddCompany(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CompanyAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetCompany(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CompanySelect", 0, parameters);
            return dt;
        }

        public static DataTable EditCompany(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CompanyModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteCompany(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CompanyDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddEmployer(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerAdd", 1 , parameters);
            return dt;
        }

        public static DataTable GetEmployer(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerMSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditEmployer(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteEmployer(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerDelete", 1 , parameters);
            return dt;
        }

        public static DataTable AddPlan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerPlanAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetPlan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerPlanSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditPlan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerPlanModify", 1, parameters);
            return dt;
        }

        public static DataTable DeletePlan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployerPlanDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddPremium(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_PremiumAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetPremium(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_PremiumSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditPremium(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_PremiumModify", 1, parameters);
            return dt;
        }

        public static DataTable DeletePremium(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_PremiumDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddEmployee(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetEmployee(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeASelect", 0, parameters);
            return dt;
        }

        public static DataTable EditEmployee(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeAModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteEmployee(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeADelete", 1, parameters);
            return dt;
        }

        public static DataTable AddHireSpan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeHireSpanAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetHireSpan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeHireSpanSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditHireSpan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeHireSpanModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteHireSpan(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeHireSpanDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddStatus(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeStatusAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetStatus(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeStatusSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditStatus(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeStatusModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteStatus(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeStatusDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddEnrollment(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeEnrollmentAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetEnrollment(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeEnrollmentSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditEnrollment(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeEnrollmentModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteEnrollment(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeEnrollmentDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddCoveredIndividual(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CoveredIndividualAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetCoveredIndividual(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CoveredIndividualSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditCoveredIndividual(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CoveredIndividualModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteCoveredIndividual(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_CoveredIndividualDelete", 1, parameters);
            return dt;
        }

        public static DataTable AddCodes(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeCodeAdd", 1, parameters);
            return dt;
        }

        public static DataTable GetCodes(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeCodeSelect", 0, parameters);
            return dt;
        }

        public static DataTable EditCodes(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeCodeModify", 1, parameters);
            return dt;
        }

        public static DataTable DeleteCodes(params SqlParameter[] parameters)
        {
            DataTable dt = ExcuteProcedure("usp_EmployeeCodeDelete", 1, parameters);
            return dt;
        }
    }
}
