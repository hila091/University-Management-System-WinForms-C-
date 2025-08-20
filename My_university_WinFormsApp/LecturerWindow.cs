using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using My_university_WinFormsApp.Models;

namespace My_university_WinFormsApp
{

    public partial class LecturerWindow : Form
    {
        private Lecturer user;
        private bool isMenuOpen;
        private bool isDay;

        Dictionary<string, Image> dayIcons = new Dictionary<string, Image>();
        Dictionary<string, Image> nightIcons = new Dictionary<string, Image>();

        public Dictionary<string, Color> yellowPaletteDay = new Dictionary<string, Color>();
        public Dictionary<string, Color> yellowPaletteNight = new Dictionary<string, Color>();
        //------------------------------------------------------------------------------------
        private List<Panel> messagesPanelList = new List<Panel>();// אני רוצה רשימה של כל הפאנלים של ההודעות זה יהיה יותר קל ככה
        //------------------------------------------------------------------------------------
        private List <Lecturer> allLecturers = new List<Lecturer>();  // רשימה שמכילה את כל המרצים שיש באותו מסלול התמחות שהמרצה הנוכחי מלמד
                                                                      //------------------------------------------------------------------------------------
                                                                      //---------------------------------------- משתנים עבור יצירת העמוד 
        private Panel topPanel;
        private Label helloLable;
        private Panel menuPanel;
        private Button prsonalInfro;
        private Button message;
        private Button search;
        private Button settings;
        private Button logOut;
        private PictureBox menuIcon;
        private Panel infroPanel;
        private Panel settingsPanel;
        private Panel coursesPanel;
        private PictureBox switchIcon;
        private Label title;
        private PictureBox profilePicture;
        private Panel searchingPanel;
        private TabControl messageTabControl;
        private Button courseTaught;
        private Panel courseTaughtPanel;
        // --------------------------------------- משתנים נוספים 
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
        //------------------------------------------------------------------------------------
        public LecturerWindow(Lecturer user)
        {

            InitializeComponent();
            createLecturerWindowPage();
            this.isDay = true; // כי הברירת מחדל זה שהמצב הוא מצב יום 
            this.isMenuOpen = false;
            this.FormClosing += CloseAll;
            this.user = user;
            // הוא נכנס לעצמו ומפעיל את הפונקציה שמחזירה את כל הסגל שיש לו במחלקה לא כולל עצמו
            this.allLecturers = this.user.allLecturersForLecturer();

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


            // פלטת צבעים צהובה מצב יום
            yellowPaletteDay["baseColor"] = Color.PaleGoldenrod;
            yellowPaletteDay["buttonColor"] = Color.Moccasin;
            yellowPaletteDay["otherColor"] = Color.LemonChiffon;
            yellowPaletteDay["titleColor"] = Color.SaddleBrown;
            yellowPaletteDay["textColor"] = Color.FromArgb(204, 153, 0);
            yellowPaletteDay["backGroundColor"] = Color.FromArgb(238, 240, 211);

            // פלטת צבעים צהובה מצב לילה
            yellowPaletteNight["baseColor"] = Color.FromArgb(139, 144, 81);
            yellowPaletteNight["buttonColor"] = Color.FromArgb(91, 95, 37);
            yellowPaletteNight["otherColor"] = Color.FromArgb(167, 172, 89);
            yellowPaletteNight["titleColor"] = Color.LemonChiffon;
            yellowPaletteNight["textColor"] = Color.FromArgb(238, 240, 211);
            yellowPaletteNight["backGroundColor"] = Color.FromArgb(105, 112, 2);


        }


        private void createLecturerWindowPage()
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
            courseTaught = new Button();
            infroPanel = new Panel();
            settingsPanel = new Panel();
            coursesPanel = new Panel();
            switchIcon = new PictureBox();
            title = new Label();
            profilePicture = new PictureBox();
            messageTabControl = new TabControl();
            courseTaughtPanel = new Panel();
            searchingPanel = new Panel();
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
            menuPanel.Location = new Point(625, 169);
            menuPanel.Name = "menuPanel";
            menuPanel.Size = new Size(210, 550);
            menuPanel.TabIndex = 3;
            // 
            // logOut
            // 
            logOut.Font = new Font("Arial", 10F, FontStyle.Bold);
            logOut.ForeColor = Color.SaddleBrown;
            logOut.ImageAlign = ContentAlignment.MiddleRight;
            logOut.Location = new Point(0, 472);
            logOut.Name = "logOut";
            logOut.Size = new Size(210, 53);
            logOut.TabIndex = 4;
            logOut.Text = " יציאה";
            logOut.TextImageRelation = TextImageRelation.TextBeforeImage;
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
            // LecturerWindow
            // 
            Controls.Add(profilePicture);
            Controls.Add(title);
            Controls.Add(menuPanel);
            Controls.Add(topPanel);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            menuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)menuIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)switchIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)profilePicture).EndInit();
        }
        private void LecturerWindow_Load(object sender, EventArgs e)
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


            // יצירת כל העמודים 

         
            createPresonalInfroPage();
            crearteSettingsPage();
            createSearchPage();
            createMessagePage();
            createCourseTaughtPage();

            removeAll();
            title.Text = "כל הקורסים";
            Controls.Add(courseTaughtPanel);

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
                              + "התמחות: " + this.user.SpecializationName + Environment.NewLine +
                               " דירוג סטודנטים" + Environment.NewLine;

                                for (int i = 0, j = this.user.Rating - 1; i < 5; i++)
                                    if (i <= j) userInfro.Text += "★";
                                    else userInfro.Text += "☆";

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
                if (student[0].AccountType == "Student")
                {
                    UserMessage.DeliverMessage(@"..\..\..\..\Files\Students\students.txt", message, otherSideInfro, student[0].UserName);

                }
                else
                    if (student[0].AccountType == "StudentTA")
                {
                    UserMessage.DeliverMessage(@"..\..\..\..\Files\Students\studentTA.txt", message, otherSideInfro, student[0].UserName);

                }
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
            searshBox.PlaceholderText = " הזן שם סטודנט, קורס או את שם המחלקה שלך";
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
                                userInfro.Text = " לא נמצא מרצה/ קורס/ סטודנט/ מחלקה שעונה על השם" + Environment.NewLine
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
           for(int i=0; i< this.allLecturers.Count; i++)
            {
                fullName = this.allLecturers[i].Name+" "+ this.allLecturers[i].FmName;
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
        private void ShowMyStudents(object? sender, EventArgs e)
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


        // -------------------------------------------------------------------
        private void ChangeMood(object sender, EventArgs e)
        {

            if (this.isDay)
            {
                //                                 day mood 

                // ---------------------------------------------------------------------------- backGroundColor

                this.BackColor = yellowPaletteDay["backGroundColor"];
               
                List<string> backGroundColor = new List<string> { };
                for (int i = 0; i < this.user.Courses.Count; i++)
                    for (int j = 0; j < this.user.Courses[i].Students.Count; j++)
                        backGroundColor.Add("courses" + i + "studentsTaught" + j);

                for (int i = 0; i < this.user.Courses.Count; i++)
                    backGroundColor.Add("coursesTaught" + i);
                UpdateControlsColors(backGroundColor, yellowPaletteDay["backGroundColor"], true, false);

                // ---------------------------------------------------------------------------- baseColor
                List<string> baseColorTools = new List<string>
                {"menuPanel","topPanel","userInfro","writingMessage","coursesNamesPanel", "studentsNamesPanel"};
                for (int i = 0; i < messagesPanelList.Count; i++)
                    baseColorTools.Add("messagePanel" + i);


                UpdateControlsColors(baseColorTools, yellowPaletteDay["titleColor"], false, true);
                UpdateControlsColors(baseColorTools, yellowPaletteDay["baseColor"], true, false);
                // ---------------------------------------------------------------------------- buttonColor
                List<string> buttonColorTools = new List<string>
                {"prsonalInfro","settings","message","search","logOut","courses"
                ,"dateSearch","importanceSearch","favoriteSearch","sendButton","courseTaught"};

                UpdateControlsColors(buttonColorTools, yellowPaletteDay["titleColor"], false, true);             
                UpdateControlsColors(buttonColorTools, yellowPaletteDay["buttonColor"], true, false);
                // ---------------------------------------------------------------------------- otherColor
                List<string> otherColorTools = new List<string>
                {"infroPanel","settingsPanel" ,"searchingPanel","showMessages","sendMessages","courseTaughtPanel" };

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
                {"prsonalInfro","settings","message","search","logOut","courses" 
                ,"dateSearch","importanceSearch","favoriteSearch","sendButton","courseTaught"};

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
                {"infroPanel","settingsPanel" ,"searchingPanel","showMessages","sendMessages","courseTaughtPanel"};

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
        }

        //--------------------------------------------------------- פעולות סגירה
        private void logOut_Click(object sender, EventArgs e)
        {
            this.user.LastLoginDate = DateTime.Now;// שומר תאריך יציאה
            HeadOfDepartment.WriteLecturerToFile(this.user);
            StartWindow newStart = new StartWindow();
            newStart.Show();
            this.Hide();
            this.Dispose();
        }
        private void CloseAll(object sender, FormClosingEventArgs e)
        {
            this.user.LastLoginDate = DateTime.Now;
            HeadOfDepartment.WriteLecturerToFile(this.user);
            Application.Exit();
        }




    }
}
