using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Service
{
    public class SVUserFunction {

        private SQLiteHelper db = new SQLiteHelper("data.db");
        public List<UserFunction> getlst() {
            List<UserFunction> lst = new List<UserFunction>();
            var dt = db.ExecuteQuery("select  * from  sys_user_fun");
            foreach (DataRow row in dt.Rows)
            {
                UserFunction function = new UserFunction();
                function.ID = Convert.ToInt32(row["Id"]);
                function.uName = row["uName"].ToString();
                function.fName = row["fName"].ToString();
                lst.Add(function);
            }
            return lst;
        }

        public int insertData(UserFunction function) {
          return  db.ExecuteNonQuery("INSERT INTO sys_user_fun(uName,fName) VALUES(@uName,@fName);SELECT last_insert_rowid();",
            new SQLiteParameter("@uName", function.uName),
            new SQLiteParameter("@fName", function.fName)
            );
        }

        public int del(UserFunction function) {
            return db.ExecuteNonQuery("DELETE FROM sys_user_fun WHERE uName=@uName and fName=@fName",
            new SQLiteParameter("@uName", function.uName),
            new SQLiteParameter("@fName", function.fName)
          );
        }
    }
    public class UserFunction
    {
        public int ID { get; set; }
        public string uName { get; set; }
        public string fName { get; set; }

    }
    public class SVSysUser
    {

        private SQLiteHelper db = new SQLiteHelper("data.db");
        public List<sys_user> getlst()
        {
            List<sys_user> lst = new List<sys_user>();
            var dt = db.ExecuteQuery("select  * from  sys_user");
            foreach (DataRow row in dt.Rows)
            {
                sys_user user = new sys_user();
                user.userName = row["userName"].ToString();
                user.userPWD = row["userPWD"].ToString();
                lst.Add(user);
            }
            return lst;
        }
    }
    public class sys_user
    {
        public string userName { get; set; }
        public string userPWD { get; set; }

    }


    public class SVPlcData
    {

        private SQLiteHelper db = new SQLiteHelper("data.db");
        public List<plcData> getlst()
        {
            List<plcData> lst = new List<plcData>();
            var dt = db.ExecuteQuery("select  * from  T_PLCData");
            foreach (DataRow row in dt.Rows)
            {
                plcData itemData = new plcData();
                itemData.ID = Convert.ToInt32(row["ID"].ToString());
                itemData.proName = row["proName"].ToString();
                itemData.N1  = Convert.ToInt32(row["N1"].ToString());
                itemData.W1  = Convert.ToInt32(row["W1"].ToString());
                itemData.N2  = Convert.ToInt32(row["N2"].ToString());
                itemData.W2  = Convert.ToInt32(row["W2"].ToString());
                itemData.N3  = Convert.ToInt32(row["N3"].ToString());
                itemData.W3  = Convert.ToInt32(row["W3"].ToString());
                itemData.NPQ = Convert.ToInt32(row["NPQ"].ToString());
                itemData.WPQ = Convert.ToInt32(row["WPQ"].ToString());
                itemData.NGQ = Convert.ToInt32(row["NGQ"].ToString());
                itemData.WGQ = Convert.ToInt32(row["WGQ"].ToString());
                itemData.NPY = Convert.ToInt32(row["NPY"].ToString());
                itemData.WPY = Convert.ToInt32(row["WPY"].ToString());
                itemData.isCurent = Convert.ToInt32(row["isCurent"].ToString());
                lst.Add(itemData);
            }
            return lst;
        }
        public int setCurentPlc(int Id) {
            return db.ExecuteNonQuery(@"update T_PLCData set isCurent=0; update T_PLCData set isCurent=0 where ID=@ID;", new SQLiteParameter("@ID", Id));
        }

        public int updData(plcData plcItem)
        {
            return db.ExecuteNonQuery(@"update T_PLCData
                                               set N1=@N1,
                                               W1=@W1,
                                               N2=@N2,
                                               W2=@W2,
                                               N3=@N3,
                                               W3=@W3,
                                               NPQ=@NPQ,
                                               WPQ=@WPQ,
                                               NGQ=@NGQ,
                                               WGQ=@WGQ,
                                               NPY=@NPY,
                                               WPY=@WPY
                                               where  ID=@ID",
              new SQLiteParameter("@N1", plcItem.N1),
              new SQLiteParameter("@W1", plcItem.W1),
              new SQLiteParameter("@N2", plcItem.N2),
              new SQLiteParameter("@W2", plcItem.W2),
              new SQLiteParameter("@N3", plcItem.N3),
              new SQLiteParameter("@W3", plcItem.W3),
              new SQLiteParameter("@NPQ", plcItem.NPQ),
              new SQLiteParameter("@WPQ", plcItem.WPQ),
              new SQLiteParameter("@NGQ", plcItem.NGQ),
               new SQLiteParameter("@WGQ", plcItem.WGQ),
              new SQLiteParameter("@NPY", plcItem.NPY),
              new SQLiteParameter("@WPY", plcItem.WPY),
              new SQLiteParameter("@ID", plcItem.ID)
              );
        }
    }

    public class plcData {
        public int ID { get; set; }
        public string proName { get;set;}
        //
        public int N1 { get; set; }
        public int W1 { get; set; }
        public int N2 { get; set; }
        public int W2 { get; set; }
        public int N3 { get; set; }
        public int W3 { get; set; }
        public int NPQ { get; set; }
        public int WPQ { get; set; }
        public int NGQ { get; set; }
        public int WGQ { get; set; }
        public int NPY { get; set; }
        public int WPY { get; set; }
        public int isCurent { get; set; }

    }

    /// <summary>
    /// 品牌管理列表
    /// </summary>
    public class SVBrandData
    {
        private SQLiteHelper db = new SQLiteHelper("data.db");
        public List<BrandData> getlst()
        {
            List<BrandData> lst = new List<BrandData>();
            var dt = db.ExecuteQuery("select  * from  T_Brand");
            foreach (DataRow row in dt.Rows)
            {
                BrandData itemData = new BrandData();
                itemData.ID = Convert.ToInt32(row["ID"].ToString());
                itemData.BrandName = row["BrandName"].ToString();
                if (string.IsNullOrEmpty(row["IsCurent"].ToString()))
                {
                    itemData.IsCurent = 0;
                }
                else {
                    itemData.IsCurent = Convert.ToInt32(row["IsCurent"].ToString());
                }
              
                lst.Add(itemData);
            }
            return lst;
        }

        public int insertData(BrandData brand)
        {
            SQLiteParameter[] pgrms = { new SQLiteParameter("@BrandName", brand.BrandName), new SQLiteParameter("@IsCurent", brand.IsCurent) };
            object data=    db.ExecuteScalar("INSERT INTO T_Brand(BrandName,IsCurent) VALUES(@BrandName,@IsCurent);SELECT last_insert_rowid();", pgrms);
            return Convert.ToInt32(data);
           
        }
        public int updData(BrandData brand)
        {
            return db.ExecuteNonQuery(@"update T_Brand
                                               set BrandName=@BrandName,
                                                   IsCurent=@IsCurent
                                               where  ID=@ID",
             new SQLiteParameter("@BrandName", brand.BrandName),
             new SQLiteParameter("@IsCurent", brand.IsCurent),
             new SQLiteParameter("@ID", brand.ID)
             );
        }

        public int switchBrandData()
        {
            return db.ExecuteNonQuery(@"update T_Brand
                                               set IsCurent=@IsCurent",
             new SQLiteParameter("@IsCurent", 0)
             );
        }

        public int delete(BrandData brand)
        {
            return db.ExecuteNonQuery(@"delete from  T_Brand
                                               where  ID=@ID",
             new SQLiteParameter("@ID", brand.ID)
             );
        }

        public int DeletAppPrgm(int bID) {
            return db.ExecuteNonQuery(@"delete from  程序参数
                                               where  BrandID=@BrandID",
             new SQLiteParameter("@BrandID", bID)
             );
        }
    }

    /// <summary>
    /// 品牌管理
    /// </summary>
    public class BrandData
    {
        public int ID { get; set; }
        public string BrandName { get; set; }
        public int IsCurent { get; set; }
    }
}
