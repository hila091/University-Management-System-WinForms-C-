using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace My_university_WinFormsApp.Models
{
    public abstract class Person
    {
        public string AccountType { get; set; } // סוג בן אדם: מתרגל סטודנט מרצה או ראש מחלקה
        public string Name { get; set; }         // שם פרטי
        public string FmName { get; set; }       // שם משפחה
        public string Age { get; set; }           // גיל
        public string PhoneNum { get; set; }      // מספר טלפון
        public string Gmail { get; set; }          // אימייל
        public string Id { get; set; }             // תעודת זהות
        public DateTime Birthday { get; set; }    // תאריך לידה
        public Image ProfileImage { get; set; }   // תמונת פרופיל
        public List<UserMessage> Messages { get; set; }  // רשימת הודעות
        public string UserName { get; set; }      // <----- תז המשתמש
        public string Password { get; set; }      // <----- השם של המייל שלו עד לשטרודל
        public DateTime LastLoginDate { get; set; }  
        public Person(string accountType, string name, string fmName, string age, string phoneNumber,
            string gmail, string id, DateTime birthday, Image profileImage)
        {
            // פעולה בונה עבור אדם חדש
            this.AccountType = accountType;
            this.Name = name;
            this.FmName = fmName;
            this.Age = age;
            this.PhoneNum = phoneNumber;
            this.Gmail = gmail;
            this.Id = id;
            this.Birthday = birthday;
            this.ProfileImage = profileImage;
            this.UserName = gmail.Substring(0, gmail.IndexOf('@')).Trim(); // <----- השם של המייל שלו עד לשטרודל
            this.Password = id;       // <----- תז המשתמש
            this.Messages = new List<UserMessage>();
            this.LastLoginDate = DateTime.Now;
        }


    }



}

