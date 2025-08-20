using System.Windows.Forms;

namespace My_university_WinFormsApp
{
    partial class HeadOfDepartmentWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // HeadOfDepartmentWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(626, 719);
            Name = "HeadOfDepartmentWindow";
            Text = "HeadOfDepartment";
            Load += HeadOfDepartmentWindow_Load;
            ResumeLayout(false);
            PerformLayout();

        }



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
        private Panel myClassPanel;
        private Panel newRequestsPanel;
        private Button myClassBtn;
        private Button newRequestsBtn;
        #endregion
    }
}