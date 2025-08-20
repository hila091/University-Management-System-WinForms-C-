
using System.Buffers.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using My_university_WinFormsApp.Models;


namespace My_university_WinFormsApp
{
    public partial class HeadOfDepartmentWindow : Form
    {
        private HeadOfDepartment user;
        private bool isMenuOpen;
        private bool isDay;
        private bool editMood;

        Dictionary<string, Image> dayIcons = new Dictionary<string, Image>();
        Dictionary<string, Image> nightIcons = new Dictionary<string, Image>();

        public Dictionary<string, Color> yellowPaletteDay = new Dictionary<string, Color>();
        public Dictionary<string, Color> yellowPaletteNight = new Dictionary<string, Color>();
        //------------------------------------------------------------------------------------
        private List<Panel> messagesPanelList = new List<Panel>();// אני רוצה רשימה של כל הפאנלים של ההודעות זה יהיה יותר קל ככה
        //------------------------------------------------------------------------------------
        private List<Lecturer> allLecturers = new List<Lecturer>();  // רשימה שמכילה את כל המרצים שיש באותו מסלול התמחות שהמרצה הנוכחי מלמד
        //------------------------------------------------------------------------------------
        private RichTextBox coreProgramLable;
        private RichTextBox courseDetails;
        private RichTextBox specializationLable;

        private RichTextBox userInfro;

        private Label dayLable;
        private Label nightLable;


        private TextBox searshBox;
        private PictureBox searchIcon;

        private TabPage sendMessages;
        private TabPage showMessages;
        private Panel messagePanel;
        private Button importanceSearch;
        private Button favoriteSearch;
        private Label searchLable;
        private ComboBox dateSearch;
        private RichTextBox messageContent;
        private Label sendingDate;
        private Label senderDetails;
        private Button replyButton;
        private Button favorMessages;
        private Panel writingPanel;
        private Button sendButton;
        private TextBox destinationName;
        private RichTextBox writingMessage;
        private Button removeMessage;

        private Panel coursesNamesPanel;
        private Panel studentsNamesPanel;
        private RichTextBox studentsTaught;
        private RichTextBox coursesTaught;

        private Button requiredRouteBtn;
        private RichTextBox specializationTrack;
        private RichTextBox CoreCourse;
        private Button remove;
        private Button changeName;
        private Panel specializationShowPanel;
        private Panel coreProgramShowPanel;

        private Button addSpecializationBtn;
        private Button addCourseBtn;

        private Label goBack;

        private Panel courseDetailsPanel;
        private Panel courseLecturerPanel;
        private Panel createSpecializationPanel;
        private Panel createCoursePanel;
        private Label lecturerLabel;
        private Label redNotes;

        private TextBox enterAnswer;
        private Button makeItEditable;
        private Button addStudentBtn;
        private DataGridView lecturerDetails; // <---- טבלה שאליה הראש מחלקה יוכל להכניס עידכונים לגבי מרצה קיים

        private Label error;

        private Dictionary<string, bool> fieldStatus = new Dictionary<string, bool>(); // מילון שדות תקינים 

        private Dictionary<string, string> errorMessage = new Dictionary<string, string>(); // מילון הודעות לשדות לא תקינים 

        // הזנת פרטים בטקסטבוקסים ליצירת אובייקטים חדשים 
        private Button apply;
        private TextBox nameBox;
        private TextBox fmNameBox;
        private DateTimePicker date;
        private TextBox gmailBox;
        private TextBox phoneNumBox;
        private TextBox idBox;
        private ComboBox selectionType;
        private OpenFileDialog openFileDialog;
        private Button createCourse;
        private ComboBox SelectExistingLecturer; // תיבת בחירה של מרצה קיים 
        private bool SelectedLecturer; // דגל שמסמן לי אם המשתמש בחר במרצה קיים או יצר מרצה
        //------------------------------------------------------------------------------------
        public HeadOfDepartmentWindow(HeadOfDepartment user)
        {

            InitializeComponent();
            HeadOfDepartmentPage();
            this.isDay = true; // כי הברירת מחדל זה שהמצב הוא מצב יום 
            this.isMenuOpen = false;
            this.SelectedLecturer = false; 
            this.FormClosing += CloseAll;
            this.user = user;
            // הוא נכנס לעצמו ומפעיל את הפונקציה שמחזירה את כל הסגל שיש לו במחלקה לא כולל עצמו
            this.allLecturers = this.user.allLecturersForHD(); //<< זאת פשוט פעולה שבאה מהמחלקה של המרצים 
            // אני כן רוצה שהראש מחלקה יהיה חלק מהאופציות האלה כדי שיוכל להסיף אותו כמרצה לקורסים
            this.allLecturers.Add(this.user);

            this.editMood = false; // מצב עריכה 
            // איפוס פלטת אייקונים למצב יום בברירת מחדל
            dayIcons["infro"] = Image.FromFile(@"..\..\..\..\imgs\infro2.png");
            dayIcons["menu"] = Image.FromFile(@"..\..\..\..\imgs\menu2.png");
            dayIcons["lightMood"] = Image.FromFile(@"..\..\..\..\imgs\lightMood2.png");
            dayIcons["nightMood"] = Image.FromFile(@"..\..\..\..\imgs\nightMood2.png");
            dayIcons["message"] = Image.FromFile(@"..\..\..\..\imgs\message2.png");
            dayIcons["search"] = Image.FromFile(@"..\..\..\..\imgs\search2.png");
            dayIcons["logOut"] = Image.FromFile(@"..\..\..\..\imgs\logOut2.png");
            dayIcons["settings"] = Image.FromFile(@"..\..\..\..\imgs\settings2.png");
            dayIcons["on"] = Image.FromFile(@"..\..\..\..\imgs\on2.png");
            dayIcons["courseTaught"] = Image.FromFile(@"..\..\..\..\imgs\teaching2.png");
            dayIcons["myClass"] = Image.FromFile(@"..\..\..\..\imgs\myClass2.png");
            dayIcons["newRequests"] = Image.FromFile(@"..\..\..\..\imgs\newRequests2.png");
            dayIcons["add"] = Image.FromFile(@"..\..\..\..\imgs\add2.png");
            dayIcons["changeName"] = Image.FromFile(@"..\..\..\..\imgs\changeName2.png");
            dayIcons["remove"] = Image.FromFile(@"..\..\..\..\imgs\removeSTruck2.png");


            // איפוס פלטת אייקונים למצב לילה 
            nightIcons["infro"] = Image.FromFile(@"..\..\..\..\imgs\infro1.png");
            nightIcons["menu"] = Image.FromFile(@"..\..\..\..\imgs\menu1.png");
            nightIcons["lightMood"] = Image.FromFile(@"..\..\..\..\imgs\lightMood1.png");
            nightIcons["nightMood"] = Image.FromFile(@"..\..\..\..\imgs\nightMood1.png");
            nightIcons["message"] = Image.FromFile(@"..\..\..\..\imgs\message1.png");
            nightIcons["search"] = Image.FromFile(@"..\..\..\..\imgs\search1.png");
            nightIcons["logOut"] = Image.FromFile(@"..\..\..\..\imgs\logOut1.png");
            nightIcons["settings"] = Image.FromFile(@"..\..\..\..\imgs\settings1.png");
            nightIcons["off"] = Image.FromFile(@"..\..\..\..\imgs\off1.png");
            nightIcons["courseTaught"] = Image.FromFile(@"..\..\..\..\imgs\teaching1.png");
            nightIcons["myClass"] = Image.FromFile(@"..\..\..\..\imgs\myClass1.png");
            nightIcons["newRequests"] = Image.FromFile(@"..\..\..\..\imgs\newRequests1.png");
            nightIcons["add"] = Image.FromFile(@"..\..\..\..\imgs\add1.png");
            nightIcons["changeName"] = Image.FromFile(@"..\..\..\..\imgs\changeName1.png");
            nightIcons["remove"] = Image.FromFile(@"..\..\..\..\imgs\removeSTruck1.png");


            // פלטת צבעים צהובה מצב יום
            yellowPaletteDay["baseColor"] = Color.PaleGoldenrod;
            yellowPaletteDay["buttonColor"] = Color.Moccasin;
            yellowPaletteDay["otherColor"] = Color.LemonChiffon;
            yellowPaletteDay["titleColor"] = Color.SaddleBrown;
            yellowPaletteDay["textColor"] = Color.FromArgb(204, 153, 0);
            yellowPaletteDay["backGroundColor"] = Color.FromArgb(238, 240, 211);

            // פלטת צבעים (ששאפה להיות צהובה) מצב לילה
            yellowPaletteNight["baseColor"] = Color.FromArgb(139, 144, 81);
            yellowPaletteNight["buttonColor"] = Color.FromArgb(91, 95, 37);
            yellowPaletteNight["otherColor"] = Color.FromArgb(167, 172, 89);
            yellowPaletteNight["titleColor"] = Color.LemonChiffon;
            yellowPaletteNight["textColor"] = Color.FromArgb(238, 240, 211);
            yellowPaletteNight["backGroundColor"] = Color.FromArgb(105, 112, 2);


            //--------------------------------- > מילון שמאחסן לי את המצב הנוכחי של כל שדה אם הוא תקין או לא
            fieldStatus["specializationName"] = false;
            fieldStatus["idBox"] = false;
            fieldStatus["phoneNumBox"] = false;
            fieldStatus["nameBox"] = false;
            fieldStatus["fmNameBox"] = false;
            fieldStatus["gmailBox"] = false;
            fieldStatus["selectionType"] = false;
            //--------------------------------- > מילון שמאחסן לי את ההודעות שהמערכת מחזירה אם המצב הנוכחי של השדה לא תקין
            errorMessage["specializationName"] = "שם המסלול לא יכול להכיל מספרים";
            errorMessage["idBox"] = "תעודת זהות לא תקינה – חייבת להיות 9 ספרות";
            errorMessage["phoneNumBox"] = "מספר טלפון לא תקין – חייב להיות 10 ספרות";
            errorMessage["nameBox"] = "שם לא תקין-  לא יכול להכיל תווים או מספרים";
            errorMessage["fmNameBox"] = "שם משפחה לא תקין -  לא יכול להכיל תווים או מספרים";
            errorMessage["gmailBox"] = "example123@email.com - אימייל חייב לכלול תבנית כזו";
            errorMessage["selectionType"] = "עליך לבחור ערך מהרשימה";

        }


        private void HeadOfDepartmentPage()
        {
            topPanel = new Panel();
            menuIcon = new PictureBox();
            helloLable = new Label();
            menuPanel = new Panel();
            logOut = new Button();
            prsonalInfro = new Button();
            settings = new Button();
            message = new Button();
            search = new Button();
            newRequestsBtn = new Button();
            courseTaught = new Button();
            myClassBtn = new Button();
            infroPanel = new Panel();
            settingsPanel = new Panel();
            coursesPanel = new Panel();
            switchIcon = new PictureBox();
            title = new Label();
            profilePicture = new PictureBox();
            messageTabControl = new TabControl();
            courseTaughtPanel = new Panel();
            searchingPanel = new Panel();
            newRequestsPanel = new Panel();
            myClassPanel = new Panel();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)menuIcon).BeginInit();
            menuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)switchIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)profilePicture).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.PaleGoldenrod;
            topPanel.Controls.Add(menuIcon);
            topPanel.Controls.Add(helloLable);
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(625, 82);
            topPanel.TabIndex = 0;
            // 
            // menuIcon
            // 
            menuIcon.Location = new Point(492, 12);
            menuIcon.Name = "menuIcon";
            menuIcon.Size = new Size(45, 45);
            menuIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            menuIcon.TabIndex = 4;
            menuIcon.TabStop = false;
            menuIcon.Click += openORcloseMenu;
            // 
            // helloLable
            // 
            helloLable.AutoSize = true;
            helloLable.Font = new Font("Arial", 15F, FontStyle.Bold);
            helloLable.ForeColor = Color.SaddleBrown;
            helloLable.Location = new Point(260, 28);
            helloLable.Name = "helloLable";
            helloLable.RightToLeft = RightToLeft.Yes;
            helloLable.Size = new Size(118, 30);
            helloLable.TabIndex = 2;
            helloLable.Text = "ברוך הבא ";
            helloLable.TextAlign = ContentAlignment.MiddleRight;
            // 
            // menuPanel
            // 
            menuPanel.BackColor = Color.PaleGoldenrod;
            menuPanel.Controls.Add(logOut);
            menuPanel.Controls.Add(prsonalInfro);
            menuPanel.Controls.Add(settings);
            menuPanel.Controls.Add(message);
            menuPanel.Controls.Add(search);
            menuPanel.Controls.Add(courseTaught);
            menuPanel.Controls.Add(newRequestsBtn);
            menuPanel.Controls.Add(myClassBtn);
            menuPanel.Location = new Point(625, 169);
            menuPanel.Name = "menuPanel";
            menuPanel.Size = new Size(210, 550);
            menuPanel.TabIndex = 3;
            // 
            // logOut
            // 
            logOut.Font = new Font("Arial", 10F, FontStyle.Bold);
            logOut.ForeColor = Color.SaddleBrown;
            logOut.Location = new Point(0, 472);
            logOut.Name = "logOut";
            logOut.Size = new Size(210, 53);
            logOut.TabIndex = 4;
            logOut.Text = " יציאה";
            logOut.TextImageRelation = TextImageRelation.TextBeforeImage;
            logOut.ImageAlign = ContentAlignment.MiddleRight;
            logOut.UseVisualStyleBackColor = true;
            logOut.Click += logOut_Click;
            // 
            // prsonalInfro
            // 
            prsonalInfro.Font = new Font("Arial", 10F, FontStyle.Bold);
            prsonalInfro.ForeColor = Color.SaddleBrown;
            prsonalInfro.ImageAlign = ContentAlignment.MiddleRight;
            prsonalInfro.Location = new Point(0, 24);
            prsonalInfro.Name = "prsonalInfro";
            prsonalInfro.Size = new Size(210, 53);
            prsonalInfro.TabIndex = 4;
            prsonalInfro.Text = " מידע אישי";
            prsonalInfro.TextImageRelation = TextImageRelation.TextBeforeImage;
            prsonalInfro.UseVisualStyleBackColor = true;
            prsonalInfro.Click += prsonalInfro_Click;
            // 
            // settings
            // 
            settings.Font = new Font("Arial", 10F, FontStyle.Bold);
            settings.ForeColor = Color.SaddleBrown;
            settings.ImageAlign = ContentAlignment.MiddleRight;
            settings.Location = new Point(0, 201);
            settings.Name = "settings";
            settings.Size = new Size(210, 53);
            settings.TabIndex = 4;
            settings.Text = " הגדרות";
            settings.TextImageRelation = TextImageRelation.TextBeforeImage;
            settings.UseVisualStyleBackColor = true;
            settings.Click += settings_Click;
            // 
            // message
            // 
            message.Font = new Font("Arial", 10F, FontStyle.Bold);
            message.ForeColor = Color.SaddleBrown;
            message.ImageAlign = ContentAlignment.MiddleRight;
            message.Location = new Point(0, 83);
            message.Name = "message";
            message.Size = new Size(210, 53);
            message.TabIndex = 4;
            message.Text = " הודעות";
            message.TextImageRelation = TextImageRelation.TextBeforeImage;
            message.UseVisualStyleBackColor = true;
            message.Click += message_Click;
            // 
            // search
            // 
            search.Font = new Font("Arial", 10F, FontStyle.Bold);
            search.ForeColor = Color.SaddleBrown;
            search.ImageAlign = ContentAlignment.MiddleRight;
            search.Location = new Point(0, 142);
            search.Name = "search";
            search.Size = new Size(210, 53);
            search.TabIndex = 4;
            search.Text = " חיפוש";
            search.TextImageRelation = TextImageRelation.TextBeforeImage;
            search.UseVisualStyleBackColor = true;
            search.Click += search_Click;
            // 
            // courseTaught
            // 
            courseTaught.Font = new Font("Arial", 10F, FontStyle.Bold);
            courseTaught.ForeColor = Color.SaddleBrown;
            courseTaught.ImageAlign = ContentAlignment.MiddleRight;
            courseTaught.Location = new Point(0, 260);
            courseTaught.Name = "courseTaught";
            courseTaught.Size = new Size(210, 53);
            courseTaught.TabIndex = 5;
            courseTaught.Text = "הקורסים שאני מלמד";
            courseTaught.TextImageRelation = TextImageRelation.TextBeforeImage;
            courseTaught.UseVisualStyleBackColor = true;
            courseTaught.Click += courseTaught_Click;
            // 
            // infroPanel
            // 
            infroPanel.Location = new Point(0, 0);
            infroPanel.Name = "infroPanel";
            infroPanel.Size = new Size(200, 100);
            infroPanel.TabIndex = 0;
            // 
            // settingsPanel
            // 
            settingsPanel.Location = new Point(0, 0);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new Size(200, 100);
            settingsPanel.TabIndex = 0;
            // 
            // coursesPanel
            // 
            coursesPanel.Location = new Point(0, 0);
            coursesPanel.Name = "coursesPanel";
            coursesPanel.Size = new Size(200, 100);
            coursesPanel.TabIndex = 0;
            // 
            // switchIcon
            // 
            switchIcon.Location = new Point(0, 0);
            switchIcon.Name = "switchIcon";
            switchIcon.Size = new Size(100, 50);
            switchIcon.TabIndex = 0;
            switchIcon.TabStop = false;
            switchIcon.Click += ChangeMood;
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Arial", 15F, FontStyle.Bold);
            title.ForeColor = Color.SaddleBrown;
            title.Location = new Point(270, 96);
            title.Name = "title";
            title.Size = new Size(57, 30);
            title.TabIndex = 4;
            title.Text = "title";
            // 
            // profilePicture
            // 
            profilePicture.Location = new Point(660, 12);
            profilePicture.Name = "profilePicture";
            profilePicture.Size = new Size(144, 139);
            profilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            profilePicture.TabIndex = 5;
            profilePicture.TabStop = false;
            // 
            // messageTabControl
            // 
            messageTabControl.Location = new Point(0, 0);
            messageTabControl.Name = "messageTabControl";
            messageTabControl.SelectedIndex = 0;
            messageTabControl.Size = new Size(200, 100);
            messageTabControl.TabIndex = 0;
            // 
            // courseTaughtPanel
            // 
            courseTaughtPanel.Location = new Point(0, 0);
            courseTaughtPanel.Name = "courseTaughtPanel";
            courseTaughtPanel.Size = new Size(200, 100);
            courseTaughtPanel.TabIndex = 0;
            // 
            // searchingPanel
            // 
            searchingPanel.AutoScroll = true;
            searchingPanel.BackColor = Color.LemonChiffon;
            searchingPanel.Location = new Point(46, 138);
            searchingPanel.Name = "searchingPanel";
            searchingPanel.Size = new Size(533, 569);
            searchingPanel.TabIndex = 4;
            //
            // myClassBtn
            //
            myClassBtn.Font = new Font("Arial", 10F, FontStyle.Bold);
            myClassBtn.ForeColor = Color.SaddleBrown;
            myClassBtn.ImageAlign = ContentAlignment.MiddleRight;
            myClassBtn.Location = new Point(0, 319);
            myClassBtn.Name = "myClassBtn";
            myClassBtn.Size = new Size(210, 53);
            myClassBtn.TabIndex = 5;
            myClassBtn.Text = "המחלקה שלי";
            myClassBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
            myClassBtn.UseVisualStyleBackColor = true;
            myClassBtn.Click += myClass_Click;
            //
            // newRequestsBtn
            //
            newRequestsBtn.Font = new Font("Arial", 10F, FontStyle.Bold);
            newRequestsBtn.ForeColor = Color.SaddleBrown;
            newRequestsBtn.ImageAlign = ContentAlignment.MiddleRight;
            newRequestsBtn.Location = new Point(0, 378);
            newRequestsBtn.Name = "newRequestsBtn";
            newRequestsBtn.Size = new Size(210, 53);
            newRequestsBtn.TabIndex = 5;
            newRequestsBtn.Text = "בקשות חדשות";
            newRequestsBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
            newRequestsBtn.UseVisualStyleBackColor = true;
            newRequestsBtn.Click += newRequests_Click;
            // 
            // HeadOfDepartmentWindow
            // 
            Controls.Add(profilePicture);
            Controls.Add(title);
            Controls.Add(menuPanel);
            Controls.Add(topPanel);

            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)menuIcon).EndInit();

            menuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)switchIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)profilePicture).EndInit();

        }
        private void HeadOfDepartmentWindow_Load(object sender, EventArgs e)
        {

            this.BackColor = yellowPaletteDay["backGroundColor"];

            profilePicture.Image = this.user.ProfileImage;

            helloLable.Text = "ברוך הבא " + this.user.Name + " " + this.user.FmName;
            menuIcon.Image = this.dayIcons["menu"];


            prsonalInfro.Image = this.dayIcons["infro"];
            prsonalInfro.BackColor = yellowPaletteDay["buttonColor"];

            message.Image = this.dayIcons["message"];
            message.BackColor = yellowPaletteDay["buttonColor"];

            search.Image = this.dayIcons["search"];
            search.BackColor = yellowPaletteDay["buttonColor"];

            settings.Image = this.dayIcons["settings"];
            settings.BackColor = yellowPaletteDay["buttonColor"];

            logOut.Image = this.dayIcons["logOut"];
            logOut.BackColor = yellowPaletteDay["buttonColor"];

            courseTaught.Image = this.dayIcons["courseTaught"];
            courseTaught.BackColor = yellowPaletteDay["buttonColor"];

            newRequestsBtn.Image = this.dayIcons["newRequests"];
            newRequestsBtn.BackColor = yellowPaletteDay["buttonColor"];

            myClassBtn.Image = this.dayIcons["myClass"];
            myClassBtn.BackColor = yellowPaletteDay["buttonColor"];


            // יצירת כל העמודים 


            createPresonalInfroPage();
            crearteSettingsPage();
            createSearchPage();
            createMessagePage();
            createCourseTaughtPage();

            createNewRequestsPage();

            createMyClassPage();



            removeAll();
            title.Text = "כל הקורסים";
            Controls.Add(courseTaughtPanel);


            /* אני צריכה לעדכן את ההתחלה של המספרי עובדים וסטודנטים כדי שלא יהיו כפילויות 
               זה צריך להיות כאן כי רק הראש מחלקה יכול להוסיף מרצים או סטודנטים
            אז אני צריכה שהמספרי עובדים וסטודנטים שמהם מתחילים ליצור אובייקטים חדשים יהיו עדכניים לשינויים האחרונים */
            Lecturer.startID = Lecturer.FindLastID();
            Student.startID = Student.FindLastID();



        }


        private void openORcloseMenu(object sender, EventArgs e)
        {
            if (this.isMenuOpen)
            {
                ClientSize = new Size(625, 719);
                this.isMenuOpen = false;
            }

            else
            {
                this.isMenuOpen = true;
                ClientSize = new Size(834, 719);
            }
        }

        // ------------------------------------------------------- פעולות ליצירת עמודים     
        private void createPresonalInfroPage()
        {

            infroPanel.AutoScroll = true;
            infroPanel.BackColor = yellowPaletteDay["otherColor"];
            infroPanel.Location = new Point(46, 138);
            infroPanel.Name = "infroPanel";
            infroPanel.Size = new Size(533, 569);
            infroPanel.TabIndex = 4;
            // 
            // userInfro
            // 
            userInfro = new RichTextBox();
            userInfro.Location = new Point(237, 26);
            userInfro.Name = "userInfro";
            userInfro.Size = new Size(270, 377);
            userInfro.TabIndex = 6;
            userInfro.Text = "";
            userInfro.Font = new Font("Arial", 10F, FontStyle.Bold);
            userInfro.ForeColor = yellowPaletteDay["textColor"];
            userInfro.BackColor = yellowPaletteDay["baseColor"];
            userInfro.RightToLeft = RightToLeft.Yes;
            userInfro.Text = this.user.Name + " " + this.user.FmName + Environment.NewLine
                              + "גיל :" + this.user.Age + Environment.NewLine
                              + "מספר טלפון: " + this.user.PhoneNum + Environment.NewLine
                              + "אימייל: " + this.user.Gmail + Environment.NewLine
                              + "מספר עובד :" + this.user.EmployeeNumber + Environment.NewLine
                              + "תז: " + this.user.Id + Environment.NewLine
                              + "אחראי/ת מחלקה: " + this.user.SpecializationName + Environment.NewLine +
                               " דירוג סטודנטים" + Environment.NewLine;


            profilePicture = new PictureBox();
            profilePicture.Location = new Point(15, 12);
            profilePicture.Name = "profilePicture";
            profilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            profilePicture.Size = new Size(160, 160);
            profilePicture.TabIndex = 5;
            profilePicture.TabStop = false;
            profilePicture.Image = this.user.ProfileImage;

            infroPanel.Controls.Add(profilePicture);
            infroPanel.Controls.Add(userInfro);

        }

        private void crearteSettingsPage()
        {
            settingsPanel.AutoScroll = true;
            settingsPanel.BackColor = yellowPaletteDay["otherColor"];
            settingsPanel.Location = new Point(46, 138);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new Size(533, 569);
            settingsPanel.TabIndex = 4;
            // 
            // dayLable
            // 
            dayLable = new Label();
            dayLable.AutoSize = true;
            dayLable.Location = new Point(360, 55);
            dayLable.Name = "dayLable";
            dayLable.Size = new Size(50, 20);
            dayLable.TabIndex = 0;
            dayLable.Text = "Day Mood";
            dayLable.ForeColor = yellowPaletteDay["titleColor"];
            dayLable.Font = new Font("Arial", 15F, FontStyle.Bold);
            // 
            // nightLable
            // 
            nightLable = new Label();
            nightLable.AutoSize = true;
            nightLable.Location = new Point(45, 55);
            nightLable.Name = "nightLable";
            nightLable.Size = new Size(50, 20);
            nightLable.TabIndex = 1;
            nightLable.Text = "Night Mood";
            nightLable.ForeColor = yellowPaletteDay["titleColor"];
            nightLable.Font = new Font("Arial", 15F, FontStyle.Bold);
            // 
            // switchIcon
            // 
            switchIcon.Location = new Point(235, 45);
            switchIcon.Name = "switchIcon";
            switchIcon.Size = new Size(60, 60);
            switchIcon.TabIndex = 2;
            switchIcon.TabStop = false;
            switchIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            switchIcon.Image = dayIcons["on"];

            switchIcon.Click += ChangeFlag;
            switchIcon.Click += ChangeMood;



            // ------------------------------
            settingsPanel.Controls.Add(switchIcon);
            settingsPanel.Controls.Add(nightLable);
            settingsPanel.Controls.Add(dayLable);
        }

        private void createMessagePage()
        {
            // 
            // dateSearch - תיבת חלוקה חיפוש לפי תאריך
            // 
            dateSearch = new ComboBox();
            dateSearch.DropDownStyle = ComboBoxStyle.DropDownList;
            dateSearch.FormattingEnabled = true;
            dateSearch.Items.AddRange(new object[] { "התאריך המוקדם ביותר", "התאריך המאוחר ביותר" });
            dateSearch.Location = new Point(244, 32);
            dateSearch.Name = "dateSearch";
            dateSearch.Size = new Size(170, 28);
            dateSearch.TabIndex = 9;
            dateSearch.SelectedIndexChanged += OrderByEarliestDateOrOldestDate;

            // 
            // importanceSearch - חיפוש לפי חשיבות
            // 
            importanceSearch = new Button();
            importanceSearch.Font = new Font("Arial", 10F, FontStyle.Bold);
            importanceSearch.ForeColor = Color.SaddleBrown;
            importanceSearch.Location = new Point(113, 26);
            importanceSearch.Name = "importanceSearch";
            importanceSearch.Size = new Size(125, 38);
            importanceSearch.TabIndex = 7;
            importanceSearch.Text = "⚠חשיבות שולח";
            importanceSearch.UseVisualStyleBackColor = true;
            importanceSearch.Click += OrderByImportance;

            // 
            // favoriteSearch - חיפוש לפי הודעות מועדפות
            // 
            favoriteSearch = new Button();
            favoriteSearch.Font = new Font("Arial", 20F, FontStyle.Bold);
            favoriteSearch.ForeColor = Color.SaddleBrown;
            favoriteSearch.Location = new Point(64, 17);
            favoriteSearch.Name = "favoriteSearch";
            favoriteSearch.Size = new Size(43, 51);
            favoriteSearch.TabIndex = 2;
            favoriteSearch.Text = "♡";
            favoriteSearch.UseVisualStyleBackColor = true;
            favoriteSearch.Click += OrderByFavor;
            // 
            // searchLable - תווית 
            //  
            searchLable = new Label();
            searchLable.AutoSize = true;
            searchLable.Font = new Font("Arial", 11F, FontStyle.Bold);
            searchLable.ForeColor = Color.SaddleBrown;
            searchLable.Location = new Point(420, 26);
            searchLable.Name = "searchLable";
            searchLable.Size = new Size(99, 26);
            searchLable.TabIndex = 1;
            searchLable.Text = "חיפוש לפי";

            showMessages = new TabPage();
            showMessages.BackColor = Color.LemonChiffon;
            showMessages.Location = new Point(4, 29);
            showMessages.Name = "showMessages";
            showMessages.Padding = new Padding(3);
            showMessages.Size = new Size(525, 536);
            showMessages.TabIndex = 1;
            showMessages.Text = "ההודעות שלי";
            showMessages.AutoScroll = true;
            // הוספה של כל שאר הרכיבים 
            showMessages.Controls.Add(dateSearch);
            showMessages.Controls.Add(importanceSearch);
            showMessages.Controls.Add(favoriteSearch);
            showMessages.Controls.Add(searchLable);


            // ------------------------------------- התחלת הצגת ההודעות

            if (this.user.Messages.Count > 0)
            {
                MessageBox.Show(" יש לך " + this.user.Messages.Count + " הודעות חדשות ");
                // 
                // showMessages
                // 
                int baseY = 70;
                for (int i = 0; i < this.user.Messages.Count; i++)
                {   // 
                    // meaaagePanel - הפאנל שמכיל את כל היופי הזה
                    // 
                    Panel messagePanel = new Panel();
                    messagePanel.BackColor = Color.PaleGoldenrod;
                    messagePanel.Location = new Point(19, baseY + i * 238);
                    messagePanel.Name = "messagePanel" + i;
                    messagePanel.Size = new Size(475, 228);
                    messagePanel.TabIndex = 0;
                    // 
                    // replyButton - כפתור שליחת הודעה בחזרה
                    // 
                    Button replyButton = new Button();
                    replyButton.Font = new Font("Arial", 10F, FontStyle.Bold);
                    replyButton.ForeColor = Color.SaddleBrown;
                    replyButton.Location = new Point(308, 162);
                    replyButton.Name = "replyMessage" + i; // שמירת ההודעה
                    replyButton.Size = new Size(87, 32);
                    replyButton.TabIndex = 13;
                    replyButton.Text = "השב";
                    replyButton.UseVisualStyleBackColor = true;
                    replyButton.Click += ReplyMessage;
                    // 
                    // favorMessages - כפתור לשינוי הודעה מועדפת
                    // 
                    Button favorMessages = new Button();
                    favorMessages.Font = new Font("Arial", 18F, FontStyle.Bold);
                    favorMessages.ForeColor = Color.SaddleBrown;
                    favorMessages.Location = new Point(410, 155);
                    favorMessages.Name = "favorMessages" + i;
                    favorMessages.Size = new Size(41, 43);
                    favorMessages.TabIndex = 12;
                    if (this.user.Messages[i].Favor)
                        favorMessages.Text = "❤";
                    else
                        favorMessages.Text = "♡";
                    favorMessages.UseVisualStyleBackColor = true;
                    favorMessages.Click += MakeItFavor;
                    // 
                    // favorMessages - כפתור לשינוי הודעה מועדפת
                    // 
                    Button removeMessage = new Button();
                    removeMessage.Font = new Font("Arial", 14F, FontStyle.Bold);
                    removeMessage.ForeColor = Color.SaddleBrown;
                    removeMessage.Location = new Point(250, 155);
                    removeMessage.Name = "removeMessage" + i;
                    removeMessage.Size = new Size(41, 43);
                    removeMessage.TabIndex = 12;
                    removeMessage.Text = "✖";
                    removeMessage.Click += RemoveMessage;
                    // 
                    // sendingDate - תווית תאריך
                    // 
                    Label sendingDate = new Label();
                    sendingDate.AutoSize = true;
                    sendingDate.Font = new Font("Arial", 10F, FontStyle.Bold);
                    sendingDate.ForeColor = Color.SaddleBrown;
                    sendingDate.Location = new Point(20, 162);
                    sendingDate.Name = "sendingDate" + i;
                    sendingDate.Size = new Size(107, 19);
                    sendingDate.TabIndex = 11;
                    sendingDate.Text = this.user.Messages[i].SendDate.ToString();
                    // 
                    // messageContent - תוכן ההודעה
                    // 
                    RichTextBox messageContent = new RichTextBox();
                    messageContent.ScrollBars = RichTextBoxScrollBars.Both;
                    messageContent.Location = new Point(20, 46);
                    messageContent.Name = "messageContent" + i;
                    messageContent.Size = new Size(431, 103);
                    messageContent.TabIndex = 13;
                    messageContent.Font = new Font("Arial", 8F, FontStyle.Bold);
                    messageContent.ForeColor = yellowPaletteDay["textColor"];
                    messageContent.RightToLeft = RightToLeft.Yes;
                    messageContent.Text = this.user.Messages[i].myMessage;
                    messageContent.ReadOnly = true;
                    // 
                    // senderDetails - פרטי השולח
                    // 
                    Label senderDetails = new Label();
                    senderDetails.AutoSize = true;
                    senderDetails.Font = new Font("Arial", 10F, FontStyle.Bold);
                    senderDetails.ForeColor = Color.SaddleBrown;
                    senderDetails.Location = new Point(335, 11);
                    senderDetails.Name = "senderDetails" + i;
                    senderDetails.Size = new Size(116, 19);
                    senderDetails.TabIndex = 10;
                    senderDetails.RightToLeft = RightToLeft.Yes;
                    senderDetails.Text = this.user.Messages[i].SenderInfro[1] + " " + this.user.Messages[i].SenderInfro[2] + " " + this.user.Messages[i].SenderInfro[3];

                    //------------- הוספה --------------- 
                    messagePanel.Controls.Add(replyButton);
                    messagePanel.Controls.Add(favorMessages);
                    messagePanel.Controls.Add(sendingDate);
                    messagePanel.Controls.Add(messageContent);
                    messagePanel.Controls.Add(senderDetails);
                    messagePanel.Controls.Add(removeMessage);

                    showMessages.Controls.Add(messagePanel); // כל פאנל שמכיל הודעה מתווסף לפאנל הכללי 
                    messagesPanelList.Add(messagePanel);// הוספה לרשימה של הפאנלים חשוב !!!!!!!!!

                }
                messageTabControl.Controls.Add(showMessages);

            }
            else
            {
                // כי אם המשתמש לא קיבל הודעות אין סיבה שהפתורים או האפשרויות האלה יהיו נישות לו
                dateSearch.Enabled = false;
                importanceSearch.Enabled = false;
                favoriteSearch.Enabled = false;
                searchLable.Enabled = false;
            }
            // 
            // sendButton - שייך לשליחת הודעות 
            // 
            sendButton = new Button();
            sendButton.Font = new Font("Arial", 11F, FontStyle.Bold);
            sendButton.ForeColor = Color.SaddleBrown;
            sendButton.Location = new Point(46, 404);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(95, 33);
            sendButton.TabIndex = 2;
            sendButton.Text = "שלח";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButn;

            // 
            // messageDestination - שייך לשליחת הודעות
            // 
            destinationName = new TextBox();
            destinationName.Location = new Point(204, 32);
            destinationName.Name = "destinationName";
            destinationName.Size = new Size(272, 36);
            destinationName.TabIndex = 1;
            destinationName.Font = new Font("Arial", 10F, FontStyle.Bold);
            destinationName.ForeColor = Color.SaddleBrown;
            destinationName.PlaceholderText = "הזן את שם מלא";
            destinationName.BackColor = yellowPaletteDay["baseColor"];
            destinationName.RightToLeft = RightToLeft.Yes;
            destinationName.Leave += TextBoxCheck;
            // 
            // writingMessage - שייך לשליחת הודעות
            // 
            writingMessage = new RichTextBox();
            writingMessage.Location = new Point(46, 74);
            writingMessage.Name = "writingMessage";
            writingMessage.Size = new Size(430, 300);
            writingMessage.TabIndex = 0;
            writingMessage.BackColor = yellowPaletteDay["baseColor"];
            writingMessage.Font = new Font("Arial", 12F, FontStyle.Bold);
            writingMessage.ForeColor = Color.SaddleBrown;
            writingMessage.RightToLeft = RightToLeft.Yes;
            writingMessage.Leave += TextBoxCheck;
            // 
            // sendMessages
            // 
            sendMessages = new TabPage();

            sendMessages.BackColor = Color.LemonChiffon;
            sendMessages.Controls.Add(sendButton);
            sendMessages.Controls.Add(destinationName);
            sendMessages.Controls.Add(writingMessage);
            sendMessages.Font = new Font("Arial", 15F, FontStyle.Bold);
            sendMessages.Location = new Point(4, 29);
            sendMessages.Name = "sendMessages";
            sendMessages.Padding = new Padding(3);
            sendMessages.Size = new Size(525, 536);
            sendMessages.TabIndex = 0;
            sendMessages.Text = "שלח הודעה";
            // 
            // messageTabControl
            // 
            messageTabControl.Controls.Add(sendMessages);
            messageTabControl.Location = new Point(46, 138);
            messageTabControl.Name = "messageTabControl";
            messageTabControl.SelectedIndex = 0;
            messageTabControl.Size = new Size(533, 569);
            messageTabControl.TabIndex = 6;



        }

        //------------------------------------------------- createMessagePage פעולות עזר עבור הפונקציה
        private void OrderByEarliestDateOrOldestDate(object sender, EventArgs e)
        {
            favoriteSearch.BackColor = Color.White;
            favoriteSearch.Text = "♡";
            importanceSearch.BackColor = Color.White;
            //----------------------------------------------------------------
            List<int> messagesOrder = new List<int>();

            for (int i = 0; i < messagesPanelList.Count; i++)
                messagesOrder.Add(i);

            // ------------------------------------------------------------------- EarliestDate
            if (dateSearch.SelectedIndex != -1)
            {

                if (dateSearch.SelectedIndex == 0)
                {
                    for (int i = 0; i < messagesPanelList.Count; i++)
                        showMessages.Controls.Remove(showMessages.Controls["messagePanel" + i]);



                    int temp;
                    for (int i = 0; i < messagesPanelList.Count - 1; i++)
                        for (int j = 0; j < messagesPanelList.Count - 1 - i; j++)
                            if (this.user.Messages[j].SendDate > this.user.Messages[j + 1].SendDate)
                            {
                                temp = messagesOrder[j];
                                messagesOrder[j] = messagesOrder[j + 1];
                                messagesOrder[j + 1] = temp;
                            }

                    int baseY = 70;
                    for (int i = 0; i < messagesOrder.Count; i++)
                    {
                        Control msgPanel = messagesPanelList[messagesOrder[i]];
                        msgPanel.Location = new Point(19, baseY + i * 238);
                        msgPanel.Name = "messagePanel" + messagesOrder[i];
                        showMessages.Controls.Add(msgPanel);
                    }

                }
                // ------------------------------------------------------------------- OldestDate
                else
                if (dateSearch.SelectedIndex == 1)
                {
                    for (int i = 0; i < messagesPanelList.Count; i++)
                        showMessages.Controls.Remove(showMessages.Controls["messagePanel" + i]);


                    int temp;
                    for (int i = 0; i < messagesPanelList.Count - 1; i++)
                        for (int j = 0; j < messagesPanelList.Count - 1 - i; j++)
                            if (this.user.Messages[j].SendDate < this.user.Messages[j + 1].SendDate)
                            {
                                temp = messagesOrder[j];
                                messagesOrder[j] = messagesOrder[j + 1];
                                messagesOrder[j + 1] = temp;
                            }


                    int baseY = 70;
                    for (int i = 0; i < messagesOrder.Count; i++)
                    {
                        Control msgPanel = messagesPanelList[messagesOrder[i]];
                        msgPanel.Location = new Point(19, baseY + i * 238);
                        msgPanel.Name = "messagePanel" + messagesOrder[i];
                        showMessages.Controls.Add(msgPanel);
                    }

                }
            }
        }
        private void OrderByFavor(object sender, EventArgs e)
        {
            dateSearch.SelectedIndex = -1;
            favoriteSearch.BackColor = Color.FromArgb(255, 204, 204);
            favoriteSearch.Text = "❤";
            importanceSearch.BackColor = Color.White;

            //----------------------------------------------------
            for (int i = 0; i < messagesPanelList.Count; i++)
                showMessages.Controls.Remove(showMessages.Controls["messagePanel" + i]);

            /* עבור כל פאנל שמכיל פרטים ופעולות על הודעה מסוימת יש מספר
             * אז כאן אני לוקחת את כל הפאנלים כמספרים מ0 עד כמות ההודעות
             * ואז אני ממיינת לפי הרשימה שהכנתי כתכונה שמכילה את כל הפאנלים עצמם
             * פאנלים שמכילים הודעה שנשמרה באהובים רופצת לתחילת הרשימה והאחרים נדחפים מאחור
             * ואזזזז אני עוברת ומשנה את המיקום שלהם 
             */
            List<int> messagesOrder = new List<int>();

            for (int i = 0; i < messagesPanelList.Count; i++)
                messagesOrder.Add(i);


            int temp;
            for (int i = 0; i < messagesPanelList.Count; i++)
                if (this.user.Messages[i].Favor)
                {
                    temp = i;
                    messagesOrder.Remove(temp);
                    messagesOrder.Insert(0, temp);

                }
            /* נניח שיש לי 60 הודעות, 15 מתוכן באהובים ואז ה15 ההן נמצאות בתחילת הרשימה
             * ואז פשוט עוברים על הרשימה מההתחלה עד הסוף ומציגים את זה
             */
            int baseY = 70;
            for (int i = 0; i < messagesOrder.Count; i++)
            {
                Control msgPanel = messagesPanelList[messagesOrder[i]];
                msgPanel.Location = new Point(19, baseY + i * 238);
                msgPanel.Name = "messagePanel" + messagesOrder[i];
                showMessages.Controls.Add(msgPanel);
            }

        }
        private void OrderByImportance(object sender, EventArgs e)
        {
            dateSearch.SelectedIndex = -1;
            favoriteSearch.BackColor = Color.White;
            favoriteSearch.Text = "♡";

            importanceSearch.BackColor = Color.FromArgb(255, 179, 71);
            //-------------------------------------------------------------------------
            List<int> messagesOrder = new List<int>();

            for (int i = 0; i < messagesPanelList.Count; i++)
                messagesOrder.Add(i);
            //--------------------------------------------------------------------------
            int temp;
            for (int i = 0; i < messagesPanelList.Count; i++)
                if (this.user.Messages[i].SenderInfro[1] == "Student")
                {
                    temp = i;
                    messagesOrder.Remove(temp);
                    messagesOrder.Insert(0, temp);

                }

            for (int i = 0; i < messagesPanelList.Count; i++)
                if (this.user.Messages[i].SenderInfro[1] == "StudentTA")
                {
                    temp = i;
                    messagesOrder.Remove(temp);
                    messagesOrder.Insert(0, temp);

                }

            for (int i = 0; i < messagesPanelList.Count; i++)
                if (this.user.Messages[i].SenderInfro[1] == "Lecturer")
                {
                    temp = i;
                    messagesOrder.Remove(temp);
                    messagesOrder.Insert(0, temp);

                }

            for (int i = 0; i < messagesPanelList.Count; i++)
                if (this.user.Messages[i].SenderInfro[1] == "HeadOfDepartment")
                {
                    temp = i;
                    messagesOrder.Remove(temp);
                    messagesOrder.Insert(0, temp);

                }

            int baseY = 70;
            for (int i = 0; i < messagesOrder.Count; i++)
            {
                Control msgPanel = messagesPanelList[messagesOrder[i]];
                msgPanel.Location = new Point(19, baseY + i * 238);
                msgPanel.Name = "messagePanel" + messagesOrder[i];
                showMessages.Controls.Add(msgPanel);
            }
        }
        private void RemoveMessage(object sender, EventArgs e)
        {
            Button removeMessage = sender as Button;
            int messageNumber = int.Parse(removeMessage.Name.Substring("removeMessage".Length));// שומר את המספר של הכפתור שהמשתמש רצה להשיב להודעה שלו


            for (int i = 0; i < messagesPanelList.Count; i++)
            {
                showMessages.Controls.Remove(showMessages.Controls["messagePanel" + i]); // להוריד את כל ההודעות 
            }
            messagesPanelList.Clear();
            this.user.Messages.RemoveAt(messageNumber);


            if (this.user.Messages.Count > 0)
            {
                int baseY = 70;
                for (int i = 0; i < this.user.Messages.Count; i++)
                {   // 
                    // meaaagePanel - הפאנל שמכיל את כל היופי הזה
                    // 
                    Panel messagePanel = new Panel();
                    messagePanel.BackColor = Color.PaleGoldenrod;
                    messagePanel.Location = new Point(19, baseY + i * 238);
                    messagePanel.Name = "messagePanel" + i;
                    messagePanel.Size = new Size(475, 228);
                    messagePanel.TabIndex = 0;
                    // 
                    // replyButton - כפתור שליחת הודעה בחזרה
                    // 
                    Button replyButton = new Button();
                    replyButton.Font = new Font("Arial", 10F, FontStyle.Bold);
                    replyButton.ForeColor = Color.SaddleBrown;
                    replyButton.Location = new Point(308, 162);
                    replyButton.Name = "replyMessage" + i; // שמירת ההודעה
                    replyButton.Size = new Size(87, 32);
                    replyButton.TabIndex = 13;
                    replyButton.Text = "השב";
                    replyButton.UseVisualStyleBackColor = true;
                    replyButton.Click += ReplyMessage;
                    // 
                    // favorMessages - כפתור לשינוי הודעה מועדפת
                    // 
                    Button favorMessages = new Button();
                    favorMessages.Font = new Font("Arial", 18F, FontStyle.Bold);
                    favorMessages.ForeColor = Color.SaddleBrown;
                    favorMessages.Location = new Point(410, 155);
                    favorMessages.Name = "favorMessages" + i;
                    favorMessages.Size = new Size(41, 43);
                    favorMessages.TabIndex = 12;
                    if (this.user.Messages[i].Favor)
                        favorMessages.Text = "❤";
                    else
                        favorMessages.Text = "♡";
                    favorMessages.UseVisualStyleBackColor = true;
                    favorMessages.Click += MakeItFavor;
                    // 
                    // favorMessages - כפתור לשינוי הודעה מועדפת
                    // 
                    removeMessage = new Button();
                    removeMessage.Font = new Font("Arial", 14F, FontStyle.Bold);
                    removeMessage.ForeColor = Color.SaddleBrown;
                    removeMessage.Location = new Point(250, 155);
                    removeMessage.Name = "removeMessage" + i;
                    removeMessage.Size = new Size(41, 43);
                    removeMessage.TabIndex = 12;
                    removeMessage.Text = "✖";
                    removeMessage.Click += RemoveMessage;
                    // 
                    // sendingDate - תווית תאריך
                    // 
                    Label sendingDate = new Label();
                    sendingDate.AutoSize = true;
                    sendingDate.Font = new Font("Arial", 10F, FontStyle.Bold);
                    sendingDate.ForeColor = Color.SaddleBrown;
                    sendingDate.Location = new Point(20, 162);
                    sendingDate.Name = "sendingDate" + i;
                    sendingDate.Size = new Size(107, 19);
                    sendingDate.TabIndex = 11;
                    sendingDate.Text = this.user.Messages[i].SendDate.ToString();
                    // 
                    // messageContent - תוכן ההודעה
                    // 
                    RichTextBox messageContent = new RichTextBox();
                    messageContent.ScrollBars = RichTextBoxScrollBars.Both;
                    messageContent.Location = new Point(20, 46);
                    messageContent.Name = "messageContent" + i;
                    messageContent.Size = new Size(431, 103);
                    messageContent.TabIndex = 13;
                    messageContent.Font = new Font("Arial", 8F, FontStyle.Bold);
                    messageContent.ForeColor = yellowPaletteDay["textColor"];
                    messageContent.RightToLeft = RightToLeft.Yes;
                    messageContent.Text = this.user.Messages[i].myMessage;
                    messageContent.ReadOnly = true;
                    // 
                    // senderDetails - פרטי השולח
                    // 
                    Label senderDetails = new Label();
                    senderDetails.AutoSize = true;
                    senderDetails.Font = new Font("Arial", 10F, FontStyle.Bold);
                    senderDetails.ForeColor = Color.SaddleBrown;
                    senderDetails.Location = new Point(335, 11);
                    senderDetails.Name = "senderDetails" + i;
                    senderDetails.Size = new Size(116, 19);
                    senderDetails.TabIndex = 10;
                    senderDetails.RightToLeft = RightToLeft.Yes;
                    senderDetails.Text = this.user.Messages[i].SenderInfro[1] + " " + this.user.Messages[i].SenderInfro[2] + " " + this.user.Messages[i].SenderInfro[3];

                    //------------- הוספה --------------- 
                    messagePanel.Controls.Add(replyButton);
                    messagePanel.Controls.Add(favorMessages);
                    messagePanel.Controls.Add(sendingDate);
                    messagePanel.Controls.Add(messageContent);
                    messagePanel.Controls.Add(senderDetails);
                    messagePanel.Controls.Add(removeMessage);

                    showMessages.Controls.Add(messagePanel); // כל פאנל שמכיל הודעה מתווסף לפאנל הכללי 
                    messagesPanelList.Add(messagePanel);// הוספה לרשימה של הפאנלים חשוב !!!!!!!!!

                }

                ChangeMood(null, EventArgs.Empty);
            }

        }
        private void ReplyMessage(object sender, EventArgs e)
        {
            /* אז כשמישהו משיב להודעה אני רוצה שהתוכנית הזאת תעביר אותו לדף של שליחת ההודעה ותכין
               לו כבר את הנתונים המתאימים בחלון כדי להשיב על ההודעה הזאת ולשלוח למי שהוא קיבל ממנו
             */
            Button replyButton = sender as Button;
            int messageNumber = int.Parse(replyButton.Name.Substring("replyMessage".Length));// שומר את המספר של הכפתור שהמשתמש רצה להשיב להודעה שלו

            string destination = this.user.Messages[messageNumber].SenderInfro[2] + " " + this.user.Messages[messageNumber].SenderInfro[3];
            destinationName.Text = destination;
            messageTabControl.SelectedTab = sendMessages; // הולך לחלון של השליחת הודעות        
        }
        private void MakeItFavor(object sender, EventArgs e)
        {
            Button favorMessages = sender as Button;
            int messageNumber = int.Parse(favorMessages.Name.Substring("favorMessages".Length));// שומר את המספר של הכפתור שהמשתמש רצה להשיב להודעה שלו

            if (favorMessages.Text == "♡")
            { favorMessages.Text = "❤"; this.user.Messages[messageNumber].Favor = true; }
            else
            { favorMessages.Text = "♡"; this.user.Messages[messageNumber].Favor = false; }

        }
        private void TextBoxCheck(object sender, EventArgs e)
        {
            if (writingMessage != null && destinationName != null)
                if (!string.IsNullOrWhiteSpace(writingMessage.Text) && !string.IsNullOrWhiteSpace(destinationName.Text))
                    // אם שדה הסיסמה ושם המשתמש לא ריקים אז אני ארצה שהכפתור יהיה זמין 
                    sendButton.Enabled = true;
                else
                    sendButton.Enabled = false;
        }
        private void SendButn(object sender, EventArgs e)
        {


            string message = writingMessage.Text; // ההודעה עצמה
            string desName = destinationName.Text; // שומר את השם והשם משפחה של אותו אדם 


            object saveResults;
            /* כדי שחבילה תגיע למי ששלח אותה   EdanAD22, Lecturer, עידן, עמדי
             * אז צריך את הפרטים של השולח 
             * כלומר אם אני שולחת הודעה לעידן עמדי אז הוא יקבל את הפרטים שלי את היוזר שלי הסוג חשבון וכו וכו כדי שאצלו יופיעו הפרטים שלי
             * ואז הוא בהמשך יוכל גם ללוח לי הודעה בחזרה 
             */


            string[] otherSideInfro = { this.user.UserName, this.user.AccountType, this.user.Name, this.user.FmName };

            // ------------------------------------------------------------------- מחפש סטודנט בשם זה

            saveResults = searchForStudent(desName.Trim());
            if (saveResults is List<Student> student)
            {
                UserMessage.DeliverMessage(@"..\..\..\..\Files\Students\studentTA.txt", message, otherSideInfro, student[0].UserName);
                // ראש מחלקה יכול לשלוח הודעות רק לסטודנטים מתרגלים , למרצים ולראשי מחלקה אחרים - לא לסטודנטים
            }
            // ------------------------------------------------------------------- מחפש מרצה בשם זה
            else
            {
                saveResults = searchForLecturer(searshBox.Text.Trim());
                if (saveResults is Lecturer lecturer)
                {
                    UserMessage.DeliverMessage(@"..\..\..\..\Files\Employees\lecturer.txt", message, otherSideInfro, lecturer.UserName);
                }
                else
                {
                    saveResults = HeadOfDepartment.getByName(desName.Trim());
                    if (saveResults is List<string> hdDetails)
                    {
                        UserMessage.DeliverMessage(@"..\..\..\..\Files\Employees\headOfDepartment.txt", message, otherSideInfro, hdDetails[4]);
                    }
                }

            }

            writingMessage.Text = "";
            destinationName.Text = "";
        }
        //-----------------------------------
        private void createSearchPage()
        {
            searchingPanel = new Panel();
            searchingPanel.AutoScroll = true;
            searchingPanel.BackColor = yellowPaletteDay["otherColor"];
            searchingPanel.Location = new Point(46, 138);
            searchingPanel.Name = "searchingPanel";
            searchingPanel.Size = new Size(533, 569);
            searchingPanel.TabIndex = 4;

            searshBox = new TextBox();
            searshBox.Location = new Point(102, 55);
            searshBox.Name = "searshBox";
            searshBox.Size = new Size(352, 50);
            searshBox.TabIndex = 0;
            searshBox.PlaceholderText = "  הזן שם מרצה, קורס, מחלקה או ראש מחלקה ";
            searshBox.RightToLeft = RightToLeft.Yes;

            searchingPanel.Controls.Add(searshBox);

            searchIcon = new PictureBox();
            searchIcon.Location = new Point(60, 50);
            searchIcon.Name = "searchIcon";
            searchIcon.Size = new Size(35, 35);
            searchIcon.TabIndex = 1;
            searchIcon.TabStop = false;
            searchIcon.Image = dayIcons["search"];
            searchIcon.Click += searchPersonByName;
            searchIcon.SizeMode = PictureBoxSizeMode.StretchImage;

            searchingPanel.Controls.Add(searchIcon);

            userInfro = new RichTextBox();
            userInfro.ScrollBars = RichTextBoxScrollBars.Vertical;
            userInfro.Location = new Point(35, 128);
            userInfro.Name = "userInfro";
            userInfro.Size = new Size(453, 386);
            userInfro.TabIndex = 6;
            userInfro.Font = new Font("Arial", 10F, FontStyle.Bold);
            userInfro.ForeColor = yellowPaletteDay["textColor"];
            userInfro.BackColor = yellowPaletteDay["baseColor"];
            userInfro.RightToLeft = RightToLeft.Yes;


            searchingPanel.Controls.Add(userInfro);


        }
        //------------------------------------------------- createSearchPage פעולות עזר עבור הפונקציה
        private void searchPersonByName(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searshBox.Text)) // אם התיבה לא ריקה הוא יבצע את החיפוש 
            {
                // ------------------------------------------------------------------- מחפש קורס בשם זה
                object saveResults = searchForCourse(searshBox.Text.Trim());
                if (saveResults is List<Course> course)
                {
                    for (int k = 0; k < course.Count; k++)
                    {
                        userInfro.Text += " שם הקורס" + course[k].Name + Environment.NewLine +
                                         " פרטי המרצה" + Environment.NewLine +
                                         " שם" + course[k].Teacher.Name + " " + course[k].Teacher.FmName + Environment.NewLine +
                                         " אימייל" + course[k].Teacher.Gmail + Environment.NewLine + Environment.NewLine +
                                         " להלן הסטודנטים בקורס" + Environment.NewLine;
                        for (int i = 0; i < course[k].Students.Count; i++)
                            userInfro.Text += course[k].Students[i].Name + " " + course[k].Students[i].FmName + Environment.NewLine;

                        userInfro.Text += Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine;
                    }
                }
                else
                {
                    // ------------------------------------------------------------------- מחפש סטודנט בשם זה
                    saveResults = searchForStudent(searshBox.Text.Trim());
                    if (saveResults is List<Student> student)
                    {
                        for (int k = 0; k < student.Count; k++)
                        {
                            userInfro.Text += " שם" + student[k].Name + " " + student[k].FmName + Environment.NewLine +
                             " אימייל" + student[k].Gmail + Environment.NewLine + " מספר טלפון " + student[k].PhoneNum;

                            userInfro.Text += Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine;
                        }

                    }

                    // ------------------------------------------------------------------- מחפש מרצה בשם זה
                    else
                    {
                        saveResults = searchForLecturer(searshBox.Text.Trim());
                        if (saveResults is Lecturer lecturer)
                        {

                            userInfro.Text += " :שם" + lecturer.Name + " " + lecturer.FmName + Environment.NewLine +
                            " מרצה עבור הקורסים הבאים";

                            for (int i = 0; i < lecturer.Courses.Count; i++)
                                userInfro.Text += lecturer.Courses[i] + " , ";

                            userInfro.Text += " אימייל" + lecturer.Gmail + Environment.NewLine + " מספר טלפון " + lecturer.PhoneNum + Environment.NewLine +
                              " סוג התמחות" + lecturer.SpecializationName +
                              " דירוג סטודנטים";

                            for (int i = 0, j = lecturer.Rating - 1; i < 5; i++)
                                if (i <= j) userInfro.Text += "★";
                                else userInfro.Text += "☆";

                            userInfro.Text += Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine;

                        }
                        else
                        {
                            saveResults = HeadOfDepartment.getByClassName(searshBox.Text);
                            if (saveResults is List<string> hdDetails)
                            {
                                userInfro.Text += " שם" + hdDetails[0] + " " + hdDetails[1] + Environment.NewLine +
                                " אימייל" + hdDetails[2] + " ראש מחלקת " + hdDetails[3];
                                userInfro.Text += Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine;
                            }
                            else
                            {
                                userInfro.Text = " לא נמצא מרצה/ קורס/ סטודנט שעונה על השם" + Environment.NewLine
                                    + searshBox.Text
                                    + Environment.NewLine + "עמך הסליחה";
                            }

                        }

                    }

                }
            }
            else
            {
                userInfro.Text = "";
                searshBox.Text = "";
            }
        }
        private object searchForCourse(string name)
        {
            List<Course> allResults = new List<Course>();
            for (int i = 0; i < this.user.Courses.Count; i++)
                if (this.user.Courses[i].Name == name)
                {
                    allResults.Add(this.user.Courses[i]);
                    allResults = allResults.Distinct().ToList(); // מסיר כפילויות
                }


            if (allResults.Count > 0)
                return allResults;
            return null;
        }
        private object searchForStudent(string name)
        {
            // חיפוש סטודנט מין הסתם מחפש גם סטודנט מתרגל 
            List<Student> allResults = new List<Student>();

            for (int i = 0; i < this.user.Courses.Count; i++)
                for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                    if (this.user.Courses[i].Students[j].Name + " " + this.user.Courses[i].Students[j].FmName == name)
                    {
                        allResults.Add(this.user.Courses[i].Students[j]);
                        allResults = allResults.Distinct().ToList();
                    }

            if (allResults.Count == 0)
                return null;

            //מסיר כפילויות
            string currentId;
            for (int i = 0; i < allResults.Count; i++)
            {
                currentId = allResults[i].Id;
                for (int j = i + 1; j < allResults.Count; j++)
                {
                    if (allResults[j].Id == currentId)
                    {
                        allResults.RemoveAt(j);
                        // ואז בזמן אמת כל האיברים זזים אינדקס אחד שמאלה אז צריך שהמצביע של הלולאה לא ישתנה הפעם
                        j--;
                    }
                }
            }
            return allResults;
        }
        private object searchForLecturer(string name)
        {
            string fullName;
            for (int i = 0; i < this.allLecturers.Count; i++)
            {
                fullName = this.allLecturers[i].Name + " " + this.allLecturers[i].FmName;
                if (fullName == name)
                    return this.allLecturers[i];
            }
            return null;
        }


        //-----------------------------------
        private void createCourseTaughtPage()
        {
            // courseTaughtPanel.AutoScroll = true; // אופציונלי אבל בדרכ מרצה מלמד 3 4 קורסים אפילו פחות אז לא צריך 
            courseTaughtPanel.BackColor = yellowPaletteDay["otherColor"];
            courseTaughtPanel.Location = new Point(46, 138);
            courseTaughtPanel.Name = "courseTaughtPanel";
            courseTaughtPanel.Size = new Size(533, 569);
            courseTaughtPanel.TabIndex = 4;

            coursesNamesPanel = new Panel();
            coursesNamesPanel.AutoScroll = true;
            coursesNamesPanel.BackColor = yellowPaletteDay["baseColor"];
            coursesNamesPanel.Location = new Point(240, 10);
            coursesNamesPanel.Name = "coursesNamesPanel";
            coursesNamesPanel.Size = new Size(280, 549);
            coursesNamesPanel.TabIndex = 4;


            studentsNamesPanel = new Panel();
            studentsNamesPanel.AutoScroll = true;
            studentsNamesPanel.BackColor = yellowPaletteDay["baseColor"];
            studentsNamesPanel.Location = new Point(10, 10);
            studentsNamesPanel.Name = "studentsNamesPanel";
            studentsNamesPanel.Size = new Size(220, 549);
            studentsNamesPanel.TabIndex = 4;


            int baseYcourses = 10; int baseYstudents = 10; string fullName;
            for (int i = 0; i < this.user.Courses.Count; i++)
            {

                coursesTaught = new RichTextBox();
                coursesTaught.Location = new Point(10, baseYcourses);
                coursesTaught.Size = new Size(260, 40);
                coursesTaught.Font = new Font("Arial", 12F, FontStyle.Bold);
                coursesTaught.ForeColor = yellowPaletteDay["titleColor"];
                coursesTaught.BackColor = yellowPaletteDay["buttonColor"];
                coursesTaught.BorderStyle = BorderStyle.None;
                coursesTaught.ReadOnly = true;
                coursesTaught.RightToLeft = RightToLeft.Yes;
                coursesTaught.Name = "coursesTaught" + i;
                coursesTaught.Text = this.user.Courses[i].Name;
                coursesTaught.Click += ShowMyStudents;

                baseYcourses += 50;

                coursesNamesPanel.Controls.Add(coursesTaught);

                baseYstudents = 10;
                for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                {

                    studentsTaught = new RichTextBox();
                    studentsTaught.Location = new Point(10, baseYstudents);
                    studentsTaught.Size = new Size(180, 40);
                    studentsTaught.Font = new Font("Arial", 10F, FontStyle.Bold);
                    studentsTaught.ForeColor = yellowPaletteDay["titleColor"];
                    studentsTaught.BackColor = yellowPaletteDay["baseColor"];
                    studentsTaught.BorderStyle = BorderStyle.None;
                    studentsTaught.ReadOnly = true;
                    studentsTaught.RightToLeft = RightToLeft.Yes;
                    studentsTaught.Name = "courses" + i + "studentsTaught" + j;
                    fullName = this.user.Courses[i].Students[j].Name + " " + this.user.Courses[i].Students[j].FmName;
                    studentsTaught.Text = fullName;
                    studentsTaught.Visible = false;
                    baseYstudents += 50;

                    studentsNamesPanel.Controls.Add(studentsTaught);
                }

            }
            courseTaughtPanel.Controls.Add(coursesNamesPanel);

            courseTaughtPanel.Controls.Add(studentsNamesPanel);

        }
        //------------------------------------------------- createCourseTaughtPage פעולות עזר עבור הפונקציה
        private void ShowMyStudents(object sender, EventArgs e)
        {
            foreach (Control c in studentsNamesPanel.Controls)
                c.Visible = false; // להסתיר את כל הקונטרולרים הקיימים כדי לפנות אפשרות לתצוגה נוחה של החדשים

            RichTextBox coursesTaught = sender as RichTextBox;
            int courseNumber = int.Parse(coursesTaught.Name.Substring("coursesTaught".Length));// שומר את מספר הקורס שהמשתמש רוצה לראות את התלמידים שלומדים אותו 
            Control control;

            for (int i = 0; i < this.user.Courses[courseNumber].Students.Count; i++)
            {
                control = studentsNamesPanel.Controls["courses" + courseNumber + "studentsTaught" + i];
                if (control != null && control is RichTextBox)
                    control.Visible = true;

            }

        }
        //-----------------------------------
        private void createNewRequestsPage()
        {
            newRequestsPanel.AutoScroll = true; // הגיוני אם יש הרבה בקשות חדשות
            newRequestsPanel.BackColor = yellowPaletteDay["otherColor"];
            newRequestsPanel.Location = new Point(46, 138);
            newRequestsPanel.Name = "newRequestsPanel";
            newRequestsPanel.Size = new Size(533, 569);
            newRequestsPanel.TabIndex = 4;


        }
        //------------------------------------------------- createNewRequestsPage פעולות עזר עבור הפונקציה
        //-----------------------------------
        private void createMyClassPage()
        {
            myClassPanel.AutoScroll = true; // הגיוני אם במחלקה יש הרבה מסלולים ובדרכ זה מה שקורה
            myClassPanel.BackColor = yellowPaletteDay["otherColor"];
            myClassPanel.Location = new Point(46, 138);
            myClassPanel.Name = "myClassPanel";
            myClassPanel.Size = new Size(533, 569);
            myClassPanel.TabIndex = 4;
            //------------------------------------------------------ כפתור הוספת מסלול חדש
            addSpecializationBtn = new Button();
            addSpecializationBtn.Location = new Point(10, 10);
            addSpecializationBtn.Name = "addSpecializationBtn";
            addSpecializationBtn.Size = new Size(55, 55);
            addSpecializationBtn.TabIndex = 4;
            addSpecializationBtn.Image = dayIcons["add"];
            addSpecializationBtn.Click += addSpecializationWindow;

            myClassPanel.Controls.Add(addSpecializationBtn);
            //------------------------------------------------------ כפתור להצגת מסלול חובה
            requiredRouteBtn = new Button();
            requiredRouteBtn.Font = new Font("Arial", 15F, FontStyle.Bold);
            requiredRouteBtn.ForeColor = Color.SaddleBrown;
            requiredRouteBtn.Location = new Point(70, 10);
            requiredRouteBtn.Name = "requiredRouteBtn";
            requiredRouteBtn.Size = new Size(420, 55);
            requiredRouteBtn.TabIndex = 4;
            requiredRouteBtn.Text = " מסלול חובה";
            requiredRouteBtn.Click += ShowCoreProgram;  // כפתור שפותח את הדף של המסלול חובה שמציג את הקורסים שלו

            myClassPanel.Controls.Add(requiredRouteBtn);
            // פעולה שהמטרה שלה ליצור מחדש את הכרטיסיות שעליהן כתובות המסלולי התמחות שיש למחחלקה
            createSpecializationTitles(); // הוספת הכותרות - שמות מסלולי התמחות
            // פעולה שהמטרה שלה זה ליצור את החלון שאליו המשתמש יגיע כשהוא ירצה לראות את רשימת הקורסים של אותה ההתמחות 
            createSpecializationShowPage(); // יצירת העמוד

            createCoursesTitles();// הוספת הכותרות - שמות הקורסים

            // ------------------ בלי קשר צריך ליצור את העמוד של הקורסי ליבה ולהוסיף את הכותרות של השמות של הקורסים
            createCoreProgramShowPage(); // יצירת העמוד

            createCoreProgramCoursesTitles();

        }

        private void addCourseWindow(object? sender, EventArgs e)
        {
            Controls.Remove(specializationShowPanel);
            Controls.Remove(coreProgramShowPanel);

            createCoursePanel = new Panel();
            createCoursePanel.AutoScroll = true;
            createCoursePanel.BackColor = yellowPaletteDay["baseColor"];
            createCoursePanel.Location = new Point(46, 138);
            createCoursePanel.Name = "createCoursePanel";
            createCoursePanel.Size = new Size(533, 569);
            createCoursePanel.TabIndex = 4;

            Label enterCourseName = new Label();
            enterCourseName.Font = new Font("Arial", 12, FontStyle.Bold);
            enterCourseName.ForeColor = Color.SaddleBrown;
            enterCourseName.Text = "הזן את שם הקורס";
            enterCourseName.Location = new Point(320, 30);
            enterCourseName.Size = new Size(180, 30);
            enterCourseName.TextAlign = ContentAlignment.MiddleRight;

            createCoursePanel.Controls.Add(enterCourseName);

            enterAnswer = new TextBox(); //< ----- אני משתמשת בזה בשביל השדות של השמות קורסים או התמחויות כי לא נראה לי שזה מצדיק תכונה שונה בבנאי בשביל כל אחד
            enterAnswer.Name = "finish";
            enterAnswer.Location = new Point(90, 30);
            enterAnswer.Size = new Size(230, 50);
            enterAnswer.TextAlign = HorizontalAlignment.Right;
            enterAnswer.PlaceholderText = "שם קורס";

            createCoursePanel.Controls.Add(enterAnswer);

            Button addBtn = sender as Button; // הכפתור שממנו הגענו מצוייד בתג שמסמל מאיפה הגענו כדי שנוכל לדעת לאן לחזור
            List<object> targList = (List<object>)addBtn.Tag;
           
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 27F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(30, 15);
            goBack.Name = "goBack";
            goBack.Size = new Size(50, 50);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Tag = targList[0];
            goBack.Click += ShowPreviousPage;
           
            createCoursePanel.Controls.Add(goBack);
            // 
            // createCourse   -------------------------------> ממש חשוב !!
            // 
            /* אז הכפתור הזה אמור להכיל את השם של המקום ממנו הוא בא אם זה מספר מסלול התמחות או מסלול ליבה 
             * אחר כך אני צריכה לדעת אם זה מסלול התמחות יש לי כמה מסלולי התמחות ותמיד תמיד מסלול ליבה אחד
             * אז אני צריכה לדעת גם מה המספר מסלול התמחות במצב כזה יהיה לי עוד פרמטר של מספר רק במקרה של מסלול התמחות
             * ואז אני אצור אובייקט של מרצה ואכניס גם אותו לתג ואז בפעולה שקורת כשלוחצים על הכפתור ישאר
             * רק להכניס אותו למקום המתאים
             */
            createCourse = new Button();
            createCourse.Name = "createCourse";
            createCourse.Size = new Size(190, 53);
            createCourse.Location = new Point(160, 520);
            createCourse.Font = new Font("Arial", 10F, FontStyle.Bold);
            createCourse.Text = "צור קורס";
            createCourse.ForeColor = Color.SaddleBrown;
            createCourse.BackColor = yellowPaletteDay["buttonColor"];
            createCourse.Enabled = false;
            createCourse.Tag = addBtn.Tag;
            createCourse.Click += AddCourse;
            createCourse.Click += ShowPreviousPage;

            createCoursePanel.Controls.Add(createCourse);
            // 
            // error
            // 
            error = new Label();
            error.AutoSize = true;
            error.ForeColor = Color.Red;
            error.Location = new Point(90, 580);
            error.Name = "error";
            error.Size = new Size(0, 20);
            error.TabIndex = 16;
            error.TextAlign = ContentAlignment.MiddleCenter;

            createCoursePanel.Controls.Add(error);

            Label enterLecturerDetails = new Label();
            enterLecturerDetails.Font = new Font("Arial", 12, FontStyle.Bold);
            enterLecturerDetails.ForeColor = Color.SaddleBrown;
            enterLecturerDetails.Text = "יש להזין את הפרטים הבאים עבור המרצה";
            enterLecturerDetails.Size = new Size(410, 20);
            enterLecturerDetails.Location = new Point(100, 85);
            enterLecturerDetails.TextAlign = ContentAlignment.MiddleRight;

            createCoursePanel.Controls.Add(enterLecturerDetails);
            // 
            // nameBox
            // 
            nameBox = new TextBox();
            nameBox.Name = "nameBox";
            nameBox.PlaceholderText = "שם";
            nameBox.RightToLeft = RightToLeft.Yes;
            nameBox.Size = new Size(150, 27);
            nameBox.Location = new Point(355, 130);
            nameBox.TabIndex = 6;
            nameBox.Leave += FieldCheck;
            createCoursePanel.Controls.Add(nameBox);
            // 
            // fmNameBox
            // 
            fmNameBox = new TextBox();
            fmNameBox.Name = "fmNameBox";
            fmNameBox.PlaceholderText = "שם משפחה";
            fmNameBox.RightToLeft = RightToLeft.Yes;
            fmNameBox.Size = new Size(130, 27); 
            fmNameBox.Location = new Point(200, 130);
            fmNameBox.TabIndex = 7;
            fmNameBox.Leave += FieldCheck;
            createCoursePanel.Controls.Add(fmNameBox);
            // 
            // idBox
            // 
            idBox = new TextBox();
            idBox.Location = new Point(325, 190);
            idBox.Name = "idBox";
            idBox.PlaceholderText = "תז";
            idBox.RightToLeft = RightToLeft.Yes;
            idBox.Size = new Size(178, 27);
            idBox.TabIndex = 8;
            idBox.Leave += FieldCheck;
            createCoursePanel.Controls.Add(idBox);
            // 
            // phoneNumBox
            // 
            phoneNumBox = new TextBox();
            phoneNumBox.Location = new Point(70, 190);
            phoneNumBox.Name = "phoneNumBox";
            phoneNumBox.PlaceholderText = "מספר טלפון";
            phoneNumBox.RightToLeft = RightToLeft.Yes;
            phoneNumBox.Size = new Size(232, 27);
            phoneNumBox.TabIndex = 9;
            phoneNumBox.Leave += FieldCheck;
            createCoursePanel.Controls.Add(phoneNumBox);
            // 
            // gmailBox
            // 
            gmailBox = new TextBox();
            gmailBox.Location = new Point(250, 238);
            gmailBox.Name = "gmailBox";
            gmailBox.PlaceholderText = "אימייל";
            gmailBox.RightToLeft = RightToLeft.Yes;
            gmailBox.Size = new Size(252, 27);
            gmailBox.TabIndex = 10;
            gmailBox.Leave += FieldCheck;
            createCoursePanel.Controls.Add(gmailBox);
            // 
            // date
            // 
            date = new DateTimePicker();
            date.Location = new Point(260, 300);
            date.MaxDate = new DateTime(2011, 7, 11, 0, 0, 0, 0);
            date.MinDate = new DateTime(1905, 7, 11, 0, 0, 0, 0);
            date.Name = "date";
            date.Size = new Size(236, 27);
            date.TabIndex = 11;
            date.Value = new DateTime(2011, 7, 11, 0, 0, 0, 0);
            createCoursePanel.Controls.Add(date);
            // 
            // apply
            // 
            apply = new Button();
            apply.Location = new Point(29, 470);
            apply.Name = "apply";
            apply.Size = new Size(150, 30);
            apply.TabIndex = 13;
            apply.Text = "בחר תמונת פרופיל";
            apply.UseVisualStyleBackColor = true;
            apply.Click += Apply_Click; 
            apply.Click += FieldCheck;
            createCoursePanel.Controls.Add(apply);
            // 
            // profilePicture
            // 
            profilePicture = new PictureBox();
            profilePicture.Location = new Point(29, 270);
            profilePicture.Name = "profilePicture";
            profilePicture.Size = new Size(150, 180);
            profilePicture.TabIndex = 12;
            profilePicture.TabStop = false;

            if (File.Exists(@"..\..\..\..\imgs\anonymous.png"))
            {
                profilePicture.Image = Image.FromFile(@"..\..\..\..\imgs\anonymous.png");
                profilePicture.SizeMode = PictureBoxSizeMode.Zoom;
                createCoursePanel.Controls.Add(profilePicture);
            }
            else
            {
                MessageBox.Show("לא נמצאה התמונה בנתיב: " + @"..\..\..\..\imgs\anonymous.png");
            }

            Label enterExistLecturer = new Label();
            enterExistLecturer.Font = new Font("Arial", 12, FontStyle.Bold);
            enterExistLecturer.ForeColor = Color.SaddleBrown;
            enterExistLecturer.Text = "השתמש במרצה קיים";
            enterExistLecturer.Size = new Size(410, 30);
            enterExistLecturer.Location = new Point(100, 340);
            enterExistLecturer.TextAlign = ContentAlignment.MiddleRight;

            createCoursePanel.Controls.Add(enterExistLecturer);

            SelectExistingLecturer = new ComboBox();
            SelectExistingLecturer.Location = new Point(230, 390);
            SelectExistingLecturer.Size = new Size(250, 30);
            SelectExistingLecturer.DropDownStyle = ComboBoxStyle.DropDownList;
            SelectExistingLecturer.SelectedIndexChanged += userSelectedExistingLecturer;
            SelectExistingLecturer.SelectedIndexChanged += FieldCheck;
            SelectExistingLecturer.Items.Add("");
            List<object> tagList = (List<object>)addBtn.Tag;
            int specializationNumber = (int)tagList[1];
            for (int i=0; i< allLecturers.Count; i++)
                if (this.user.SpecializationCourses[specializationNumber].Name == allLecturers[i].SpecializationName
                    || allLecturers[i].Id == this.user.Id )
                    SelectExistingLecturer.Items.Add(allLecturers[i].Name+" "+allLecturers[i].FmName+" - "+ allLecturers[i].Id);

            createCoursePanel.Controls.Add(SelectExistingLecturer);

            Controls.Add(createCoursePanel);
        }

        private void userSelectedExistingLecturer(object? sender, EventArgs e)
        {
            if (this.SelectedLecturer && SelectExistingLecturer.SelectedIndex == 0) 
            { // אם הוא בחר מרצה אבל עכשיו רוצה לשנות ולא לבחור אז צריך לאפשר לו את העריכה על התיבות 
                this.SelectedLecturer = false;

                date.Enabled = true;
                nameBox.Enabled = true;
                fmNameBox.Enabled = true;
                idBox.Enabled = true;
                phoneNumBox.Enabled = true;
                apply.Enabled = true;
                gmailBox.Enabled = true;
            }
            else 
                if (!this.SelectedLecturer && SelectExistingLecturer.SelectedIndex != 0)
                { // אם הוא במצב שהוא כותב על תיבות כלומר לא בחר במרצה והוא משנה את הבחירה ובוחר מרצה אז צריך לסגור את העריכה על התיבות
                    this.SelectedLecturer = true;

                    date.Enabled = false;
                    nameBox.Enabled = false;
                    fmNameBox.Enabled = false;
                    idBox.Enabled = false;
                    phoneNumBox.Enabled = false;
                    apply.Enabled = false;
                    gmailBox.Enabled = false;
                }

            
        }

        private void AddCourse(object? sender, EventArgs e)
        {
            Button createCourseBttn = sender as Button;
            List<object> tagList = createCourseBttn.Tag as List<object>; //המרה

            string addingType = tagList[0] as string; 
           
            if (addingType == "comeFromspecializationShowPanel")
            {
                int specializationNumber = (int)tagList[1];
                if (this.SelectedLecturer) // בחר במרצה ולכן צריך לעדכן את הפרטים של אותו מרצה
                {
                    /*  SelectExistingLecturer.SelectedIndex +1
                     * כי אם אמרנו שכדי לבטל את הבחירה הוא בוחר באופציה הראשונה שהיא 0 אז השמת המרצים מתחילה מהמקום 1 
                     * ואז כדי להגיע לאותו מרצה ולקחת את האובייקט שלו מהרשימה אני צריכה לעשות פחות 1 
                     */
                    int lecturerIndex = SelectExistingLecturer.SelectedIndex - 1; // שומר את המיקום של אותו מרצה ברשימת המרצים של הראש מחלקה
                    Lecturer newL = (Lecturer) Login.CreateUser(@"..\..\..\..\Files\Employees\lecturer.txt", allLecturers[lecturerIndex].UserName, allLecturers[lecturerIndex].Password, "Lecturer");
                    newL.Courses.Add(new Course(enterAnswer.Text));


                    Course.AddCourse(specializationNumber, enterAnswer.Text, this.user.SpecializationName);// תיעוד הקורס החדש
                    this.user.SpecializationCourses[specializationNumber].Courses.Add(new Course(enterAnswer.Text, newL));
                    
                    
                    
                    HeadOfDepartment.WriteLecturerToFile(newL);
                    /* מה שחכם בפונקציה הזאת זה שהיא כותבת לקבצים מרצה חדש רק אם היא מזהה שהמרצה לא נמצא בהם
                     *  אם הוא נמצא היא תעדכן את הפרטים שלו אז היא שימושית לשני הדברים
                     */
                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    {
                        myClassPanel.Controls.Remove(myClassPanel.Controls["specializationTrack" + i]);
                        myClassPanel.Controls.Remove(myClassPanel.Controls["removeSTruck" + i]);
                        myClassPanel.Controls.Remove(myClassPanel.Controls["changeSpecializationName" + i]);
                    }

                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    {
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "removeCourse" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "changeCourseName" + i]);
                    }

                    createSpecializationTitles(); // יצירה מחדש כי מחקנו
                    createCoursesTitles(); // יצירה מחדש כי מחקנו
                }
                else if (!this.SelectedLecturer && !Lecturer.ExistenceOfLecturer(idBox.Text.Trim(), gmailBox.Text.Trim()))
                    // בחר ליצור מרצה חדש והמרצה לא קיים עדיין  
                {
                   Lecturer newL = new Lecturer("Lecturer", nameBox.Text.Trim(), fmNameBox.Text.Trim(),
                   (DateTime.Today.Year - date.Value.Year).ToString(), phoneNumBox.Text.Trim(), gmailBox.Text.Trim(), idBox.Text.Trim(),
                   date.Value, profilePicture.Image, this.user.SpecializationCourses[specializationNumber].Name.Trim());

                    newL.Courses.Add(new Course(enterAnswer.Text));//           !!!!!

                    Course newCourse = new Course(enterAnswer.Text, newL); // יצירת קורס חדש

                    allLecturers.Add(newL); // מתווסף לרשימת המרצים הקיימים
                  
                    this.user.SpecializationCourses[specializationNumber].Courses.Add(newCourse);


                    Course.AddCourse( specializationNumber, enterAnswer.Text, this.user.SpecializationName);

                    HeadOfDepartment.WriteLecturerToFile(newL); // כתיבה לקובץ מרצים

                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    {
                        myClassPanel.Controls.Remove(myClassPanel.Controls["specializationTrack" + i]);
                        myClassPanel.Controls.Remove(myClassPanel.Controls["removeSTruck" + i]);
                        myClassPanel.Controls.Remove(myClassPanel.Controls["changeSpecializationName" + i]);
                    }

                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    {
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "removeCourse" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "changeCourseName" + i]);
                    }

                    createSpecializationTitles(); // יצירה מחדש כי מחקנו
                    createCoursesTitles(); // יצירה מחדש כי מחקנו

                }
                //this.user.SpecializationCourses[specializationNumber].Courses.Add(new Course())
            }
            //else
            //    if (addingType == "comeFromcoreProgramShowPanel")
            //{
            //    newLecturer = tagList[1] as Lecturer;
            //    Lecturer newL = new Lecturer("Lecturer", nameBox.Text.Trim(), fmNameBox.Text.Trim(),
            //    (DateTime.Today.Year - date.Value.Year).ToString(), phoneNumBox.Text.Trim(), gmailBox.Text.Trim(), idBox.Text.Trim(),
            //    date.Value, profilePicture.Image, this.user.SpecializationCourses[specializationNumber].Name.Trim());

            //}


          //  List<object> tagList = (List<object>)addBttn.Tag;
            /* הכפתור שממנו הגענו לפונקציה הזאת מחזיק ברשימה שאנ צריכה כדי להתקדם הלאה
                כרגע מה שיש בכפתור ההוא זה הסוג קורס אם הוא במסלול ליבה או מסלול התמחות 
                ואם הוא במסלול התמחות יהיה בו בתא ה1 גם את המספר מסלול התמחות
                כדי ליצור אוביייקט של מרצה אני גם צריכה את הסוג התמחות שלו שזה השם של המסלול התמחות
                אז אני אשמור אותו  כי אני צריכה אותו   */
            //MessageBox.Show(specializationNumber + "מסלול מספר");
            //MessageBox.Show(tagList.Count + "turl");

            //tagList.Add(new Lecturer("Lecturer", nameBox.Text.Trim(), fmNameBox.Text.Trim(),
            //    (DateTime.Today.Year - date.Value.Year).ToString(), phoneNumBox.Text.Trim(), gmailBox.Text.Trim(), idBox.Text.Trim(),
            //    date.Value, profilePicture.Image, this.user.SpecializationCourses[specializationNumber].Name.Trim()));


        }


        public void RemoveError(string errorMessage, string textBoxName)
        {
            // פעולה שמקבלת את השם של התיבה ואת ההודעה שהיא אמורה להציג ומוחקת את השגיאה ומקטינה את החלון למצב המקורי שלו בהתאמה
            /* כאן אני מחליפה את הודעה של השגיאה והאנטר בכלום כלומר יש חיפוש של ההודעה הספציפית של השגיאה
             * כי היה יכול להיות לי כמה שגיאות ואני רוצה את השגיאה המדוברת הספציפית שהיא תימחק
             */
            error.Text = error.Text.Replace(errorMessage + Environment.NewLine, "");
        }
        public void AddError(string errorMessage, string textBoxName)
        {
            // פעולה שמקבלת את השם של התיבה ואת ההודעת שגיאה שהיא אמורה להציג ומוסיפה את ההודעת שגיאה, היא גם תגדיל את החלון בהתאמה
            fieldStatus[textBoxName] = false; // בגלל שיש שגיאה המצב הנוכחי של אותו שדה יהיה שלילי 
             error.Text += errorMessage + Environment.NewLine; // מוסיפים בתווית את השגיאה ואנטר 
            /*                חשוב!!
             *   Environment.NewLine  --> /n פירושו להוסיף את האנטר ואני לא משתמשת ב
             *   כי בכל מערכת הפעלה זה שונה אז זה יותר יעיל אם אני ארצה שהוא יעבוד על מחשב אחר   
             */
        }
        
        private void FieldCheck(object? sender, EventArgs e)
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
            if ((fieldStatus["gmailBox"] && fieldStatus["fmNameBox"] && fieldStatus["nameBox"]
                && fieldStatus["idBox"] && fieldStatus["phoneNumBox"] && SelectExistingLecturer.SelectedIndex ==0)
                || (SelectExistingLecturer.SelectedIndex != 0))
                createCourse.Enabled = true;
            else
                createCourse.Enabled = false;


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

        private void addSpecializationWindow(object? sender, EventArgs e)
        {
            Controls.Remove(myClassPanel);

            createSpecializationPanel = new Panel();
            createSpecializationPanel.AutoScroll = true;
            createSpecializationPanel.BackColor = yellowPaletteDay["baseColor"];
            createSpecializationPanel.Location = new Point(46, 200);
            createSpecializationPanel.Name = "createSpecializationPanel";
            createSpecializationPanel.Size = new Size(533, 200);
            createSpecializationPanel.TabIndex = 4;

            Label enterSPname = new Label();
            enterSPname.Font = new Font("Arial", 13, FontStyle.Bold);
            enterSPname.ForeColor = Color.SaddleBrown;
            enterSPname.Text = " הזן את שם מסלול ההתמחות החדש במחלקת "+ Environment.NewLine +this.user.SpecializationName;
            enterSPname.Size = new Size(410, 50);
            enterSPname.Location = new Point(100, 10);
            enterSPname.TextAlign = ContentAlignment.MiddleRight;

            createSpecializationPanel.Controls.Add(enterSPname);

            goBack = new Label();
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 25F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(30, 15);
            goBack.Name = "goBack";
            goBack.Size = new Size(50, 50);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Click += ShowPreviousPage;

            createSpecializationPanel.Controls.Add(goBack);

            enterAnswer = new TextBox();
            enterAnswer.Name = "finish";
            enterAnswer.Location = new Point(140, 90); 
            enterAnswer.Size = new Size(250, 50);
            enterAnswer.TextAlign = HorizontalAlignment.Right;

            createSpecializationPanel.Controls.Add(enterAnswer);

            Button finish = new Button();
            finish.Name = "finish";
            finish.Size = new Size(190, 53);
            finish.Location = new Point(180, 130);
            finish.Font = new Font("Arial", 10F, FontStyle.Bold);
            finish.Text = "צור מסלול חדש";
            finish.ForeColor = Color.SaddleBrown;
            finish.BackColor = yellowPaletteDay["buttonColor"];

            finish.Click += ShowPreviousPage;
            finish.Click += AddSpecialization;

            createSpecializationPanel.Controls.Add(finish);

            Controls.Add(createSpecializationPanel);

        }
        private void AddSpecialization(object? sender, EventArgs e)
        {
            if( !Regex.IsMatch(enterAnswer.Text, @"^\d{10}$"))
            {
                Specialization.AddNewSpecialization(this.user.SpecializationCourses.Count, enterAnswer.Text, this.user.SpecializationName);
                this.user.SpecializationCourses.Add(new Specialization(enterAnswer.Text));
                enterAnswer.Text = "";
                createSpecializationTitles();
            }
            else
            {
                MessageBox.Show("הודעה קופצת");
            }
           
        }

        private void ShowCoreProgram(object sender, EventArgs e)
        {
            Controls.Remove(myClassPanel);
            Controls.Add(coreProgramShowPanel);
        }


        private void createSpecializationShowPage()
        {
            //------------------------------------------------------ חלון שמציג את הקורסים שיש במסלול התחות שנבחר
            specializationShowPanel = new Panel();
            specializationShowPanel.AutoScroll = true;
            specializationShowPanel.BackColor = yellowPaletteDay["otherColor"];
            specializationShowPanel.Location = new Point(46, 138);
            specializationShowPanel.Name = "specializationShowPanel";
            specializationShowPanel.Size = new Size(533, 569);
            specializationShowPanel.TabIndex = 4;
            //----------------------------------------------------- כפתור הוספת קורס חדש  
            addCourseBtn = new Button();
            addCourseBtn.Location = new Point(70, 10);
            addCourseBtn.Name = "addCourseToSP";
            addCourseBtn.Font = new Font("Arial", 14F, FontStyle.Bold);
            addCourseBtn.ForeColor = yellowPaletteDay["titleColor"];
            addCourseBtn.Size = new Size(430, 55);
            addCourseBtn.TabIndex = 4;
            addCourseBtn.Text = " הוספת קורס חדש ";
            addCourseBtn.Image = dayIcons["add"];
            addCourseBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
            addCourseBtn.ImageAlign = ContentAlignment.MiddleCenter;
            addCourseBtn.Click += addCourseWindow;
            // כשהמשתמש ילחץ על הכפתור הוא יראה את 
            specializationShowPanel.Controls.Add(addCourseBtn);

            goBack = new Label();
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 25F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(10, 10);
            goBack.Name = "goBack";
            goBack.Size = new Size(50, 50);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Click += ShowPreviousPage;

            specializationShowPanel.Controls.Add(goBack);


            redNotes = new Label();
            redNotes.AutoSize = true;
            redNotes.Font = new Font("Arial", 15F, FontStyle.Bold);
            redNotes.ForeColor = Color.Red;
            redNotes.Location = new Point(70, 230);
            redNotes.Name = "redNotes";
            redNotes.Size = new Size(40, 20);
            redNotes.TabIndex = 4;
            redNotes.Text = "לא קיימים קורסים במסלול הזה עדיין ";
            redNotes.Visible = false; // <--------- זה יחשף רק אם אין קורסים ויעלם כשנחזור עמוד אחד אחורה

            specializationShowPanel.Controls.Add(redNotes);

            createCoursesTitles();


            createCoreProgramShowPage();
        }

        private void createCoreProgramShowPage()
        {
            //------------------------------------------------------ חלון שמציג את הקורסים שחייב לעשות המסלול ליבה של ההתמחות
            coreProgramShowPanel = new Panel();
            coreProgramShowPanel.AutoScroll = true;
            coreProgramShowPanel.BackColor = yellowPaletteDay["otherColor"];
            coreProgramShowPanel.Location = new Point(46, 138);
            coreProgramShowPanel.Name = "coreProgramShowPanel";
            coreProgramShowPanel.Size = new Size(533, 569);
            coreProgramShowPanel.TabIndex = 4;
            //----------------------------------------------------- כפתור הוספת קורס חדש  
            addCourseBtn = new Button();
            addCourseBtn.Location = new Point(70, 10);
            addCourseBtn.Name = "addCourseToCP";
            addCourseBtn.Font = new Font("Arial", 14F, FontStyle.Bold);
            addCourseBtn.ForeColor = yellowPaletteDay["titleColor"];
            addCourseBtn.Size = new Size(430, 55);
            addCourseBtn.TabIndex = 4;
            addCourseBtn.Text = " הוספת קורס חדש ";
            addCourseBtn.Image = dayIcons["add"];
            addCourseBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
            addCourseBtn.ImageAlign = ContentAlignment.MiddleCenter;
            addCourseBtn.Click += addCourseWindow;
            addCourseBtn.Tag = new List<Object> { "comeFrom" + specializationShowPanel.Name };

            coreProgramShowPanel.Controls.Add(addCourseBtn);

            //----------------------------------------------------- כפתור חזרה אחורה  
            goBack = new Label();
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 25F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(10, 10);
            goBack.Name = "goBack";
            goBack.Size = new Size(50, 50);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Tag = new List<Object> { "comeFrom" + coreProgramShowPanel.Name };
            goBack.Click += ShowPreviousPage;


            coreProgramShowPanel.Controls.Add(goBack);
        }

       



        private void ShowCourse(object sender, EventArgs e)
        {   /*  המטרה: לאפשר דף של צפיה בקורסים, איך עושים את זה? שאלה טובה
             *  אני רוצה לאפשר לכולם להגיע לדף הזה כלומר שכל הפניות לצפיה בקורסים
             *  בין אם זה ממסלול ההתמחות או מהמסלול ליבה 
             *  כולם צריכים להפנות את מספר הקורס ומספר ההתמחות אם זה רלוונטי לדף הזה 
             */


            MessageBox.Show("אם הגעת לכאן סימן שאתה רוצה לראות קורס מסויים " + Environment.NewLine+
                               "יש קורסים שלא קיימים כי לא עשיתי לכולם דוגמאות" + Environment.NewLine +
                               "בגלל זה תהיה שגיאה אז בפועל המערכת לא בנויה ככה " + Environment.NewLine +
                               "כלומר, עבור כל קורס חייב להיות מרצה ותלמידים אין דבר כזה קורס ריק " + Environment.NewLine +
                               "אז אם יש שגיאה.. זה אמור להיות ככה"); 
            
            // --------------------- הכנת העמוד של הקורס עצמו 
            courseDetailsPanel = new Panel();
            courseDetailsPanel.AutoScroll = true;
            courseDetailsPanel.BackColor = yellowPaletteDay["otherColor"];
            courseDetailsPanel.Location = new Point(46, 138);
            courseDetailsPanel.Name = "courseDetailsPanel";
            courseDetailsPanel.Size = new Size(533, 569);
            courseDetailsPanel.TabIndex = 4;

            //  כפתור חזרה לעמוד הקודם
            goBack = new Label();
            goBack.AutoSize = true;
            goBack.Font = new Font("Arial", 25F, FontStyle.Bold);
            goBack.ForeColor = Color.SaddleBrown;
            goBack.Location = new Point(10, 10);
            goBack.Name = "goBack";
            goBack.Size = new Size(50, 30);
            goBack.TabIndex = 20;
            goBack.Text = "☜";
            goBack.Click += ShowPreviousPage;

            courseDetailsPanel.Controls.Add(goBack);
           
            RichTextBox catchCourse = sender as RichTextBox;
            string name = ""; string fmName = ""; string id = ""; string phoneNum = ""; string gmail = "";
            string age = ""; string birthday = ""; string rating = ""; string employeeNumber = ""; string specialization = "";

            if (this.Controls.Contains(coreProgramShowPanel)) // קורס שנמצא במסלול ליבה
            {
                goBack.Name = "goBackToCourseProgramPage";
                int courseNumber = int.Parse(catchCourse.Name.Substring("CoreCourse".Length));
                title.Text = this.user.CoreCourse.Courses[courseNumber].Name;

                name = this.user.CoreCourse.Courses[courseNumber].Teacher.Name;
                fmName = this.user.CoreCourse.Courses[courseNumber].Teacher.FmName;
                id = this.user.CoreCourse.Courses[courseNumber].Teacher.Id;
                phoneNum = this.user.CoreCourse.Courses[courseNumber].Teacher.PhoneNum;
                gmail = this.user.CoreCourse.Courses[courseNumber].Teacher.Gmail;
                age = this.user.CoreCourse.Courses[courseNumber].Teacher.Age;
                birthday = this.user.CoreCourse.Courses[courseNumber].Teacher.Birthday.ToString("dd/MM/yyyy");
                rating = this.user.CoreCourse.Courses[courseNumber].Teacher.Rating.ToString();
                employeeNumber = this.user.CoreCourse.Courses[courseNumber].Teacher.EmployeeNumber.ToString();
                specialization = this.user.CoreCourse.Courses[courseNumber].Teacher.SpecializationName;
                // -------------- הוספה של המרצים שמלמדים את הקורס ------------------
                createCoreProgramCourseDetails();
            }
            else
            if (this.Controls.Contains(specializationShowPanel)) // קורס שנמצא במסלול התמחות
            {
                goBack.Name = "goBackToSpecializationPage";
                int specializationNumber = int.Parse(catchCourse.Name.Substring("specialization".Length, 1));
                int courseNumber = int.Parse(catchCourse.Name.Substring("specializationIcourseProgram".Length));

                MessageBox.Show("courseNumber" + courseNumber + "specializationNumber" + specializationNumber + "have" + this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Students.Count);

                title.Text = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Name;
                name = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Name;
                fmName = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.FmName;
                id = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Id;
                phoneNum = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.PhoneNum;
                gmail = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Gmail;
                age = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Age;
                birthday = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Birthday.ToString("dd/MM/yyyy");
                rating = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.Rating.ToString();
                employeeNumber = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.EmployeeNumber.ToString();
                specialization = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Teacher.SpecializationName;

                // -------------- הוספת כפתור העריכה- הוא לא יהיה נגיש במסלול ליבה כי הוא לצפיה בלבד ------------------

                /*כפתור שהופך עריכה לזמינה אז זה תלוי והוא צריך לקבל את הפרמטרים של הקורס באיזה מסלול הוא ואם הוא במסלול ליבה וכו וכו
                 * אז אני משתמשת ב תג אבל אני לא כותבת אותו פה כלומר אני יוצרת אותו כשאני יוצרת את כל מה 
                 * שקשור להכנסה של הפריטים הנכונים לקורס כי זה נראה לי לא יעיל כל פעם ליצור את הכפתור הזה מחדש
                 * הוא יווצר פעם אחת ואז כשאני עושה את הפעולה 
                 * createSpecializationCourseDetails
                 * אני מכניסה את כל השינויים הרלוונטים כי טיפול במסלול ליבה ומסלול התמחות הוא שונה וכל אחד נכתב ברשימה שונה 
                 */
                addStudentBtn = new Button();
                addStudentBtn.Font = new Font("Arial", 15F, FontStyle.Bold);
                addStudentBtn.ForeColor = Color.SaddleBrown;
                addStudentBtn.BackColor = Color.White;
                addStudentBtn.Name = "addStudentBtn";
                addStudentBtn.Location = new Point(40, 230);
                addStudentBtn.Size = new Size(430, 40);
                addStudentBtn.TabIndex = 4;
                addStudentBtn.Text = "הוסף סטודנט +";
                // addStudentBtn.Click += AddStudent;

                courseDetailsPanel.Controls.Add(addStudentBtn);
                /*  אני לא אתן במסלול ליבה את האפשרות ליצור סטודנט חדש , כשהמשתמש ירצה להוסיף סטודנט זה יהיה\
                 *  במסלול התמחות ושם כשיוסיפו סטודנט הוא אוטומטית יכנס גם למסלול ליבה
                 */
                makeItEditable = new Button();
                makeItEditable.Font = new Font("Arial", 15F, FontStyle.Bold);
                makeItEditable.ForeColor = Color.SaddleBrown;
                makeItEditable.Location = new Point(70, 10);
                makeItEditable.Name = "makeItEditable";
                makeItEditable.Size = new Size(430, 40);
                makeItEditable.TabIndex = 4;
                makeItEditable.Text = "הפוך עריכה לזמינה";
                makeItEditable.Click += MakeAllEditable;  // כפתור שפותח את הדף של המסלול חובה שמציג את הקורסים שלו

                courseDetailsPanel.Controls.Add(makeItEditable);

                // -------------- הוספה של המרצה שמלמד את הקורס ------------------
                lecturerLabel = new Label();
                lecturerLabel.AutoSize = true;
                lecturerLabel.Font = new Font("Arial", 15F, FontStyle.Bold);
                lecturerLabel.ForeColor = Color.SaddleBrown;
                lecturerLabel.Location = new Point(0, 60);
                lecturerLabel.Name = "lecturerLabel";
                lecturerLabel.Size = new Size(40, 20);
                lecturerLabel.TabIndex = 4;
                lecturerLabel.Text = "------------------ פרטי מרצה נוכחי -----------------";

                courseDetailsPanel.Controls.Add(lecturerLabel);
                // -------------------------------------------------------- נתוני מרצה 
                lecturerDetails = new DataGridView();
                lecturerDetails.Location = new Point(0, 90);
                lecturerDetails.Size = new Size(518, 120);
                lecturerDetails.Dock = DockStyle.None;
                lecturerDetails.ColumnCount = 5;
                lecturerDetails.ColumnHeadersVisible = false;
                lecturerDetails.RowHeadersVisible = false;

                lecturerDetails.ScrollBars = ScrollBars.None;

                lecturerDetails.AllowUserToAddRows = false;
                lecturerDetails.ReadOnly = false;

                lecturerDetails.SelectionMode = DataGridViewSelectionMode.CellSelect;
                lecturerDetails.MultiSelect = false;

                lecturerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                lecturerDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int i = 0; i < lecturerDetails.ColumnCount; i++)
                    lecturerDetails.Columns[i].Width = 102;

                lecturerDetails.Rows.Add("מייל", "מספר טלפון", "תז", "שם משפחה", "שם");
                lecturerDetails.Rows.Add(gmail, phoneNum, id, fmName, name);
                lecturerDetails.Rows.Add("התמחות", "מספר עובד", "דירוג מרצה", "תאריך לידה", "גיל");
                lecturerDetails.Rows.Add(specialization, employeeNumber, rating, birthday, age);

                lecturerDetails.Rows[0].ReadOnly = true;
                lecturerDetails.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;
                lecturerDetails.Rows[0].DefaultCellStyle.ForeColor = Color.White;

                lecturerDetails.Rows[2].ReadOnly = true;
                lecturerDetails.Rows[2].DefaultCellStyle.BackColor = Color.DarkGray;
                lecturerDetails.Rows[2].DefaultCellStyle.ForeColor = Color.White;

                // כשהמצב עריכה לא פעיל אז
                lecturerDetails.Rows[1].ReadOnly = true;
                lecturerDetails.Rows[3].ReadOnly = true;
                courseDetailsPanel.Controls.Add(lecturerDetails);
                // -------------- הוספה של הסטודנטים כל אחד והטבלה עם הפרטים עליו ------------------
                createSpecializationCourseDetails(specializationNumber, courseNumber); // יוצר את הפרטים של הקורס אם מדובר בקורס שנמצא במסלול התמחות

            }
            Controls.Remove(coreProgramShowPanel);
            Controls.Remove(specializationShowPanel);


          
            //private Label studentLabel;
            //private Label lecturerLabel;


              
            Controls.Add(courseDetailsPanel);



        }
        private void createSpecializationCourseDetails(int specializationNumber, int courseNumber)
        { 
            /* פונקציה שיוצרת את הפרטים הנכונים עבור אותו קורס שנמצא במסלול התמחות  */
            int baseY = 210;

            for (int i = 0; i < this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Students.Count; i++)
            {
                Student studentX = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Students[i];
                Label studentLabel = new Label();
                studentLabel.AutoSize = true;
                studentLabel.Font = new Font("Arial", 15F, FontStyle.Bold);
                studentLabel.ForeColor = Color.SaddleBrown;
                studentLabel.Location = new Point(30, baseY);
                studentLabel.Name = "studentLabel"+i;
                studentLabel.Size = new Size(50, 30);
                studentLabel.TabIndex = 4; 
                studentLabel.Text = "---------------------סטודנט " +(i+1) + "--------------------";

                courseDetailsPanel.Controls.Add(studentLabel);

                Button remove = new Button();
                remove.Location = new Point(470, baseY);
                remove.Name = "removeStudentFromspecialization" + i;
                remove.Size = new Size(40, 40);
                remove.TabIndex = 4;
                remove.Image = dayIcons["remove"];
                //                      מספר קורס      מספר התמחות       מספר סטודנט   
                remove.Tag = new object[] { i, specializationNumber, courseNumber };// <---- הדבר הכי טוב שקרה לי בעבודה הזאת
                remove.Click += RemoveStudent;

                courseDetailsPanel.Controls.Add(remove);

                DataGridView studentDetails = new DataGridView();
                studentDetails.Name = "studentDetails" + i ;
                studentDetails.Location = new Point(0, baseY + 50);
                studentDetails.Size = new Size(518, 120);
                studentDetails.Dock = DockStyle.None;
                studentDetails.ColumnCount = 5;
                studentDetails.ColumnHeadersVisible = false;
                studentDetails.RowHeadersVisible = false;

                studentDetails.ScrollBars = ScrollBars.None;

                studentDetails.AllowUserToAddRows = false;


                studentDetails.SelectionMode = DataGridViewSelectionMode.CellSelect;
                studentDetails.MultiSelect = false;

                studentDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                studentDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int j = 0; j < studentDetails.ColumnCount; j++)
                    studentDetails.Columns[j].Width = 102;

                studentDetails.Rows.Add("מייל", "מספר טלפון", "תז", "שם משפחה", "שם");
                studentDetails.Rows.Add(studentX.Gmail, studentX.PhoneNum, studentX.Id, studentX.FmName, studentX.Name);
                studentDetails.Rows.Add("מספר סטודנט", "נקז נוכחי", "נקז כולל", "תאריך לידה", "גיל");
                studentDetails.Rows.Add(studentX.StudentNumber, studentX.CurrentCredits, studentX.TotalCredits, studentX.Birthday, studentX.Age);

                studentDetails.Rows[0].ReadOnly = true;
                studentDetails.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;
                studentDetails.Rows[0].DefaultCellStyle.ForeColor = Color.White;

                studentDetails.Rows[2].ReadOnly = true;
                studentDetails.Rows[2].DefaultCellStyle.BackColor = Color.DarkGray;
                studentDetails.Rows[2].DefaultCellStyle.ForeColor = Color.White;

                // כשהמצב עריכה לא פעיל אז
                studentDetails.Rows[1].ReadOnly = true;
                studentDetails.Rows[3].ReadOnly = true;

                courseDetailsPanel.Controls.Add(studentDetails);

                baseY += 170;
            }

            addStudentBtn.Tag = new object[] { specializationNumber, courseNumber };
            addStudentBtn.Location = new Point(40, baseY+30);
            makeItEditable.Tag = new object[] { specializationNumber, courseNumber  };

            courseDetailsPanel.Controls.Add(addStudentBtn);



        }
   
        private void createCoreProgramCourseDetails()
        {
            string id;
            string name;
            string firstName;
            int baseY = 60;
            for (int i=0; i< this.user.CoreCourse.Courses.Count; i++) {
                Lecturer lecturerX = this.user.CoreCourse.Courses[i].Teacher;

                lecturerDetails = new DataGridView();
                lecturerDetails.Location = new Point(0, baseY);
                lecturerDetails.Name = "lecturerDetails" + i;
                lecturerDetails.Size = new Size(518, 120);
                lecturerDetails.Dock = DockStyle.None;
                lecturerDetails.ColumnCount = 5;
                lecturerDetails.ColumnHeadersVisible = false;
                lecturerDetails.RowHeadersVisible = false;

                lecturerDetails.ScrollBars = ScrollBars.None;

                lecturerDetails.AllowUserToAddRows = false;
                lecturerDetails.ReadOnly = false;

                lecturerDetails.SelectionMode = DataGridViewSelectionMode.CellSelect;
                lecturerDetails.MultiSelect = false;

                lecturerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                lecturerDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                for (int j = 0; j < lecturerDetails.ColumnCount; j++)
                    lecturerDetails.Columns[j].Width = 102;

                lecturerDetails.Rows.Add("מייל", "מספר טלפון", "תז", "שם משפחה", "שם");
                lecturerDetails.Rows.Add(lecturerX.Gmail, lecturerX.PhoneNum, lecturerX.Id, lecturerX.FmName, lecturerX.Name);
                lecturerDetails.Rows.Add("התמחות", "מספר עובד", "דירוג מרצה", "תאריך לידה", "גיל");
                lecturerDetails.Rows.Add(lecturerX.SpecializationName, lecturerX.EmployeeNumber, lecturerX.Rating, lecturerX.Birthday, lecturerX.Age);

                lecturerDetails.Rows[0].ReadOnly = true;
                lecturerDetails.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;
                lecturerDetails.Rows[0].DefaultCellStyle.ForeColor = Color.White;

                lecturerDetails.Rows[2].ReadOnly = true;
                lecturerDetails.Rows[2].DefaultCellStyle.BackColor = Color.DarkGray;
                lecturerDetails.Rows[2].DefaultCellStyle.ForeColor = Color.White;
                courseDetailsPanel.Controls.Add(lecturerDetails);

                ComboBox studentInCourse = new ComboBox();
                studentInCourse.Location = new Point(230, baseY+130);
                studentInCourse.Size = new Size(250, 30);
                studentInCourse.DropDownStyle = ComboBoxStyle.DropDownList;
                studentInCourse.Items.Add("סטודנטים בקורס");
             
                for (int k = 0; k <this.user.CoreCourse.Courses[i].Students.Count; k++)
                {
                    firstName = this.user.CoreCourse.Courses[i].Students[k].Name;
                    name = this.user.CoreCourse.Courses[i].Students[k].FmName;
                    id = this.user.CoreCourse.Courses[i].Students[k].Id;
                    studentInCourse.Items.Add(firstName+" "+ name+" - "+ id);
                }

                courseDetailsPanel.Controls.Add(studentInCourse);

                baseY += 170;


            }
            
        }



        private void RemoveStudent(object sender, EventArgs e)
        {
            // פעולה שמוחקת סטודנט אם זה מקורס שבמסלוןל התמחות ובין אם זה מקורס שבמסלול ליבה 
            Button removeBtn = sender as Button;
            if (removeBtn.Name.Substring(0, removeBtn.Name.Length - 1) == "removeStudentFromspecialization")
            {
                int courseNum = (int)((object[])removeBtn.Tag)[2];
                int specializationNum = (int)((object[])removeBtn.Tag)[1];
                int studentNum = (int)((object[])removeBtn.Tag)[0];
                
                for (int i = 0; i < this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.Count; i++)
                {
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["studentLabel" + i]);
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["removeStudentFromspecialization" + i]);
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["student" + i]);
                }

                this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.RemoveAt(studentNum);
                createSpecializationCourseDetails(specializationNum, courseNum); // יוצר את הפרטים של הקורס אם מדובר בקורס שנמצא במסלול התמחות

            }
            else if(removeBtn.Name == "removeStudentFromsCoreProgram")
            {
                int courseNum = (int)((object[])removeBtn.Tag)[1];
                int studentNum = (int)((object[])removeBtn.Tag)[2];
                this.user.CoreCourse.Courses[courseNum].Students.RemoveAt(studentNum);

                for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                {
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["studentLabel" + i]);
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["removeStudentFromspecialization" + i]);
                    courseDetailsPanel.Controls.Remove(courseDetailsPanel.Controls["student" + i]);
                }
            }


        }

       
        //------ יצירת כותרות
        private void createSpecializationTitles() 
        {
            //------------------------------------------------------ הצגת מסלולי התמחות       
            int baseY = 80;
            for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
            {

                RichTextBox specializationTrack = new RichTextBox();
                specializationTrack.Location = new Point(110, baseY);
                specializationTrack.Font = new Font("Arial", 14F, FontStyle.Bold);
                specializationTrack.ForeColor = yellowPaletteDay["titleColor"];
                specializationTrack.BackColor = yellowPaletteDay["buttonColor"];
                specializationTrack.BorderStyle = BorderStyle.None;
                specializationTrack.ReadOnly = true;
                specializationTrack.RightToLeft = RightToLeft.Yes;
                specializationTrack.Name = "specializationTrack" + i;
                specializationTrack.Size = new Size(390, 40);
                specializationTrack.TabIndex = 4;
                specializationTrack.Text = this.user.SpecializationCourses[i].Name;
                specializationTrack.DoubleClick += ShowSpecializationTruck;

                Button remove = new Button();
                remove.Location = new Point(10, baseY);
                remove.Name = "removeSTruck" + i;
                remove.Size = new Size(40, 40);
                remove.TabIndex = 4;
                remove.Image = dayIcons["remove"];
                remove.Click += RemoveSpecializationTrack;

                Button changeName = new Button();
                changeName.Location = new Point(60, baseY);
                changeName.Name = "changeSpecializationName" + i;
                changeName.Size = new Size(40, 40);
                changeName.TabIndex = 4;
                changeName.Image = dayIcons["changeName"];
                changeName.Click += ChangeName;

                myClassPanel.Controls.Add(specializationTrack);
                myClassPanel.Controls.Add(remove);
                myClassPanel.Controls.Add(changeName);

                baseY += 60;
            }
            ChangeMood(null, EventArgs.Empty);
        }
        private void createCoursesTitles()
        {
            //----------------------------------------------------- הוספת הקורסים לחלון 

            for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
            {
              
                 int baseY = 80;
                for (int j = 0; j < this.user.SpecializationCourses[i].Courses.Count; j++)
                {
                    RichTextBox courseProgram = new RichTextBox();
                    courseProgram.Location = new Point(110, baseY);
                    courseProgram.Font = new Font("Arial", 14F, FontStyle.Bold);
                    courseProgram.ForeColor = yellowPaletteDay["titleColor"];
                    courseProgram.BackColor = yellowPaletteDay["buttonColor"];
                    courseProgram.BorderStyle = BorderStyle.None;
                    courseProgram.ReadOnly = true;
                    courseProgram.RightToLeft = RightToLeft.Yes;
                    courseProgram.Name = "specialization" + i + "courseProgram" + j;
                    courseProgram.Size = new Size(390, 40);
                    courseProgram.TabIndex = 4;
                    courseProgram.Text = this.user.SpecializationCourses[i].Courses[j].Name;
                    courseProgram.DoubleClick += ShowCourse;

                    Button remove = new Button();
                    remove.Location = new Point(10, baseY);
                    remove.Name = "specialization" + i + "removeCourse" + j;
                    remove.Size = new Size(40, 40);
                    remove.TabIndex = 4;
                    remove.Image = dayIcons["remove"];
                    remove.Click += RemoveSpecializationTrack;

                    Button changeName = new Button();
                    changeName.Location = new Point(60, baseY);
                    changeName.Name = "specialization" + i + "changeCourseName" + j;
                    changeName.Size = new Size(40, 40);
                    changeName.TabIndex = 4;
                    changeName.Image = dayIcons["changeName"];
                    changeName.Click += ChangeName;

                    courseProgram.Visible = false;
                    remove.Visible = false;
                    changeName.Visible = false;

                    specializationShowPanel.Controls.Add(courseProgram);
                    specializationShowPanel.Controls.Add(remove);
                    specializationShowPanel.Controls.Add(changeName);

                    baseY += 60;
                }
            }
            ChangeMood(null, EventArgs.Empty);

        }
        private void createCoreProgramCoursesTitles()
        {
            //------------------------------------------------------ 
            int baseY = 80;
            for (int i = 0; i < this.user.CoreCourse.Courses.Count; i++)
            {

                RichTextBox CoreCourse = new RichTextBox();
                CoreCourse.Location = new Point(110, baseY);
                CoreCourse.Font = new Font("Arial", 14F, FontStyle.Bold);
                CoreCourse.ForeColor = yellowPaletteDay["titleColor"];
                CoreCourse.BackColor = yellowPaletteDay["buttonColor"];
                CoreCourse.BorderStyle = BorderStyle.None;
                CoreCourse.ReadOnly = true;
                CoreCourse.RightToLeft = RightToLeft.Yes;
                CoreCourse.Name = "CoreCourse" + i;
                CoreCourse.Size = new Size(390, 40);
                CoreCourse.TabIndex = 4;
                CoreCourse.Text = this.user.CoreCourse.Courses[i].Name;
                CoreCourse.DoubleClick += ShowCourse;

                Button remove = new Button();
                remove.Location = new Point(10, baseY);
                remove.Name = "removeCoreCourse" + i;
                remove.Size = new Size(40, 40);
                remove.TabIndex = 4;
                remove.Image = dayIcons["remove"];
                remove.Click += RemoveSpecializationTrack;

                Button changeName = new Button();
                changeName.Location = new Point(60, baseY);
                changeName.Name = "changeCoreCourseName" + i;
                changeName.Size = new Size(40, 40);
                changeName.TabIndex = 4;
                changeName.Image = dayIcons["changeName"];
                changeName.Click += ChangeName;

                coreProgramShowPanel.Controls.Add(CoreCourse);
                coreProgramShowPanel.Controls.Add(remove);
                coreProgramShowPanel.Controls.Add(changeName);

                baseY += 60;
            }
            ChangeMood(null, EventArgs.Empty);
        }
        // ------------------------------------------------ 
        private void ShowSpecializationTruck(object sender, EventArgs e)
        {
            /* פעולה שמציגה את הדף עם הקורסים הנוכחיים של המסלול התמחות שהמשתמש בחר לראות 
               לא רציתי ליצור כל פעם מחדש את העמודים האלה כי חבל על המאמץ זה יכול להגיע ל10 או 15 קורסי התמחות
                בכל אחד מהם יש נניח 16 קורסים בדרכ? באוניברסיטה אמיתית? אז זה סתם ליצור שוב הכל כי פתחתי דף שמציג מסלול
                העדפתי ליצור את כולם ופשוט לשחק עם הנראות שלהם על הדף
                יש מצב שאני טועה והיה עדיף ליצור שוב מחדש אם כן אני אשמח לדעת מה השיקולים שעושים כשבונים מערכת כזאת*/
            RichTextBox specializationTrack = sender as RichTextBox;
            title.Text = " מסלול " + specializationTrack.Text;
            int specializationNamber = int.Parse(specializationTrack.Name.Substring("specializationTrack".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק
            specializationShowPanel.Controls["addCourseToSP"].Tag = new List<Object> { "comeFrom" + specializationShowPanel.Name, specializationNamber };

            // מחזיק בכפתור יצירת קורס את ה"מוצא" של הקורס כלומר מאיזה דף הוא בא ואיזה התמחות
            // למשל עבור כפתור כזה אני אקבל comeFromspecializationShowPanel ומספר כלשהו
            // מסויים x ואז אני אדע שהקורס נוסף ממסלול התמחות
            // (אני לא חושבת שזאת היתה החלטה חכמה או דרך נכונה לעשות את זה זה יעבוד אבל זה מסורבל ולא יעיל או תחזוקתי)

            if (this.user.SpecializationCourses[specializationNamber].Courses.Count != 0)
            {
                for (int j = 0; j < this.user.SpecializationCourses[specializationNamber].Courses.Count; j++)
                {

                    specializationShowPanel.Controls["specialization" + specializationNamber + "courseProgram" + j].Visible = true;
                    specializationShowPanel.Controls["specialization" + specializationNamber + "removeCourse" + j].Visible = true;
                    specializationShowPanel.Controls["specialization" + specializationNamber + "changeCourseName" + j].Visible = true;

                }
            }
            else
                redNotes.Visible = true;

            Controls.Remove(myClassPanel);
            Controls.Add(specializationShowPanel);
        }

        // ----------------------------------------------------- פעולות חשובות כל אחת מתחשבת במצב הנוכחי שבו הפורם נמצא 
        private void MakeAllEditable(object? sender, EventArgs e)
        {
            Button editBtn = sender as Button;
            int courseNum = (int)((object[])editBtn.Tag)[1];
            int specializationNum = (int)((object[])editBtn.Tag)[0];
            DataGridView dgv;

            if (editMood)
            { // אם הוא במצב עריכה אז צריך לצאת ממצב עריכה ואז 
                editMood = false;

                makeItEditable.Text = "הפוך עריכה לזמינה";
                lecturerDetails.Rows[1].ReadOnly = true; // ניתן לקריאה וכתיבה
                lecturerDetails.Rows[3].ReadOnly = true; // ניתן לקריאה וכתיבה

                if (this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.Count != 0)
                    for (int i = 0; i < this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.Count; i++)
                    {
                        dgv = courseDetailsPanel.Controls["studentDetails" + i] as DataGridView;
                        dgv.Rows[1].ReadOnly = true; // ניתן לקריאה וכתיבה
                        dgv.Rows[3].ReadOnly = true; // ניתן לקריאה וכתיבה

                    }
            }
            else
            {
                if (!editMood)
                {// אם הוא לא במצב עריכה ולוחץ אנחנו רוצים להפוך את העריכה לזמינה אז..
                    editMood = true;


                    makeItEditable.Text = "סיים";
                    lecturerDetails.Rows[1].ReadOnly = false; // ניתן לקריאה וכתיבה
                    lecturerDetails.Rows[3].ReadOnly = false; // ניתן לקריאה וכתיבה


                    if (this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.Count != 0)
                        for (int i = 0; i < this.user.SpecializationCourses[specializationNum].Courses[courseNum].Students.Count; i++)
                        {
                            dgv = courseDetailsPanel.Controls["studentDetails" + i] as DataGridView;
                            dgv.Rows[1].ReadOnly = false; // ניתן לקריאה וכתיבה
                            dgv.Rows[3].ReadOnly = false; // ניתן לקריאה וכתיבה
                        }


                }
                else MessageBox.Show("יש בעיה בעריכה של קורסים קיימים");
            }

        }
        private void ShowPreviousPage(object sender, EventArgs e)
        { /* פעולה שתחזור לדף הקודם בהתחשב בדף הנוכחי שהפורם מציג בזמן אמת */
            if (this.Controls.Contains(specializationShowPanel))
            {
               for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                for (int j = 0; j < this.user.SpecializationCourses[i].Courses.Count; j++)
                {
                    specializationShowPanel.Controls["specialization" + i + "courseProgram" + j].Visible = false;
                    specializationShowPanel.Controls["specialization" + i + "removeCourse" + j].Visible = false;
                    specializationShowPanel.Controls["specialization" + i + "changeCourseName" + j].Visible = false;
                      
                }
                specializationShowPanel.Controls["redNotes"].Visible = false;
                Controls.Remove(specializationShowPanel);
                Controls.Add(myClassPanel);
                title.Text = "המחלקה שלי - " + this.user.SpecializationName;

            }
            else
                 if (this.Controls.Contains(coreProgramShowPanel))
                 {
                    Controls.Remove(coreProgramShowPanel);
                    Controls.Remove(specializationShowPanel);
                    Controls.Add(myClassPanel);
                   title.Text = "המחלקה שלי - " + this.user.SpecializationName;

            }
            else
            {
                if (this.Controls.Contains(courseDetailsPanel))
                {
                    Label goBackBtn = sender as Label;
                    if (goBackBtn.Name == "goBackToSpecializationPage")
                    {
                        Controls.Remove(courseDetailsPanel);
                        Controls.Add(specializationShowPanel);
                        title.Text = "";
                    }
                    else
                    if (goBackBtn.Name == "goBackToCourseProgramPage")
                    {
                        Controls.Remove(courseDetailsPanel);
                        Controls.Add(coreProgramShowPanel);
                        title.Text = "";
                    }
                }
                else
                {
                    if (this.Controls.Contains(createSpecializationPanel))
                    {
                        for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                        {
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["CoreCourse" + i]);
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["removeCoreCourse" + i]);
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["changeCoreCourseName" + i]);
                        }
                        Controls.Remove(createSpecializationPanel);
                        Controls.Add(myClassPanel);
                        title.Text = "המחלקה שלי - " + this.user.SpecializationName;
                    }
                    else
                    {
                        if (this.Controls.Contains(createCoursePanel))
                        {
                            if (sender is Label) 
                            { 
                                Label whereWeComeFrom = sender as Label;
                                if ((whereWeComeFrom.Tag as string) == "comeFromspecializationShowPanel")
                                {
                                    Controls.Remove(createCoursePanel);
                                    Controls.Add(specializationShowPanel);
                                    title.Text = "";
                                }
                                else
                                if ((whereWeComeFrom.Tag as string) == "comeFromcoreProgramShowPanel")
                                {
                                    Controls.Remove(createCoursePanel);
                                    Controls.Add(coreProgramShowPanel);
                                    title.Text = "";
                                }
                            }
                            else
                            {
                                if (sender is Button)
                                {
                                    Button whereWeComeFrom = sender as Button;
                                    if ((whereWeComeFrom.Tag as string) == "comeFromspecializationShowPanel")
                                    {
                                        Controls.Remove(createCoursePanel);
                                        Controls.Add(specializationShowPanel);
                                        title.Text = "";
                                    }
                                    else
                                    if ((whereWeComeFrom.Tag as string) == "comeFromcoreProgramShowPanel")
                                    {
                                        Controls.Remove(createCoursePanel);
                                        Controls.Add(coreProgramShowPanel);
                                        title.Text = "";
                                    }
                                }
                            }
                           
                        }
                       
                    }
                }
            }

        }
        private void DoneEditing(object sender, EventArgs e)
        {    /* פעולה שיוצאת ממצב עריכה */
            RichTextBox isBeingEdited = sender as RichTextBox;
            string oldName;
            if (editMood)
            {
                editMood = false; // יוצא ממצב עריכה

                if (Controls.Contains(myClassPanel)) // עידכון שם של אחד מהמסלולי התמחות
                {
                    int specializationNamber = int.Parse(isBeingEdited.Name.Substring("specializationTrack".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק

                    oldName = this.user.SpecializationCourses[specializationNamber].Name;

                    MessageBox.Show("old name " + oldName + " new name" + isBeingEdited.Text);
                    Specialization.UpdateSpecializationName(isBeingEdited.Text, oldName);

                    this.user.SpecializationCourses[specializationNamber].Name = oldName;
                    isBeingEdited.ReadOnly = true;
                    isBeingEdited.Leave -= DoneEditing;
                    if (isDay)
                    {
                        isBeingEdited.BackColor = yellowPaletteDay["buttonColor"];
                        isBeingEdited.ForeColor = yellowPaletteDay["titleColor"];
                    }
                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                        myClassPanel.Controls["changeSpecializationName" + i].Enabled = true;
                }
                else
                {
                    if (Controls.Contains(specializationShowPanel)) // עידכון שם קורס באחד ממסלולי ההתמחות
                    {

                        int specializationNumber = int.Parse(isBeingEdited.Name.Substring("specialization".Length, 1));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק
                        int courseNumber = int.Parse(isBeingEdited.Name.Substring("specializationIcourseProgram".Length));

                        oldName = this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Name;
                        this.user.SpecializationCourses[specializationNumber].Courses[courseNumber].Name = specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + courseNumber].Text;

                        MessageBox.Show("use to be" + oldName + "now its" + isBeingEdited.Text);

                        Course.UpdateCourseName(specializationNumber, isBeingEdited.Text, oldName, this.user.SpecializationName);

                        isBeingEdited.ReadOnly = true;
                        isBeingEdited.Leave -= DoneEditing;
                        if (isDay)
                        {
                            isBeingEdited.BackColor = yellowPaletteDay["buttonColor"];
                            isBeingEdited.ForeColor = yellowPaletteDay["titleColor"];
                        }

                        for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                            for (int j = 0; j < this.user.SpecializationCourses[i].Courses.Count; j++)
                                specializationShowPanel.Controls["specialization" + i + "changeCourseName" + j].Enabled = true;

                    }
                    else
                    {
                        if (Controls.Contains(coreProgramShowPanel)) // עידכון שם קורס במסלול ליבה
                        {

                            int coreCourseNumber = int.Parse(isBeingEdited.Name.Substring("CoreCourse".Length));

                            oldName = this.user.CoreCourse.Courses[coreCourseNumber].Name;
                            MessageBox.Show("9999");
                            Course.UpdateCoreCourseName(isBeingEdited.Text, oldName, this.user.SpecializationName);
                            this.user.CoreCourse.Courses[coreCourseNumber].Name = isBeingEdited.Text;
                            isBeingEdited.ReadOnly = true;
                            isBeingEdited.Leave -= DoneEditing;
                            if (isDay)
                            {
                                isBeingEdited.BackColor = yellowPaletteDay["buttonColor"];
                                isBeingEdited.ForeColor = yellowPaletteDay["titleColor"];
                            }
                            for (int i = 0; i < this.user.CoreCourse.Courses.Count; i++)
                                coreProgramShowPanel.Controls["changeCoreCourseName" + i].Enabled = true;
                        }
                    }
                }
                   

            }

        }
        private void ChangeName(object sender, EventArgs e)
        {
            /* פעולה שמאפשרת מצב עריכה */
            if (!editMood) // בודק אם המשתמש לא נמצא במצב עריכה
            {
                editMood = true;

                Button changeNameBtn = sender as Button;

                if (Controls.Contains(myClassPanel))
                {
                    int specializationNamber = int.Parse(changeNameBtn.Name.Substring("changeSpecializationName".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק

                    RichTextBox rtb = myClassPanel.Controls["specializationTrack" + specializationNamber] as RichTextBox;
                    rtb.ReadOnly = false;
                    rtb.BackColor = Color.LightSteelBlue;
                    rtb.ForeColor = Color.White;
                    rtb.Leave += DoneEditing;
                    rtb.Focus();

                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                        myClassPanel.Controls["changeSpecializationName" + i].Enabled = false;

                }
                else
                {
                    if(Controls.Contains(specializationShowPanel)) {
                        int specializationNumber = int.Parse(changeNameBtn.Name.Substring("specialization".Length, 1));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק
                        int courseNumber = int.Parse(changeNameBtn.Name.Substring("specializationIchangeCourseName".Length));

                     //   MessageBox.Show("editing specializationNumber"+ specializationNumber+ "courseNumber"+ courseNumber);

                        RichTextBox rtb = specializationShowPanel.Controls["specialization" + specializationNumber+ "courseProgram"+ courseNumber] as RichTextBox;
                        rtb.ReadOnly = false;
                        rtb.BackColor = Color.LightSteelBlue;
                        rtb.ForeColor = Color.White;
                        rtb.Leave += DoneEditing;
                        rtb.Focus();


                            for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                                for (int j = 0; j < this.user.SpecializationCourses[i].Courses.Count; j++)
                                specializationShowPanel.Controls["specialization" + i+ "changeCourseName"+j].Enabled = false;

                    }
                    else
                    {
                        if (Controls.Contains(coreProgramShowPanel))
                        {
                            int coreCourseNumber = int.Parse(changeNameBtn.Name.Substring("changeCoreCourseName".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק

                            RichTextBox rtb = coreProgramShowPanel.Controls["CoreCourse" + coreCourseNumber] as RichTextBox;
                            rtb.ReadOnly = false;
                            rtb.BackColor = Color.LightSteelBlue;
                            rtb.ForeColor = Color.White;
                            rtb.Leave += DoneEditing;
                            rtb.Focus();

                            for (int i = 0; i < this.user.CoreCourse.Courses.Count; i++)
                                coreProgramShowPanel.Controls["changeCoreCourseName" + i].Enabled = false;
                        }
                    }
                }

            }

        }
        private void RemoveSpecializationTrack(object sender, EventArgs e)
        {
            /* פעולה שמוחקת קורס מסלול התמחות או מסלול ליבה בהתחשב במצב של הדף הוכחי
            * ובהתחשב בצורך - אם יש צורך למחוק קורס צריך לדעת מה לעדכן
            * אם צריך למחוק מסלול התמחות זה משפיע על הקורסים הקיימים בתוכו כי קיימים רכיבים שמציגים קורסים
            * עם השמות והפרטים של הקורס שנמחק אז גם בזה צריך להתחשב
            * הדבר הלא יעיל בעליל אבל היחידי שמצאתי שנכון היה ליצור את הקורסים האלה שוב מחדש ולהוסיף אותם
            * כמובן אחרי שאני מוחקת את כל הקודמים
            */
            if (Controls.Contains(myClassPanel)) // אם הצב הנוכחי זה הדף שמציג את המסלולי התמחות והכפתור של המסלול ליבה אני רוצה אפשרות למחוק מסלול
            {
                Button removeSTruckBtn = sender as Button;
                int specializationNumber = int.Parse(removeSTruckBtn.Name.Substring("removeSTruck".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק

                for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                {
                    myClassPanel.Controls.Remove(myClassPanel.Controls["specializationTrack" + i]);
                    myClassPanel.Controls.Remove(myClassPanel.Controls["removeSTruck" + i]);
                    myClassPanel.Controls.Remove(myClassPanel.Controls["changeSpecializationName" + i]);
                }

                for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                {
                    specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + i]);
                    specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "removeCourse" + i]);
                    specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "changeCourseName" + i]);
                }
                this.user.SpecializationCourses.RemoveAt(specializationNumber);
              
                createSpecializationTitles(); // יצירה מחדש כי מחקנו
                createCoursesTitles(); // יצירה מחדש כי מחקנו
            }
            else
            {
                if (Controls.Contains(specializationShowPanel)) // אם המצב הנוכחי זה ההצגה של הקורסים שיש באותו מסלול התמחות אני רוצה לאפשר מחיקה של קורס במסלול התמחות
                {
                    Button removeSTruckBtn = sender as Button;
                    int specializationNumber = int.Parse(removeSTruckBtn.Name.Substring("specialization".Length, 1));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק
                    int courseNumber = int.Parse(removeSTruckBtn.Name.Substring("removeCourseIspecialization".Length));

                    for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    {
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "removeCourse" + i]);
                        specializationShowPanel.Controls.Remove(specializationShowPanel.Controls["specialization" + specializationNumber + "changeCourseName" + i]);
                    }

                    this.user.SpecializationCourses[specializationNumber].Courses.RemoveAt(courseNumber);
                    createCoursesTitles(); // יצירה מחדש כי מחקנו

                    for (int j = 0; j < this.user.SpecializationCourses[specializationNumber].Courses.Count; j++)
                    {
                        specializationShowPanel.Controls["specialization" + specializationNumber + "courseProgram" + j].Visible = true;
                        specializationShowPanel.Controls["specialization" + specializationNumber + "removeCourse" + j].Visible = true;
                        specializationShowPanel.Controls["specialization" + specializationNumber + "changeCourseName" + j].Visible = true;

                    }
                }
                else
                {
                    if (Controls.Contains(coreProgramShowPanel)) // כמו למעלה, אני רוצה אפשרות למחוק קורס שיש אבל כשהוא נמצא במסלול ליבה
                    {
                        Button removeCoreCourse = sender as Button;
                        int coreCourseNumber = int.Parse(removeCoreCourse.Name.Substring("removeCoreCourse".Length));// שומר את המספר של המסלול התמחות שהמשתמש רוצה למחוק

                        for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                        {
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["CoreCourse" + i]);
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["removeCoreCourse" + i]);
                            coreProgramShowPanel.Controls.Remove(coreProgramShowPanel.Controls["changeCoreCourseName" + i]);
                        }

                        this.user.CoreCourse.Courses.RemoveAt(coreCourseNumber);

                        createCoreProgramCoursesTitles(); // יצירה מחדש כי מחקנו

                    }
                }
            }
                
            
        }

        // ------------------------------------------------------ הצגת עמודים 
        private void prsonalInfro_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "מידע אישי";
            Controls.Add(infroPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        private void message_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "הודעות";
            Controls.Add(messageTabControl);
            ChangeMood(null, EventArgs.Empty);
        }
        private void search_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "חיפוש";
            Controls.Add(searchingPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        private void settings_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "הגדרות";
            Controls.Add(settingsPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        private void courseTaught_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "הקורסים שאני מלמד";
            Controls.Add(courseTaughtPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        private void myClass_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "המחלקה שלי - " + this.user.SpecializationName;
            Controls.Add(myClassPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        private void newRequests_Click(object sender, EventArgs e)
        {
            removeAll();
            title.Text = "בקשות חדשות";
            Controls.Add(newRequestsPanel);
            ChangeMood(null, EventArgs.Empty);
        }
        // -------------------------------------------------------------------
        private void ChangeMood(object sender, EventArgs e)
        {

            if (this.isDay)
            {
                //                                 day mood 

                // ---------------------------------------------------------------------------- backGroundColor

                this.BackColor = yellowPaletteDay["backGroundColor"];

                // ---------------------------------------------------------------------------- baseColor
                List<string> baseColorTools = new List<string>
                {"menuPanel","topPanel","userInfro","writingMessage","coursesNamesPanel", "studentsNamesPanel"};
                for (int i = 0; i < messagesPanelList.Count; i++)
                    baseColorTools.Add("messagePanel" + i);
                
                for (int i = 0; i < this.user.Courses.Count; i++)
                    for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                        baseColorTools.Add("courses" + i + "studentsTaught" + j);


                UpdateControlsColors(baseColorTools, yellowPaletteDay["titleColor"], false, true);
                UpdateControlsColors(baseColorTools, yellowPaletteDay["baseColor"], true, false);
                // ---------------------------------------------------------------------------- buttonColor
                List<string> buttonColorTools = new List<string>
                {"prsonalInfro","settings","message","search","logOut","courses", "myClassBtn","newRequestsBtn"
                ,"dateSearch","importanceSearch","favoriteSearch","sendButton","courseTaught"};
               
                for (int i = 0; i < this.user.Courses.Count; i++)
                    baseColorTools.Add("coursesTaught" + i);

                for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    baseColorTools.Add("specializationTrack" + i);


                UpdateControlsColors(buttonColorTools, yellowPaletteDay["titleColor"], false, true);
                UpdateControlsColors(buttonColorTools, yellowPaletteDay["buttonColor"], true, false);
                // ---------------------------------------------------------------------------- otherColor
                List<string> otherColorTools = new List<string>
                {"infroPanel","settingsPanel" ,"searchingPanel","showMessages","sendMessages","courseTaughtPanel"
                ,"specializationShowPanel"};

                UpdateControlsColors(otherColorTools, yellowPaletteDay["otherColor"], true, false);

                // ---------------------------------------------------------------------------- titleColor
                List<string> titleColorTools = new List<string>
                {"helloLable","title","dayLable","nightLable","searchLable",
                "prsonalInfro","settings","message","search","logOut","courses"};

                for (int i = 0; i < messagesPanelList.Count; i++)
                {
                    titleColorTools.Add("senderDetails" + i);
                    titleColorTools.Add("sendingDate" + i);
                }

                for (int i = 0; i < this.user.Courses.Count; i++)
                    titleColorTools.Add("coursesTaught" + i);

                for (int i = 0; i < this.user.Courses.Count; i++)
                    for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                        titleColorTools.Add("courses" + i + "studentsTaught" + j);

                UpdateControlsColors(titleColorTools, yellowPaletteDay["titleColor"], false, true);
                // ---------------------------------------------------------------------------- textColor
                List<string> textColorTools = new List<string>
                {"userInfro"};

                UpdateControlsColors(textColorTools, yellowPaletteDay["textColor"], false, true);
                UpdateControlsColors(textColorTools, yellowPaletteDay["baseColor"], true, false);

                // ---------------------------------------------------------------------------- icons             
                menuIcon.Image = this.dayIcons["menu"];
                prsonalInfro.Image = this.dayIcons["infro"];
                message.Image = this.dayIcons["message"];
                search.Image = this.dayIcons["search"];
                settings.Image = this.dayIcons["settings"];
                logOut.Image = this.dayIcons["logOut"];
                //  courses.Image = this.dayIcons["logOut"];
                switchIcon.Image = this.dayIcons["on"];
                courseTaught.Image = this.dayIcons["courseTaught"];
                newRequestsBtn.Image = this.dayIcons["newRequests"];
                myClassBtn.Image = this.dayIcons["myClass"];
                // ---------------------------------------------------------------------------- backToWhite             
                List<string> backToWhite = new List<string>
                { "dateSearch", "importanceSearch", "favoriteSearch", "sendButton" };
                for (int i = 0; i < messagesPanelList.Count; i++)
                {
                    backToWhite.Add("removeMessage" + i);
                    backToWhite.Add("favorMessages" + i);
                    backToWhite.Add("replyMessage" + i);
                    backToWhite.Add("messageContent" + i);
                }

                UpdateControlsColors(backToWhite, Color.White, true, false);
                UpdateControlsColors(backToWhite, yellowPaletteDay["titleColor"], false, true);

            }
            else
            {
                //                                 night mood


                // ---------------------------------------------------------------------------- backGroundColor
                this.BackColor = yellowPaletteNight["backGroundColor"];                //List<string> mainFormTools = new List<string>
                //// ---------------------------------------------------------------------------- baseColor
                List<string> baseColorTools = new List<string>
                {"menuPanel","topPanel","userInfro","writingMessage" ,"coursesNamesPanel", "studentsNamesPanel"};

                UpdateControlsColors(baseColorTools, yellowPaletteNight["baseColor"], true, false);
                // ---------------------------------------------------------------------------- buttonColor
                List<string> buttonColorTools = new List<string>
                {"prsonalInfro","settings","message","search","logOut","courses", "myClassBtn","newRequestsBtn"
                ,"dateSearch","importanceSearch","favoriteSearch","sendButton","courseTaught"};

                for (int i = 0; i < this.user.SpecializationCourses.Count; i++)
                    baseColorTools.Add("specializationTrack" + i);

                for (int i = 0; i < messagesPanelList.Count; i++)
                {
                    buttonColorTools.Add("removeMessage" + i);
                    buttonColorTools.Add("favorMessages" + i);
                    buttonColorTools.Add("replyMessage" + i);
                    buttonColorTools.Add("messagePanel" + i);
                }
                for (int i = 0; i < this.user.Courses.Count; i++)
                    buttonColorTools.Add("coursesTaught" + i);


                for (int i = 0; i < this.user.Courses.Count; i++)
                    for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                        buttonColorTools.Add("courses" + i + "studentsTaught" + j);


                UpdateControlsColors(buttonColorTools, yellowPaletteNight["buttonColor"], true, false);
                UpdateControlsColors(buttonColorTools, yellowPaletteNight["titleColor"], false, true);
                // ---------------------------------------------------------------------------- otherColor
                List<string> otherColorTools = new List<string>
                {"infroPanel","settingsPanel" ,"searchingPanel","showMessages","sendMessages","courseTaughtPanel"
                ,"specializationShowPanel"};

                UpdateControlsColors(otherColorTools, yellowPaletteNight["otherColor"], true, false);
                // ---------------------------------------------------------------------------- titleColor
                List<string> titleColorTools = new List<string>
                {"helloLable","title","dayLable","nightLable","searchLable" };

                for (int i = 0; i < messagesPanelList.Count; i++)
                {
                    titleColorTools.Add("senderDetails" + i);
                    titleColorTools.Add("sendingDate" + i);
                }

                UpdateControlsColors(titleColorTools, yellowPaletteNight["titleColor"], false, true);

                // ---------------------------------------------------------------------------- textColor
                List<string> textColorTools = new() { "userInfro" };
                for (int i = 0; i < messagesPanelList.Count; i++)
                    textColorTools.Add("messageContent" + i);


                UpdateControlsColors(textColorTools, yellowPaletteNight["baseColor"], true, false);
                UpdateControlsColors(textColorTools, yellowPaletteNight["textColor"], false, true);
                // ---------------------------------------------------------------------------- icons             
                menuIcon.Image = this.nightIcons["menu"];
                prsonalInfro.Image = this.nightIcons["infro"];
                message.Image = this.nightIcons["message"];
                search.Image = this.nightIcons["search"];
                settings.Image = this.nightIcons["settings"];
                logOut.Image = this.nightIcons["logOut"];
                // courses.Image = this.nightIcons["logOut"];
                switchIcon.Image = this.nightIcons["off"];
                courseTaught.Image = this.nightIcons["courseTaught"];
                newRequestsBtn.Image = this.nightIcons["newRequests"];
                myClassBtn.Image = this.nightIcons["myClass"];

               
            }

        }
        private void ChangeFlag(object sender, EventArgs e)
        {
            if (this.isDay) this.isDay = false;
            else this.isDay = true;
        }
        private void UpdateControlsColors(List<string> controlNames, Color color, bool changeBackColor, bool changeForeColor)
        {
            void UpdateControlRecursive(Control parent)
            {
                foreach (Control control in parent.Controls)
                {
                    // MessageBox.Show(control.Name + " == " + string.Join(", ", controlNames));
                    if (controlNames.Contains(control.Name))
                    {
                        if (changeBackColor)
                            control.BackColor = color;

                        if (changeForeColor)
                            control.ForeColor = color;
                    }


                    if (control.HasChildren)
                        UpdateControlRecursive(control);
                }
            }

            UpdateControlRecursive(this);
        }


        public void removeAll()
        {
            // הפאנלים שיש:     מידע אישי, הודעות , חיפוש , הגדרות , הקורסים שאני מלמד          
            Controls.Remove(settingsPanel);
            Controls.Remove(infroPanel);
            Controls.Remove(searchingPanel);
            Controls.Remove(messageTabControl);
            Controls.Remove(courseTaughtPanel);
            
            Controls.Remove(newRequestsPanel);
            Controls.Remove(myClassPanel);
            Controls.Remove(createSpecializationPanel);
            Controls.Remove(coreProgramShowPanel);
            Controls.Remove(createCoursePanel);
            Controls.Remove(specializationShowPanel); 


        }


        //--------------------------------------------------------- פעולות סגירה
        private void logOut_Click(object sender, EventArgs e)
        {
            this.user.LastLoginDate = DateTime.Now;// שומר תאריך יציאה
            HeadOfDepartment.WriteHeadOfDepartmentToFile(this.user);
            StartWindow newStart = new StartWindow();
            newStart.Show();
            this.Hide();
            this.Dispose();
        }
        private void CloseAll(object sender, FormClosingEventArgs e)
        {
            this.user.LastLoginDate = DateTime.Now;

            HeadOfDepartment.WriteHeadOfDepartmentToFile(this.user);
            Application.Exit();
        }



    }
}
