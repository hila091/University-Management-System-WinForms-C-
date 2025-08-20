using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_university_WinFormsApp.Models
{
    public abstract class StudyTrack
    { /*   אז ככה:
       *   יש את העיניין שלסטודנט יש את המסלול הלימודי שהוא לומד ואת ההתמחות שלו
       *   אז אם המסלול הלימודי שלו הוא נניח הנדסאי אז ההתמחות היא הנדסת תוכנה, 
       *   Secialization ו CoreProgram כך או כך אני אצטרך שבלונה כזאת כדי שיהיה לי נוח לעבוד עם המחלקות של 
       *   הרי לכך יצרנו את המחלקה הבאה:
       */

        public string Name { get; set; } // שם המסלול
        public List<Course> Courses { get; set; } // רשימת קורסים לאותו מסלול
        public List<Student> Students { get; set; } 

       
        public StudyTrack()
        {
            this.Name = "";
            this.Courses = new List<Course>();
            this.Students = new List<Student>();
        }
        public StudyTrack(string name)
        { 
            this.Name = name;
            this.Courses = new List<Course>();
            this.Students = new List<Student>();
        }


        /* בגלל שזאת מחלקה אבסטרקטית ואלו לא פעולות אבסטרקטיות אז כל מי שיורש ממנה 
         * מקבל אוטומטית את היכולת לממש את הפעולות האלה
         * אם הייתי כותבת אותן כפעולות אבסטרקטיות הן היו חייבות לממש אותן בדרך השונה והקסומה שלהן
         */


    }
}
