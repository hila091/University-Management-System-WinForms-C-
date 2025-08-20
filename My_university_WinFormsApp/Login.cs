using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using System.Xml.Linq;
using My_university_WinFormsApp.Models;
using System.Globalization;

namespace My_university_WinFormsApp
{
    partial class Login : Form
    {
        private Form startWindow; // כאן נשמר החלון שממנו הגענו החלון התחלה כדי שיהיה נוח להציג ולהסתיר אותו
        private string LoginType; // מכיל את סוג הכניסה

        //---------------------------------------- משתנים עבור יצירת העמוד 
        private Panel backgroundPanel;
        private TextBox userNameBox;
        private TextBox passwordBox;
        private Label password;
        private Label userName;
        private Label title;
        private Label goBack;
        private Button loginButton;
        private Label line;
        // --------------------------------------- 

        public Login(Form startWindow, string loginType)
        {
            this.FormClosing += CloseAll;
            InitializeComponent();
            createLoginPage();
            this.startWindow = startWindow;
            this.LoginType = loginType;
        }

        private void createLoginPage()
        {
            backgroundPanel = new Panel();
            line = new Label();
            loginButton = new Button();
            goBack = new Label();
            userNameBox = new TextBox();
            passwordBox = new TextBox();
            password = new Label();
            userName = new Label();
            title = new Label();
            backgroundPanel.SuspendLayout();
            SuspendLayout();
            // 
            // backgroundPanel
            // 
            backgroundPanel.BackColor = Color.PaleGoldenrod;
            backgroundPanel.Controls.Add(line);
            backgroundPanel.Controls.Add(loginButton);
            backgroundPanel.Controls.Add(goBack);
            backgroundPanel.Controls.Add(userNameBox);
            backgroundPanel.Controls.Add(passwordBox);
            backgroundPanel.Controls.Add(password);
            backgroundPanel.Controls.Add(userName);
            backgroundPanel.Controls.Add(title);
            backgroundPanel.Location = new Point(0, 0);
            backgroundPanel.Name = "backgroundPanel";
            backgroundPanel.Size = new Size(705, 211);
            backgroundPanel.TabIndex = 0;
            backgroundPanel.ResumeLayout(false);
            backgroundPanel.PerformLayout();
            // 
            // line
            // 
            line.AutoSize = true;
            line.ForeColor = Color.SaddleBrown;
            line.Location = new Point(39, 58);
            line.Name = "line";
            line.Size = new Size(633, 20);
            line.TabIndex = 2;
            line.Text = "--------------------------------------------------------------------------------------------------------";
            // 
            // loginButton
            // 
            loginButton.Font = new Font("Arial", 12F, FontStyle.Bold);
            loginButton.ForeColor = Color.SaddleBrown;
            loginButton.Location = new Point(321, 168);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(111, 29);
            loginButton.TabIndex = 22;
            loginButton.Text = "התחברות";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += LoginButton_Click;
            // 
            // goBack
            // 
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 25F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(39, 9);
            goBack.Name = "goBack";
            goBack.Size = new Size(52, 49);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Click += ReturnToStartWindow;
            // 
            // userNameBox
            // 
            userNameBox.Location = new Point(63, 127);
            userNameBox.Name = "userNameBox";
            userNameBox.Size = new Size(195, 27);
            userNameBox.TabIndex = 0;
            userNameBox.MaxLength = 250;
            userNameBox.TextChanged += TextBoxCheck;
            // 
            // passwordBox
            // 
            passwordBox.Font = new Font("Arial", 12F, FontStyle.Bold);
            passwordBox.Location = new Point(470, 127);
            passwordBox.Name = "passwordBox";
            passwordBox.Size = new Size(195, 30);
            passwordBox.TabIndex = 1;
            passwordBox.UseSystemPasswordChar = true;
            passwordBox.MaxLength = 9;
            passwordBox.TextChanged += TextBoxCheck;
            // 
            // password
            // 
            password.AutoSize = true;
            password.Font = new Font("Arial", 12F, FontStyle.Bold);
            password.ForeColor = Color.SaddleBrown;
            password.Location = new Point(539, 81);
            password.Name = "password";
            password.Size = new Size(71, 24);
            password.TabIndex = 2;
            password.Text = ":סיסמה";
            // 
            // userName
            // 
            userName.AutoSize = true;
            userName.Font = new Font("Arial", 12F, FontStyle.Bold);
            userName.ForeColor = Color.SaddleBrown;
            userName.Location = new Point(107, 81);
            userName.Name = "userName";
            userName.Size = new Size(119, 24);
            userName.TabIndex = 1;
            userName.Text = ":שם משתמש";
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Arial", 12F, FontStyle.Bold);
            title.ForeColor = Color.SaddleBrown;
            title.Location = new Point(266, 29);
            title.Name = "title";
            title.Size = new Size(0, 24);
            title.TabIndex = 0;

            Controls.Add(backgroundPanel);

            
        }
        private void CloseAll(object sender, FormClosingEventArgs e){  Application.Exit(); }
        private void ReturnToStartWindow(object sender, EventArgs e)
        {
            this.startWindow.Show();
            this.Hide();
            this.Dispose();
        }

       
        private void Login_Load(object sender, EventArgs e)
        {
            switch (this.LoginType)
            {
                case "Student":
                    title.Text = "כניסה בתור: סטודנט";
                    break;
                case "StudentTA":
                    title.Text = "כניסה בתור: סטודנט מתרגל";
                    break;
                case "HeadOfDepartment":
                    title.Text = "כניסה בתור: ראש מחלקה";
                    break;
                case "Lecturer":
                    title.Text = "כניסה בתור: מרצה";
                    break;
            }
        }
        private void TextBoxCheck(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(passwordBox.Text) && !string.IsNullOrWhiteSpace(userNameBox.Text))
                // אם שדה הסיסמה ושם המשתמש לא ריקים אז אני ארצה שהכפתור יהיה זמין 
                loginButton.Enabled = true;
            else
                loginButton.Enabled = false;
        }

        public static Person CreateUser (string path, string username, string password, string accountType)
        {
            string name = "", fmName = "", age = "", phoneNumber = "", gmail = "", id = ""; int employeeNumber = 0;
            int studentNumber = 0, currentCredits = 0, totalCredits = 0; DateTime birthday = DateTime.MinValue, lastLoginDate = DateTime.MinValue;
            Image profileImage = null;
            /* פעולה שקורת אחרי שהחשבון נמצא ואני רוצה בעצם לייצא את המידע הנחוץ מהקובץ לאובייקטים שיהיה לי נוח לעבוד איתם 
             * במילים אחרות טעינה מקבצים
             * הקטע שבגלל שזה אוניברסיטה וזאת מערכת גדולה אין פואנטה שאני אטען את כל המידע מכל הקבצים
             * לכן:
             * 
             * אם זה סטודנט הוא יטען את מה שרלוונטי כמו הקורסים שלו המרצים שלו וראש המחלקה שלו 
             * אם זה סטודנט מתרגל הוא יטען את הקורסים שהוא מלמד הקורסים שהוא לומד המרצים וכן הלאה וכן הלאה
             * אם זה ראש מחלקה הוא יטען את כל מה שקשור למחלקה שלו
             * ויורשה גם לכתוב הודעות בעזרת השם לשאר ראשי המחלקות תהיה לו כזאת רשימה מגניבה
             */

            /*             תזכיר
             * פורמטי שמירה לפי סוגי חשבונות
             * קובץ סטודנטים - כל 4 שורות 
             * קובץ סטודנטים מתרגלים -כל 5 שורות
             * קובץ מרצים - כל 3 שורות
             * קובץ ראשי מחלקה -כל 2 שורות
             */

            if (File.Exists(path))
            {
                string[] splitLine;
                 using (StreamReader sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        splitLine = line.Split(',');
                        if (splitLine[0] == username && splitLine[1] == password)
                        {
                            switch (accountType) 
                            {
                                case "Student":
                                    name = splitLine[3];
                                    fmName = splitLine[4];
                                    age = splitLine[5];
                                    phoneNumber = splitLine[6];
                                    gmail = splitLine[7];
                                    id = splitLine[8];
                                    birthday = DateTime.ParseExact(splitLine[9], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    studentNumber = int.Parse(splitLine[2]);
                                    profileImage = SearchingImgByID(@"..\..\..\..\Files\Students\ProfilePictures", studentNumber);
                                    totalCredits = int.Parse(splitLine[10]);
                                    currentCredits = int.Parse(splitLine[11]);
                                    Student userS = new Student(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, studentNumber, currentCredits, totalCredits);
                                    userS.LastLoginDate = DateTime.ParseExact( splitLine[12],"dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture);
                                    
                                    userS.CoreCourse = CoreProgram.LoadingCourse(splitLine[13]);                                    
                                    userS.SpecializationCourses = Specialization.LoadingCourse(splitLine[14]);
                                    line = sr.ReadLine();
                                    userS.Messages= UserMessage.LodingUserMessages(line);

                                    return userS;


                                case "StudentTA":
                                    name = splitLine[3];
                                    fmName = splitLine[4];
                                    age = splitLine[5];
                                    phoneNumber = splitLine[6];
                                    gmail = splitLine[7];
                                    id = splitLine[8];
                                    birthday = DateTime.ParseExact(splitLine[9], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    studentNumber = int.Parse(splitLine[2]);
                                    profileImage = SearchingImgByID(@"..\..\..\..\Files\Students\ProfilePictures", studentNumber);
                                    currentCredits = int.Parse(splitLine[10]);
                                    totalCredits = int.Parse(splitLine[11]);
                                    StudentTA userSTA = new StudentTA(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, studentNumber, currentCredits, totalCredits);

                                    userSTA.LastLoginDate = DateTime.ParseExact(splitLine[12], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    
                                    userSTA.CoreCourse = CoreProgram.LoadingCourse(splitLine[13]);
                                    userSTA.SpecializationCourses = Specialization.LoadingCourse(splitLine[14]);

                                    line = sr.ReadLine();
                                    userSTA.CourseTaught = StudentTA.LoadingCourseTaught(line);
                                    line = sr.ReadLine();
                                    userSTA.Messages = UserMessage.LodingUserMessages(line);


                                    return userSTA;

                                case "Lecturer":
                                    name = splitLine[3];
                                    fmName = splitLine[4];
                                    age = splitLine[5];
                                    phoneNumber = splitLine[6];
                                    gmail = splitLine[7];
                                    id = splitLine[8];
                                    birthday = DateTime.ParseExact(splitLine[9], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    employeeNumber = int.Parse(splitLine[2]);
                                    profileImage = Login.SearchingImgByID(@"..\..\..\..\Files\Employees\ProfilePictures", employeeNumber);
                                    Lecturer userL = new Lecturer(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, splitLine[11], employeeNumber);
                                    
                                    userL.Rating = int.Parse(splitLine[12]);
                                    userL.LastLoginDate = DateTime.Parse(splitLine[10]);

                                    // קריאת הקורסים שהוא מלמד 
                                    
                                    line = sr.ReadLine();
                                    userL.Courses = StudentTA.LoadingCourseTaught(line);


                                    line = sr.ReadLine();
                                    userL.Messages = UserMessage.LodingUserMessages(line);

                                    return userL;   

                                case "HeadOfDepartment":
                                    
                                    name = splitLine[3];
                                    fmName = splitLine[4];
                                    age = splitLine[5];
                                    phoneNumber = splitLine[6];
                                    gmail = splitLine[7];
                                    id = splitLine[8];
                                    birthday = DateTime.Parse(splitLine[9]);
                                    employeeNumber = int.Parse(splitLine[2]);
                                    profileImage = Login.SearchingImgByID(@"..\..\..\..\Files\Employees\ProfilePictures", employeeNumber);
                                    string classSubject = splitLine[11];
                                    HeadOfDepartment userHD = new HeadOfDepartment(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, classSubject, employeeNumber);
                                    userHD.LastLoginDate = DateTime.Parse(splitLine[10]);

                                    userHD.CoreCourse = CoreProgram.LoadingCourse(classSubject); // טוען את הקורסים הבסיסיים של ראש המחלקה שחובה לעשות
                                    
                                    userHD.SpecializationCourses = Specialization.AllSpecializationList(classSubject);

                                    line = sr.ReadLine();
                                    userHD.Courses = StudentTA.LoadingCourseTaught(line);

                                    line = sr.ReadLine();
                                    userHD.Messages = UserMessage.LodingUserMessages(line);

                                    return userHD; 

                            }

                        }
                        else
                        {
                            while (line != "*")
                                line = sr.ReadLine();
                        }

                    }
                }
                return null;
            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return null;
        }
        

       

        public static Image SearchingImgByID(string imgPath, int serialNumber)
        {
            // פעולה שמקבלת מספר סידורי ונתיב ומחזירה את התמונה שהיא מוצאת של אותו אובייקט מרצה תלמיד ראש מחלקה וכו וכו
            imgPath += "/" + serialNumber + ".png";

            if (File.Exists(imgPath))
            {
                return Image.FromFile(imgPath);
            }
            else
            {
                MessageBox.Show("התמונה לא נמצאה לא ניתן לטעון את האובייקט בצורה מלאה" + serialNumber+"---"+ imgPath);
                return null;
            }
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {// אם הכפתור נלחץ משמע יש שם משתמש וסיסמה בתיבות וצריך לפי סוג הכניסה לחפש אם יש חשבון או לא
            string password = passwordBox.Text;
            string userName = userNameBox.Text;
            string path = @"";

            switch (this.LoginType)
            {
                case "Student":
                    path = @"..\..\..\..\Files\Students\students.txt";
                    if(CreateAccount.FindAccount(path, userName, password))
                    {
                        Student user = (Student)Login.CreateUser(path, userName, password, "Student");// המרה לסטודנט כי זה מחזיר בן אדם
                        if (user != null)
                        {
                            MessageBox.Show("ההתחברות בוצעה בהצלחה!" + Environment.NewLine+"שעת כניסה אחרונה : "+ user.LastLoginDate);
                            StudentWindow studentWindow = new StudentWindow(user);
                            studentWindow.Show();
                            this.Hide();
                        }
                    }
                    else
                        MessageBox.Show(":החשבון בעל" + Environment.NewLine+" שם המשתמש "+ userName+ Environment.NewLine+ " והסיסמה " + password+ Environment.NewLine + " לא נמצאים במערכת שלנו");
                    break;
                case "StudentTA":
                    path = @"..\..\..\..\Files\Students\studentTA.txt";
                    if (CreateAccount.FindAccount(path, userName, password))
                    {// לעשות כזה של תאריך כניסה אחרון
                        StudentTA user = (StudentTA)Login.CreateUser(path, userName, password, "StudentTA");// המרה לסטודנט כי זה מחזיר בן אדם
                        if (user != null)
                        {
                            MessageBox.Show("ההתחברות בוצעה בהצלחה!" + Environment.NewLine + "שעת כניסה אחרונה : " + user.LastLoginDate);
                            StudentTAWindow StudentTAWindow = new StudentTAWindow(user);
                            StudentTAWindow.Show();
                            this.Hide();
                        }
                    }
                    else
                        MessageBox.Show(":החשבון בעל" + Environment.NewLine + " שם המשתמש " + userName + Environment.NewLine + " והסיסמה " + password + Environment.NewLine + " לא נמצאים במערכת שלנו");
                    break;
                case "HeadOfDepartment":
                    path = @"..\..\..\..\Files\Employees\headOfDepartment.txt";
                    if (CreateAccount.FindAccount(path, userName, password))
                    {
                        HeadOfDepartment user = (HeadOfDepartment)Login.CreateUser(path, userName, password, "HeadOfDepartment");// המרה לסטודנט כי זה מחזיר בן אדם
                        if (user != null)
                        {
                            MessageBox.Show("ההתחברות בוצעה בהצלחה!" + Environment.NewLine + "שעת כניסה אחרונה : " + user.LastLoginDate);
                            HeadOfDepartmentWindow headOfDepartment = new HeadOfDepartmentWindow(user);
                            headOfDepartment.Show();
                            this.Hide();
                        }
                    }
                    else
                        MessageBox.Show(":החשבון בעל" + Environment.NewLine + " שם המשתמש " + userName + Environment.NewLine + " והסיסמה " + password + Environment.NewLine + " לא נמצאים במערכת שלנו");
                    break;
                case "Lecturer":
                    path = @"..\..\..\..\Files\Employees\lecturer.txt";
                    if (CreateAccount.FindAccount(path, userName, password))
                    {
                        Lecturer user = (Lecturer)Login.CreateUser(path, userName, password, "Lecturer");// המרה לסטודנט כי זה מחזיר בן אדם
                        if (user != null)
                        {
                            MessageBox.Show("ההתחברות בוצעה בהצלחה!" + Environment.NewLine + "שעת כניסה אחרונה : " + user.LastLoginDate);
                            LecturerWindow lecturerWindow = new LecturerWindow(user);
                            lecturerWindow.Show();
                            this.Hide();
                        }
                    }
                    else
                        MessageBox.Show(":החשבון בעל" + Environment.NewLine + " שם המשתמש " + userName + Environment.NewLine + " והסיסמה " + password + Environment.NewLine + " לא נמצאים במערכת שלנו");
                    break;
               
            }
        }
    }
}
