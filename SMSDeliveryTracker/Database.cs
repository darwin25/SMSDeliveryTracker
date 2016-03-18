//Required assemblies
using Android.Database.Sqlite;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace SMSDeliveryTracker
{
    class Database
    {
        //SQLiteDatabase object for database handling
        private SQLiteDatabase sqldb;
        //String for Query handling
        private string sqldb_query;
        //String for Message handling
        private string sqldb_message;
        //Bool to check for database availability
        private bool sqldb_available;
        //Zero argument constructor, initializes a new instance of Database class
        public Database()
        {
            sqldb_message = "";
            sqldb_available = false;
        }
        //One argument constructor, initializes a new instance of Database class with database name parameter
        public Database(string sqldb_name)
        {
            try
            {
                sqldb_message = "";
                sqldb_available = false;
                CreateDatabase(sqldb_name);
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }
        //Gets or sets value depending on database availability
        public bool DatabaseAvailable
        {
            get { return sqldb_available; }
            set { sqldb_available = value; }
        }
        //Gets or sets the value for message handling
        public string Message
        {
            get { return sqldb_message; }
            set { sqldb_message = value; }
        }
        //Creates a new database which name is given by the parameter
        public void CreateDatabase(string sqldb_name)
        {
            try
            {
                sqldb_message = "";
                string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sqldb_path = Path.Combine(sqldb_location, sqldb_name);
                bool sqldb_exists = File.Exists(sqldb_path);
                if (!sqldb_exists)
                {
                   
                    this.CreateTableMessageIn(sqldb_path);
                    this.CreateTableDelivery(sqldb_path);

                    sqldb_message = "Database: " + sqldb_name + " created";
                }
                else
                {
                    sqldb = SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
                    sqldb_message = "Database: " + sqldb_name + " opened";
                }
                sqldb_available = true;
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        
        private void CreateTableMessageIn(string sqldb_path)
        {
            sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path, null);
            sqldb_query = "CREATE TABLE IF NOT EXISTS MessageIn (Id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , SendTime DATETIME , ReceiveTime DATETIME, MessageFrom VARCHAR, MessageTo VARCHAR, SMSC VARCHAR, MessageText VARCHAR, MessageType VARCHAR, MessageParts INTEGER, MessagePDU VARCHAR, Gateway VARCHAR, UserId VARCHAR);";
            sqldb.ExecSQL(sqldb_query);            
        }

        private void CreateTableDelivery(string sqldb_path)
        {
            sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path, null);
            sqldb_query = "CREATE TABLE IF NOT EXISTS Delivery ( _id INTEGER PRIMARY KEY AUTOINCREMENT  NOT NULL , DeliveryId VARCHAR, CustomerName VARCHAR, Address VARCHAR, MobileNumber VARCHAR, CommitedDeliveryTime VARCHAR, ActualDeliveryTime VARCHAR, DateTimeUpdated VARCHAR, OrderAmount VARCHAR, DeliveryStatus VARCHAR);";
            sqldb.ExecSQL(sqldb_query);
            sqldb.ExecSQL("INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('12345-1','Darwin Pasco','Pasig','+639982292906','3/17/2016 12:00:00 AM','200','PENDING / UNASSIGNED'); ");
            sqldb.ExecSQL("INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('12345-2','Connie Pasco','Pasig','+639982292906','3/17/2016 12:00:00 AM','300','PENDING / UNASSIGNED'); ");
            sqldb.ExecSQL("INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('12345-3','Vito Pasco','Pasig','+639982292906','3/17/2016 12:00:00 AM','400','PENDING / UNASSIGNED'); ");
            sqldb.ExecSQL("INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('12345-4','Rocco Pasco','Pasig','+639982292906','3/17/2016 12:00:00 AM','500','PENDING / UNASSIGNED'); ");
            sqldb.ExecSQL("INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('12345-5','Ava Pasco','Pasig','+639982292906','3/17/2016 12:00:00 AM','600','PENDING / UNASSIGNED'); ");
        }

        public void AddRecordMessageIn(object obj)

        {
            MessageInVO vo = (MessageInVO)obj;
            try
            {
                sqldb_query = "INSERT INTO MessageIn (MessageText, MessageFrom) VALUES ('" + vo.MessageText + "','" + vo.MessageFrom + "');";
                sqldb.ExecSQL(sqldb_query);                
                sqldb_message = "Record saved";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        public string AddRecordMessageIn2(string messagetext, string messagefrom)

        {
            string functionReturnValue = "";

            try
            {
                sqldb_query = "INSERT INTO MessageIn (MessageText, MessageFrom) VALUES ('" + messagetext + "','" + messagefrom + "');";
                sqldb.ExecSQL(sqldb_query);
                functionReturnValue = "Saved-MessageIn";
            }
            catch (SQLiteException ex)
            {
                functionReturnValue = ex.Message + "- MessageIn";
            }

            return functionReturnValue;
        }
        public void AddRecordDelivery(string MessageText)

        {            
            try
            {
                string value = MessageText;
                string[] parts = value.Split('|');
                
                sqldb_query = "INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('" + parts[0].ToString() + "','" + parts[1].ToString() + "','" + parts[2].ToString() + "','" + parts[3].ToString() + "','" + parts[4].ToString()+ "','" + parts[5].ToString() + "','PENDING / UNASSIGNED' ); ";
                sqldb.ExecSQL(sqldb_query);
                sqldb_message = "Record saved";
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        public string AddRecordDelivery2(string MessageText)

        {
            string functionReturnValue = "";

            try
            {
                string value = MessageText;
                string[] parts = value.Split('|');

                sqldb_query = "INSERT INTO Delivery (DeliveryId, CustomerName, Address, MobileNumber, CommitedDeliveryTime, OrderAmount, DeliveryStatus) VALUES ('" + parts[0].ToString() + "','" + parts[1].ToString() + "','" + parts[2].ToString() + "','" + parts[3].ToString() + "','" + parts[4].ToString() + "','" + parts[5].ToString() + "','PENDING / UNASSIGNED' ); ";
                sqldb.ExecSQL(sqldb_query);
                functionReturnValue = "Saved-Delivery";
            }
            catch (SQLiteException ex)
            {
                functionReturnValue = ex.Message + "- Delivery"; ;
            }

            return functionReturnValue;
        }

        public void UpdateRecord(string deliveryid, string deliverystatus, string datetimeupdated)
        {
            try
            {

                string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sqldb_path = Path.Combine(sqldb_location, "deliverydb");
              
                var connection = new SqliteConnection("Data Source=" + sqldb_path);
                var command = connection.CreateCommand();
                connection.Open();

                command.CommandText = "UPDATE Delivery SET DeliveryStatus ='" + deliverystatus + "', DateTimeUpdated ='" + datetimeupdated + "' WHERE DeliveryId ='" + deliveryid + "';";
                command.CommandType = CommandType.Text;

                int rowcount = command.ExecuteNonQuery();
                
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        public void UpdateRecord(string deliveryid, string deliverystatus, string datetimeupdated, string actualdeliverytime)
        {
            try
            {
                string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string sqldb_path = Path.Combine(sqldb_location, "deliverydb");

                var connection = new SqliteConnection("Data Source=" + sqldb_path);
                var command = connection.CreateCommand();
                connection.Open();

                command.CommandText = "UPDATE Delivery SET DeliveryStatus ='" + deliverystatus + "', DateTimeUpdated ='" + datetimeupdated + "', ActualDeliveryTime ='" + actualdeliverytime + "'WHERE DeliveryId ='" + deliveryid + "';";
                command.CommandType = CommandType.Text;

                int rowcount = command.ExecuteNonQuery();                
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
        }

        public Android.Database.ICursor GetRecordCursor()
        {
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT _id, DeliveryId, CustomerName, CommitedDeliveryTime FROM Delivery;";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Record not found";
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return sqldb_cursor;
        }

        //Searches a record and returns an Android.Database.ICursor cursor
        //Shows records according to search criteria
        public Android.Database.ICursor GetRecordCursor(string sValue)
        {
            Android.Database.ICursor sqldb_cursor = null;
            try
            {
                sqldb_query = "SELECT * FROM Delivery WHERE DeliveryId = '" +  sValue + "';";
                sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
                if (!(sqldb_cursor != null))
                {
                    sqldb_message = "Record not found";
                }
            }
            catch (SQLiteException ex)
            {
                sqldb_message = ex.Message;
            }
            return sqldb_cursor;
        }

        public DataTable GetDeliveryDetails( string sValue)
        {
            string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sqldb_path = Path.Combine(sqldb_location, "deliverydb");

            DataTable dt = new DataTable();

            var connection = new SqliteConnection("Data Source=" + sqldb_path);
            var command = connection.CreateCommand();
            connection.Open();

            command.CommandText = "SELECT * FROM Delivery WHERE DeliveryId = '" + sValue + "';";
            command.CommandType = CommandType.Text;
            
            dt.Load(command.ExecuteReader());

            return dt;
       
        }
    }
}