using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MicroscopeApp
{
    public partial class ReportForm : Form
    {
        private DataGridView dgvReports;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnClose;
        private PictureBox picPreview;
        private Label lblDetails;

        public ReportForm()
        {
            InitializeComponentCustom();
            LoadData();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Sews Morocco - Inspection Reports & Search";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            // Search Box
            txtSearch = new TextBox { Location = new Point(20, 20), Size = new Size(300, 30), Font = new Font("Segoe UI", 12) };
            txtSearch.TextChanged += (s, e) => LoadData(txtSearch.Text.Trim());

            // Search Button
            btnSearch = new Button { Text = "Search", Location = new Point(330, 20), Size = new Size(100, 32), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnSearch.Click += (s, e) => LoadData(txtSearch.Text.Trim());

            // DataGridView
            dgvReports = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(680, 500),
                BackgroundColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.Black,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvReports.SelectionChanged += DgvReports_SelectionChanged;

            // Preview PictureBox
            picPreview = new PictureBox
            {
                Location = new Point(715, 70),
                Size = new Size(345, 350),
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Details Label
            lblDetails = new Label
            {
                Location = new Point(715, 430),
                Size = new Size(345, 100),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Select a row to view details."
            };

            // Close Button
            btnClose = new Button { Text = "Close", Location = new Point(960, 540), Size = new Size(100, 40), BackColor = Color.FromArgb(192, 57, 43), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(dgvReports);
            this.Controls.Add(picPreview);
            this.Controls.Add(lblDetails);
            this.Controls.Add(btnClose);
        }

        private void LoadData(string filter = "")
        {
            DataTable dt = DatabaseHelper.SearchInspections(filter);
            dgvReports.DataSource = dt;
        }

        private void DgvReports_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReports.SelectedRows.Count > 0)
            {
                var row = dgvReports.SelectedRows[0];
                string imagePath = row.Cells["ImagePath"].Value.ToString();
                string serial = row.Cells["SerialNumber"].Value.ToString();
                string reference = row.Cells["ReferenceName"].Value.ToString();
                string status = row.Cells["Status"].Value.ToString();
                string timestamp = row.Cells["Timestamp"].Value.ToString();
                string shift = row.Cells["ShiftName"].Value.ToString();

                lblDetails.Text = $"Serial: {serial}\nRef: {reference}\nStatus: {status}\nShift: {shift}\nTime: {timestamp}";

                if (File.Exists(imagePath))
                {
                    if (picPreview.Image != null) picPreview.Image.Dispose();
                    picPreview.Image = new Bitmap(imagePath);
                }
                else
                {
                    picPreview.Image = null;
                }
            }
        }
    }
}
