using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using My_university_WinFormsApp.Models;
namespace My_university_WinFormsApp
{
    // תזכיר לעצמי -->    MessageBox.Show("הודעה קופצת");
    public partial class CreateAccount : Form
    {
        /* אני רוצה להגדיר ערך מילוני כתכונה עבור כל השדות של הקלט שיש במסמך הרשמה ואז זה פשוט יהיה מסודר יותר כשארצה 
         * לדעת אם הם תקינים או לא בשלב מאוחר יותר בפעולות אחרות 
         */
        // יותר תחזוקתי ונוח
        private Dictionary<string, bool> fieldStatus = new Dictionary<string, bool>();

        private Dictionary<string, string> errorMessage = new Dictionary<string, string>();

        private Form startWindow; // כאן נשמר החלון שממנו הגענו החלון התחלה כדי שיהיה נוח להציג ולהסתיר אותו
        //---------------------------------------- משתנים עבור יצירת העמוד 
        private Label login;
        private Label line;
        private Label loginASlable;
        private TextBox fmNameBox;
        private TextBox nameBox;
        private TextBox gmailBox;
        private TextBox phoneNumBox;
        private TextBox idBox;
        private DateTimePicker date;
        private OpenFileDialog openFileDialog;
        private PictureBox profilePicture;
        private Panel backgroundPanel;
        private Button apply;
        private Button sendBox;
        private ComboBox selectionType;
        private Label error;
        private ComboBox academicSubject;
        private Label Subjectselection;
        private ComboBox specializationSubject;
        private ComboBox classSubject;
        private Label goBack;
        // --------------------------------------- 

        public CreateAccount(Form startWindow)
        {
            this.FormClosing += CloseAll;
            InitializeComponent();
            createAccountPage();
            this.startWindow = startWindow;
            //--------------------------------- > מילון שמאחסן לי את המצב הנוכחי של כל שדה אם הוא תקין או לא
            fieldStatus["idBox"] = false;
            fieldStatus["phoneNumBox"] = false;
            fieldStatus["nameBox"] = false;
            fieldStatus["fmNameBox"] = false;
            fieldStatus["gmailBox"] = false;
            fieldStatus["selectionType"] = false;
            //--------------------------------- > מילון שמאחסן לי את ההודעות שהמערכת מחזירה אם המצב הנוכחי של השדה לא תקין
            errorMessage["idBox"] = "תעודת זהות לא תקינה – חייבת להיות 9 ספרות";
            errorMessage["phoneNumBox"] = "מספר טלפון לא תקין – חייב להיות 10 ספרות";
            errorMessage["nameBox"] = "שם לא תקין-  לא יכול להכיל תווים או מספרים";
            errorMessage["fmNameBox"] = "שם משפחה לא תקין -  לא יכול להכיל תווים או מספרים";
            errorMessage["gmailBox"] = "example123@email.com - אימייל חייב לכלול תבנית כזו";
            errorMessage["selectionType"] = "עליך לבחור ערך מהרשימה";
        }
        private void CloseAll(object sender, FormClosingEventArgs e) { Application.Exit(); }

        //------------------------- מה שקשור לחלון מילוי הטופס -----------------------------
        
        
        public void RemoveError(string errorMessage, string textBoxName)
        {
            // פעולה שמקבלת את השם של התיבה ואת ההודעה שהיא אמורה להציג ומוחקת את השגיאה ומקטינה את החלון למצב המקורי שלו בהתאמה
            error.Text = error.Text.Replace(errorMessage + Environment.NewLine, "");
            /* כאן אני מחליפה את הודעה של השגיאה והאנטר בכלום כלומר יש חיפוש של ההודעה הספציפית של השגיאה
             * כי היה יכול להיות לי כמה שגיאות ואני רוצה את השגיאה המדוברת הספציפית שהיא תימחק
             */
            backgroundPanel.Height -= 20; // מקטינה בחזרה את הפאנל ב20
            ClientSize = new Size(702, ClientSize.Height - 20); // מקטינה בחזרה את החלון ב20
        }

        public void AddError(string errorMessage, string textBoxName)
        {
            // פעולה שמקבלת את השם של התיבה ואת ההודעת שגיאה שהיא אמורה להציג ומוסיפה את ההודעת שגיאה, היא גם תגדיל את החלון בהתאמה
            fieldStatus[textBoxName] = false; // בגלל שיש שגיאה המצב הנוכחי של אותו שדה יהיה שלילי 
            backgroundPanel.Height += 20; // מרחיבים את הפאנל למטה ב20 
            ClientSize = new Size(702, ClientSize.Height + 20); // מרחיבים את החלון למטה ב20 
            error.Text += errorMessage + Environment.NewLine; // מוסיפים בתווית את השגיאה ואנטר 
            /*                חשוב!!
             *   Environment.NewLine  --> /n פירושו להוסיף את האנטר ואני לא משתמשת ב
             *   כי בכל מערכת הפעלה זה שונה אז זה יותר יעיל אם אני ארצה שהוא יעבוד על מחשב אחר   
             */
        }

        private void TextBoxCheck(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && !string.IsNullOrWhiteSpace(textBox.Text))
            { // כל השאר שיש כאן
                /* string.IsNullOrWhiteSpace(textBox.Text) <---- פונקציה במחלקת סטרינג שמקבלת מחרוזת ומחזירה משתנה בוליאני אם הוא ריק או לא
                 * כי אין פואנטה לבדוק שדה שנשאר ריק
                 */
                // ---------------------------------------------------------------------------------------------------------------------------------------------- "phoneBox"

                if (textBox.Name == "phoneNumBox")
                {
                    if (!Regex.IsMatch(textBox.Text, @"^\d{10}$")) // בדיקה האם השדה שהמשתמש הזין תקין או לא
                    { // אם הוא לא תקין
                        if (fieldStatus[textBox.Name] || !error.Text.Contains(errorMessage[textBox.Name]))
                            // אם הסטטוס של התיבה תקין או אם אין הודעת שגיאה כרגע אז צריך להוסיף שגיאה
                            AddError(errorMessage[textBox.Name], textBox.Name);
                    }
                    else
                    { // אם הוא תקין
                        if (error.Text.Contains(errorMessage[textBox.Name])) // בודק אם יש הודעת שגיאה ממיקודם אם יש הוא יסיר אותה
                            RemoveError(errorMessage[textBox.Name], textBox.Name);

                        fieldStatus[textBox.Name] = true; // ובכל מקרה אם הערך כרגע הוא תקין אז הסטטוס יתחלף לתקין
                    }
                }
                // ---------------------------------------------------------------------------------------------------------------------------------------------- "idBox"
                if (textBox.Name == "idBox")
                {
                    if (!Regex.IsMatch(textBox.Text, @"^\d{9}$"))
                    {
                        if (fieldStatus[textBox.Name] || !error.Text.Contains(errorMessage[textBox.Name]))
                            AddError(errorMessage[textBox.Name], textBox.Name);
                    }
                    else
                    {
                        if (error.Text.Contains(errorMessage[textBox.Name]))
                            RemoveError(errorMessage[textBox.Name], textBox.Name);

                        fieldStatus[textBox.Name] = true;
                    }
                }
                // ---------------------------------------------------------------------------------------------------------------------------------------------- "nameBox"
                if (textBox.Name == "nameBox")
                {
                    if (!Regex.IsMatch(textBox.Text, @"^[a-zA-Zא-ת']+$"))
                    {
                        if (fieldStatus[textBox.Name] || !error.Text.Contains(errorMessage[textBox.Name]))
                            AddError(errorMessage[textBox.Name], textBox.Name);
                    }
                    else
                    {
                        if (error.Text.Contains(errorMessage[textBox.Name]))
                            RemoveError(errorMessage[textBox.Name], textBox.Name);

                        fieldStatus[textBox.Name] = true;
                    }
                }
                // ---------------------------------------------------------------------------------------------------------------------------------------------- "fmNameBox"
                if (textBox.Name == "fmNameBox")
                {
                    if (!Regex.IsMatch(textBox.Text, @"^[a-zA-Zא-ת']+$"))
                    {
                        if (fieldStatus[textBox.Name] || !error.Text.Contains(errorMessage[textBox.Name]))
                            AddError(errorMessage[textBox.Name], textBox.Name);
                    }
                    else
                    {
                        if (error.Text.Contains(errorMessage[textBox.Name]))
                            RemoveError(errorMessage[textBox.Name], textBox.Name);

                        fieldStatus[textBox.Name] = true;
                    }
                }

                // ---------------------------------------------------------------------------------------------------------------------------------------------- "gmailBox"
                if (textBox.Name == "gmailBox")
                {
                    if (!Regex.IsMatch(textBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        if (fieldStatus[textBox.Name] || !error.Text.Contains(errorMessage[textBox.Name]))
                            AddError(errorMessage[textBox.Name], textBox.Name);
                    }
                    else
                    {
                        if (error.Text.Contains(errorMessage[textBox.Name]))
                            RemoveError(errorMessage[textBox.Name], textBox.Name);

                        fieldStatus[textBox.Name] = true;
                    }
                }
                // ---------------------------------------------------------------------------------------------------------------------------------------------- "selectionType"
            }
            else
            if (sender is System.Windows.Forms.ComboBox comboBox && comboBox.Name == "selectionType")
            {
                if (selectionType.SelectedIndex == -1)// אם אין בחירה
                    AddError(errorMessage[comboBox.Name], comboBox.Name);

                else
                {
                    if (error.Text.Contains(errorMessage[comboBox.Name]))
                        RemoveError(errorMessage[comboBox.Name], comboBox.Name);

                    fieldStatus[comboBox.Name] = true;
                }
            }
            // בדיקה האם כל השדות תקנים אם כן אני הופכת את הכפתור שליחה ללחיץ אחרת אני משאירה אותו בברירת מחדל שלו
            if (fieldStatus["gmailBox"] && fieldStatus["fmNameBox"] && fieldStatus["nameBox"] && fieldStatus["idBox"] && fieldStatus["phoneNumBox"] && fieldStatus["selectionType"] &&
                (specializationSubject.SelectedIndex != -1 || academicSubject.SelectedIndex != -1 || classSubject.SelectedIndex != -1))
                sendBox.Enabled = true;
            else
                sendBox.Enabled = false;


        }

        private void ShowOtherSelections(object sender, EventArgs e)
        {
            if (selectionType.SelectedIndex != -1)
            {
                switch (selectionType.SelectedIndex)
                {
                    case 0:
                    case 1: //  נבחר סטודנט רגיל או סטודנט מתרגל
                        classSubject.Hide();
                        specializationSubject.Hide();
                        academicSubject.Show();
                        Subjectselection.Text = "- אני רוצה ללמוד";
                        Subjectselection.Location = new Point(528, 342);
                        // איפוס שני הקופסאות האחרות
                        classSubject.SelectedIndex = -1;
                        specializationSubject.SelectedIndex = -1; break;
                    case 2: // נבחר מרצה
                        academicSubject.Hide();
                        classSubject.Hide();
                        specializationSubject.Show();
                        Subjectselection.Text = "- תחום התמחות";
                        Subjectselection.Location = new Point(535, 342);
                        // איפוס שני הקופסאות האחרות
                        academicSubject.SelectedIndex = -1;
                        classSubject.SelectedIndex = -1;
                        break;
                    case 3:// נבחר ראש מחלקה 
                        specializationSubject.Hide();
                        academicSubject.Hide();
                        classSubject.Show();
                        Subjectselection.Text = "- מחלקת";
                        Subjectselection.Location = new Point(581, 342);
                        // איפוס שני הקופסאות האחרות
                        specializationSubject.SelectedIndex = -1;
                        academicSubject.SelectedIndex = -1;
                        break;
                }





            }
        }

        //------------------- מה שקשור לשליחת הפרטים והבקשה ליצירת חשבון -------------------



        public static bool FindAccount(string path, string userName, string password)
        {
        /* מטרת הפעולה: לחפש את המשתמש על סמך השם משתמש וסיסמה שלו לפי סוג המשתמש - כל משתמש נשמר בקובץ טקסט אחר
         * 
         *                                          פורמטי שמירה לפי סוגי חשבונות
         *                                                קובץ סטודנטים - כל 4 שורות
         *                                        קובץ סטודנטים מתרגלים - כל 5 שורות 
         *                                                   קובץ מרצים - כל 3 שורות
         *                                              קובץ ראשי מחלקה - כל 2 שורות                                     
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

                        if (splitLine[0] == userName && splitLine[1] == password)
                        {
                            return true;
                        }
                        else
                        {
                            while (line != "*")
                                line = sr.ReadLine();
                        }       

                    }
                }
                MessageBox.Show("החשבון הזה לא קיים אנחנו יוצרים אותו בשבילך");
                return false;
            }
            MessageBox.Show("הקובץ לחיפוש לא נמצא במערכת יש לבדוק את התקייה Files");
            return false;
        }


        private void Send_Click(object sender, EventArgs e)
        {
            /* שם משתמש וסיסמה לא יווצרו על ידי המערכת,
             * כשמשתמש יחליט ליצור חשבון חדש אם אין את החשבון הזה כבר הוא יצור אותו ובכניסה תופיע חלונית שתציג לו את השם משתמש והסיסמה שנוצרו לו
             * ---------------------------------------------------
             *                      סיסמה  --> תהיה התז של המשתמש     
             *  שם המשתמש --> יהיה ההתחלה של המייל שלו עד לשטרודל 
             * ---------------------------------------------------
             *  אני לא רוצה ליצור אובייקט מהנתונים שיש לי לפני שאני בודקת אם המשתמש הזה באמת לא קיים במערכת אז:
             */

            string password = idBox.Text.Trim();
            string userName = gmailBox.Text.Substring(0, gmailBox.Text.IndexOf('@')).Trim(); // "example123" הוא יחזיר "example123@gmail.com" עבור

            string path = @""; // משתנה שמאחסן בתוכו את המסלול הנכון לקובץ בו נרצה לבדוק או לנווט כדי ליצור את המשתמש
            string accountType = "";
            switch (selectionType.Text)
            {
                case "סטודנט רגיל":
                    path = @"..\..\..\..\Files\Students\students.txt";
                    accountType = "Student";
                    break;
                case "סטודנט מתרגל":
                    path = @"..\..\..\..\Files\Students\studentTA.txt";
                    accountType = "StudentTA";
                    break;
                case "מרצה":
                    path = @"..\..\..\..\Files\Employees\lecturer.txt";
                    accountType = "Lecturer";
                    break;
                case "ראש מחלקה":
                    path = @"..\..\..\..\Files\Employees\headOfDepartment.txt";
                    accountType = "HeadOfDepartment";
                    break;
            }


            // בדיקה במסלול הנבחר אם המשתמש קיים 
            if (!FindAccount(path, userName, password))
            // החשבון לא קיים לכן ניצור אותו ונצרוב את הנתונים שלו בקובץ של הגשת בקשה לחשבון שפתוח רק לראשי מחלקה
            {
                string name = nameBox.Text.Trim();
                string fmName = fmNameBox.Text.Trim();
                string age = (DateTime.Today.Year - date.Value.Year).ToString(); // מציאת הגיל על סמך התאריך
                string phoneNumber = phoneNumBox.Text.Trim();
                string gmail = gmailBox.Text.Trim();
                string id = idBox.Text.Trim();
                DateTime birthday = date.Value.Date; // DateTime כי הוא מחזיר כבר את האובייקט בתור date.Text ולא date.Value
                Image profileImage = profilePicture.Image;

                MessageBox.Show("הודעה קופצת" + "/" + userName + "," + password + "/" + birthday.ToString("dd/MM/yyyy"));

                switch (selectionType.Text)
                {
                    /*  הרשאת יצירת אובייקט כלשהו ניתנת רק לראש המחלקה אז הפעולה באה ממנו כמו פעולה רותית כזאת 
                     *  והוספה של האובייקטים לרשימת הבקשות ולקובץ הבקשות
                     */

                    case "סטודנט רגיל":
                        Student newS = HeadOfDepartment.CreateStudent(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage);
                        HeadOfDepartment.AddRequestToFile(newS, accountType);
                        HeadOfDepartment.RegistrationRequest.Add(newS);
                        break;
                    case "סטודנט מתרגל":
                        StudentTA newTAS = HeadOfDepartment.CreateStudentTA(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage);
                        HeadOfDepartment.RegistrationRequest.Add(newTAS);
                        HeadOfDepartment.AddRequestToFile(newTAS, accountType);
                        break;
                    case "מרצה":
                        Lecturer newL = HeadOfDepartment.CreateLecturer(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, specializationSubject.Text);
                        HeadOfDepartment.AddRequestToFile(newL, accountType);
                        HeadOfDepartment.RegistrationRequest.Add(newL);
                        break;
                    case "ראש מחלקה":
                        HeadOfDepartment newHD = new HeadOfDepartment(accountType, name, fmName, age, phoneNumber, gmail, id, birthday, profileImage, classSubject.Text);
                        HeadOfDepartment.AddRequestToFile(newHD, accountType);
                        HeadOfDepartment.RegistrationRequest.Add(newHD);
                        break;
                }

            }
            else MessageBox.Show("החשבון הזה כבר קיים");


        }

        private void Apply_Click(object sender, EventArgs e)
        {
            /*  אני רוצה שכשלוחצים על הכפתור בחירה יפתח דיאלוג כזה עם הקבצים של המחשב 
             *  OpenFileDialog (דרך הבחירה בקופסאת כלים את הכלי )
             *  ואז אני מגדירה שאפשר לבחור רק קבצים שמשוייכים לתמונות 
             */
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "בחר תמונת פרופיל";
            openFileDialog.Filter = "קבצי תמונה|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)//  המערכת מחכה עד שהמשתמש יבחר תמונה
            {// אם הוא בחר תמונה יקרו הדברים הבאים
                string filePath = openFileDialog.FileName;
                profilePicture.Image = Image.FromFile(filePath);
                profilePicture.SizeMode = PictureBoxSizeMode.Zoom; // שתתאים לגודל התיבה
                apply.Text = "בחרתי!";
                apply.BackColor = Color.SpringGreen;
            }
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            
            if (File.Exists(@"..\..\..\..\imgs\anonymous.png"))
            {
                profilePicture.Image = Image.FromFile(@"..\..\..\..\imgs\anonymous.png");
                profilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("לא נמצאה התמונה בנתיב: " + @"..\..\..\..\imgs\anonymous.png");
            }
        }

        private void createAccountPage()
        {
            // 
            // goBack
            // 
            goBack = new Label();
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
            // classSubject
            // 
            classSubject = new ComboBox();
            classSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            classSubject.FormattingEnabled = true;
            classSubject.Items.AddRange(new object[] { "הנדסה", "מדעי המחשב וטכנולוגיה", "עסקים וכלכלה", "מדעי החיים ובריאות", "מדעים מדויקים וטכנולוגיים", "משפטים ומדעי החברה", "אמנויות ועיצוב", "מדעי הרוח" });
            classSubject.Location = new Point(356, 338);
            classSubject.Name = "classSubject";
            classSubject.Size = new Size(189, 28);
            classSubject.TabIndex = 19;
            classSubject.SelectedIndexChanged += TextBoxCheck;
            // 
            // error
            // 
            error = new Label();
            error.AutoSize = true;
            error.ForeColor = Color.Red;
            error.Location = new Point(236, 429);
            error.Name = "error";
            error.Size = new Size(0, 20);
            error.TabIndex = 16;
            error.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sendBox
            // 
            sendBox = new Button();
            sendBox.Enabled = false;
            sendBox.Location = new Point(300, 395);
            sendBox.Name = "sendBox";
            sendBox.Size = new Size(123, 29);
            sendBox.TabIndex = 15;
            sendBox.Text = "צור משתמש";
            sendBox.UseVisualStyleBackColor = true;
            sendBox.Click += Send_Click;
            // 
            // Subjectselection
            // 
            Subjectselection = new Label();
            Subjectselection.AutoSize = true;
            Subjectselection.Font = new Font("Arial", 12F, FontStyle.Bold);
            Subjectselection.ForeColor = Color.SaddleBrown;
            Subjectselection.Location = new Point(528, 342);
            Subjectselection.Name = "Subjectselection";
            Subjectselection.Size = new Size(0, 24);
            Subjectselection.TabIndex = 18;
            // 
            // selectionType
            // 
            selectionType = new ComboBox();
            selectionType.DropDownStyle = ComboBoxStyle.DropDownList;
            selectionType.FormattingEnabled = true;
            selectionType.Items.AddRange(new object[] { "סטודנט רגיל", "סטודנט מתרגל", "מרצה", "ראש מחלקה" });
            selectionType.Location = new Point(394, 288);
            selectionType.Name = "selectionType";
            selectionType.Size = new Size(151, 28);
            selectionType.TabIndex = 14;
            selectionType.SelectedIndexChanged += ShowOtherSelections;
            selectionType.Leave += TextBoxCheck;
            // 
            // apply
            // 
            apply = new Button();
            apply.Location = new Point(39, 297);
            apply.Name = "apply";
            apply.Size = new Size(175, 29);
            apply.TabIndex = 13;
            apply.Text = "בחר תמונת פרופיל";
            apply.UseVisualStyleBackColor = true;
            apply.Click += Apply_Click;
            // 
            // profilePicture
            // 
            profilePicture = new PictureBox();
            profilePicture.Location = new Point(39, 76);
            profilePicture.Name = "profilePicture";
            profilePicture.Size = new Size(175, 205);
            profilePicture.TabIndex = 12;
            profilePicture.TabStop = false;
            // 
            // date
            // 
            date = new DateTimePicker();
            date.Location = new Point(436, 135);
            date.MaxDate = new DateTime(2011, 7, 11, 0, 0, 0, 0);
            date.MinDate = new DateTime(1905, 7, 11, 0, 0, 0, 0);
            date.Name = "date";
            date.Size = new Size(236, 27);
            date.TabIndex = 11;
            date.Value = new DateTime(2011, 7, 11, 0, 0, 0, 0);
            // 
            // gmailBox
            // 
            gmailBox = new TextBox();
            gmailBox.Location = new Point(358, 238);
            gmailBox.Name = "gmailBox";
            gmailBox.PlaceholderText = "אימייל";
            gmailBox.RightToLeft = RightToLeft.Yes;
            gmailBox.Size = new Size(314, 27);
            gmailBox.TabIndex = 10;
            gmailBox.Leave += TextBoxCheck;
            // 
            // academicSubject
            // 
            academicSubject = new ComboBox();
            academicSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            academicSubject.FormattingEnabled = true;
            academicSubject.Items.AddRange(new object[] { "הנדסה – הנדסת תוכנה", "הנדסה – הנדסת חשמל ואלקטרוניקה", "הנדסה – הנדסת מכונות", "הנדסה – הנדסת אזרחית", "הנדסה – הנדסת כימיה", "הנדסה – הנדסת תעשייה וניהול", "הנדסה – הנדסת אווירונאוטיקה וחלל", "הנדסה – הנדסת ביוטכנולוגיה", "מדעי המחשב וטכנולוגיה – מדעי המחשב", "מדעי המחשב וטכנולוגיה – מדעי הנתונים (Data Science)", "מדעי המחשב וטכנולוגיה – בינה מלאכותית ולמידת מכונה", "מדעי המחשב וטכנולוגיה – אבטחת מידע וסייבר", "מדעי המחשב וטכנולוגיה – רשתות ומחשוב ענן", "מדעי המחשב וטכנולוגיה – הנדסת תוכנה", "מדעי המחשב וטכנולוגיה – אינטראקציה אדם–מחשב (HCI)", "עסקים וכלכלה – חשבונאות", "עסקים וכלכלה – שיווק", "עסקים וכלכלה – מימון והשקעות", "עסקים וכלכלה – מנהל עסקים", "עסקים וכלכלה – כלכלה", "עסקים וכלכלה – יזמות והקמת עסקים", "עסקים וכלכלה – לוגיסטיקה ושרשרת אספקה", "מדעי החיים ובריאות – רפואה", "מדעי החיים ובריאות – סיעוד", "מדעי החיים ובריאות – מדעי המעבדה הרפואית", "מדעי החיים ובריאות – ביולוגיה", "מדעי החיים ובריאות – תזונה ודיאטה", "מדעי החיים ובריאות – ביוטכנולוגיה", "מדעי החיים ובריאות – בריאות הציבור", "מדעי החיים ובריאות – פסיכולוגיה קלינית", "מדעים מדויקים וטכנולוגיים – פיזיקה", "מדעים מדויקים וטכנולוגיים – מתמטיקה", "מדעים מדויקים וטכנולוגיים – סטטיסטיקה", "מדעים מדויקים וטכנולוגיים – כימיה", "מדעים מדויקים וטכנולוגיים – גיאולוגיה", "מדעים מדויקים וטכנולוגיים – מדעי הסביבה", "מדעים מדויקים וטכנולוגיים – מדעי האטמוספירה ואוקיינוגרפיה", "משפטים ומדעי החברה – משפטים", "משפטים ומדעי החברה – מדעי המדינה", "משפטים ומדעי החברה – סוציולוגיה", "משפטים ומדעי החברה – תקשורת וחינוך מדיה", "משפטים ומדעי החברה – אנתרופולוגיה", "משפטים ומדעי החברה – חינוך (גיל הרך, יסודי, חינוך מיוחד)", "משפטים ומדעי החברה – עבודה סוציאלית", "אמנויות ועיצוב – אדריכלות", "אמנויות ועיצוב – עיצוב גרפי", "אמנויות ועיצוב – אומנות פלסטית (רישום, פיסול)", "אמנויות ועיצוב – עיצוב פנים", "אמנויות ועיצוב – עיצוב תעשייתי", "אמנויות ועיצוב – תיאטרון וקולנוע", "אמנויות ועיצוב – מוזיקה (הלחנה, ביצוע)", "מדעי הרוח – פילוסופיה", "מדעי הרוח – לשון וספרות", "מדעי הרוח – היסטוריה", "מדעי הרוח – דתות ותיאולוגיה", "מדעי הרוח – יחסים בין־לאומיים" });
            academicSubject.Location = new Point(194, 342);
            academicSubject.Name = "academicSubject";
            academicSubject.Size = new Size(309, 28);
            academicSubject.TabIndex = 17;
            academicSubject.SelectedIndexChanged += TextBoxCheck;
            academicSubject.Hide();
            // 
            // specializationSubject
            // 
            specializationSubject = new ComboBox();
            specializationSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            specializationSubject.FormattingEnabled = true;
            specializationSubject.Items.AddRange(new object[] { "הנדסת תוכנה", "הנדסת חשמל", "הנדסת מכונות", "הנדסה אזרחית", "מדעי המחשב", "אבטחת מידע", "מדעי הנתונים", "מתמטיקה", "פיזיקה", "כימיה", "ביולוגיה", "ביוטכנולוגיה", "רפואה", "סיעוד", "פסיכולוגיה", "מדעי ההתנהגות", "חינוך מיוחד", "חינוך לגיל הרך", "כלכלה", "משפטים", "מנהל עסקים", "חשבונאות", "שיווק", "אדריכלות", "עיצוב גרפי", "תיאטרון", "קולנוע", "ספרות", "היסטוריה", "פילוסופיה", "מדעי המדינה", "תקשורת" });
            specializationSubject.Location = new Point(300, 342);
            specializationSubject.Name = "specializationSubject";
            specializationSubject.Size = new Size(203, 28);
            specializationSubject.TabIndex = 17;
            specializationSubject.SelectedIndexChanged += TextBoxCheck;
            specializationSubject.Hide();
            // 
            // phoneNumBox
            // 
            phoneNumBox = new TextBox();
            phoneNumBox.Location = new Point(236, 189);
            phoneNumBox.Name = "phoneNumBox";
            phoneNumBox.PlaceholderText = "מספר טלפון";
            phoneNumBox.RightToLeft = RightToLeft.Yes;
            phoneNumBox.Size = new Size(232, 27);
            phoneNumBox.TabIndex = 9;
            phoneNumBox.Leave += TextBoxCheck;
            // 
            // idBox
            // 
            idBox = new TextBox();
            idBox.Location = new Point(494, 189);
            idBox.Name = "idBox";
            idBox.PlaceholderText = "תז";
            idBox.RightToLeft = RightToLeft.Yes;
            idBox.Size = new Size(178, 27);
            idBox.TabIndex = 8;
            idBox.Leave += TextBoxCheck;
            // 
            // fmNameBox
            // 
            fmNameBox = new TextBox();
            fmNameBox.Location = new Point(341, 86);
            fmNameBox.Name = "fmNameBox";
            fmNameBox.PlaceholderText = "שם משפחה";
            fmNameBox.RightToLeft = RightToLeft.Yes;
            fmNameBox.Size = new Size(150, 27);
            fmNameBox.TabIndex = 7;
            fmNameBox.Leave += TextBoxCheck;
            // 
            // nameBox
            // 
            nameBox = new TextBox();
            nameBox.Location = new Point(522, 86);
            nameBox.Name = "nameBox";
            nameBox.PlaceholderText = "שם";
            nameBox.RightToLeft = RightToLeft.Yes;
            nameBox.Size = new Size(150, 27);
            nameBox.TabIndex = 6;
            nameBox.Leave += TextBoxCheck;
            // 
            // loginASlable
            // 
            loginASlable = new Label();
            loginASlable.AutoSize = true;
            loginASlable.Font = new Font("Arial", 12F, FontStyle.Bold);
            loginASlable.ForeColor = Color.SaddleBrown;
            loginASlable.Location = new Point(581, 288);
            loginASlable.Name = "loginASlable";
            loginASlable.Size = new Size(93, 24);
            loginASlable.TabIndex = 5;
            loginASlable.Text = "- הירשם כ";
            // 
            // line
            // 
            line = new Label();
            line.AutoSize = true;
            line.ForeColor = Color.SaddleBrown;
            line.Location = new Point(39, 53);
            line.Name = "line";
            line.Size = new Size(633, 20);
            line.TabIndex = 2;
            line.Text = "--------------------------------------------------------------------------------------------------------";
            // 
            // login
            // 
            login = new Label();
            login.AutoSize = true;
            login.Font = new Font("Arial", 12F, FontStyle.Bold);
            login.ForeColor = Color.SaddleBrown;
            login.Location = new Point(289, 29);
            login.Name = "login";
            login.Size = new Size(134, 24);
            login.TabIndex = 1;
            login.Text = "צור חשבון חדש";
            // 
            // backgroundPanel
            // 
            backgroundPanel = new Panel();
            backgroundPanel.BackColor = Color.PaleGoldenrod;
            backgroundPanel.Controls.Add(goBack);
            backgroundPanel.Controls.Add(classSubject);
            backgroundPanel.Controls.Add(error);
            backgroundPanel.Controls.Add(sendBox);
            backgroundPanel.Controls.Add(Subjectselection);
            backgroundPanel.Controls.Add(selectionType);
            backgroundPanel.Controls.Add(apply);
            backgroundPanel.Controls.Add(profilePicture);
            backgroundPanel.Controls.Add(date);
            backgroundPanel.Controls.Add(gmailBox);
            backgroundPanel.Controls.Add(academicSubject);
            backgroundPanel.Controls.Add(specializationSubject);
            backgroundPanel.Controls.Add(phoneNumBox);
            backgroundPanel.Controls.Add(idBox);
            backgroundPanel.Controls.Add(fmNameBox);
            backgroundPanel.Controls.Add(nameBox);
            backgroundPanel.Controls.Add(loginASlable);
            backgroundPanel.Controls.Add(line);
            backgroundPanel.Controls.Add(login);
            backgroundPanel.Location = new Point(0, 0);
            backgroundPanel.Name = "backgroundPanel";
            backgroundPanel.Size = new Size(703, 436);

            backgroundPanel.TabIndex = 0;
            backgroundPanel.ResumeLayout(false);
            backgroundPanel.PerformLayout();
            Controls.Add(backgroundPanel);
            // 
            // openFileDialog1
            // 
            openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "בחירת תמונת פרופיל";
            backgroundPanel.SuspendLayout();
            ((ISupportInitialize)profilePicture).BeginInit();
            ((ISupportInitialize)profilePicture).EndInit();
            SuspendLayout();
           
        }
        private void ReturnToStartWindow(object sender, EventArgs e)
        {
            this.startWindow.Show();
            this.Hide();
            this.Dispose();
        }
        
    }


}
