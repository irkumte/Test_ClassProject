using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CodeScreen.Assessments.TradeCancelling
{
    static class ExcessiveTradeCancellingChecker
    {
       public static List<string> lstcancellingCompanies ;
        public static int noOfCompanies = 0;
        public static List<string> CompaniesInvolvedInExcessiveCancellations()
        {
            // Returns the list of companies that are involved in excessive cancelling.
            //TODO Implement
            readcsv();
            return lstcancellingCompanies;
        }

        public static int TotalNumberOfWellBehavedCompanies()
        {
            // Returns the total number of companies that are not involved in any excessive cancelling.
            //TODO Implement
            return lstcancellingCompanies.Count;
        }

        private static void readcsv()
        {
            DirectoryInfo df = new DirectoryInfo(Directory.GetCurrentDirectory().ToString());
            FileInfo[] files;
            FileInfo datafile = null;
            bool dataFileFound = false;
            while(1==1)
            {
                df = new DirectoryInfo(df.Parent.FullName);
                files = df.GetFiles();
                foreach(FileInfo fileInfo in files)
                {
                    if(fileInfo.Name=="Trades.data")
                    {
                        datafile = fileInfo;
                        dataFileFound = true;
                        break;
                    }
                }

                if(dataFileFound)
                {
                    break;
                }
            }

            var lines = File.ReadAllLines(datafile.FullName);//.Select(a => a.Split(';'));
            //var csv = from line in lines
            //          select (from piece in line
            //                  select piece);

            lstcancellingCompanies = new List<string>();

            

           

            string[] splitItem;
            string[] splitItem2;
            DateTime dt1;
            DateTime dt2;
            double cancellingRatio=0;
            int quantity1 = 0;
            int quantity2 = 0;
            int faultQuantity = 0;
            double ratio = 1;

            List<string> iterated = new List<string>();
          
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    
                    splitItem = lines[i].Split(',');
               

                    if (splitItem.Length>1 )
                    {

                    if (iterated.Contains(splitItem[1]))
                    {
                        continue;
                    }
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            faultQuantity = 0;
                            cancellingRatio = 0;
                            splitItem2 = lines[j].Split(',');
                            if (splitItem2.Length > 1 && splitItem2[1] == splitItem[1])
                            {
                                dt1 = Convert.ToDateTime(splitItem[0]);
                                dt2 = Convert.ToDateTime(splitItem2[0]);
                                quantity1 = Convert.ToInt32(splitItem[3]);
                                quantity2 = Convert.ToInt32(splitItem2[3]);

                                if ((dt2 - dt1).TotalSeconds <= 60)
                                {
                                    if (splitItem[2] == "F")
                                    {
                                        faultQuantity = quantity1;
                                    }
                                    if (splitItem2[2] == "F")
                                    {
                                        faultQuantity = faultQuantity + quantity2;
                                    }

                                    if (faultQuantity > 0)
                                    {
                                        cancellingRatio =(double)( faultQuantity / (quantity1 + quantity2));
                                    }
                                }

                                if (cancellingRatio > (ratio / 3))
                                {
                                    lstcancellingCompanies.Add(splitItem[1]);                                                                 
                                    break;
                                }
                            }                           

                            
                        }
                    if (!iterated.Contains(splitItem[1]))
                    {
                        iterated.Add(splitItem[1]);
                    }
                    
                }

                   
               
               
                }


            
        }

    }
}
