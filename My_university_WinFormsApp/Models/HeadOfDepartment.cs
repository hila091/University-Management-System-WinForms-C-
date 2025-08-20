

using System.Text.RegularExpressions;

namespace My_university_WinFormsApp.Models
{
    public class HeadOfDepartment : Lecturer
    {
        public CoreProgram CoreCourse { get; set; } // נניח הוא אחראי על מחלקת רפואה אז זה יכיל את הקורסים הבסיסיים לתואר ברפואה
        public List<Specialization>  SpecializationCourses { get; set; } // ואז יש רשימה של כל התתי מחלקות שהוא אחראי עליהם בתוך רפואה
        public static List<Person> RegistrationRequest { get; set; } = new List<Person>(); // רשימת בקשות הירשמות

        /*                             RegistrationRequest <---  הסבר חשוב לגבי 
         * אני לא רוצה שסטודט שרושם את עצמו יוכל פשוט.. לרשום את עצמו
         * הכל צריך לעבור דרך הראש מחלקה כמו במערכת אוניברסיטאית אמיתית , שבה הסטודנט לא יכול לרשום את עצמו
         * אלא לבקש ולהסדיר את זה מול האוניברסיטה
         * אבל בגלל שאני לא באמת כותבת מערכת אוניברסיטאית אמיתית וגדולה אז נעשה את זה בדרך הזאת
         * 
         * אני לא רוצה שכשמשתמש כלשהו רושם את עצמו (חוץ מראש המחלקה עצמו) יהיה לו מותר פשוט להכניס את עצמו למערכת
         * בשבילי זה אומר גם לכתוב אותו לקובץ ולא בא לי את זה כי זאת תהיה מערכת פרוצה
         * אז כל משתמש חדש שנרשם למערכת יוצר לעצמו כיאילו את האובייקט שלו והוא מתווסף לרשימה הזאת
         * 
         * הפרופיל שלו יהיה לא מציאותי לא קיים באמת אבל אצל ראש המחלקה תתקבל הודעה של ניסיון התחברות ובקשה לפרופיל חדש
         * רק הוא יוכל לאשר את ההתחברות שלו ולצרף אותו למערכת כולל כתיבת קבצים
         * עד אז האובייקט שלו נמצא בהולד ברשימה הנוכחית הזאת
         * 
         * איך זה יראה בפועל? המשתמש ינסה ליצור מהתמש , יצליח אבל תהיה לו הודעה שהפרופיל מושבת עד להסכמה של ראש המחלקה
         * יחד עם המשתמש והסיסמה שהמערכת תיתן לו 
         * אצל ראש המחלקה תתקבל בקשה חדשה אם הוא יאשר החשבון יואשר והמשתמש יוכל להיכנס 
         * 
         */

        public HeadOfDepartment(string accountType, string name, string fmName, string age, string phoneNumber, 
            string gmail, string id, DateTime birthday, Image profileImage, string classSubject) // <------ כאן במקום להעביר למרצה את ההתמחות של הראש מחלקה מעבירים את השם של המחלקה 
            : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, classSubject)
        {
            // זה הזה של הירושה הוא מעביר את הפרטים שקשורים לבן אדם לבנאי שלו  <---- base 
            this.EmployeeNumber = startID++;
            RegistrationRequest=new List<Person>();
            this.SpecializationCourses = new List<Specialization>();
            this.CoreCourse = new CoreProgram();
        }
        public HeadOfDepartment(string accountType, string name, string fmName, string age, string phoneNumber,
           string gmail, string id, DateTime birthday, Image profileImage, string classSubject, int employeeNumber)
           : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, classSubject, employeeNumber)
        {
            // זה הזה של הירושה הוא מעביר את הפרטים שקשורים לבן אדם לבנאי שלו  <---- base 
            this.EmployeeNumber = employeeNumber;
            RegistrationRequest = new List<Person>();
            this.SpecializationCourses = new List<Specialization>();
            this.CoreCourse = new CoreProgram();
        }

        //---------- פעולות הוספה ------------      

        //---------- פעולות הסרה ------------

        //---------- פעולות יצירה ------------

        public static Student CreateStudent(string accountType, string name, string fmName, string age, string phoneNumber, 
            string gmail, string id, DateTime birthday, Image profileImage)
        {
            Student newStudent = new Student(accountType, name,  fmName,  age,  phoneNumber,  gmail,  id,  birthday, profileImage);
            return newStudent;  
        }

        public static StudentTA CreateStudentTA(string accountType, string name, string fmName, string age, string phoneNumber,
           string gmail, string id, DateTime birthday, Image profileImage)
        {
            StudentTA newStudent = new StudentTA(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage);
            return newStudent;
        }

        public static Lecturer CreateLecturer(string accountType, string name, string fmName, string age, string phoneNumber,
           string gmail, string id, DateTime birthday, Image profileImage, string specializations)
        {
            Lecturer newStudent = new Lecturer(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, specializations);
            return newStudent;
        }
        // -------------------------------------------------------------------------------------->   פעולות עזר נוספות 

        public static bool RequestAlreadySent(string gmail, string id)
        {
          /*                AddRequestToFile <---  הסבר חשוב לגבי
           *   פעולה זאת מטרתה לכתוב בקובץ שנגיש לכל ראשי המחלקות שיהיו, הוא יכיל את כל הבקשות 
           *   של יצירת חשבונות דשים ויחכה לאישור שלהם לפני הכנסה שלהם למערכת
           *   
           *                                              :תזכיר
           * ---------------------------------------------------
           *                      סיסמה  --> תהיה התז של המשתמש     
           *  שם המשתמש --> יהיה ההתחלה של המייל שלו עד לשטרודל 
           * ---------------------------------------------------
           * 
           * לא קיים אימיילים זהים לחשבונות שונים ותז הוא גם ייחודי לכל בן אדםלכן השם משתמש והסיסמה לא יכולים ליווצר פעמיים
           * מה שכן יכול לקרות זה שמשתמש יגיש בקשה פעמיים ליצירת אותו חשבון וזה ידפוק את המערכת
           * 
           * נ.ב הפעולה לא תקרא מקבצים כי כשהתוכנית רצה הדבר הראשון שיקרה יהיה טעינה מהקבצים כלומר הרשימת בקשות הציבורית
           * של ראשי המחלקה תהיה נגישה וככה יהיה קל יותר לבדוק כפילויות 
           */

            if(RegistrationRequest.Count > 0) {
                for (int i = 0; i < RegistrationRequest.Count; i++)
                    if (RegistrationRequest[i].Id == id || RegistrationRequest[i].Gmail == gmail)
                    {
                        MessageBox.Show("כבר ניסית ליצור חשבון עם המשתמש הזה- אנא ממך, תגוון");
                        return true;
                    }
                return false;
            }
            return false;

        }

        public static void AddRequestToFile(Person person, string requestType)
        {
            string path = @"..\..\..\..\Files\Employees\newRequests\accountRequests.txt"; //כתיבה לתיקיית הבקשות 
            string imgPath = @"..\..\..\..\Files\Employees\newRequests\ProfilePictures";


            if (File.Exists(path) && !RequestAlreadySent(person.Gmail, person.Id)) // אם הבקשה עוד לא נשלחה והקובץ קיים הוא יכתוב את הבקשה
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    MessageBox.Show("phoneNumber" + person.PhoneNum + "gmail" + person.Gmail + "id" + person.Id + "birthday" + person.Birthday.ToString("dd/MM/yyyy"));

                    sw.Write(requestType + ",");
                    sw.Write(person.Name + ",");
                    sw.Write(person.FmName + ",");
                    sw.Write(person.Age + ",");
                    sw.Write(person.PhoneNum + ",");
                    sw.Write(person.Gmail + ",");
                    sw.Write(person.Id + ",");
                    sw.Write(person.Birthday.ToString("dd/MM/yyyy") + ",");
                    sw.Write(person.UserName + ",");
                    sw.Write(person.Password + ",");

                    sw.WriteLine();

                }
                MessageBox.Show("הכתיבה הצליחה! ");
            }
            else MessageBox.Show("הכתיבה נכשלה");

            //------------------------------- שמירת התמונה בקובץ התמונות של הסטודנטים

            if (Directory.Exists(imgPath)) // כי תיקייה לא קובץ Directory
            {
                person.ProfileImage.Save(imgPath + "/" + person.Id + ".png", System.Drawing.Imaging.ImageFormat.Png);

                MessageBox.Show("שמירת תמונה נעשתה בהצלחה!");
            }
            else MessageBox.Show("שמירת תמונה נכשלה");


        }




        // -------------------------------------------------------------------------------------->   פעולות כתיבה לקבצים 


        public static void WriteToFile(Person person)
        { // רק לראש מחלקה יש גישה להרשאת משתמש חדש כלומר צריבה שלו בקבצים מה שיכנה לו מקום במערכת אז הכל מכאן:

            switch (person)
            {
                //-------------------------------------  StudentTA

                case StudentTA newTAS:
                    WriteStudentTAToFile(newTAS);
                    break;

                //-------------------------------------  Student

                case Student newS:
                    WriteStudentToFile(newS);
                    break;

                //-------------------------------------  HeadOfDepartment

                case HeadOfDepartment newHD:
                    WriteHeadOfDepartmentToFile(newHD);
                break;

                //-------------------------------------  Lecturer

                case Lecturer newL:
                    WriteLecturerToFile(newL);
                break;

            }
        }
       
        public static void WriteStudentToFile(Student newS)
        {
            /*
             פורמט שמירת סטודנט לקובץ - כל סטודנט 2 שורות ונגמר בכוכבית

             שורה ראשונה: שם משתמש,סיסמה,מספר,שם,שם משפחה,גיל,מספר טלפון,אימייל,תז,תאריך יומו,נק סהכ,נק נוכחיות,קורס ליבה,קורס התמחות
                                                                                                שורה שניה: ~ מידע על ההודעות שהם מקבלים
            */

            string path = @"..\..\..\..\Files\Students\students.txt";
            string imgPath = @"..\..\..\..\Files\Students\ProfilePictures";

            if (File.Exists(path))
            {
              
                List<string> readAllLines = File.ReadAllLines(path).ToList();
                bool found = false;

                for (int i = 0; i < readAllLines.Count; i += 3)
                {
                    if (readAllLines[i].Split(',')[0] == newS.UserName)
                    {  

                        readAllLines[i] = newS.UserName + "," + newS.Password + "," + newS.StudentNumber + "," + newS.Name + "," +
                                          newS.FmName + "," + newS.Age + "," + newS.PhoneNum + "," + newS.Gmail + "," + newS.Id + "," +
                                          newS.Birthday.ToString("dd/MM/yyyy") + "," + newS.TotalCredits + "," + newS.CurrentCredits + "," + newS.LastLoginDate + "," +
                                          newS.CoreCourse.Name + "," + newS.SpecializationCourses.Name + ",";

                        

                        readAllLines[i + 1] = UserMessage.ConvertlistToString(newS.Messages);
                        readAllLines[i + 2] = "*";
                        File.WriteAllLines(path, readAllLines);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    readAllLines.Add(newS.UserName + "," + newS.Password + "," + newS.StudentNumber + "," + newS.Name + "," +
                                     newS.FmName + "," + newS.Age + "," + newS.PhoneNum + "," + newS.Gmail + "," + newS.Id + "," +
                                     newS.Birthday + "," + newS.TotalCredits + "," + newS.CurrentCredits + "," + newS.LastLoginDate + "," +
                                     newS.CoreCourse.Name + "," + newS.SpecializationCourses.Name + ",");

                    if (newS.Messages.Count > 0)
                    {
                        string messages = "";
                        for (int k = 0; k < newS.Messages.Count; k++)
                        {
                            for (int j = 0; j < 3; j++)
                                messages += newS.Messages[k].SenderInfro[j] + ",";
                            messages += newS.Messages[k].SenderInfro[^1];

                            messages += newS.Messages[k].myMessage + "|" + newS.Messages[k].SendDate + "|" + newS.Messages[k].Favor + "~";
                        }
                        readAllLines.Add(messages);
                    }
                    else
                        readAllLines.Add(UserMessage.ConvertlistToString(newS.Messages));

                    readAllLines.Add("*");

                    File.WriteAllLines(path, readAllLines);
                }
            }
            else MessageBox.Show("הכתיבה לקובץ סטודנטים נכשלה");
             

            /* רציתי לתת אפשרות למשתמש להחליף את התמונה אבל לא הספקתי 
               בנתיים לא הבנתי למה הוא אף פעם לא היה שומר את התמונה
               מסתבר שזה בגלל שהיא פתוחה בעמוד שמוצג באותו הרגע כי כשאני משתמשת 
               בתמונה אז יש נעילה אוטומטית לקובץ אז זה מגניב לראות את זה קורה 
            */
            //if(Directory.Exists(imgPath))
            //{
            //    string fullPath = Path.Combine(imgPath, newS.StudentNumber + ".png");
            //    newS.ProfileImage.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);

            //    MessageBox.Show("שמירת תמונה נעשתה בהצלחה!");
            //}
            //else MessageBox.Show("שמירת תמונה נכשלה");
        }

        public static void WriteStudentTAToFile(StudentTA newTAS)
        {
            /*
             פורמט שמירת סטודנט מתרגל לקובץ - כל סטודנט מתרגל 3 שורות ונגמר בכוכבית

             שורה ראשונה: שם משתמש,סיסמה,מספר,שם,שם משפחה,גיל,מספר טלפון,אימייל,תז,תאריך יומו,נק סהכ,נק נוכחיות,קורס ליבה,קורס התמחות
                                                                       שורה שניה: ~ שמות הקורסים אותו הם מדריכים
                                                                       שורה שלישית: ~ מידע על ההודעות שהם מקבלים
            */

            string path = @"..\..\..\..\Files\Students\studentTA.txt";
            string imgPath = @"..\..\..\..\Files\Students\ProfilePictures";

            if (File.Exists(path))
            {
                List<string> readAllLines = File.ReadAllLines(path).ToList();
                string courses;
                bool found = false;

                for (int i = 0; i < readAllLines.Count; i += 4)
                {
                    if (readAllLines[i].Split(',')[0] == newTAS.UserName) // אם הסטודנט כבר קיים
                    {
                        readAllLines[i] = newTAS.UserName + "," + newTAS.Password + "," + newTAS.StudentNumber + "," + newTAS.Name + "," +
                                          newTAS.FmName + "," + newTAS.Age + "," + newTAS.PhoneNum + "," + newTAS.Gmail + "," + newTAS.Id + "," +
                                          newTAS.Birthday.ToString("dd/MM/yyyy") + "," + newTAS.TotalCredits + "," + newTAS.CurrentCredits + "," + newTAS.LastLoginDate + ","+
                                          newTAS.CoreCourse.Name + "," + newTAS.SpecializationCourses.Name + ",";
                        courses = "";
                        for (int k = 0; k < newTAS.CourseTaught.Count; k++)
                            courses += newTAS.CourseTaught[k].Name + ",";

                        readAllLines[i + 1] = courses;

                        readAllLines[i + 2] = UserMessage.ConvertlistToString(newTAS.Messages);

                        readAllLines[i + 3] = "*";

                        File.WriteAllLines(path, readAllLines);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    readAllLines.Add(newTAS.UserName + "," + newTAS.Password + "," + newTAS.StudentNumber + "," + newTAS.Name + "," +
                                     newTAS.FmName + "," + newTAS.Age + "," + newTAS.PhoneNum + "," + newTAS.Gmail + "," + newTAS.Id + "," +
                                     newTAS.Birthday + "," + newTAS.TotalCredits + "," + newTAS.CurrentCredits + "," + newTAS.LastLoginDate + ","+
                                     newTAS.CoreCourse.Name + "," + newTAS.SpecializationCourses.Name + ",");
                    courses = "";
                    for (int k = 0; k < newTAS.CoreCourse.Courses.Count; k++)
                        courses += newTAS.CoreCourse.Courses[k].Name + ",";
                    readAllLines.Add(courses);

                    if (newTAS.Messages.Count > 0)
                    {
                        string messages = "";
                        for (int k = 0; k < newTAS.Messages.Count; k++)
                        {
                            for (int j = 0; j < 3; j++)
                                messages += newTAS.Messages[k].SenderInfro[j] + ",";
                            messages += newTAS.Messages[k].SenderInfro[^1];

                            messages += newTAS.Messages[k].myMessage + "|" + newTAS.Messages[k].SendDate + "|" + newTAS.Messages[k].Favor + "~";
                        }
                        readAllLines.Add(messages);
                    }
                    else
                        readAllLines.Add("-");

                    readAllLines.Add("*");

                    File.WriteAllLines(path, readAllLines);
               
                }
            }
            else MessageBox.Show("הכתיבה לקובץ סטודנטים מתרגלים נכשלה");

            //if (Directory.Exists(imgPath))
            //{
            //    string fullPath = Path.Combine(imgPath, newTAS.StudentNumber + ".png");
            //    newTAS.ProfileImage.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);

            //    MessageBox.Show("שמירת תמונה נעשתה בהצלחה!");
            //}
            //else MessageBox.Show("שמירת תמונה נכשלה");
        }

        public static void WriteLecturerToFile(Lecturer newL)
        {
            /*      פורמט שמירת מרצה לקובץ – כל מרצה נשמר ב-3 שורות:
             * שורה ראשונה: שם משתמש, סיסמה, מספר עובד, שם, שם משפחה, גיל, טלפון, אימייל, ת"ז, תאריך לידה, התחברות אחרונה, התמחות, דירוג
             *                                                                                           שורה שניה: שמות הקורסים שהוא מלמד
             *                                                                                    שורה שלישית: ~ מידע על ההודעות שהוא מקבל
             */

            string path = @"..\..\..\..\Files\Employees\lecturer.txt";
            string imgPath = @"..\..\..\..\Files\Employees\ProfilePictures";

            if (File.Exists(path))
            {
                List<string> readAllLines = File.ReadAllLines(path).ToList();
                string courses;
                bool found = false;

                for (int i = 0; i < readAllLines.Count; i+=4)
                {
                    if (readAllLines[i].Split(',')[0] == newL.UserName)
                    {
                        readAllLines[i] = newL.UserName + "," + newL.Password + "," + newL.EmployeeNumber + "," + newL.Name + "," +
                                          newL.FmName + "," + newL.Age + "," + newL.PhoneNum + "," + newL.Gmail + "," + newL.Id + "," +
                                          newL.Birthday.ToString("dd/MM/yyyy") + "," + newL.LastLoginDate + "," + newL.SpecializationName 
                                          + "," + newL.Rating + ",";


                        if (newL.Courses.Count == 0)
                            courses = "-";
                        else
                        {
                            courses = "";
                            for (int k = 0; k < newL.Courses.Count; k++)
                                courses += newL.Courses[k].Name + ",";
                        }

                        readAllLines[i + 1] = courses;

                        readAllLines[i + 2] = UserMessage.ConvertlistToString(newL.Messages);

                        readAllLines[i + 3] = "*";
                        File.WriteAllLines(path, readAllLines);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    readAllLines.Add(newL.UserName + "," + newL.Password + "," + newL.EmployeeNumber + "," + newL.Name + "," +
                                     newL.FmName + "," + newL.Age + "," + newL.PhoneNum + "," + newL.Gmail + "," + newL.Id + "," +
                                     newL.Birthday.ToString("dd/MM/yyyy") + "," + newL.LastLoginDate + "," + newL.SpecializationName + "," + newL.Rating + ",");

                    courses = "";
                    for (int k = 0; k < newL.Courses.Count; k++)
                        courses += newL.Courses[k].Name + ",";
                    readAllLines.Add(courses);

                    if (newL.Messages.Count > 0)
                    {
                        string messages = "";
                        for (int k = 0; k < newL.Messages.Count; k++)
                        {
                            for (int j = 0; j < 3; j++)
                                messages += newL.Messages[k].SenderInfro[j] + ",";
                            messages += newL.Messages[k].SenderInfro[^1];

                            messages += newL.Messages[k].myMessage + "|" + newL.Messages[k].SendDate + "|" + newL.Messages[k].Favor + "~";
                        }
                        readAllLines.Add(messages);
                    }
                    else
                        readAllLines.Add("-");

                    readAllLines.Add("*");
                    File.WriteAllLines(path, readAllLines);
                    MessageBox.Show("הכתיבה לקובץ מרצים הצליחה!");
                }
            }
            else MessageBox.Show("הכתיבה לקובץ מרצים נכשלה");

            //if (Directory.Exists(imgPath))
            //{
            //    string fullPath = Path.Combine(imgPath, newL.EmployeeNumber + ".png");
            //    newL.ProfileImage.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);

            //    MessageBox.Show("שמירת תמונה נעשתה בהצלחה!");
            //}
            //else MessageBox.Show("שמירת תמונה נכשלה");
        }

        public static void WriteHeadOfDepartmentToFile(HeadOfDepartment newHD)
        {
            /*                   פורמט שמירת ראש מחלקה לקובץ      כל ראש מחלקה - 3 שורות ונגמר בכוכבית
            *                               
            * שורה ראשונה: שם משתמש,סיסמה,המחלקה שהוא אחראי עליה,מספר,שם,שם משפחה,גיל,מספר טלפון,אימייל,תז,תאריך יומו    
            *                                                                         שורה שניה: שמות הקורסים שהוא מלמד 
            *                                                                 שורה שלישית: ~ מידע על ההודעות שהם מקבלים
            *                                                                  
            */

            string path = @"..\..\..\..\Files\Employees\headOfDepartment.txt"; //כתיבה לקובץ ראשי המחלקות
            string imgPath = @"..\..\..\..\Files\Employees\ProfilePictures";

            if (File.Exists(path))
            {
                List <string> readAllLines = File.ReadAllLines(path).ToList();
                string courses;
                bool found = false; 
                for (int i = 0; i < readAllLines.Count; i+= 4)
                    if (readAllLines[i].Split(',')[0] == newHD.UserName) // אם הוא קיים כבר בקובץ אז נעדכן פשוט 
                    { //  int.Parse(readAllLines[i].Split(',')[2]) כי שם נמצא המספר עובד וצריך לבצע המרה ממחרוזת למספר אז
                        readAllLines[i] = newHD.UserName + "," + newHD.Password + "," + newHD.EmployeeNumber + "," + newHD.Name + "," +
                                          newHD.FmName + "," + newHD.Age + "," + newHD.PhoneNum + "," + newHD.Gmail + "," + newHD.Id + "," +
                                          newHD.Birthday.ToString("dd/MM/yyyy") + "," + newHD.LastLoginDate + "," + newHD.SpecializationName + ",";
                       
                        if (newHD.Courses.Count==0)
                           courses = "-";
                        else
                        {
                            courses = "";
                            for (int k = 0; k < newHD.Courses.Count; k++)
                                courses += newHD.Courses[k].Name + ",";
                        }

                        readAllLines[i + 1] = courses;

                        readAllLines[i + 2] = UserMessage.ConvertlistToString(newHD.Messages);

                        readAllLines[i + 3] = "*";

                        File.WriteAllLines(path, readAllLines);

                        found = true;
                        break;
                    }


                if(!found) {
                    
                    // אם הוא הגיע לכאן סימן שהוא לא מצא אז צריך להוסיף אותו     
                    readAllLines.Add(newHD.UserName + "," + newHD.Password + "," + newHD.EmployeeNumber + "," + newHD.Name + "," +
                                                newHD.FmName + "," + newHD.Age + "," + newHD.PhoneNum + "," + newHD.Gmail + "," + newHD.Id + "," +
                                                newHD.Birthday + "," + newHD.LastLoginDate + "," + newHD.SpecializationName + "," );
                
                    courses = "";
                    for (int k = 0; k < newHD.Courses.Count; k++)
                        courses += newHD.Courses[k].Name + ",";
                    readAllLines.Add(courses);

                    if (newHD.Messages.Count > 0)
                    {
                        string messages = "";
                        for (int k = 0; k < newHD.Messages.Count; k++)
                        {
                            for (int j = 0; j < 3; j++) // כי יש 5 נתונים : שם משתמש של יעד, שם, שם משפחה, ההודעה של השולח, ההודעה של היעד,תאריף, כוכב או לא
                                messages += newHD.Messages[k].SenderInfro[j] + ",";
                            messages += newHD.Messages[k].SenderInfro[^1]; // כי אני לא צריכה פסיק אחרי הערך האחרון מסתבר שכשכותבים ^1 זה אומר האיבר האחרון במערך

                            messages += newHD.Messages[k].myMessage  + "|" + newHD.Messages[k].SendDate + "|" + newHD.Messages[k].Favor + "~";
                        }
                        readAllLines.Add(messages); ;
                    }
                    else
                        readAllLines.Add("-");
                    readAllLines.Add("*");
                    File.WriteAllLines(path, readAllLines);


                    MessageBox.Show("הכתיבה לקובץ תלמידים הצליחה! ");
                }
            }
            else MessageBox.Show("הכתיבה לקובץ סטודנטים נכשלה");

            //------------------------------- שמירת התמונה בקובץ התמונות של הסטודנטים

            //if (Directory.Exists(imgPath))
            //{
            //    string fullPath = Path.Combine(imgPath, newHD.EmployeeNumber + ".png");
            //    newHD.ProfileImage.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);

            //    MessageBox.Show("שמירת תמונה נעשתה בהצלחה!");
            //}
            //else MessageBox.Show("שמירת תמונה נכשלה");


        }

        //---------------------------------------------------------------------------------
        public static HeadOfDepartment getNameByCourse(string courseName)
        {

            string pathHD = @"..\..\..\..\Files\Employees\headOfDepartment.txt";
            if (File.Exists(pathHD))
            {
                string name = "", fmName = "", age = "", phoneNumber = "", gmail = "", id = ""; int employeeNumber = 0;
                int studentNumber = 0, currentCredits = 0, totalCredits = 0; DateTime birthday = DateTime.MinValue, lastLoginDate = DateTime.MinValue;
                Image profileImage = null;

                string[] splitLine;
                string line;
                string[] saveDetails;

                using (StreamReader sr = new StreamReader(pathHD))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        saveDetails = line.Split(','); // שומר את השורה הראשונה של הפרטים על אותו מרצה

                        line = sr.ReadLine(); // שורה שניה בקובץ זה הרשימת שמות של הקורסים שהוא מלמד
                        splitLine = line.Split(',');

                        for (int i = 0; i < splitLine.Length; i++)
                            if (courseName == splitLine[i])
                            {
                                splitLine = line.Split(' ');
                                name = saveDetails[3];
                                fmName = saveDetails[4];
                                age = saveDetails[5];
                                phoneNumber = saveDetails[6];
                                gmail = saveDetails[7];
                                id = saveDetails[8];
                                birthday = DateTime.Parse(saveDetails[9]);
                                employeeNumber = int.Parse(saveDetails[2]);
                                profileImage = Login.SearchingImgByID(@"..\..\..\..\Files\Employees\ProfilePictures", employeeNumber);
                                HeadOfDepartment userHD = new HeadOfDepartment("HeadOfDepartment", name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, saveDetails[11], employeeNumber);

                                userHD.LastLoginDate = DateTime.Parse(saveDetails[10]);

                                return userHD;
                            }


                        while ((line = sr.ReadLine()) != null)
                            if (line == "*")
                                break;
                    }
                    return null;
                }
            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return null;
        }

        public static List<string> getByClassName (string className)
        {
        // פונקציה שמחזירה נתונים על ראש מחלקה בצורה הכי שטחית כלומר יחסית היא לא מחזירה נתונים רגישים וכן היא מחזירה יוזרניים אבל הוא לא נחשף זה בשביל המערכת
            List<string> hdDetails = new List<string>();    

            if (File.Exists(@"..\..\..\..\Files\Employees\headOfDepartment.txt"))
            {
                List<string> readAllLines = File.ReadAllLines(@"..\..\..\..\Files\Employees\headOfDepartment.txt").ToList();

                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i +=4)
                {
               

                    splitLine = readAllLines[i].Split(',');
                   
                    if (splitLine[11] == className)
                    {
                        hdDetails.Add(splitLine[3]); // שם
                        hdDetails.Add(splitLine[4]); // שם משפחה
                        hdDetails.Add(splitLine[7]); // אימייל
                        hdDetails.Add(splitLine[11]); // שם מחלקה 
                        hdDetails.Add(splitLine[0]); // שם משתמש 
                        return hdDetails;
                    }
                   
                }
                return null;
            }
            else MessageBox.Show(" שליחת ההודעה נכשלה יש בעיה בנתיב");
            return null;
        }

        public static List<string> getByName(string hdName)
        {
            List<string> hdDetails = new List<string>();

            if (File.Exists(@"..\..\..\..\Files\Employees\headOfDepartment.txt"))
            {
                List<string> readAllLines = File.ReadAllLines(@"..\..\..\..\Files\Employees\headOfDepartment.txt").ToList();

                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 4)
                {


                    splitLine = readAllLines[i].Split(',');
                    MessageBox.Show(splitLine[3] + " " + splitLine[4] + "="+ hdName);
                    if (splitLine[3]+" "+ splitLine[4] == hdName )
                    {

                        hdDetails.Add(splitLine[3]); // שם
                        hdDetails.Add(splitLine[4]); // שם משפחה
                        hdDetails.Add(splitLine[7]); // אימייל
                        hdDetails.Add(splitLine[11]); // שם מחלקה 
                        hdDetails.Add(splitLine[0]); // שם משתמש
                        return hdDetails;
                    }

                }
                return null;
            }
            else MessageBox.Show(" שליחת ההודעה נכשלה יש בעיה בנתיב");
            return null;
        }


        public List<Lecturer> allLecturersForHD()
        { /* פונקציה שאוספת את כל הפרטים עבור אותו מרצה אבל אלו פרטים שטחיים של טעינה עבור מידע 
              אין כאן מידע לגבי ההודעות שלו או הקורסים שהוא מלמד 
           */
            List<Lecturer> allLecturers = new List<Lecturer>();

            List<string> allSpecializationsName = new List<string>();
            for (int i = 0; i < this.SpecializationCourses.Count; i++)
            {
                allSpecializationsName.Add(this.SpecializationCourses[i].Name);
            }

            string pathL = @"..\..\..\..\Files\Employees\lecturer.txt";
            string[] splitLine;


            if (File.Exists(pathL))
            {
                string name = "", fmName = "", age = "", phoneNumber = "", gmail = "", id = ""; int employeeNumber = 0;
                int studentNumber = 0, currentCredits = 0, totalCredits = 0; DateTime birthday = DateTime.MinValue, lastLoginDate = DateTime.MinValue;
                Image profileImage = null;


                List<string> readAllLines = File.ReadAllLines(pathL).ToList();

                for (int i = 0; i < readAllLines.Count; i += 4)
                {
                    splitLine = readAllLines[i].Split(',');
                    for (int k = 0; k < this.SpecializationCourses.Count; k++)
                    {
                       // MessageBox.Show(allSpecializationsName[k]+"=="+ splitLine[11]+"&&"+ splitLine[8]+"=="+ this.Id);

                        if (allSpecializationsName[k].Trim() == splitLine[11].Trim() && splitLine[8].Trim() != this.Id.Trim())
                        {
                            name = splitLine[3];
                            fmName = splitLine[4];
                            age = splitLine[5];
                            phoneNumber = splitLine[6];
                            gmail = splitLine[7];
                            id = splitLine[8];
                            birthday = DateTime.Parse(splitLine[9]);
                            employeeNumber = int.Parse(splitLine[2]);
                            profileImage = Login.SearchingImgByID(@"..\..\..\..\Files\Employees\ProfilePictures", employeeNumber);
                            Lecturer userL = new Lecturer("Lecturer", name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, splitLine[11], employeeNumber);

                            userL.Rating = int.Parse(splitLine[12]);
                            userL.LastLoginDate = DateTime.Parse(splitLine[10]);

                            allLecturers.Add(userL);
                            break;
                        }
                    }
                   
                }
                return allLecturers;
            }
            else MessageBox.Show(" יש בעיה בנתיב");
            return allLecturers;
        }


    }






}


