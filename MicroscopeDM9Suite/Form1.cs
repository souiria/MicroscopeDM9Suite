
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using System;
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

        // **NOTE on Zoom/Pan/Measurement:**
        // These features require more complex rendering/drawing logic (e.g., custom PictureBox or overlay).
        // For simplicity and focus, the video streaming framework is provided, and the 
        // placeholders for zoom/pan are omitted to keep this initial code functional.

        public Form1()
        {
            InitializeComponent();
            InitializeCameraDevices();
        }

        // =================================================================
        // ## 1. Camera Initialization
        // =================================================================

        private void InitializeCameraDevices()
        {
            try
            {
                // Enumerate all video input devices (USB cameras)
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                // Populate the ComboBox
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

                cboDevices.SelectedIndex = 0; // Select the first device by default
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Could not initialize video devices: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click1(object sender, EventArgs e)
        {
            if (isStreaming) return; // Prevent double start

            if (videoDevices.Count == 0 || cboDevices.SelectedIndex == -1)
            {
                MessageBox.Show("No device selected or found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the moniker string for the selected device
            FilterInfo selectedDevice = videoDevices[cboDevices.SelectedIndex];
            videoSource = new VideoCaptureDevice(selectedDevice.MonikerString);
            LoadResolutions(videoSource);
            // ⬇️ INSERT THE RESOLUTION SELECTION CODE HERE
            if (videoSource.VideoCapabilities.Length > 0)
            {
                var bestRes = videoSource.VideoCapabilities
                                         .OrderByDescending(r => r.FrameSize.Width * r.FrameSize.Height)
                                         .First();
                videoSource.VideoResolution = bestRes;
            }

            // Set up the NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);

            // Start the video source
            videoSource.Start();
            isStreaming = true;
            btnStart.Enabled = false;
            BtnStop.Enabled = true;
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
            // Apply resolution from ComboBox
            if (cmbResolution.SelectedIndex >= 0)
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

                // Clear the display
                if (videoPictureBox.Image != null)
                {
                    videoPictureBox.Image.Dispose();
                    videoPictureBox.Image = null;
                }

                btnStart.Enabled = true;
                BtnStop.Enabled = false;
            }
        }

        // Crucial: Stop camera when the form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }

        // =================================================================
        // ## 2. Live Stream Handler and Image Enhancement
        // =================================================================

        private void Video_NewFrame0(object sender, NewFrameEventArgs eventArgs)
        {
            // Use Invoke to safely update the UI from the camera's thread
            if (this.InvokeRequired)
            {
                this.Invoke(new NewFrameEventHandler(Video_NewFrame), sender, eventArgs);
            }
            else
            {
                try
                {
                    // Clone the frame to avoid threading issues and allow manipulation
                    Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

                    // --- Apply Image Enhancements (Filters) ---
                    ApplyFilters(ref frame);

                    // Dispose the previous image to free memory
                    if (videoPictureBox.Image != null)
                    {
                        videoPictureBox.Image.Dispose();
                    }

                    // Display the processed frame
                    videoPictureBox.Image = frame;
                }
                catch (Exception ex)
                {
                    // Simple error handling for streaming issues
                    System.Diagnostics.Debug.WriteLine("Stream error: " + ex.Message);
                }
            }
        }
        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            // Resize to fit PictureBox size
            //Bitmap resized = new Bitmap(videoPictureBox.Width, videoPictureBox.Height);
            //using (Graphics g = Graphics.FromImage(resized))
            //{
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(frame, new Rectangle(0, 0, resized.Width, resized.Height));
            //}

            //if (videoPictureBox.Image != null)
            //    videoPictureBox.Image.Dispose();

            //videoPictureBox.Image = resized;
            Bitmap resized = new Bitmap(videoPictureBox.Width, videoPictureBox.Height);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(frame, new Rectangle(0, 0, resized.Width, resized.Height));
            }
            videoPictureBox.Image = resized;
            frame.Dispose();
        }

        private void ApplyFilters(ref Bitmap frame)
        {
            // 1. Brightness Correction
            if (trbBrightness.Value != 0)
            {
                // BrightnessCorrection takes an integer adjustment in the constructor
                // Range is typically -255 to +255
                int adjustment = trbBrightness.Value;
                BrightnessCorrection brightnessFilter = new BrightnessCorrection(adjustment);

                brightnessFilter.ApplyInPlace(frame);
            }
            // 2. Grayscale Toggle
            if (chkGrayscale.Checked)
            {
                frame = grayscaleFilter.Apply(frame);
            }

            // NOTE: Annotation drawing would happen here if it's on the live stream
        }

        // =================================================================
        // ## 3. Feature Implementations
        // =================================================================

        // --- Capture and Save ---

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (videoPictureBox.Image == null) return;

            string fileName = txtcodebare.Text.Trim();

            if (string.IsNullOrEmpty(fileName))
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Scan the Harness";
                return;
            }

            try
            {
                // Base folder
                string baseFolder = @"\\mokeaps001\MEDIA\KenitraQuality\";

                // Build subfolders: Year\Month\Day
                string yearFolder = DateTime.Now.Year.ToString();
                string monthFolder = DateTime.Now.Month.ToString("D2"); // 01..12
                string dayFolder = DateTime.Now.Day.ToString("D2");     // 01..31

                string fullFolder = Path.Combine(baseFolder, yearFolder, monthFolder, dayFolder);

                // Ensure folder exists
                Directory.CreateDirectory(fullFolder);

                // Combine folder + filename
                // Ensure filename has an extension
                 fileName = txtcodebare.Text.Trim();
                if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                {
                    // default to .jpg if no extension provided
                    fileName += ".jpg";
                }

                // Combine folder + filename
                string targetPath = Path.Combine(fullFolder, fileName);


                using (Bitmap capturedImage = new Bitmap(videoPictureBox.Image))
                {
                    ImageFormat format = GetImageFormat(Path.GetExtension(targetPath));
                    capturedImage.Save(targetPath, format);
                }

                // ✅ Show success in green label
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = $"Image saved to: {targetPath}";

                // ✅ Clear the textbox
                txtcodebare.Clear();
                txtcodebare.Select();
            }
            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = $"Error saving image: {ex.Message}";
            }
        }

        // --- Device Settings Control ---
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                // Displays the standard Windows DirectShow property dialog for the USB camera
                videoSource.DisplayPropertyPage(this.Handle);
            }
            else
            {
                MessageBox.Show("Please start the camera feed first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- Filter Control Event Handlers ---
        private void trbBrightness_Scroll(object sender, EventArgs e)
        {
            // The next frame processed by Video_NewFrame will pick up the new value
            // You might add a label to show the current value if desired.
        }

        private void chkGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            // The next frame processed by Video_NewFrame will pick up the new state
        }

        // --- Helper function ---
        private ImageFormat GetImageFormat(string extension)
        {
            return extension.ToLower() switch
            {
                ".png" => ImageFormat.Png,
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                _ => ImageFormat.Jpeg,
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadResolutions(VideoCaptureDevice device)
        {
            cmbResolution.Items.Clear();

            foreach (var cap in device.VideoCapabilities)
            {
                cmbResolution.Items.Add($"{cap.FrameSize.Width}x{cap.FrameSize.Height} @ {cap.AverageFrameRate}fps");
            }

            if (cmbResolution.Items.Count > 0)
                cmbResolution.SelectedIndex = 0; // default to first resolution
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (videoPictureBox.Image == null) return;

            string codeText = txtcodebare.Text.Trim();

            if (string.IsNullOrEmpty(codeText))
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Scan the Harness";
                return;
            }

            try
            {
                // Base folder
                string baseFolder = @"\\mokeaps001\MEDIA\KenitraQuality\";

                // Build subfolders: Year\Month\Day\CodeText
                string yearFolder = DateTime.Now.Year.ToString();
                string monthFolder = DateTime.Now.Month.ToString("D2");
                string dayFolder = DateTime.Now.Day.ToString("D2");

                // 🔹 Add new folder named after txtcodebare.Text
                string fullFolder = Path.Combine(baseFolder, yearFolder, monthFolder, dayFolder, codeText);

                // Ensure folder exists
                Directory.CreateDirectory(fullFolder);

                // Build filename (timestamp to avoid overwriting)
                string fileName = $"{DateTime.Now:HHmmssfff}.jpg"; // e.g. 142530123.jpg

                string targetPath = Path.Combine(fullFolder, fileName);

                using (Bitmap capturedImage = new Bitmap(videoPictureBox.Image))
                {
                    // 🔹 Draw traceability text
                    using (Graphics g = Graphics.FromImage(capturedImage))
                    {
                        string overlayText = codeText;
                        Font font = new Font("Arial", 14, FontStyle.Bold);
                        Brush brush = new SolidBrush(Color.Red);

                        // Position bottom-right with padding
                        SizeF textSize = g.MeasureString(overlayText, font);
                        PointF position = new PointF(
                            capturedImage.Width - textSize.Width - 10,
                            capturedImage.Height - textSize.Height - 10);

                        g.DrawString(overlayText, font, brush, position);
                    }

                    // Save with correct format
                    ImageFormat format = GetImageFormat(Path.GetExtension(fileName));
                    capturedImage.Save(targetPath, format);
                }

                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = $"Image saved to: {targetPath}";

                //txtcodebare.Clear();
                //txtcodebare.Select();
            }
            catch (Exception ex)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = $"Error saving image: {ex.Message}";

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtcodebare.Clear();
            txtcodebare.Select();
        }
    }
}