using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic.Devices;

namespace My_university_WinFormsApp.Models
{
    public class CoreProgram : StudyTrack
    {
        public CoreProgram (string name ) : base(name) { }
        public CoreProgram()  { }

        /*  היא מחלקה שמהטרה שלה לדמות את המסלול שתלמיד חייב לעבור כדי להגיע לתואר שלו CoreProgram
         *  כלומר אם יש את המסלול הנדסאי ובתוכו יש הנדסת תוכנה אז זאת אומרת שהסטודנט חייב להשלים את כל 
         *  הקורסים שקשורים למסלול הנדסאי ואז הוא יוכל להמשיך להתמחות שלו בהנדסת תוכנה
         *  
         *  באה כדי לדמות את המסלולי ליבה שהמקצוע דורש CoreProgram אז המחלקה
         *  StudyTrack והיא יורשת מהמחלקה האבסטרקטית
         */
        public static CoreProgram LoadingCourse(string coreProgramName)
        {

            CoreProgram cp = new CoreProgram(coreProgramName);  // מכין מסלול ליבה חדש
            string path = @"..\..\..\..\Files\allStudyTrack.txt";
            Course c;
            Lecturer l;
            if (File.Exists(path))
            {
                string[] splitLine;
                string line;

                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        
                        splitLine = line.Split(',');
                       
                        if (splitLine[0] == coreProgramName)
                        {
                            if (splitLine.Length < 1)
                            {
                                MessageBox.Show("המסלול" + coreProgramName + "לא מכיל קורסים כרגע");
                                return cp;
                            }

                            for (int i = 1; i < splitLine.Length; i++)
                            {
                                
                                c = Course.LoadingCourse(splitLine[i]);// מחזיק בקורס אחד מתוך הרשימה                             

                                l = Lecturer.getNameByCourse(splitLine[i]);
                                if (l== null)
                                     l = HeadOfDepartment.getNameByCourse(splitLine[i]);
                                c.Teacher =l;

                                cp.Courses.Add(c);
                                cp.Students = cp.Students.Union(c.Students).ToList(); // מעדכן את רשימת הסטודנטים ככה שיהיו בלי כפילויות                             
                               
                            }
                           

                            return cp;
                        }
                        while ((line = sr.ReadLine()) != null)
                            if (line == "*")
                                break;
                        
                    }
                    MessageBox.Show("המסלול" + coreProgramName + "לא נמצא");
                    return null;
                }


            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה!!! Files");
            return null;
        }

       
    }
}
