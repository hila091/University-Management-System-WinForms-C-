using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace My_university_WinFormsApp.Models
{
    public class UserMessage
    {
        public string myMessage { get; set; } // ההודעה שהמשתמש שולח
        public string[] SenderInfro { get; set; } // שם משתמש של היעד ,סוג שולח, שם, שם משפחה, אם ההודעה במועדפים או לא
        public DateTime SendDate { get; set; }
        public bool Favor { get; set; }

        public UserMessage( string[] SenderInfro,string sendMessage, DateTime SendDate) // תחילת שיחה
        {
           
            this.myMessage = sendMessage; // ההודעה שאני שולחת

            // ואז אני בעצם השולחת אז יהיה את הפרטים שלי 
            this.SenderInfro = SenderInfro; // כאן הפרטים של היעד !!!!!

            this.SendDate = SendDate;
            this.Favor = false;
        }

        public UserMessage(string[] senderInfro, string sendMessage,  DateTime SendDate,bool favor ) 
        {
            this.SenderInfro= senderInfro; // פרטי היעד
            this.myMessage = sendMessage; // ההודעה שאני שולחת
            this.SendDate= SendDate;
            this.Favor = favor;

        }

       
        public static List<UserMessage> LodingUserMessages(string line)
        {
            /* מטרת הפעולה היא לטעון את כל ההודעות שיש למשתמש שהשורת ההודעות מהקובץ שייכת אליו
             * 
             * בין כל הודעה להודעה יש סימן הפרדה ~ 
             * בין כל תכונה לתכונה באובייקט הודעה יש סימן הפרדה |
             * בין כל ערך במערך של המידע על האדם שאליו מיועדת ההודעה יש סימן הפרדה ,
             * 
             * אז אם יש לי שורה ארוכה מאוד שמכילה את כל ההודעות שאדם מקבל
             * כי לא אכפת לי שזה לא קריא לבן אדם זה צריך להיות נוח למכונה 
             * אז זה אומר לקחת את השורה הזאת להפריד אותה ל~ ואז לקבל מערך של כל האובייקטי הודעות 
             * כל אחד מאלה להפריד ל| ובמיקום ה0 בכל אחד מהם להפריד בעזרת, למערך
             * 
             * זה יראה בערך ככה:
             */
            List<UserMessage> userMessages = new List<UserMessage>();

            if (line != "-") {
             

                string[] allMessages = line.Split('~');
                string[] messages;
                string[] destinationInfro;
                UserMessage currentMessage;
                for (int i = 0; i < allMessages.Length-1; i++)
                {
                    messages = allMessages[i].Split('|');
              
                    destinationInfro = messages[0].Split(',');// כי שם הוא מחזזיק את המערך שמכיל את הנתונים של השולח

              
                    currentMessage = new UserMessage( destinationInfro, messages[1],
                    DateTime.ParseExact(messages[2], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), bool.Parse(messages[3]));

                    userMessages.Add(currentMessage);                   
                }
                return userMessages;
            }
            return userMessages;


        }

        //------------------------------------------------------------------------------------------
        public static void DeliverMessage(string path, string message, string [] otherSideInfro, string destUserName) 
        {

            if (File.Exists(path))
            {
               
                // כי סטודנט נכתב כל שתי שורות אחרי כוכבית ואחריו יתחיל סטודנט חדש בשורה הרביעית 
                List<string> readAllLines = File.ReadAllLines(path).ToList();

                string[] splitLine; int messagesIndex; int steps;

                if (path == @"..\..\..\..\Files\Employees\lecturer.txt" || path == @"..\..\..\..\Files\Students\studentTA.txt" || 
                    path == @"..\..\..\..\Files\Employees\headOfDepartment.txt")
                {
                    steps = 4; 
                    messagesIndex = 2;
                }
                else
                {
                     steps = 3;
                     messagesIndex = 1;                                
                }

                //------------------------------------------------------------------------------------------------------------------
                for (int i = 0; i < readAllLines.Count; i += steps)
                {
                   splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == destUserName)
                    { // אם השם משתמש של היעד הוא השם משתמש שכתוב בהודעה אז מצאנו את האיש שלנו
                        // עידכון השורה ושליחת ההודעה                       
                         
                        readAllLines[i+ messagesIndex] = UserMessage.ConvertlistToString(UserMessage.MakeMessage(readAllLines[i + messagesIndex], message, otherSideInfro));
                        File.WriteAllLines(path, readAllLines);     
                        MessageBox.Show("ההודעה נשלחה בהצלחה!");
                        break;
                    }
                }
                   
               
                
            }
            else MessageBox.Show(" שליחת ההודעה נכשלה יש בעיה בנתיב");
        }

        public static string ConvertlistToString(List<UserMessage> list)
        {
            string messages = "";

            if (list.Count > 0)
            {

                for (int k = 0; k < list.Count; k++)
                {
                    /*   תזכיר
                     *   string[] otherSideInfro = { this.user.UserName, this.user.AccountType, this.user.Name, this.user.FmName };
                     *   שם משתמש , סוג חשבון, שם, שם משפחה
                     */
                    for (int j = 0; j < 3; j++)
                        messages += list[k].SenderInfro[j] + ",";
                    messages += list[k].SenderInfro[^1];

                    messages += "|" + list[k].myMessage + "|" + list[k].SendDate + "|" + list[k].Favor+"~";

                }
                return messages;
            }

            return messages = "-"; ;
        }

        public static List<UserMessage> MakeMessage(string line, string message, string[] otherSideInfro)
        {
            /* פעולה שמקבלת את השורה של האדם בקובץ ששומרת את כל ההודעות שיש לו ועושה סדר
           * זה קצת כמו תיבת דואר אז הפעולה הזאת לוקחת את כל מה שיש בתיבת דואר עוברת על המכתבים 
           * ובודקת איזו שיחה שייכת לה , מעדכנת בהתאם ומחזירה לתיבת דואר מחדש 
           * 
           * אז כאן היא בודקת את התיבת דואר של היעד ומכניסה את ההודעה אליו בהתאם
           * הפעולה הזאת קיימת אחרי שנבדק שלא התחילה שיחה 
           * אם שיחה כבר פתוחה עם אותו בן אדם היא לא תאפשר התחלה של שיחה חדשה
           */
            List<UserMessage> list = UserMessage.LodingUserMessages(line);

            if (list.Count == 0) // אם אין לו הודעה ממנו עדיין הוא צריך לשלוח לו חבילה שנראת ככה
            {
                //  dan12Sa, Students, דן, לוי, false
                list.Add(new UserMessage(otherSideInfro, message, DateTime.Now, false));
                return list;
            }


            for (int i = 0; i < list.Count; i++)
                if (list[i].SenderInfro[0] == otherSideInfro[0]) 
                {
                    list[i].myMessage = message;
                    list[i].SendDate = DateTime.Now;
                    return list;
                }
            list.Add(new UserMessage(otherSideInfro, message, DateTime.Now, false));
            return list;

        }


        





    }
}
