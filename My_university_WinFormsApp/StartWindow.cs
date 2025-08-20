using System.Drawing.Printing;
using System.Windows.Forms;

namespace My_university_WinFormsApp
{
    public partial class StartWindow : Form
    {
        //---------------------------------------- ������ ���� ����� ����� 
        private Button student;
        private Button employee;
        private Button HeadOfDepartment;
        private Panel upperPanel;
        private PictureBox logo;
        private LinkLabel newAccount;
        private Button studentTA;
        // --------------------------------------- 
        public StartWindow()
        {
            InitializeComponent();
            createStartWindowPage();
        }

        private void createStartWindowPage()
        {
            student = new Button();
            employee = new Button();
            HeadOfDepartment = new Button();
            newAccount = new LinkLabel();
            upperPanel = new Panel();
            logo = new PictureBox();
            studentTA = new Button();
            upperPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
            SuspendLayout();
            // 
            // student
            // 
            student.BackColor = Color.DarkGoldenrod;
            student.Font = new Font("Arial", 12F, FontStyle.Bold);
            student.ForeColor = Color.White;
            student.Location = new Point(239, 289);
            student.Name = "student";
            student.Size = new Size(242, 48);
            student.TabIndex = 0;
            student.Text = "Student";
            student.UseVisualStyleBackColor = false;
            student.Click += Student_Click;
            student.MouseEnter += HoverButton;
            student.MouseLeave += OffButton;
            // 
            // employee
            // 
            employee.BackColor = Color.DarkGoldenrod;
            employee.Font = new Font("Arial", 12F, FontStyle.Bold);
            employee.ForeColor = Color.White;
            employee.Location = new Point(239, 143);
            employee.Name = "employee";
            employee.Size = new Size(242, 48);
            employee.TabIndex = 1;
            employee.Text = "Employee";
            employee.UseVisualStyleBackColor = false;
            employee.Click += Lecturer_Click;
            employee.MouseEnter += HoverButton;
            employee.MouseLeave += OffButton;
            // 
            // HeadOfDepartment
            // 
            HeadOfDepartment.BackColor = Color.DarkGoldenrod;
            HeadOfDepartment.Font = new Font("Arial", 12F, FontStyle.Bold);
            HeadOfDepartment.ForeColor = Color.White;
            HeadOfDepartment.Location = new Point(239, 364);
            HeadOfDepartment.Name = "HeadOfDepartment";
            HeadOfDepartment.Size = new Size(242, 48);
            HeadOfDepartment.TabIndex = 2;
            HeadOfDepartment.Text = "HeadOfDepartment";
            HeadOfDepartment.UseVisualStyleBackColor = false;
            HeadOfDepartment.Click += HeadOfDepartment_Click;
            HeadOfDepartment.MouseEnter += HoverButton;
            HeadOfDepartment.MouseLeave += OffButton;
            // 
            // newAccount
            // 
            newAccount.AutoSize = true;
            newAccount.Location = new Point(287, 430);
            newAccount.Name = "newAccount";
            newAccount.Size = new Size(151, 20);
            newAccount.TabIndex = 4;
            newAccount.TabStop = true;
            newAccount.Text = "Create a new account";
            newAccount.LinkClicked += NewAccount_LinkClicked;
            // 
            // upperPanel
            // 
            upperPanel.BackColor = Color.PaleGoldenrod;
            upperPanel.Controls.Add(logo);
            upperPanel.Location = new Point(0, 0);
            upperPanel.Name = "upperPanel";
            upperPanel.Size = new Size(720, 108);
            upperPanel.TabIndex = 3;
            // 
            // logo
            // 
            logo.Location = new Point(227, -39);
            logo.Name = "logo";
            logo.Size = new Size(272, 188);
            logo.TabIndex = 4;
            logo.TabStop = false;
            // 
            // studentTA
            // 
            studentTA.BackColor = Color.DarkGoldenrod;
            studentTA.Font = new Font("Arial", 12F, FontStyle.Bold);
            studentTA.ForeColor = Color.White;
            studentTA.Location = new Point(239, 215);
            studentTA.Name = "studentTA";
            studentTA.Size = new Size(242, 48);
            studentTA.TabIndex = 5;
            studentTA.Text = "studentTA";
            studentTA.UseVisualStyleBackColor = false;
            studentTA.MouseEnter += HoverButton;
            studentTA.MouseLeave += OffButton;
            studentTA.Click += StudentTA_Click;
            // 
            // StartWindow
            // 
            Controls.Add(studentTA);
            Controls.Add(upperPanel);
            Controls.Add(student);
            Controls.Add(employee);
            Controls.Add(HeadOfDepartment);
            Controls.Add(newAccount);
            upperPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)logo).EndInit();

        }

        private void StartWindow_Load(object sender, EventArgs e)
        {

            if (File.Exists(@"..\..\..\..\imgs\logo.png"))
            {
                logo.Image = Image.FromFile(@"..\..\..\..\imgs\logo.png");
                logo.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("�� ����� ������ �����: " + @"..\..\..\..\imgs\logo.png");
            }

            MessageBox.Show("������� ������"+Environment.NewLine+
                             "----- ��������  -----" + Environment.NewLine +
                             "�� �����: lior  �����: 123456789" + Environment.NewLine +
                             "�� �����: dan  �����: 1000002" + Environment.NewLine +
                              "----- �������� ������� -----" + Environment.NewLine +
                             "�� �����: gal6  �����: 301006789" + Environment.NewLine +
                             "�� �����: or7  �����: 301007890" + Environment.NewLine +
                              "----- ����� -----" + Environment.NewLine +
                             "�� �����: erezB  �����: 120034567" + Environment.NewLine +
                             "�� �����: yossi  �����: 120056789" + Environment.NewLine +
                             "----- ��� ����� -----" + Environment.NewLine +
                              "�� �����: shulamit  �����: 222222222"
                             );

            
        }

        public static void HoverButton(object sender, EventArgs e)
        {
            // ������ ���� ��� �������� ���� �� �����
            if (sender != null && sender is Button)
            {
                Button button = (Button)sender;
                button.BackColor = Color.SaddleBrown;
            }
            /*�� ������� ������� ������� �� ������ ��� ������ �� �������� ��� 
             * ����� ��� �� ����� ����� ����� ����� ���� ��� ����� ����� ���� �������� 
             */

        }

        public static void OffButton(object sender, EventArgs e)
        {
            // ������ ���� ��� �������� ���� �� ����� 
            if (sender != null && sender is Button)
            {
                Button button = (Button)sender;
                button.BackColor = Color.DarkGoldenrod;
            }
        }

        // ----------------- ������ ����� ������ ---------------------
        private void Lecturer_Click(object sender, EventArgs e)
        {
            Login loginASLecturer = new Login(this, "Lecturer");
            loginASLecturer.Show();
            this.Hide();
        }
        private void Student_Click(object sender, EventArgs e)
        {
            Login loginASstudent = new Login(this, "Student");
            loginASstudent.Show();
            this.Hide();
        }
        private void StudentTA_Click(object sender, EventArgs e)
        {
            Login loginASstudentTA = new Login(this, "StudentTA");
            loginASstudentTA.Show();
            this.Hide();
        }
        private void HeadOfDepartment_Click(object sender, EventArgs e)
        {
            Login loginASheadOfDepartment = new Login(this, "HeadOfDepartment");
            loginASheadOfDepartment.Show();
            this.Hide();

        }
        private void NewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount(this);
            createAccount.Show();
            this.Hide();
        }

      



    }
}
