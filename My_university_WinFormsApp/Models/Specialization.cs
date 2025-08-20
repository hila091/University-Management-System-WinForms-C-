
namespace My_university_WinFormsApp.Models
{
    public class Specialization : StudyTrack
    {
        public Specialization(string name) : base(name) { }
        public Specialization() { }

        /* המטרה שלה היא להכיל את תחום ההתמחות את הקורסים שהופכים את המסלול למה שהוא  Specialization ואז יש את המחלקה 
         * כלומר אם באוניברסיטה יש תואר במחשבים ואז יש כמה סוגים של תואר במחשבים אחד שמתמחה יותר בבינה מלאכותית
         * ואחר שמתמקצע יותר בתחום המתמטי אז ההתמחות נכנסת כאן כדי לתת את הקורסים שהופכים את המסלול לאותה ההתמחות
         */

        public static  Specialization LoadingCourse(string SpecializationName)
        {
            Specialization sp = new Specialization(SpecializationName);  // מכין מסלול ליבה חדש
            Course c;
            Lecturer l; 
            string path = @"..\..\..\..\Files\allStudyTrack.txt";

            if (File.Exists(path))
            {
                string[] splitLine;
                string line;

                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = sr.ReadLine();
                        // קוראת 2 שורות כי השורה הראשונה זה מסלול ליבה והשורות הבאות עד לכוכבית זה המסלולי התמחות
                       
                        while (line != "*") {
                            splitLine = line.Split(',');
                           
                            if (splitLine[0] == SpecializationName)
                            {

                                if (splitLine.Length < 1)
                                {
                                    MessageBox.Show("המסלול" + SpecializationName + "לא מכיל קורסים כרגע");
                                    return sp;
                                }
                                for (int i = 1; i < splitLine.Length; i++)
                                {
                                    c = Course.LoadingCourse(splitLine[i]);// מחזיק בקורס אחד מתוך הרשימה                             
                                    l = Lecturer.getNameByCourse(splitLine[i]);
                                    if (l == null)
                                        l = HeadOfDepartment.getNameByCourse(splitLine[i]);
                                    c.Teacher = l;

                                    sp.Courses.Add(c);
                                    sp.Students = sp.Students.Union(c.Students).ToList(); // מעדכן את רשימת הסטודנטים ככה שיהיו בלי כפילויות                             
                                }
                                return sp;
                            }
                            line = sr.ReadLine();
                        }
                        while ((line = sr.ReadLine()) != null)
                                if (line == "*")
                                    break;
                             
                    }

                    MessageBox.Show("המסלול" + SpecializationName + "לא נמצא");
                    return null;
                }


            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return null;
        }

        public static List<Specialization> AllSpecializationList(string coreProgram)
        {
            List< Specialization > specializationList = new List< Specialization >();
            Specialization sp;
            Course c;
            Lecturer l;

            string path = @"..\..\..\..\Files\allStudyTrack.txt";

            if (File.Exists(path))
            {
                string [] splitLine;
                List<string> readAllLines = File.ReadAllLines(path).ToList();

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    /* בכל התחלה של כתיבת מחלקה נכתבת שורה ראשונה בשם המחלקה ורשימת הקורסים שהם חובה אצלה
                     * מהשורה השניה ועד לכוכבית זה השם של המסלול התמחות ורשימת הקורסים שצריך לעשות בשביל אותו מסלול
                     */
                    if (readAllLines[i].Split(',')[0] == coreProgram)
                    { 
                        i++; // כדי להגיע לשורה הבאה להתחלה של הפירוט התמחויות
                        while (readAllLines[i]!= "*")
                        {
                            splitLine = readAllLines[i].Split(',');
                            sp =  new Specialization(splitLine[0]); // שם ההתמחות 
                           
                            for (int k = 1; k < splitLine.Length; k++)
                            {
                                c = Course.LoadingCourse(splitLine[k]);// מחזיק בקורס אחד מתוך הרשימה                             
                                if (c != null) 
                                { 
                                    l = Lecturer.getNameByCourse(splitLine[k]);// אם המרצה לא במרצים אז אולי הוא מרצה שהוא גם ראש מחלקה
                                    if (l == null) // לא יכול להיות קורס בלי מרצה זה למה לא המשכתי לבדיקה נוספת
                                        l = HeadOfDepartment.getNameByCourse(splitLine[k]);
                                    c.Teacher = l;
                                       //   MessageBox.Show(l.Name + " "+l.FmName);
                                 
                                    sp.Courses.Add(c);
                                    sp.Students = sp.Students.Union(c.Students).ToList(); // מעדכן את רשימת הסטודנטים ככה שיהיו בלי כפילויות                             
                                }
                            }
                            specializationList.Add(sp); 

                            i++;
                        }
                        return specializationList;
                    }
                }
                return null;
            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return null;
        }

        public static void AddNewSpecialization(int amountSPInDepartment, string specializationName, string departmentName)
        {
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";
            
            int lastSpecialization = 0; // יחזיק את ההתמחות  האחרונה 
            if (File.Exists(pathAllStudyTrack))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();
                string[] splitLine;

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == departmentName)
                    {
                        readAllLines.Insert(i + amountSPInDepartment + 1, specializationName);

                        lastSpecialization += splitLine.Length;
                        i++;
                        while (readAllLines[i] != specializationName)
                        {
                            splitLine = readAllLines[i].Split(',');
                           // MessageBox.Show(string.Join(",", splitLine));
                            lastSpecialization += splitLine.Length;
                           // MessageBox.Show("lastSpecialization is      "+ lastSpecialization);
                            i++;
                        }

                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }

                    lastSpecialization += splitLine.Length;
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

            if (File.Exists(pathAllCourses))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                int countdown =  lastSpecialization; // נוריד כל פעם כי בקובץ הזה כל קבוצת קורסים נכנסת להתמחות אחת

                for (int i = 0; i < readAllLines.Count; i++)
                {
                   // MessageBox.Show(readAllLines[i] + "-----8888--------->" + "countdown" + countdown);

                    if (countdown > 0)
                        countdown--;
                    else
                    {
                        readAllLines.Insert(i, "*");
                        break;
                    }
                }

                File.WriteAllLines(pathAllCourses, readAllLines);

            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllCourses);

        }

        public static void UpdateSpecializationName( string newSpecializationName, string oldSpecializationName)
        {
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";

            if (File.Exists(pathAllStudyTrack))
            {
                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();
                string[] splitLine;

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == oldSpecializationName) 
                    {
                        splitLine[0] = newSpecializationName;

                        readAllLines[i] = string.Join(",", splitLine);
                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }
                       
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);
        }

        
    
    
    }
}
