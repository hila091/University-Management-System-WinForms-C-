

namespace My_university_WinFormsApp.Models
{
    public class Lecturer : Person
    {
        public static int startID = 5000000 ; // מרצים מקבלים מספר עובד שמתחיל מהמספר הנל
        public int EmployeeNumber { get; set; } 
        public List <Course> Courses { get; set; } // רשימת הקורסים שהמרצה מלמד בפועל
        public string SpecializationName { get; set; } // ההתמחות של אותו מרצה
        public int Rating { get; set; }  = 0;




        public Lecturer(string accountType, string name, string fmName, string age, string phoneNumber, string gmail,
            string id, DateTime birthday, Image profileImage, string specializations) 
            : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            //  יצירת מרצה חדש אין קורסים אין התמחויות הכל נקי בהתחלה
            this.EmployeeNumber = startID++;
            this.Courses = new List<Course>();
            this.SpecializationName = specializations;
        }
        public Lecturer(string accountType, string name, string fmName, string age, string phoneNumber, string gmail,
           string id, DateTime birthday, Image profileImage, string specializations,int employeeNumber )
           : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            // בטעינת מרצה חדש כשרוצים להוסיף לו את המספר עובד ישר ולא לעדכן או ליצור מחדש
            this.EmployeeNumber = employeeNumber;
            this.Courses = new List<Course>();
            this.SpecializationName = specializations;
        }

      
      
        public static Lecturer LoadingFullLecturerByCourse(string otherId, string otherGmail)
        {
           
            string pathL = @"..\..\..\..\Files\Employees\lecturer.txt";
            if (File.Exists(pathL))
            {
                string name = "", fmName = "", age = "", phoneNumber = "", gmail = "", id = ""; int employeeNumber = 0;
                int studentNumber = 0, currentCredits = 0, totalCredits = 0; DateTime birthday = DateTime.MinValue, lastLoginDate = DateTime.MinValue;
                Image profileImage = null;

                string[] splitLine;
                string line;
                string[] saveDetails; 

                using (StreamReader sr = new StreamReader(pathL))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        splitLine = line.Split(','); // שומר את השורה הראשונה של הפרטים על אותו מרצה

                        for (int i = 0; i < splitLine.Length; i++)
                        {
                            MessageBox.Show(splitLine[7].Trim()+" =="+ otherGmail.Trim()+"&&"+ splitLine[8].Trim()+"=="+ otherId.Trim());
                            if (splitLine[7].Trim() == otherGmail.Trim() && splitLine[8].Trim() == otherId.Trim()) // לוודא שאני לא מכלילה את עצמו ברשימה
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

                                line = sr.ReadLine();// שורת קורסים 
                                splitLine = line.Split(' ');

                                for (int j = 0; j < splitLine.Length; j++)
                                    userL.Courses.Add(Course.LoadingCourse(splitLine[i]));

                                line = sr.ReadLine();// שורת ההודעות 
                                userL.Messages = UserMessage.LodingUserMessages(line);

                                return userL;
                            }
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


        public static Lecturer getNameByCourse(string courseName)
        {
          
            string pathL = @"..\..\..\..\Files\Employees\lecturer.txt";
            if (File.Exists(pathL))
            {
                string name = "", fmName = "", age = "", phoneNumber = "", gmail = "", id = ""; int employeeNumber = 0;
                int studentNumber = 0, currentCredits = 0, totalCredits = 0; DateTime birthday = DateTime.MinValue, lastLoginDate = DateTime.MinValue;
                Image profileImage = null;

                string[] splitLine;
                string line;
                string[] saveDetails;

                using (StreamReader sr = new StreamReader(pathL))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        saveDetails = line.Split(','); // שומר את השורה הראשונה של הפרטים על אותו מרצה
                       
                        line = sr.ReadLine(); // שורה שניה בקובץ זה הרשימת שמות של הקורסים שהוא מלמד
                        splitLine = line.Split(',');
                        for (int i = 0; i < splitLine.Length-1; i++)
                        {
                            
                            if (courseName.Trim() == splitLine[i].Trim())
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
                                Lecturer userL = new Lecturer("Lecturer", name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, saveDetails[11], employeeNumber);

                                userL.Rating = int.Parse(saveDetails[12]);
                                userL.LastLoginDate = DateTime.Parse(saveDetails[10]);



                                return userL;
                            }
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


        public List<Lecturer> allLecturersForLecturer()
        {
            List<Lecturer> allLecturers = new List<Lecturer>();

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
                    
                    if (splitLine[11].Trim() == this.SpecializationName.Trim() && splitLine[8].Trim() != this.Id.Trim()) // לוודא שאני לא מכלילה את עצמו ברשימה
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
                    }
                }
               
            }
            else MessageBox.Show(" יש בעיה בנתיב");






            return allLecturers;
        }


       public static bool ExistenceOfLecturer (string id, string gmail)
       {

            string pathL = @"..\..\..\..\Files\Employees\lecturer.txt";
            string[] splitLine;


            if (File.Exists(pathL))
            {
                List<string> readAllLines = File.ReadAllLines(pathL).ToList();

                for (int i = 0; i < readAllLines.Count; i += 4)
                {
                    splitLine = readAllLines[i].Split(',');

                    //MessageBox.Show(splitLine[7]+"=="+ gmail+"&&"+ splitLine[8]+"=="+ id);

                    if (splitLine[7].Trim() == gmail.Trim() || splitLine[8].Trim() == id.Trim()) // בדיקה שהמרצה לא קיים על סמך תז ואימייל
                    {
                        MessageBox.Show( "לא תוכל ליצור מרצה חדש שקיים במערכת");
                        return true;
                    }
                }
                return false;
            }
            else MessageBox.Show(" יש בעיה בנתיב");
            return true;
       }


        /* סבבה אז עכשיו זה החלק הסופר דופר קריטי, כל פעם שמריצים את האפליקציה המערכת מבחינתה
           כשהיא תיצור את המרצה הבא בין אם הוא ראש מחלקה או פשוט מרצה, היא תתחיל מהמספר עובד 5000000 
           וזה מאוד מאוד מאוד לא טוב כי אז יהיו לי כפילויות כל פעם שאפתח את התוכנה וכו וכו 
          שלה startID משמע צריך פונקציה שכל פעם שנריץ את התוכנית, הדבר הראשון שהיא תעשה תהיה לעדכן את ה 
          להיות למספר עובד הכי גבוה וממנו היא תמשיך לספור
           (אני יודעת שגם זה לא חכם והיה עדיף מילון כי כשנמחוק מרצה אז המספר עובד שלו יעלם איתו 
             אבל כבר לא יהיה מנוצל וחבל כי יש גבול לכמות העובדים ובסוף יגיעו אליה
             אבלללל אני אעשה את זה ככה בנתיים ובלי מילון כי זמן אבל זה רעיון והיה יכול להיות מגניב לממש אותו)
        */

        public static int FindLastID()
        {
            int lastID = 5000000;
            string pathL = @"..\..\..\..\Files\Employees\lecturer.txt"; // קובץ מרצים
            string pathHD = @"..\..\..\..\Files\Employees\headOfDepartment.txt"; // קובץ ראשי המחלקות

            if (File.Exists(pathL))
            {
                List<string> readAllLines = File.ReadAllLines(pathL).ToList();
                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 4) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",");
                    if (int.Parse(splitLine[2]) > lastID)
                        lastID = int.Parse(splitLine[2]);
                }
            }
            else MessageBox.Show("הנתיבים "+ pathL);

            if (File.Exists(pathHD))
            {
                List<string> readAllLines = File.ReadAllLines(pathHD).ToList();
                string[] splitLine;
                for (int i = 0; i < readAllLines.Count; i += 4) //בקפיצות של 4 כי זה קובץ של מרצים
                {
                    splitLine = readAllLines[i].Split(",");
                    if (int.Parse(splitLine[2]) > lastID)
                        lastID = int.Parse(splitLine[2]);
                }
            }
            else MessageBox.Show("הנתיבים " + pathHD);

            return   lastID++ ; // כי אם המספר עובד האחרון הוא 500009 נניח אז אני צריכה להכין אותו להבא בתור כבר

        }

    }
}
