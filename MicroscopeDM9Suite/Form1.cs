using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace MicroscopeApp
{
    public partial class Form1 : Form
    {
        // --- AForge.NET Device Variables ---
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private bool isStreaming = false;

        // --- Feature Variables ---
        private BrightnessCorrection brightnessFilter = new BrightnessCorrection(0);
        private Grayscale grayscaleFilter = Grayscale.CommonAlgorithms.BT709;

        // --- Reference & Multi-Shot Tracking ---
        private Dictionary<string, int> referenceConfig = new Dictionary<string, int>();
        private string currentMatchedReference = string.Empty;
        private int remainingPictures = 0;
        private int totalPicturesRequired = 0;
        private string currentSerialNumber = string.Empty;
        private float zoomFactor = 0.5f;
        private const float ZoomStep = 0.2f;
        private const float MinZoom = 0.4f;
        private const float MaxZoom = 5.0f;

        public Form1()
        {
            InitializeComponent();
            InitializeCameraDevices();
            LoadReferenceFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtcodebare.Select();

            videoPictureBox.MouseWheel += videoPictureBox_MouseWheel;
            videoPictureBox.Focus();
        }

        // =================================================================
        // ## Load Reference.txt Configuration
        // =================================================================
        private void LoadReferenceFile()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reference.txt");
                if (File.Exists(filePath))
                {
                    referenceConfig.Clear();
                    foreach (string line in File.ReadAllLines(filePath))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(';');
                        if (parts.Length == 2)
                        {
                            string referenceName = parts[0].Trim();
                            if (int.TryParse(parts[1].Trim(), out int count))
                            {
                                referenceConfig[referenceName] = count;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Reference.txt not found in application directory!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Reference.txt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================================================================
        // ## 1. Camera Initialization
        // =================================================================
        private void InitializeCameraDevices()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                cboDevices.Items.Clear();
                if (videoDevices.Count == 0)
                {
                    cboDevices.Items.Add("No Video Devices Found");
                    cboDevices.Enabled = false;
                    return;
                }
                foreach (FilterInfo device in videoDevices)
                {
                    cboDevices.Items.Add(device.Name);
                }
                cboDevices.SelectedIndex = 0;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Could not initialize video devices: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isStreaming) return;
            if (videoDevices.Count == 0 || cboDevices.SelectedIndex == -1)
            {
                MessageBox.Show("No device selected or found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FilterInfo selectedDevice = videoDevices[cboDevices.SelectedIndex];
            videoSource = new VideoCaptureDevice(selectedDevice.MonikerString);
            LoadResolutions(videoSource);

            if (cmbResolution.SelectedIndex >= 0 && cmbResolution.SelectedIndex < videoSource.VideoCapabilities.Length)
            {
                videoSource.VideoResolution = videoSource.VideoCapabilities[cmbResolution.SelectedIndex];
            }

            videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
            videoSource.Start();
            isStreaming = true;
            btnStart.Enabled = false;
            BtnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                isStreaming = false;

                if (videoPictureBox.Image != null)
                {
                    videoPictureBox.Image.Dispose();
                    videoPictureBox.Image = null;
                }
                btnStart.Enabled = true;
                BtnStop.Enabled = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }

        // =================================================================
        // ## 2. Live Stream Handler
        // =================================================================
        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            try
            {
                Bitmap displayFrame;

                if (zoomFactor > 1.0f)
                {
                    int cropWidth = (int)(frame.Width / zoomFactor);
                    int cropHeight = (int)(frame.Height / zoomFactor);

                    int x = (frame.Width - cropWidth) / 2;
                    int y = (frame.Height - cropHeight) / 2;

                    Rectangle cropArea = new Rectangle(
                        x,
                        y,
                        cropWidth,
                        cropHeight);

                    Bitmap croppedFrame = frame.Clone(
                        cropArea,
                        frame.PixelFormat);

                    displayFrame = new Bitmap(
                        videoPictureBox.Width,
                        videoPictureBox.Height);

                    using (Graphics g = Graphics.FromImage(displayFrame))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(
                            croppedFrame,
                            new Rectangle(
                                0,
                                0,
                                displayFrame.Width,
                                displayFrame.Height));
                    }

                    croppedFrame.Dispose();
                }
                else
                {
                    displayFrame = new Bitmap(
                        videoPictureBox.Width,
                        videoPictureBox.Height);

                    using (Graphics g = Graphics.FromImage(displayFrame))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(
                            frame,
                            new Rectangle(
                                0,
                                0,
                                displayFrame.Width,
                                displayFrame.Height));
                    }
                }

                if (videoPictureBox.InvokeRequired)
                {
                    videoPictureBox.BeginInvoke(new Action(() =>
                    {
                        if (videoPictureBox.Image != null)
                        {
                            videoPictureBox.Image.Dispose();
                        }

                        videoPictureBox.Image = displayFrame;
                    }));
                }
                else
                {
                    if (videoPictureBox.Image != null)
                    {
                        videoPictureBox.Image.Dispose();
                    }

                    videoPictureBox.Image = displayFrame;
                }
            }
            catch
            {
                // Ignore frame errors
            }
            finally
            {
                frame.Dispose();
            }
        }

        // =================================================================
        // ## 3. Barcode Processing & Validation (Keydown / Enter)
        // =================================================================
        private void txtcodebare_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ProcessScannedBarcode();
            }
        }

        private void ProcessScannedBarcode()
        {
            // Clean up any trailing newline or whitespace characters from the scanner
            string scannedInput = txtcodebare.Text.Replace("\r", "").Replace("\n", "").Trim();

            if (string.IsNullOrEmpty(scannedInput))
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Scan a valid barcode.";
                return;
            }

            // Reload config in case file changed
            LoadReferenceFile();

            // Find matching reference inside the scanned string
            currentMatchedReference = string.Empty;
            foreach (var kvp in referenceConfig)
            {
                if (scannedInput.Contains(kvp.Key))
                {
                    currentMatchedReference = kvp.Key;
                    totalPicturesRequired = kvp.Value;
                    break;
                }
            }

            if (string.IsNullOrEmpty(currentMatchedReference))
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Reference not found in Reference.txt!";
                txtcodebare.Clear();
                txtcodebare.Select();
                return;
            }

            // Explicitly set session variables
            currentSerialNumber = scannedInput;
            remainingPictures = totalPicturesRequired;

            // Lock input box to make process solid
            txtcodebare.ReadOnly = true;

            // Update status to show countdown is ready
            statusLabel.ForeColor = Color.Blue;
            statusLabel.Text = $"Ref: {currentMatchedReference} | Photos Left: {remainingPictures}/{totalPicturesRequired}";
        }


        // =================================================================
        // ## 4. Capture Action (OK / NOK handling)    string baseFolder = @"C:\temp";
        // =================================================================
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveCapturedImage("OK");
        }

        private void btnNOK_Click(object sender, EventArgs e)
        {
            SaveCapturedImage("NOK");
        }

        private void SaveCapturedImage(string statusResult)
        {
            // Safety check if scan wasn't properly locked in
            if (string.IsNullOrEmpty(currentSerialNumber) || remainingPictures <= 0)
            {
                MessageBox.Show("Please scan a valid barcode first and ensure countdown is active.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcodebare.ReadOnly = false;
                txtcodebare.Clear();
                txtcodebare.Select();
                return;
            }

            if (videoPictureBox.Image == null)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Camera feed is not active.";
                return;
            }

            try
            {
                // Choose your base folder (Change to network path if needed: @"\\mokeaps001\MEDIA\KenitraQuality")
                string baseFolder = @"C:\temp";
                string yearFolder = DateTime.Now.Year.ToString();
                string monthFolder = DateTime.Now.Month.ToString("D2");
                string dayFolder = DateTime.Now.Day.ToString("D2");

                // 🛡️ SANITIZE SERIAL NUMBER: Remove/replace illegal Windows folder characters (like /, \, :, etc.)
                string safeSerialNumber = currentSerialNumber;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeSerialNumber = safeSerialNumber.Replace(c, '_');
                }
                // Also replace path separators just in case
                safeSerialNumber = safeSerialNumber.Replace('/', '_').Replace('\\', '_');

                // Folder structure: Base\Year\Month\Day\SafeSerialNumber
                string fullFolder = Path.Combine(baseFolder, yearFolder, monthFolder, dayFolder, safeSerialNumber);

                // Ensure directory exists
                Directory.CreateDirectory(fullFolder);

                // Filename includes timestamp, sequence index, and OK/NOK status
                int shotIndex = (totalPicturesRequired - remainingPictures) + 1;
                string fileName = $"{DateTime.Now:HHmmssfff}_{statusResult}_Shot{shotIndex}.jpg";
                string targetPath = Path.Combine(fullFolder, fileName);

                // Capture current zoomed/displayed frame and save
                using (Bitmap capturedImage = new Bitmap(videoPictureBox.Image))
                {
                    // Draw overlay text (Serial + Status + Shot count)
                    using (Graphics g = Graphics.FromImage(capturedImage))
                    {
                        string overlayText = $"{currentSerialNumber} - {statusResult} ({shotIndex}/{totalPicturesRequired})";
                        Font font = new Font("Arial", 14, FontStyle.Bold);
                        Brush brush = statusResult == "OK" ? Brushes.LimeGreen : Brushes.Red;

                        SizeF textSize = g.MeasureString(overlayText, font);
                        PointF position = new PointF(
                            capturedImage.Width - textSize.Width - 10,
                            capturedImage.Height - textSize.Height - 10);

                        // Draw background box for visibility
                        g.FillRectangle(Brushes.Black, position.X - 5, position.Y - 5, textSize.Width + 10, textSize.Height + 10);
                        g.DrawString(overlayText, font, brush, position);
                    }

                    capturedImage.Save(targetPath, ImageFormat.Jpeg);
                }

                // Decrement countdown tracker
                remainingPictures--;

                if (remainingPictures > 0)
                {
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = $"Saved [{statusResult}]. Remaining Photos: {remainingPictures}/{totalPicturesRequired}";
                }
                else
                {
                    // Completed full countdown sequence
                    statusLabel.ForeColor = Color.DarkGreen;
                    MessageBox.Show($"All {totalPicturesRequired} pictures captured successfully for serial: {currentSerialNumber}", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetSession();
                }
            }
            catch (Exception ex)
            {
                // Detailed error display to help diagnose any remaining path/permission issues
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = $"Save Error: {ex.Message}";
                MessageBox.Show($"Detailed Save Error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // --- Reset Session ---
        private void button3_Click(object sender, EventArgs e)
        {
            ResetSession();
        }

        private void ResetSession()
        {
            currentSerialNumber = string.Empty;
            currentMatchedReference = string.Empty;
            remainingPictures = 0;
            totalPicturesRequired = 0;

            txtcodebare.ReadOnly = false;
            txtcodebare.Clear();
            txtcodebare.Select();

            statusLabel.ForeColor = Color.Black;
            statusLabel.Text = "Ready. Scan a new harness barcode.";
        }

        // --- Helper functions ---
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadResolutions(VideoCaptureDevice device)
        {
            cmbResolution.Items.Clear();
            foreach (var cap in device.VideoCapabilities)
            {
                cmbResolution.Items.Add($"{cap.FrameSize.Width}x{cap.FrameSize.Height} @ {cap.AverageFrameRate}fps");
            }
            if (cmbResolution.Items.Count > 0)
                cmbResolution.SelectedIndex = 0;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.DisplayPropertyPage(this.Handle);
            }
            else
            {
                MessageBox.Show("Please start the camera feed first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Unused event stubs kept for designer compatibility
 
        private void trbBrightness_Scroll(object sender, EventArgs e) { }
        private void chkGrayscale_CheckedChanged(object sender, EventArgs e) { }

        private void txtcodebare_TextChanged(object sender, EventArgs e)
        {

        }
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (zoomFactor < MaxZoom)
            {
                zoomFactor += ZoomStep;
            }

            statusLabel.ForeColor = Color.Blue;
            statusLabel.Text = $"Zoom: {zoomFactor:0.0}x";
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            zoomFactor -= ZoomStep;
            zoomFactor -= ZoomStep;

            if (zoomFactor < MinZoom)
            {
                zoomFactor = MinZoom;
            }

            statusLabel.Text = $"Zoom: {zoomFactor:0.0}x";
        }
        private void videoPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (zoomFactor < MaxZoom)
                {
                    zoomFactor += ZoomStep;
                }
            }
            else
            {
                zoomFactor -= ZoomStep;

                if (zoomFactor < 1.0f)
                {
                    zoomFactor = 1.0f;
                }
            }

            statusLabel.Text = $"Zoom: {zoomFactor:0.0}x";
        }
    }
}
