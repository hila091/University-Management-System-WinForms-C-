using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace My_university_WinFormsApp.Models
{
    public class Course
    {
        // עבור מחלקת קורס יש את התכונות הבאות:
        public string Name { get; set; } // שם קורס
        public List <Student> Students { get; set; } // רשימת תלמידים שלומדים את הקורס
        public Lecturer Teacher { get; set; }

        public Course(string name)
        {
          
            this.Name = name;
            this.Students = new List<Student>();


        }

        public Course(string name, Lecturer newTeacher)
        {
           
            this.Name = name;
            this.Teacher = newTeacher;
            this.Students = new List<Student>();
            // הוא שולח בקשה למחלקה של המרצה להשתמש בפעולה שמביאה אובייקט מסוג מרצה על סמך אותו שם קורס שהוא מלמד
        }


        
      

        public static Course LoadingCourse(string courseName)
        {
            Student s;
            Course c = new Course(courseName);  // מכין קורס חדש עם השם שלו 
            // הדבר הבא זה ללכת לקובץ של הקורסים ולאסוף את המספרי סטודנט מהשורה הנכונה ולהכניס לרשימה
            string path = @"..\..\..\..\Files\allCourses.txt";

            if (File.Exists(path))
            {
                string[] splitLine;
                string line;
                List<string> list;

                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        splitLine = line.Split(',');
                       
                        if (splitLine[0].Trim() == courseName.Trim())
                        {

                            if (splitLine.Length < 1)
                            {
                                MessageBox.Show("הקורס" + courseName + "לא מכיל סטודנטים כרגע");
                                return c;
                            }

                            for (int i = 1; i < splitLine.Length; i++)
                            {
                                s = Student.LoadingStudent(splitLine[i]);
                                if(s== null )
                                    s = StudentTA.LoadingStudentTA(splitLine[i]);
                                if(s!=null)
                                c.Students.Add(s);
                            }
                            return c;
                        }
                    }

                }
            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return null;
        }

       
        
        public static void AddCourse( int specializationNumber, string courseName, string departmentName)
        {
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";

            int courseCounter = 0; // משתנה שסופר כמה קורסים יש עד המקום בו רוצים להוסיף קורס שזה בסוף הרשימה 
            if (File.Exists(pathAllStudyTrack))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();
                string[] splitLine;
                string newLine = ""; // כאן נרכיב את השורה החדשה עם הקורס החדש, השורה המעודכנת

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == departmentName)
                    {//                specializationNumber      מספר ההתמחות שאליו רוצים להכניס את הקורס
                        splitLine = readAllLines[specializationNumber + 1 + i].Split(',');

                        for (int k = 0; k < splitLine.Length; k++)
                            newLine += splitLine[k] + ",";
                        newLine += courseName;
                        readAllLines[specializationNumber + 1+i] = newLine;

                        for(int k = i;k< specializationNumber+1+i; k++)
                        {
                            splitLine = readAllLines[k].Split(',');
                            courseCounter += splitLine.Length;
                        }

                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }

                    courseCounter += splitLine.Length;
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

            if (File.Exists(pathAllCourses))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                int countdown = courseCounter; // נוריד כל פעם כי בקובץ הזה כל קבוצת קורסים נכנסת להתמחות אחת

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    // MessageBox.Show(readAllLines[i] + "-----8888--------->" + "countdown" + countdown);

                    if (countdown > 0)
                        countdown--;
                    else
                    {
                        readAllLines.Insert(i, courseName);
                        break;
                    }
                }

                File.WriteAllLines(pathAllCourses, readAllLines);

            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllCourses);

        }

        public static void UpdateCourseName( int specializationNumber, string newCourseName, string oldCourseName, string departmentName)
        {
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";
            string[] splitLine;

            int courseCounter = 0; // משתנה שסופר כמה קורסים יש עד המקום בו רוצים להוסיף קורס שזה בסוף הרשימה 
            if (File.Exists(pathAllStudyTrack))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == departmentName)
                    {
                        splitLine = readAllLines[specializationNumber + 1 + i].Split(',');

                        for (int k = 0; k < splitLine.Length; k++)
                        {
                            if(splitLine[k] == oldCourseName)
                                splitLine[k] = newCourseName;
                        }
                        readAllLines[specializationNumber + 1+i] = string.Join(",", splitLine);

                       

                        for (int k = i; k < specializationNumber + 1 + i; k++)
                        {
                            splitLine = readAllLines[k].Split(',');
                            courseCounter += splitLine.Length;
                        }

                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }

                    courseCounter += splitLine.Length;
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

            if (File.Exists(pathAllCourses))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                int countdown = courseCounter; // נוריד כל פעם כי בקובץ הזה כל קבוצת קורסים נכנסת להתמחות אחת

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    // MessageBox.Show(readAllLines[i] + "-----8888--------->" + "countdown" + countdown);

                    if (countdown > 0)
                        countdown--;
                    else
                    {
                        
                        splitLine = readAllLines[i].Split(",");
                        splitLine[0] = newCourseName;
                        readAllLines[i]= string.Join(",", splitLine);
                        break;
                    }
                }

                File.WriteAllLines(pathAllCourses, readAllLines);

            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllCourses);

        }

        //public static void RemoveCourseName(int specializationNumber, string newCourseName, string oldCourseName, string departmentName)
        //{
        //    string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
        //    string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";
        //    string[] splitLine;

        //    int courseCounter = 0; // משתנה שסופר כמה קורסים יש עד המקום בו רוצים להוסיף קורס שזה בסוף הרשימה 
        //    if (File.Exists(pathAllStudyTrack))
        //    {

        //        List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();

        //        for (int i = 0; i < readAllLines.Count; i++)
        //        {
        //            splitLine = readAllLines[i].Split(',');
        //            if (splitLine[0] == departmentName)
        //            {
        //                splitLine = readAllLines[specializationNumber + 1 + i].Split(',');

        //                for (int k = 0; k < splitLine.Length; k++)
        //                {
        //                    if (splitLine[k] == oldCourseName)
        //                        splitLine[k] = newCourseName;
        //                }
        //                readAllLines[specializationNumber + 1 + i] = string.Join(",", splitLine);



        //                for (int k = i; k < specializationNumber + 1 + i; k++)
        //                {
        //                    splitLine = readAllLines[k].Split(',');
        //                    courseCounter += splitLine.Length;
        //                }

        //                File.WriteAllLines(pathAllStudyTrack, readAllLines);
        //                break;
        //            }

        //            courseCounter += splitLine.Length;
        //        }
        //    }
        //    else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

        //    if (File.Exists(pathAllCourses))
        //    {

        //        List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
        //        int countdown = courseCounter; // נוריד כל פעם כי בקובץ הזה כל קבוצת קורסים נכנסת להתמחות אחת

        //        for (int i = 0; i < readAllLines.Count; i++)
        //        {
        //           // MessageBox.Show(readAllLines[i] + "-----8888--------->" + "countdown" + countdown);

        //            if (countdown > 0)
        //                countdown--;
        //            else
        //            {

        //                splitLine = readAllLines[i].Split(",");
        //                splitLine[0] = newCourseName;
        //                readAllLines[i] = string.Join(",", splitLine);
        //                break;
        //            }
        //        }

        //        File.WriteAllLines(pathAllCourses, readAllLines);

        //    }
        //    else MessageBox.Show(" יש בעיה בנתיב" + pathAllCourses);

        //}

        public static void UpdateCoreCourseName(string newCoreCourseName, string oldCoreCourseName, string departmentName)
        {
            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";
            string[] splitLine;

            int courseCounter = 0; // משתנה שסופר כמה קורסים יש עד המקום בו רוצים להוסיף קורס שזה בסוף הרשימה 
            if (File.Exists(pathAllStudyTrack))
            {
                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == departmentName)
                    {
                        for (int k = 1; k < splitLine.Length; k++)
                        {
                            if (splitLine[k] == oldCoreCourseName)
                            {
                                splitLine[k] = newCoreCourseName;
                                break;
                            }
                            courseCounter++;
                        }
                        readAllLines[i] = string.Join(",", splitLine);
                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }
                    courseCounter += splitLine.Length;
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

            if (File.Exists(pathAllCourses))
            {

                List<string> readAllLines = File.ReadAllLines(pathAllCourses).ToList();
                int countdown = courseCounter; // נוריד כל פעם כי בקובץ הזה כל קבוצת קורסים נכנסת להתמחות אחת

                for (int i = 0; i < readAllLines.Count; i++)
                {
                   // MessageBox.Show(readAllLines[i] + "-----8888--------->" + "countdown" + countdown);

                    if (countdown > 0)
                        countdown--;
                    else
                    {
                        
                        splitLine = readAllLines[i].Split(",");
                        splitLine[0] = newCoreCourseName;
                        readAllLines[i] = string.Join(",", splitLine);
                        break;
                    }
                }

                File.WriteAllLines(pathAllCourses, readAllLines);

            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllCourses);












        }


        public static void RemoveCourse (string courseName)
        {
            /* פעולה שמקבלת קורס ומסירה אותו מהמערכת
             * היא תצטרך גם לעבור על כל התלמידים שנמצאים בתוכו ולהסיר את השם קורס בתוכם ורק אז לסגור את עצמו
             */

            string pathAllCourses = @"..\..\..\..\Files\allCourses.txt";
            string pathAllStudyTrack = @"..\..\..\..\Files\allStudyTrack.txt";
            string[] splitLine;

            int courseCounter = 0; // משתנה שסופר כמה קורסים יש עד המקום בו רוצים להוסיף קורס שזה בסוף הרשימה 
            if (File.Exists(pathAllStudyTrack))
            {
                List<string> readAllLines = File.ReadAllLines(pathAllStudyTrack).ToList();

                for (int i = 0; i < readAllLines.Count; i++)
                {
                    splitLine = readAllLines[i].Split(',');
                    if (splitLine[0] == departmentName)
                    {
                        for (int k = 1; k < splitLine.Length; k++)
                        {
                            if (splitLine[k] == oldCoreCourseName)
                            {
                                splitLine[k] = newCoreCourseName;
                                break;
                            }
                            courseCounter++;
                        }
                        readAllLines[i] = string.Join(",", splitLine);
                        File.WriteAllLines(pathAllStudyTrack, readAllLines);
                        break;
                    }
                    courseCounter += splitLine.Length;
                }
            }
            else MessageBox.Show(" יש בעיה בנתיב" + pathAllStudyTrack);

        }














    }






}
