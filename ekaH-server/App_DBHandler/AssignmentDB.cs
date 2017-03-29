using ekaH_server.Models;
using ekaH_server.Models.UserModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.App_DBHandler
{
    public class AssignmentDB
    {
        public static DateTime getDeadlineOfAssignment(int assignID)
        {
            MySqlConnection conn = DBConnection.getInstance().getConnection();
            string query = "select deadline from assignment where id= @id";

            MySqlDataReader reader = null;
            DateTime res = default(DateTime);
            try
            {

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("id", assignID);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    res = reader.GetDateTime(0);
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            reader.Dispose();
            return res;
        }

        // Tries to store the solution. PUT request. If its student, then they can change everything but grade.
        // Professor can change the grade. ONLY professor can.
        public static bool storeAssignment(Submission submission, bool isStudent)
        {
            if (getDeadlineOfAssignment(submission.AssignmentID) < submission.SubmittedTime)
            {
                return false;
            }

            MySqlConnection conn = DBConnection.getInstance().getConnection();
            string query = "insert into submission(assignmentID, studentID, grade, submissionContent, submissionDateTime) values " +
                "(@aID, @studentID, @grade, @content, @submissionDateTime) " +
                "ON DUPLICATE KEY update grade = @grade, submissionContent = @content, submissionDateTime = @submissionDateTime; ";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("aID", submission.AssignmentID);
                cmd.Parameters.AddWithValue("studentID", submission.StudentID);
                cmd.Parameters.AddWithValue("grade", isStudent? -1 : submission.Grade);
                cmd.Parameters.Add("@content", MySqlDbType.LongBlob).Value = submission.RawContent;
                cmd.Parameters.AddWithValue("submissionDateTime", isStudent? DateTime.Now : submission.SubmittedTime);

                cmd.ExecuteNonQuery();
            }catch(MySqlException ex)
            {
                throw ex;
            }
            return true;
        }

        // Returns the submission object by the given ID.
        public static Submission getSubmissionByID(int id)
        {
            MySqlConnection conn = DBConnection.getInstance().getConnection();
            string query = "select * from submission where id=@id";

            MySqlDataReader reader = null;
            Submission sub = null;
            try
            {
                MySqlCommand cmd= new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("id", id);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sub = UserWorker.extractSubmission(reader);
                }
            }catch(MySqlException ex)
            {
                throw ex;
            }
            reader.Dispose();
            return sub;
        }

        // Deletes the submission object by the given ID.
        public static void deleteSubmission(int id)
        {
            MySqlConnection conn = DBConnection.getInstance().getConnection();
            string query = "delete * from submission where id=@id";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}