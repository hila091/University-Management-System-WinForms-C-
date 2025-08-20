using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_university_WinFormsApp.Models
{
    public class StudentTA : Student
    {
        public List<Course> CourseTaught { get; set; } // רשימת הקורסים שהמתרגל יכול ללמד

        public StudentTA(string accountType, string name, string fmName, string age, string phoneNumber, string gmail, string id,
        DateTime birthday, Image profileImage, int studentNumber, int currentCredits, int totalCredits)
            : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            this.CourseTaught = new List<Course>();
            StudentNumber = studentNumber;
        }

        public StudentTA(string accountType, string name, string fmName, string age, string phoneNumber, string gmail, string id,
       DateTime birthday, Image profileImage)
           : base(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage)
        {
            this.CourseTaught = new List<Course>();
        }



        public static StudentTA LoadingStudentTA(string studentNum)
        {
            string path = @"..\..\..\..\Files\Students\studentTA.txt";

            if (File.Exists(path))
            {
                string[] splitLine;
                string line;
                StudentTA user = null;
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
                            user = new StudentTA("StudentTA", name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, studentNumber, currentCredits, totalCredits);


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

        public static List<Course> LoadingCourseTaught(string line)
        {
          
            Course c; Lecturer l;
            string[] splitLine = line.Split(',');
            
            List<Course> courseTaught = new List<Course>();
            if (line == "-")
                return courseTaught;
          
            for (int i = 0; i < splitLine.Length-1; i++)
            {
                c = Course.LoadingCourse(splitLine[i]);// מחזיק בקורס אחד מתוך הרשימה                             
                l = Lecturer.getNameByCourse(splitLine[i]);
                if (l == null)
                    l = HeadOfDepartment.getNameByCourse(splitLine[i]);
                c.Teacher = l;
                courseTaught.Add(c);    
            }
            return courseTaught;


        }



    }
}
