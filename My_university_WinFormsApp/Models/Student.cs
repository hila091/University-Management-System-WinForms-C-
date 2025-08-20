using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace My_university_WinFormsApp.Models
{
    public class Student : Person
    {
        public static int startID = 1000000; //   סטודנטים מקבלים מספר סטודנט שמתחיל מהמספר הנל
        public int StudentNumber { get; set; }
        public CoreProgram CoreCourse { get ; set; }  // רשימת קורסים ליבתיים עבור אותו מקצוע למידה -> לדוגמה: הנדסאים
        public Specialization SpecializationCourses { get; set; } // רשימת קורסים שקשורים לאותה התמחות עבור אותו מקצוע למידה -> לדוגמה: הנדסאים - הנדסת חשמל

        public int CurrentCredits { get; set; } 
        public int TotalCredits { get; set; }

        public Student(string accountType, string name, string fmName, string age, string phoneNumber, string gmail, string id,
            DateTime birthday, Image profileImage)
            : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            this.StudentNumber = startID++;
            this.CurrentCredits = 0;
            this.TotalCredits = 0;
            this.CoreCourse = new CoreProgram();
            this.SpecializationCourses = new Specialization();    
        }
        public Student(string accountType, string name, string fmName, string age, string phoneNumber, string gmail, string id,
            DateTime birthday, Image profileImage,int studentNumber, int currentCredits, int totalCredits)
            : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            this.StudentNumber = studentNumber;
            this.CurrentCredits = currentCredits;
            this.TotalCredits = totalCredits;
            this.CoreCourse = new CoreProgram();
            this.SpecializationCourses = new Specialization();
        }


        /*   אוקיי אוקיי עשיתי כמה חושבים עם עצמי
         *   אני חושבת שאני אעשה 2 סוגי טעינות של סטודנטים מהקבצים
         *   טעינה אחת תהיה בשביל סטודנט עצמו שרוצה לפתוח את הדף שלו ולראות את הנתונים כמו קורסים שיש לו וחברים שיש לו במחלקה
         *   אחר כך אני אעשה עוד טעינה מקבצים שתציג את הסטודנט בלי הקורסים שלו כלומר אני אשתמש בבנאי הראשון של המידע השטחי יותר
         *   בלי צפיה בקורסים ואז ככה אני אוסיף אותו לסטודנט שפותח את הדף בית 
         *   זה רעיון טוב
         *   נראה לי
         *   ננסה
         */


        public static Student LoadingStudent(string studentNum)
        {
            string path = @"..\..\..\..\Files\Students\students.txt";

            if (File.Exists(path))
            {
                string[] splitLine;
                string line; 
                Student user = null;
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        splitLine = line.Split(',');
                        if (studentNum == splitLine[2])
                        {
                            string name = splitLine[3];
                            string fmName = splitLine[4];
                            string age = splitLine[5];
                            string phoneNumber = splitLine[6];
                            string gmail = splitLine[7];
                            string id = splitLine[8];
                            int studentNumber = int.Parse(splitLine[2]);
                            DateTime birthday = DateTime.ParseExact(splitLine[9], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            Image profileImage = Login.SearchingImgByID(@"..\..\..\..\Files\Students\ProfilePictures", studentNumber);
                            int currentCredits = int.Parse(splitLine[11]);
                            int totalCredits = int.Parse(splitLine[10]);
                            user = new Student("Student", name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, studentNumber, currentCredits, totalCredits);

                            
                            return user;                   
                               
                        }
                        while ((line = sr.ReadLine()) != null)
                            if (line == "*")
                                break;
                    }

                }
            }
            return null;
        }


        public static int FindLastID()
        {
            int lastID = 5000000;
            string pathS = @"..\..\..\..\Files\Students\students.txt"; // קובץ סטודנטים 
            string pathSTA = @"..\..\..\..\Files\Students\studentTA.txt"; // קובץ סטודנטים מתרגלים 

            if (File.Exists(pathS))
            {
                List<string> readAllLines = File.ReadAllLines(pathS).ToList();
                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 3) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",");
                    if (int.Parse(splitLine[2]) > lastID)
                        lastID = int.Parse(splitLine[2]);
                    else
                        if (int.Parse(splitLine[2]) == lastID)
                        MessageBox.Show("יש שני מרצים עם אותו מספר עובד");
                }
            }
            else MessageBox.Show("הנתיבים " + pathS);

            if (File.Exists(pathSTA))
            {
                List<string> readAllLines = File.ReadAllLines(pathSTA).ToList();
                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 4) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",");
                    if (int.Parse(splitLine[2]) > lastID)
                        lastID = int.Parse(splitLine[2]);
                    else
                        if (int.Parse(splitLine[2]) == lastID)
                        MessageBox.Show("יש שני מרצים עם אותו מספר עובד");
                }
            }
            else MessageBox.Show("הנתיבים " + pathSTA);

            return lastID++; // כי אם המספר עובד האחרון הוא 500009 נניח אז אני צריכה להכין אותו להבא בתור כבר

        }


        // פעולה למחיקת סטודנט מהקבצים 
        public static bool RemoveStudent(string studentNum)
        {
            string pathS = @"..\..\..\..\Files\Students\students.txt"; // קובץ סטודנטים 
            string pathSTA = @"..\..\..\..\Files\Students\studentTA.txt"; // קובץ סטודנטים מתרגלים 

            if (File.Exists(pathS))
            {
                List<string> readAllLines = File.ReadAllLines(pathS).ToList();
                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 3) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",");
                    if (splitLine[2] == studentNum) //  אם מספר סטודנט נמצא מתאים למספר הסטודנט שאנחנו רוצים למחוק מהקובץ 
                    {
                        readAllLines.RemoveAt(i);
                        readAllLines.RemoveAt(i + 1);
                        readAllLines.RemoveAt(i + 2);
                        Student.RemoveStudentFromCourses(studentNum);
                        return true;
                    }   
                }
                return false;
            }
            else MessageBox.Show("הנתיבים " + pathS);
            return false;
        }

        public static bool RemoveStudentFromCourses(string studentNum)
        { // המטרה של הפעולה כאן היא לקחת סטודנט ולהסיר אותו מכל קורס קיים שהוא רשום בו 
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt"; // קובץ שמאחסן את כל הקורסים
            /*
                 קובץ כזה בנוי במבנה הזה 

                תכנות מונחה עצמים,1000000,1000001,1000002,1000003,1000004,1000005
                ניתוח מערכות,1000000,1000001,1000002,1000003,1000004,1000005
                מסדי נתונים,1000000,1000001,1000002,1000003,1000004,1000005
                פיתוח תוכנה מתקדם,1000000,1000001,1000002,1000003,1000004,1000005
                בדיקות תוכנה,1000000,1000001,1000002,1000003,1000004,1000005
                *
                
                עבור כל מסלול כלשהו במחלקה בין אם זה מסלול ליבה או מסלול התמחות יש לו רשימת קורסים שמופיעים בקבוצה
                עבור כל קבוצה יופיע קודם שם הקורס שנמצא באותו מסלול ואחריו רשימה של המספרי סטודנטים שמשתתפים בקורס הזה
                כאן למשל רואים מסלול התמחות של הנדסת תוכנה , מופיעות בו כל הרשומות של הקורסים שנלמדים בו וכל הסטודנטים
                שעוברים את אותו הקורס, אחרי כל קבוצה כזאת תירשם כוכבית להפרדה
                  
             */
            if (File.Exists(pathAllCourses))
            {
                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                List <string> splitLine;
                bool flag = false;
                for (int i = 0; i < readAllLines.Count; i += 3) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    
                    splitLine = readAllLines[i].Split(",").ToList();
                    for(int k=1; k<splitLine.Count; k++)
                        if (splitLine[k] == studentNum)
                        {
                            splitLine.RemoveAt(k);
                            readAllLines[i] = string.Join(",", splitLine);
                            File.WriteAllLines(pathAllCourses, readAllLines);
                            flag = true;    
                        }
                }
                return flag;
            }
            else MessageBox.Show("הנתיבים " + pathAllCourses);
            return false;
        }


        public static bool RemoveCourseFromStudent(string courseName, string studentNum )
        {
            /* המטרה של הפעולה היא לקבל שם קורס ולהסיר אותו מסטודנט, כשהפעולה מקבלת את שם הקורס ומספר הסטודנט */
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt"; // קובץ שמאחסן את כל הקורסים
            
            if (File.Exists(pathAllCourses))
            {
                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                List<string> splitLine;
                bool flag = false;
                for (int i = 0; i < readAllLines.Count; i += 3) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",").ToList(); // שומר 
                    if (studentNum = readAllLines[i].)
                    splitLine = readAllLines[i].Split(",").ToList();
                    for (int k = 0; k < splitLine.Count; k++)
                        if (splitLine[k] == courseName)
                        {
                            splitLine.RemoveAt(k);
                            readAllLines[i] = string.Join(",", splitLine);
                            File.WriteAllLines(pathAllCourses, readAllLines);
                            flag = true;
                        }
                }
                return flag;
            }
            else MessageBox.Show("הנתיבים " + pathAllCourses);
            return false;
        }





    }

}

